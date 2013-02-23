using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Jonesie.Web.Utility
{

    [AttributeUsage(AttributeTargets.Method)]
    public class ActionBreadcrumbAttribute : Attribute
    {
       // public Func<ActionResult> Action { get; set; }
        public string Action { get; set; }

        public ActionBreadcrumbAttribute(string action)
        {
            Action = action;
        }
    }
}
