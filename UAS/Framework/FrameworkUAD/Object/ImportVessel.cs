using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using FrameworkUAD.Entity;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class ImportVessel
    {
        public ImportVessel()
        {
            DataOriginal = new DataTable();
            HeadersOriginal = new HashSet<string>();
            ImportFile = null;
            TotalRowCount = 0;
            OriginalRowCount = 0;
            ImportErrorCount = 0;
            ImportedRowCount = 0;
            HasError = false;
            ImportErrors = new HashSet<ImportError>();
            DataTransformed = new DataTable();
            HeadersTransformed = new HashSet<string>();
            TransformedRowCount = 0;
            TransformedRowToOriginalRowMap = new Dictionary<int, int>();
        }
        public ImportVessel(FileInfo importFile)
        {
            DataOriginal = new DataTable();
            HeadersOriginal = new HashSet<string>();
            ImportFile = importFile;
            TotalRowCount = 0;
            OriginalRowCount = 0;
            ImportErrorCount = 0;
            ImportedRowCount = 0;
            HasError = false;
            ImportErrors = new HashSet<ImportError>();
            DataTransformed = new DataTable();
            HeadersTransformed = new HashSet<string>();
            TransformedRowCount = 0;
            TransformedRowToOriginalRowMap = new Dictionary<int, int>();
        }

        #region Properties
        [DataMember]
        public DataTable DataOriginal { get; set; }
        [DataMember]
        public HashSet<string> HeadersOriginal { get; set; }
        [DataMember]
        public FileInfo ImportFile { get; set; }
        [DataMember]
        public int SourceFileID { get; set; }
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
        public DataTable DataTransformed { get; set; }
        [DataMember]
        public HashSet<string> HeadersTransformed { get; set; }
        [DataMember]
        public int TransformedRowCount { get; set; }

        /// <summary>
        /// Key = Transformed RowImportNumber, Value = Original RowImportNumber
        /// </summary>
        /// [DataMember]
        public Dictionary<int, int> TransformedRowToOriginalRowMap { get; set; }
        #endregion
    }
}
