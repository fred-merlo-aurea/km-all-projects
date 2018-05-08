using System;
using System.Collections.Generic;

namespace ECN_Framework.Accounts.Report
{
    [Serializable]
    public class DigitalEditionBillingProxy : IDigitalEditionBillingProxy
    {
        public IList<DigitalEditionBilling> Get(int month, int year)
        {
            return DigitalEditionBilling.Get(month, year);
        }
    }
}
