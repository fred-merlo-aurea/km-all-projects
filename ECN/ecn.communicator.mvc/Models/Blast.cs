using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.communicator.mvc.Models
{
    public class Blast
    {
        public Blast()
        {

        }
        public string Time { get; set; }
        public string blast { get; set; }
        public int BlastID { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}