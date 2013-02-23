using Jonesie.Web.Common;
using Jonesie.Web.Contracts.Core;
using Jonesie.Web.Contracts.Data;
using Jonesie.Web.Entities.Data;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using Dapper;
using DapperExtensions;

namespace Jonesie.Web.Data
{
  /// <summary>
  /// Provide a service for accessing navigation data for the site
  /// </summary>
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class NavigationRepository : BaseRepository, INavigationRepository
  {

    /// <summary>
    /// Gets all the options.
    /// </summary>
    /// <returns></returns>
    public DataSet<SiteNavigation> GetOptions()
    {
      return GetOptions(null);
    }

    /// <summary>
    /// Gets the options at any level of the menu.
    /// </summary>
    /// <param name="menuName">Name of the menu.</param>
    /// <param name="activeOnly">if set to <c>true</c> [active only].</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public DataSet<SiteNavigation> GetOptions(string menuName, bool activeOnly = false)
    {
      return PerformCachedQuery<DataSet<SiteNavigation>>("NavigationRepository.GetOptions({0})".FormatWith(menuName),
          (con) =>
          {
            var result = con.Query<SiteNavigation>(
                Properties.Resources.SiteNavigation_ByMenuNameList_Get,
                new { MenuName = menuName, ActiveOnly = activeOnly });

            return new DataSet<SiteNavigation>(result, result.Count(), pageSize: result.Count(), page: 0);

          });
    }


    /// <summary>
    /// Gets a single option.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns></returns>
    public SiteNavigation GetOption(int id)
    {
      return PerformCachedQuery<SiteNavigation>("NavigationRepository.GetOption({0})".FormatWith(id),
          (con) =>
          {
            return con.Get<SiteNavigation>(id);

            //return GetConnection().Query<NavigationOption>(
            //    Properties.Resources.SiteNavigation_Get,
            //    new { SiteNavigationId = id }).FirstOrDefault();
          });
    }

    /// <summary>
    /// Deletes the option.
    /// </summary>
    /// <param name="id">The id.</param>
    public void DeleteOption(int id)
    {
      ExecuteQuery("NavigationRepository.DeleteOption({0})".FormatWith(id),
          (con) =>
          {
            con.Execute(Properties.Resources.SiteNavigation_Delete, new { SiteNavigationId = id });
            Cache.RemoveFromCache("NavigationRepository.*");
          });
    }

    /// <summary>
    /// Saves the option.
    /// </summary>
    /// <param name="option">The option.</param>
    public void UpdateOption(SiteNavigation option)
    {
      ExecuteQuery("NavigationRepository.SaveOption({0})".FormatWith(option.DisplayLabel),
          (con) =>
          {
            con.Execute(Properties.Resources.SiteNavigation_Update, option);
            Cache.RemoveFromCache("NavigationRepository.*");
          });
    }
  }
}
