using System;
using System.Runtime.Serialization;


namespace FrameworkUAS.Entity
{
    /// <summary>
    /// class for MVC project
    /// </summary>
    [Serializable]
    [DataContract]
    public class DataCompareDownloadView
    {
        public DataCompareDownloadView()
        {
            DcDownloadId = 0;
            SourceFileID = 0;
            ClientId = 0;
            DcTargetCodeId = 0;
            DcTypeCodeId = 0;
            CreatedByUserID = 0;
            DateCreated = DateTime.Now;
            WhereClause = "";
            TotalRecordCount = 0;
            DownloadFileName ="";
            DcTargetIdUad = 0;
            FileComparisonCost = 0;
            TotalDownLoadCost = 0;

        }
        #region Properties
        [DataMember]
        public int DcDownloadId { get; set; }
        [DataMember]
        public int SourceFileID { get; set; }
        [DataMember]
        public int ClientId { get; set; }
        [DataMember]
        public int DcTargetCodeId { get; set; }
        [DataMember]
        public int DcTargetIdUad { get; set; }
        [DataMember]
        public int DcTypeCodeId { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public string WhereClause { get; set; }
        [DataMember]
        public int TotalRecordCount { get; set; }
        [DataMember]
        public string DownloadFileName { get; set; }
        [DataMember]
        public decimal FileComparisonCost { get; set; }
        [DataMember]
        public decimal TotalDownLoadCost { get; set; }


        #endregion
    }
}
