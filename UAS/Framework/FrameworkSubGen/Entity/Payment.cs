using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace FrameworkSubGen.Entity
{
    [Serializable]
    [DataContract]
    public class Payment : IEntity
    {
        public Payment() 
        {
            amount = 0;
            notes = string.Empty;
            transaction_id = string.Empty;
            type = Enums.PaymentType.Credit;
            date_created = DateTime.Now;
            STRecordIdentifier = new Guid();
        }
        #region Properties
        [DataMember]
        public double amount { get; set; }
        [DataMember]
        [StringLength(15, ErrorMessage = "The value cannot exceed 15 characters.")]
        public string notes { get; set; }	//	Char(15)
        [DataMember]
        [StringLength(15, ErrorMessage = "The value cannot exceed 15 characters.")]
        public string transaction_id { get; set; }		//Char(15)
        [DataMember]
        public Entity.Enums.PaymentType type { get; set; }
        [DataMember]
        public int account_id { get; set; }
        [DataMember]
        public int order_id { get; set; }
        [DataMember]
        public int subscriber_id { get; set; }
        [DataMember]
        public int subscription_id { get; set; }
        [DataMember]
        public DateTime date_created { get; set; }
        [DataMember]
        public Guid STRecordIdentifier { get; set; }
        [DataMember]
        public int bundle_id { get; set; }
        #endregion
    }
}
