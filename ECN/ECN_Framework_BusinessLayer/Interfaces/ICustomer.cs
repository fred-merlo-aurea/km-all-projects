using System.Collections.Generic;
using ECN_Framework_Entities.Accounts;

namespace ECN.Framework.BusinessLayer.Interfaces
{
    public interface ICustomer
    {
        Customer GetByCustomerID(int customerID, bool getChildren);
        IList<Customer> GetByBaseChannelID(int baseChannelID);
        bool Exists(int customerID);
    }
}
