using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FrameworkSubGen.Entity
{
    [Serializable]
    [DataContract]
    public class ChangeDataCapture
    {
        public ChangeDataCapture()
        {
            custom_fields = new List<CustomField>();
            subscriptions = new List<Subscription>();
            subscribers = new List<Subscriber>();
            addresses = new List<Address>();
            list_downloads = new List<MailingList>();
            bundles = new List<Bundle>();
        }
        #region Properties
        [DataMember]
        public List<CustomField> custom_fields { get; set; }
        [DataMember]
        public List<Subscription> subscriptions { get; set; }
        [DataMember]
        public List<Subscriber> subscribers { get; set; }
        [DataMember]
        public List<Address> addresses { get; set; }
        [DataMember]
        public List<MailingList> list_downloads { get; set; }
        [DataMember]
        public List<Purchase> purchases { get; set; }
        [DataMember]
        public List<Bundle> bundles { get; set; }
        [DataMember]
        public int account_id { get; set; }
        #endregion
    }
}
