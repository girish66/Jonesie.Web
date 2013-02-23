using System.ComponentModel.Composition;
using System.Web.Http;

namespace Jonesie.Web.Contracts.Core
{
    /// <summary>
    /// A contract for all modularised services
    /// </summary>
    [InheritedExport(typeof(IServiceModule))]
    public interface IServiceModule : IModule
    {
        /// <summary>
        /// Registers the routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        void RegisterRoutes(HttpRouteCollection routes);
    }
}
