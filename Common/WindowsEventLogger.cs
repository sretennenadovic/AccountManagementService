using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class WindowsEventLogger
    {
        public static void LogData(string message)
        {
            var parts = message.Split(',');

            if (!EventLog.SourceExists("AuditClientWEL"))
            {
                EventLog.CreateEventSource("AuditClientWEL", "AuditClientWELLog");
            }

            EventLog eventLog = new EventLog();

            eventLog.Source = "AuditClientWEL";
            if (parts[5].Equals("i"))
            {
                eventLog.WriteEntry(message.Substring(0, message.Length - 2), EventLogEntryType.Information, 101, 1);
            }
            else if (parts[5].Equals("e"))
            {
                eventLog.WriteEntry(message.Substring(0, message.Length - 2), EventLogEntryType.Error, 101, 1);
            }
        }
    }
}
