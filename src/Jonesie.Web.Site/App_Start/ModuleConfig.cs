using Jonesie.Web.Contracts.Core;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Optimization;
using System.Web.Routing;


namespace Jonesie.Web.Site
{
  /// <summary>
  /// Module startup helpers
  /// </summary>
  public class ModuleConfig
  {
    /// <summary>
    /// Registers the routes.
    /// </summary>
    /// <param name="viewRoutes">The view routes.</param>
    /// <param name="apiRoutes">The API routes.</param>
    /// <param name="logger">The logger.</param>
    public static void RegisterRoutes(RouteCollection viewRoutes, HttpRouteCollection apiRoutes, ILogger logger)
    {

      logger.LogDebug("START: ModuleConfig.RegisterRoutes");
      try
      {
        logger.Indent();

        logger.LogDebug("GetViewModules");
        var vModules = MEFConfig.Container.GetExports<IViewModule>();
        logger.LogDebug("GetServiceModules");
        var sModules = MEFConfig.Container.GetExports<IServiceModule>();

        logger.LogDebug("Iterating ViewModules");
        logger.Indent();
        foreach (var module in vModules)
        {
          var mod = module.Value;
          mod.RegisterRoutes(viewRoutes);

          logger.LogDebug(mod.GetType().FullName);
        }
        logger.UnIndent();

        logger.LogDebug("Iterating ServiceModules");
        logger.Indent();
        foreach (var module in sModules)
        {
          var mod = module.Value;
          mod.RegisterRoutes(apiRoutes);

          logger.LogDebug(mod.GetType().FullName);
        }
        logger.UnIndent();
      }
      finally
      {
        logger.UnIndent();
        logger.LogDebug("END: ModuleConfig.RegisterRoutes");
      }
    }

    /// <summary>
    /// Registers the bundles.
    /// </summary>
    /// <param name="bundles">The bundles.</param>
    /// <param name="logger">The logger.</param>
    public static void RegisterBundles(BundleCollection bundles, ILogger logger)
    {
      logger.LogDebug("START: ModuleConfig.RegisterBundles");
      try
      {
        logger.Indent();
        logger.LogDebug("GetViewModules");
        var vModules = MEFConfig.Container.GetExports<IViewModule>();

        // process each module
        logger.LogDebug("Iterating ViewModules");
        logger.Indent();
        foreach (var module in vModules)
        {
          // get the non lazy module value
          var mod = module.Value; // as IViewModule;

          if (mod != null)
          {
            // add any bundles the module may need
            mod.RegisterBundles(bundles);
          }

          logger.LogDebug(mod.GetType().FullName);
        }
        logger.UnIndent();

      }
      finally
      {
        logger.UnIndent();
        logger.LogDebug("END: ModuleConfig.RegisterBundles");
      }
    }

    /// <summary>
    /// Initialises the modules internals.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public static void InitialiseModules(ILogger logger)
    {
      logger.LogDebug("START: ModuleConfig.InitialiseModules");
      try
      {
        logger.Indent();
        logger.LogDebug("GetModules");

        var modules = MEFConfig.Container.GetExports<IServiceModule>().Select(m => (IModule)(m.Value))
                    .Union(
                        MEFConfig.Container.GetExports<IViewModule>().Select(m => (IModule)(m.Value))
                    );

        // process each module
        logger.LogDebug("Iterating Modules");
        logger.Indent();
        foreach (var module in modules)
        {
          // get the non lazy module value
          if (module != null)
          {
            module.InternalInit();
          }

          logger.LogDebug(module.GetType().FullName);
        }
        logger.UnIndent();
      }
      finally
      {
        logger.UnIndent();
        logger.LogDebug("END: ModuleConfig.InitialiseModules");
      }
    }
  }
}