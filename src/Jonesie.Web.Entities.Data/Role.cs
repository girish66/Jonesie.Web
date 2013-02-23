using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jonesie.Web.Entities.Data
{
  public partial class Role
  {

    public List<RoleActionMap> RoleActionMaps { get; private set; }

    public Role()
    {
      RoleActionMaps = new List<RoleActionMap>();
    }
  }
}
