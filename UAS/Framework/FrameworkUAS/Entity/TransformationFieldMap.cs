using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{    
    [Serializable]
    [DataContract]
    public class TransformationFieldMap
    {
        public TransformationFieldMap() { }
        #region Properties
        [DataMember]
        public int TransformationFieldMapID { get; set; }
        [DataMember]
        public int TransformationID { get; set; }
        [DataMember]
        public int SourceFileID { get; set; }
        [DataMember]
        public int FieldMappingID { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
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
                hash = hash * mult + TransformationFieldMapID.GetHashCode();
                hash = hash * mult + TransformationID.GetHashCode();
                hash = hash * mult + SourceFileID.GetHashCode();
                hash = hash * mult + FieldMappingID.GetHashCode();
                hash = hash * mult + IsActive.GetHashCode();
                hash = hash * mult + DateCreated.GetHashCode();
                hash = hash * mult + DateUpdated.GetHashCode();
                hash = hash * mult + CreatedByUserID.GetHashCode();
                hash = hash * mult + UpdatedByUserID.GetHashCode();
                return hash;
            }
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as TransformationFieldMap);
        }
        public bool Equals(TransformationFieldMap other)
        {
            // Check for null
            if (ReferenceEquals(other, null))
                return false;

            // Check for same reference
            if (ReferenceEquals(this, other))
                return true;

            // Check for same Values
            return (TransformationFieldMapID == other.TransformationFieldMapID &&
                   TransformationID == other.TransformationID &&
                   SourceFileID == other.SourceFileID &&
                   FieldMappingID == other.FieldMappingID &&
                   IsActive == other.IsActive &&
                   DateCreated == other.DateCreated &&
                   DateUpdated == other.DateUpdated &&
                   CreatedByUserID == other.CreatedByUserID &&
                   UpdatedByUserID == other.UpdatedByUserID);
        }
        #endregion
    }
}
