using Jonesie.Web.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jonesie.Web.Module.Admin.Models
{
  public class NavigationOptionsViewModel : BaseViewModel
  {

    public NavigationOptionsViewModel()
      : base("Option", "Navigation Option")
    {
      if(HttpContext.Current.User.IsInRole("Administrators"))
      {
        ToolbarButtons = BaseViewModelToolbarEnum.Add | BaseViewModelToolbarEnum.Edit | BaseViewModelToolbarEnum.Delete | BaseViewModelToolbarEnum.Refresh;
      }
    }
  }
}