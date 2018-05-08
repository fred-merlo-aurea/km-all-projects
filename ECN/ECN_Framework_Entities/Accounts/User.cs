//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Runtime.Serialization;
//using ECN_Framework_Common.Objects;

//namespace ECN_Framework_Entities.Accounts
//{
//    [Serializable]
//    [DataContract]
//    public class User
//    {

//        //readonly static string[] _transformationProperties = 
//        //{
//        //    "UserID", "DefaultClientID", "DefaultClientGroupID", "FirstName", "LastName", "Password", "UserName", "Salt", "EmailAddress",             
//        //    "IsActive", "ActiveFlag", 
//        //    "AccessKey", "IsAccessKeyValid",            
//        //    "ClientGroups","UserClientSecurityGroupMaps","CurrentClientGroup","CurrentClient","CurrentSecurityGroup","OpenedClients",            
//        //    "DateUpdated","UpdatedByUserID","DateCreated","CreatedByUserID"
//        //};

//        public static implicit operator User(KMPlatform.Entity.User user)
//        {
//            System.Diagnostics.Trace.Write(user, "implicit conversion from Platform user");
//            //return Core.Utilities.Transformer<KMPlatform.Entity.User, User>.Transform(user, _transformationProperties);
//            return Core.Utilities.Transformer<KMPlatform.Entity.User, User>.Transform(user, (eu, pu) => 
//            {
//                eu.ActiveFlag = pu.IsActive ? "Y" : "N";
//                return eu;
//            });
//        }
        
//        public static implicit operator KMPlatform.Entity.User(User ecnUser)  // implicit digit to byte conversion operator
//        {
//            System.Diagnostics.Trace.Write(ecnUser, "implicit conversion to Platform user");
//            return Core.Utilities.Transformer<User, KMPlatform.Entity.User>.Transform(ecnUser, (pu, eu) =>
//            {
//                pu.IsActive = "Y" == (eu.ActiveFlag ?? "").ToUpper();
//                return pu;
//            });
//            //return Core.Utilities.Transformer<User, KMPlatform.Entity.User>.Transform(ecnUser, _transformationProperties);
//            /*return new KMPlatform.Entity.User
//            {
//                UserID = ecnUser.UserID,
//                DefaultClientID = ecnUser.DefaultClientID ?? -1,
//                DefaultClientGroupID = ecnUser.DefaultClientGroupID ?? -1,
//                FirstName = ecnUser.FirstName,
//                LastName = ecnUser.LastName,
//                UserName = ecnUser.UserName,
//                Password = ecnUser.Password,
//                Salt = ecnUser.Salt,
//                EmailAddress = ecnUser.EmailAddress,
//                IsActive = "Y" == (ecnUser.ActiveFlag ?? "").ToUpper(),
//                AccessKey = ecnUser.AccessKey ?? Guid.Empty,
//                IsAccessKeyValid = ecnUser.IsAccessKeyValid ?? false,                

//                ClientGroups = ecnUser.ClientGroups ?? new List<KMPlatform.Entity.ClientGroup>(),
//                UserClientSecurityGroupMaps = ecnUser.UserClientSecurityGroupMaps ?? new List<KMPlatform.Entity.UserClientSecurityGroupMap>(),
//                CurrentClientGroup = ecnUser.CurrentClientGroup,
//                CurrentClient = ecnUser.CurrentClient,
//                CurrentSecurityGroup = ecnUser.CurrentSecurityGroup,
//                OpenedClients = ecnUser.OpenedClients ?? new List<KMPlatform.Entity.Client>(),

//                DateUpdated = ecnUser.UpdatedDate,
//                UpdatedByUserID = ecnUser.UpdatedUserID,
//                DateCreated = ecnUser.CreatedDate ?? DateTime.Now,
//                CreatedByUserID = ecnUser.CreatedUserID ?? -1,
//            };*/
//        }

//        public User()
//        {
//            // platform user compatibility
//            DefaultClientGroupID = -1;
//            DefaultClientID = 0;
//            FirstName = string.Empty;
//            LastName = string.Empty;
//            UserName = string.Empty;
//            Password = string.Empty;
//            Salt = string.Empty;
//            EmailAddress = string.Empty;
//            ClientGroups = new List<KMPlatform.Entity.ClientGroup>();
//            UserClientSecurityGroupMaps = new List<KMPlatform.Entity.UserClientSecurityGroupMap>();
//            CurrentClientGroup = null;
//            CurrentClient = null;
//            CurrentSecurityGroup = null;
//            OpenedClients = new List<KMPlatform.Entity.Client>();

//            UserID = -1;
//            CustomerID = null;
//            UserName = string.Empty;
//            Password = string.Empty;

//            ActiveFlag = string.Empty;

