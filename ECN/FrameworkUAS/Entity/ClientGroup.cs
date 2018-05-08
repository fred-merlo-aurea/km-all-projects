using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace KMPlatform.Entity
{
    [Serializable]
    [DataContract]
    public class ClientGroup
    {
        public ClientGroup() 
        {
            ClientGroupID = 0;
            ClientGroupName = string.Empty;
            ClientGroupDescription = string.Empty;
            Color = "black";
            IsActive = false;
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;

            Clients = new List<Client>();
            SecurityGroups = new List<SecurityGroup>();
            UADUrl = string.Empty;
        }
        #region Properties
        [DataMember]
        public int ClientGroupID { get; set; }
        [DataMember]
        public string ClientGroupName { get; set; }
        [DataMember]
        public string ClientGroupDescription { get; set; }
        [DataMember]
        public string Color { get; set; }
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
        [DataMember]
        public string UADUrl { get; set; }
        #endregion

        [DataMember]
        public List<Client> Clients { get; set; }
        [DataMember]
        public List<SecurityGroup> SecurityGroups { get; set; }
    }
}
