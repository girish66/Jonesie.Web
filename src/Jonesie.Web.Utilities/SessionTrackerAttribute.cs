using Jonesie.Web.Contracts.Core;
using Jonesie.Web.Entities.Data;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jonesie.Web.Utilities
{
  public class SessionTrackerAttribute : ActionFilterAttribute, IActionFilter
  {
    ISettings _settings;

    public SessionTrackerAttribute()
    {
      _settings = DependencyResolver.Current.GetService<ISettings>();
    }

    public override void OnActionExecuted(ActionExecutedContext filterContext)
    {
      base.OnActionExecuted(filterContext);
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      var sess = _settings.Sessions.FirstOrDefault(s => s.SessionId == HttpContext.Current.Session.SessionID);
      if (sess == null)
      {
        sess = new UserSession
        {
          SessionId = HttpContext.Current.Session.SessionID,
          IPAddress = HttpContext.Current.Request.UserHostAddress,
          Started = DateTimeOffset.Now,
        };

        _settings.Sessions.Add(sess);
      }

      sess.LastRequest = DateTimeOffset.Now;
      sess.UserId = HttpContext.Current.User.Identity.IsAuthenticated ? HttpContext.Current.User.Identity.Name : string.Empty;

      base.OnActionExecuting(filterContext);
    }
  }
}