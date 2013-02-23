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
    /// A special type of view module that display something on the site toolbar
    /// </summary>
    [InheritedExport]
    public interface IToolboxModule : IViewModule
    {
        /// <summary>
        /// Gets a list of items for the toolbox.
        /// </summary>
        IEnumerable<ToolboxItem> Items { get; }
    }
}
