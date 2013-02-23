using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Jonesie.Web.Common
{
  public static class EnumExtensions
  {
    public static SelectList ToSelectList<TEnum>(this TEnum enumObj) where TEnum : struct, IConvertible
    {
      var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                   select new { Id = e.ToInt32(null), Name = e };

      return new SelectList(values, "Id", "Name", enumObj);
    }
  }
}
