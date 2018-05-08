using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class ImportError
    {
        public ImportError() 
        {
            ImportErrorID = 0;
            SourceFileID = 0;
            RowNumber = 0;
            FormattedException = string.Empty;
            ClientMessage = string.Empty;
            MAFField = string.Empty;
            BadDataRow = string.Empty;
            ThreadID = 0;
            DateCreated = DateTime.Now;
            ProcessCode = string.Empty;
            IsDimensionError = false;
            IsFatalError = false;
        }
        public ImportError(int threadID, string processCode, int sourceFileID, string clientMessage, string mafField = "", int rowNumber = 0, string formattedException = "", string badDataRow = "", bool isDimensionError = false) 
        {
            SourceFileID = sourceFileID;
            RowNumber = rowNumber;
            FormattedException = formattedException;
            ClientMessage = clientMessage;
            MAFField = mafField;
            BadDataRow = badDataRow;
            ThreadID = threadID;
            DateCreated = DateTime.Now;
            ProcessCode = processCode;
            IsDimensionError = isDimensionError;
        }

        #region Properties
        [DataMember]
        public int ImportErrorID { get; set; }
        [DataMember]
        public int SourceFileID { get; set; }
        [DataMember]
        public int RowNumber { get; set; }
        [DataMember]
        public string FormattedException { get; set; }
        [DataMember]
        public string ClientMessage { get; set; }
        [DataMember]
        public string MAFField { get; set; }
        [DataMember]
        public string BadDataRow { get; set; }
        [DataMember]
        public int ThreadID { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public string ProcessCode { get; set; }
        [DataMember]
        public bool IsDimensionError { get; set; }
        [DataMember]
        public bool IsFatalError { get; set; }
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
                hash = hash * mult + ImportErrorID.GetHashCode();
                hash = hash * mult + SourceFileID.GetHashCode();
                hash = hash * mult + RowNumber.GetHashCode();
                hash = hash * mult + FormattedException.GetHashCode();
                hash = hash * mult + ClientMessage.GetHashCode();
                hash = hash * mult + MAFField.GetHashCode();
                hash = hash * mult + BadDataRow.GetHashCode();
                hash = hash * mult + ThreadID.GetHashCode();
                hash = hash * mult + DateCreated.GetHashCode();
                hash = hash * mult + ProcessCode.GetHashCode();
                hash = hash * mult + IsDimensionError.GetHashCode();
                hash = hash * mult + IsFatalError.GetHashCode();
                return hash;
            }
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as ImportError);
        }
        public bool Equals(ImportError other)
        {
            // Check for null
            if (ReferenceEquals(other, null))
                return false;

            // Check for same reference
            if (ReferenceEquals(this, other))
                return true;

            // Check for same Values
            return (ImportErrorID == other.ImportErrorID &&
                    SourceFileID == other.SourceFileID &&
                    RowNumber == other.RowNumber &&
                    FormattedException == other.FormattedException &&
                    ClientMessage == other.ClientMessage &&
                    MAFField == other.MAFField &&
                    BadDataRow == other.BadDataRow &&
                    ThreadID == other.ThreadID &&
                    DateCreated == other.DateCreated &&
                    ProcessCode == other.ProcessCode &&
                    IsDimensionError == other.IsDimensionError &&
                    IsFatalError == other.IsFatalError);
        }
        #endregion
    }
}
