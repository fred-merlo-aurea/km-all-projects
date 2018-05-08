using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class FieldMultiMap : IFieldMapping
    {
        public FieldMultiMap() { }
        #region Properties
        [DataMember]
        public int FieldMultiMapID { get; set; }
        [DataMember]
        public int FieldMappingID { get; set; }
        [DataMember]
        public int FieldMappingTypeID { get; set; }
        [DataMember]
        public string MAFField { get; set; }
        [DataMember]
        public string DataType { get; set; }
        [DataMember]
        public string PreviewData { get; set; }
        [DataMember]
        public int ColumnOrder { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int UpdatedByUserID { get; set; }
        #endregion
        #region HashSet support overrides
        //item.PubCode, item.FName, item.LName, item.Company, item.Title, item.Address, item.Email, item.Phone
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int mult = 486187739;
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * mult + FieldMultiMapID.GetHashCode();
                hash = hash * mult + FieldMappingID.GetHashCode();
                hash = hash * mult + FieldMappingTypeID.GetHashCode();
                hash = hash * mult + MAFField.GetHashCode();
                hash = hash * mult + DataType.GetHashCode();
                hash = hash * mult + PreviewData.GetHashCode();
                hash = hash * mult + ColumnOrder.GetHashCode();
                hash = hash * mult + DateCreated.GetHashCode();
                hash = hash * mult + DateUpdated.GetHashCode();
                hash = hash * mult + CreatedByUserID.GetHashCode();
                hash = hash * mult + UpdatedByUserID.GetHashCode();
                return hash;
            }
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as FieldMultiMap);
        }
        public bool Equals(FieldMultiMap other)
        {
            // Check for null
            if (ReferenceEquals(other, null))
                return false;

            // Check for same reference
            if (ReferenceEquals(this, other))
                return true;

            // Check for same Values
            return (FieldMultiMapID == other.FieldMultiMapID &&
                   FieldMappingID == other.FieldMappingID &&
                   FieldMappingTypeID == other.FieldMappingTypeID &&
                   MAFField == other.MAFField &&
                   DataType == other.DataType &&
                   PreviewData == other.PreviewData &&
                   ColumnOrder == other.ColumnOrder &&
                   DateCreated == other.DateCreated &&
                   DateUpdated == other.DateUpdated &&
                   CreatedByUserID == other.CreatedByUserID &&
                   UpdatedByUserID == other.UpdatedByUserID);
        }
        #endregion
    }
}
