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

namespace Jonesie.Web.Module.Pages.Data
{
  /// <summary>
  /// A repository for CMS Pages
  /// </summary>
  [Export]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class PageRepository : BaseRepository, IPageRepository
  {
    /// <summary>
    /// Gets a composed page by id.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public ComposedPage GetComposedPage(int id)
    {
      return null;
    }

    /// <summary>
    /// Gets a composed page by Url.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <returns></returns>
    public ComposedPage GetComposedPage(string url)
    {
      return null;
    }


    /// <summary>
    /// Deletes a composed page.
    /// </summary>
    /// <param name="pageId">The page id.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void DeleteComposedPage(int pageId)
    {
      
    }

    /// <summary>
    /// Updates a composed page.
    /// </summary>
    /// <param name="page">The page.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void UpdateComposedPage(ComposedPage page)
    {
      
    }

    /// <summary>
    /// Publishes a composed page.
    /// </summary>
    /// <param name="pageId">The page id.</param>
    public void PublishComposedPage(int pageId)
    {
      
    }

  }
}
