using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Accounts.Report
{
    [Serializable]
    [DataContract]
    public class ChannelReport
    {
        public ChannelReport() 
        { 
        }

        public int BasechannelID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerType { get; set; }
        public int cCount { get; set; }
        public int uCount { get; set; }
        public int mtdcount { get; set; }
        public int ytdcount { get; set; }
        public int mtdsent { get; set; }
        public int ytdsent { get; set; }
    }
}
