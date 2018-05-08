using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KMPS.ActivityImport.Entity
{
    public class Customer
    {
        public Customer() { }

        [XmlElement("CustomerID")]
        public int CustomerID { get; set; }
        [XmlElement("CustomerName")]
        public string CustomerName { get; set; }
        [XmlElement("SQL")]
        public string SQL { get; set; }
        [XmlElement("FTP_Site")]
        public string FTP_Site { get; set; }
        [XmlElement("FTP_User")]
        public string FTP_User { get; set; }
        [XmlElement("FTP_Password")]
        public string FTP_Password { get; set; }
        [XmlElement("FilePath")]
        public string FilePath { get; set; }
        [XmlElement("FileArchive")]
        public string FileArchive { get; set; }
        [XmlElement("LogPath")]
        public string LogPath { get; set; }
        [XmlElement("MissFilePath")]
        public string MissFilePath { get; set; }
        [XmlElement("BadFilePath")]
        public string BadFilePath { get; set; }
        [XmlArray("Processes"), XmlArrayItem("Process")]
        public List<Process> Processes { get; set; }
    }
}
