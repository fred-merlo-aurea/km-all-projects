using System;

namespace ECN_Framework_DataLayer.Communicator
{
    internal class FillLayoutContentArgs
    {
        public bool? Archived { get; set; }
        public string ArchiveFilter { get; set; }
        public int? BaseChannelId { get; set; }
        public string ContentTitle { get; set; }
        public int? CustomerId { get; set; }
        public int? CurrentPage { get; set; }
        public int? FolderId { get; set; }
        public string LayoutName { get; set; }
        public int? PageSize { get; set; }
        public string SortDirection { get; set; }
        public string SortColumn { get; set; }
        public int? UserId { get; set; }
        public DateTime? UpdatedDateFrom { get; set; }
        public DateTime? UpdatedDateTo { get; set; }
        public int? ValidatedOnly { get; set; }
    }
}
