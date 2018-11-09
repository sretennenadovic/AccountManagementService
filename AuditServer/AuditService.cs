using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditServer
{
    public class AuditService : IAuditService
    {
        Object o = new Object();
        public void SendLogs(string logs)
        {
            var messages = logs.Split('_');
            int i = 0;

            foreach (var item in messages)
            {
                var parts = item.Split(',');

                if (!EventLog.SourceExists("AuditServer"))
                {
                    EventLog.CreateEventSource("AuditServer", "AuditServerLog");
                }

                EventLog eventLog = new EventLog();

                eventLog.Source = "AuditServer";
                if (parts[5].Equals("i"))
                {
                    lock (o)
                    {
                        eventLog.WriteEntry(messages[i].Substring(0, messages[i].Length - 2), EventLogEntryType.Information, 101, 1);
                    }
                }
                else if (parts[5].Equals("e"))
                {
                    lock (o)
                    {
                        eventLog.WriteEntry(messages[i].Substring(0, messages[i].Length - 2), EventLogEntryType.Error, 101, 1);
                    }
                }
                i++;
            }
        }
    }
}
