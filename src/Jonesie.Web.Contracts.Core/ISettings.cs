using Jonesie.Web.Entities.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jonesie.Web.Contracts.Core
{
    /// <summary>
    /// A contract for some globally shared settings - which may be considered a dirty hack ....
    /// </summary>
    [InheritedExport(typeof(ISettings))]
    public interface ISettings
    {

        #region general

        string SiteTitle { get; }

        string SiteCopyright { get; }

        List<UserSession> Sessions { get; }

        #endregion

        #region caching control

        /// <summary>
        /// Gets the cache short timeout secs.
        /// </summary>
        /// <value>
        /// The cache short timeout secs.
        /// </value>
        int CacheShortTimeoutSecs { get; }
        /// <summary>
        /// Gets the cache medium timeout secs.
        /// </summary>
        /// <value>
        /// The cache medium timeout secs.
        /// </value>
        int CacheMediumTimeoutSecs { get; }
        /// <summary>
        /// Gets the cache long timeout secs.
        /// </summary>
        /// <value>
        /// The cache long timeout secs.
        /// </value>
        int CacheLongTimeoutSecs { get; }
        /// <summary>
        /// Gets the cache options.
        /// </summary>
        /// <value>
        /// The cache options.
        /// </value>
        string CacheOptions { get; }

        #endregion

        #region database

        /// <summary>
        /// Gets the DB connection string.
        /// </summary>
        /// <value>
        /// The DB connection string.
        /// </value>
        string DBConnectionString { get; }

        /// <summary>
        /// Gets the default number of records for a page of records.
        /// </summary>
        /// <value>
        /// The default size of the page.
        /// </value>
        int DBDefaultPageSize { get; }

        /// <summary>
        /// Path to the database files
        /// </summary>
        string DBPath { get; }

        #endregion

        #region search

        /// <summary>
        /// Gets or sets the name of the search configuration file.
        /// </summary>
        /// <value>
        /// The name of the search configuration file.
        /// </value>
        string SearchConfigurationFileName { get; }

        #endregion

        #region logging

        /// <summary>
        /// Gets or sets the name of the log.
        /// </summary>
        /// <value>
        /// The name of the log.
        /// </value>
        string LogConfiguration { get; }

        #endregion

    }
}
