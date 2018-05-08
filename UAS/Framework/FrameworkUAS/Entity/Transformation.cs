using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class Transformation
    {
        public Transformation()
        {
            FieldMap = new HashSet<TransformationFieldMap>();
            PubMap = new HashSet<TransformationPubMap>();
        }
        #region Properties
        [DataMember]
        public int TransformationID { get; set; }
        [DataMember]
        public int TransformationTypeID { get; set; }
        [DataMember]
        public string TransformationName { get; set; }
        [DataMember]
        public string TransformationDescription { get; set; }
        [DataMember]
        public bool MapsPubCode { get; set; }
        [DataMember]
        public bool LastStepDataMap { get; set; }
        [DataMember]
        public int ClientID { get; set; }
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
        [DataMember]
        public bool IsTemplate { get; set; }
        #endregion

        [DataMember]
        public HashSet<TransformationFieldMap> FieldMap { get; set; }
        [DataMember]
        public HashSet<TransformationPubMap> PubMap { get; set; }

        #region HashSet support overrides
        //item.PubCode, item.FName, item.LName, item.Company, item.Title, item.Address, item.Email, item.Phone
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int mult = 486187739;
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * mult + TransformationID.GetHashCode();
                hash = hash * mult + TransformationTypeID.GetHashCode();
                hash = hash * mult + TransformationName.GetHashCode();
                hash = hash * mult + TransformationDescription.GetHashCode();
                hash = hash * mult + MapsPubCode.GetHashCode();
                hash = hash * mult + LastStepDataMap.GetHashCode();
                hash = hash * mult + ClientID.GetHashCode();
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
            return Equals(obj as Transformation);
        }
        public bool Equals(Transformation other)
        {
            // Check for null
            if (ReferenceEquals(other, null))
                return false;

            // Check for same reference
            if (ReferenceEquals(this, other))
                return true;

            // Check for same Values
            return (TransformationID == other.TransformationID &&
                   TransformationTypeID == other.TransformationTypeID &&
                   TransformationName == other.TransformationName &&
                   TransformationDescription == other.TransformationDescription &&
                   MapsPubCode == other.MapsPubCode &&
                   LastStepDataMap == other.LastStepDataMap &&
                   ClientID == other.ClientID &&
                   IsActive == other.IsActive &&
                   DateCreated == other.DateCreated &&
                   DateUpdated == other.DateUpdated &&
                   CreatedByUserID == other.CreatedByUserID &&
                   UpdatedByUserID == other.UpdatedByUserID);
        }
        #endregion
    }
}
