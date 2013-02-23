using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jonesie.Web.Entities.Core
{
    /// <summary>
    /// A search result
    /// </summary>
    public class SearchResult
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResult" /> class.
        /// </summary>
        public SearchResult()
        {
            Properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or sets the primary id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the abstract.
        /// </summary>
        /// <value>
        /// The abstract.
        /// </value>
        public string Abstract { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>
        /// The create date.
        /// </value>
        public DateTime? EditDate { get; set; }

        /// <summary>
        /// Gets the custom properties that are stored in the index.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public Dictionary<string, object> Properties { get; private set; }

    }
}
