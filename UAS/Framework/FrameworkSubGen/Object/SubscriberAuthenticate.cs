using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace FrameworkSubGen.Object
{
    [Serializable]
    [DataContract]
    public class SubscriberAuthenticate
    {
        public SubscriberAuthenticate()
        {
            subscribers = new List<Entity.Subscriber>();
        }
        #region Properties
        [DataMember]
        [StringLength(50, ErrorMessage = "The value cannot exceed 50 characters.")]
        public string login { get; set; }
        [DataMember]
        [StringLength(50, ErrorMessage = "The value cannot exceed 50 characters.")]
        public string password { get; set; }
        [DataMember]
        public bool authenticated { get; set; }

        [DataMember]
        public List<Entity.Subscriber> subscribers { get; set; }
        #endregion

        #region old
        //[DataMember]
        //public int subscriber_id { get; set; }
        //[DataMember]
        //[StringLength(5, ErrorMessage = "The value cannot exceed 5 characters.")]
        //public string renewal_code { get; set; }
        //[DataMember]

        //[DataMember]

        //[DataMember]
        //public int subscription_id { get; set; }
        //[DataMember]
        //public DateTime date_created { get; set; }
        //[DataMember]
        //public DateTime date_expired { get; set; }
        //[DataMember]
        //public int publication_id { get; set; }
        //[DataMember]
        //public int issues_left { get; set; }
        //[DataMember]
        //public Entity.Enums.SubscriptionType type { get; set; }
        #endregion
    }
}
