using AccountManagement;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AuditClientTXT
{
    public class AuditTXTService : iPayment
    {
        Object obj = new Object();


        public bool AddAccount(string accountNumber)
        {
            if (ServiceSecurityContext.Current.PrimaryIdentity.Name.Split('=')[2].Contains("AccountManagers"))
            {
                lock (obj)
                {
                    if (!Accounts.accounts.ContainsKey(accountNumber))
                    {
                        Accounts.accounts.Add(accountNumber, 0);
                        TXTLogger.LogData(ServiceSecurityContext.Current.PrimaryIdentity.Name.Split(',')[0].Split('=')[1] + "," + System.DateTime.UtcNow.ToString() + ",AddAccount," + accountNumber + "," +"-1,i");
                        return true;
                    }
                    else
                    {
                        TXTLogger.LogData(ServiceSecurityContext.Current.PrimaryIdentity.Name.Split(',')[0].Split('=')[1] + "," + System.DateTime.UtcNow.ToString() + ",AddAccount," + accountNumber + "," + "-1,e");
                        return false;
                    }
                }
            }
            else
            {
                TXTLogger.LogData(ServiceSecurityContext.Current.PrimaryIdentity.Name.Split(',')[0].Split('=')[1] + "," + System.DateTime.UtcNow.ToString() + ",AddAccount," + accountNumber + "," + "-1,e");
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
                        TXTLogger.LogData(ServiceSecurityContext.Current.PrimaryIdentity.Name.Split(',')[0].Split('=')[1] + "," + System.DateTime.UtcNow.ToString() + ",Delete," + accountNumber + "," + "-1,i");
                        return true;
                    }
                    else
                    {
                        TXTLogger.LogData(ServiceSecurityContext.Current.PrimaryIdentity.Name.Split(',')[0].Split('=')[1] + "," + System.DateTime.UtcNow.ToString() + ",Delete," + accountNumber + "," + "-1,e");
                        return false;
                    }
                }
            }
            else
            {
                TXTLogger.LogData(ServiceSecurityContext.Current.PrimaryIdentity.Name.Split(',')[0].Split('=')[1] + "," + System.DateTime.UtcNow.ToString() + ",Delete," + accountNumber + "," + "-1,e");
                throw new SecurityException("You don't have permission to delete account.");
            }
        }

        public bool Pay(string accountNumber, double sum)
        {
            if (ServiceSecurityContext.Current.PrimaryIdentity.Name.Split('=')[2].Contains("AccountUsers"))
            {
                lock (obj)
                {
                    if (Accounts.accounts.ContainsKey(accountNumber))
                    {
                        Accounts.accounts[accountNumber] += sum;
                        TXTLogger.LogData(ServiceSecurityContext.Current.PrimaryIdentity.Name.Split(',')[0].Split('=')[1] + "," + System.DateTime.UtcNow.ToString() + ",Pay," + accountNumber + "," +sum.ToString()+",i");
                        return true;
                    }
                    else
                    {
                        TXTLogger.LogData(ServiceSecurityContext.Current.PrimaryIdentity.Name.Split(',')[0].Split('=')[1] + "," + System.DateTime.UtcNow.ToString() + ",Pay," + accountNumber + "," + sum.ToString() + ",e");
                        return false;
                    }
                }
            }
            else
            {
                TXTLogger.LogData(ServiceSecurityContext.Current.PrimaryIdentity.Name.Split(',')[0].Split('=')[1] + "," + System.DateTime.UtcNow.ToString() + ",Pay," + accountNumber + "," + sum.ToString() + ",e");
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
                        TXTLogger.LogData(ServiceSecurityContext.Current.PrimaryIdentity.Name.Split(',')[0].Split('=')[1] + "," + System.DateTime.UtcNow.ToString() + ",PayOff," + accountNumber + "," + sum.ToString() + ",i");
                        return true;
                    }
                    else
                    {
                        TXTLogger.LogData(ServiceSecurityContext.Current.PrimaryIdentity.Name.Split(',')[0].Split('=')[1] + "," + System.DateTime.UtcNow.ToString() + ",PayOff," + accountNumber + "," + sum.ToString() + ",e");
                        return false;
                    }
                }
            }
            else
            {
                TXTLogger.LogData(ServiceSecurityContext.Current.PrimaryIdentity.Name.Split(',')[0].Split('=')[1] + "," + System.DateTime.UtcNow.ToString() + ",PayOff," + accountNumber + "," + sum.ToString() + ",e");
                throw new SecurityException("You don't have permission to pay off.");
            }
        }
    }
}
