using Jonesie.Web.Utilities;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;
using System.Web.Mvc;

namespace Jonesie.Web.Site
{
  public static class MEFConfig
  {

    /// <summary>
    /// Gets the container.
    /// </summary>
    /// <value>
    /// The container.
    /// </value>
    public static CompositionContainer Container { get; private set; }

    /// <summary>
    /// Registers MEF parts in a container and configure MVC to use it.
    /// </summary>
    /// <param name="app">The app.</param>
    /// <param name="modulePath">The module path.</param>
    public static void RegisterMEF(MvcApplication app, string modulePath)
    {
      lock (app)
      {
        if (Container == null)
        {
          createContainer(modulePath);

          // use a custom controller factory that uses MEF
          ControllerBuilder.Current.SetControllerFactory(new MEFControllerFactory(Container));

          var resolver = new MEFDependencyResolver(Container);
          // Install MEF dependency resolver for MVC
          DependencyResolver.SetResolver(resolver);
          // Install MEF dependency resolver for Web API
          System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }
      }
    }

    /// <summary>
    /// Creates the container and fills it with goodies
    /// </summary>
    /// <param name="modulePath">The module path.</param>
    private static void createContainer(string modulePath)
    {
      var aggCatalog = new AggregateCatalog(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
      aggCatalog.Catalogs.Add(new DirectoryCatalog(modulePath, Properties.Settings.Default.Modules)); 
      aggCatalog.Catalogs.Add(new AssemblyCatalog(Assembly.LoadFrom(Path.Combine(modulePath, Properties.Settings.Default.LoggingProvider))));
      aggCatalog.Catalogs.Add(new AssemblyCatalog(Assembly.LoadFrom(Path.Combine(modulePath, Properties.Settings.Default.DataProvider))));
      aggCatalog.Catalogs.Add(new AssemblyCatalog(Assembly.LoadFrom(Path.Combine(modulePath, Properties.Settings.Default.CacheProvider))));
      Container = new CompositionContainer(aggCatalog);
    }
  }
}