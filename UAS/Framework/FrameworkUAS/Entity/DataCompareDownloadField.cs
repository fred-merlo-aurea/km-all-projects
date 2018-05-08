using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class DataCompareDownloadField
    {
        public DataCompareDownloadField()
        {
            DcDownloadFieldId = 0;
            DcDownloadId = 0;
            DcDownloadFieldCodeId = 0;
            ColumnName = string.Empty;
            ColumnID = 0;
            IsDescription = false;
        }
        #region Properties
        [DataMember]
        public int DcDownloadFieldId { get; set; }
        [DataMember]
        public int DcDownloadId { get; set; }
        [DataMember]
        public int DcDownloadFieldCodeId { get; set; }
        [DataMember]
        public string ColumnName { get; set; }
        [DataMember]
        public int ColumnID { get; set; }
        [DataMember]
        public bool IsDescription { get; set; }
        #endregion
    }
}
