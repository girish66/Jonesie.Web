using Jonesie.Web.Entities.Core;
using Jonesie.Web.Entities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jonesie.Web.Module.Admin.Models
{
  public class RoleViewModel : BaseViewModel
  {
    public RoleViewModel()
      : base("Role", "Role", BaseViewModelToolbarEnum.Add | BaseViewModelToolbarEnum.Edit | BaseViewModelToolbarEnum.Delete | BaseViewModelToolbarEnum.Refresh)
    {

    }

    public Role Role { get; set; }
  }
}