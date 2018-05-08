using ECN_Framework_Entities.Accounts;

namespace ecn.activityengines.Tests.Setup.Interfaces
{
    public interface ICustomerBusiness
    {
        Customer GetByCustomerID(int customerId, bool getChildren);
    }
}
