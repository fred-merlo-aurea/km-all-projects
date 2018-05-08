using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FrameworkUAD.Entity;

namespace UAS.Web.Models.Circulations
{
    public class IssueSplitProductViewModel : AbstractViews.ProductViewModel
    {
        public List<int> compSubIDs;
        internal List<FrameworkUAD.Entity.IssueCompDetail> comps;
        public List<DateTime> lastmodifieddates { get; set; }
        public int IssueID { get; set; }
        public int WaveID { get; set; }
        public int ImbSeqCounter { get; set; }
        public int RemainingCount { get; set; }
        public string SplitName { get; set; }
        public string SplitDescription { get; set; }
        public FrameworkUAD.Object.Counts RecordCounts { get; set; }
        public FrameworkUAD.Entity.Issue Issue { get; set; }
        public Dictionary<int, string> QualDict { get; set; }
        public List<int> RemainingPool { get; set; }
        public Dictionary<int, string> ImbSeqs { get; set; }
        public Dictionary<int, string> CompImbSeqs { get; set; }
        public List<FrameworkUAD.Entity.CopiesProductSubscription> subscriptionList { get; set; }
        public List<IssueSplitContainer> SplitsList { get; set; }
        public List<IssueSplitContainer> TempSplitsList { get; set; }
        public bool ReadOnly { get; set; }
        public FrameworkUAD.Object.FilterCollection FilterCollection { get; set; }
        public List<FrameworkUAD.Entity.Issue> IssueList { get; set; }
        public List<FrameworkUAD.Entity.WaveMailing> WaveList { get; set; }
        public IssueSplitProductViewModel()
        {
            lastmodifieddates = new List<DateTime>();
            TempSplitsList = new List<IssueSplitContainer>();
            SplitsList = new List<IssueSplitContainer>();
            IssueList = new List<FrameworkUAD.Entity.Issue>();
            WaveList = new List<FrameworkUAD.Entity.WaveMailing>();
            Issue = new FrameworkUAD.Entity.Issue();
            RecordCounts = new FrameworkUAD.Object.Counts();
            QualDict = new Dictionary<int, string>();
            ImbSeqs = new Dictionary<int, string>();
            CompImbSeqs = new Dictionary<int, string>();
            subscriptionList = new List<FrameworkUAD.Entity.CopiesProductSubscription>();
            RemainingPool = new List<int>();
            compSubIDs = new List<int>();
            comps = new List<FrameworkUAD.Entity.IssueCompDetail>();
        }

    }
    public class IssueSplitContainer
    {
        public int IssueSplitId { get; set; }
        public int IssueId { get; set; }
        public string IssueSplitCode { get; set; }
        public string IssueSplitName { get; set; }
        public string IssueSplitDescription { get; set; }
        public int IssueSplitCount { get; set; }
        public int IssueSplitRecords { get; set; }
        public int RecordCount { get; set; }
        public int FilterId { get; set; }
        public int ProductID { get; set; }
        public string KeyCode { get; set; }
        public bool IsActive { get; set; }
        //After you export, NotExported is set to false. This controls additional code if you try to Uncheck an exported Split.
        public bool NotExported { get; set; }
        //Save is a bound field to a CheckBox that states whether you want to Export the file or not.
        public bool Save { get; set; }
        public int DesiredCount { get; set; }
        public FrameworkUAD.Entity.IssueSplit IssueSplit { get; set; }
        public FrameworkUAD.Object.FilterMVC Filter { get; set; }
        // public List<FrameworkUAD.> Filters { get; set; }
        public List<int> SubscriberIDs { get; set; }
        public List<SplitChildViewModel> ChildSplits { get; set; }
        public IssueSplitContainer(FrameworkUAD.Entity.IssueSplit split, FrameworkUAD.Object.FilterMVC filter, List<int> subscriberIDs, int productID)
        {
            this.IssueSplitId = split.IssueSplitId;
            this.IssueId = split.IssueId;
            this.IssueSplitCode = split.IssueSplitCode;
            this.IssueSplitName = split.IssueSplitName;
            this.IssueSplitCount = split.IssueSplitCount;
            this.RecordCount = subscriberIDs.Count;
            this.IssueSplit = split;
            this.FilterId = split.FilterId;
            this.KeyCode = split.KeyCode;
            this.IsActive = split.IsActive;
            this.Filter = filter;
            this.SubscriberIDs = subscriberIDs;
            this.DesiredCount = split.IssueSplitCount;
            this.IssueSplitDescription = split.IssueSplitDescription;
            this.IssueSplitRecords = split.IssueSplitRecords;
            this.ProductID = productID;
            this.NotExported = true;
            this.ChildSplits = new List<SplitChildViewModel>();
        }


    }
    public class IssueSplitReport
    {
        public string SplitName { get; set; }
        public string SplitDescription { get; set; }
        public string KeyCode { get; set; }
        public int Records { get; set; }
        public int Copies { get; set; }
        public IssueSplitReport(string name, string description, string keyCode, int records, int copies)
        {
            this.SplitName = name;
            this.SplitDescription = description;
            this.KeyCode = keyCode;
            this.Records = records;
            this.Copies = copies;
        }
    }
    public class IssueFinalizeViewModel
    {
        public string FinalizeOperation { get; set; }
        public int CurrentIssueID { get; set; }
        public int CurrentProductID { get; set; }
        public int MaxIMB { get; set; }
        public int OrginalIMB { get; set; }
        public string NextIssueName { get; set; }
        public string NextIssueCode { get; set; }
        public string WaveMailingName { get; set; }
        public int WaveNumber { get; set; }
        public List<FrameworkUAD.Entity.IssueSplit> Splits { get; set; }
        public IssueFinalizeViewModel()
        {
            Splits = new List<IssueSplit>();
        }
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

