using Jonesie.Web.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jonesie.Web.Module.Admin.Models
{
  public class RolesViewModel : BaseViewModel
  {
    public RolesViewModel()
      : base("Role", "Role", BaseViewModelToolbarEnum.Add | BaseViewModelToolbarEnum.Edit | BaseViewModelToolbarEnum.Delete | BaseViewModelToolbarEnum.Refresh)
    {

    }
  }
}