using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkSubGen.Entity
{
    [Serializable]
    [DataContract]
    public class Purchase : IEntity
    {
        public Purchase() 
        {
            billing_address_id = 0;
            bundle_id = 0;
            invoice_id = 0;
            name = string.Empty;
            subscriber_id = 0;
            account_id = 0;
            IsProcessed = false;
            ProcessedDate = DateTime.Now;
        }
        #region Properties
        [DataMember]
        public int billing_address_id { get; set; }
        [DataMember]
        public int bundle_id { get; set; }
        [DataMember]
        public int invoice_id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public int subscriber_id { get; set; }
        [DataMember]
        public int account_id { get; set; }
        [DataMember]
        public bool IsProcessed { get; set; }
        [DataMember]
        public DateTime ProcessedDate { get; set; }
        #endregion
    }
}
