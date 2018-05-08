using System.Collections.Generic;

namespace ECN_Framework.Accounts.Report
{
    public interface INewCustomerProxy
    {
        IList<NewCustomer> Get(int month, int year, bool isTestBlast);
    }
}
