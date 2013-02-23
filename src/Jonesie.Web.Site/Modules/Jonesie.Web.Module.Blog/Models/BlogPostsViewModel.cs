using Jonesie.Web.Entities.Core;
using Jonesie.Web.Module.Blog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jonesie.Web.Module.Blog.Models
{
  public class BlogPostsViewModel : BaseViewModel
  {
    public BlogPost Post { get; set; }

    public BlogPostsViewModel()
      : base("BlogPost", "Post")
    {
      if (HttpContext.Current.User.IsInRole("BlogAdmin"))
      {
        ToolbarButtons = BaseViewModelToolbarEnum.Add | BaseViewModelToolbarEnum.Delete | BaseViewModelToolbarEnum.Edit;
      }
    }


  }
}