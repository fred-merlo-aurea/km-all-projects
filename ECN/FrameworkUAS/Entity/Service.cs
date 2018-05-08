using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace KMPlatform.Entity
{
    [Serializable]
    [DataContract]
    public class Service
    {
        public Service() 
        {
            ServiceID = 0;
            ServiceName = string.Empty;
            Description = string.Empty;
            ServiceCode = string.Empty;
            DisplayOrder = 0;
            IsEnabled = false;
            IsAdditionalCost = false;
            HasFeatures = false;
            DefaultRate = 0;
            DefaultDurationInMonths = 0;
            DefaultApplicationID = 0;
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;

            ServiceFeatures = new List<ServiceFeature>();
            Applications = new List<Application>();
        }
        #region Properties
        [DataMember]
        public int ServiceID { get; set; }
        [DataMember]
        public string ServiceName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string ServiceCode { get; set; }
        [DataMember]
        public int DisplayOrder { get; set; }
        [DataMember]
        public bool IsEnabled { get; set; }
        [DataMember]
        public bool IsAdditionalCost { get; set; }
        [DataMember]
        public bool HasFeatures { get; set; }
        [DataMember]
        public decimal DefaultRate { get; set; }
        [DataMember]
        public int DefaultDurationInMonths { get; set; }
        [DataMember]
        public int DefaultApplicationID { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        #endregion
        [DataMember]
        public List<ServiceFeature> ServiceFeatures { get; set; }
        [DataMember]
        public List<Application> Applications { get; set; }
    }
}
