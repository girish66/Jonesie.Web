using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jonesie.Web.Entities.Data
{

  [MetadataType(typeof(RoleActionMap_MetaData))]
  public partial class RoleActionMap
  {
    public string RoleName { get; set; }
  }

  public class RoleActionMap_MetaData
  {
    [Required]
    public string Path { get; set; }
  }
}
