using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace AuditClientWEL
{
    class Program
    {
        static void Main(string[] args)
        {
            int sleepTime = 0;

            WCFService service = new WCFService();
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.None;
            string address = "net.tcp://localhost:22222/AuditServer";

            ServerProxy proxy = new ServerProxy(binding, address);

            Console.WriteLine("Server is started..");

            sleepTime = ConfigLoader.Config("../../../../Common/AuditClientConfig.xml");

            Task t = new Task(() => Action(sleepTime, proxy));
            t.Start();
            
            Console.ReadKey();

            service.CloseHost();
            t.Dispose();
        }

        private static void Action(int sleepTime, ServerProxy proxy)
        {
            int lastIndexSent = 0;
            string logs = String.Empty;
            while (true)
            {
                logs = String.Empty;
                Thread.Sleep(sleepTime * 1000);

                if (EventLog.SourceExists("AuditClientWEL"))   //this log file must exists
                {
                    EventLog eventLog = new EventLog();
                    eventLog.Log = "AuditClientWELLog";
                    if (lastIndexSent < eventLog.Entries.Count)
                    {
                        if (eventLog.Entries[lastIndexSent].EntryType == EventLogEntryType.Information)
                        {
                            logs = eventLog.Entries[lastIndexSent].Message + ",i";
                        }
                        else
                        {
                            logs = eventLog.Entries[lastIndexSent].Message + ",e";
                        }

                        for (int i = lastIndexSent + 1; i < eventLog.Entries.Count; i++)
                        {
                            if (eventLog.Entries[i].EntryType == EventLogEntryType.Information)
                            {
                                logs += ("_" + eventLog.Entries[i].Message + ",i");
                            }
                            else
                            {
                                logs += ("_" + eventLog.Entries[i].Message + ",e");
                            }

                            lastIndexSent = i;              //next time this index will be different (we do not want to send every time same data)
                        }
                        lastIndexSent++;
                        proxy.SendLogs(logs);
                    }
                    else if(eventLog.Entries.Count != 0 && eventLog.Entries.Count < lastIndexSent)
                    {
                        lastIndexSent = 0;

                        if (eventLog.Entries[lastIndexSent].EntryType == EventLogEntryType.Information)
                        {
                            logs = eventLog.Entries[lastIndexSent].Message + ",i";
                        }
                        else
                        {
                            logs = eventLog.Entries[lastIndexSent].Message + ",e";
                        }

                        for (int i = lastIndexSent + 1; i < eventLog.Entries.Count; i++)
                        {
                            if (eventLog.Entries[i].EntryType == EventLogEntryType.Information)
                            {
                                logs += ("_" + eventLog.Entries[i].Message + ",i");
                            }
                            else
                            {
                                logs += ("_" + eventLog.Entries[i].Message + ",e");
                            }

                            lastIndexSent = i;              //next time this index will be different (we do not want to send every time same data)
                        }
                        lastIndexSent++;
                        proxy.SendLogs(logs);
                    }
                    else if (eventLog.Entries.Count == 0)
                    {
                        lastIndexSent = 0;
                    }
                }
            }
        }
    }
}
