using System;
using System.Data;
using System.Data.Common.Fakes;
using System.Data.Fakes;
using System.Data.OleDb.Fakes;

using NUnit.Framework;
using Shouldly;

using ecn.communicator.classes.ImportData;
using ecn.communicator.classes.ImportData.Fakes;

namespace ECN.Tests.Communicator.FileImporter_cs
{
    [TestFixture]
    public class GetDataTableByFileTypeTest : Fakes
    {
        private const string Path1 = @"c:\folder1";
        private const string Path2 = @"c:\folder2";
        private const string FileName1Xls = "1.xls";
        private const string FileName1Xlsx = "1.xlsx";
        private const string FileTypeX = "X";
        private const string FileTypeC = "C";
        private const string FileTypeO = "O";
        private const string FileTypeXml = "XML";
        private const string FileNameSampleXml = "sample.xml";
        private const int StartLine = 7;
        private const int MaxRecordsToRetrieve = 119;
        private const int FakeLine = -1;
        private const string ExcelSheet1 = "sheet1";
        private const string DelimiterComma = ",";

        [SetUp]
        public void Setup()
        {
            SetupFakes();
        }

        [TearDown]
        public void TearDown()
        {
            DisposeFakes();
        }

        [TestCase(Path2, FileName1Xls, "", "", "", true)]
        [TestCase(
            Path1, FileName1Xls, FileTypeX,
            @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=c:\folder1\1.xls;Extended Properties='Excel 8.0;HDR=NO;IMEX=1;'",
            "SELECT * FROM [" + ExcelSheet1 + "$]")]
        [TestCase(
            Path2, FileName1Xlsx, FileTypeX,
            @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=c:\folder2\1.xlsx;Extended Properties='Excel 12.0;HDR=NO;IMEX=1;'",
            "SELECT * FROM [" + ExcelSheet1 + "$]")]
        [TestCase(
            Path2, FileName1Xls, FileTypeC,
            @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=c:\folder2;Extended Properties='Text;HDR=NO;'",
            "SELECT * FROM " + FileName1Xls + " ")]
        [TestCase(Path2, FileName1Xls, FileTypeO, "", "")]
        [TestCase(Path2, FileName1Xls, FileTypeXml, "", "")]
        public void GetDataTableByFileType_PathFileType_CorrespondingDataSet(
            string path,
            string fileName,
            string fileType,
            string expectedConnStr,
            string expectedQueryStr,
            bool shouldThrowException = false)
        {
            // Arrange
            var calledStartLine = FakeLine;
            var calledmaxRecordsToRetrieve = FakeLine;
            var calledFileName = FileNameSampleXml;
            var calledTable = new DataTable();
            ShimDbDataAdapter.AllInstances.FillDataSetInt32Int32String = 
                (adapter, pDataSet, pStartLine, pMaxRecordsToRetrieve, pFileName) =>
                    {
                        calledStartLine = pStartLine;
                        calledmaxRecordsToRetrieve = pMaxRecordsToRetrieve;
                        calledFileName = pFileName;

                        calledTable.TableName = pFileName;
                        pDataSet.Tables.Add(calledTable);

                        return 0;
                     };

            var calledQueryString = string.Empty;
            var calledConnStr = string.Empty;
            ShimOleDbDataAdapter.ConstructorStringString = (adapter, pQueryStr, pConnStr) =>
            {
                calledQueryString = pQueryStr;
                calledConnStr = pConnStr;
            };

            ShimFileImporter.ConstructSchemaStringString = (s, s1) =>
            {
            };

            ShimFileImporter.getDatafromTXTStringStringInt32Int32String = 
                (s, pFileName, pStartLine, pMaxRecordsToRetrieve, pDelimiter) =>
                    {
                        calledStartLine = pStartLine;
                        calledmaxRecordsToRetrieve = pMaxRecordsToRetrieve;
                        calledFileName = pFileName;

                        calledTable.TableName = pFileName;
                        var set = new DataSet();
                        set.Tables.Add(calledTable);
                        return set;
                    };

            ShimDataSet.AllInstances.ReadXmlString = (set, s) =>
            {
                calledStartLine = StartLine;
                calledmaxRecordsToRetrieve = MaxRecordsToRetrieve;
                calledTable.TableName = calledFileName;
                set.Tables.Add(calledTable);
                return XmlReadMode.Auto;
            };

            // Act
            if (shouldThrowException)
            {
                Should.Throw<ArgumentException>(
                    () => FileImporter.GetDataTableByFileType(
                            path, fileType, fileName, ExcelSheet1, StartLine, MaxRecordsToRetrieve, DelimiterComma));
                return;
            }

            var table = FileImporter.GetDataTableByFileType(
                path, fileType, fileName, ExcelSheet1, StartLine, MaxRecordsToRetrieve, DelimiterComma);

            // Assert
            table.ShouldNotBeNull();
            table.TableName.ShouldBe(calledFileName);
            table.ShouldBeSameAs(calledTable);
            calledStartLine.ShouldBe(StartLine);
            calledmaxRecordsToRetrieve.ShouldBe(MaxRecordsToRetrieve);
            calledQueryString.ShouldBe(expectedQueryStr);
            calledConnStr.ShouldBe(expectedConnStr);
        }
    }
}
