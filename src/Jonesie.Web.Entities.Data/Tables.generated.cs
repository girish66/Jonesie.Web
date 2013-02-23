﻿/*******************************************
*
* Generated by a tool.  Do not alter.  
* 
*******************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace Jonesie.Web.Entities.Data
{             

  public partial class RoleActionMap  : BaseEntity             
  {
    public Int32 RoleActionMapId { get; set; }                    
    public Int32 RoleId { get; set; }                    
    public String Path { get; set; }                    

    private static string[] _columnNames = { "roleactionmapid","roleid","path" };

    public static string[] ColumnNames 
    {
      get 
      {
        return _columnNames;
      }
    }
  }

  public partial class SchemaVersion  : BaseEntity             
  {
    public String SchemaSetName { get; set; }                    
    public Int32 Version { get; set; }                    
    public DateTimeOffset LastUpdated { get; set; }                    

    private static string[] _columnNames = { "schemasetname","version","lastupdated" };

    public static string[] ColumnNames 
    {
      get 
      {
        return _columnNames;
      }
    }
  }

  public partial class SiteNavigation  : BaseEntity             
  {
    public Int32 SiteNavigationId { get; set; }                    
    public String MenuName { get; set; }                    
    public String ChildMenuName { get; set; }                    
    public String DisplayLabel { get; set; }                    
    public String Controller { get; set; }                    
    public String Action { get; set; }                    
    public String Url { get; set; }                    
    public Int32 OptionOrder { get; set; }                    
    public Boolean Active { get; set; }                    
    public String Roles { get; set; }                    

    private static string[] _columnNames = { "sitenavigationid","menuname","childmenuname","displaylabel","controller","action","url","optionorder","active","roles" };

    public static string[] ColumnNames 
    {
      get 
      {
        return _columnNames;
      }
    }
  }

  public partial class UserProfile  : BaseEntity             
  {
    public Int32 UserId { get; set; }                    
    public String UserName { get; set; }                    
    public DateTimeOffset Created { get; set; }                    
    public DateTimeOffset LastLogin { get; set; }                    

    private static string[] _columnNames = { "userid","username","created","lastlogin" };

    public static string[] ColumnNames 
    {
      get 
      {
        return _columnNames;
      }
    }
  }

  public partial class Role  : BaseEntity             
  {
    public Int32 RoleId { get; set; }                    
    public String RoleName { get; set; }                    

    private static string[] _columnNames = { "roleid","rolename" };

    public static string[] ColumnNames 
    {
      get 
      {
        return _columnNames;
      }
    }
  }

  public partial class UserRole  : BaseEntity             
  {
    public Int32 UserId { get; set; }                    
    public Int32 RoleId { get; set; }                    

    private static string[] _columnNames = { "userid","roleid" };

    public static string[] ColumnNames 
    {
      get 
      {
        return _columnNames;
      }
    }
  }
}