using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

using ECN_Framework_Common.Functions;

namespace ECN.Framework.Common.Tests.Functions.FileImporter_cs
{
    [TestFixture]
    public class GetDatasourceTest: Fakes
    {
        private const string Path1 = @"c:\folder1";
        private const string Path2 = @"c:\folder2";
        private const string FileName1Xls = "1.xls";
        private const string FileName1Xlsx = "1.xlsx";
        private const string FileTypeX = "X";
        private const string FileTypeC = "C";
        private const string FileTypeO = "O";

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

        [TestCase(Path2, FileName1Xls, "", "")]
        [TestCase(Path1, FileName1Xls, FileTypeX, 
            @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=c:\folder1\1.xls;Extended Properties='Excel 8.0;HDR=NO;IMEX=1;'")]
        [TestCase(Path2, FileName1Xlsx, FileTypeX, 
            @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=c:\folder2\1.xlsx;Extended Properties='Excel 12.0;HDR=NO;IMEX=1;'")]
        [TestCase(Path2, FileName1Xls, FileTypeC, 
            @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=c:\folder2;Extended Properties='Text;HDR=NO;MaxScanRows=10000;'")]
        [TestCase(Path2, FileName1Xls, FileTypeO, 
            @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=c:\folder2;Extended Properties='Text;HDR=NO;FMT=TabDelimited';")]
        public void GetDatasource_PathFileType_CorrespondingConnStr(
            string path, 
            string fileName, 
            string filetype, 
            string expectedConnStr)
        {
            // Arrange
            var privateType = new PrivateType(typeof(FileImporter));

            // Act
            var connStr = privateType.InvokeStatic("getDatasource", BindingFlags.NonPublic, path, fileName, filetype);

            // Assert
            connStr.ShouldBe(expectedConnStr);
        }
    }
}
