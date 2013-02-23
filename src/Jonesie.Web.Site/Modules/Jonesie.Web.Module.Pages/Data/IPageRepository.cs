using Jonesie.Web.Entities.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Jonesie.Web.Module.Pages.Data
{
  [InheritedExport]
  public interface IPageRepository
  {

    /// <summary>
    /// Gets a composed page by id
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns></returns>
    ComposedPage GetComposedPage(int id);

    /// <summary>
    /// Gets a composed page by Url.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <returns></returns>
    ComposedPage GetComposedPage(string url);

    /// <summary>
    /// Deletes a composed page.
    /// </summary>
    /// <param name="pageId">The page id.</param>
    void DeleteComposedPage(int pageId);

    /// <summary>
    /// Updates a composed page.
    /// </summary>
    /// <param name="page">The page.</param>
    void UpdateComposedPage(ComposedPage page);

    /// <summary>
    /// Publishes a composed page.
    /// </summary>
    /// <param name="pageId">The page id.</param>
    void PublishComposedPage(int pageId);
  }
}
