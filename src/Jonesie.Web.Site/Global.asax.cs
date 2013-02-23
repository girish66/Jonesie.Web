using Jonesie.Web.Common;
using Jonesie.Web.Contracts.Core;
using Jonesie.Web.Contracts.Data;
using Jonesie.Web.Entities.Core;
using Jonesie.Web.Site.Controllers;
using Jonesie.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebMatrix.WebData;

namespace Jonesie.Web.Site
{
  /// <summary>
  /// The main application
  /// </summary>
  public class MvcApplication : System.Web.HttpApplication
  {

    static ILogger _logger;
    static ISettings _settings;

    /// <summary>
    /// Start the Application.
    /// </summary>
    protected void Application_Start()
    {
      System.Diagnostics.Trace.WriteLine("{0} :: Application Starting".FormatWith(DateTime.Now.ToString("HH:mm:ss.ff")));

      // MEF will load all the modules from the specified path
      // currently this is just the normal bin folder as having them
      // anywhere else can cause problems for the views

      System.Diagnostics.Trace.WriteLine("{0} ::  Initialising MEF".FormatWith(DateTime.Now.ToString("HH:mm:ss.ff")));

      MEFConfig.RegisterMEF(this, Server.MapPath(@"~\bin\"));

      // System.Diagnostics.Trace.WriteLine("{0} ::  Initialising SignalR".FormatWith(DateTime.Now.ToString("HH:mm:ss.ff")));
      // GlobalHost.DependencyResolver = new SignalRMefDependencyResolver(MefConfig.Container);
      // RouteTable.Routes.MapHubs(); //new SignalRMefDependencyResolver(MefConfig.Container)

      // once mef is configured we can check the database
      CheckDatabase(); 


      _logger = MEFConfig.Container.GetExport<ILogger>().Value;
      _settings = MEFConfig.Container.GetExport<ISettings>().Value;
      _logger.Indent();
      try
      {

        System.Diagnostics.Trace.WriteLine("{0} ::  Configuring View Engine".FormatWith(DateTime.Now.ToString("HH:mm:ss.ff")));

        // Setup a custom view engine.
        // This knows how to deal with views in module folders.
        ViewEngines.Engines.Clear();

        // WARNING!!! Helpers assume that JonesieViewEngine is the first index in the ViewEngines array!!! Changing this
        // will break a lot of things.
        ViewEngines.Engines.Add(new JonesieViewEngine(MEFConfig.Container.GetExports<IViewModule>()));

        // Just the usual MVC startup fluff
        AreaRegistration.RegisterAllAreas();
        AuthConfig.RegisterAuth(_settings);
        WebApiConfig.Register(GlobalConfiguration.Configuration);
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

        // need to map the SignalR hub early
        RouteTable.Routes.MapHubs();

        // register routes for service and view modules and bundles for view modules
        ModuleConfig.RegisterRoutes(RouteTable.Routes, GlobalConfiguration.Configuration.Routes, _logger);
        ModuleConfig.RegisterBundles(BundleTable.Bundles, _logger);

        // now that everything else is done we can tell the modules to go ahead and get ready 
        ModuleConfig.InitialiseModules(_logger); 

        // register the default routes and bundle after the modules
        RouteConfig.RegisterRoutes(RouteTable.Routes);
        BundleConfig.RegisterBundles(BundleTable.Bundles);

        // we need to do this early so we can use IsInRole for security trimming the menus
        System.Diagnostics.Trace.WriteLine("{0} ::  Initialising Web Security".FormatWith(DateTime.Now.ToString("HH:mm:ss.ff")));
        WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", false);
      }
      finally
      {
        _logger.UnIndent();
      }

      System.Diagnostics.Trace.WriteLine("{0} :: Application Started".FormatWith(DateTime.Now.ToString("HH:mm:ss.ff")));
    }

    /// <summary>
    /// Handles the Error event of the Application control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    protected void Application_Error(object sender, EventArgs e)
    {
      var httpContext = ((MvcApplication)sender).Context;

      var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));
      var currentController = " ";
      var currentAction = " ";

      if (currentRouteData != null) 
      {
        if (currentRouteData.Values["controller"] != null && !String.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
        {
          currentController = currentRouteData.Values["controller"].ToString();
        }

        if (currentRouteData.Values["action"] != null && !String.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
        {
          currentAction = currentRouteData.Values["action"].ToString();
        }
      }

      var ex = Server.GetLastError();

      var controller = new ErrorController();
      var routeData = new RouteData();
      var action = "Index";

      if (ex is HttpException)
      {
        var httpEx = ex as HttpException;

        switch (httpEx.GetHttpCode())
        {
          case 404:
            action = "HttpError404";
            break;

          // others if any

          default:
            action = "Index";
            break;
        }

        _logger.LogError("HTTP ERROR: {0} :: {1}".FormatWith(ex.Message, httpEx.GetHttpCode()), ex);
      }
      else
      {
        _logger.LogError(ex.Message, ex);
      }

      httpContext.ClearError();
      httpContext.Response.Clear();
      httpContext.Response.StatusCode = ex is HttpException ? ((HttpException)ex).GetHttpCode() : 500;
      httpContext.Response.TrySkipIisCustomErrors = true;
      routeData.Values["controller"] = "Error";
      routeData.Values["action"] = action;

      controller.ViewData.Model = new HandleErrorInfo(ex, currentController, currentAction);
      ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
    }

    /// <summary>
    /// Handles the End event of the Session control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    protected void Session_End(object sender, EventArgs e)
    {
      var sess = _settings.Sessions.FirstOrDefault(s => s.SessionId == Session.SessionID);
      if (sess != null)
        _settings.Sessions.Remove(sess);
    }

    /// <summary>
    /// Checks the database for existence and correctness.
    /// </summary>
    public void CheckDatabase()
    {
      var dbManager = MEFConfig.Container.GetExport<IDatabaseManager>();

      // apply any version upgrades - but only once
      if (dbManager  != null)
      {
        dbManager.Value.UpdateBaseSchema();
      }
    }
  }
}