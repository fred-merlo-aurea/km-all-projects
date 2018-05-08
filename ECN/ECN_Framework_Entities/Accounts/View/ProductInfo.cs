using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECN_Framework_Entities.Accounts.View
{
    public class ProductInfo
    {
        public int ProductID { get; set; }

        public ProductInfo(int productID)
        {
            ProductID = productID;
        }
    }
}
