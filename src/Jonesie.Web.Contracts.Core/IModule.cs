using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jonesie.Web.Contracts.Core
{
  public interface IModule
  {
    /// <summary>
    /// Initialize internal fluff.
    /// </summary>
    void InternalInit();
  }
}
