using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AuditServer
{
    class Program
    {
        static void Main(string[] args)
        {
            int sleepTime = 0;
            sleepTime = ConfigLoader.Config("../../../../Common/AuditServerConfig.xml");

            int[] param = new int[3];
            param = ConfigLoader.ServerAnalizeParamsRead("../../../../Common/ServerParams.xml");

            Task t = new Task(() => Analize(sleepTime,param));
            t.Start();

            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:22222/AuditServer";

            ServiceHost host = new ServiceHost(typeof(AuditService));
            host.AddServiceEndpoint(typeof(IAuditService),binding,address);

            host.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            host.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            host.Open();

            Console.WriteLine("Audit service is started..");
            Console.ReadLine();
            t.Dispose();
            host.Close();
        }

        private static void Analize(int sleepTime,int[] param)
        {
            Dictionary<string, int[]> dataForAnalyzing = new Dictionary<string, int[]>();
            List<string> logsForAnalizing = new List<string>();
            int lastIndexAnalized = 0;
            while (true)
            {
                dataForAnalyzing.Clear();
                logsForAnalizing.Clear();
                Thread.Sleep(sleepTime * 1000);

                if (!EventLog.SourceExists("AuditServer"))
                {
                    continue;
                }

                EventLog eventLog = new EventLog();
                eventLog.Log = "AuditServerLog";
                if (lastIndexAnalized < eventLog.Entries.Count)
                {
                    for (int i = lastIndexAnalized + 1; i < eventLog.Entries.Count; i++)
                    {
                        if (eventLog.Entries[i].EntryType == EventLogEntryType.Information)
                        {
                            logsForAnalizing.Add(eventLog.Entries[i].Message + ",i");
                        }
                        else
                        {
                            logsForAnalizing.Add(eventLog.Entries[i].Message + ",e");
                        }

                        lastIndexAnalized = i;              //next time this index will be different (we do not want to send every time same data)
                    }
                    lastIndexAnalized++;
                }
                else if (eventLog.Entries.Count != 0 && eventLog.Entries.Count < lastIndexAnalized)
                {
                    lastIndexAnalized = 0;

                    for (int i = lastIndexAnalized + 1; i < eventLog.Entries.Count; i++)
                    {
                        if (eventLog.Entries[i].EntryType == EventLogEntryType.Information)
                        {
                            logsForAnalizing.Add(eventLog.Entries[i].Message + ",i");
                        }
                        else
                        {
                            logsForAnalizing.Add(eventLog.Entries[i].Message + ",e");
                        }

                        lastIndexAnalized = i;              //next time this index will be different (we do not want to send every time same data)
                    }
                    lastIndexAnalized++;
                }
                else if (eventLog.Entries.Count == 0)
                {
                    lastIndexAnalized = 0;
                }

                //now we have to analyze list and analyzed data put to dictionary
                foreach (var item in logsForAnalizing)
                {
                    if (!dataForAnalyzing.ContainsKey(item.Split(',')[3]) && item.Split(',')[2].Equals("PayOff"))
                    {
                        dataForAnalyzing.Add(item.Split(',')[3], new int[2] { 0, 0 });
                        if (item.Split(',')[5].Equals("i"))
                        {
                            dataForAnalyzing[item.Split(',')[3]][0]++;
                        }
                        else if (item.Split(',')[5].Equals("e"))
                        {
                            dataForAnalyzing[item.Split(',')[3]][1]++;
                        }

                        if(item.Split(',')[5].Equals("i") && Int32.Parse(item.Split(',')[4]) > param[2])
                        {
                            if (!EventLog.SourceExists("AuditServerAnalyze"))
                            {
                                EventLog.CreateEventSource("AuditServerAnalyze", "AuditServerAnalyzeLog");
                            }

                            EventLog Log = new EventLog();
                            Log.Source = "AuditServerAnalyze";

                            Log.WriteEntry("From account Number: "+item.Split(',')[3]+", is paid off more than "+param[2].ToString()+" at: "+item.Split(',')[1], EventLogEntryType.Warning, 101, 1);
                        }
                    }
                    else if(dataForAnalyzing.ContainsKey(item.Split(',')[3]) && item.Split(',')[2].Equals("PayOff"))
                    {
                        if (item.Split(',')[5].Equals("i"))
                        {
                            dataForAnalyzing[item.Split(',')[3]][0]++;
                        }
                        else if (item.Split(',')[5].Equals("e"))
                        {
                            dataForAnalyzing[item.Split(',')[3]][1]++;
                        }
                    } 
                }

                //writing to log if its necessary
                foreach (var item in dataForAnalyzing)
                {
                    if (!EventLog.SourceExists("AuditServerAnalyze"))
                    {
                        EventLog.CreateEventSource("AuditServerAnalyze", "AuditServerAnalyzeLog");
                    }

                    if (item.Value[0] > param[0])
                    {
                        EventLog Log = new EventLog();
                        Log.Source = "AuditServerAnalyze";

                        Log.WriteEntry("From account Number: " +item.Key + ", is paid off "+ item.Value[0].ToString()+ "successfully and that is more than " + param[0].ToString() + ",for the last "+sleepTime.ToString()+" sec/min.", EventLogEntryType.Information, 101, 1);
                    }

                    if(item.Value[1] > param[1])
                    {
                        EventLog Log = new EventLog();
                        Log.Source = "AuditServerAnalyze";

                        Log.WriteEntry("From account Number: " + item.Key + ", is paid off " + item.Value[1].ToString() + "unsuccessfully and that is more than " + param[1].ToString() + ",for the last " + sleepTime.ToString() + " sec/min.", EventLogEntryType.Information, 101, 1);
                    }
                }
            }
        }
    }
}
