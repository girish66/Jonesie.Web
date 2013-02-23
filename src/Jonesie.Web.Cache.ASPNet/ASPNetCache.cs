using Jonesie.Web.Contracts.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Jonesie.Web.Cache.ASPNet
{
  /// <summary>
  /// An implementation of ICache using the ASPNet web cache.
  /// </summary>
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class ASPNetCache : ICache
  {
    #region members
    System.Web.Caching.Cache _cache;
    ILogger _logger;
    ISettings _settings;
    TimeSpan _shortTimeout;
    TimeSpan _mediumTimeout;
    TimeSpan _longTimeout;

    /// <summary>
    /// Need a seperate list of all the keys in the cache 
    /// </summary>
    List<string> _keys = new List<string>();
    #endregion

    #region construction

    /// <summary>
    /// Initializes a new instance of the <see cref="ASPNetCache" /> class using MEF.
    /// </summary>
    /// <param name="settings">The settings.</param>
    [ImportingConstructor]
    public ASPNetCache(ISettings settings, ILogger logger)
      : this(settings.CacheShortTimeoutSecs, settings.CacheMediumTimeoutSecs, settings.CacheLongTimeoutSecs, logger)
    {
      _settings = settings;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ASPNetCache" /> class.
    /// </summary>
    /// <param name="shortTimeoutSecs">The short timeout.</param>
    /// <param name="mediumTimeoutSecs">The medium timeout.</param>
    /// <param name="longTimeoutSecs">The long timeout.</param>
    public ASPNetCache(int shortTimeoutSecs, int mediumTimeoutSecs, int longTimeoutSecs, ILogger logger)
    {
      _logger = logger;
      _shortTimeout = new TimeSpan(0, 0, shortTimeoutSecs);
      _mediumTimeout = new TimeSpan(0, 0, mediumTimeoutSecs);
      _longTimeout = new TimeSpan(0, 0, longTimeoutSecs);

      _cache = System.Web.HttpRuntime.Cache;

      if (_cache == null)
      {
        _cache = new System.Web.Caching.Cache();
      }
    }
    #endregion

    #region ICache implementation
    /// <summary>
    /// Adds to short term cache.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="o">The o.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void AddToShortTermCache(string key, object o)
    {
      //_cache.Add(key, o, null, DateTime.MaxValue, new TimeSpan(0, _shortTimeout, 0), System.Web.Caching.CacheItemPriority.Normal, null);

      _cache.Insert(key, o, null, DateTime.MaxValue, _shortTimeout);
      _keys.Add(key);
      _logger.LogDebug("Added " + key + " to short term cache");
    }

    /// <summary>
    /// Adds to medium term cache.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="o">The o.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void AddToMediumTermCache(string key, object o)
    {
      _cache.Insert(key, o, null, DateTime.MaxValue, _mediumTimeout);
      _keys.Add(key);
      _logger.LogDebug("Added " + key + " to medium term cache");
    }

    /// <summary>
    /// Adds to long term cache.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="o">The o.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void AddToLongTermCache(string key, object o)
    {
      _cache.Insert(key, o, null, DateTime.MaxValue, _longTimeout);
      _keys.Add(key);
      _logger.LogDebug("Added " + key + " to long term cache");
    }

    /// <summary>
    /// Gets from cache.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public T GetFromCache<T>(string key)
    {
      return GetFromCache<T>(key, null);
    }

    /// <summary>
    /// Gets from cache.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key.</param>
    /// <param name="getter">The getter.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public T GetFromCache<T>(string key, Func<T> getter)
    {
      try
      {
        object item = _cache.Get(key);
        if (item == null && getter != null)
        {
          item = getter();
          AddToMediumTermCache(key, item);
        }

        if (item != null)
        {
          return (T)item;
        }

        return default(T);
      }
      finally
      {
        _logger.LogDebug("Fetched " + key + " from cache");
      }
    }

    /// <summary>
    /// Forcibly removes and item from the cache.
    /// </summary>
    /// <param name="key">The key.</param>
    public void RemoveFromCache(string key)
    {
      var removedKeys = new List<string>();
      var rx = new Regex(key);

      foreach (var akey in _keys)
      {
        if (rx.IsMatch(akey))
        {
          _cache.Remove(akey);
          removedKeys.Add(akey);
        }
      }

      foreach (var akey in removedKeys)
      {
        _keys.Remove(akey);
      }

      _logger.LogDebug("Removed " + key + " from cache");
    }

    #endregion
  }
}
