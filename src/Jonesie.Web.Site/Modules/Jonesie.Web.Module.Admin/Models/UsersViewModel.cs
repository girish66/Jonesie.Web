using Jonesie.Web.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jonesie.Web.Module.Admin.Models
{
  public class UsersViewModel : BaseViewModel
  {

    public UsersViewModel()
      : base("User", "User", BaseViewModelToolbarEnum.Add | BaseViewModelToolbarEnum.Edit | BaseViewModelToolbarEnum.Delete | BaseViewModelToolbarEnum.Refresh)
    {

    }
  }
}