using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class TransformAssign
    {
        public TransformAssign() { }
        #region Properties
        [DataMember]
        public int TransformAssignID { get; set; }
        [DataMember]
        public int TransformationID { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public bool HasPubID { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        [DataMember]
        public int PubID { get; set; }
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
                hash = hash * mult + TransformAssignID.GetHashCode();
                hash = hash * mult + TransformationID.GetHashCode();
                hash = hash * mult + Value.GetHashCode();
                hash = hash * mult + IsActive.GetHashCode();
                hash = hash * mult + HasPubID.GetHashCode();
                hash = hash * mult + DateCreated.GetHashCode();
                hash = hash * mult + DateUpdated.GetHashCode();
                hash = hash * mult + CreatedByUserID.GetHashCode();
                hash = hash * mult + UpdatedByUserID.GetHashCode();
                return hash;
            }
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as TransformAssign);
        }
        public bool Equals(TransformAssign other)
        {
            // Check for null
            if (ReferenceEquals(other, null))
                return false;

            // Check for same reference
            if (ReferenceEquals(this, other))
                return true;

            // Check for same Values
            return (TransformAssignID == other.TransformAssignID &&
                   TransformationID == other.TransformationID &&
                   Value == other.Value &&
                   IsActive == other.IsActive &&
                   HasPubID == other.HasPubID &&
                   DateCreated == other.DateCreated &&
                   DateUpdated == other.DateUpdated &&
                   CreatedByUserID == other.CreatedByUserID &&
                   UpdatedByUserID == other.UpdatedByUserID);
        }
        #endregion
    }
}
