using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace KMPlatform.Entity
{
    [Serializable]
    [DataContract]
    public class Client
    {
        public Client()
        {
            ClientID = 0;
            AccessKey = Guid.NewGuid();
            ClientName = string.Empty;
            DisplayName = string.Empty;
            ClientCode = string.Empty;
            ClientTestDBConnectionString = string.Empty;
            ClientLiveDBConnectionString = string.Empty;
            IsActive = false;
            IgnoreUnknownFiles = true;
            AccountManagerEmails = string.Empty;
            ClientEmails = string.Empty;
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
            HasPaid = false;
            IsKMClient = true;
            IsAMS = false;
            ParentClientId = 0;
            HasChildren = false;
            FtpFolder = string.Empty;

            ClientConfigurations = new List<ClientConfigurationMap>();
            Products = new List<Object.Product>();
            ClientConnections = new Object.ClientConnections();
            Services = new List<Service>();
        }
        #region Properties
        [DataMember]
        public int ClientID { get; set; }
        [DataMember]
        public string ClientName { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public string ClientCode { get; set; }
        [DataMember]
        public string ClientTestDBConnectionString { get; set; }
        [DataMember]
        public string ClientLiveDBConnectionString { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public bool IgnoreUnknownFiles { get; set; }
        [DataMember]
        public string AccountManagerEmails { get; set; }
        [DataMember]
        public string ClientEmails { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        [DataMember]
        public bool HasPaid { get; set; }
        [DataMember]
        public bool IsKMClient { get; set; }
        [DataMember]
        public bool IsAMS { get; set; }
        [DataMember]
        public int ParentClientId { get; set; }
        [DataMember]
        public bool HasChildren { get; set; }
        [DataMember]
        public Guid AccessKey { get; set; }
        [DataMember]
        public string FtpFolder { get; set; }
        #endregion
        [DataMember] 
        public List<ClientConfigurationMap> ClientConfigurations { get; set; }
        [DataMember]
        public Object.ClientConnections ClientConnections { get; set; }


        [DataMember]
        public List<Object.Product> Products { get; set; }

        [DataMember]
        public List<Service> Services { get; set; }

        public bool UseSubGen { get { return Products.Exists(x=> x.UseSubGen == true); }}
    }
}
