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
        bool Pay(string accountNumber,double sum);
        [OperationContract]
        bool PayOff(string accountNumber, double sum);
        [OperationContract]
        bool AddAccount(string accountNuber);
        [OperationContract]
        bool Delete(string accountNuber);
    }
}
