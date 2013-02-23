using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;

namespace Jonesie.Web.Utilities
{
  /// <summary>
  /// A dependency resolver for MVC and Web API
  /// </summary>
  public class MEFDependencyResolver : System.Web.Http.Dependencies.IDependencyResolver, System.Web.Mvc.IDependencyResolver
  {
    private readonly CompositionContainer _container;

    /// <summary>
    /// Initializes a new instance of the <see cref="MEFDependencyResolver" /> class.
    /// </summary>
    /// <param name="container">The container.</param>
    public MEFDependencyResolver(CompositionContainer container)
    {
      _container = container;
    }

    /// <summary>
    /// Starts a resolution scope.
    /// </summary>
    /// <returns>
    /// The dependency scope.
    /// </returns>
    public IDependencyScope BeginScope()
    {
      return this;
    }

    /// <summary>
    /// Retrieves a service from the scope.
    /// </summary>
    /// <param name="serviceType">The service to be retrieved.</param>
    /// <returns>
    /// The retrieved service.
    /// </returns>
    public object GetService(Type serviceType)
    {
      var export = _container.GetExports(serviceType, null, null).SingleOrDefault();

      return null != export ? export.Value : null;
    }

    /// <summary>
    /// Retrieves a collection of services from the scope.
    /// </summary>
    /// <param name="serviceType">The collection of services to be retrieved.</param>
    /// <returns>
    /// The retrieved collection of services.
    /// </returns>
    public IEnumerable<object> GetServices(Type serviceType)
    {
      var exports = _container.GetExports(serviceType, null, null);

      if (exports.Any())
      {
        return exports.Select(x => x.Value).ToArray();
      }

      return new object[] { };
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      ;
    }
  }
}