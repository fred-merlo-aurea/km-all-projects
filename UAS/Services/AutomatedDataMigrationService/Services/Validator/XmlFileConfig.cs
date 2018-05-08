using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace ADMS.Services.Validator
{
    class XmlFileConfig
    {
        public FileMapping GetXmlFileConfig(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(FileMapping));
            StreamReader reader = new StreamReader(fileName);
            FileMapping fileMapping = (FileMapping)serializer.Deserialize(reader);
            reader.Close();
            return fileMapping;
        }
    }

    public class EditMappingList
    {
        public string ColumnName { get; set; }
        public string MappedTo { get; set; }
        public string PubNum { get; set; }
        public string DataType { get; set; }
        public string PreviewData { get; set; }
    }

    [XmlType(AnonymousType = true)]
    [XmlRoot("File")]
    public class FileMapping
    {
        [XmlElement("FileNamePrefix")]
        public string FileNamePrefix { get; set; }
        [XmlElement("Extension")]
        public string Extension { get; set; }
        [XmlArray("Columns"), XmlArrayItem("Column")]
        public List<Column> Columns { get; set; }
        [XmlElement("Valid")]
        public string Valid { get; set; }
    }

    public class Column
    {
        public Column() { }

        [XmlElement("ColumnName")]
        public string ColumnName { get; set; }
        [XmlElement("MappedTo")]
        public string MappedTo { get; set; }
        [XmlElement("PubNum")]
        public string PubNum { get; set; }
        [XmlElement("DataType")]
        public string DataType { get; set; }
        [XmlElement("PreviewData")]
        public string PreviewData { get; set; }
        [XmlElement("Ignore", IsNullable = false)]
        public string Ignore { get; set; }
        [XmlElement("ColumnIndex", IsNullable = false)]
        public int ColumnIndex { get; set; }
    }
}
