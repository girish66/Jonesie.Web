using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;
using Jonesie.Web.Contracts.Core;
using Jonesie.Web.Common;
using Microsoft.AspNet.SignalR;

namespace Jonesie.Web
{
    [HubName("jonesie")]
    public class MessageHub : Hub, IWebNotification
    {
        /// <summary>
        /// The _cache is used to store the currently logged on users
        /// </summary>
        ICache _cache;

        public MessageHub()
        {
            _cache = System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ICache)) as ICache;
        }

        /// <summary>
        /// Registers the user against a connection id
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        public void RegisterUser(string userName)
        {
            _cache.RemoveFromCache("Jonesie.Web.MessageHub_" + Context.ConnectionId);
            _cache.RemoveFromCache("Jonesie.Web.MessageHub_" + userName);
            _cache.AddToMediumTermCache("Jonesie.Web.MessageHub_" + Context.ConnectionId, userName);
            _cache.AddToMediumTermCache("Jonesie.Web.MessageHub_" + userName, Context.ConnectionId);

#if DEBUG
            //NotifyCurrentUser(WebNotificationType.Information, "Your ID is " + userName + " (" + Context.ConnectionId + ")");
            //NotifyOtherUsers(WebNotificationType.Warning, userName + " just connected using id " + Context.ConnectionId);
#endif
        }

        /// <summary>
        /// Notifies all users.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        public void NotifyAllUsers(WebNotificationType type, string message)
        {
          GlobalHost.ConnectionManager.GetHubContext("jonesie").Clients.All.showalert(type.ToString(), message, type.Description());
        }

        /// <summary>
        /// Notifies the current user.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        public void NotifyCurrentUser(WebNotificationType type, string message)
        {
          var c = GlobalHost.ConnectionManager.GetHubContext("jonesie");
          c.Clients.Client(Context.ConnectionId).showalert(type.ToString(), message, type.Description());
        }

        /// <summary>
        /// Notifies the other users.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        public void NotifyOtherUsers(WebNotificationType type, string message)
        {
          GlobalHost.ConnectionManager.GetHubContext("jonesie").Clients.AllExcept(new string[] { Context.ConnectionId }).showalert(type.ToString(), message, type.Description());
        }
    }
}