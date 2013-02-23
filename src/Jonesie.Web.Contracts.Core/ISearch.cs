using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jonesie.Web.Entities.Core;

namespace Jonesie.Web.Contracts.Core
{
    /// <summary>
    /// A contract for anything that will search
    /// </summary>
    [InheritedExport]
    public interface ISearch
    {
        /// <summary>
        /// Performs the search.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="searchWords">The search words.</param>
        /// <param name="searchField">The search field.</param>
        /// <returns></returns>
        IEnumerable<SearchResult> PerformSearch(string category, string searchWords, string searchField);
    }

    //[InheritedExport]
    //public interface ISearchOptions
    //{

    //}
}
