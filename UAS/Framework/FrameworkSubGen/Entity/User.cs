using System;
using System.Runtime.Serialization;

namespace FrameworkSubGen.Entity
{
    [Serializable]
    [DataContract]
    public class User : IEntity
    {
        public User() { }
        #region Properties
        [DataMember]
        public int user_id { get; set; }
        [DataMember]
        public string first_name { get; set; }
        [DataMember]
        public string last_name { get; set; }
        [DataMember]
        public string password { get; set; }
        [DataMember]
        public string password_md5 { get; set; }
        [DataMember]
        public string email { get; set; }
        [DataMember]
        public bool is_admin { get; set; }
        [DataMember]
        public int account_id { get; set; }
        [DataMember]
        public int KMUserID { get; set; }
        #endregion
    }
}
