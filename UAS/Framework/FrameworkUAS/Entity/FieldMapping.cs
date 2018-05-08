using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class FieldMapping: IFieldMapping
    {
        public FieldMapping() 
        {
            FieldMultiMappings = new HashSet<FieldMultiMap>();
        }
        #region Properties
        [DataMember]
        public int FieldMappingID { get; set; }
        [DataMember]
        public int FieldMappingTypeID { get; set; }
        [DataMember]
        public bool IsNonFileColumn { get; set; }
        [DataMember]
        public int SourceFileID { get; set; }
        [DataMember]
        public string IncomingField { get; set; }
        [DataMember]
        public string MAFField { get; set; }
        [DataMember]
        public int PubNumber { get; set; }
        [DataMember]
        public string DataType { get; set; }
        [DataMember]
        public string PreviewData { get; set; }
        [DataMember]
        public int ColumnOrder { get; set; }
        [DataMember]
        public bool HasMultiMapping { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        [DataMember]
        public int DemographicUpdateCodeId { get; set; }
        #endregion        

        [DataMember]
        public HashSet<FieldMultiMap> FieldMultiMappings { get; set; }

        #region HashSet support overrides
        //item.PubCode, item.FName, item.LName, item.Company, item.Title, item.Address, item.Email, item.Phone
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int mult = 486187739;
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * mult + FieldMappingID.GetHashCode();
                hash = hash * mult + FieldMappingTypeID.GetHashCode();
                hash = hash * mult + IsNonFileColumn.GetHashCode();
                hash = hash * mult + SourceFileID.GetHashCode();
                hash = hash * mult + IncomingField.GetHashCode();
                hash = hash * mult + MAFField.GetHashCode();
                hash = hash * mult + PubNumber.GetHashCode();
                hash = hash * mult + DataType.GetHashCode();
                hash = hash * mult + PreviewData.GetHashCode();
                hash = hash * mult + ColumnOrder.GetHashCode();
                hash = hash * mult + HasMultiMapping.GetHashCode();
                hash = hash * mult + DateCreated.GetHashCode();
                hash = hash * mult + DateUpdated.GetHashCode();
                hash = hash * mult + CreatedByUserID.GetHashCode();
                hash = hash * mult + UpdatedByUserID.GetHashCode();
                hash = hash * mult + DemographicUpdateCodeId.GetHashCode();
                return hash;
            }
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as FieldMapping);
        }
        public bool Equals(FieldMapping other)
        {
            // Check for null
            if (ReferenceEquals(other, null))
                return false;

            // Check for same reference
            if (ReferenceEquals(this, other))
                return true;

            // Check for same Values
            return (FieldMappingID == other.FieldMappingID &&
                   FieldMappingTypeID == other.FieldMappingTypeID &&
                   IsNonFileColumn == other.IsNonFileColumn &&
                   SourceFileID == other.SourceFileID &&
                   IncomingField == other.IncomingField &&
                   MAFField == other.MAFField &&
                   PubNumber == other.PubNumber &&
                   DataType == other.DataType &&
                   PreviewData == other.PreviewData &&
                   ColumnOrder == other.ColumnOrder &&
                   HasMultiMapping == other.HasMultiMapping &&
                   DateCreated == other.DateCreated &&
                   DateUpdated == other.DateUpdated &&
                   CreatedByUserID == other.CreatedByUserID &&
                   UpdatedByUserID == other.UpdatedByUserID &&
                   DemographicUpdateCodeId == other.DemographicUpdateCodeId);
        }
        #endregion
    }
}
