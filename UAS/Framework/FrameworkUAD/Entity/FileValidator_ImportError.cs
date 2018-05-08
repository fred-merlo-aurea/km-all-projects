using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class FileValidator_ImportError
    {
        public FileValidator_ImportError() { }
        public FileValidator_ImportError(int threadID, string processCode, int sourceFileID, string clientMessage, string mafField = "", int rowNumber = 0, string formattedException = "", string badDataRow = "", bool isDimensionError = false) 
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
        public int FV_ImportErrorID { get; set; }
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
        #endregion
    }
}
