using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.MarketingAutomation.Models.PostModels.ECN_Objects
{
    public class CustomerSelect
    {
        public int CustomerID { get; set; }

        public string CustomerName { get; set; }

        public int PlatformClientID { get; set; }
    }
}