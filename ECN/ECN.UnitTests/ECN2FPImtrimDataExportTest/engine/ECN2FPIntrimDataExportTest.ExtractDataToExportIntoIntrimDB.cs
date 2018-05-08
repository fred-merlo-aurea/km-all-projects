using System;
using System.Data;
using System.IO.Fakes;
using ecn.kmps.ECN2FPImtrimDataExport;
using NUnit.Framework;
using Shouldly;

namespace ECN2FPImtrimDataExport.Tests.engine
{
    /// <summary>
    /// Unit tests for <see cref="ECN2FPIntrimDataExport.extractDataToExportINTOIntrimDB"/>
    /// </summary>
    public partial class Ecn2FpIntrimDataExportTest
    {
        [Test]
        [TestCaseSource(nameof(ExtractDataToExportTestCases))]
        public void ExtractDataToExportIntoIntrimDB_WhenCalled_VerifyParameters(
            string parameterName,
            SqlDbType dbType,
            int size,
            string parameterValue)
        {
            // Arrange
            SetupExportDataTable();
            ECN2FPIntrimDataExport.logFile = new ShimStreamWriter().Instance;
            var message = $"----- SUCCESSFULLY EXPORTED 1 ROWS FOR - GroupID: {GroupId} CustomerID: {CustomerId} -----";

            // Act
            ECN2FPIntrimDataExport.extractDataToExportINTOIntrimDB(_dataTable, GroupId, CustomerId);

            // Assert
            var parameter = _insertCommand.Parameters[parameterName];
            _export.ShouldSatisfyAllConditions(
                () => parameter.SqlDbType.ShouldBe(dbType),
                () => parameter.Size.ShouldBe(size),
                () => parameter.Value.ToString().ShouldBe(parameterValue),
                () => _message.ShouldContain(message));
        }

        [Test]
        public void ExtractDataToExportIntoIntrimDB_WhenCalledWithInvalidDataTable_ThrowArgumentException()
        {
            // Arrange
            _invalidValue = InvalidValue;
            SetupExportDataTable();
            ECN2FPIntrimDataExport.logFile = new ShimStreamWriter().Instance;

            // Act
            Action action = () => ECN2FPIntrimDataExport.extractDataToExportINTOIntrimDB(_dataTable, GroupId, CustomerId);

            // Assert
            action.ShouldThrow<ArgumentException>();
        }
    }
}
