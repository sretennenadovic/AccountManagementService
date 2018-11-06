using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditClientWEL
{
    class Program
    {
        static void Main(string[] args)
        {
            WCFService service = new WCFService();
            Console.WriteLine("Server is started..");
            Console.ReadKey();
        }
    }
}
