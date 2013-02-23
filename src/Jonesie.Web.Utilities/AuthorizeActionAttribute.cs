using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using Jonesie.Web.Contracts.Core;
using Jonesie.Web.Contracts.Data;
using Jonesie.Web.Entities.Data;
using System.Security.Principal;

namespace Jonesie.Web.Utilities
{
  /// <summary>
  /// Filter all requests for authorization based on the role maps
  /// </summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
  public class AuthorizeActionAttribute : AuthorizeAttribute
  {
    public ISecurityRepository SecurityRepository { get; set; }

    protected static AuthorizeActionAttribute _instance;

    protected static Dictionary<string, List<string>> _roleSPMapping = null;

    /// <summary>
    /// Simple constructor to save the instance statically so the cache can be cleared
    /// </summary>
    public AuthorizeActionAttribute()
      : base()
    {
      _instance = this;
      SecurityRepository = DependencyResolver.Current.GetService<ISecurityRepository>();
    }

    /// <summary>
    /// Gets the role to service point mappings.
    /// </summary>
    /// <value>
    /// The role SP mappings.
    /// </value>
    protected static Dictionary<string, List<string>> RoleSPMapping
    {
      get
      {
        if (_roleSPMapping == null)
        {
          var mappings = _instance.SecurityRepository.GetRoleActionMaps();

          // these should come from a data layer
          _roleSPMapping = new Dictionary<string, List<string>>();

          foreach (var mapping in mappings.Items)
          {
            if (!_roleSPMapping.ContainsKey(mapping.RoleName))
            {
              _roleSPMapping.Add(mapping.RoleName, new List<string>());
            }

            _roleSPMapping[mapping.RoleName].Add(mapping.Path);
          }
        }

        return _roleSPMapping;
      }
    }

    /// <summary>
    /// Clears the cache.
    /// </summary>
    public static void ClearCache()
    {
      _roleSPMapping = null;
    }

    /// <summary>
    /// Called when a process requests authorization.
    /// </summary>
    /// <param name="filterContext">The filter context, which encapsulates information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute"/>.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="filterContext"/> parameter is null.</exception>
    public override void OnAuthorization(AuthorizationContext filterContext)
    {
      //// update the user profile every time they request something
      //if (HttpContext.Current.User.Identity.IsAuthenticated)
      //{
      //  UserProfileService.UpdateUserActivity(HttpContext.Current.User.Identity.Name);
      //}

      if (filterContext == null)
      {
        throw new ArgumentNullException("filterContext");
      }

      // get the current controller and action 
      var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
      var action = filterContext.ActionDescriptor.ActionName;

      // see if the current action wants to override the action name for the menu path
      var aba = filterContext.ActionDescriptor.GetCustomAttributes(typeof(ActionBreadcrumbAttribute), false).FirstOrDefault();
      if (aba != null)
      {
        action = ((ActionBreadcrumbAttribute)aba).Action;
      }

      // add the current controller and action path to the RouteData and ViewBag 
      filterContext.RouteData.DataTokens.Add("menu_controller", controller);
      filterContext.RouteData.DataTokens.Add("menu_action", action);
      filterContext.Controller.ViewBag.MenuPath = controller + "_" + action;  // not sure if this is needed yet...

      // allow anonymouse stuff straight through
      if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
      {
        base.OnAuthorization(filterContext);
        return;
      }

      if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
      {
        // auth failed, redirect to login page
        filterContext.Result = new HttpUnauthorizedResult();
        return;
      }

      if (UserHasServicePoint(filterContext.HttpContext.User, getServicePointName(filterContext)))
      {
        base.OnAuthorization(filterContext);
        return;
      }

      //filterContext.Controller.TempData.Add("RedirectReason", "You are not authorized to access this page.");
      filterContext.Result = new RedirectResult("~/Error/Unauthorized");
    }

    /// <summary>
    /// Does a user have the require service point permission?
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="servicePoint">The service point.</param>
    /// <returns></returns>
    public static bool UserHasServicePoint(IPrincipal user, string servicePoint)
    {

      // get the roles that allow this service point
      List<string> roles = getRolesForServicePoint(servicePoint);

      foreach (var role in roles)
      {
        try
        {
          if (role == "*" || user.IsInRole(role))
          {
            return true;
          }
        }
        catch
        {
        }
      }

      return false;
    }

    /// <summary>
    /// Gets the roles for service point.
    /// </summary>
    /// <param name="servicePoint">The service point.</param>
    /// <returns></returns>
    private static List<string> getRolesForServicePoint(string servicePoint) 
    {
      var roles = new List<string>();

      foreach (var role in RoleSPMapping.Keys)
      {
        foreach (var sp in RoleSPMapping[role])
        {
          var rx = new Regex(sp);

          if (role == "*" || rx.Matches(servicePoint).Count > 0)
          {
            roles.Add(role);
            break;
          }
        }
      }

      return roles;
    }

    /// <summary>
    /// Gets the name of the role. Theoretical construct that illustrates a problem with the
    /// area name. RouteData is apparently insecure, but the area name is available there.
    /// </summary>
    /// <param name="filterContext">The filter context.</param>
    /// <param name="version">The version.</param>
    /// <returns></returns>
    string getServicePointName(AuthorizationContext filterContext)
    {
      // var verb = filterContext.HttpContext.Request.HttpMethod;

      // recommended way to access controller and action names
      var controllerFullName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.FullName;
      var actionName = filterContext.ActionDescriptor.ActionName;

      return String.Format("{0}.{1}", controllerFullName, actionName); //-{2} //, verb);
    }
  }
}