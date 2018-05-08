using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAS.Web.Models.Circulations
{
    public class FileHistory
    {
        public FileHistory() { }

        public FileHistory(int clientID, string processCode, int recordsProcessed)
        {
            ClientID = clientID;
            ProcessCode = processCode;
            RecordsProcessed = recordsProcessed;
        }

        public FileHistory(int clientID, string processCode, int dimensionProfileCount, int dimensionRecordCount)
        {
            ClientID = clientID;
            ProcessCode = processCode;
            DimensionProfileCount = dimensionProfileCount;
            DimensionRecordCount = dimensionRecordCount;
        }

        public FileHistory(int clientID, string processCode, string appType, int pubID, string pubCode, string fileName, int fileTypeID, string fileType, DateTime dateImported, 
            string status, string failedReason, int recordsTotal, int recordsInvalid, int recordsTransformed, int recordsDuplicates, int recordsIgnored, int recordsProcessed, int dimensionProfileCount, int dimensionRecordCount,bool hasdownloadAccess)
        {
            ClientID = clientID;
            ProcessCode = processCode;
            AppType = appType;
            PubID = pubID;
            PubCode = pubCode;
            FileName = fileName;
            FileTypeID = fileTypeID;
            FileType = fileType;
            DateImported = dateImported;
            Status = status;
            FailedReason = failedReason;
            RecordsTotal = recordsTotal;
            RecordsInvalid = recordsInvalid;
            RecordsTransformed = recordsTransformed;
            RecordsDuplicates = recordsDuplicates;
            RecordsIgnored = recordsIgnored;
            RecordsProcessed = recordsProcessed;
            DimensionProfileCount = dimensionProfileCount;
            DimensionRecordCount = dimensionRecordCount;
            HasDownLoadAccess = hasdownloadAccess;
        }

        public int ClientID { get; set; }
        public string ProcessCode { get; set; }
        public string AppType { get; set; }
        public int PubID { get; set; }
        public string PubCode { get; set; }
        public string FileName { get; set; }
        public int FileTypeID { get; set; }
        public string FileType { get; set; }
        public DateTime DateImported { get; set; }
        public string Status { get; set; }
        public string FailedReason { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsProcessed { get; set; }
        public int RecordsDuplicates { get; set; }
        public int RecordsTransformed { get; set; }
        public int RecordsInvalid { get; set; }
        public int RecordsIgnored { get; set; }
        public int DimensionProfileCount { get; set; }
        public int DimensionRecordCount { get; set; }

        public bool HasDownLoadAccess { get; set; }
    }
}