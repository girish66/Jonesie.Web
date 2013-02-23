using Jonesie.Web.Common;
using Jonesie.Web.Contracts.Core;
using Jonesie.Web.Entities.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jonesie.Web.Module.JonesieHome.Models
{
  public class BroadcastMessageViewModel : BaseViewModel
  {

    [Required(ErrorMessage="Message cannot be blank.")]
    public string Message { get; set; }

    public WebNotificationType Type { get; set; }

    public SelectList TypeList
    {
      get
      {
        return Type.ToSelectList();
      }
    }


    public BroadcastMessageViewModel()
      : base("Broadcast", "Broadcast")
    {

    }
  }
}