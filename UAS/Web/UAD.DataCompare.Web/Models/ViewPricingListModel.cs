using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAD.DataCompare.Web.Models
{
    public class ViewPricingListModel
    {
        public int DcViewID { get; set; }
        public int SourceFileID { get; set; }
        public int TargetId { get; set; }
        public int? ScopeId { get; set; }
        public int TypeId { get; set; }
        public string FileName { get; set; }
        public DateTime DateCompared { get; set; }
        public DateTime UnpaidDate { get; set; }
        public string User { get; set; }
        public string Billable { get; set; }
        public string Target { get; set; }
        public string Scope { get; set; }
        public string TypeOfComparision { get; set; }
        public int TotalDownloaded { get; set; }
        public decimal Price { get; set; }
        public string TotalDownLoadCost { get; set; }
        public string FileComaprsionCost { get; set; }
        public PaymentStatus PStatus { get; set; }
        public string Notes { get; set; }
        public int TotalRecordCount { get; set; }
        public int FileRecordCount { get; set; }
        public int MatchedRecordCount { get; set; }
        public List<PaymentStatus> payStatusList = new List<PaymentStatus>();
        public bool IsAdmin { get; set; }
        public ViewPricingListModel()
        {
            DcViewID = 0;
            SourceFileID = 0;
            FileName = "";
            DateCompared = DateTime.Now;
            User = "N/A";
            Billable = "No";
            Target = "";
            Scope = "";
            TypeOfComparision = "";
            TotalDownloaded = 0;
            Price = 0;
            Notes = "";
            TotalRecordCount = 0;
            FileRecordCount = 0;
            MatchedRecordCount = 0;
            Notes = "";
            IsAdmin = false;
            PStatus = new PaymentStatus();
            UnpaidDate = DateCompared.AddDays(14);

        }
    }

    public class PaymentStatus
    {
        public int DCViewID { get; set; }
        public int PaymentStatusID { get; set; }
        public string PaymentStatusName { get; set; }

    }


}