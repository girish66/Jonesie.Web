using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nustache.Mvc;
using Jonesie.Web.Contracts.Core;

namespace Jonesie.Web.Utilities
{
  /// <summary>
  /// A custom view engine that extends the Razor one slightly
  /// </summary>
  public class JonesieViewEngine : RazorViewEngine
  {
    #region members
    /// <summary>
    /// The empty locations
    /// </summary>
    static readonly string[] _emptyLocations;


    /// <summary>
    /// The _modules that are loaded
    /// </summary>
    IEnumerable<Lazy<IViewModule>> _modules;

    #endregion

    #region props

    /// <summary>
    /// The view location formats for modules
    /// </summary>
    public string[] ModuleViewLocationFormats { get; private set; }

    /// <summary>
    /// The partial view location formats for modules
    /// </summary>
    public string[] ModulePartialViewLocationFormats { get; private set; }

    #endregion

    #region construction
    /// <summary>
    /// Initializes a new instance of the <see cref="JonesieViewEngine" /> class.
    /// </summary>
    /// <param name="modules">The modules.</param>
    public JonesieViewEngine(IEnumerable<Lazy<IViewModule>> modules)
    {
      _modules = modules;
      ModuleViewLocationFormats = new string[] { };
      ModulePartialViewLocationFormats = new string[] { };

      FileExtensions = new string[] { "cshtml", "mustache" };

      // allow for views to be in the modules folder
      List<string> viewLocationFormats = new List<string>();
      List<string> partialViewLocationFormats = new List<string>();
      List<string> moduleViewLocationFormats = new List<string>();
      List<string> modulePartialViewLocationFormats = new List<string>();

      foreach (string ext in FileExtensions)
      {

        // Move mobile views to a different folder rather than mixing them with normal views
        // {0} = view name
        // {1} = controller name
        viewLocationFormats.Add("~/Views/{1}/{0}." + ext);
        //viewLocationFormats.Add("~/Views/{1}/Mobile/{0}." + ext);
        viewLocationFormats.Add("~/Views/Shared/{0}." + ext);
        //viewLocationFormats.Add("~/Views/Shared/Mobile/{0}." + ext);

        partialViewLocationFormats.Add("~/Views/{1}/{0}." + ext);
        //partialViewLocationFormats.Add("~/Views/{1}/Mobile/{0}." + ext);
        partialViewLocationFormats.Add("~/Views/Shared/{0}." + ext);
        //partialViewLocationFormats.Add("~/Views/Shared/Mobile/{0}." + ext);

        // {2} = module name
        moduleViewLocationFormats.Add("~/Modules/{2}/Views/{1}/{0}." + ext);
        //moduleViewLocationFormats.Add("~/Modules/{2}/Views/{1}/Mobile/{0}." + ext);
        moduleViewLocationFormats.Add("~/Modules/{2}/Views/Shared/{0}." + ext);
        //moduleViewLocationFormats.Add("~/Modules/{2}/Views/Shared/Mobile/{0}." + ext);

        // modules can also use views from the main site
        moduleViewLocationFormats.AddRange(viewLocationFormats);

        modulePartialViewLocationFormats.Add("~/Modules/{2}/Views/{1}/{0}." + ext);
        //modulePartialViewLocationFormats.Add("~/Modules/{2}/Views/{1}/Mobile/{0}." + ext);
        modulePartialViewLocationFormats.Add("~/Modules/{2}/Views/Shared/{0}." + ext);
        //modulePartialViewLocationFormats.Add("~/Modules/{2}/Views/Shared/Mobile/{0}." + ext);

        // modules can also use partials from the main site
        modulePartialViewLocationFormats.AddRange(partialViewLocationFormats);
      }

      ViewLocationFormats = viewLocationFormats.ToArray();
      PartialViewLocationFormats = partialViewLocationFormats.ToArray();
      ModuleViewLocationFormats = moduleViewLocationFormats.ToArray();
      ModulePartialViewLocationFormats = modulePartialViewLocationFormats.ToArray();

    }

    #endregion

    /// <summary>
    /// Creates a view by using the specified controller context and the paths of the view and master view.
    /// </summary>
    /// <param name="controllerContext">The controller context.</param>
    /// <param name="viewPath">The path to the view.</param>
    /// <param name="masterPath">The path to the master view.</param>
    /// <returns>
    /// The view.
    /// </returns>
    protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
    {
      if (viewPath.EndsWith(".mustache"))
      {
        return new NustacheView(this, controllerContext, viewPath, masterPath);
      }
      return base.CreateView(controllerContext, viewPath, masterPath);
    }

