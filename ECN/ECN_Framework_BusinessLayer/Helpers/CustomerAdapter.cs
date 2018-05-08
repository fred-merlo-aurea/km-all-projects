using System.Collections.Generic;
using ECN.Framework.BusinessLayer.Interfaces;
using ECN_Framework_Entities.Accounts;

namespace ECN.Framework.BusinessLayer.Helpers
{
    public class CustomerAdapter : ICustomer
    {
        public bool Exists(int customerID)
        {
            return ECN_Framework_BusinessLayer.Accounts.Customer.Exists(customerID);
        }

        public IList<Customer> GetByBaseChannelID(int baseChannelID)
        {
            return ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(baseChannelID);
        }

        public Customer GetByCustomerID(int customerID, bool getChildren)
        {
            return ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(customerID, getChildren);
        }
    }
}
