using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jonesie.Web.Entities.Data;

namespace Jonesie.Web.Contracts.Data
{
  /// <summary>
  /// A contract for all security data
  /// </summary>
  [InheritedExport(typeof(ISecurityRepository))]
  public interface ISecurityRepository
  {

    #region roles

    /// <summary>
    /// Gets the role .
    /// </summary>
    /// <returns></returns>
    DataSet<Role> GetRoles(int page = 1, int pageSize = 100, string sortColumn = "RoleName", bool descending = false, string filter = null);

    /// <summary>
    /// Gets a role.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns></returns>
    Role GetRole(int id);

    /// <summary>
    /// Updates the role.
    /// </summary>
    /// <param name="role">The role.</param>
    void UpdateRole(Role role);

    /// <summary>
    /// Deletes the role
    /// </summary>
    /// <param name="id">The id.</param>
    void DeleteRole(int id);


    #endregion

    #region role action map
    /// <summary>
    /// Gets the role action maps.
    /// </summary>
    /// <returns></returns>
    DataSet<RoleActionMap> GetRoleActionMaps(int page = 1, int pageSize = 100, string sortColumn = "Role", bool descending = false, string filter = null);

    /// <summary>
    /// Gets a role action map.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns></returns>
    RoleActionMap GetRoleActionMap(int id);

    /// <summary>
    /// Updates the role action map.
    /// </summary>
    /// <param name="ram">The ram.</param>
    void UpdateRoleActionMap(RoleActionMap ram);

    /// <summary>
    /// Deletes the role action map.
    /// </summary>
    /// <param name="id">The id.</param>
    void DeleteRoleActionMap(int id);

    #endregion

    #region user

    /// <summary>
    /// Gets the users.
    /// </summary>
    /// <param name="page">The page.</param>
    /// <param name="pageSize">Size of the page.</param>
    /// <param name="sortColumn">The sort column.</param>
    /// <param name="descending">if set to <c>true</c> [descending].</param>
    /// <param name="filter">The filter.</param>
    /// <returns></returns>
    DataSet<UserProfile> GetUsers(int page = 1, int pageSize = 100, string sortColumn = "UserName", bool descending = false, string filter = null);

    /// <summary>
    /// Gets a user.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns></returns>
    UserProfile GetUser(int id);


    /// <summary>
    /// Updates the user or creates a new one
    /// </summary>
    /// <param name="siteUser">The site user.</param>
    void UpdateUser(UserProfile siteUser);

    /// <summary>
    /// Deletes a user.
    /// </summary>
    /// <param name="id">The user id.</param>
    void DeleteUser(int id);

    #endregion

  }
}
