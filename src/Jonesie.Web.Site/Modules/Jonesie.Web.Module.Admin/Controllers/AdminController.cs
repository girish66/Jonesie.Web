using Jonesie.Web.Contracts.Core;
using Jonesie.Web.Module.Admin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jonesie.Web.Module.Admin.Controllers
{
  [Export]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class AdminController : Controller
  {

    [ImportingConstructor]
    public AdminController(ISettings settings)
    {

    }


    /// <summary>
    /// Display the dashboard
    /// </summary>
    /// <returns></returns>
    public ActionResult Index()
    {

      
      return View(new AdminDashboardViewModel()
        {

        });
    }

  }
}
