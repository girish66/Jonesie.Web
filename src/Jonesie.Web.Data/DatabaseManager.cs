using Jonesie.Web.Contracts.Data;
using Jonesie.Web.Entities.Data;
using Jonesie.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using Jonesie.Web.Contracts.Core;
using System.ComponentModel.Composition;
using System.Transactions;
using System.IO;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace Jonesie.Web.Data
{
  /// <summary>
  /// Manage the database
  /// </summary>
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class DatabaseManager : BaseRepository, IDatabaseManager
  {

    bool _updatingSchema = false;
    static bool _databaseChecked;

    /// <summary>
    /// See if the databases exists.
    /// </summary>
    /// <returns></returns>
    public bool DatabaseExists()
    {
      return ConnectionFactory.DatabaseExists(Settings.DBConnectionString);
    }

    /// <summary>
    /// Gets a schema version record
    /// </summary>
    /// <param name="schemaSetame">The schema set name.</param>
    /// <returns></returns>
    public SchemaVersion GetSchemaVersion(string schemaSetName)
    {
      return PerformNonCachedQuery<SchemaVersion>("Jonesie.Web.Data.DatabaseMaager.GetSchemaVersion({0}".FormatWith(schemaSetName),
        (con) =>
        {
          return con.Query<SchemaVersion>(Properties.Resources.SchemaVersion_Get, new { SchemaSetName = schemaSetName }).FirstOrDefault();
        });
    }

    /// <summary>
    /// Updates the schema.
    /// </summary>
    /// <param name="schemaSetame">The schema setame.</param>
    /// <param name="version">The version.</param>
    /// <param name="updater">The updater.</param>
    public void UpdateSchema(string schemaSetame, int version, Action updater)
    {
      if (!_updatingSchema)
      {
        _updatingSchema = true;
        try
        {

          SchemaVersion sv = null;

          if (DatabaseExists())
          {
            sv = GetSchemaVersion(schemaSetame);
          }

          if (sv == null || sv.Version < version)
          {
            //using (var scope = new TransactionScope())  // cant use a transaction for things like CREATE DATABASE
            {
              updater();
              UpdateSchemaVersion(schemaSetame, version);
              //scope.Complete();
            }
          }
        }
        finally
        {
          _updatingSchema = false;
        }
      }

    }

    /// <summary>
    /// Updates the base schema.
    /// </summary>
    public void UpdateBaseSchema()
    {
      // apply any version upgrades - but only once
      if (!_databaseChecked)
      {
        UpdateSchema("_base", 1, updateBaseSchema);
        _databaseChecked = true;
      }
    }

    /// <summary>
    /// Updates the schema version.
    /// </summary>
    /// <param name="schemaSetName">Name of the schema set.</param>
    /// <param name="version">The version.</param>
    public void UpdateSchemaVersion(string schemaSetName, int version)
    {
      ExecuteQuery("Jonesie.Web.Data.DatabaseManager.UpdateSchemaVersion",
        (con) =>
        {
          con.Execute(Properties.Resources.SchemaVersion_Update, new { SchemaSetName = schemaSetName, Version = version });
        });
    }

    /// <summary>
    /// Executes the DDL.
    /// </summary>
    /// <param name="ddl">The DDL.</param>
    public void ExecuteDDL(string ddl, string databaseName = null)
    {
      ExecuteDDL(ddl, databaseName, new Dictionary<string, string>());
    }

    /// <summary>
    /// Executes the DDL with some overridden variables
    /// </summary>
    /// <param name="ddl">The DDL.</param>
    /// <param name="databaseName">Name of the database.</param>
    /// <param name="variables">The variables.</param>
    public void ExecuteDDL(string ddl, string databaseName, Dictionary<string, string> variables)
    {
      if (variables == null)
      {
        variables = new Dictionary<string, string>();
      }

      if (databaseName == null)
      {
        SqlConnectionStringBuilder cb = new SqlConnectionStringBuilder(Settings.DBConnectionString);
        databaseName = cb.InitialCatalog;
      }


      Logger.LogDebug("START: DatabaseManager.ExecuteDDL");
      ExecuteQuery("Jonesie.Web.Data.DatabaseManager.ExecuteDDL",
        (connection) =>
        {
          // start with the master database and keep using it as it may switch to other databases (i.e., USE blah)
          var rxVars = new Regex(@"^\s*:setvar (.*) ""(.*)""\s*[\n]", RegexOptions.IgnoreCase | RegexOptions.Multiline);
          var rxComments = new Regex(@"/\*(?>(?:(?!\*/|/\*).)*)(?>(?:/\*(?>(?:(?!\*/|/\*).)*)\*/(?>(?:(?!\*/|/\*).)*))*).*?\*/|--.*?\r?[\n]", RegexOptions.Singleline);
          var rxPrint = new Regex(@"^\s*PRINT .*?\r?[\n]", RegexOptions.IgnoreCase | RegexOptions.Multiline);
          var rxColonThings = new Regex(@"^\s*:.*\r?[\n]", RegexOptions.IgnoreCase | RegexOptions.Multiline);
          var rxUse = new Regex(@"USE\s*\[(\S*)\]\s*[;]", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);

          // split the ddl - look for "GO" in a line by itself
          var parts = Regex.Split(ddl, "^\\s*GO\\s*\\n", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);

          for (var i = 0; i < parts.Length; i++)
          {
            string ddlpart = parts[i];

            if (!string.IsNullOrWhiteSpace(ddlpart))
            {
              // remove comments
              ddlpart = rxComments.Replace(ddlpart, string.Empty);

              // remove print statements
              // CANT ignore these as there are instances of IF statemets with just a PRINT in side the BEING/END
              // ddlpart = rxPrint.Replace(ddlpart, string.Empty);

              // look for variable declarations like :setvar name value
              var matches = rxVars.Matches(ddlpart);
              foreach (Match matchResult in matches)
              {
                if (matchResult.Groups.Count == 3)
                {
                  var name = matchResult.Groups[1].Captures[0].Value;
                  var value = matchResult.Groups[2].Captures[0].Value;

                  if (!variables.ContainsKey(name))
                  {
                    variables.Add(name, value);
                  }

                  ddlpart = ddlpart.Replace(matchResult.Groups[0].Captures[0].Value, string.Empty);
                }
              }

              // remove anything else that starts with a colon
              ddlpart = rxColonThings.Replace(ddlpart, string.Empty);

              if (!string.IsNullOrWhiteSpace(ddlpart))
              {
                // insert variables into ddl
                foreach (var key in variables.Keys)
                {
                  var rx = @"\$\({0}\)".FormatWith(key);
                  ddlpart = Regex.Replace(ddlpart, rx, variables[key], RegexOptions.IgnoreCase);
                }

                // see if the only thing in this ddl is a USE statement
                ddlpart = ddlpart.Trim();

                var matchResult = rxUse.Match(ddlpart);
                if (matchResult.Success)
                {
                  Logger.LogDebug("Change Database Context to " + databaseName);
                  databaseName = matchResult.Groups[1].Captures[0].Value;
                  connection.ChangeDatabase(databaseName);
                }
                else
                {
                  Logger.LogDebug(ddlpart);
                  connection.Execute(ddlpart);
                }
              }
            }
          }
        },
        () => { return GetExclusiveConnection(databaseName); },
        noTransaction: true);

      Logger.LogDebug("END: DatabaseManager.ExecuteDDL");
    }

    /// <summary>
    /// Updates the schema.
    /// </summary>
    private void updateBaseSchema()
    {

      // some variables for the script
      SqlConnectionStringBuilder cb = new SqlConnectionStringBuilder(Settings.DBConnectionString);

      // create new variables to override the ones in the script
      var vars = new Dictionary<string, string>();
      vars.Add("DatabaseName", cb.InitialCatalog);

      // could use something else but this should work, mostly...
      vars.Add("DefaultFilePrefix", cb.InitialCatalog);

      //TODO: Get these from settings
      vars.Add("DefaultDataPath", Settings.DBPath);
      vars.Add("DefaultLogPath", Settings.DBPath);

      // must execute the main base schema v 1 from MASTER as it could be creating the database
      ExecuteDDL(Properties.Resources.Schema_Base_1, databaseName: "master", variables: vars);
    }
  }
}
