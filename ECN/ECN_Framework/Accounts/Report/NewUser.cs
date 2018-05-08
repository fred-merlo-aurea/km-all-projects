using System;
using System.Collections.Generic;

namespace ECN_Framework.Accounts.Report
{
    [Serializable]
    public class NewUser : ReportBase<NewUser>
    {
        public string UserName { get; set; }

        private const string ReportName = "rpt_NewUserreport";

        public static IList<NewUser> Get(int month, int year, bool testBlast)
        {
            return Get(month, year, testBlast, ReportName);
        }
    }
}