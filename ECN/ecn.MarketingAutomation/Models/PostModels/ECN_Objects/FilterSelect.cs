using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.MarketingAutomation.Models.PostModels.ECN_Objects
{
    public class FilterSelect
    {
        public int CustomerID { get; set; }
        public int GroupID { get; set; }
        public string FilterName { get; set; }
        public int FilterID { get; set; }
       
    }
}