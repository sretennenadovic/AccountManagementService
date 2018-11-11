using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AuditClientTXT
{
    class Program
    {
        static void Main(string[] args)
        {
            int sleepTime = 0;

            WCFService service = new WCFService();
            Console.WriteLine("Server is started..");

            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:22222/AuditServer";

            ServerProxy proxy = new ServerProxy(binding, address);

            sleepTime = ConfigLoader.Config("../../../../Common/AuditClientConfig.xml");

            Task t = new Task(() => Action(sleepTime, proxy));
            t.Start();

            Console.ReadKey();

            t.Dispose();
            service.CloseHost();
        }

        private static void Action(int sleepTime, ServerProxy proxy)
        {
            int lastIndexSent = 0;
            string logs = String.Empty;
            List<string> Logs = new List<string>();
            while (true)
            {
                logs = String.Empty;
                Logs.Clear();
                Thread.Sleep(sleepTime * 1000);

                if (!File.Exists("AuditClientTXTLog"))
                {
                    continue;
                }

                using (var sr = new StreamReader("AuditClientTXTLog"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Logs.Add(line);
                    }
                }

                for (int i = lastIndexSent; i < Logs.Count; i++)
                {
                    var parts = Logs[i].Split(',', '=');
                    for (int j = 1; j < parts.Length; j += 2)
                    {
                        if (j == parts.Length)
                        {
                            if (Logs[i] != Logs.Last())
                            {
                                logs += parts[j] + '_';
                            }
                            else
                            {
                                logs += parts[j];
                            }
                        }
                        else
                        {
                            logs += parts[j] + ',';
                        }
                    }
                    lastIndexSent = i;
                }
                lastIndexSent++;
                proxy.SendLogs(logs);
            }
        }
    }
}
