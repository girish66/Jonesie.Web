using Jonesie.Web.Contracts.Core;
using Jonesie.Web.Contracts.Data;
using Jonesie.Web.Entities.Data;
using Jonesie.Web.Module.JonesieHome.Controllers;
using System;
using System.Linq;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Jonesie.Web.Module.JonesieHome
{
  /// <summary>
  /// The definition of this module
  /// </summary>
  public class Module : IViewModule
  {

    static bool _initDoneOnce = false;

    /// <summary>
    /// Gets or sets the database manager.
    /// </summary>
    /// <value>
    /// The database manager.
    /// </value>
    [Import]
    public IDatabaseManager DatabaseManager { get; set; }

    /// <summary>
    /// Gets or sets the navigation repository.
    /// </summary>
    /// <value>
    /// The navigation repository.
    /// </value>
    [Import]
    public INavigationRepository NavigationRepository { get; set; }

    /// <summary>
    /// Gets the controllers.
    /// </summary>
    /// <value>
    /// The controllers.
    /// </value>
    public Type[] Controllers
    {
      get
      {
        return new Type[] { typeof(HomeController) };
      }
    }

    /// <summary>
    /// Registers the css and script bundles for this view module
    /// </summary>
    /// <param name="bundles">The bundles.</param>
    public void RegisterBundles(BundleCollection bundles)
    {
      //bundles.Add(new ScriptBundle("~/bundles/salescompanies").Include(
      //"~/Modules/{0}/Scripts/SalesCompaniesRemoteModel*".FormatWith(Name)
      //));
    }

    /// <summary>
    /// Registers the routes.
    /// </summary>
    /// <param name="routes">The routes.</param>
    public void RegisterRoutes(RouteCollection routes)
    {
      //routes.MapRoute(
      //    name: Properties.Settings.Default.RouteName,
      //    url: "{controller}/{action}/{id}",
      //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
      //);
    }

    /// <summary>
    /// Initialize internal fluff.
    /// </summary>
    public void InternalInit()
    {
      if (!_initDoneOnce)
      {
        // do stuff just once
        // make sure we have the default menu set for this module
        if (!NavigationRepository.GetOptions("Home").Items.Any(no => no.Action == "About"))
        {
          NavigationRepository.UpdateOption(new SiteNavigation
          {
            Action = "About",
            Active = true,
            Controller = "Home",
            DisplayLabel = "Home",
            MenuName = "Home",
            OptionOrder = 10,
            Roles = string.Empty,
            Url = null
          });
        }

        if (!NavigationRepository.GetOptions("Home").Items.Any(no => no.Action == "Contact"))
        {
          NavigationRepository.UpdateOption(new SiteNavigation
          {
            Action = "Contact",
            Active = true,
            Controller = "Home",
            DisplayLabel = "Contact",
            MenuName = "Home",
            OptionOrder = 20,
            Roles = string.Empty,
            Url = null
          });
        }

        _initDoneOnce = true;
      }
    }
  }
}