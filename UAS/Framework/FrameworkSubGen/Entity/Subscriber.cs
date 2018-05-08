using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using KM.Common.Functions;

namespace FrameworkSubGen.Entity
{
    [Serializable]
    [DataContract]
    public class Subscriber : IEntity
    {
        public Subscriber()
        {
            create_date = DateTimeFunctions.GetMinDate();
            delete_date = DateTimeFunctions.GetMinDate();
            //subscriptions = new List<Subscription>();
        }
        #region Properties
        [DataMember]
        public int subscriber_id { get; set; }
        [DataMember]
        [StringLength(5, ErrorMessage = "The value cannot exceed 5 characters.")]
        public string renewal_code { get; set; }
        [DataMember]
        [StringLength(100, ErrorMessage = "The value cannot exceed 100 characters.")]
        public string email { get; set; }
        [DataMember]
        [StringLength(25, ErrorMessage = "The value cannot exceed 25 characters.")]
        public string password { get; set; }
        [DataMember]
        [StringLength(32, ErrorMessage = "The value cannot exceed 32 characters.")]
        public string password_md5 { get; set; }
        [DataMember]
        [StringLength(25, ErrorMessage = "The value cannot exceed 25 characters.")]
        public string first_name { get; set; }
        [DataMember]
        [StringLength(25, ErrorMessage = "The value cannot exceed 25 characters.")]
        public string last_name { get; set; }
        [DataMember]
        [StringLength(100, ErrorMessage = "The value cannot exceed 100 characters.")]
        public string source { get; set; }
        [DataMember]
        public DateTime create_date { get; set; }
        [DataMember]
        public DateTime delete_date { get; set; }

        //[DataMember]
        //public List<Subscription> subscriptions { get; set; }
        [DataMember]
        public int account_id { get; set; }
        #endregion
    }
}
