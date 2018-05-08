using System.Data;
using Core_AMS.Utilities;
using KM.Common.Import;
using NUnit.Framework;
using Shouldly;

namespace UAS.UnitTests.Core_AMS.Utilities.FileImporter_cs
{
    [TestFixture]
    public class LoadFileDataSet_StartRow_Test : Fakes
    {
        [Test]
        public void LoadFileDataSet_StartRow_Test_EmptyFile_NoRecords()
        {
            // Arrange, Act
            var dataSet = FileImporter.LoadFileDataSet(FileInfo1, 0, 1, null);

            // Assert
            DataTable dtData;
            DataTable dtErrors;
            DataTable dtCounts;
            AssertDatasetTables(dataSet, out dtData, out dtErrors, out dtCounts);

            dtData.Columns.Count.ShouldBe(0);
            dtData.Rows.Count.ShouldBe(0);

            dtErrors.Columns.Count.ShouldBe(5);
            dtErrors.Rows.Count.ShouldBe(0);

            dtCounts.Columns.Count.ShouldBe(3);
            dtCounts.Rows.Count.ShouldBe(1);
            dtCounts.Rows[0][FieldCountsTotalRows].ShouldBe(1);
            dtCounts.Rows[0][FieldCountsRowImportCount].ShouldBe(0);
            dtCounts.Rows[0][FieldCountsRowErrorCount].ShouldBe(0);
        }

        [Test]
        public void LoadFileDataSet_StartRow_Test_File3RowTake11_Table1Row()
        {
            // Arrange
            FillFileCol2Row3();

            // Act
            var dataSet = FileImporter.LoadFileDataSet(FileInfo1, 1, 1, null);

            // Assert
            DataTable dtData;
            DataTable dtErrors;
            DataTable dtCounts;
            AssertDatasetTables(dataSet, out dtData, out dtErrors, out dtCounts);

            dtData.Columns.Count.ShouldBe(2);
            dtData.Columns[0].ColumnName.ShouldBe(FieldFileCol1Name);
            dtData.Columns[1].ColumnName.ShouldBe(FieldFileCol2Name);
            dtData.Rows.Count.ShouldBe(1);
            dtData.Rows[0][FieldFileCol1Name].ShouldBe(ValueFile1);
            dtData.Rows[0][FieldFileCol2Name].ShouldBe(ValueFile2);

            dtErrors.Columns.Count.ShouldBe(5);
            dtErrors.Rows.Count.ShouldBe(0);

            dtCounts.Columns.Count.ShouldBe(3);
            dtCounts.Rows.Count.ShouldBe(1);
            dtCounts.Rows[0][FieldCountsTotalRows].ShouldBe(1);
            dtCounts.Rows[0][FieldCountsRowImportCount].ShouldBe(1);
            dtCounts.Rows[0][FieldCountsRowErrorCount].ShouldBe(0);
        }

        [Test]
        public void LoadFileDataSet_StartRow_Test_File3RowTake22_Table1Row()
        {
            // Arrange
            FillFileCol2Row3();

            // Act
            var dataSet = FileImporter.LoadFileDataSet(FileInfo1, 2, 2, null);

            // Assert
            DataTable dtData, dtErrors, dtCounts;
            AssertDatasetTables(dataSet, out dtData, out dtErrors, out dtCounts);

            dtData.Columns.Count.ShouldBe(2);
            dtData.Columns[0].ColumnName.ShouldBe(FieldFileCol1Name);
            dtData.Columns[1].ColumnName.ShouldBe(FieldFileCol2Name);
            dtData.Rows.Count.ShouldBe(2);
            dtData.Rows[0][FieldFileCol1Name].ShouldBe(ValueFile3);
            dtData.Rows[0][FieldFileCol2Name].ShouldBe(ValueFile4);
            dtData.Rows[1][FieldFileCol1Name].ShouldBe(ValueFile5);
            dtData.Rows[1][FieldFileCol2Name].ShouldBe(ValueFile6);

            dtErrors.Columns.Count.ShouldBe(5);
            dtErrors.Rows.Count.ShouldBe(0);

            dtCounts.Columns.Count.ShouldBe(3);
            dtCounts.Rows.Count.ShouldBe(1);
            dtCounts.Rows[0][FieldCountsTotalRows].ShouldBe(2);
            dtCounts.Rows[0][FieldCountsRowImportCount].ShouldBe(2);
            dtCounts.Rows[0][FieldCountsRowErrorCount].ShouldBe(0);
        }
    }
}
