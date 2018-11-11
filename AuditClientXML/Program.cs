using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace AuditClientXML
{
    class Program
    {
        static void Main(string[] args)
        {
            int sleepTime = 0;

            WCFService service = new WCFService();
            Console.WriteLine("Server is started..");

            sleepTime = ConfigLoader.Config("../../../../Common/AuditClientConfig.xml");
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:22222/AuditServer";

            ServerProxy proxy = new ServerProxy(binding, address);

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
            while (true)
            {
                logs = String.Empty;
                Thread.Sleep(sleepTime * 1000);

                if (!File.Exists("AuditClientXMLLog"))
                {
                    continue;
                }

                XmlDocument xDocument = new XmlDocument();
                xDocument.Load("AuditClientXMLLog");
                XmlNodeList nodes = xDocument.SelectNodes("Logs/Log");

                for (int i = lastIndexSent; i < nodes.Count; i++)
                {
                    logs += nodes[i].ChildNodes[0].InnerText + "," +
                            nodes[i].ChildNodes[1].InnerText + "," +
                            nodes[i].ChildNodes[2].InnerText + "," +
                            nodes[i].ChildNodes[3].InnerText + "," +
                            nodes[i].ChildNodes[4].InnerText + "," +
                            nodes[i].ChildNodes[5].InnerText;

                    if(i != nodes.Count-1)
                    {
                        logs += "_";
                    }
                    lastIndexSent = i;
                }
                lastIndexSent++;
                proxy.SendLogs(logs);
            }
        }
    }
}
