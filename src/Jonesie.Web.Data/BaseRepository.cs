using Jonesie.Web.Common;
using Jonesie.Web.Contracts.Core;
using Jonesie.Web.Contracts.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace Jonesie.Web.Data
{
  /// <summary>
  /// A base repository to rule them all
  /// </summary>
  public class BaseRepository
  {
    #region members

    [Import]
    public ISettings Settings { get; protected set; }

    [Import]
    public ILogger Logger { get; protected set; }

    [Import]
    public ICache Cache { get; protected set; }

    [Import]
    public IConnectionFactory ConnectionFactory { get; protected set; }

    [Import]
    public IDatabaseManager DatabaseManager { get; protected set; }

    #endregion

    ///// <summary>
    ///// Initializes a new instance of the <see cref="BaseRepository" /> class.
    ///// </summary>
    //public BaseRepository()
    //{
    //}

    /// <summary>
    /// Opens the connection.
    /// </summary>
    /// <returns></returns>
    public IDbConnection GetConnection()
    {
      return ConnectionFactory.GetConnection(Settings.DBConnectionString);
    }

    /// <summary>
    /// Gets a connection for a specific database 
    /// </summary>
    /// <param name="dbname">The dbname.</param>
    /// <returns></returns>
    public IDbConnection GetExclusiveConnection(string dbname = null)
    {
      var cstring = Settings.DBConnectionString;

      if (dbname != null)
      {
        SqlConnectionStringBuilder cb = new SqlConnectionStringBuilder(Settings.DBConnectionString)
        {
          InitialCatalog = dbname
        };
        cstring = cb.ConnectionString;
      }

      var con = new SqlConnection(cstring);
      con.Open();
      return con;
    }

    /// <summary>
    /// Performs the cached query with logging
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key.</param>
    /// <param name="func">The func.</param>
    /// <returns></returns>
    public T PerformCachedQuery<T>(string key, Func<IDbConnection, T> func, Func<IDbConnection> getConnection = null)
    {
      Logger.LogDebug("START: {0}".FormatWith(key));
      Logger.Indent();
      try
      {
        return Cache.GetFromCache<T>(key, () =>
        {
          using (var scope = new TransactionScope(TransactionScopeOption.Required))
          {
            IDbConnection con = null;
            try
            {
              if (getConnection != null)
                con = getConnection();
              else
                con = GetExclusiveConnection();

              var result = func(con);
              scope.Complete();
              return result;
            }
            finally
            {
              if (con != null)
                con.Dispose();
            }
          }
        });

      }
      catch (Exception ex)
      {
        Logger.LogError("FAIL: {0}".FormatWith(key), ex);
        throw;
      }
      finally
      {
        Logger.UnIndent();
        Logger.LogDebug("END: {0}".FormatWith(key));
      }

    }

    /// <summary>
    /// Performs the non cached query with logging
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key.</param>
    /// <param name="func">The func.</param>
    /// <returns></returns>
    public T PerformNonCachedQuery<T>(string key, Func<IDbConnection, T> func, Func<IDbConnection> getConnection = null)
    {
      IDbConnection con = null;
      Logger.LogDebug("START: {0}".FormatWith(key));
      Logger.Indent();
      try
      {
        using (var scope = new TransactionScope(TransactionScopeOption.Required))
        {
          if (getConnection != null)
            con = getConnection();
          else
            con = GetExclusiveConnection();

          var result = func(con);
          scope.Complete();
          return result;
        }
      }
      catch (Exception ex)
      {
        Logger.LogError("FAIL: {0}".FormatWith(key), ex);
        throw;
      }
      finally
      {
        if (con != null)
          con.Dispose();
        Logger.UnIndent();
        Logger.LogDebug("END: {0}".FormatWith(key));
      }
    }

    /// <summary>
    /// Executes the query with logging
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="action">The action.</param>
    /// <param name="getConnection">The get connection.</param>
    /// <param name="noTransaction">if set to <c>true</c> [no transaction].</param>
    public void ExecuteQuery(string key, Action<IDbConnection> action, Func<IDbConnection> getConnection = null, bool noTransaction = false)
    {
      IDbConnection con = null;
      Logger.LogDebug("START: {0}".FormatWith(key));
      Logger.Indent();
      try
      {
        TransactionScope scope = null;
        if (!noTransaction)
        {
          scope = new TransactionScope(TransactionScopeOption.Required);
        }

        try
        {

          if (getConnection != null)
            con = getConnection();
          else
            con = GetExclusiveConnection();

          action(con);
        }
        finally
        {
          if (scope != null)
          {
            scope.Complete();
            scope.Dispose();
          }
        }
      }
      catch (Exception ex)
      {
        Logger.LogError("FAIL: {0}".FormatWith(key), ex);
        throw;
      }
      finally
      {
        if (con != null)
          con.Dispose();
        Logger.UnIndent();
        Logger.LogDebug("END: {0}".FormatWith(key));
      }
    }


    /*



    /// <summary>
    /// Performs the cached query with logging
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key.</param>
    /// <param name="func">The func.</param>
    /// <returns></returns>
    public T PerformCachedQuery<T>(string key, Func<T> func)
    {
      Logger.LogDebug("START: {0}".FormatWith(key));
      try
      {
        return Cache.GetFromCache<T>(key, () =>
        {
          return func();
        });

      }
      catch (Exception ex)
      {
        Logger.LogError("FAIL: {0}".FormatWith(key), ex);
        throw;
      }
      finally
      {
        Logger.LogDebug("END: {0}".FormatWith(key));
      }

    }

    /// <summary>
    /// Performs the non cached query with logging
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key.</param>
    /// <param name="func">The func.</param>
    /// <returns></returns>
    public T PerformNonCachedQuery<T>(string key, Func<T> func)
    {
      Logger.LogDebug("START: {0}".FormatWith(key));
      try
      {
        return func();
      }
      catch (Exception ex)
      {
        Logger.LogError("FAIL: {0}".FormatWith(key), ex);
        throw;
      }
      finally
      {
        Logger.LogDebug("END: {0}".FormatWith(key));
      }
    }

    /// <summary>
    /// Executes the query with logging
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="action">The action.</param>
    public void ExecuteQuery(string key, Action action)
    {
      Logger.LogDebug("START: {0}".FormatWith(key));
      try
      {
        action();
      }
      catch (Exception ex)
      {
        Logger.LogError("FAIL: {0}".FormatWith(key), ex);
        throw;
      }
      finally
      {
        Logger.LogDebug("END: {0}".FormatWith(key));
      }
    }

     * 
     */
 

    /// <summary>
    /// Gets the first item for page.
    /// </summary>
    /// <param name="page">The page.</param>
    /// <param name="pageSize">Size of the page.</param>
    /// <returns></returns>
    public int GetFirstItemForPage(int page, int pageSize)
    {
      return Math.Max((page - 1) * pageSize + 1, 1);
    }

    /// <summary>
    /// Gets the end item for page.
    /// </summary>
    /// <param name="page">The page.</param>
    /// <param name="pageSize">Size of the page.</param>
    /// <returns></returns>
    public int GetEndItemForPage(int page, int pageSize)
    {
      // all pages?
      if (page == 0)
        return 0;

      return GetFirstItemForPage(page, pageSize) + pageSize - 1;
    }
  }
}
