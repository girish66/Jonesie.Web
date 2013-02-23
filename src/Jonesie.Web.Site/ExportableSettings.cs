using Jonesie.Web.Contracts.Core;
using Jonesie.Web.Entities.Core;
using Jonesie.Web.Entities.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Jonesie.Web.Site
{
  /// <summary>
  /// Settings and global data for the site - shared by all connections
  /// </summary>
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class ExportableSettings : ISettings
  {
    #region members
    /// <summary>
    /// The _sessions
    /// </summary>
    private List<UserSession> _sessions;

    #endregion

    #region construction

    /// <summary>
    /// Initializes a new instance of the <see cref="ExportableSettings" /> class.
    /// </summary>
    public ExportableSettings()
    {
      _sessions = new List<UserSession>();
    }

    #endregion

    #region cache stuff
    public int CacheShortTimeoutSecs
    {
      get { return Properties.Settings.Default.CacheShortTimeoutSecs; }
    }

    public int CacheMediumTimeoutSecs
    {
      get { return Properties.Settings.Default.CacheMediumTimeoutSecs; }
    }

    public int CacheLongTimeoutSecs
    {
      get { return Properties.Settings.Default.CacheLongTimeoutSecs; }
    }

    public string CacheOptions
    {
      get { return Properties.Settings.Default.CacheOptions; }
    }

    #endregion

    #region database stuff

    public string DBConnectionString
    {
      get
      {
        return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
      }
    }

    public int DBDefaultPageSize
    {
      get { return Properties.Settings.Default.DBDefaultPageSize; }
    }

    public string DBPath
    {
        get { return Properties.Settings.Default.DBPath; }
    }

    #endregion

    #region search
    public string SearchConfigurationFileName
    {
      get { return Properties.Settings.Default.SearchConfigurationFileName; }
    }

    #endregion

    #region logging

    public string LogConfiguration
    {
      get { return Properties.Settings.Default.LogConfiguration; }
    }

    #endregion

    #region site global

    public string SiteTitle
    {
      get
      {
        return Properties.Settings.Default.SiteTitle;
      }
    }

    public string SiteCopyright
    {
      get
      {
        return Properties.Settings.Default.SiteCopyright;
      }
    }

    public List<UserSession> Sessions
    {
      get
      {
        return _sessions;
      }
    }

    #endregion
  }
}