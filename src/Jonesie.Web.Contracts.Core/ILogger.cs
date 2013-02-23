using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jonesie.Web.Contracts.Core
{
    /// <summary>
    /// A contract for all loggers
    /// </summary>
    [InheritedExport(typeof(ILogger))]
    public interface ILogger
    {
        /// <summary>
        /// Logs the debug message.
        /// </summary>
        /// <param name="message">The message.</param>
        void LogDebug(string message);

        /// <summary>
        /// Logs the info message.
        /// </summary>
        /// <param name="message">The message.</param>
        void LogInfo(string message);

        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="message">The message.</param>
        void LogWarning(string message);

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        void LogError(string message);

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        void LogError(string message, Exception ex);

        /// <summary>
        /// Indents this instance.
        /// </summary>
        void Indent();

        /// <summary>
        /// Uns the indent.
        /// </summary>
        void UnIndent();
    }
}
