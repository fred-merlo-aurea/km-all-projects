using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class ClientFTP
    {
        public ClientFTP() { }
        #region Properties
        [DataMember]
        public int FTPID { get; set; }
        [DataMember]
        public int ClientID { get; set; }
        [DataMember]
        public string Server { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string Folder { get; set; }      
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public bool IsExternal { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public bool FTPConnectionValidated { get; set; }
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
