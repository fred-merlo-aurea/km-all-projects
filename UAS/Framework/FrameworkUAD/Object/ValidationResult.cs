using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class ValidationResult
    {
        public ValidationResult() 
        {
            ImportFile = null;
            SourceFileID = 0;
            ProcessCode = string.Empty;
            OriginalRowCount = 0;
            TransformedRowCount = 0;
            OriginalDuplicateRecordCount = 0;
            TransformedDuplicateRecordCount = 0;
            TotalRowCount = 0;
            HasError = false;
            RecordImportErrorCount = 0;
            DimensionImportErrorCount = 0;
            ImportedRowCount = 0;
            UnexpectedColumns = new HashSet<string>();
            NotFoundColumns = new HashSet<string>();
            DuplicateColumns = new HashSet<string>();
            RecordImportErrors = new HashSet<Entity.ImportError>();
            HeadersOriginal = new StringDictionary();
            HeadersTransformed = new StringDictionary();
            DimensionImportErrorSummaries = new HashSet<ImportErrorSummary>();
        }
        public ValidationResult(FileInfo importFile, int sourceFileID, string processCode,
                                int originalRowCount = 0, int transformedRowCount = 0, int totalRowCount = 0, bool hasError = false, int importErrorCount = 0, int importedRowCount =0,int dimensionImportErrorCount = 0,
                                List<string> unexpectedColumns = null, List<string> notFoundColumns = null, List<string> duplicateColumns = null, List<Entity.ImportError> importErrors = null,List<ImportErrorSummary> importErrorSummaries = null)
        {
            ImportFile = importFile;
            SourceFileID = sourceFileID;
            ProcessCode = processCode;
            OriginalRowCount = originalRowCount;
            TransformedRowCount = transformedRowCount;
            OriginalDuplicateRecordCount = 0;
            TransformedDuplicateRecordCount = 0;
            TotalRowCount = totalRowCount;
            HasError = hasError;
            ImportedRowCount = importedRowCount;
            DimensionImportErrorCount = dimensionImportErrorCount;
            RecordImportErrorCount = importErrorCount;
            UnexpectedColumns = unexpectedColumns == null ? new HashSet<string>() : new  HashSet<string>(unexpectedColumns);
            NotFoundColumns = notFoundColumns == null ? new HashSet<string>() : new HashSet<string>(notFoundColumns);
            DuplicateColumns = duplicateColumns == null ? new HashSet<string>() : new HashSet<string>(duplicateColumns);
            RecordImportErrors = importErrors == null ? new HashSet<Entity.ImportError>() : new HashSet<Entity.ImportError>(importErrors);
            HeadersOriginal = new StringDictionary();
            HeadersTransformed = new StringDictionary();
            DimensionImportErrorSummaries = importErrorSummaries == null ? new HashSet<ImportErrorSummary>() : new HashSet<ImportErrorSummary>(importErrorSummaries);
        }
        #region Properties
        [DataMember]
        public FileInfo ImportFile { get; set; }
        [DataMember]
        public int SourceFileID { get; set; }
        [DataMember]
        public string ProcessCode { get; set; }
        [DataMember]
        public int OriginalRowCount { get; set; }
        [DataMember]
        public int TransformedRowCount { get; set; }
        [DataMember]
        public int OriginalDuplicateRecordCount { get; set; }
        [DataMember]
        public int TransformedDuplicateRecordCount { get; set; }
        [DataMember]
        public int TotalRowCount { get; set; }
        [DataMember]
        public int ImportedRowCount { get; set; }
        [DataMember]
        public bool HasError { get; set; }
        [DataMember]
        public int RecordImportErrorCount { get; set; }
        [DataMember]
        public int DimensionImportErrorCount { get; set; }

        [DataMember]
        public StringDictionary HeadersOriginal { get; set; }
        [DataMember]
        public StringDictionary HeadersTransformed { get; set; }
        [DataMember]
        public HashSet<string> UnexpectedColumns { get; set; }
        [DataMember]
        public HashSet<string> NotFoundColumns { get; set; }
        [DataMember]
        public HashSet<string> DuplicateColumns { get; set; }
        [DataMember]
        public HashSet<FrameworkUAD.Entity.ImportError> RecordImportErrors { get; set; }
        [DataMember]
        public HashSet<FrameworkUAD.Object.ImportErrorSummary> DimensionImportErrorSummaries { get; set; }
        [DataMember]
        public StringDictionary BadDataOriginalHeaders { get; set; }        
        #endregion
    }
}
