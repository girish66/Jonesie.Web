using Jonesie.Web.Entities.Data;
using Jonesie.Web.Entities.Data.Blog;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Jonesie.Web.Data.Blog
{
  public interface IBlogRepository
  {
    DataSet<BlogPost> GetPosts(int page = 1, int pageSize = 10, string sortColumn = "Posted", bool descending = true, string filter = null);
  }
}
