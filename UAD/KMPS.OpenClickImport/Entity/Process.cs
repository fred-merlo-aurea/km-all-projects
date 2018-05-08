using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KMPS.ActivityImport.Entity
{

    [XmlRoot("Process")]
    public class Process
    {
        public Process() { }

        [XmlElement("ProcessType")]
        public string ProcessType { get; set; }
        [XmlElement("IsEnabled")]
        public bool IsEnabled { get; set; }
        [XmlElement("FtpFolder")]
        public string FtpFolder { get; set; }
        [XmlElement("FileFolder")]
        public string FileFolder { get; set; }
        [XmlElement("FilePrefix")]
        public string FilePrefix { get; set; }
        [XmlElement("FileExtension")]
        public string FileExtension { get; set; }
        [XmlElement("IsQuoteEncapsulated")]
        public bool IsQuoteEncapsulated { get; set; }
        [XmlElement("ColumnDelimiter")]
        public string ColumnDelimiter { get; set; }
        [XmlElement("ColumnCount")]
        public int ColumnCount { get; set; }
        [XmlElement("ColumnHeaders")]
        public string ColumnHeaders { get; set; }
    }
}
