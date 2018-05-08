using System;
using System.Runtime.Serialization;
using KM.Common.Functions;

namespace FrameworkSubGen.Entity
{
    [Serializable]
    [DataContract]
    public class Account : IEntity
    {
        public Account()
        {
            created = DateTimeFunctions.GetMinDate();
        }
        #region Properties
        [DataMember]
        public int account_id { get; set; }
        [DataMember]
        public string company_name { get; set; }
        [DataMember]
        public string email { get; set; }
        [DataMember]
        public string website { get; set; }
        [DataMember]
        public bool active { get; set; }
        [DataMember]
        public bool api_active { get; set; }
        [DataMember]
        public string api_key { get; set; }
        [DataMember]
        public string api_login { get; set; }
        [DataMember]
        public Enums.Plan plan { get; set; }
        [DataMember]
        public string audit_type { get; set; }
        [DataMember]
        public DateTime created { get; set; }
        [DataMember]
        public string master_checkout { get; set; }
        [DataMember]
        public int KMClientId { get; set; }
        #endregion
    }
}
