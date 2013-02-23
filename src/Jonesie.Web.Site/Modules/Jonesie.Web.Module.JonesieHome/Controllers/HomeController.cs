using Jonesie.Web.Contracts.Core;
using Jonesie.Web.Module.JonesieHome.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jonesie.Web.Module.JonesieHome.Controllers
{
  [AllowAnonymous]
  [Export]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class HomeController : Controller
  {

    [Import]
    protected ILogger Logger { get; set; }

    [Import]
    protected IWebNotification Notifier { get; set; }


    [AllowAnonymous]
    public ActionResult Index()
    {
      ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

      return View();
    }

    [AllowAnonymous]
    public ActionResult About()
    {
      ViewBag.Message = "Your app description page.";

      return View();
    }

    [AllowAnonymous]
    public ActionResult Contact()
    {
      ViewBag.Message = "Your contact page.";

      return View();
    }

    public ActionResult SendBroadcast()
    {
      return PartialView("BroadcastMessage", new BroadcastMessageViewModel());
    }

    [HttpPost]
    public ActionResult SendBroadcast(BroadcastMessageViewModel vm)
    {
      if (ModelState.IsValid)
      {
        try
        {
          Notifier.NotifyAllUsers(vm.Type, vm.Message);
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
        ModelState.AddModelError("", "The message is not valid.");
        Response.StatusCode = 400;
        return Json(new { Success = false, Message = "The message is not valid." }, JsonRequestBehavior.DenyGet);
      }

    }
  }
}
