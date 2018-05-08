using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class RecordIdentifier
    {
        public RecordIdentifier() 
        {
            SORecordIdentifier = new Guid();
            STRecordIdentifier = new Guid();
            SFRecordIdentifier = new Guid();
            IGrp_No = new Guid();
        }
        #region Properties
        [DataMember]
        public Guid SORecordIdentifier { get; set; }
        [DataMember]
        public Guid STRecordIdentifier { get; set; }
        [DataMember]
        public Guid SFRecordIdentifier { get; set; }
        [DataMember]
        public Guid IGrp_No { get; set; }
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
                hash = hash * mult + SORecordIdentifier.GetHashCode();
                hash = hash * mult + STRecordIdentifier.GetHashCode();
                hash = hash * mult + SFRecordIdentifier.GetHashCode();
                hash = hash * mult + IGrp_No.GetHashCode();
                return hash;
            }
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as RecordIdentifier);
        }
        public bool Equals(RecordIdentifier other)
        {
            // Check for null
            if (ReferenceEquals(other, null))
                return false;

            // Check for same reference
            if (ReferenceEquals(this, other))
                return true;

            // Check for same Values
            return (SORecordIdentifier == other.SORecordIdentifier &&
                   STRecordIdentifier == other.STRecordIdentifier &&
                   SFRecordIdentifier == other.SFRecordIdentifier &&
                   IGrp_No == other.IGrp_No);
        }
        #endregion
    }
}
