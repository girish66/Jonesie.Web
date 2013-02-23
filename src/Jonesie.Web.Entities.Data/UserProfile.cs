using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Jonesie.Web.Entities.Data
{
  [MetadataType(typeof(UserProfile_MetaData))]
  public partial class UserProfile : BaseEntity
  {
    [Display(Name = "Roles")]
    public List<Role> Roles { get; set; }

    public UserProfile()
    {
      Roles = new List<Role>();
    }
  }

  public class UserProfile_MetaData
  {
    [Display(Name="User Name")]
    [Required(ErrorMessage="A unique user name is required")]
    public string UserName { get; set; }

    [Display(Name="Account Created")]
    [DisplayFormat(DataFormatString="ddd MMM yyyy HH:mm z")]
    public DateTimeOffset Created { get; set; }

    [Display(Name="Last Login")]
    [DisplayFormat(DataFormatString = "ddd MMM yyyy HH:mm z")]
    public DateTimeOffset? LastLogin { get; set; }

  }
}
