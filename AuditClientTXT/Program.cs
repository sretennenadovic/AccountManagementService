using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            sleepTime = ConfigLoader.Config("../../../../Common/AuditClientConfig.xml");

            Console.ReadKey();
        }
    }
}
