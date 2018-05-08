using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FrameworkSubGen.Entity
{
    [Serializable]
    [DataContract]
    public class ImportDimension
    {
        public ImportDimension()
        {
            Dimensions = new List<ImportDimensionDetail>();
            DateCreated = DateTime.Now;
            DateUpdated = null;
            IsMergedToUAD = false;
            DateMergedToUAD = null;
        }
        #region Properties
        [DataMember]
        public int ImportDimensionId { get; set; }
        [DataMember]
        public int SystemSubscriberID { get; set; }
        [DataMember]
        public int SubscriptionID { get; set; }
        [DataMember]
        public int PublicationID { get; set; }
        [DataMember]
        public int account_id { get; set; }
        [DataMember]
        public List<ImportDimensionDetail> Dimensions { get; set; }

        //columns for sending to UAD
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public bool IsMergedToUAD { get; set; }
        [DataMember]
        public DateTime? DateMergedToUAD { get; set; }
        #endregion
    }
}
