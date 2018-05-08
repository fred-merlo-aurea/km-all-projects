using System;
using ECN_Framework_BusinessLayer.Accounts.Interfaces;
using KMPlatform.Entity;
using AccountEntities = ECN_Framework_Entities.Accounts;

namespace ECN_Framework_BusinessLayer.Accounts
{
    [Serializable]
    public class CustomerManager : ICustomerManager
    {
        public bool Exists(int customerId)
        {
            return Customer.Exists(customerId);
        }

        public bool Exists(string customerName, int customerId)
        {
            return Customer.Exists(customerName, customerId);
        }

        public AccountEntities.Customer GetByCustomerId(int customerId, bool getChildren)
        {
            return Customer.GetByCustomerID(customerId, getChildren);
        }

        public AccountEntities.Customer GetByCustomerId(int customerId, User user, bool getChildren)
        {
            return Customer.GetByCustomerID(customerId, user, getChildren);
        }
        
        public AccountEntities.Customer GetByClientID(int clientId, bool getChildren)
        {
            return Customer.GetByClientID(clientId, getChildren);
        }
     }
}
