using Core_AMS.Utilities;
using KM.Common.Import;
using NUnit.Framework;
using Shouldly;

namespace UAS.UnitTests.Core_AMS.Utilities.FileImporter_cs
{
    [TestFixture]
    public class LoadFileTest: Fakes
    {

        [Test]
        public void LoadFile_EmptyFile_NoRecords()
        {
            // Arrange, Act
            var dataTable = FileImporter.LoadFile(FileInfo1, null);

            // Assert
            dataTable.ShouldNotBeNull();
            dataTable.Rows.Count.ShouldBe(0);
            dataTable.Columns.Count.ShouldBe(0);
        }

        [Test]
        public void LoadFile_FilledFile1ColRow_RecordsFilled()
        {
            // Arrange
            FillFileCol2RowCol1();

            // Act
            var dataTable = FileImporter.LoadFile(FileInfo1, null);

            // Assert
            dataTable.ShouldNotBeNull();
            dataTable.Rows.Count.ShouldBe(0);
            dataTable.Columns.Count.ShouldBe(2);
            dataTable.Columns[0].ColumnName.ShouldBe(FieldFileCol1Name);
            dataTable.Columns[1].ColumnName.ShouldBe(FieldFileCol2Name);
        }

        [Test]
        public void LoadFile_FilledFile2ColRow_RecordsFilled()
        {
            // Arrange
            FillFileCol2RowCol2();

            // Act
            var dataTable = FileImporter.LoadFile(FileInfo1, null);

            // Assert
            dataTable.ShouldNotBeNull();
            dataTable.Columns.Count.ShouldBe(2);
            dataTable.Columns[0].ColumnName.ShouldBe(FieldFileCol1Name);
            dataTable.Columns[1].ColumnName.ShouldBe(FieldFileCol2Name);
            dataTable.Rows.Count.ShouldBe(1);
            dataTable.Rows[0][FieldFileCol1Name].ShouldBe(ValueFile1);
            dataTable.Rows[0][FieldFileCol2Name].ShouldBe(ValueFile2);
        }
    }
}
