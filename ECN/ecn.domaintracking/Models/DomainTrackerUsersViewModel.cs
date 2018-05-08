using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using ecn.domaintracking.Models.Shared;

namespace ecn.domaintracking.Models
{
    public class DomainTrackerUsersViewModel
    {
        #region " Properties "
        public ECN_Framework_Entities.DomainTracker.DomainTracker DomainTracker { get; set; }
        public List<ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile> ProfileList { get; set; }
        public List<List<ECN_Framework_Entities.DomainTracker.DomainTrackerActivity>> ActivityList { get; set; }
        public List<DataTable> DomainTrackerValue { get; set; }
  
        public DataTable emailRecordsDT { get; set; }
        public int ECNGroupId { get; set; }
        public int ECNFolderId { get; set; }
        public int ProfileCount { get; set; }
        public string ECNGroupName { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public int DomainTrackId { get; set; }
        public string PageUrl { get; set; }

        public string FilterType { get; set; }

        public bool ShowUnknown { get; set; }

        
        public GroupSelectorModel GroupSelectorModel { get; set; }

        #endregion

        #region " Constructor "
        public DomainTrackerUsersViewModel()
        {
            DomainTracker = new ECN_Framework_Entities.DomainTracker.DomainTracker();
            ProfileList = new List<ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile>();
            ActivityList = new List<List<ECN_Framework_Entities.DomainTracker.DomainTrackerActivity>>();
            DomainTrackerValue = new List<DataTable>();

            fromDate = string.Empty;
            toDate = string.Empty;
            DomainTrackId = 0;
            PageUrl = string.Empty;
            FilterType = "known";
            GroupSelectorModel = new GroupSelectorModel();
            ShowUnknown = false;
        }
        #endregion
        
        public List<ECN_Framework_Entities.DomainTracker.DomainTrackerUserActivityToExport> GetUserListForExport()
        {

            List<ECN_Framework_Entities.DomainTracker.DomainTrackerUserActivityToExport> userExportList = new List<ECN_Framework_Entities.DomainTracker.DomainTrackerUserActivityToExport>();
            for (int i = 0; i < ProfileList.Count; i++)
            {
                for (int b = 0; b < ActivityList[i].Count; b++)
                {
                    ECN_Framework_Entities.DomainTracker.DomainTrackerUserActivityToExport userActvity = new ECN_Framework_Entities.DomainTracker.DomainTrackerUserActivityToExport();
                    userActvity.EmailAddress = ProfileList[i].EmailAddress;
                    if (ActivityList[i] != null && ActivityList[i][b] != null)
                    {
                        userActvity.PageURL = ActivityList[i][b].PageURL;
                        userActvity.TimeStamp = ActivityList[i][b].TimeStamp.ToString();
                        userActvity.IPAddress = ActivityList[i][b].IPAddress;
                        userActvity.OS = ActivityList[i][b].OS;
                        userActvity.Browser = ActivityList[i][b].Browser;
                    }
                    userExportList.Add(userActvity);
                }
            }
            return userExportList;
        }

    }
}