﻿using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{    
    [Serializable]
    [DataContract]
    public class TransformationFieldMultiMap
    {
        public TransformationFieldMultiMap() { }
        #region Properties
        [DataMember]
        public int TransformationFieldMultiMapID { get; set; }
        [DataMember]
        public int TransformationID { get; set; }
        [DataMember]
        public int SourceFileID { get; set; }
        [DataMember]
        public int FieldMappingID { get; set; }
        [DataMember]
        public int FieldMultiMapID { get; set; }
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
                hash = hash * mult + TransformationFieldMultiMapID.GetHashCode();
                hash = hash * mult + TransformationID.GetHashCode();
                hash = hash * mult + SourceFileID.GetHashCode();
                hash = hash * mult + FieldMappingID.GetHashCode();
                hash = hash * mult + FieldMultiMapID.GetHashCode();
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
            return Equals(obj as TransformationFieldMultiMap);
        }
        public bool Equals(TransformationFieldMultiMap other)
        {
            // Check for null
            if (ReferenceEquals(other, null))
                return false;

            // Check for same reference
            if (ReferenceEquals(this, other))
                return true;

            // Check for same Values
            return (TransformationFieldMultiMapID == other.TransformationFieldMultiMapID &&
                   TransformationID == other.TransformationID &&
                   SourceFileID == other.SourceFileID &&
                   FieldMappingID == other.FieldMappingID &&
                   FieldMultiMapID == other.FieldMultiMapID &&
                   IsActive == other.IsActive &&
                   DateCreated == other.DateCreated &&
                   DateUpdated == other.DateUpdated &&
                   CreatedByUserID == other.CreatedByUserID &&
                   UpdatedByUserID == other.UpdatedByUserID);
        }
        #endregion
    }
}
