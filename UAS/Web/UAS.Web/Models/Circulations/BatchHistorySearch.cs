using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAS.Web.Models.Circulations
{
    public class BatchHistorySearch
    {
        public KeyValuePair<int, string> Product { get; set; }
        public KeyValuePair<int, string> User { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string BatchNumber { get; set; }
        public KeyValuePair<int, string> Client { get; set; }
    }
}