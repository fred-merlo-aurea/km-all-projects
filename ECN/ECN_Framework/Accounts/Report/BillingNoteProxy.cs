using System;
using System.Collections.Generic;

namespace ECN_Framework.Accounts.Report
{
    [Serializable]
    public class BillingNoteProxy : IBillingNoteProxy
    {
        public IList<BillingNote> GetAll()
        {
            return BillingNote.GetAll();
        }
    }
}
