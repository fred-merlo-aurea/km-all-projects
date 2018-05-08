using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaidPub.Objects
{
    public class Item
    {
      
        public int ItemID { get; set; }

        public string ItemCode { get; set; }

        public string ItemDescription { get; set; }

        public string ItemName { get; set; }

        public string ItemAmount { get; set; }

        public string ItemQty { get; set; }

        public int CustID { get; set; }

        public int GroupID { get; set; }
    }
}
