using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.domaintracking.Models
{
    public class Customer
    {
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string AccessKey { get; set; }
        public bool hasChildren { get { return true; } }
    }
}