using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAS.Web.Models.Circulations
{
    public class BatchHistoryWithName
    {
        public string BatchHistoryName { get; set; }
        public IEnumerable<BatchHistory> BatchHistoryIEnum { get; set; }
        public List<KeyValuePair<int, string>> Users { get; set; }
    }
}