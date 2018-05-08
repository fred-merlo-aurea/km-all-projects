using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class Filter
    {
        public Filter()
        {
            FilterId = 0;
            FilterName = string.Empty;
            ProductId = 0;
            IsActive = false;
            BrandId = 0;
            ClientID = 0;
            FilterGroupID = 0;
            DateCreated =  DateTime.Now;
            DateUpdated = null;
            CreatedByUserID = 0;
            UpdatedByUserID = null;
        }
        #region Properties
        [DataMember]
        public int FilterId {get;set;}
        [DataMember]
        public string FilterName {get;set;}
        [DataMember]
        public int ProductId {get;set;}
        [DataMember]
        public bool IsActive {get;set;}
        [DataMember]
        public int BrandId {get;set;}
        [DataMember]
        public int ClientID { get; set; }
        [DataMember]
        public int FilterGroupID {get;set;}
        [DataMember]
        public DateTime DateCreated {get;set;}
        [DataMember]
        public DateTime? DateUpdated {get;set;}
        [DataMember]
        public int CreatedByUserID {get;set;}
        [DataMember]
        public int? UpdatedByUserID {get;set;}
        #endregion
    }
}
