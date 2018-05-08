using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Activity.Report
{
    [Serializable]
    [DataContract]
    public class EmailFatigueReport
    {
        public EmailFatigueReport(){}

        public int Grouping { get; set; }
        public string Action{get;set;}
        public int Touch1 { get; set; }
        public int Touch2{get;set;}
        public int Touch3 { get; set; }
        public int Touch4 { get; set; }
        public int Touch5 { get; set; }
        public int Touch6 { get; set; }
        public int Touch7 { get; set; }
        public int Touch8 { get; set; }
        public int Touch9 { get; set; }
        public int Touch10 { get; set; }
        public int Touch11_20 { get; set; }
        public int Touch21_30 { get; set; }
        public int Touch31_40 { get; set; }
        public int Touch41_50 { get; set; }
        public int Touch51Plus { get; set; }
    }

    [Serializable]
    [DataContract]
    public class EmailFatigueReportPercent
    {
        public EmailFatigueReportPercent() { }

        public int Grouping { get; set; }
        public string Action { get; set; }
        public string Touch1 { get; set; }
        public string Touch2 { get; set; }
        public string Touch3 { get; set; }
        public string Touch4 { get; set; }
        public string Touch5 { get; set; }
        public string Touch6 { get; set; }
        public string Touch7 { get; set; }
        public string Touch8 { get; set; }
        public string Touch9 { get; set; }
        public string Touch10 { get; set; }
        public string Touch11_20 { get; set; }
        public string Touch21_30 { get; set; }
        public string Touch31_40 { get; set; }
        public string Touch41_50 { get; set; }
        public string Touch51Plus { get; set; }
    }
}
