using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.VisualBasic.FileIO;

namespace KM.Common.Utilities.Xml
{
    public static class XmlSerializerUtility
    {
        private const string CharactersToReplace = @"[^\x09\x0A\x0D\x20-\xD7FF\xE000-\xFFFD\x10000-x10FFFF]";
        private const string Delimiter = ",";
        private const string CanNotBeNullOrEmptyErrorMessage = "Cannot be null or empty.";

        public static IEnumerable<XDocument> SerializeCsvFile(Stream stream, string rootNode, string itemNode, int maxCollectionSize = 0, bool lowerCaseHeader = true)
        {
            Guard.NotNull(stream, nameof(stream));
            Guard.NotNullOrWhitespace(rootNode, nameof(rootNode), CanNotBeNullOrEmptyErrorMessage);
            Guard.NotNullOrWhitespace(itemNode, nameof(itemNode), CanNotBeNullOrEmptyErrorMessage);

            var documentList = new List<XDocument>();

            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            using (var textFieldParser = new TextFieldParser(stream))
            {
                textFieldParser.HasFieldsEnclosedInQuotes = true;
                textFieldParser.SetDelimiters(Delimiter);

                var headerValues = textFieldParser.ReadFields();
                if (lowerCaseHeader)
                {
                    headerValues = headerValues?.Select(str => str.ToLower()).ToArray();
                }

                while (!textFieldParser.EndOfData)
                {
                    var xDocument = new XDocument();
                    xDocument.Add(new XElement(rootNode));

                    var collectionItemCount = 0;

                    while ((collectionItemCount < maxCollectionSize || maxCollectionSize <= 0)
                        && !textFieldParser.EndOfData)
                    {
                        var csvData = textFieldParser.ReadFields();
                        xDocument.Root?.Add(BuildElement(headerValues, csvData, itemNode));
                        collectionItemCount++;
                    }

                    documentList.Add(xDocument);
                }
            }

            return documentList;
        }

        private static XElement BuildElement(IReadOnlyList<string> headerValues, IReadOnlyList<string> csvData, string itemNode)
        {
            var newItemElement = new XElement(itemNode);
            
            if (csvData.Count == headerValues.Count)
            {
                for (var i = 0; i < headerValues.Count; i++)
                {
                    var content = Regex.Replace(csvData[i], CharactersToReplace, string.Empty);
                    newItemElement.Add(new XElement(headerValues[i], content));
                }
            }
           
            return newItemElement;
        }
    }
}
