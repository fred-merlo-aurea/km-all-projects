using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAS.Web.Models.Circulations
{
    public class AddKillProductViewModel : AbstractViews.ProductViewModel
    {
        public int ProductID { get; set; }
        public string AddRemove { get; set; }
        public int ImbSeqCounter { get; set; }
        public int RemainingCount { get; set; }
        public FrameworkUAD.Object.Counts RecordCounts { get; set; }
        public Dictionary<int, string> QualDict { get; set; }
        public List<int> RemainingPool { get; set; }
        public Dictionary<int, string> ImbSeqs { get; set; }
        public Dictionary<int, string> CompImbSeqs { get; set; }
        public List<FrameworkUAD.Entity.CopiesProductSubscription> subscriptionList { get; set; }
        public List<AddKillContainer> AddKillList { get; set; }
        public bool ReadOnly { get; set; }
        public FrameworkUAD.Object.FilterCollection FilterCollection { get; set; }
        public AddKillProductViewModel()
        {
            AddKillList = new List<AddKillContainer>();
            RecordCounts = new FrameworkUAD.Object.Counts();
            QualDict = new Dictionary<int, string>();
            ImbSeqs = new Dictionary<int, string>();
            CompImbSeqs = new Dictionary<int, string>();
            subscriptionList = new List<FrameworkUAD.Entity.CopiesProductSubscription>();
            RemainingPool = new List<int>();            
        }
    }

    public class AddKillContainer
    {
        public string ContainerId { get; set; }
        public bool NotUpdated { get; set; }
        public string Type { get; set; }
        public int ActualCount { get; set; }        
        public int ProductID { get; set; }        
        public bool Update { get; set; }
        public int DesiredCount { get; set; }
        public List<int> SubscriberIDs { get; set; }
        public FrameworkUAD.Object.FilterMVC Filter { get; set; }
        public AddKillContainer() { }            
        public AddKillContainer(string containerId, int desiredCount, FrameworkUAD.Object.FilterMVC filter, List<int> subscriberIDs, int productID, string type)
        {
            this.ContainerId = containerId;
            this.Type = type;
            this.ActualCount = subscriberIDs.Count;
            this.Filter = filter;
            this.SubscriberIDs = subscriberIDs;
            this.DesiredCount = desiredCount;
            this.ProductID = productID;
            this.NotUpdated = true;            
        }
    }

    public class DownloadColumns
    {
        public string DisplayName { get; set; }
        public string DownloadName { get; set; }
        public string Type { get; set; }
        public DownloadColumns() { }
    }

    public class DesiredCount
    {
        public string ID { get; set; }
        public int Count { get; set; }
    }

    public class AddKillContainerOriginal
    {
        //public int IssueSplitId { get; set; }
        //public int IssueId { get; set; }
        //public string IssueSplitCode { get; set; }
        //public int IssueSplitCount { get; set; }
        //public int IssueSplitRecords { get; set; }
        ////After you export, NotExported is set to false. This controls additional code if you try to Uncheck an exported Split.
        //public bool NotExported { get; set; }
        public bool NotUpdated { get; set; }
        public string Type { get; set; }
        public int ActualCount { get; set; }
        public int FilterId { get; set; }
        public int ProductID { get; set; }
        //public string KeyCode { get; set; }
        public bool IsActive { get; set; }        
        //Save is a bound field to a CheckBox that states whether you want to Export the file or not.
        public bool Save { get; set; }
        public int DesiredCount { get; set; }
        //public FrameworkUAD.Entity.IssueSplit IssueSplit { get; set; }
        public FrameworkUAD.Object.FilterMVC Filter { get; set; }
        // public List<FrameworkUAD.> Filters { get; set; }
        public List<int> SubscriberIDs { get; set; }
        public List<SplitChildViewModel> ChildSplits { get; set; }
        public AddKillContainerOriginal(int count, FrameworkUAD.Object.FilterMVC filter, List<int> subscriberIDs, int productID, string type)
        {
            //this.IssueSplitId = split.IssueSplitId;
            //this.IssueId = split.IssueId;
            //this.IssueSplitCode = split.IssueSplitCode;
            //this.IssueSplitCount = split.IssueSplitCount;
            //this.RecordCount = subscriberIDs.Count;
            //this.IssueSplit = split;
            this.Type = type;
            this.ActualCount = subscriberIDs.Count;
            //this.FilterId = split.FilterId;
            //this.KeyCode = split.KeyCode;
            //this.IsActive = split.IsActive;
            this.Filter = filter;
            this.SubscriberIDs = subscriberIDs;
            this.DesiredCount = count;
            //this.IssueSplitRecords = split.IssueSplitRecords;
            this.ProductID = productID;
            this.NotUpdated = true;
            //this.NotExported = true;
            this.ChildSplits = new List<SplitChildViewModel>();
        }

        public class SplitChildViewModel
        {
            public SplitChildViewModel(string name, string parentName, string desc, int count, List<int> ids)
            {
                this.SplitName = name;
                this.SplitDescription = desc;
                this.SplitParent = parentName;
                this.SplitCount = count;
                this.SubscriberIDs = ids;
            }
            public string SplitName { get; set; }
            public string SplitDescription { get; set; }
            public int SplitCount { get; set; }
            public string SplitParent { get; set; }
            public List<int> SubscriberIDs { get; set; }
        }
        //public class AddKillReport
        //{
        //    public string SplitName { get; set; }
        //    public string SplitDescription { get; set; }
        //    public string KeyCode { get; set; }
        //    public int Records { get; set; }
        //    public int Copies { get; set; }
        //    public AddKillReport(string name, string description, string keyCode, int records, int copies)
        //    {
        //        this.SplitName = name;
        //        this.SplitDescription = description;
        //        this.KeyCode = keyCode;
        //        this.Records = records;
        //        this.Copies = copies;
        //    }
        //}

        public class FilterCombinations
        {
            public FilterCombinations(List<int> criteria)
            {
                this.Criteria = criteria;
            }

            public List<int> Criteria { get; set; }
        }

    }
}