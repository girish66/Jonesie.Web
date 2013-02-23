using Jonesie.Web.Entities.Data;
using System;
using System.ComponentModel.Composition;

namespace Jonesie.Web.Contracts.Data
{
  /// <summary>
  /// A contract for managing site navigation data
  /// </summary>
  [InheritedExport(typeof(INavigationRepository))]
  public interface INavigationRepository
  {

    /// <summary>
    /// Gets all the options.
    /// </summary>
    /// <returns></returns>
    DataSet<SiteNavigation> GetOptions();

    /// <summary>
    /// Gets the options at any level of the menu.
    /// </summary>
    /// <param name="menuName">Name of the menu.</param>
    /// <param name="activeOnly">if set to <c>true</c> [active only].</param>
    /// <returns></returns>
    DataSet<SiteNavigation> GetOptions(string menuName, bool activeOnly = false);

    /// <summary>
    /// Gets a single option.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns></returns>
    SiteNavigation GetOption(int id);

    /// <summary>
    /// Deletes the option.
    /// </summary>
    /// <param name="id">The id.</param>
    void DeleteOption(int id);

    /// <summary>
    /// Saves the option.
    /// </summary>
    /// <param name="option">The option.</param>
    void UpdateOption(SiteNavigation option);
  }
}
