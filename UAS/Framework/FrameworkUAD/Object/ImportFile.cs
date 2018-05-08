using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using FrameworkUAD.Entity;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class ImportFile
    {
        public ImportFile()
        {
            DataOriginal = new Dictionary<int, StringDictionary>();
            HeadersOriginal = new StringDictionary();
            ProcessFile = null;
            ClientId = 0;
            SourceFileId = 0;
            TotalRowCount = 0;
            OriginalRowCount = 0;
            ImportErrorCount = 0;
            ImportedRowCount = 0;
            HasError = false;
            ProcessCode = string.Empty;
            ImportErrors = new HashSet<ImportError>();
            DataTransformed = new Dictionary<int, StringDictionary>();
            HeadersTransformed = new StringDictionary();
            TransformedRowCount = 0;
            TransformedRowToOriginalRowMap = new Dictionary<int, int>();
            OriginalDuplicateRecordCount = 0;
            TransformedDuplicateRecordCount = 0;
            ThreadId = 0;
            BadDataOriginalHeaders = new StringDictionary();
        }
        public ImportFile(FileInfo importFile)
        {
            DataOriginal = new Dictionary<int, StringDictionary>();
            HeadersOriginal = new StringDictionary();
            ProcessFile = importFile;
            ClientId = 0;
            SourceFileId = 0;
            TotalRowCount = 0;
            OriginalRowCount = 0;
            ImportErrorCount = 0;
            ImportedRowCount = 0;
            HasError = false;
            ProcessCode = string.Empty;
            ImportErrors = new HashSet<ImportError>();
            DataTransformed = new Dictionary<int, StringDictionary>();
            HeadersTransformed = new StringDictionary();
            TransformedRowCount = 0;
            TransformedRowToOriginalRowMap = new Dictionary<int, int>();
            ThreadId = 0;
            BadDataOriginalHeaders = new StringDictionary();
        }
        #region Properties
        [DataMember]
        public Dictionary<int, StringDictionary> DataOriginal { get; set; }
        [DataMember]
        public StringDictionary HeadersOriginal { get; set; }
        [DataMember]
        public FileInfo ProcessFile { get; set; }
        [DataMember]
        public int ClientId { get; set; }
        [DataMember]
        public int SourceFileId { get; set; }
        [DataMember]
        public int TotalRowCount { get; set; }
        [DataMember]
        public int ImportedRowCount { get; set; }
        [DataMember]
        public int OriginalRowCount { get; set; }
        [DataMember]
        public int ImportErrorCount { get; set; }
        [DataMember]
        public bool HasError { get; set; }
        [DataMember]
        public string ProcessCode { get; set; }
        [DataMember]
        public HashSet<ImportError> ImportErrors { get; set; }
        [DataMember]
        public Dictionary<int, StringDictionary> DataTransformed { get; set; }
        [DataMember]
        public StringDictionary HeadersTransformed { get; set; }
        [DataMember]
        public int TransformedRowCount { get; set; }
        [DataMember]
        public int OriginalDuplicateRecordCount { get; set; }
        [DataMember]
        public int TransformedDuplicateRecordCount { get; set; }
        [DataMember]
        public int ThreadId { get; set; }
        [DataMember]
        public StringDictionary BadDataOriginalHeaders { get; set; }

        public Dictionary<int, int> TransformedRowToOriginalRowMap { get; set; }
        #endregion
    }
}
