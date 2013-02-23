using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jonesie.Web.Site.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View("Error");
        }

        [AllowAnonymous]
        public ActionResult HttpError404()
        {
            return View("Error404");
        }

        //public ActionResult HttpError500(string message)
        //{
        //    ViewBag.Message = message;
        //    return View("Error500");
        //}

        [AllowAnonymous]
        public ActionResult Unauthorized()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult General(string message)
        {
            ViewBag.Message = message;
            return View("Error");
        }
    }
}
