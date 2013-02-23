using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Routing;
using Jonesie.Web.Contracts.Core;
using Jonesie.Web.Module.Blog.Controllers;
using Jonesie.Web.Contracts.Data;
using System.ComponentModel.Composition;
using Jonesie.Web.Entities.Data;

namespace Jonesie.Web.Module.Blog
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
        return new Type[] { typeof(BlogController) };
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
        // make sure we have the correct database schema
        DatabaseManager.UpdateSchema("Blog", 1, () => { DatabaseManager.ExecuteDDL(Properties.Resources.Schema_Blog_1); });

        // make sure we have the default menu set for this module
        if(!NavigationRepository.GetOptions("Main").Items.Any(no => no.Controller == "Blog" && no.Action == "Index"))
        {
          NavigationRepository.UpdateOption(new SiteNavigation
          {
            Action = "Index", Active = true, Controller = "Blog", DisplayLabel = "Blog", MenuName = "Main", OptionOrder = 10, Roles = string.Empty, Url = null 
          });
        }

        _initDoneOnce = true;
      }
    }
  }
}