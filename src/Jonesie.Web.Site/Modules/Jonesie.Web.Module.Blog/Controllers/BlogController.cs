using Jonesie.Web.Contracts.Core;
using Jonesie.Web.Data;
using Jonesie.Web.Module.Blog.Models;
using Jonesie.Web.Module.Blog.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jonesie.Web.Module.Blog.Controllers
{
  [Export]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class BlogController : Controller
  {

    IBlogRepository _repos;
    ISettings _settings;

    [ImportingConstructor]
    public BlogController(ISettings settings, IBlogRepository repos)
    {
      _repos = repos;
      _settings = settings;
    }

    /// <summary>
    /// Get the Index view.
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous] 
    public ActionResult Index()
    {
      return View(new BlogPostsViewModel { Controller = "Blog" });
    }

    [AllowAnonymous]
    public ActionResult GetBlogPosts(int page = 1, int pageSize = 10, string sortColumn = "Posted", bool descending = true, string filter = null)
    {
      var posts = _repos.GetPosts(page: page, pageSize: pageSize, sortColumn: sortColumn, descending: descending, filter: filter);
      return Json(posts, JsonRequestBehavior.AllowGet);
    }

    public ActionResult NewBlogPost()
    {
      return PartialView("BlogPost", new BlogPostViewModel { IsNew = true, Controller = "Blog", Post = new BlogPost() });
    }

    public ActionResult EditBlogPost(int id)
    {

      return PartialView("BlogPost", new BlogPostViewModel { IsNew = false, Controller = "Blog", Post = _repos.GetPost(id) });
    }

    public ActionResult DeleteBlogPost(int id)
    {

      return PartialView("BlogPost", new BlogPostViewModel { IsNew = false, Delete = true, Controller = "Blog", Post = _repos.GetPost(id) });
    }

    [HttpPost]
    [ValidateInput(false)]
    public ActionResult SaveBlogPost(BlogPostsViewModel vm)
    {
      if (vm.Delete)
      {
        _repos.DeletePost(vm.Post.Id);
        return Json(new { Success = true }, JsonRequestBehavior.DenyGet);
      }
      else
        if (ModelState.IsValid)
        {
          vm.Post.Posted = DateTimeOffset.Now;

          try
          {
            _repos.UpdatePost(vm.Post);
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
          ModelState.AddModelError("", "The blog post is not valid.");
          Response.StatusCode = 400;
          return Json(new { Success = false, Message = "The blog post is not valid." }, JsonRequestBehavior.DenyGet);
        }
    }
  }
}
