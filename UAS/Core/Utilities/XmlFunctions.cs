using System;
using System.Xml;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Data;

namespace Core_AMS.Utilities
{
    public class XmlFunctions
    {
        public string ToXml<T>(T x)
        {
            string xmlString = ServiceStack.Text.XmlSerializer.SerializeToString<T>(x);
            return xmlString;
        }
        public static string CleanAllXml(string dirty)
        {
            string clean = FormatXMLSpecialCharacters(dirty);
            clean = CleanNonValidXMLCharacters(clean);
            clean = CleanInvalidXmlChars(clean);
            return clean;
        }
        public static string FormatXMLSpecialCharacters(string dirty)
        {
            //var validXmlChars = dirty.Where(ch => XmlConvert.IsXmlChar(ch)).ToArray();
            //string converted = new string(validXmlChars);
            //string secure = SecurityElement.Escape(converted);
            //string withOutSpecialCharacters = new string(secure.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)).ToArray());
            //string clean = withOutSpecialCharacters.Replace("-", "").Replace("—", "").Replace("–", "").Replace("'", "").Replace("’", "").Replace("ñ", "n").Replace("™", "").Replace("ã", "").Replace("•", "").Replace("ö","").Replace("Ã","").Replace("©","").Replace("¨","").ToString();

            dirty = dirty.Replace("\0", string.Empty);
            if (string.IsNullOrEmpty(dirty))
                dirty = string.Empty;
            dirty = dirty.Trim();
            dirty = dirty.Replace("&", "&amp;");
            dirty = dirty.Replace("\"", "&quot;");
            dirty = dirty.Replace("<", "&lt;");
            dirty = dirty.Replace(">", "&gt;");
            dirty = dirty.Replace("'", "&apos;").Replace("’", "&apos;");
            byte[] bytes = Encoding.Default.GetBytes(dirty);
            return Encoding.UTF8.GetString(bytes); 
        }
        public static string RemoveFormatXMLSpecialCharacters(string dirty)
        {
            dirty = dirty.Replace("\0", string.Empty);
            if (string.IsNullOrEmpty(dirty))
                dirty = string.Empty;
            dirty = dirty.Trim();
            dirty = dirty.Replace("&amp;", "&");
            dirty = dirty.Replace("&quot;", "\"");
            dirty = dirty.Replace("&lt;", "<");
            dirty = dirty.Replace("&gt;", ">");
            dirty = dirty.Replace("&apos;", "'").Replace("&apos;", "’");
            byte[] bytes = Encoding.Default.GetBytes(dirty);
            return Encoding.UTF8.GetString(bytes);
        }
        public static string RemoveNonAscii(string dirty)
        {
            string clean = Encoding.ASCII.GetString(
                Encoding.Convert(
                    Encoding.UTF8,
                    Encoding.GetEncoding(
                        Encoding.ASCII.EncodingName,
                        new EncoderReplacementFallback(string.Empty),
                        new DecoderExceptionFallback()
                        ),
                    Encoding.UTF8.GetBytes(dirty)
                )
            );

            return clean;
        }
        public static string CleanNonValidXMLCharacters(string textIn)
        {
            StringBuilder textOut = new StringBuilder(); // Used to hold the output.
            char current; // Used to reference the current character.


            if (textIn == null || textIn == string.Empty) return string.Empty; // vacancy test.
            for (int i = 0; i < textIn.Length; i++)
            {
                current = textIn[i];


                if ((current == 0x9 || current == 0xA || current == 0xD) ||
                    ((current >= 0x20) && (current <= 0xD7FF)) ||
                    ((current >= 0xE000) && (current <= 0xFFFD)) ||
                    ((current >= 0x10000) && (current <= 0x10FFFF)))
                {
                    textOut.Append(current);
                }
            }
            return textOut.ToString();
        }
        public static string CleanInvalidXmlChars(string text)
        {
            string re = @"[^\x09\x0A\x0D\x20-\xD7FF\xE000-\xFFFD\x10000-x10FFFF]";
            return Regex.Replace(text, re, "");
        }  

