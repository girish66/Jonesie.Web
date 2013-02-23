using Jonesie.Web.Contracts.Data;
using Jonesie.Web.Entities.Data;
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
  public class NavigationController : Controller
  {
    INavigationRepository _repos;

    [ImportingConstructor]
    public NavigationController(INavigationRepository repos)
    {
      _repos = repos;
    }

    public ActionResult Index()
    {
      return View(new NavigationOptionsViewModel { Controller = "Navigation" });
    }

    public ActionResult GetOptions(int page, int pageSize = 100, string sortColumn = "", bool orderDescending = false, string filter = "")
    {
      var ds = _repos.GetOptions(null, true);
      return Json(ds, JsonRequestBehavior.AllowGet);
    }

    public ActionResult NewOption()
    {
      return PartialView("Option", new NavigationOptionViewModel { Controller = "Navigation", Option = new SiteNavigation(), IsNew = true });
    }

    public ActionResult EditOption(int id)
    {
      return PartialView("Option", new NavigationOptionViewModel { Controller = "Navigation", Option = _repos.GetOption(id), IsNew = false });
    }

    public ActionResult DeleteOption(int id)
    {
      return PartialView("Option", new NavigationOptionViewModel { Controller = "Navigation", Option = _repos.GetOption(id), IsNew = false, Delete = true });
    }

    public ActionResult SaveOption(NavigationOptionViewModel vm)
    {
      if (vm.Delete)
      {
        _repos.DeleteOption(vm.Option.SiteNavigationId);
        return Json(new { Success = true }, JsonRequestBehavior.DenyGet);
      }
      else
        if (ModelState.IsValid)
        {
          try
          {
            _repos.UpdateOption(vm.Option);
            Response.StatusCode = 200;
            return Json(new { Success = true }, JsonRequestBehavior.DenyGet);
          }
          catch (Exception ex)
          {
            Response.StatusCode = 400;
            return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.DenyGet);
          }
        }
        else
        {
          ModelState.AddModelError("", "The Option is not valid.");
          Response.StatusCode = 400;
          return Json(new { Success = false, Message = "The Option is not valid." }, JsonRequestBehavior.DenyGet);
        }
    }
  }
}
