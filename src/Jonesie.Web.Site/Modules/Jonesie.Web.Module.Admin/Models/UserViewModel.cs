using Jonesie.Web.Entities.Core;
using Jonesie.Web.Entities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jonesie.Web.Module.Admin.Models
{
  public class UserViewModel : BaseViewModel
  {

    List<Role> _allRoles;


    public UserViewModel()
      : base("User", "User")
    {
    }

    public UserViewModel(UserProfile user, List<Role> allRoles)
      : this()
    {
      _allRoles = allRoles;
      User = user;
      SelectedRoles = user.Roles.Select(r => r.RoleId).ToArray();      
    }

    public UserProfile User { get; set; }

    public int[] SelectedRoles
    {
      get;
      set;
    }

    public MultiSelectList RolesList
    {
      get
      {
        if (_allRoles != null)
        {
          //.Select(r => new SelectListItem() { Value = r.RoleId.ToString(), Text = r.RoleName})
          //, User.Roles.Select(r=> r.RoleId)
          return new MultiSelectList(_allRoles, "RoleId", "RoleName");
        }

        return null;
      }
    }
  }
}