using System;
using System.Collections.Generic;
using System.Linq;
using Harmony.MessageBus.Core;
using System.Data;

namespace Core.ADMS.Events
{
    public class RowValidated : IEventMessage
    {
        private readonly Guid _eventId;

        public RowValidated(System.IO.FileInfo fileName, KMPlatform.Entity.Client client, DataTable fileData, DataRow validatedDataRow, FrameworkUAS.Entity.SourceFile sourceFile, string processCode,
                            List<string> fileHeaderColumns, List<FrameworkUAS.Entity.FieldMapping> fieldMapping, int rowNumber, int totalRows, bool isValid)
        {
            _eventId = Guid.NewGuid();
            ImportFile = fileName;
            Client = client;
            FileData = fileData;
            ValidatedDataRow = validatedDataRow;
            SourceFile = sourceFile;
            FileHeaderColumns = fileHeaderColumns;
            FieldMapping = fieldMapping;
            RowNumber = rowNumber;
            TotalRows = totalRows;
            IsValid = isValid;
            ProcessCode = processCode;
        }

        public Guid MessageId
        {
            get
            {
                return _eventId;
            }
        }

        public System.IO.FileInfo ImportFile { get; set; }
        public KMPlatform.Entity.Client Client { get; set; }
        public DataTable FileData { get; set; }
        public DataRow ValidatedDataRow { get; set; }
        public FrameworkUAS.Entity.SourceFile SourceFile { get; set; }
        public List<string> FileHeaderColumns { get; set; }
        public List<FrameworkUAS.Entity.FieldMapping> FieldMapping { get; set; }
        public int RowNumber { get; set; }
        public int TotalRows { get; set; }
        public bool IsValid { get; set; }
        public string ProcessCode { get; set; }
    }
}
