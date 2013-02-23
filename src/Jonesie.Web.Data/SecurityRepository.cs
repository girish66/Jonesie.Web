using Dapper;
using Jonesie.Web.Common;
using Jonesie.Web.Contracts.Core;
using Jonesie.Web.Contracts.Data;
using Jonesie.Web.Entities.Data;
using System.ComponentModel.Composition;
using System.Linq;

namespace Jonesie.Web.Data
{
  /// <summary>
  /// A repository for security related data
  /// </summary>
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class SecurityRepository : BaseRepository, ISecurityRepository
  {

    const string CACHE_KEY = "Jonesie_Web_Data_SecurityRepository_{0}";

    #region roles

    /// <summary>
    /// Gets the role .
    /// </summary>
    /// <returns></returns>
    public DataSet<Role> GetRoles(int page = 1, int pageSize = 100, string sortColumn = "RoleName", bool descending = false, string filter = null)
    {
      // make sure the sortColumn is valid (SQL Injection Check)
      if (!Role.ColumnNames.Contains(sortColumn.ToLower()))
        sortColumn = null;
      if (string.IsNullOrEmpty(sortColumn))
        sortColumn = "RoleName";

      return
        PerformNonCachedQuery(CACHE_KEY.FormatWith("Roles_{0}_{1}_{2}_{3}_{4}".FormatWith(page, pageSize, sortColumn, descending, filter)),
        (con) =>
        {

          var order = sortColumn + (descending ? " desc" : string.Empty);
          var where = string.Empty;

          if (!string.IsNullOrWhiteSpace(filter))
          {
            where = " and RoleName like @RoleName";
            filter = "%" + filter.Trim() + "%";
          }

          using (var multi = con.QueryMultiple(Properties.Resources.Role_List_Get.FormatWith(where, order),
                                                          new
                                                          {
                                                            RoleName = filter,
                                                            FirstItem = GetFirstItemForPage(page, pageSize),
                                                            PageSize = pageSize
                                                          }))
          {
            var totalCount = multi.Read<int>().SingleOrDefault();
            var items = multi.Read<Role>();

            return
              new Jonesie.Web.Entities.Data.DataSet<Role>(items.ToList(), totalCount, sortColumn: sortColumn, orderDescending: descending, page: page, pageSize: pageSize);
          }
        });

    }

    /// <summary>
    /// Gets a role.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns></returns>
    public Role GetRole(int id)
    {
      return PerformCachedQuery<Role>(CACHE_KEY.FormatWith("Role_{0}".FormatWith(id)),
           (con) =>
           {
             var multi = con.QueryMultiple(Properties.Resources.Role_Get,
                     new { RoleId = id });

             return multi.Read<Role, RoleActionMap, Role>((role, ram) =>
             {
               if (ram != null)
                 role.RoleActionMaps.Add(ram);
               return role;
             },
             splitOn: "RoleId").FirstOrDefault();

           });
    }

    /// <summary>
    /// Updates the role.
    /// </summary>
    /// <param name="role">The role.</param>
    public void UpdateRole(Role role)
    {
      ExecuteQuery(CACHE_KEY.FormatWith("Role_Update_{0}".FormatWith(role.RoleId)),
        (con) =>
        {
          con.Execute(
            Properties.Resources.Role_Update, role);

          Cache.RemoveFromCache(CACHE_KEY.FormatWith("Role.*"));
        });
    }

    /// <summary>
    /// Deletes the role
    /// </summary>
    /// <param name="id">The id.</param>
    public void DeleteRole(int id)
    {
      ExecuteQuery(CACHE_KEY.FormatWith("Role_Delete_{0}".FormatWith(id)),
        (con) =>
        {
          con.Execute(
            Properties.Resources.Role_Delete, new { RoleId = id });

          Cache.RemoveFromCache(CACHE_KEY.FormatWith("Role.*"));
        });
    }

    #endregion

    #region role action maps

    /// <summary>
    /// Gets the role action maps.
    /// </summary>
    /// <returns></returns>
    public Entities.Data.DataSet<RoleActionMap> GetRoleActionMaps(int page = 1, int pageSize = 100, string sortColumn = "Path", bool descending = false, string filter = null)
    {
      return
        PerformNonCachedQuery(CACHE_KEY.FormatWith("RoleActionMaps_{0}_{1}_{2}_{3}_{4}".FormatWith(page, pageSize, sortColumn, descending, filter)),
          (con) =>
          {
            // make sure the sortColumn is valid (SQL Injection Check)
            if (!RoleActionMap.ColumnNames.Contains(sortColumn.ToLower()))
              sortColumn = null;
            if (string.IsNullOrEmpty(sortColumn))
              sortColumn = "Path";

            var order = sortColumn + (descending ? " desc" : string.Empty);

            var where = string.Empty;
            if (filter != null)
            {
              where = " and Path like @Path";
              filter = "%" + filter.Trim() + "%";
            }

            using (var multi = con.QueryMultiple(Properties.Resources.RoleActionMap_List_Get.FormatWith(where, order),
                                                            new
                                                            {
                                                              Path = filter,
                                                              FirstItem = GetFirstItemForPage(page, pageSize),
                                                              PageSize = pageSize
                                                            }))
            {
              var totalCount = multi.Read<int>().SingleOrDefault();
              var items = multi.Read<RoleActionMap>();

              return
                new Jonesie.Web.Entities.Data.DataSet<RoleActionMap>(items.ToList(), totalCount, sortColumn: sortColumn, orderDescending: descending, page: page, pageSize: pageSize);
            }
          });
    }

    /// <summary>
    /// Gets a role action map.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns></returns>
    public Entities.Data.RoleActionMap GetRoleActionMap(int id)
    {
      return PerformCachedQuery<RoleActionMap>(CACHE_KEY.FormatWith("RoleActionMap_{0}".FormatWith(id)),
          (con) =>
          {
            return con.Query<RoleActionMap>(
                    Properties.Resources.RoleActionMap_Get, new { RoleActionMapId = id }).FirstOrDefault();
          });
    }

    /// <summary>
    /// Updates the role action map.
    /// </summary>
    /// <param name="ram">The ram.</param>
    public void UpdateRoleActionMap(RoleActionMap ram)
    {
      ExecuteQuery(CACHE_KEY.FormatWith("RoleActionMap_Update_{0}".FormatWith(ram.RoleActionMapId)),
          (con) =>
          {
            con.Execute(Properties.Resources.RoleActionMap_Update, param: ram);
            Cache.RemoveFromCache(CACHE_KEY.FormatWith("RoleActionMap.*"));
          });
    }

    /// <summary>
    /// Deletes the role action map.
    /// </summary>
    /// <param name="id">The id.</param>
    public void DeleteRoleActionMap(int id)
    {
      ExecuteQuery(CACHE_KEY.FormatWith("RoleActionMap_Delete_{0}".FormatWith(id)),
          (con) =>
          {
            con.Execute(Properties.Resources.RoleActionMap_Delete, new { RoleActionMapId = id });
            Cache.RemoveFromCache(CACHE_KEY.FormatWith("RoleActionMap.*"));
          });
    }

    #endregion

    #region users

    /// <summary>
    /// Gets the users.
    /// </summary>
    /// <param name="page">The page.</param>
    /// <param name="pageSize">Size of the page.</param>
    /// <param name="sortColumn">The sort column.</param>
    /// <param name="descending">if set to <c>true</c> [descending].</param>
    /// <param name="filter">The filter.</param>
    /// <returns></returns>
    public DataSet<UserProfile> GetUsers(int page = 1, int pageSize = 100, string sortColumn = "UserName", bool descending = false, string filter = null)
    {
      return
          PerformNonCachedQuery(CACHE_KEY.FormatWith("Users_{0}_{1}_{2}_{3}_{4}".FormatWith(page, pageSize, sortColumn, descending, filter)),
          (con) =>
          {
            if (string.IsNullOrEmpty(sortColumn))
              sortColumn = "UserName";

            var order = sortColumn + (descending ? " desc" : string.Empty);

            if (filter != null)
            {
              filter = "and UserName like '%" + filter + "%'";
            }
            else
            {
              filter = string.Empty;
            }

            using (var multi = con.QueryMultiple(Properties.Resources.Users_Get.FormatWith(filter, order),
                                                            new
                                                            {
                                                              FirstItem = GetFirstItemForPage(page, pageSize),
                                                              PageSize = pageSize
                                                            }))
            {
              var totalCount = multi.Read<int>().SingleOrDefault();
              var items = multi.Read<UserProfile>();

              return
                new DataSet<UserProfile>(items, totalCount, sortColumn: sortColumn, orderDescending: descending, page: page, pageSize: pageSize);
            }
          });
    }

    /// <summary>
    /// Gets a user.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns></returns>
    public UserProfile GetUser(int id)
    {
      return PerformNonCachedQuery(CACHE_KEY.FormatWith("User_{0}".FormatWith(id)),
        (con) =>
        {
          var multi = con.QueryMultiple(
            Properties.Resources.User_Get,
            new { UserId = id });

          var result = multi.Read<UserProfile>().SingleOrDefault();
          result.Roles = multi.Read<Role>().ToList();

          return result;
        });
    }

    /// <summary>
    /// Updates the user or creates a new one
    /// </summary>
    /// <param name="siteUser">The site user.</param>
    public void UpdateUser(UserProfile siteUser)
    {
      ExecuteQuery(CACHE_KEY.FormatWith("User_Update_{0}".FormatWith(siteUser.UserId)),
        (con) =>
        {
          con.Execute(
            Properties.Resources.User_Update, siteUser);

          con.Execute(
              Properties.Resources.UserRole_Clear,
              new { UserId = siteUser.UserId });

          siteUser.Roles.ForEach(role =>
            con.Execute(
              Properties.Resources.UserRole_Update,
              new { UserId = siteUser.UserId, RoleId = role.RoleId })
          );

          Cache.RemoveFromCache(CACHE_KEY.FormatWith("User.*"));
        });
    }

    /// <summary>
    /// Deletes a user.
    /// </summary>
    /// <param name="userId">The user id.</param>
    public void DeleteUser(int userId)
    {
      ExecuteQuery(CACHE_KEY.FormatWith("User_Delete_{0}".FormatWith(userId)),
        (con) =>
        {
          con.Execute(
            Properties.Resources.User_Delete, new { UserId = userId });

          Cache.RemoveFromCache(CACHE_KEY.FormatWith("User.*"));
        });
    }


    #endregion
  }
}
