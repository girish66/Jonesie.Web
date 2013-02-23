using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jonesie.Web.Contracts.Core
{
    /// <summary>
    /// A service for sending notifications to clients
    /// </summary>
    [InheritedExport(typeof(IWebNotification))]
    public interface IWebNotification
    {
        void NotifyAllUsers(WebNotificationType type, string message);
        void NotifyCurrentUser(WebNotificationType type, string message);
        void NotifyOtherUsers(WebNotificationType type, string message);
    }


    /// <summary>
    /// Types of notifications to the client
    /// </summary>
    public enum WebNotificationType {
        [Description("info")]
        Information,
        [Description("warning")]
        Warning,
        [Description("error")]
        Error,
        [Description("success")]
        Success
    }
}
