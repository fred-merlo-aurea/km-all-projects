using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.qatools.Models
{
    public class IndexViewModel
    {
        public IEnumerable<KeyValuePair<string, string>> Params { get; set; }
        public IEnumerable<string> Saved { get; set; }
    }
}