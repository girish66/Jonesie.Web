using Jonesie.Web.Entities.Core;
using Jonesie.Web.Entities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jonesie.Web.Module.Admin.Models
{
  public class RoleActionMapViewModel : BaseViewModel
  {

    public RoleActionMapViewModel()
      : base("RoleActionMap", "RoleActionMap")
    {
      
    }

    public RoleActionMapViewModel(RoleActionMap ram, DataSet<Role> roles)
      : this()
    {

      RoleActionMap = ram;
      if (roles != null)
      {
        RolesList = new SelectList(roles.Items, "RoleId", "RoleName", RoleActionMap.RoleId);
      }
    }

    public SelectList RolesList { get; private set; }

    public RoleActionMap RoleActionMap { get; set; }
  }
}