using System.Data;
using Core_AMS.Utilities;
using KM.Common.Import;
using NUnit.Framework;
using Shouldly;

namespace UAS.UnitTests.Core_AMS.Utilities.FileImporter_cs
{
    [TestFixture]
    public class GetDataSetTest : Fakes
    {
        [Test]
        public void GetDataSet_EmptyFile_NoRecords()
        {
            // Arrange, Act
            var dataSet = FileImporter.GetDataSet(FileInfo1, null);

            //// Assert
            DataTable dtData;
            DataTable dtErrors;
            DataTable dtCounts;
            AssertDatasetTables(dataSet, out dtData, out dtErrors, out dtCounts);

            dtData.Columns.Count.ShouldBe(0);
            dtData.Rows.Count.ShouldBe(0);

            dtErrors.Columns.Count.ShouldBe(4);
            dtErrors.Rows.Count.ShouldBe(1);
            dtErrors.Rows[0][FieldErrorsClientMessage].ToString()
                .Substring(0, ErrorImportFile.Length)
                .ShouldBe(ErrorImportFile);

            dtCounts.Columns.Count.ShouldBe(3);
            dtCounts.Rows.Count.ShouldBe(1);
            dtCounts.Rows[0][FieldCountsTotalRows].ShouldBe(1);
            dtCounts.Rows[0][FieldCountsRowImportCount].ShouldBe(0);
            dtCounts.Rows[0][FieldCountsRowErrorCount].ShouldBe(1);
        }

        [Test]
        public void GetDataSet_FilledFile1ColRow_RecordsFilled()
        {
            // Arrange
            FillFileCol2RowCol1();

            // Act
            var dataSet = FileImporter.GetDataSet(FileInfo1, null);

            // Assert
            DataTable dtData;
            DataTable dtErrors;
            DataTable dtCounts;
            AssertDatasetTables(dataSet, out dtData, out dtErrors, out dtCounts);

            dtData.Columns.Count.ShouldBe(2);
            dtData.Rows.Count.ShouldBe(0);

            dtErrors.Columns.Count.ShouldBe(4);
            dtErrors.Rows.Count.ShouldBe(1);
            dtErrors.Rows[0][FieldFormattedError].ShouldBe(ErrorExpectedColumnCount);

            dtCounts.Columns.Count.ShouldBe(3);
            dtCounts.Rows.Count.ShouldBe(1);
            dtCounts.Rows[0][FieldCountsTotalRows].ShouldBe(1);
            dtCounts.Rows[0][FieldCountsRowImportCount].ShouldBe(0);
            dtCounts.Rows[0][FieldCountsRowErrorCount].ShouldBe(1);
        }

        [Test]
        public void GetDataSet_FilledFile2ColRow_RecordsFilled()
        {
            // Arrange
            FillFileCol2RowCol2();

            // Act
            var dataSet = FileImporter.GetDataSet(FileInfo1, null);

            //// Assert
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

            dtErrors.Columns.Count.ShouldBe(4);
            dtErrors.Rows.Count.ShouldBe(0);

            dtCounts.Columns.Count.ShouldBe(3);
            dtCounts.Rows.Count.ShouldBe(1);
            dtCounts.Rows[0][FieldCountsTotalRows].ShouldBe(1);
            dtCounts.Rows[0][FieldCountsRowImportCount].ShouldBe(1);
            dtCounts.Rows[0][FieldCountsRowErrorCount].ShouldBe(0);
        }
    }
}
