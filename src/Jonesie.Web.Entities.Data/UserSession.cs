using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jonesie.Web.Entities.Data
{
  public class UserSession : BaseEntity
  {
    public string SessionId { get; set; }

    public string IPAddress { get; set; }

    public DateTimeOffset Started { get; set; }

    public DateTimeOffset LastRequest { get; set; }

    public static string[] ColumnNames
    {
      get { return new string[] { "IPAddress","Started","LastRequest" }; }
    }

    public string Started_UTC
    {
      get
      {
        return Started.ToString("r");
      }
    }

    public string LastRequest_UTC
    {
      get
      {
        return LastRequest.ToString("r");
      }
    }

    public string UserId { get; set; }
  }
}