    public class ExportModel
    {
        public int ProductID { get; set; }
        public int IssueID { get; set; }
        public List<int> IssueSplitIDs { get; set; }
        public bool IsDefault { get; set; }
        public List<DownloadField> DownloadFields { get; set; }
        public List<NewColumn> NewColumnsFields { get; set; }
        public ExportModel()
        {
            ProductID = 0;
            IssueID = 0;
            IssueSplitIDs = new List<int>();
            IsDefault = true;
            DownloadFields = new List<DownloadField>();
            NewColumnsFields = new List<NewColumn>();
        }
    }
    public class ExportFieldsViewModel
    {
        public List<DownloadField> ProfileFields { get; set; }
        public List<DownloadField> DemoFields { get; set; }
        public List<DownloadField> AdHocFields { get; set; }
        public List<DownloadField> PaidFields { get; set; }
        public ExportFieldsViewModel()
        {
            ProfileFields = new List<DownloadField>();
            DemoFields = new List<DownloadField>();
            AdHocFields = new List<DownloadField>();
            PaidFields = new List<DownloadField>();
        }
    }
    public class DownloadField
    {
        public string DisplayName { get; set; }
        public string DownloadName { get; set; }
        public string Type { get; set; }
        public DownloadField()
        {

        }
        public DownloadField(string display, string prefix, string type)
        {
            DisplayName = display;
            DownloadName = prefix + ".[" + display + "]";
            Type = type;
        }
    }
    public class NewColumn
    {
        public string Name { get; set; }
        public List<string> Columns { get; set; }
        public string Delimiter { get; set; }
        public NewColumn()
        {

        }

        public NewColumn(string name, List<string> cols, string delimiter)
        {
            this.Name = name;
            this.Columns = cols;
            this.Delimiter = delimiter;
        }
    }



    public class FilterCombinations
    {
        public FilterCombinations(List<int> criteria)
        {
            this.Criteria = criteria;
        }

        public List<int> Criteria { get; set; }
    }
    public class DesiredCountTrnsferViewModel
    {
        public int FromIssuePlitId { get; set; }
        public int ToIssueSplitId { get; set; }
        public int MovedRecordCount { get; set; }
        public int TotalRecordCount { get; set; }
    }
}