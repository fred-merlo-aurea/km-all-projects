using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class TransformSplitTrans
    {
        #region Properties  
        [DataMember]
        public int SplitTransformID { get; set; }
        [DataMember]
        public int TransformationID { get; set; }
        [DataMember]
        public int SplitBeforeID { get; set; }
        [DataMember]
        public int DataMapID { get; set; }
        [DataMember]
        public int SplitAfterID { get; set; }
        [DataMember]
        public string Column { get; set; }
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
                hash = hash * mult + SplitTransformID.GetHashCode();
                hash = hash * mult + TransformationID.GetHashCode();
                hash = hash * mult + SplitBeforeID.GetHashCode();
                hash = hash * mult + DataMapID.GetHashCode();
                hash = hash * mult + SplitAfterID.GetHashCode();
                hash = hash * mult + Column.GetHashCode();
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
            return Equals(obj as TransformSplitTrans);
        }
        public bool Equals(TransformSplitTrans other)
        {
            // Check for null
            if (ReferenceEquals(other, null))
                return false;

            // Check for same reference
            if (ReferenceEquals(this, other))
                return true;

            // Check for same Values
            return (SplitTransformID == other.SplitTransformID &&
                   TransformationID == other.TransformationID &&
                   SplitBeforeID == other.SplitBeforeID &&
                   DataMapID == other.DataMapID &&
                   SplitAfterID == other.SplitAfterID &&
                   Column == other.Column &&
                   IsActive == other.IsActive &&
                   DateCreated == other.DateCreated &&
                   DateUpdated == other.DateUpdated &&
                   CreatedByUserID == other.CreatedByUserID &&
                   UpdatedByUserID == other.UpdatedByUserID);
        }
        #endregion
    }
}
