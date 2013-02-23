using Jonesie.Web.Entities.Core;
using Jonesie.Web.Entities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jonesie.Web.Module.Admin.Models
{
  public class NavigationOptionViewModel : BaseViewModel
  {

    public NavigationOptionViewModel()
      : base("Option", "Navigation Option")
    {
    }

    public SiteNavigation Option { get; set; }
  }
}