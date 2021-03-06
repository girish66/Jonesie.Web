﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Jonesie.Web.Data.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Jonesie.Web.Data.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to delete RoleActionMap where RoleId = @RoleId
        ///
        ///delete webpages_Roles where RoleId = @RoleId.
        /// </summary>
        internal static string Role_Delete {
            get {
                return ResourceManager.GetString("Role_Delete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select
        ///    r.RoleId, r.[RoleName],
        ///    ram.RoleId, ram.RoleActionMapId, ram.[Path], ram.row_version
        ///from
        ///    webpages_Roles r
        ///    left join RoleActionMap ram on ram.RoleId = r.RoleId
        ///where
        ///    r.RoleId = @RoleId.
        /// </summary>
        internal static string Role_Get {
            get {
                return ResourceManager.GetString("Role_Get", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select
        ///  count(*) total_rows
        ///from
        ///  webpages_Roles
        ///where 
        ///  1=1 {0};
        ///
        ///select
        ///    RoleId, [RoleName]
        ///from
        ///    webpages_Roles 
        ///where
        ///    1 = 1 {0}
        ///order by
        ///  {1}
        ///offset @FirstItem - 1 rows
        ///fetch next @PageSize rows only;.
        /// </summary>
        internal static string Role_List_Get {
            get {
                return ResourceManager.GetString("Role_List_Get", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to if exists (select 1 from  webpages_Roles where RoleId = @RoleId)
        ///begin
        ///    update webpages_Roles set RoleName = @RoleName where RoleId = @RoleId
        ///end
        ///else
        ///begin
        ///    insert webpages_Roles (RoleName)
        ///    values (@RoleName)
        ///end.
        /// </summary>
        internal static string Role_Update {
            get {
                return ResourceManager.GetString("Role_Update", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to delete RoleActionMap where RoleActionMapId = @RoleActionMapId.
        /// </summary>
        internal static string RoleActionMap_Delete {
            get {
                return ResourceManager.GetString("RoleActionMap_Delete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select
        ///    ram.RoleActionMapId, ram.[RoleId], ram.[Path], ram.row_version, r.RoleName
        ///from
        ///    RoleActionMap ram
        ///    inner join webpages_Roles r on r.RoleId = ram.RoleId
        ///where
        ///    RoleActionMapId = @RoleActionMapId.
        /// </summary>
        internal static string RoleActionMap_Get {
            get {
                return ResourceManager.GetString("RoleActionMap_Get", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select
        ///  count(*) total_rows
        ///from
        ///  RoleActionMap
        ///where 
        ///  1=1 {0};
        ///
        ///select
        ///    ram.RoleActionMapId, ram.[RoleId], ram.[Path], ram.row_version, r.RoleName
        ///from
        ///    RoleActionMap ram
        ///    inner join webpages_Roles r on r.RoleId = ram.RoleId
        ///where
        ///    1 = 1 {0}
        ///order by
        ///  {1}
        ///offset @FirstItem - 1 rows
        ///fetch next @PageSize rows only;.
        /// </summary>
        internal static string RoleActionMap_List_Get {
            get {
                return ResourceManager.GetString("RoleActionMap_List_Get", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to if exists (select 1 from RoleActionMap where RoleActionMapId = @RoleActionMapId)
        ///begin
        ///    update RoleActionMap set RoleId = @RoleId, Path = @Path where RoleActionMapId = @RoleActionMapId
        ///end
        ///else
        ///begin
        ///    insert RoleActionMap (RoleId, Path)
        ///    values (@RoleId, @Path)
        ///end.
        /// </summary>
        internal static string RoleActionMap_Update {
            get {
                return ResourceManager.GetString("RoleActionMap_Update", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /*
        ///Deployment script for Jonesie.Web
        ///
        ///This code was generated by a tool.
        ///Changes to this file may cause incorrect behavior and will be lost if
        ///the code is regenerated.
        ///*/
        ///
        ///GO
        ///SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;
        ///
        ///SET NUMERIC_ROUNDABORT OFF;
        ///
        ///
        ///GO
        ///:setvar DatabaseName &quot;Jonesie.Web&quot;
        ///:setvar DefaultFilePrefix &quot;Jonesie.Web&quot;
        ///:setvar DefaultDataPath &quot;&quot;
        ///:setvar DefaultLogPath &quot;&quot;
        ///
        ///GO
        ///:on error exit
        ///GO
        ////*
        ///Detect SQLCMD mode and [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Schema_Base_1 {
            get {
                return ResourceManager.GetString("Schema_Base_1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IF (DB_ID(@DatabaseName) IS NOT NULL) 
        ///BEGIN
        ///  SELECT 1
        ///END
        ///ELSE
        ///BEGIN
        ///  SELECT 0
        ///END
        ///.
        /// </summary>
        internal static string Schema_DBExists {
            get {
                return ResourceManager.GetString("Schema_DBExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IF OBJECT_ID(&apos;dbo.SchemaVersion&apos;, &apos;U&apos;) IS NOT NULL
        ///begin
        ///  select
        ///    SchemaSetName, [Version], LastUpdated
        ///  from
        ///    SchemaVersion
        ///  where
        ///    SchemaSetName = @SchemaSetName
        ///
        ///end
        ///else
        ///begin
        ///  select @SchemaSetName as SchemaSetName, -1 as Version, GETUTCDATE() as LastUpdated
        ///end.
        /// </summary>
        internal static string SchemaVersion_Get {
            get {
                return ResourceManager.GetString("SchemaVersion_Get", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to if exists (select 1 from SchemaVersion where SchemaSetName = @SchemaSetName)
        ///begin
        ///  update
        ///    SchemaVersion
        ///  set
        ///    [Version] = @Version, LastUpdated = GETUTCDATE()
        ///  where
        ///    SchemaSetName = @SchemaSetName
        ///
        ///end
        ///else
        ///begin
        ///  insert SchemaVersion (SchemaSetName, [Version], LastUpdated)
        ///  values (@SchemaSetName, @Version, GETUTCDATE())
        ///end.
        /// </summary>
        internal static string SchemaVersion_Update {
            get {
                return ResourceManager.GetString("SchemaVersion_Update", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select
        ///    SiteNavigationId, MenuName, ChildMenuName, DisplayLabel, Controller, [Action], Url, OptionOrder, Active, Roles, row_version
        ///from 
        ///    SiteNavigation
        ///where
        ///    MenuName = isNull(@MenuName, MenuName) and (@ActiveOnly = 0 or Active = 1)
        ///order by
        ///    MenuName, OptionOrder.
        /// </summary>
        internal static string SiteNavigation_ByMenuNameList_Get {
            get {
                return ResourceManager.GetString("SiteNavigation_ByMenuNameList_Get", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to delete
        ///    SiteNavigation
        ///where
        ///    SiteNavigationId = @SiteNavigationId
        ///.
        /// </summary>
        internal static string SiteNavigation_Delete {
            get {
                return ResourceManager.GetString("SiteNavigation_Delete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select SiteNavigationId, [Action], Active, ChildMenuName, Controller, DisplayLabel, MenuName, OptionOrder, Url, Roles, row_version
        ///from SiteNavigation 
        ///where SiteNavigationId = @SiteNavigationId
        ///.
        /// </summary>
        internal static string SiteNavigation_Get {
            get {
                return ResourceManager.GetString("SiteNavigation_Get", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to if exists (select 1 from SiteNavigation where SiteNavigationid = @SiteNavigationid)
        ///begin
        ///    update
        ///        SiteNavigation
        ///    set
        ///        [Action] = @Action, Active = @Active, ChildMenuName = @ChildMenuName, Controller = @Controller, DisplayLabel = @DisplayLabel, MenuName = @MenuName, OptionOrder = @OptionOrder, Url = @Url, Roles = @Roles
        ///    where
        ///        SiteNavigationId = @SiteNavigationId
        ///end
        ///else
        ///begin
        ///    insert SiteNavigation([Action], Active, ChildMenuName, Controller, DisplayLabel, Men [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SiteNavigation_Update {
            get {
                return ResourceManager.GetString("SiteNavigation_Update", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to delete
        ///  UserProfile
        ///where
        ///    UserId  = @userid
        ///.
        /// </summary>
        internal static string User_Delete {
            get {
                return ResourceManager.GetString("User_Delete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select	
        ///  u.UserId,	
        ///  u.UserName,	
        ///  u.Created,
        ///  u.LastLogin,
        ///  u.row_version
        ///from
        ///  UserProfile u
        ///where
        ///    u.UserId  = @userid
        ///
        ///select 
        ///  *
        ///from
        ///  webpages_UsersInRoles uir 
        ///  inner join webpages_Roles r on r.RoleId = uir.RoleId
        ///where
        ///    uir.UserId  = @userid
        ///.
        /// </summary>
        internal static string User_Get {
            get {
                return ResourceManager.GetString("User_Get", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to if exists (select 1 from UserProfile where userid = @userid)
        ///begin
        ///    update
        ///        UserProfile
        ///    set
        ///        UserName = @UserName, LastLogin = @LastLogin
        ///    where
        ///        UserId = @UserId
        ///end
        ///else
        ///begin
        ///    insert UserProfile(UserName, Created, LastLogin)
        ///    values (@UserName, @Created, @LastLogin)
        ///end.
        /// </summary>
        internal static string User_Update {
            get {
                return ResourceManager.GetString("User_Update", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to delete webpages_UsersInRoles where UserId = @UserId.
        /// </summary>
        internal static string UserRole_Clear {
            get {
                return ResourceManager.GetString("UserRole_Clear", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to if not exists (select 1 from webpages_UsersInRoles where userid = @UserId &amp;&amp; roleid = @RoleId)
        ///begin
        ///    insert webpages_UsersInRoles(RoleId, UserId)
        ///    values (@RoleId, @UserId)
        ///end.
        /// </summary>
        internal static string UserRole_Update {
            get {
                return ResourceManager.GetString("UserRole_Update", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select
        ///  count(*) total_rows
        ///from
        ///  UserProfile
        ///where 
        ///  1=1 {0};
        ///  
        ///select	
        ///  u.UserId,	
        ///  u.UserName,	
        ///  u.Created,
        ///  u.LastLogin,
        ///  u.row_version
        ///from
        ///  UserProfile u
        ///where
        ///    1 = 1 {0}
        ///order by
        ///  {1}
        ///offset @FirstItem - 1 rows
        ///fetch next @PageSize rows only;
        ///
        ///
        ///.
        /// </summary>
        internal static string Users_Get {
            get {
                return ResourceManager.GetString("Users_Get", resourceCulture);
            }
        }
    }
}