        public static XmlNode FindNode(XmlNodeList list, string nodeName)
        {
            if (list.Count > 0)
            {
                foreach (XmlNode node in list)
                {
                    if (node.Name.Equals(nodeName))
                        return node;
                    if (node.HasChildNodes)
                    {
                        XmlNode nodeFound = FindNode(node.ChildNodes, nodeName);
                        if (nodeFound != null)
                            return nodeFound;
                    }
                }
            }
            return null;
        }
        public static XDocument SerializeCsvFile(Stream stream, string rootNode, string itemNode)
        {
            return SerializeCsvFile(stream, rootNode, itemNode, 0).First();
        }
        public static List<XDocument> SerializeCsvFile(Stream stream, string rootNode, string itemNode, int maxCollectionSize)
        {
            List<XDocument> documentList = new List<XDocument>();
            stream.Position = 0;

            using (TextFieldParser parser = new TextFieldParser(stream))
            {
                parser.HasFieldsEnclosedInQuotes = true;
                parser.SetDelimiters(",");

                string[] headerValues = parser.ReadFields();

                string[] headerValuesLower = (from str in headerValues
                                              select str.ToLower()).ToArray();

                while (!parser.EndOfData)
                {
                    XDocument xDoc = new XDocument(new XElement(rootNode));
                    int collectionItemCount = 0;

                    while ((collectionItemCount < maxCollectionSize || maxCollectionSize <= 0)
                        && !parser.EndOfData)
                    {
                        string[] csvData = parser.ReadFields();
                        xDoc.Root.Add(BuildElement(headerValuesLower, csvData, itemNode));
                        collectionItemCount++;
                    }

                    documentList.Add(xDoc);
                }
            }

            return documentList;
        }
        public static void WriteXML<T>(T o, string filePathName, string rootElementName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XPathNavigator nav = xmlDoc.CreateNavigator();
            using (XmlWriter writer = nav.AppendChild())
            {
                System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T), new XmlRootAttribute(rootElementName));
                ser.Serialize(writer, o); 
            }
            File.WriteAllText(filePathName, xmlDoc.InnerXml);
        }
        public static XmlDocument GetXML<T>(T o, string rootElementName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            
            try
            {
                XPathNavigator nav = xmlDoc.CreateNavigator();
                using (XmlWriter writer = nav.AppendChild())
                {
                    System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T), new XmlRootAttribute(rootElementName));
                    ser.Serialize(writer, o);
                }
            }
            catch (Exception ex)
            {
                string error = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            }
            return xmlDoc;
        }
        public static XDocument GetXDoc<T>(T o)
        {
            string xml = ToXML<T>(o);
            return XDocument.Parse(xml);
        }
        public static string ToXML<T>(T o)
        {
            string xml = ServiceStack.Text.XmlSerializer.SerializeToString<T>(o);
            return CleanSerializedXML(xml);
        }
        public static T FromXML<T>(string xml)
        {
            var item = ServiceStack.Text.XmlSerializer.DeserializeFromString<T>(xml);
            return item;
        }
        public static string CleanSerializedXML(string dirtyXML)
        {
            string returnClean = dirtyXML.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "").Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "").Replace("xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"", "").Replace("xmlns=\"http://schemas.datacontract.org/2004/07/FrameworkUAS.Entity\"", "").Replace("xmlns=\"http://schemas.datacontract.org/2004/07/FrameworkUAS.Object\"", "").Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "").Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "").Replace("xmlns=\"http://schemas.datacontract.org/2004/07/WpfControls\"", "").Replace("i:nil=\"true\"", "").Trim();
            return returnClean;
        }
        private static XElement BuildElement(string[] headerValues, string[] csvData, string itemNode)
        {
            XElement newItemElement = new XElement(itemNode);
            string re = @"[^\x09\x0A\x0D\x20-\xD7FF\xE000-\xFFFD\x10000-x10FFFF]";

            if (csvData.Length == headerValues.Length)
            {
                for (int i = 0; i < headerValues.Length; i++)
                {
                    newItemElement.Add(new XElement(headerValues[i], Regex.Replace(csvData[i], re, "")));
                }
            }

            return newItemElement;
        }
        public static XDocument CreateFromFile(FileInfo file)
        {
            XDocument doc = XDocument.Load(file.FullName, LoadOptions.None);
            return doc;
        }
        public static int GetRecordCount(FileInfo file)
        {
            XDocument doc = XDocument.Load(file.FullName, LoadOptions.None);
            int childrenCount = doc.Root.Elements().Count(); // counts direct children of the root element
            return childrenCount;
        }
        public static int GetRecordCount(string xml)
        {
            XDocument doc = XDocument.Parse(xml);
            int childrenCount = doc.Root.Elements().Count(); // counts direct children of the root element
            return childrenCount;
        }
        public static DataTable CreateDataTableFromXmlFile(FileInfo file)
        {            
            DataSet ds = new DataSet();
            ds.ReadXml(file.FullName);
            DataTable dt = ds.Tables[0];

            return dt;
        }
    }
}
