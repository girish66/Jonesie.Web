using Jonesie.Web.Contracts.Core;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jonesie.Web.Utilities
{
  /// <summary>
  /// A factory for controller using a MEF container
  /// </summary>
  public class MEFControllerFactory : DefaultControllerFactory
  {
    private readonly CompositionContainer _compositionContainer;

    /// <summary>
    /// Initializes a new instance of the <see cref="MEFControllerFactory" /> class.
    /// </summary>
    /// <param name="compositionContainer">The composition container.</param>
    public MEFControllerFactory(CompositionContainer compositionContainer)
    {
      _compositionContainer = compositionContainer;
    }

    /// <summary>
    /// Retrieves the controller type for the specified name and request context.
    /// </summary>
    /// <param name="requestContext">The context of the HTTP request, which includes the HTTP context and route data.</param>
    /// <param name="controllerName">The name of the controller.</param>
    /// <returns>
    /// The controller type.
    /// </returns>
    protected override Type GetControllerType(System.Web.Routing.RequestContext requestContext, string controllerName)
    {
      var modules = _compositionContainer.GetExports<IViewModule>();

      if (modules.Count() > 0)
      {
        foreach (var module in modules)
        {
          foreach (var controller in module.Value.Controllers)
          {
            if (controller.Name == controllerName + "Controller")
            {
              return controller;
            }
          }
        }
      }

      return base.GetControllerType(requestContext, controllerName);
    }

    /// <summary>
    /// Retrieves the controller instance for the specified request context and controller type.
    /// </summary>
    /// <param name="requestContext">The context of the HTTP request, which includes the HTTP context and route data.</param>
    /// <param name="controllerType">The type of the controller.</param>
    /// <returns>
    /// The controller instance.
    /// </returns>
    protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
    {
      IController result = null;

      if (controllerType != null)
      {
        var export = _compositionContainer.GetExports(controllerType, null, null).SingleOrDefault();

        if (null != export)
        {
          result = export.Value as IController;
        }
        else
        {
          result = base.GetControllerInstance(requestContext, controllerType);
          _compositionContainer.ComposeParts(result);
        }
      }

      if (result == null)
      {
        throw new HttpException(404, "Doofuss!");
      }

      return result;
    }
  }

}