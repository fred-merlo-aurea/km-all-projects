using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
//using System.Text;

namespace KMPlatform.Entity
{
   

    [Serializable]
    [DataContract]
    public class User
    {
        #region view models
        public class EcnAccountsUserListGridViewModel
        {
            public string UserName { get; set; }
            public int UserID { get; set; }
            public string Status { get; set; }
            public bool IsKMStaff { get; set; }
            public bool IsPlatformAdministrator { get; set; }
            public bool IsActive { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int UserClientSecurityGroupMapID { get; set; }
            public int ClientGroupID { get; set; }
            public int ClientID { get; set; }
            public string ClientName { get; set; }
            public int SecurityGroupID { get; set; }
            public string SecurityGroupName { get; set; }
            public KMPlatform.Enums.SecurityGroupAdministrativeLevel AdministrativeLevel { get; set; }
        }
        #endregion view models
        public User() 
        {
            UserID = 0;
            DefaultClientGroupID = 0;
            DefaultClientID = 0;
            Status = Enums.UserStatus.Disabled;
            FirstName = string.Empty;
            LastName = string.Empty;
            UserName = string.Empty;
            Password = string.Empty;
            Phone = string.Empty;
            Salt = string.Empty;
            EmailAddress = string.Empty;
            IsActive = false;
            AccessKey = Guid.Empty;
            IsAccessKeyValid = false;
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
            IsKMStaff = false;
            IsPlatformAdministrator = false;
            ClientGroups = new List<ClientGroup>();
            UserClientSecurityGroupMaps = new List<UserClientSecurityGroupMap>();
            CurrentClientGroup = null;
            CurrentClient = null;
            CurrentSecurityGroup = null;
            OpenedClients = new List<KMPlatform.Entity.Client>();
            IsReadOnly = false;
            RequirePasswordReset = false;
            //Applications = new List<KMPlatform.Entity.Application>();
            //MenuFeatures = new Dictionary<string,List<MenuFeature>>();
            //Services = new List<Service>();
            //ServiceFeatures = new Dictionary<string, List<ServiceFeature>>();
        }

        private int _customerID = 0;
        [DataMember]
        public int CustomerID
        {
            get
            {
                return _customerID;
            }
            set
            {
                _customerID = value;
            }
        }

        #region Properties
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public int DefaultClientGroupID { get; set; }
        [DataMember]
        public int DefaultClientID { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Salt { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public Guid AccessKey { get; set; }
        [DataMember]
        public Enums.UserStatus Status { get; set; }
        [DataMember]
        public bool IsAccessKeyValid { get; set; }
        [DataMember]
        public bool IsKMStaff { get; set; }
        [DataMember]
        public bool IsPlatformAdministrator { get; set; }        
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        [DataMember]
        public bool IsReadOnly { get; set; }
        [DataMember]
        public bool RequirePasswordReset { get; set; }
        #endregion

        private string _fullName;
        [DataMember]
        public string FullName
        {
            get
            {
                _fullName = FirstName + " " + LastName;
                return _fullName;
            }
            set
            {
                _fullName = value;
            }
        }

        [DataMember]
        public List<ClientGroup> ClientGroups { get; set; }
        [DataMember]
        public List<UserClientSecurityGroupMap> UserClientSecurityGroupMaps { get; set; }

        [DataMember]
        public ClientGroup CurrentClientGroup { get; set; }
        

        private Client _currentClient = null;
        [DataMember]
        public Client CurrentClient {
            get 
            {
                return _currentClient;
            }
            set
            {
                _currentClient = value;

                if (_currentClient != null)
                {
                    this.CustomerID = KMPlatform.DataAccess.ECN.getCustomerIDbyClientID(_currentClient.ClientID);
                }
            }
        
        }
        [DataMember]//caused error
        public SecurityGroup CurrentSecurityGroup { get; set; }
        [DataMember]
        public List<KMPlatform.Entity.Client> OpenedClients { get; set; }

        //[DataMember]
        //public List<KMPlatform.Entity.Application> Applications { get; set; }
        //[DataMember]
        //public Dictionary<string,List<KMPlatform.Entity.MenuFeature>> MenuFeatures { get; set; }
        //[DataMember]
        //public List<KMPlatform.Entity.Service> Services { get; set; }
        //[DataMember]
        //public Dictionary<string, List<KMPlatform.Entity.ServiceFeature>> ServiceFeatures { get; set; }

        
    }
}
