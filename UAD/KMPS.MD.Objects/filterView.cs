using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using System.Text;

namespace KMPS.MD.Objects
{
    public class filterView
    {
        #region Properties
        public int FilterViewNo { get; set; }
        public string FilterViewName { get; set; }
        public string SelectedFilterNo { get; set; }
        public string SuppressedFilterNo { get; set; }
        public string SelectedFilterGroupID { get; set; }
        public string SuppressedFilterGroupID { get; set; }
        public string SelectedFilterOperation { get; set; }
        public string SuppressedFilterOperation { get; set; }
        public string FilterDescription { get; set; }
        public int Count { get; set; }
        public List<int> SubscriptionID { get; set; }
        #endregion

        public filterView()
        {
            FilterViewNo = 0;
            FilterViewName = string.Empty; 
            SelectedFilterNo = string.Empty;
            SuppressedFilterNo = string.Empty;
            SelectedFilterGroupID = string.Empty;
            SuppressedFilterGroupID = string.Empty;
            SelectedFilterOperation = string.Empty;
            SelectedFilterOperation = string.Empty;
            SuppressedFilterOperation = string.Empty;
            FilterDescription = string.Empty;
            Count = 0;
            SubscriptionID = new List<int>();
        }
    }
}