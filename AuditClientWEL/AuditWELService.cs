using AccountManagement;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AuditClientWEL
{

    public class AuditWELService : iPayment
    {
        Object obj = new Object();


        public bool AddAccount(string accountNumber)
        {
            //IIdentity i =  ServiceSecurityContext.Current.PrimaryIdentity;
            
            //var genericPrincipal = new GenericPrincipal(Thread.CurrentPrincipal.Identity, null);
            if (ServiceSecurityContext.Current.PrimaryIdentity.Name.Split('=')[2].Contains("AccountManagers"))
            {
                lock (obj)
                {
                    if (!Accounts.accounts.ContainsKey(accountNumber))
                    {
                        Accounts.accounts.Add(accountNumber, 0);
                        WindowsEventLogger.LogData(ServiceSecurityContext.Current.PrimaryIdentity.Name+","+DateTime.Now+",AddAccount,"+accountNumber+"-1,i");
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                throw new SecurityException("You don't have permission to add account.");
            }
        }

        public bool Delete(string accountNumber)
        {
            if (ServiceSecurityContext.Current.PrimaryIdentity.Name.Split('=')[2].Contains("AccountManagers"))
            {
                lock (obj)
                {
                    if (!Accounts.accounts.ContainsKey(accountNumber))
                    {
                        Accounts.accounts.Remove(accountNumber);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            } else
            {
                throw new SecurityException("You don't have permission to delete account.");
            }
        }

        public bool Pay(string accountNumber, double sum)
        {
            if (ServiceSecurityContext.Current.PrimaryIdentity.Name.Split('=')[2].Contains("AccountUsers"))
            {
                lock (obj)
                {
                    if(Accounts.accounts.ContainsKey(accountNumber))
                    {
                        Accounts.accounts[accountNumber] += sum;
                        return true;
                    }  
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                throw new SecurityException("You don't have permission to pay.");
            }
        }

        public bool PayOff(string accountNumber, double sum)
        {
            if (ServiceSecurityContext.Current.PrimaryIdentity.Name.Split('=')[2].Contains("AccountUsers"))
            {
                lock (obj)
                {
                    if (Accounts.accounts.ContainsKey(accountNumber))
                    {
                        Accounts.accounts[accountNumber] -= sum;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                throw new SecurityException("You don't have permission to pay off.");
            }
        }
    }
}
