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
            int lastIndexAnalized = 0;
            while (true)
            {
                dataForAnalyzing.Clear();                   //clear dictionary
                Thread.Sleep(sleepTime);                    //sleep some time 

                if (EventLog.SourceExists("AuditServer"))   //this log file must exists
                {
                    EventLog eventLog = new EventLog();
                    eventLog.Log = "AuditServerLog";

                    for (int i = lastIndexAnalized+1; i < eventLog.Entries.Count; i++)
                    {
                        Console.WriteLine("{0}", eventLog.Entries[i].Message);  //here will be logic for new log file param[0] = A, param[1] = B, param[2] = C
                        lastIndexAnalized = i;              //next time this index will be different (we do not want to analize every time same data)
                    }
                    lastIndexAnalized++;
                }
            }
        }
    }
}
