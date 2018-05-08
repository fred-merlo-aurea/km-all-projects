using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAS.Web.Models.Circulations
{
    public class BatchHistory
    {
        public int BatchID { get; set; }
        public KeyValuePair<int, string> Product { get; set; }
        public string BatchNumber { get; set; }
        public KeyValuePair<int, string> User { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateFinalized { get; set; }
        public string BatchCount { get; set; }
    }    
}