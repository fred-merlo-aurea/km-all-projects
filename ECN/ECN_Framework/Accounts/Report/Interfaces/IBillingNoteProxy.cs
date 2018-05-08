using System.Collections.Generic;

namespace ECN_Framework.Accounts.Report
{
    public interface IBillingNoteProxy
    {
        IList<BillingNote> GetAll();
    }
}
