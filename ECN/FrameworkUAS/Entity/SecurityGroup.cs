using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace KMPlatform.Entity
{
    [Serializable]
    [DataContract]
    public class SecurityGroup
    {
        public SecurityGroup()
        {
            SecurityGroupID = 0;
            SecurityGroupName = string.Empty;
            ClientID = 0;
            ClientGroupID = 0;
            IsActive = false;
            Description = string.Empty;
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
            Services = new List<Service>();
            AdministrativeLevel = Enums.SecurityGroupAdministrativeLevel.None;
            Permissions = new Dictionary<Tuple<Enums.Services, Enums.ServiceFeatures>, List<Enums.Access>>();
        }

        #region Properties
        [DataMember]
        public int SecurityGroupID { get; set; }
        [DataMember]
        public string SecurityGroupName { get; set; }
        [DataMember]
        public int ClientID { get; set; }
        [DataMember]
        public int ClientGroupID { get; set; }
        [IgnoreDataMember]
        public Enums.SecurityGroupAdministrativeLevel AdministrativeLevel { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
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
        public List<Service> Services { get; set; }

        [DataMember]
        public Dictionary<Tuple<KMPlatform.Enums.Services, KMPlatform.Enums.ServiceFeatures>, List<KMPlatform.Enums.Access>> Permissions { get; set; }
    }
}
