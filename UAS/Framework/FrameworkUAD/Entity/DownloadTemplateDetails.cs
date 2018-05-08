using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class DownloadTemplateDetails
    {
        public DownloadTemplateDetails() { }

        #region Properties
        [DataMember]
        public int DownloadTemplateDetailsID { get; set; }
        [DataMember]
        public int DownloadTemplateID { get; set; }
        [DataMember]
        public string ExportColumn { get; set; }
        [DataMember]
        public bool IsDescription { get; set; }
        [DataMember]
        public string FieldCase { get; set; }
        #endregion
    }
  }
