using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class TransformJoin
    {
        public TransformJoin() { }
        #region Properties
        [DataMember]
        public int TransformJoinID { get; set; }
        [DataMember]
        public int TransformationID { get; set; }
        [DataMember]
        public string ColumnsToJoin { get; set; } 
        [DataMember]
        public string Delimiter { get; set; }
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
                hash = hash * mult + TransformJoinID.GetHashCode();
                hash = hash * mult + TransformationID.GetHashCode();
                hash = hash * mult + ColumnsToJoin.GetHashCode();
                hash = hash * mult + Delimiter.GetHashCode();
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
            return Equals(obj as TransformJoin);
        }
        public bool Equals(TransformJoin other)
        {
            // Check for null
            if (ReferenceEquals(other, null))
                return false;

            // Check for same reference
            if (ReferenceEquals(this, other))
                return true;

            // Check for same Values
            return (TransformJoinID == other.TransformJoinID &&
                   TransformationID == other.TransformationID &&
                   ColumnsToJoin == other.ColumnsToJoin &&
                   Delimiter == other.Delimiter &&
                   IsActive == other.IsActive &&
                   DateCreated == other.DateCreated &&
                   DateUpdated == other.DateUpdated &&
                   CreatedByUserID == other.CreatedByUserID &&
                   UpdatedByUserID == other.UpdatedByUserID);
        }
        #endregion
    }
}
