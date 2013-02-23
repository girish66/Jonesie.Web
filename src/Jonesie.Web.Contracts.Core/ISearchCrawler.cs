using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Jonesie.Web.Contracts.Core
{
    /// <summary>
    /// All crawlers implement this
    /// </summary>
    [InheritedExport]
    public interface ISearchCrawler
    {
        /// <summary>
        /// Begins the crawl.
        /// </summary>
        /// <param name="updateStatus">The update status action is called when the crawler has something to report. It passes a message and the percentage complete.</param>
        void BeginCrawl(Action<string, int> updateStatus);

    }
}
