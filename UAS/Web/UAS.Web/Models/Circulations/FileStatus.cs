using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAS.Web.Models.Circulations
{
    public class FileStatus
    {
        public string FileName { get; set; }
        public string PubCode { get; set; }
        public string FileType { get; set; }

        public int Totalsteps { get; set; }
        public int StepsCompleted { get; set; }
        public string Status { get; set; }

        public DateTime StartTime { get; set; }

        public int OriginalRecordCount { get; set; }
        public bool isPassMaxProcessingTime { get; set; }
    }

    public class FileStatusWithName
    {
        public string FileStatusName { get; set; }
        public IEnumerable<FileStatus> FileStatusIEnum { get; set; }
    }
}