//            AcceptTermsDate = null;
//            RoleID = null;

//            AccessKey = null;
//            CreatedUserID = null;
//            CreatedDate = null;
//            UpdatedUserID = null;
//            UpdatedDate = null;
//            IsDeleted = null;

//            IsSysAdmin = false;
//            IsChannelAdmin = false;
//            IsAdmin = false;
//            HasUserAccess = false;
//            HasCustomerAccess = false;
//            HasChannelAccess = false;

//            CommunicatorOptions = string.Empty;
//            CollectorOptions = string.Empty;
//            CreatorOptions = string.Empty;
//            AccountsOptions = string.Empty;

//            UserAction = new List<UserAction>();
//            UserGroup = new List<ECN_Framework_Entities.Communicator.UserGroup>();

//            Applications = new List<KMPlatform.Entity.Application>();
//            MenuFeatures = new Dictionary<string, List<KMPlatform.Entity.MenuFeature>>();
//            Services = new List<KMPlatform.Entity.Service>();
//            ServiceFeatures = new Dictionary<string, List<KMPlatform.Entity.ServiceFeature>>();
//        }

//        // platform user compatibility
//        [DataMember]
//        public bool? IsAccessKeyValid { get; set; }
//        [DataMember]
//        public int? DefaultClientID { get; set; }
//        [DataMember]
//        public int? DefaultClientGroupID { get; set; }
//        [DataMember]
//        public string FirstName { get; set; }
//        [DataMember]
//        public string LastName { get; set; }
//        [DataMember]
//        public string Salt { get; set; }
//        [DataMember]
//        public string EmailAddress { get; set; }
//        [DataMember]
//        public List<KMPlatform.Entity.ClientGroup> ClientGroups { get; set; }
//        [DataMember]
//        public List<KMPlatform.Entity.UserClientSecurityGroupMap> UserClientSecurityGroupMaps { get; set; }
//        [DataMember]
//        public KMPlatform.Entity.ClientGroup CurrentClientGroup { get; set; }
//        [DataMember]
//        public KMPlatform.Entity.Client CurrentClient { get; set; }
//        [DataMember]
//        public KMPlatform.Entity.SecurityGroup CurrentSecurityGroup { get; set; }
//        [DataMember]
//        public List<KMPlatform.Entity.Client> OpenedClients { get; set; }


//        [DataMember]
//        public int UserID { get; set; }
//        [DataMember]
//        public int? CustomerID { get; set; }
//        [DataMember]
//        public int? DefaultCustomerID { get; set; }
//        [DataMember]
//        public string UserName { get; set; }
//        [DataMember]
//        public string Password { get; set; }
        
//        [DataMember]
//        public string ActiveFlag { get; set; }
//        [DataMember]
//        public DateTime? AcceptTermsDate { get; set; }
//        [DataMember]
//        public int? RoleID { get; set; }
//        [DataMember]
//        public Guid? AccessKey { get; set; }
//        [DataMember]
//        public int? CreatedUserID { get; set; }
//        [DataMember]
//        public DateTime? CreatedDate { get; private set; }
//        [DataMember]
//        public int? UpdatedUserID { get; set; }
//        [DataMember]
//        public DateTime? UpdatedDate { get; private set; }
//        [DataMember]
//        public bool? IsDeleted { get; set; }

//        // these all need to be turned into virtual accessors that calculate the expected values
//        // based on the platform user role/permission system
//        public List<ECN_Framework_Entities.Accounts.UserAction> UserAction { get; set; }
//        public List<ECN_Framework_Entities.Communicator.UserGroup> UserGroup { get; set; }
//        [DataMember]
//        public string CommunicatorOptions { get; set; }
//        [DataMember]
//        public string CollectorOptions { get; set; }
//        [DataMember]
//        public string CreatorOptions { get; set; }
//        [DataMember]
//        public string AccountsOptions { get; set; }
//        //Roles
//        public bool IsSysAdmin { 
//            get{
//                return true;
//            }
//            set { }
//        }
//        public bool IsChannelAdmin { get; set; }
//        public bool IsAdmin { get; set; }
//        public bool HasUserAccess { get; set; }
//        public bool HasCustomerAccess { get; set; }
//        public bool HasChannelAccess { get; set; }

//        [DataMember]
//        public List<KMPlatform.Entity.Application> Applications { get; set; }
//        [DataMember]
//        public Dictionary<string, List<KMPlatform.Entity.MenuFeature>> MenuFeatures { get; set; }
//        [DataMember]
//        public List<KMPlatform.Entity.Service> Services { get; set; }
//        [DataMember]
//        public Dictionary<string, List<KMPlatform.Entity.ServiceFeature>> ServiceFeatures { get; set; }
//    }
//}
