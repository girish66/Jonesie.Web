using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Jonesie.Web.Contracts.Data;
using Dapper;

namespace Jonesie.Web.Data
{
  /// <summary>
  /// A connection factory that is shared by all - one connection per session one hopes
  /// </summary>
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class ConnectionFactory : IConnectionFactory, IDisposable
  {

    /// <summary>
    /// The _connections
    /// </summary>
    Dictionary<string, IDbConnection> _connections = new Dictionary<string, IDbConnection>();

    /// <summary>
    /// See if the databases exists.
    /// </summary>
    /// <param name="dbName">Name of the db.</param>
    /// <returns></returns>
    public bool DatabaseExists(string connectionString)
    {
      var cb = new SqlConnectionStringBuilder(connectionString);
      var dbName = cb.InitialCatalog;
      cb.InitialCatalog = "master";
      using(var connection = new SqlConnection(cb.ConnectionString)) 
      {
        connection.Open();
        return connection.Query<int>(Properties.Resources.Schema_DBExists, new { DatabaseName = dbName }).SingleOrDefault() == 1;
      }
    }



    /// <summary>
    /// Gets the connection.
    /// </summary>
    /// <param name="connectionString">The connection string.</param>
    /// <returns></returns>
    public IDbConnection GetConnection(string connectionString)
    {
      if (_connections.ContainsKey(connectionString))
      {
        return _connections[connectionString];
      }

      var con = new SqlConnection(connectionString);
      con.Open();

      //if (Transaction.Current != null)
      //{
      //    con.EnlistTransaction(Transaction.Current);
      //}

      _connections.Add(connectionString, con);

      return con;
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      foreach (var con in _connections.Values)
      {
        con.Close();
        con.Dispose();
      }

      _connections.Clear();
    }
  }
}
