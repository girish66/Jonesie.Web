using Jonesie.Web.Entities.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Jonesie.Web.Contracts.Data
{
  /// <summary>
  /// A contract for a service to maintain and update the database schema
  /// </summary>
  [InheritedExport(typeof(IDatabaseManager))]
  public interface IDatabaseManager
  {

    /// <summary>
    /// Gets the version for a subset of the schema 
    /// </summary>
    /// <param name="schemaSetame">The schema setame.</param>
    /// <returns></returns>
    SchemaVersion GetSchemaVersion(string schemaSetame);

    /// <summary>
    /// Executes the DDL.
    /// </summary>
    /// <param name="ddl">The DDL.</param>
    /// <param name="databaseName">Name of the database.</param>
    void ExecuteDDL(string ddl, string databaseName = null);

    /// <summary>
    /// Executes the DDL with overridden variables
    /// </summary>
    /// <param name="ddl">The DDL.</param>
    /// <param name="databaseName">Name of the database.</param>
    /// <param name="variables">The variables.</param>
    void ExecuteDDL(string ddl, string databaseName, Dictionary<string, string> variables = null);

    /// <summary>
    /// Databases the exists.
    /// </summary>
    /// <param name="connectionString">The connection string.</param>
    /// <returns></returns>
    bool DatabaseExists();

    /// <summary>
    /// Updates the schema version.
    /// </summary>
    /// <param name="schemaSetName">Name of the schema set.</param>
    /// <param name="version">The version.</param>
    void UpdateSchemaVersion(string schemaSetName, int version);

    /// <summary>
    /// Updates the base schema.
    /// </summary>
    void UpdateBaseSchema();

    /// <summary>
    /// Updates a schema subset and the version record for it
    /// </summary>
    /// <param name="schemaSetName">The schema set name.</param>
    /// <param name="version">The version.</param>
    /// <param name="updater">The updater.</param>
    void UpdateSchema(string schemaSetName, int version, Action updater);

  }
}