    /// <summary>
    /// Creates a partial view using the specified controller context and partial path.
    /// </summary>
    /// <param name="controllerContext">The controller context.</param>
    /// <param name="partialPath">The path to the partial view.</param>
    /// <returns>
    /// The partial view.
    /// </returns>
    protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
    {
      if (partialPath.EndsWith(".mustache"))
      {
        return new NustacheView(this, controllerContext, partialPath, null);
      }
      return base.CreatePartialView(controllerContext, partialPath);
    }

    /// <summary>
    /// Finds the specified view by using the specified controller context and master view name.
    /// </summary>
    /// <param name="controllerContext">The controller context.</param>
    /// <param name="viewName">The name of the view.</param>
    /// <param name="masterName">The name of the master view.</param>
    /// <param name="useCache">true to use the cached view.</param>
    /// <returns>
    /// The page view.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">controllerContext</exception>
    /// <exception cref="System.ArgumentException">viewName must be specified.;viewName</exception>
    public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
    {
      IViewModule module = GetModule(controllerContext.Controller);

      if (controllerContext == null)
      {
        throw new ArgumentNullException("controllerContext");
      }

      if (string.IsNullOrEmpty(viewName))
      {
        throw new ArgumentException("viewName must be specified.", "viewName");
      }

      if (module != null)
      {
        string controllerName = controllerContext.RouteData.GetRequiredString("controller");
        string[] strArray, strArray2;

        string viewPath = this.GetPath(
            controllerContext,
            ModuleViewLocationFormats,
            "ModuleViewLocationFormats",
            viewName,
            module.GetType().Namespace,
            controllerName,
            "View",
            useCache,
            out strArray);

        string masterPath = this.GetPath(
            controllerContext,
            this.MasterLocationFormats,
            "MasterLocationFormats",
            masterName,
            module.GetType().Namespace,
            controllerName,
            "Master",
            useCache,
            out strArray2);

        if (!string.IsNullOrEmpty(viewPath) && (!string.IsNullOrEmpty(masterPath) || string.IsNullOrEmpty(masterName)))
        {
          return new ViewEngineResult(this.CreateView(controllerContext, viewPath, masterPath), this);
        }

        if (strArray2 != null)
        {
          strArray = strArray.Union<string>(strArray2).ToArray();
        }

        return new ViewEngineResult(strArray);
      }
      else
      {
        return base.FindView(controllerContext, viewName, masterName, useCache);
      }
    }

    /// <summary>
    /// Finds the specified partial view by using the specified controller context.
    /// </summary>
    /// <param name="controllerContext">The controller context.</param>
    /// <param name="partialViewName">The name of the partial view.</param>
    /// <param name="useCache">true to use the cached partial view.</param>
    /// <returns>
    /// The partial view.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">controllerContext</exception>
    /// <exception cref="System.ArgumentException">viewName must be specified.;viewName</exception>
    public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
    {
      IViewModule module = GetModule(controllerContext.Controller); 

      if (controllerContext == null)
      {
        throw new ArgumentNullException("controllerContext");
      }

      if (string.IsNullOrEmpty(partialViewName))
      {
        throw new ArgumentException("viewName must be specified.", "partialViewName");
      }

      if (module != null)
      {
        string controllerName = controllerContext.RouteData.GetRequiredString("controller");
        string[] strArray, strArray2;

        string partialViewPath = this.GetPath(
            controllerContext,
            ModulePartialViewLocationFormats,
            "ModulePartialViewLocationFormats",
            partialViewName,
            module.GetType().Namespace,
            controllerName,
            "Partial",
            useCache,
            out strArray);

        if (string.IsNullOrEmpty(partialViewPath))
        {
          return new ViewEngineResult(strArray);
        }
        return new ViewEngineResult(this.CreatePartialView(controllerContext, partialViewPath), this);
      }
      else
      {
        return base.FindPartialView(controllerContext, partialViewName, useCache);
      }
    }

    /// <summary>
    /// Gets a value that indicates whether a file exists in the specified virtual file system (path).
    /// </summary>
    /// <param name="controllerContext">The controller context.</param>
    /// <param name="virtualPath">The virtual path.</param>
    /// <returns>
    /// true if the file exists in the virtual file system; otherwise, false.
    /// </returns>
    protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
    {
      try
      {
        return File.Exists(controllerContext.HttpContext.Server.MapPath(virtualPath));
      }
      catch (HttpException exception)
      {
        if (exception.GetHttpCode() != 0x194)
          throw;
        return false;
      }
      catch
      {
        return false;
      }
    }

    #region Utilities

    /// <summary>
    /// Gets the module.
    /// </summary>
    /// <param name="controller">The controller.</param>
    /// <returns></returns>
    public IViewModule GetModule(ControllerBase controller)
    {
      var lazyMod = _modules.FirstOrDefault(m => m.Value.Controllers.Contains(controller.GetType()));

      if (lazyMod != null)
        return lazyMod.Value;

      return null;
    }

