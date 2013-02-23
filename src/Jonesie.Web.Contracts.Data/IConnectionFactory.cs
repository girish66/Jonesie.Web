using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jonesie.Web.Contracts.Data
{
  /// <summary>
  /// A contract for a database connection factory
  /// </summary>
  [InheritedExport]
  public interface IConnectionFactory
  {
    /// <summary>
    /// Databases the exists.
    /// </summary>
    /// <param name="connectionString">The connection string.</param>
    /// <returns></returns>
    bool DatabaseExists(string connectionString);

    /// <summary>
    /// Gets the connection.
    /// </summary>
    /// <param name="connectionString">The connection string.</param>
    /// <returns></returns>
    IDbConnection GetConnection(string connectionString);
  }
}
