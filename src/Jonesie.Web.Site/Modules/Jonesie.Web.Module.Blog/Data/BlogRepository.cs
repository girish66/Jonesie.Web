using Jonesie.Web.Common;
using Jonesie.Web.Contracts.Core;
using Jonesie.Web.Contracts.Data;
using Jonesie.Web.Entities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using System.ComponentModel.Composition;
using Jonesie.Web.Data;

namespace Jonesie.Web.Module.Blog.Data
{
  /// <summary>
  /// A repository for blog posts etc
  /// </summary>
  [Export]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class BlogRepository : BaseRepository, IBlogRepository
  {

    const string CACHE_KEY = "Jonesie_Web_Data_Blog_BlogRepository_{0}";

    /// <summary>
    /// Gets the posts.
    /// </summary>
    /// <param name="page">The page.</param>
    /// <param name="pageSize">Size of the page.</param>
    /// <param name="sortColumn">The sort column.</param>
    /// <param name="descending">if set to <c>true</c> [descending].</param>
    /// <param name="filter">The filter.</param>
    /// <returns></returns>
    public DataSet<BlogPost> GetPosts(int page = 1, int pageSize = 10, string sortColumn = "Posted", bool descending = true, string filter = null)
    {
      return PerformCachedQuery(CACHE_KEY.FormatWith("Posts({0},{1},{2})".FormatWith(page, sortColumn, filter)),
        (con) =>
        {
          if (string.IsNullOrEmpty(sortColumn))
            sortColumn = "Posted";

          var order = sortColumn + (descending ? " desc" : string.Empty);

          if (filter != null)
          {
            filter = "and ((Title like '%" + filter + "%') or (Author like '%" + filter + "%') or (Body like '%" + filter + "%'))";
          }
          else
          {
            filter = string.Empty;
          }

          using (var multi = con.QueryMultiple(Properties.Resources.BlogPosts_Get.FormatWith(filter, order),
                                                          new
                                                          {
                                                            FirstItem = GetFirstItemForPage(page, pageSize),
                                                            PageSize = pageSize
                                                          }))
          {
            var totalCount = multi.Read<int>().SingleOrDefault();
            var items = multi.Read<BlogPost>();

            return
              new DataSet<BlogPost>(items, totalCount, sortColumn: sortColumn, orderDescending: descending, page: page, pageSize: pageSize);
          }
        });
    }

    public BlogPost GetPost(int id)
    {
      return PerformNonCachedQuery(CACHE_KEY.FormatWith("Post({0})".FormatWith(id)),
        (con) =>
        {
          return con.Query<BlogPost>(Properties.Resources.BlogPost_Get, new { Id = id }).FirstOrDefault();
        });
    }


    public void DeletePost(int blogPostId)
    {
      ExecuteQuery(CACHE_KEY.FormatWith("Post_Delete({0})".FormatWith(blogPostId)),
        (con) =>
        {
          con.Execute(
            Properties.Resources.BlogPost_Delete, new { Id = blogPostId });
          Cache.RemoveFromCache(@"Jonesie\.Web\.Data\.Blog\.BlogRepository\..*");
        });
    }

    public void UpdatePost(BlogPost blogPost)
    {
      ExecuteQuery("Jonesie.Web.Data.BlogRepository.UpdatePost({0}, {1})".FormatWith(blogPost.Id, blogPost.Title),
        (con) =>
        {
          con.Execute(
            Properties.Resources.BlogPost_Update, blogPost);

          // clear the cache
          Cache.RemoveFromCache(CACHE_KEY.FormatWith(".*"));
        });
    }
  }
}
