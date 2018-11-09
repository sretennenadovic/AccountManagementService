using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AuditClientWEL
{
    public class ServerProxy : ChannelFactory<IAuditService>, IAuditService, IDisposable
    {
        IAuditService factory;

        public ServerProxy(NetTcpBinding binding, string address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public void SendLogs(string logs)
        {
            try
            {
                factory.SendLogs(logs);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
        }
    }
}

