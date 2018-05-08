using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkSubGen.Entity
{
    [Serializable]
    [DataContract]
    public class ImportDimensionDetail
    {
        public ImportDimensionDetail() { }
        public ImportDimensionDetail(int importDimensionId, string dimensionField, string dimensionValue, int systemSubscriberID, int subscriptionID, int publicationID)
        {
            ImportDimensionId = importDimensionId;
            DimensionField = dimensionField;
            DimensionValue = dimensionValue;
            SystemSubscriberID = systemSubscriberID;
            SubscriptionID = subscriptionID;
            PublicationID = publicationID;
        }
        #region Properties
        [DataMember]
        public int ImportDimensionId { get; set; }
        [DataMember]
        public string DimensionField { get; set; }
        [DataMember]
        public string DimensionValue { get; set; }
        [DataMember]
        public int SystemSubscriberID { get; set; }
        [DataMember]
        public int SubscriptionID { get; set; }
        [DataMember]
        public int PublicationID { get; set; }
        #endregion
    }
}
