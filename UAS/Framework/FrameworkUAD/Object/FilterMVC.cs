using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAD.Object
{
    public class FilterMVC
    {
        public FilterMVC()
        {

            FilterID = 0;
            FilterNo = 0;
            Count = 0;
            Fields = new List<Object.FilterDetails>();
            Subscribers = new List<Object.Subscriber>();
            ViewType = FrameworkUAD.BusinessLogic.Enums.ViewType.None;
            PubID = 0;
            BrandID = 0;
            FilterGroupID = 0;
            FilterGroupName = string.Empty;
            Executed = false;
            SubscriberIDs = new List<int>();
        }

        #region Properties
        public int FilterID { get; set; }
        public int FilterNo { get; set; }
        public string FilterName { get; set; }
        public string FilterDescription { get; set; }
        public int FilterGroupID { get; set; }
        public string FilterGroupName { get; set; }
        public int PubID { get; set; }
        public int IssueID { get; set; }
        public int BrandID { get; set; }
        public FrameworkUAD.BusinessLogic.Enums.ViewType ViewType { get; set; }
        public bool Executed { get; set; }
        public string FilterType { get; set; }
        public int Count { get; set; }
        public List<Object.FilterDetails> Fields { get; set; }
        public List<Object.Subscriber> Subscribers { get; set; }
        public List<int> SubscriberIDs { get; set; }
        public string FilterQuery { get; set; }
        #endregion
    }
}
