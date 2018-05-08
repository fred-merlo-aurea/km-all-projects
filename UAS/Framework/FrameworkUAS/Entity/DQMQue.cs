using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class DQMQue
    {
        public DQMQue() { }
        public DQMQue(string processCode, int clientID, bool isDemo, bool isADMS, int sourceFileId) 
        {
            ProcessCode = processCode;
            ClientID = clientID;
            IsDemo = isDemo;
            IsADMS = isADMS;
            DateCreated = DateTime.Now;
            IsQued = false;
            DateQued = null;
            IsCompleted = false;
            DateCompleted = null;
            SourceFileId = sourceFileId;
        }
        #region Properties
        [DataMember]
        public string ProcessCode { get; set; }
        [DataMember]
        public int ClientID { get; set; }
        [DataMember]
        public bool IsDemo { get; set; }
        [DataMember]
        public bool IsADMS { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public bool IsQued { get; set; }
        [DataMember]
        public DateTime? DateQued { get; set; }
        [DataMember]
        public bool IsCompleted { get; set; }
        [DataMember]
        public DateTime? DateCompleted { get; set; }
        [DataMember]
        public int SourceFileId { get; set; }
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
                hash = hash * mult + ProcessCode.GetHashCode();
                hash = hash * mult + ClientID.GetHashCode();
                hash = hash * mult + IsDemo.GetHashCode();
                hash = hash * mult + IsADMS.GetHashCode();
                hash = hash * mult + DateCreated.GetHashCode();
                hash = hash * mult + IsQued.GetHashCode();
                hash = hash * mult + DateQued.GetHashCode();
                hash = hash * mult + IsCompleted.GetHashCode();
                hash = hash * mult + DateCompleted.GetHashCode();
                hash = hash * mult + SourceFileId.GetHashCode();
                return hash;
            }
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as DQMQue);
        }
        public bool Equals(DQMQue other)
        {
            // Check for null
            if (ReferenceEquals(other, null))
                return false;

            // Check for same reference
            if (ReferenceEquals(this, other))
                return true;

            // Check for same Values
            return (ProcessCode == other.ProcessCode &&
                   ClientID == other.ClientID &&
                   IsDemo == other.IsDemo &&
                   IsADMS == other.IsADMS &&
                   DateCreated == other.DateCreated &&
                   IsQued == other.IsQued &&
                   DateQued == other.DateQued &&
                   IsCompleted == other.IsCompleted &&
                   DateCompleted == other.DateCompleted &&
                   SourceFileId == other.SourceFileId);
        }
        #endregion
    }
}
