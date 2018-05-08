using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ecn.domaintracking.Models
{
    public class DomainTrackerReportsViewModel
    {
        #region " Properties "
        public ECN_Framework_Entities.DomainTracker.DomainTracker DomainTracker { get; set; }
        public DataTable BrowserStats { get;set; }

        public bool ShowUnknown { get; set; }
        public DataTable PlatformStats { get; set; }
        
        public DataTable GetTotalViews { get; set; }
        public DataTable DomainTrackerActivity { get; set; }
        public DataTable HeatMapStats { get; set; }
        public DataTable HeatTbl { get; set; }
        public DataTable HeatTblWorld { get; set; }
        public string TotalPageViews { get; set; }
        public string KnownPageViews { get; set; }

        public string UnknownPageViews { get; set; }
        public DateTime ToDateTime { get; set; }
        public DateTime FromDateTime { get; set; }
        public bool IsUsaOnly {get;set;}

        public DataTable StateCountTbl { get; set; }

        
        public DataTable CountyCountTbl { get; set; }

        

        public DataTable HeatMapStatsDS { get; set; }

        public string TypeFilter { get; set; }

        
        #endregion

        #region " Constructor "
        public DomainTrackerReportsViewModel()
        {
            DomainTracker = new ECN_Framework_Entities.DomainTracker.DomainTracker();
            BrowserStats = new DataTable();
            PlatformStats = new DataTable();
            GetTotalViews = new DataTable();
            HeatTbl = new DataTable();
            DomainTrackerActivity = new DataTable();
            TotalPageViews = String.Empty;
            IsUsaOnly = true;
            StateCountTbl = new DataTable();
            ShowUnknown = false;
            TypeFilter = "Known";

        }
        #endregion
    }
}