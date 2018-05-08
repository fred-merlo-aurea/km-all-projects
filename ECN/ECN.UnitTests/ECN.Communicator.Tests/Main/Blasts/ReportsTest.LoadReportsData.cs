using NUnit.Framework;

namespace ECN.Communicator.Tests.Main.Blasts
{
    public partial class ReportsTest
    {
        private bool _hasAccess;

        [Test]
        public void loadReports_InvalidDataTable_VerifyLabelValues()
        {
            // Arrange
            _hasAccess = false;

            // Act
            _privateObject.Invoke(LoadReportsData, GetInvalidDataTable());

            // Assert
            VerifyLabelValuesWithInvalidDataTable();
        }

        [Test]
        public void loadReports_NotAuthorized_VerifyLabelValues()
        {
            // Arrange
            _hasAccess = false;

            // Act
            _privateObject.Invoke(LoadReportsData, GetReportingDataTable());

            // Assert
            VerifyLabelValuesIfNotAuthorized();
        }

        [Test]
        public void loadReports_Authorized_VerifyLabelValues()
        {
            // Arrange
            _hasAccess = true;

            // Act
            _privateObject.Invoke(LoadReportsData, GetReportingDataTable());

            // Assert
            VerifyLabelValueIfAuthorized();
        }
    }
}