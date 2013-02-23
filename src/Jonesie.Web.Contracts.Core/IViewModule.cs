using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Web.Optimization;
using System.Web.Routing;

namespace Jonesie.Web.Contracts.Core
{
  /// <summary>
  /// A contract for all view type modules
  /// </summary>
  [InheritedExport(typeof(IViewModule))]
  public interface IViewModule : IModule
  {
    /// <summary>
    /// Gets the controllers.
    /// </summary>
    /// <value>
    /// The controllers.
    /// </value>
    Type[] Controllers { get; }


    /// <summary>
    /// Registers the css and script bundles for this view module
    /// </summary>
    /// <param name="bundles">The bundles.</param>
    void RegisterBundles(BundleCollection bundles);

    /// <summary>
    /// Registers the routes.
    /// </summary>
    /// <param name="routes">The routes.</param>
    void RegisterRoutes(RouteCollection routes);
  }
}
