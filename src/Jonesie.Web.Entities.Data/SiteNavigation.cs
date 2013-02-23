using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jonesie.Web.Entities.Data
{
  [MetadataType(typeof(SiteNavigation_MetaData))]
  public partial class SiteNavigation 
  {

  }

  public class SiteNavigation_MetaData
  {
    [Display(Name= "Menu Group", Description="The menu group for this option")]
    [Required]
    public string MenuName { get; set; }

    [Display(Name = "Child Menu", Description = "The name of the child menu group for this option")]
    [Required]
    public string ChildMenuName { get; set; }

    [Display(Name="Order", Description="The sort order for menu options")]
    [Range(1, 99999, ErrorMessage="Order must be between 1 and 99999")]
    [Required]
    public int OptionOrder { get; set; }

    [Display(Name="Active", Description="Only Active options will be displayed in the menu.")]
    public bool Active { get; set; }

    [Display(Name="Caption", Description="The menu caption")]
    [Required]
    public string DisplayLabel { get; set; }

    [Display(Name="URL", Description="The URL for external web pages.  Either the URL or the Controller and Action must be specified")]
    public string Url { get; set; }

    [Display(Name="Controller", Description="")]
    public string Controller { get; set; }

    [Display(Name = "Action", Description = "")]
    public string Action { get; set; }

    [Display(Name = "Roles", Description = "")]
    public string Roles { get; set; }
  }
}
