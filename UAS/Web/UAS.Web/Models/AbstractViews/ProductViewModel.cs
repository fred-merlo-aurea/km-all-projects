using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAS.Web.Models.AbstractViews
{
    public abstract class ProductViewModel
    {
        public int BrandID { get; set; }
        public int PubID { get; set; }
        public string PubName { get; set; }
        public int ClientID { get; set; }
        public List<int> FilterIDs { get; set; }
        public bool IsCirc { get; set; }
        public FrameworkUAD.BusinessLogic.Enums.ViewType ViewType { get; set; }
        public List<int> SubscriptionIDs { get; set; }

        public FrameworkUAD.Object.FilterCollection Filters = null;
        public ProductViewModel()
        {
            BrandID = 0;
            PubID = 0;
            PubName = string.Empty;
            ClientID = 0;
            FilterIDs = new List<int>();
            IsCirc = false;
            ViewType = FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView;
            SubscriptionIDs = new List<int>();
        }
    }
}