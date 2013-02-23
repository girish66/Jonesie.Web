using Jonesie.Web.Utilities;
using System.ComponentModel.Composition;
using System.Web.Mvc;

namespace Jonesie.Web.Site
{
  public class FilterConfig
  {
    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
      filters.Add(new HandleErrorAttribute());

      //var att = new AuthorizeActionAttribute();
      //MEFConfig.Container.ComposeParts(att);
      filters.Add(new AuthorizeActionAttribute());

      // cant track the session till after authentication
      filters.Add(new SessionTrackerAttribute());
    }
  }
}