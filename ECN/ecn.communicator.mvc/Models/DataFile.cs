using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.communicator.mvc.Models
{
    public class DataFile
    {
        public string FileName { get; set; }
        public long Size { get; set; }
        public DateTime Date { get; set; }
    }
}