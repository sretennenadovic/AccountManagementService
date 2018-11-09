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
            Console.WriteLine("1. Windows Event Logger");
            Console.WriteLine("2. XML Logger");
            Console.WriteLine("3. TXT Logger");
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
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://10.1.212.105:9999/Service"), new X509CertificateEndpointIdentity(srvCert));

            using (WCFClient proxy = new WCFClient(binding, address))
            {
                do
                {
                    Console.WriteLine("Choose one of the options: ");
                    Console.WriteLine("1. Pay");
                    Console.WriteLine("2. Pay off");
                    Console.WriteLine("3. Add account");
                    Console.WriteLine("4. Delete account");
                    string opt = Console.ReadLine();
                    int ammount = 0;

                    switch (opt)
                    {
                        case "1":
                            Console.WriteLine("Enter account number: ");
                            string accNum = Console.ReadLine();
                            Console.WriteLine("Enter amout: ");
                            try
                            {
                                ammount = Convert.ToInt32(Console.ReadLine());
                                
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("You must enter number value");
                            }
                            proxy.Pay(accNum, ammount);
                            break;
                        case "2":
                            Console.WriteLine("Enter account number: ");
                            accNum = Console.ReadLine();
                            Console.WriteLine("Enter amout: ");
                            
                            try
                            {
                                ammount = Convert.ToInt32(Console.ReadLine());
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("You must enter number value");
                            }
                            proxy.PayOff(accNum, ammount);
                            break;
                        case "3":
                            Console.WriteLine("Enter account number: ");
                            accNum = Console.ReadLine();

                            proxy.AddAccount(accNum);
                            break;
                        case "4":
                            Console.WriteLine("Enter account number: ");
                            accNum = Console.ReadLine();

                            proxy.Delete(accNum);
                            break;
                        default:
                            break;
                    }              
                } while (true);
            }

            Console.ReadKey();
        }
    }
}
