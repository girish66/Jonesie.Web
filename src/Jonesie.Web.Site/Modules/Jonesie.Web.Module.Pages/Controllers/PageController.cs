using Jonesie.Web.Module.Pages.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jonesie.Web.Module.Pages.Controllers
{
  [Export]
  [PartCreationPolicy(CreationPolicy.NonShared)]
  public class PageController : Controller
  {

    IPageRepository _pageRepos;

    [ImportingConstructor]
    public PageController(IPageRepository pageRepos)
    {
      _pageRepos = pageRepos;
    }

    /// <summary>
    /// View a page by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ActionResult Index(int id)
    {
      var page = _pageRepos.GetComposedPage(id);

      if (page == null)
      {
        throw new HttpException(404, "Page could not be found");
      }

      // get the full template path for the page and widget

      return View("Page", page);
    }


    public ActionResult _Admin()
    {
      return View("PageAdmin");
    }
  }
}
