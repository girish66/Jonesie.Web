using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jonesie.Web.Common
{
    public static class StringExtensions
    {
        public static string FormatWith(this string s, params object[] parms)
        {
            return string.Format(s, parms);
        }
    }
}
