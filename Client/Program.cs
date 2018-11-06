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
            switch(option)
            {
                case "1":
                    srvCertCN = "AMSWEL";
                    break;

                case "2":
                    srvCertCN = "AMSXML";
                    break;
            }

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            /// Use CertManager class to obtain the certificate based on the "srvCertCN" representing the expected service identity.
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9999/Receiver"), new X509CertificateEndpointIdentity(srvCert));
        }
    }
}
