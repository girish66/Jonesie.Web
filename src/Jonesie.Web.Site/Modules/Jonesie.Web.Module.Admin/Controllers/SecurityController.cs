using Jonesie.Web.Contracts.Core;
using Jonesie.Web.Contracts.Data;
using Jonesie.Web.Entities.Core;
using Jonesie.Web.Entities.Data;
using Jonesie.Web.Module.Admin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jonesie.Web.Module.Admin.Controllers
{
  /// <summary>
  /// Controller for security related pages
  /// </summary>
  [Export]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class SecurityController : Controller
  {

    #region members
    ISettings _settings;
    ISecurityRepository _securityRepository;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="SecurityController" /> class.
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <param name="securityRepository">The security repository.</param>
    [ImportingConstructor]
    public SecurityController(ISettings settings, ISecurityRepository securityRepository)
    {
      _settings = settings;
      _securityRepository = securityRepository;
    }


    /// <summary>
    /// Get the top index view.  Use Ajax for the rest
    /// </summary>
    /// <returns></returns>
    public ActionResult Index()
    {
      return View(new CurrentSessionsViewModel());
    }

    #region session stuff

    /// <summary>
    /// Gets the sessions view.
    /// </summary>
    /// <returns></returns>
    public ActionResult GetSessionsView()
    {
      return PartialView("CurrentSessions", new CurrentSessionsViewModel { Controller = "Security" });
    }

    /// <summary>
    /// Gets the current sessions.
    /// </summary>
    /// <param name="page">The page.</param>
    /// <param name="pageSize">Size of the page.</param>
    /// <param name="sortColumn">The sort column.</param>
    /// <param name="orderDescending">if set to <c>true</c> [order descending].</param>
    /// <param name="filter">The filter.</param>
    /// <returns></returns>
    public ActionResult GetSessions(int page, int pageSize = 100, string sortColumn = "", bool orderDescending = false, string filter = "")
    {
      int firstItem = (page - 1) * pageSize;
      return Json(new DataSet<UserSession>(_settings.Sessions.Skip(firstItem).Take(pageSize), _settings.Sessions.Count, page: page, pageSize: pageSize), JsonRequestBehavior.AllowGet);

    }

    #endregion

    #region user stuff

    /// <summary>
    /// Gets the users view.
    /// </summary>
    /// <returns></returns>
    public ActionResult GetUsersView()
    {
      return PartialView("Users", new UsersViewModel { Controller = "Security" });
    }

    /// <summary>
    /// Gets the users.
    /// </summary>
    /// <param name="page">The page.</param>
    /// <param name="pageSize">Size of the page.</param>
    /// <param name="sortColumn">The sort column.</param>
    /// <param name="orderDescending">if set to <c>true</c> [order descending].</param>
    /// <param name="filter">The filter.</param>
    /// <returns></returns>
    public ActionResult GetUsers(int page, int pageSize = 100, string sortColumn = "UserName", bool orderDescending = false, string filter = "")
    {
      var users = _securityRepository.GetUsers(page: page, pageSize: pageSize, sortColumn: sortColumn, descending: orderDescending, filter: filter);
      return Json(users, JsonRequestBehavior.AllowGet);
    }

    /// <summary>
    /// News the user.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public ActionResult NewUser()
    {
      var roles = _securityRepository.GetRoles(0);
      return PartialView("User", new UserViewModel(new UserProfile(), roles.Items) { Controller = "Security", IsNew = true });
    }

    /// <summary>
    /// Edits the user.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns></returns>
    [HttpGet]
    public ActionResult EditUser(int id)
    {
      var roles = _securityRepository.GetRoles(0);
      return PartialView("User", new UserViewModel(_securityRepository.GetUser(id), roles.Items) { Controller = "Security", IsNew = false });
    }

    /// <summary>
    /// Deletes the user.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns></returns>
    [HttpGet]
    public ActionResult DeleteUser(int id)
    {
      var roles = _securityRepository.GetRoles(0);
      return PartialView("User", new UserViewModel(_securityRepository.GetUser(id), roles.Items) { Controller = "Security", IsNew = false, Delete = true });
    }

    /// <summary>
    /// Saves the user.
    /// </summary>
    /// <param name="uvm">The uvm.</param>
    /// <returns></returns>
    [HttpPost]
    public ActionResult SaveUser(UserViewModel uvm)
    {

      if (uvm.Delete)
      {
        _securityRepository.DeleteUser(uvm.User.UserId);
        return Json(new { Success = true }, JsonRequestBehavior.DenyGet);
      }
      else
        if (ModelState.IsValid)
        {
          try
          {
            var allRoles = _securityRepository.GetRoles(0);
            uvm.User.Roles = allRoles.Items.Where(r => uvm.SelectedRoles.Contains(r.RoleId)).ToList();

            _securityRepository.UpdateUser(uvm.User);
            Response.StatusCode = 200;
            return Json(new { Success = true }, JsonRequestBehavior.DenyGet);
          }
          catch (Exception ex)
          {
            Response.StatusCode = 400;
            return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.DenyGet);            
          }
        }
        else
        {
          ModelState.AddModelError("", "The user record is not valid.");
          Response.StatusCode = 400;
          return Json(new { Success = false, Message = "The user record is not valid." }, JsonRequestBehavior.DenyGet);
        }
    }

    #endregion

    #region role stuff

    /// <summary>
    /// Gets the roles view.
    /// </summary>
    /// <returns></returns>
    public ActionResult GetRolesView()
    {
      return PartialView("Roles", new RolesViewModel { Controller = "Security" });
    }

    /// <summary>
    /// Gets the role.
    /// </summary>
    /// <param name="page">The page.</param>
    /// <param name="pageSize">Size of the page.</param>
    /// <param name="sortColumn">The sort column.</param>
    /// <param name="orderDescending">if set to <c>true</c> [order descending].</param>
    /// <param name="filter">The filter.</param>
    /// <returns></returns>
    public ActionResult GetRoles(int page, int pageSize = 100, string sortColumn = "RoleName", bool orderDescending = false, string filter = "")
    {
      var roles = _securityRepository.GetRoles(page: page, pageSize: pageSize, sortColumn: sortColumn, descending: orderDescending, filter: filter);
      return Json(roles, JsonRequestBehavior.AllowGet);
    }

    /// <summary>
    /// Create a new Role
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public ActionResult NewRole()
    {
      return PartialView("Role", new RoleViewModel() { Controller = "Security", IsNew = true, Role = new Role() });
    }

    /// <summary>
    /// Edit a role.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns></returns>
    [HttpGet]
    public ActionResult EditRole(int id)
    {
      return PartialView("Role", new RoleViewModel() { Controller = "Security", IsNew = true, Role = _securityRepository.GetRole(id) });
    }

    /// <summary>
    /// Delete a role action map.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns></returns>
    [HttpGet]
    public ActionResult DeleteRole(int id)
    {
      return PartialView("Role", new RoleViewModel() { Controller = "Security", IsNew = false, Role = _securityRepository.GetRole(id), Delete = true });
    }

    /// <summary>
    /// Save a role.
    /// </summary>
    /// <param name="rvm">The view model.</param>
    /// <returns></returns>
    [HttpPost]
    public ActionResult SaveRole(RoleViewModel rvm)
    {

      if (rvm.Delete)
      {
        _securityRepository.DeleteRole(rvm.Role.RoleId);
        return Json(new { Success = true }, JsonRequestBehavior.DenyGet);
      }
      else
        if (ModelState.IsValid)
        {
          try
          {
            _securityRepository.UpdateRole(rvm.Role);
            Response.StatusCode = 200;
            return Json(new { Success = true }, JsonRequestBehavior.DenyGet);
          }
          catch (Exception ex)
          {
            Response.StatusCode = 400;
            return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.DenyGet);
          }
        }
        else
        {
          ModelState.AddModelError("", "The role record is not valid.");
          Response.StatusCode = 400;
          return Json(new { Success = false, Message = "The role record is not valid." }, JsonRequestBehavior.DenyGet);
        }
    }

    #endregion

    #region roleactionmap stuff

    /// <summary>
    /// Gets the roles view.
    /// </summary>
    /// <returns></returns>
    public ActionResult GetRoleActionMapsView()
    {
      return PartialView("RoleActionMaps", new RoleActionMapsViewModel { Controller = "Security" });
    }

    /// <summary>
    /// Gets the role action maps.
    /// </summary>
    /// <param name="page">The page.</param>
    /// <param name="pageSize">Size of the page.</param>
    /// <param name="sortColumn">The sort column.</param>
    /// <param name="orderDescending">if set to <c>true</c> [order descending].</param>
    /// <param name="filter">The filter.</param>
    /// <returns></returns>
    public ActionResult GetRoleActionMaps(int page, int pageSize = 100, string sortColumn = "UserName", bool orderDescending = false, string filter = "")
    {
      var rams = _securityRepository.GetRoleActionMaps(page: page, pageSize: pageSize, sortColumn: sortColumn, descending: orderDescending, filter: filter);
      return Json(rams, JsonRequestBehavior.AllowGet);
    }

    /// <summary>
    /// Create a new RoleActionMap.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public ActionResult NewRoleActionMap()
    {
      return PartialView("RoleActionMap", new RoleActionMapViewModel(new RoleActionMap() , _securityRepository.GetRoles()) { Controller = "Security", IsNew = true});
    }

    /// <summary>
    /// Edit a role action mapo.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns></returns>
    [HttpGet]
    public ActionResult EditRoleActionMap(int id)
    {
      return PartialView("RoleActionMap", new RoleActionMapViewModel(_securityRepository.GetRoleActionMap(id), _securityRepository.GetRoles()) { Controller = "Security", IsNew = true });
    }

    /// <summary>
    /// Delete a role action map.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns></returns>
    [HttpGet]
    public ActionResult DeleteRoleActionMap(int id)
    {
      return PartialView("RoleActionMap", new RoleActionMapViewModel(_securityRepository.GetRoleActionMap(id), null) { Controller = "Security", IsNew = false, Delete = true });
    }

    /// <summary>
    /// Save a role action map.
    /// </summary>
    /// <param name="rvm">The view model.</param>
    /// <returns></returns>
    [HttpPost]
    public ActionResult SaveRoleActionMap(RoleActionMapViewModel rvm)
    {

      if (rvm.Delete)
      {
        _securityRepository.DeleteRoleActionMap(rvm.RoleActionMap.RoleActionMapId);
        return Json(new { Success = true }, JsonRequestBehavior.DenyGet);
      }
      else
        if (ModelState.IsValid)
        {
          try
          {
            _securityRepository.UpdateRoleActionMap(rvm.RoleActionMap);
            Response.StatusCode = 200;
            return Json(new { Success = true }, JsonRequestBehavior.DenyGet);
          }
          catch (Exception ex)
          {
            Response.StatusCode = 400;
            return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.DenyGet);
          }
        }
        else
        {
          ModelState.AddModelError("", "The role authorization record is not valid.");
          Response.StatusCode = 400;
          return Json(new { Success = false, Message = "The role authorization record is not valid." }, JsonRequestBehavior.DenyGet);
        }
    }


    #endregion
  }
}
