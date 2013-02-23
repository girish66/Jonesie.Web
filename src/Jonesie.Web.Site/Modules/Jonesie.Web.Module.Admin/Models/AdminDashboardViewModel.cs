using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jonesie.Web.Module.Admin.Models
{
  /// <summary>
  /// A view module for the admin dashboard
  /// </summary>
  public class AdminDashboardViewModel
  {
    #region properties
    /// <summary>
    /// Gets or sets the current online user count.
    /// </summary>
    /// <value>
    /// The current online user count.
    /// </value>
    public int CurrentOnlineUserCount { get; set; }

    /// <summary>
    /// Gets or sets the total registered user count.
    /// </summary>
    /// <value>
    /// The total registered user count.
    /// </value>
    public int TotalRegisteredUserCount { get; set; }

    /// <summary>
    /// Gets or sets the loaded modules.
    /// </summary>
    /// <value>
    /// The loaded modules.
    /// </value>
    public IEnumerable<string> LoadedModules { get; set; }

    #endregion
  }
}