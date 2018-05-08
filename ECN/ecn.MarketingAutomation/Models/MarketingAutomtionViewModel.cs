using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.MarketingAutomation.Models
{
    public class MarketingAutomtionViewModel:ModelBase
    {
        public MarketingAutomtionViewModel()
        {
            TotalRecordCounts = String.Empty; 
            MarketingAutomationID = 0;
            Name = String.Empty;
            CustomerID = 0;
            CustomerName = String.Empty;
            CreatedDate = String.Empty; 
            UpdatedDate= null;
            StartDate = DateTime.MinValue;
            EndDate = DateTime.MinValue;
            LastPublishedDate = String.Empty;
            State = "Saved";
        }
        public string TotalRecordCounts { get; set; }
        public int MarketingAutomationID { get; set; }
        public string Name { get; set; }

        public int CustomerID { get; set; }

        public string CustomerName { get; set; }

        public string CreatedDate { get; set; }

        public string UpdatedDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string LastPublishedDate { get; set; }
        public String State { get; set; }
    }
}