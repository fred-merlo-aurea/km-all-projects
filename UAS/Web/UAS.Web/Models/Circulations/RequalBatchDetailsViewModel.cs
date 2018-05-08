using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace UAS.Web.Models.Circulations
{
    public class RequalBatchDetailsViewModel
    {
        [Required]
        public int ProductID { get; set; }
        [Required]
        public int Par3CID { get; set; }
        public int QSourceID { get; set; }
        public int PubSubscriptionID { get; set; }
        public string SubSrc { get; set; }
        public DateTime? QDate { get; set; }
        public List<ProductSubscriptionDetailVM> PubSubDetails { get; set; }
        public List<int> SelectedResponseGroups { get; set; }

        public List<SelectListItem> ProductList { get; set; }

        public List<SelectListItem> QSourceList { get; set; }

        public List<SelectListItem> Par3CList { get; set; }

        public List<RequalDemos> ResponseGroupList { get; set; }

        public RequalBatchDetailsViewModel()
        {
            ProductID = 0;
            QSourceID = 0;
            Par3CID = 0;
            QDate = DateTime.Now;
            PubSubscriptionID = 0;
            ProductList = new List<SelectListItem>();
            QSourceList = new List<SelectListItem>();
            Par3CList = new List<SelectListItem>();
            ResponseGroupList = new List<RequalDemos>();
            SelectedResponseGroups = new List<int>();
            PubSubDetails = new List<ProductSubscriptionDetailVM>();
        }

    }

    public class RequalDemos
    {
        public int Value { get; set; }
        public string Text { get; set; }
        public bool IsMultiple { get; set; }
        public bool IsRequired { get; set; }
    }
    public class ProductSubscriptionDetailVM
    {
        public ProductSubscriptionDetailVM()
        {
            PubSubscriptionDetailID = 0;
            PubSubscriptionID = 0;
            SubscriptionID = 0;
            CodeSheetID = 0;
            ResponseGroupID = 0;
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
            ResponseOther = "";
            DemoChecked = false;
        }
        public int PubSubscriptionDetailID { get; set; }
        public int PubSubscriptionID { get; set; }
        public int SubscriptionID { get; set; }
        public int CodeSheetID { get; set; }
        public int ResponseGroupID { get; set; }
        public bool DemoChecked { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedByUserID { get; set; }
        public string ResponseOther { get; set; }
        
    }
}