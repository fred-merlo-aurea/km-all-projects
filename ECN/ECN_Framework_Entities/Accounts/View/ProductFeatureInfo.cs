using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECN_Framework_Entities.Accounts.View
{
    public class ProductFeatureInfo
    {
        public int UserActionID { get; set; }
        public int ActionID { get; set; }
        public string Active { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string DisplayName { get; set; }
        public string ActionCode { get; set; }
        public int? DisplayOrder { get; set; }

        public ProductFeatureInfo(int userActionID,
                                  int actionID,
                                  string active,
                                  int productID,
                                  string productName,
                                  string displayName,
                                  string actionCode,
                                  int? displayOrder)
        {
            UserActionID = userActionID;
            ActionID = actionID;
            Active = active;
            ProductID = productID;
            ProductName = productName;
            DisplayName = displayName;
            ActionCode = actionCode;
            DisplayOrder = displayOrder;
        }
    }
}
