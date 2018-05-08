using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class DownloadTemplate : IDownloadTemplate
    {
        public DownloadTemplate() { }

        [DataMember]
        public int DownloadTemplateID { get; set; }
        [DataMember]
        public string DownloadTemplateName { get; set; }
        [DataMember]
        public int BrandID { get; set; }
        [DataMember]
        public int PubID { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime UpdatedDate { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
    }
}
