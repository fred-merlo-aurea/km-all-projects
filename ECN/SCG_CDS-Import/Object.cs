using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Net;
using System.Configuration;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Net.Mail;
using System.Threading;
namespace SCG_CDS_Import
{

    [Serializable]
    [DataContract]
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRoot("ImportFiles")]
    public class ImportFiles
    {
        [DataMember]
        [XmlElement("ImportFile")]
        public List<ImportFile> Files { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ImportFile
    {
        public ImportFile() { }
        #region Properties
        [DataMember]
        [XmlElement("GroupID")]
        public int GroupID { get; set; }

        [DataMember]
        [XmlElement("GroupName")]
        public string GroupName { get; set; }

        [DataMember]
        [XmlElement("CustomerID")]
        public int CustomerID { get; set; }

        [DataMember]
        [XmlElement("AuthKey")]
        public string AuthKey { get; set; }

        [DataMember]
        [XmlElement("FileName")]
        public string FileName { get; set; }

        [DataMember]
        [XmlElement("FileExtension")]
        public string FileExtension { get; set; }

        [DataMember]
        [XmlElement("ColumnDelimiter")]
        public string ColumnDelimiter { get; set; }

        [DataMember]
        [XmlElement("IsQuoteEncapsulated")]
        public bool IsQuoteEncapsulated { get; set; }

        [DataMember]
        [XmlArray("Fields"), XmlArrayItem("Field")]
        public List<Field> Fields { get; set; }
        #endregion
    }

    [Serializable]
    [DataContract]
    [XmlRoot("Field")]
    public class Field
    {
        public Field() { }
        #region Properties
        [DataMember]
        [XmlElement("FileField")]
        public string FileField { get; set; }

        [DataMember]
        [XmlElement("ECNField")]
        public string ECNField { get; set; }

        [DataMember]
        [XmlElement("CombineFields")]
        public string CombineFields { get; set; }

        [DataMember]
        [XmlElement("IsUDF")]
        public bool IsUDF { get; set; }

        [DataMember]
        [XmlElement("Ignore")]
        public bool Ignore { get; set; }

        #endregion
    }

    public class TransactionType
    {
        public TransactionType() { }

        public string Pub { get; set; }
        public int AddCount { get; set; }
        public int ChangeCount { get; set; }
        public int DeleteCount { get; set; }
    }

}
