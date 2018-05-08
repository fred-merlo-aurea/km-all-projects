using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace KMPlatform.Entity
{
    [Serializable]
    [DataContract]
    public class Application
    {
        public Application() 
        {
            ApplicationID = 0;
            ApplicationName = string.Empty;
            Description = string.Empty;
            ApplicationCode = string.Empty;
            DefaultView = string.Empty;
            IsActive = false;
            IconFullName = string.Empty;
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
            Menus = new List<Menu>();
            FromEmailAddress = string.Empty;
            ErrorEmailAddress = string.Empty;
        }
        #region Properties
        [DataMember]
        public int ApplicationID { get; set; }
        [DataMember]
        public string ApplicationName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string ApplicationCode { get; set; }
        [DataMember]
        public string DefaultView { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string IconFullName { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int UpdatedByUserID { get; set; }
        [DataMember]
        public string FromEmailAddress { get; set; }
        [DataMember]
        public string ErrorEmailAddress { get; set; }
        #endregion

        [DataMember]
        public List<Menu> Menus { get; set; }
    }
}
