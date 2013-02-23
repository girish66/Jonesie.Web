using Jonesie.Web.Contracts.Core;
using Jonesie.Web.Contracts.Data;
using Jonesie.Web.Entities.Data;
using Jonesie.Web.Module.Admin.Controllers;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web.Optimization;
using System.Web.Routing;

namespace Jonesie.Web.Module.Admin
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
        return new Type[] { typeof(AdminController), typeof(NavigationController), typeof(SecurityController) };
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
      //    url: "{controller}/{action}/{id}", //" + Properties.Settings.Default.ControllerPath + 
      //    defaults: new { controller = "Blog", action = "Index", id = UrlParameter.Optional }
      //);
    }

    /// <summary>
    /// Initialize internal fluff.
    /// </summary>
    public void InternalInit()
    {
      if (!_initDoneOnce)
      {

        // make sure we have the default menu set for this module
        if (!NavigationRepository.GetOptions("Main").Items.Any(no => no.Controller == "Admin"))
        {
          NavigationRepository.UpdateOption(new SiteNavigation
          {
            Action = "Index",
            Active = true,
            Controller = "Admin",
            DisplayLabel = "Admin",
            MenuName = "Main",
            OptionOrder = 9000,
            Roles = "Administrators",
            Url = null
          });

          NavigationRepository.UpdateOption(new SiteNavigation
          {
            Action = "Index",
            Active = true,
            Controller = "Admin",
            DisplayLabel = "Dashboard",
            MenuName = "Admin",
            OptionOrder = 10,
            Roles = "Administrators",
            Url = null
          });

          NavigationRepository.UpdateOption(new SiteNavigation
          {
            Action = "Index",
            Active = true,
            Controller = "Navigation",
            DisplayLabel = "Navigation",
            MenuName = "Admin",
            OptionOrder = 20,
            Roles = "Administrators",
            Url = null
          });

          NavigationRepository.UpdateOption(new SiteNavigation
          {
            Action = "Index",
            Active = true,
            Controller = "Security",
            DisplayLabel = "Security",
            MenuName = "Admin",
            OptionOrder = 30,
            Roles = "Administrators",
            Url = null
          });
        }

        _initDoneOnce = true;
      }
    }
  }
}