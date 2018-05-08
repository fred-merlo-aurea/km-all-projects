using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailMarketing.API.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int? BaseChannelID { get; set; }
        public Guid APIAccessKey { get; set; }
    }
}