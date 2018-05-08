using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Activity.Report
{
    [Serializable]
    [DataContract]
    public class LinkReportList
    {
        public LinkReportList() 
        { 
        }        

        public int BlastID { get; set; }
        public string emailsubject { get; set; }
        public string blastcategory { get; set; }
        public DateTime ActionDate { get; set; }
        public int linkownerIndexID { get; set; }
        public string linkownername { get; set; }
        public string linktype { get; set; }
        public string Alias { get; set; }
        public string Link { get; set; }
        public string ActionFrom { get; set; }
        public int clickcount { get; set; }
        public int Uclickcount { get; set; }
        public int viewcount { get; set; }

        public int sendcount { get; set; }
    }
}
