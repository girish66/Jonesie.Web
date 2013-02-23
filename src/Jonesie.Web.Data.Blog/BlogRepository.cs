using Jonesie.Web.Common;
using Jonesie.Web.Contracts.Core;
using Jonesie.Web.Contracts.Data;
using Jonesie.Web.Entities.Data;
using Jonesie.Web.Entities.Data.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using System.ComponentModel.Composition;

namespace Jonesie.Web.Data.Blog
{
  /// <summary>
  /// A repository for blog posts etc
  /// </summary>
  [Export]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class BlogRepository : BaseRepository, IBlogRepository
  {

    /// <summary>
    /// Initializes a new instance of the <see cref="BlogRepository" /> class.
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="cache">The cache.</param>
    /// <param name="connectionFactory">The connection factory.</param>
    /// <param name="databaseManager">The database manager.</param>
    [ImportingConstructor]
    public BlogRepository(ISettings settings, ILogger logger, ICache cache, IConnectionFactory connectionFactory, IDatabaseManager databaseManager)
      : base(settings, logger, cache, connectionFactory, databaseManager)
    {

    }

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
      return PerformCachedQuery("Jonesie.Web.Data.Blog.BlogRepository.GetPosts({0},{1},{2})".FormatWith(page, sortColumn, filter), () =>
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

        using (var multi = GetConnection().QueryMultiple(Properties.Resources.BlogPosts_Get.FormatWith(filter, order),
                                                        new
                                                        {
                                                          FirstItem = GetFirstItemForPage(page, pageSize),
                                                          PageSize = pageSize
                                                        }))
        {
          var items = multi.Read<BlogPost>();
          var totalCount = multi.Read<int>().SingleOrDefault();

          return
            new DataSet<BlogPost>(items, totalCount, sortColumn: sortColumn, orderDescending: descending, page: page, pageSize: pageSize);
        }
      });
    }

    public BlogPost GetPost(int id)
    {
      return PerformNonCachedQuery("Jonesie.Web.Data.Blog.BlogRepository.GetPost({0})".FormatWith(id), () =>
      {
        return GetConnection().Query<BlogPost>(Properties.Resources.BlogPost_Get, new { Id = id }).FirstOrDefault();
      });
    }
  }
}
