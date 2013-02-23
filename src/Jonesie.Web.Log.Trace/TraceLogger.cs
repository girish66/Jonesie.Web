using Jonesie.Web.Contracts.Core;
using Jonesie.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jonesie.Web.Log.Trace
{
    public class TraceLogger : ILogger
    {
      int _indentLevel = 0;

      public void LogDebug(string message)
      {
        System.Diagnostics.Trace.WriteLine(makeMessage("DEBUG", message));
      }

      public void LogInfo(string message)
      {
        System.Diagnostics.Trace.WriteLine(makeMessage("INFO", message));
      }

      public void LogWarning(string message)
      {
        System.Diagnostics.Trace.WriteLine(makeMessage("WARNING", message));
      }

      public void LogError(string message)
      {
        System.Diagnostics.Trace.WriteLine(makeMessage("ERROR", message));
      }

      public void LogError(string message, Exception ex)
      {
        System.Diagnostics.Trace.WriteLine(makeMessage("ERROR", message));
        System.Diagnostics.Trace.WriteLine(ex);
      }


      public void Indent()
      {
        _indentLevel += 1;
      }

      public void UnIndent()
      {
        _indentLevel = Math.Max(0, --_indentLevel);
      }

      private string makeMessage(string type, string message)
      {
        return "{0} :: {1}: {3}{2}".FormatWith(DateTime.Now.ToString("HH:mm:ss.ff"), type, message, new String(' ', _indentLevel));
      }
    }
}
