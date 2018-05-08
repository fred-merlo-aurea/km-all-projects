using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class DataMapping
    {
        public DataMapping() { }
        #region Properties
        [DataMember]
        public int DataMappingID { get; set; }
        [DataMember]
        public int FieldMapping { get; set; }
        [DataMember]
        public string IncomingValue { get; set; }
        [DataMember]
        public string MAFValue { get; set; }
        [DataMember]
        public bool Ignore { get; set; }
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
                hash = hash * mult + DataMappingID.GetHashCode();
                hash = hash * mult + FieldMapping.GetHashCode();
                hash = hash * mult + IncomingValue.GetHashCode();
                hash = hash * mult + MAFValue.GetHashCode();
                hash = hash * mult + Ignore.GetHashCode();
                hash = hash * mult + DateCreated.GetHashCode();
                hash = hash * mult + DateUpdated.GetHashCode();
                hash = hash * mult + CreatedByUserID.GetHashCode();
                hash = hash * mult + UpdatedByUserID.GetHashCode();
                return hash;
            }
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as DataMapping);
        }
        public bool Equals(DataMapping other)
        {
            // Check for null
            if (ReferenceEquals(other, null))
                return false;

            // Check for same reference
            if (ReferenceEquals(this, other))
                return true;

            // Check for same Values
            return (DataMappingID == other.DataMappingID &&
                   FieldMapping == other.FieldMapping &&
                   IncomingValue == other.IncomingValue &&
                   MAFValue == other.MAFValue &&
                   Ignore == other.Ignore &&
                   DateCreated == other.DateCreated &&
                   DateUpdated == other.DateUpdated &&
                   CreatedByUserID == other.CreatedByUserID &&
                   UpdatedByUserID == other.UpdatedByUserID);
        }
        #endregion
    }
}
