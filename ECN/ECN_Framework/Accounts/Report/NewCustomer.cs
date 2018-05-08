using System;
using System.Collections.Generic;

namespace ECN_Framework.Accounts.Report
{
    [Serializable]
    public class NewCustomer : ReportBase<NewCustomer>
    {
        public string ContactName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        private const string ReportName = "rpt_NewCustomerreport";

        public static IList<NewCustomer> Get(int month, int year, bool testBlast)
        {
            return Get(month, year, testBlast, ReportName);
        }
    }
}