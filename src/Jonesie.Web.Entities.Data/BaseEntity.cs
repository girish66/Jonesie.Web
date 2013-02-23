using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jonesie.Web.Entities.Data
{
  /// <summary>
  /// A base type for all entities.
  /// </summary>
  public abstract class BaseEntity
  {
    public byte[] row_version { get; set; }
  }
}
