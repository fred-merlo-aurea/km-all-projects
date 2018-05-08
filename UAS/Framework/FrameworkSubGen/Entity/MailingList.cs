using System;
using System.Runtime.Serialization;
using KM.Common.Functions;

namespace FrameworkSubGen.Entity
{
    [Serializable]
    [DataContract]
    public class MailingList : IEntity
    {
        public MailingList()
        {
            date_created = DateTimeFunctions.GetMinDate();
            grace_from_date = DateTimeFunctions.GetMinDate();
        }
        #region Properties
        [DataMember]
        public int count { get; set; }
        [DataMember]
        public DateTime date_created { get; set; }
        [DataMember]
        public DateTime grace_from_date { get; set; }
        [DataMember]
        public int grace_issues { get; set; }
        [DataMember]
        public bool is_gap { get; set; }
        [DataMember]
        public int mailing_list_id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public int parent_mailing_list_id { get; set; }
        [DataMember]
        public Enums.Status status { get; set; }
        [DataMember]
        public int account_id { get; set; }
        #endregion
    }
}
