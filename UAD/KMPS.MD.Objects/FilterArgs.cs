using System.Text;

namespace KMPS.MD.Objects
{
    internal class FilterArgs
    {
        public Filter Filter { get; set; }
        public string Query { get; set; } = string.Empty;
        public string DimQuery { get; set; } = string.Empty;
        public string OpenQuery { get; set; } = string.Empty;
        public string ClickQuery { get; set; } = string.Empty;
        public string VisitQuery { get; set; } = string.Empty;
        public int OpenCount { get; set; } = -1;
        public int ClickCount { get; set; } = -1;
        public int VisitCount { get; set; } = -1;
        public string SelectList { get; set; }
        public string AdditionalFilters { get; set; }
        public string Where { get; set; } = string.Empty;
        public string PubIds { get; set; } = string.Empty;
        public bool AddedMasterId { get; set; }
        public bool JoinBlastForOpen { get; set; }
        public bool JoinBlastForClick { get; set; }
        public StringBuilder OpenBlastCondition { get; set; } = new StringBuilder();
        public StringBuilder ClickBlastCondition { get; set; } = new StringBuilder();
        public StringBuilder OpenCondition { get; set; } = new StringBuilder();
        public StringBuilder ClickCondition { get; set; } = new StringBuilder();
        public StringBuilder VisitCondition { get; set; } = new StringBuilder();
        public string Link { get; set; } = string.Empty;
        public string DomainTrackingId { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string OpenSearchType { get; set; } = string.Empty;
        public string ClickSearchType { get; set; } = string.Empty;
        public string CreateTempTableQuery { get; set; } = string.Empty;
        public string DropTempTableQuery { get; set; } = string.Empty;
    }
}
