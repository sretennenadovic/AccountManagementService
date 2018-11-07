using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AccountManagement
{
    [ServiceContract]
    public interface iPayment
    {
        //this methods are used from clients to manipulate their accounts
        [OperationContract]
        void Pay(double accountNumber,double sum);
        [OperationContract]
        void PayOff(double accountNumber, double sum);
        [OperationContract]
        void AddAccount();
        [OperationContract]
        void Delete();
    }
}
