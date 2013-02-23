using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Jonesie.Web.Entities.Data.Blog
{
  [Export]
  public class BlogPost : BaseEntity
  {
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]    
    public string Author { get; set; }

    [Required]
    public string Email { get; set; }

    public DateTimeOffset Posted { get; set; }

    [Required]
    public string Body { get; set; }

    public override string[] ColumnNames
    {
      get { return new string[] { "Title", "Author", "Email", "Posted", "Body" }; }
    }
  }
}
