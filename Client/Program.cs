using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            string option;
            string srvCertCN = String.Empty;
            Console.WriteLine("Choose one option:");
            option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    srvCertCN = "AMSWEL";
                    break;
                case "2":
                    srvCertCN = "AMSXML";
                    break;
                case "3":
                    srvCertCN = "AMSTXT";
                    break;
            }

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            
            /// Use CertManager class to obtain the certificate based on the "srvCertCN" representing the expected service identity.
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://10.1.212.152:9999/AuditWELService"), new X509CertificateEndpointIdentity(srvCert));

            using (WCFClient proxy = new WCFClient(binding, address))
            {
                proxy.AddAccount("1");
            }

            Console.ReadKey();
        }
    }
}
