using System;
using System.Collections.Generic;

namespace ECN_Framework.Accounts.Report
{
    [Serializable]
    public class NewCustomerProxy : INewCustomerProxy
    {
        public IList<NewCustomer> Get(int month, int year, bool isTestBlast)
        {
            return NewCustomer.Get(month, year, isTestBlast);
        }
    }
}
