using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jonesie.Web.Entities.Core
{
    public class BaseViewModel
    {

        public string Entity { get; protected set; }
        public string EntityLabel { get; protected set; }
        public string Controller { get; set; }
        public bool IsNew { get; set; }
        public bool Delete { get; set; }

        public BaseViewModelToolbarEnum ToolbarButtons { get; set; }

        public BaseViewModel(string entity, string entityLabel)
        {
            Entity = entity;
            EntityLabel = entityLabel;
        }

        public BaseViewModel(string entity, string entityLabel, BaseViewModelToolbarEnum buttons)
            : this(entity, entityLabel)
        {
            ToolbarButtons = buttons;
        }
    }

    [Flags]
    public enum BaseViewModelToolbarEnum
    {
        None = 0,
        Add = 1,
        Edit = 2,
        Delete = 4,
        Refresh = 8
    }
}
