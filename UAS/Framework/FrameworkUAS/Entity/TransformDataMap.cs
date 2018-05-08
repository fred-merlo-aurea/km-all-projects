using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class TransformDataMap
    {
        public TransformDataMap() { }
        #region Properties
        [DataMember]
        public int TransformDataMapID { get; set; }
        [DataMember]
        public int TransformationID { get; set; }
        [DataMember]
        public int PubID { get; set; }
        [DataMember]
        public string MatchType { get; set; }
        [DataMember]
        public string SourceData { get; set; }
        [DataMember]
        public string DesiredData { get; set; }
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
                hash = hash * mult + TransformDataMapID.GetHashCode();
                hash = hash * mult + TransformationID.GetHashCode();
                hash = hash * mult + PubID.GetHashCode();
                hash = hash * mult + MatchType.GetHashCode();
                hash = hash * mult + SourceData.GetHashCode();
                hash = hash * mult + DesiredData.GetHashCode();
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
            return Equals(obj as TransformDataMap);
        }
        public bool Equals(TransformDataMap other)
        {
            // Check for null
            if (ReferenceEquals(other, null))
                return false;

            // Check for same reference
            if (ReferenceEquals(this, other))
                return true;

            // Check for same Values
            return (TransformDataMapID == other.TransformDataMapID &&
                   TransformationID == other.TransformationID &&
                   PubID == other.PubID &&
                   MatchType == other.MatchType &&
                   SourceData == other.SourceData &&
                   DesiredData == other.DesiredData &&
                   IsActive == other.IsActive &&
                   DateCreated == other.DateCreated &&
                   DateUpdated == other.DateUpdated &&
                   CreatedByUserID == other.CreatedByUserID &&
                   UpdatedByUserID == other.UpdatedByUserID);
        }
        #endregion
    }
}
