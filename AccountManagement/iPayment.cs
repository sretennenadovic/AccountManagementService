using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManagement
{
    public interface iPayment
    {
        //methods for clients
        void Pay(double accountNumber,double sum);
        void PayOff(double accountNumber, double sum);
    }
}
