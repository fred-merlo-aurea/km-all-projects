using System.IO.Fakes;
using System.Reflection;
using FrameworkUAD.Object;
using NUnit.Framework;
using Shouldly;

namespace UAS.UnitTests.FrameworkUAD.BusinessLogic
{
    [TestFixture]
    public class ImportFileTest : Common.Fakes
    {
        private const string GetImportFileJsonMethodname = "GetImportFileJson";

        [Test]
        public void GetImportFileText_EmptyFile_NoRecords()
        {
            // Arrange Act
            var result = privateObject.Invoke("GetImportFileText", BindingFlags.Instance | BindingFlags.NonPublic, FileInfo1, null);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType<ImportFile>();

            var importFile = result as ImportFile;
            importFile.ImportedRowCount.ShouldBe(0);
            importFile.ImportErrorCount.ShouldBe(0);
            importFile.OriginalRowCount.ShouldBe(0);
        }

        [Test]
        public void GetImportFileText_FilledFile1ColRow_RecordsFilled()
        {
            // Arrange
            FillFileCol2RowCol1();

            // Act
            var result = privateObject.Invoke("GetImportFileText", BindingFlags.Instance | BindingFlags.NonPublic, FileInfo1, null);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType<ImportFile>();

            var importFile = result as ImportFile;
            importFile.ImportedRowCount.ShouldBe(0);
            importFile.ImportErrorCount.ShouldBe(1);
            importFile.OriginalRowCount.ShouldBe(0);
            importFile.TotalRowCount.ShouldBe(1);
        }

        [Test]
        public void GetImportFileText_FilledFile2ColRow_RecordsFilled()
        {
            // Arrange
            FillFileCol2Row2();

            // Act
            var result = privateObject.Invoke("GetImportFileText", BindingFlags.Instance | BindingFlags.NonPublic, FileInfo1, null);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType<ImportFile>();

            var importFile = result as ImportFile;
            importFile.ImportedRowCount.ShouldBe(2);
            importFile.ImportErrorCount.ShouldBe(0);
            importFile.OriginalRowCount.ShouldBe(2);
            importFile.TotalRowCount.ShouldBe(2);

            importFile.DataOriginal[1][FieldFileCol1Name].ShouldBe(ValueFile1);
            importFile.DataOriginal[1][FieldFileCol2Name].ShouldBe(ValueFile2);
            importFile.DataOriginal[2][FieldFileCol1Name].ShouldBe(ValueFile3);
            importFile.DataOriginal[2][FieldFileCol2Name].ShouldBe(ValueFile4);
        }

        [Test]
        public void GetImportFileJson_WithOneArgument_ShouldCreateImportFile()
        {
            // Arrange
            ShimStreamReader.AllInstances.ReadToEnd = (reader) => "[{\"asd\": 0}]";

            var args = new object[]
            {
                FileInfo1
            };

            // Act
            var result = privateObject.Invoke(GetImportFileJsonMethodname, args) as ImportFile;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.HasError.ShouldBeFalse(),
                () => result.ImportedRowCount.ShouldBe(1),
                () => result.TotalRowCount.ShouldBe(1),
                () => result.ImportErrorCount.ShouldBe(0));
        }

        [Test]
        public void GetImportFileJson_WithErrorArgument_ShouldCreateImportFileWithError()
        {
            // Arrange
            ShimStreamReader.AllInstances.ReadToEnd = (reader) => "[{nu\"ll: 0}]";

            var args = new object[]
            {
                FileInfo1
            };

            // Act
            var result = privateObject.Invoke(GetImportFileJsonMethodname, args) as ImportFile;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.HasError.ShouldBeTrue(),
                () => result.ImportedRowCount.ShouldBe(1),
                () => result.TotalRowCount.ShouldBe(1),
                () => result.ImportErrorCount.ShouldBe(1));
            
        }

        [Test]
        public void GetImportFileJson_WithEmptyArgument_ShouldCreateImportFileWithError()
        {
            // Arrange
            ShimStreamReader.AllInstances.ReadToEnd = (reader) => "[]";

            var args = new object[]
            {
                FileInfo1
            };

            // Act
            var result = privateObject.Invoke(GetImportFileJsonMethodname, args) as ImportFile;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.HasError.ShouldBeTrue(),
                () => result.ImportedRowCount.ShouldBe(0),
                () => result.TotalRowCount.ShouldBe(0));
        }
    }
}
