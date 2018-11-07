using AccountManagement;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class WCFClient : ChannelFactory<iPayment>, iPayment, IDisposable
    {
        iPayment factory;

        public WCFClient(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            /// cltCertCN.SubjectName should be set to the client's username. .NET WindowsIdentity class provides information about Windows user running the given process
            string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            //this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
            //this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            /// Set appropriate client's certificate on the channel. Use CertManager class to obtain the certificate based on the "cltCertCN"
            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

            factory = this.CreateChannel();
        }

        public bool AddAccount(string s)
        {
            bool retVal = false;
            try
            {
                retVal = factory.AddAccount("1");
            }catch(Exception e)
            {
                Console.WriteLine("[AddAccount] ERROR = {0}", e.Message);
            }
            return retVal;
        }

        public bool Delete(string accountNumber)
        {
            bool retVal = false;
            try
            {
                retVal = factory.Delete("1");
            }
            catch (Exception e)
            {
                Console.WriteLine("[Delete] ERROR = {0}", e.Message);
            }
            return retVal;
        }

        public bool Pay(string accountNumber, double sum)
        {
            bool retVal = false;
            try
            {
                retVal = factory.Pay(accountNumber,sum);
            }
            catch (Exception e)
            {
                Console.WriteLine("[Pay] ERROR = {0}", e.Message);
            }
            return retVal;
        }

        public bool PayOff(string accountNumber, double sum)
        {
            bool retVal = false;
            try
            {
                retVal = factory.PayOff(accountNumber, sum);
            }
            catch (Exception e)
            {
                Console.WriteLine("[PayOff] ERROR = {0}", e.Message);
            }
            return retVal;
        }

        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }

            this.Close();
        }
    }
}
