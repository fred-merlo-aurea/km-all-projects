using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAS.Web.Models.Circulations
{
    public class FileType
    {
        public FileType() { }

        public FileType(int fileTypeID, string fileTypeName, bool isCirc, string recordSource)
        {
            FileTypeID = fileTypeID;
            FileTypeName = fileTypeName;
            IsCirc = isCirc;
            RecordSource = recordSource;
        }

        public int FileTypeID { get; set; }
        public string FileTypeName { get; set; }
        public bool IsCirc { get; set; }
        public string RecordSource { get; set; }
    }
}