using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.communicator.mvc.Models
{
    public class ImportDataOptions
    {
        public string file { get; set; }
        public string ftc { get; set; }
        public string stc { get; set; }
        public string gid { get; set; }
        public string dupes { get; set; }
        public string fileType { get; set; }
        public string separator { get; set; }
        public string sheetName { get; set; }
        public string lineStart { get; set; }
        public string dl { get; set; }
        public string lblGUID { get; set; }
        public int numOfColumns { get; set; }
        public List<string> dropDownValues { get; set; }
    }
}