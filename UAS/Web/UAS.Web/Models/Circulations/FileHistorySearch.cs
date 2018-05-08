using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAS.Web.Models.Circulations
{
    public class FileHistorySearch
    {
        public FileHistorySearch()
        {
        }
        public KMPlatform.Entity.Client Client { get; set; }
        public List<Publication> Pubs { get; set; }
        public int PubID { get; set; }
        public string FileName { get; set; }
        public List<FileType> FileTypes { get; set; }
        public int FileTypeID { get; set; }
        public string FileTypeName { get; set; }
        public string RecordSource { get; set; }//CIRC UAD API
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool isCirc { get; set; }
        public bool isUAD { get; set; }
        public List<FileHistory> FileHistoryResults { get; set; }

    }
}