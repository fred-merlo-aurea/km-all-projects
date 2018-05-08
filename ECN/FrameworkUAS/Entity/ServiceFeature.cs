using System;
using System.Runtime.Serialization;

namespace KMPlatform.Entity
{
    [Serializable]
    [DataContract]
    public class ServiceFeature
    {

        #region nested classes - reports: v_ServiceFeature_GetClientGroupTreeList, v_ServiceFeature_GetSecurityGroupTreeList, v_ServiceFeature_GetEmptySecurityGroupTreeList

        public class ClientGroupTreeListRow
        {
            public string ID { get; set; }
            public int ServiceID { get; set; }
            public int ServiceFeatureID { get; set; }
            public string ServiceName { get; set; }
            public string ServiceFeatureName { get; set; }
            public string Description { get; set; }
            public bool IsAdditionalCost { get; set; }
            public string PID { get; set; }
            public int MAPID { get; set; }
            public bool IsEnabled { get; set; }
            public int ServiceDisplayOrder { get; set; }
            public int ServiceFeatureDisplayOrder { get; set; }
        }

        public class SecurityGroupTreeListRow : ClientGroupTreeListRow
        {
            public int ServiceFeatureAccessMapID { get; set; }
            public string AccessLevelName { get; set; }
        }

        #endregion nested classes - reports: v_ServiceFeature_GetClientGroupTreeList, v_ServiceFeature_GetSecurityGroupTreeList, v_ServiceFeature_GetEmptySecurityGroupTreeList

        public ServiceFeature() 
        {
            ServiceFeatureID = 0;
            ServiceID = 0;
            SFName = string.Empty;
            Description = string.Empty;
            SFCode = string.Empty;
            DisplayOrder = 0;
            IsEnabled = false;
            IsAdditionalCost = false;
            DefaultRate = 0;
            DefaultDurationInMonths = 0;
            KMAdminOnly = true;
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
        }
        #region Properties
        [DataMember]
        public int ServiceFeatureID { get; set; }        
        [DataMember]
        public int ServiceID { get; set; }
        [DataMember]
        public string SFName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string SFCode { get; set; }
        [DataMember]
        public int DisplayOrder { get; set; }
        [DataMember]
        public bool IsEnabled { get; set; }
        [DataMember]
        public bool IsAdditionalCost { get; set; }
        [DataMember]
        public decimal DefaultRate { get; set; }
        [DataMember]
        public int DefaultDurationInMonths { get; set; }
        [DataMember]
        public bool KMAdminOnly { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        #endregion
    }
}
