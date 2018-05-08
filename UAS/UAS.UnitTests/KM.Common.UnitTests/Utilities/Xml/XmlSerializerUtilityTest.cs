using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using KM.Common.Utilities.Xml;
using NUnit.Framework;
using Shouldly;

namespace KM.Common.UnitTests.Utilities.Xml
{
    [TestFixture]
    public class XmlSerializerUtilityTest
    {
        private const string RootNodeName = "MasterGroupList";
        private const string ItemNodeName = "MasterGroup";
        private const string Header = "Header";
        private const string TestData = @"
Header1, Header2, Header3
Field11, Field12, Field13
Field21, Field22, Field23
Field31, Field32, Field33
";

        private const string TestDataXml = @"<MasterGroupList>
  <MasterGroup>
    <Header1>Field11</Header1>
    <Header2>Field12</Header2>
    <Header3>Field13</Header3>
  </MasterGroup>
  <MasterGroup>
    <Header1>Field21</Header1>
    <Header2>Field22</Header2>
    <Header3>Field23</Header3>
  </MasterGroup>
  <MasterGroup>
    <Header1>Field31</Header1>
    <Header2>Field32</Header2>
    <Header3>Field33</Header3>
  </MasterGroup>
</MasterGroupList>";

        private const string TestDataXmlFirstTwoRows = @"<MasterGroupList>
  <MasterGroup>
    <Header1>Field11</Header1>
    <Header2>Field12</Header2>
    <Header3>Field13</Header3>
  </MasterGroup>
  <MasterGroup>
    <Header1>Field21</Header1>
    <Header2>Field22</Header2>
    <Header3>Field23</Header3>
  </MasterGroup>
</MasterGroupList>";

        private const string TestDataXmlLastRow = @"<MasterGroupList>
  <MasterGroup>
    <Header1>Field31</Header1>
    <Header2>Field32</Header2>
    <Header3>Field33</Header3>
  </MasterGroup>
</MasterGroupList>";
        private const string CarriageReturn = "\r";

        [Test]
        public void SerializeCsvFile_ValidInput_XmlCreated()
        {
            // Arrange
            XDocument xDocument;

            // Act
            using (var stream = GenerateStreamFromString(TestData))
            {
                xDocument = XmlSerializerUtility.SerializeCsvFile(stream, RootNodeName, ItemNodeName, 0, false).First();
            }

            // Assert
            xDocument.ShouldSatisfyAllConditions(
                () => xDocument.ShouldNotBeNull(),
                () => xDocument.Root.ShouldNotBeNull(),
                () => xDocument.Root?.Name.ShouldBe(RootNodeName),
                () => CompareXNodeToString(xDocument, TestDataXml).ShouldBeTrue());
        }

        [Test]
        public void SerializeCsvFile_ValidInputMaxCollectionSizeSet_XmlCreated()
        {
            // Arrange
            const int maxCollectionSize = 2;
            IEnumerable<XDocument> xDocumentList;

            // Act
            using (var stream = GenerateStreamFromString(TestData))
            {
                xDocumentList = XmlSerializerUtility.SerializeCsvFile(stream, RootNodeName, ItemNodeName, maxCollectionSize, false);
            }

            // Assert
            xDocumentList.ShouldSatisfyAllConditions(
                () => xDocumentList.Count().ShouldBe(2),
                () => xDocumentList.First().ShouldNotBeNull(),
                () => xDocumentList.First().Root.ShouldNotBeNull(),
                () => xDocumentList.First().Root?.Name.ShouldBe(RootNodeName),
                () => CompareXNodeToString(xDocumentList.First(), TestDataXmlFirstTwoRows).ShouldBeTrue(),
                () => xDocumentList.Last().ShouldNotBeNull(),
                () => xDocumentList.Last().Root.ShouldNotBeNull(),
                () => xDocumentList.Last().Root?.Name.ShouldBe(RootNodeName),
                () => CompareXNodeToString(xDocumentList.Last(), TestDataXmlLastRow).ShouldBeTrue());
        }

        [Test]
        public void SerializeCsvFile_ValidInputLowerCaseHeader_XmlCreated()
        {
            // Arrange
            XDocument xDocument;

            // Act
            using (var stream = GenerateStreamFromString(TestData))
            {
                xDocument = XmlSerializerUtility.SerializeCsvFile(
                    stream,
                    RootNodeName,
                    ItemNodeName)
                    .First();
            }

            // Assert
            xDocument.ShouldSatisfyAllConditions(
                () => xDocument.ShouldNotBeNull(),
                () => xDocument.Root.ShouldNotBeNull(),
                () => xDocument.Root?.Name.ShouldBe(RootNodeName),
                () => CompareXNodeToString(xDocument, TestDataXml.Replace(Header, Header.ToLower())).ShouldBeTrue());
        }

        [Test]
        public void SerializeCsvFile_StreamNull_ArgumentNullException()
        {
            // Arrange
            var rootNodeName = string.Empty;

            // Act, Assert
            Should.Throw<ArgumentNullException>(
                () =>
                {
                    XmlSerializerUtility.SerializeCsvFile(null, rootNodeName, ItemNodeName);
                });
        }

        [Test]
        public void SerializeCsvFile_EmptyRootNodeName_ArgumentException()
        {
            // Arrange
            var rootNodeName = string.Empty;

            // Act, Assert
            Should.Throw<ArgumentException>(
                () =>
                {
                    using (var stream = GenerateStreamFromString(TestData))
                    {
                        XmlSerializerUtility.SerializeCsvFile(stream, rootNodeName, ItemNodeName);
                    }
                });
        }

        [Test]
        public void SerializeCsvFile_EmptyItemNodeName_ArgumentException()
        {
            // Arrange
            var itemNodeName = string.Empty;

            // Act, Assert
            Should.Throw<ArgumentException>(
                () =>
                {
                    using (var stream = GenerateStreamFromString(TestData))
                    {
                        XmlSerializerUtility.SerializeCsvFile(stream, RootNodeName, itemNodeName);
                    }
                });
        }

        private static bool CompareXNodeToString(XNode xDocument, string xmlText)
        {
            var stringDocument = RemoveCarriageReturn(xDocument.ToString(SaveOptions.None));
            var xmlTextToCompare = RemoveCarriageReturn(xmlText);

            return stringDocument.Equals(xmlTextToCompare, StringComparison.InvariantCulture);
        }

        private static string RemoveCarriageReturn(string input)
        {
            return input.Replace(CarriageReturn, string.Empty);
        }

        private static MemoryStream GenerateStreamFromString(string text)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(text));
        }
    }
}
