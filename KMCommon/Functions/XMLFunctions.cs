using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace KM.Common
{
    public class XMLFunctions
    {
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
        /// <summary>
        /// XML Encode
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>Encoded string</returns>
        public static string XmlEncode(string str)
        {
            if (str == null)
                return null;
            str = Regex.Replace(str, @"[^\u0009\u000A\u000D\u0020-\uD7FF\uE000-\uFFFD]", "", RegexOptions.Compiled);
            return XmlEncodeAsIs(str);
        }

        /// <summary>
        /// XML Encode as is
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>Encoded string</returns>
        public static string XmlEncodeAsIs(string str)
        {
            if (str == null)
                return null;
            using (StringWriter sw = new StringWriter())
            using (XmlTextWriter xwr = new XmlTextWriter(sw))
            {
                xwr.WriteString(str);
                String sTmp = sw.ToString();
                return sTmp;
            }
        }

        /// <summary>
        /// Encodes an attribute
        /// </summary>
        /// <param name="str">Attribute</param>
        /// <returns>Encoded attribute</returns>
        public static string XmlEncodeAttribute(string str)
        {
            if (str == null)
                return null;
            str = Regex.Replace(str, @"[^\u0009\u000A\u000D\u0020-\uD7FF\uE000-\uFFFD]", "", RegexOptions.Compiled);
            return XmlEncodeAttributeAsIs(str);
        }

        /// <summary>
        /// Encodes an attribute as is
        /// </summary>
        /// <param name="str">Attribute</param>
        /// <returns>Encoded attribute</returns>
        public static string XmlEncodeAttributeAsIs(string str)
        {
            return XmlEncodeAsIs(str).Replace("\"", "&quot;");
        }

        /// <summary>
        /// Decodes an attribute
        /// </summary>
        /// <param name="str">Attribute</param>
        /// <returns>Decoded attribute</returns>
        public static string XmlDecode(string str)
        {
            var sb = new StringBuilder(str);
            return sb.Replace("&quot;", "\"").Replace("&apos;", "'").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&").ToString();
        }

        /// <summary>
        /// Serializes a datetime
        /// </summary>
        /// <param name="dateTime">Datetime</param>
        /// <returns>Serialized datetime</returns>
        public static string SerializeDateTime(DateTime dateTime)
        {
            var xmlS = new XmlSerializer(typeof(DateTime));
            var sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                xmlS.Serialize(sw, dateTime);
                return sb.ToString();
            }
        }

        /// <summary>
        /// Deserializes a datetime
        /// </summary>
        /// <param name="dateTime">Datetime</param>
        /// <returns>Deserialized datetime</returns>
        public static DateTime DeserializeDateTime(string dateTime)
        {
            var xmlS = new XmlSerializer(typeof(DateTime));
            using (StringReader sr = new StringReader(dateTime))
            {
                object test = xmlS.Deserialize(sr);
                return (DateTime)test;
            }
        }

        public static string RemoveNonAscii(string dirty)
        {
            var clean = Encoding.ASCII.GetString(
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

        public static string CleanNonValidXMLCharacters(string input)
        {
            if (String.IsNullOrWhiteSpace(input))
            {
                return String.Empty;
            }

            char current;
            var textOut = new StringBuilder();             
            for (int i = 0; i < input.Length; i++)
            {
                current = input[i];

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

      public static string EscapeXmlString(string dirty)
        {
            var subject = !string.IsNullOrEmpty(dirty)
                ? dirty.Replace("\0", string.Empty).Trim()
                : string.Empty;

            var builder = new StringBuilder(subject)
              .Replace("&", "&amp;")
              .Replace("\"", "&quot;")
              .Replace("<", "&lt;")
              .Replace(">", "&gt;")
              .Replace("'", string.Empty)
              .Replace("’", string.Empty);

            var bytes = Encoding.Default.GetBytes(builder.ToString());
            return Encoding.UTF8.GetString(bytes); 
        }
    }
}
