using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace UAD.DataCompare.Web.Models
{
    public class Datacompare
    {
        public Datacompare()
        {
            FileName = "";
            fileFullPath = "";
            NotiFicationEmail = "";
            Extension = string.Empty;
            Delimiter = string.Empty;
            IsTextQualifier = "false";
            QDateFormat = "MMDDYYYY";
            errorMessagesDict = null;
            ColumnMapping = null;

        }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string NotiFicationEmail { get; set; }

        [DataMember]
        public string Extension { get; set; }
        [DataMember]
        public string Delimiter { get; set; }
        [DataMember]
        public string IsTextQualifier { get; set; }
        [DataMember]
        public string QDateFormat { get; set; }
        [DataMember]
        public string fileFullPath { get; set; }
        [DataMember]
        public int BatchSize { get; set; }
        [DataMember]
        public List<ColumnMapper> ColumnMapping { get; set;  }
        [DataMember]
        public Dictionary<string,string> errorMessagesDict { get; set; }
    }


    
}