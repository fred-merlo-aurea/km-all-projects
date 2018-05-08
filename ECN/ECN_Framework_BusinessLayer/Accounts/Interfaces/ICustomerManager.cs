using KMPlatform.Entity;

namespace ECN_Framework_BusinessLayer.Accounts.Interfaces
{
    public interface ICustomerManager
    {
        bool Exists(int customerId);
        bool Exists(string customerName, int customerId);
        ECN_Framework_Entities.Accounts.Customer GetByCustomerId(int customerId, bool getChildren);
        ECN_Framework_Entities.Accounts.Customer GetByCustomerId(int customerId, User user, bool getChildren);
        ECN_Framework_Entities.Accounts.Customer GetByClientID(int clientId, bool getChildren);
    }
}