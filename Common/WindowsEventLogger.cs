using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class WindowsEventLogger
    {
        public static void LogData(string message)
        {
            var parts = message.Split(',');

            using (EventLog eventLog = new EventLog("AuditClientWELLogger"))
            {
                eventLog.Source = "AuditClientWELL";
                if (parts[6].Equals("i"))
                {
                    eventLog.WriteEntry(message.Substring(0, message.Length - 2), EventLogEntryType.Information, 101, 1);
                }else if (parts[6].Equals("e"))
                {
                    eventLog.WriteEntry(message.Substring(0, message.Length - 2), EventLogEntryType.Error, 101, 1);
                }
            }
        }
    }
}
