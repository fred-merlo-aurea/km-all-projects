using System.Collections.Generic;

namespace ECN_Framework.Accounts.Report
{
    public interface IDigitalEditionBillingProxy
    {
        IList<DigitalEditionBilling> Get(int month, int year);
    }
}