    /// <summary>
    /// Gets the path.
    /// </summary>
    /// <param name="controllerContext">The controller context.</param>
    /// <param name="locations">The locations.</param>
    /// <param name="locationsPropertyName">Name of the locations property.</param>
    /// <param name="name">The name.</param>
    /// <param name="moduleName">Name of the module.</param>
    /// <param name="controllerName">Name of the controller.</param>
    /// <param name="cacheKeyPrefix">The cache key prefix.</param>
    /// <param name="useCache">if set to <c>true</c> [use cache].</param>
    /// <param name="searchedLocations">The searched locations.</param>
    /// <returns></returns>
    /// <exception cref="System.InvalidOperationException">locations must not be null or emtpy.</exception>
    private string GetPath(ControllerContext controllerContext, string[] locations, string locationsPropertyName,
        string name, string moduleName, string controllerName, string cacheKeyPrefix, bool useCache, out string[] searchedLocations)
    {
      searchedLocations = _emptyLocations;
      if (string.IsNullOrEmpty(name))
        return string.Empty;
      if ((locations == null) || (locations.Length == 0))
        throw new InvalidOperationException("locations must not be null or emtpy.");
      bool flag = IsSpecificPath(name);
      string key = this.CreateCacheKey(cacheKeyPrefix, name, flag ? string.Empty : controllerName, moduleName);
      if (useCache)
      {
        string viewLocation = this.ViewLocationCache.GetViewLocation(controllerContext.HttpContext, key);
        if (viewLocation != null)
        {
          return viewLocation;
        }
      }
      if (!flag)
      {
        return this.GetPathFromGeneralName(controllerContext, locations, name, controllerName, moduleName, key, ref searchedLocations);
      }
      return this.GetPathFromSpecificName(controllerContext, name, key, ref searchedLocations);
    }

    /// <summary>
    /// Determines whether [is specific path] [the specified name].
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>
    ///   <c>true</c> if [is specific path] [the specified name]; otherwise, <c>false</c>.
    /// </returns>
    private static bool IsSpecificPath(string name)
    {
      char ch = name[0];
      if (ch != '~')
        return (ch == '/');
      return true;
    }

    /// <summary>
    /// Creates the cache key.
    /// </summary>
    /// <param name="prefix">The prefix.</param>
    /// <param name="name">The name.</param>
    /// <param name="controllerName">Name of the controller.</param>
    /// <param name="moduleName">Name of the module.</param>
    /// <returns></returns>
    private string CreateCacheKey(string prefix, string name, string controllerName, string moduleName)
    {
      return string.Format(CultureInfo.InvariantCulture, ":ViewCacheEntry:{0}:{1}:{2}:{3}:{4}",
          new object[] { base.GetType().AssemblyQualifiedName, prefix, name, controllerName, moduleName });
    }

    /// <summary>
    /// Gets the name of the path from general.
    /// </summary>
    /// <param name="controllerContext">The controller context.</param>
    /// <param name="locations">The locations.</param>
    /// <param name="name">The name.</param>
    /// <param name="controllerName">Name of the controller.</param>
    /// <param name="moduleName">Name of the module.</param>
    /// <param name="cacheKey">The cache key.</param>
    /// <param name="searchedLocations">The searched locations.</param>
    /// <returns></returns>
    private string GetPathFromGeneralName(ControllerContext controllerContext, string[] locations, string name,
        string controllerName, string moduleName, string cacheKey, ref string[] searchedLocations)
    {
      string virtualPath = string.Empty;
      searchedLocations = new string[locations.Length];
      for (int i = 0; i < locations.Length; i++)
      {
        string str2 = string.Format(CultureInfo.InvariantCulture, locations[i], new object[] { name, controllerName, moduleName });
        if (this.FileExists(controllerContext, str2))
        {
          searchedLocations = _emptyLocations;
          virtualPath = str2;
          this.ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, virtualPath);
          return virtualPath;
        }
        searchedLocations[i] = str2;
      }
      return virtualPath;
    }

    /// <summary>
    /// Gets the name of the path from specific.
    /// </summary>
    /// <param name="controllerContext">The controller context.</param>
    /// <param name="name">The name.</param>
    /// <param name="cacheKey">The cache key.</param>
    /// <param name="searchedLocations">The searched locations.</param>
    /// <returns></returns>
    private string GetPathFromSpecificName(ControllerContext controllerContext, string name, string cacheKey, ref string[] searchedLocations)
    {
      string virtualPath = name;
      if (!this.FileExists(controllerContext, name))
      {
        virtualPath = string.Empty;
        searchedLocations = new string[] { name };
      }
      this.ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, virtualPath);
      return virtualPath;
    }

    #endregion

  }
}
