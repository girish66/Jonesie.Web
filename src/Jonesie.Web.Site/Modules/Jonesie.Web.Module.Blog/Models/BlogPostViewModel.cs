using Jonesie.Web.Entities.Core;
using Jonesie.Web.Module.Blog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jonesie.Web.Module.Blog.Models
{
  public class BlogPostViewModel : BaseViewModel
  {

    public BlogPost Post { get; set; }

    public BlogPostViewModel()
      : base("BlogPost", "Post")
    {
      
    }
  }
}