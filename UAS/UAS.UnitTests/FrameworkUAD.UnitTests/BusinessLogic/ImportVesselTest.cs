using System;
using System.Data;
using System.IO;
using System.Linq;
using FrameworkUAD.BusinessLogic;
using FrameworkUAD.UnitTests.BusinessLogic.Common;
using KM.Common.Import;
using KM.Common.Import.Fakes;
using NUnit.Framework;
using Shouldly;

namespace FrameworkUAD.UnitTests.BusinessLogic
{
    [TestFixture]
    public class ImportVesselTest : Fakes
    {
        private const string FileNameSample = "Sample.txt";
        private const string ColumnCol1 = "Col1";
        private const int RowCount = 1;
        private const string ErrorMessageSample = "Error Message";
        private const string FormattedExceptionSample = "Formatted Exception";

        [SetUp]
        public void Initialize()
        {
            SetupFakes();
        }

        [Test]
        public void GetImportVesselDbfData_Counts_CountsCalled()
        {
            // Arrange
            var table = CreateSampleTable();
            ShimFileImporter.ConvertFoxProDBFToDataTableFileInfoInt32Int32 = (_, __, ___) => table;
            var importVessel = new ImportVessel();

            // Act
            var result = importVessel.GetImportVesselDbfData(new FileInfo(FileNameSample), 0, RowCount);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.DataOriginal.ShouldBeSameAs(table),
                () => result.OriginalRowCount.ShouldBe(table.Rows.Count),
                () => result.TotalRowCount.ShouldBe(table.Rows.Count),
                () => result.HasError.ShouldBeFalse());
        }

        [Test]
        public void GetImportVesselDbfData_NoCounts_NoCountsCalled()
        {
            // Arrange
            var table = CreateSampleTable();
            ShimFileImporter.ConvertFoxProDBFToDataTableFileInfo = _ => table;
            var importVessel = new ImportVessel();

            // Act
            var result = importVessel.GetImportVesselDbfData(new FileInfo(FileNameSample));

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.DataOriginal.ShouldBeSameAs(table),
                () => result.OriginalRowCount.ShouldBe(table.Rows.Count),
                () => result.TotalRowCount.ShouldBe(table.Rows.Count),
                () => result.HasError.ShouldBeFalse());
        }

        [Test]
        public void GetImportVesselDbfData_ErrorEmitted_ErrorResult()
        {
            // Arrange
            ShimFileImporter.ConvertFoxProDBFToDataTableFileInfo = info => throw new Exception();
            var importVessel = new ImportVessel();

            // Act, Assert
            Should.Throw<NullReferenceException>(
                () => importVessel.GetImportVesselDbfData(new FileInfo(FileNameSample)));
        }

        [Test]
        public void LoadFileImportVesselData_NoCounts_NoCountsCalled()
        {
            // Arrange
            var tableData = new DataTable(ImportVessel.TableData);
            tableData.Columns.Add(ColumnCol1);
            var newRowData = tableData.NewRow();
            newRowData[ColumnCol1] = RowCount;
            tableData.Rows.Add(newRowData);

            var tableCounts = new DataTable(ImportVessel.TableCounts);
            tableCounts.Columns.Add(ImportVessel.ColumnRowImportCount);
            tableCounts.Columns.Add(ImportVessel.ColumnRowErrorCount);
            tableCounts.Columns.Add(ImportVessel.ColumnTotalRows);
            var newRowCounts = tableCounts.NewRow();
            newRowCounts[ImportVessel.ColumnRowImportCount] = RowCount;
            newRowCounts[ImportVessel.ColumnRowErrorCount] = RowCount;
            newRowCounts[ImportVessel.ColumnTotalRows] = RowCount;
            tableCounts.Rows.Add(newRowCounts);

            var tableErrors = new DataTable(ImportVessel.TableErrors);
            tableErrors.Columns.Add(ImportVessel.ColumnRowNumber);
            tableErrors.Columns.Add(ImportVessel.ColumnBadDataRow);
            tableErrors.Columns.Add(ImportVessel.ColumnClientMessage);
            tableErrors.Columns.Add(ImportVessel.ColumnFormattedError);
            var newRowError = tableErrors.NewRow();
            newRowError[ImportVessel.ColumnRowNumber] = RowCount;
            newRowError[ImportVessel.ColumnBadDataRow] = RowCount;
            newRowError[ImportVessel.ColumnClientMessage] = ErrorMessageSample;
            newRowError[ImportVessel.ColumnFormattedError] = FormattedExceptionSample;
            tableErrors.Rows.Add(newRowError);

            ShimFileImporter.LoadFileDataSetFileInfoFileConfiguration = (info, configuration) =>
            {
                var dataSet = new DataSet();
                dataSet.Tables.Add(tableData);
                dataSet.Tables.Add(tableCounts);
                dataSet.Tables.Add(tableErrors);
                return dataSet;
            };

            var importVessel = new ImportVessel();

            // Act
            var result = importVessel.LoadFileImportVesselData(new FileInfo(FileNameSample), new FileConfiguration());

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.DataOriginal.ShouldBeSameAs(tableData),
                () => result.ImportErrors.Count.ShouldBe(1),
                () => result.ImportErrors.Single().FormattedException.ShouldBe(FormattedExceptionSample),
                () => result.HasError.ShouldBeTrue());
        }

        private static DataTable CreateSampleTable()
        {
            var table = new DataTable();
            table.Columns.Add(ColumnCol1);
            var newRow = table.NewRow();
            newRow[ColumnCol1] = RowCount;
            table.Rows.Add(newRow);
            return table;
        }
    }
}
