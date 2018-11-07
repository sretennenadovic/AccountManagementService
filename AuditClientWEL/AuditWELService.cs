using AccountManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AuditClientWEL
{

    public class AuditWELService : iPayment
    {

        public void AddAccount()
        {
            Console.WriteLine("Primiri der strasti...");
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void Pay(double accountNumber, double sum)
        {
            throw new NotImplementedException();
        }

        public void PayOff(double accountNumber, double sum)
        {
            throw new NotImplementedException();
        }
    }
}
