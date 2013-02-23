using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jonesie.Web.Contracts.Core
{
    /// <summary>
    /// A contract for a cache 
    /// </summary>
    [InheritedExport(typeof(ICache))]
    public interface ICache
    {

        /// <summary>
        /// Adds to short term cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="o">The o.</param>
        void AddToShortTermCache(string key, object o);

        /// <summary>
        /// Adds to medium term cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="o">The o.</param>
        void AddToMediumTermCache(string key, object o);

        /// <summary>
        /// Adds to long term cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="o">The o.</param>
        void AddToLongTermCache(string key, object o);

        /// <summary>
        /// Gets a typed value from cache if it's in it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        T GetFromCache<T>(string key);

        /// <summary>
        /// Gets from cache and adds to the cache if it's not there already
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="getter">The getter.</param>
        /// <returns></returns>
        T GetFromCache<T>(string key, Func<T> getter);

        /// <summary>
        /// Forcibly removes and item from the cache.
        /// </summary>
        /// <param name="key">The key.</param>
        void RemoveFromCache(string key);

    }
}
