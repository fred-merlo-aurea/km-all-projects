using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECN_Framework_Entities.Accounts.View
{
    public class ProductActionInfo
    {
        public int UserActionID { get; set; }
        public int ActionID { get; set; }
        public string Active { get; set; }
        public int ProductID { get; set; }
        public string WebsiteAddress { get; set; }
        public string ProductName { get; set; }
        public string DisplayName { get; set; }
        public string ActionCode { get; set; }

        public ProductActionInfo(int userActionID,
                                 int actionID,
                                 string active,
                                 int productID,
                                 string websiteAddress,
                                 string productName,
                                 string displayName,
                                 string actionCode)
        {
            UserActionID = userActionID;
            ActionID = actionID;
            Active = active;
            ProductID = productID;
            WebsiteAddress = websiteAddress;
            ProductName = productName;
            DisplayName = displayName;
            ActionCode = actionCode;
        }
    }
}
