using System;
using System.Collections.Generic;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using NUnit.Framework;
using Shouldly;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;

namespace ECN.Communicator.Tests.Main.ECNWizard
{
    public partial class WizardScheduleTest
    {
        private const string ReportTime = "01:00";
        private const string FromEmail = "test@test.com";
        private const string ToEmail = "abc@abc.com";
        private const string CcList = "sample@sample.com";
        private const string FtpExports = "SampleFtpExport";
        private const string FtpUrl = "ftp://sample.ftp.com";
        private const string FtpUsername = "SampleFtpUserName";
        private const string FtpPassword = "SampleFtpPassword";
        private const string FtpExportFormat = "pdf";
        private const int BlastReportId = 9;
        private const int FtpReportId = 10;
        private const int OtherReportId = 99;
        private const string SaveScheduleReportMethodName = "SaveScheduleReport";

        private bool _isReportQueueCleared = false;
        private bool _isReportScheduleDeleted = false;
        private List<int> _deletedReportQueueId = new List<int>();
        private List<int> _deletedReportScheduleId = new List<int>();
        private CommunicatorEntities.ReportSchedule _savedReportSchedule = null;

        [Test]
        public void SaveScheduleReport_WhenScheduleBlastReportTrue_Test()
        {
            // Arrange
            SetUpFakesForSaveScheduleReportMethod();
            SetWizardSchedulePageProperties();
            var newBlastList = new List<BlastAbstract> { new BlastChampion { BlastID = 1 } };
            var campaignItemId = 1;
            _testEntity.scheduleBlastReport = true;

            // Act
            _privateTestObject.Invoke(SaveScheduleReportMethodName, newBlastList);

            // Assert
            _isReportQueueCleared.ShouldBeTrue();
            _isReportScheduleDeleted.ShouldBeTrue();
            _deletedReportQueueId.ShouldSatisfyAllConditions(
                () => _deletedReportQueueId.ShouldNotBeEmpty(),
                () => _deletedReportQueueId.Count.ShouldBe(2),
                () => _deletedReportQueueId.ShouldContain(BlastReportId),
                () => _deletedReportQueueId.ShouldContain(FtpReportId));
            _deletedReportScheduleId.ShouldSatisfyAllConditions(
                () => _deletedReportScheduleId.ShouldNotBeEmpty(),
                () => _deletedReportScheduleId.Count.ShouldBe(1),
                () => _deletedReportScheduleId.ShouldContain(FtpReportId));
            _savedReportSchedule.ShouldNotBeNull();
            _savedReportSchedule.ShouldSatisfyAllConditions(
                () => _savedReportSchedule.ReportScheduleID.ShouldBe(BlastReportId),
                () => _savedReportSchedule.StartTime.ShouldBe(ReportTime),
                () => _savedReportSchedule.StartDate.ShouldContain(DateTime.UtcNow.ToString("MM/dd/yyyy")),
                () => _savedReportSchedule.FromEmail.ShouldBe(FromEmail),
                () => _savedReportSchedule.ToEmail.ShouldBe(ToEmail),
                () => _savedReportSchedule.ReportID.ShouldBe(1),
                () => _savedReportSchedule.ReportParameters.ShouldContain(CcList));
        }

        [Test]
        public void SaveScheduleReport_WhenScheduleBlastReportFalse_Test()
        {
            // Arrange
            SetUpFakesForSaveScheduleReportMethod();
            SetWizardSchedulePageProperties();
            var newBlastList = new List<BlastAbstract> { new BlastChampion { BlastID = 1 } };
            var campaignItemId = 1;
            _testEntity.scheduleBlastReport = false;

            // Act
            _privateTestObject.Invoke(SaveScheduleReportMethodName, newBlastList);

            // Assert
            _isReportQueueCleared.ShouldBeTrue();
            _isReportScheduleDeleted.ShouldBeTrue();
            _deletedReportQueueId.ShouldSatisfyAllConditions(
                () => _deletedReportQueueId.ShouldNotBeEmpty(),
                () => _deletedReportQueueId.Count.ShouldBe(2),
                () => _deletedReportQueueId.ShouldContain(BlastReportId),
                () => _deletedReportQueueId.ShouldContain(FtpReportId));
            _deletedReportScheduleId.ShouldSatisfyAllConditions(
                () => _deletedReportScheduleId.ShouldNotBeEmpty(),
                () => _deletedReportScheduleId.Count.ShouldBe(2),
                () => _deletedReportScheduleId.ShouldContain(BlastReportId),
                () => _deletedReportScheduleId.ShouldContain(FtpReportId));
            _savedReportSchedule.ShouldBeNull();
        }

        [Test]
        public void SaveScheduleReport_WhenScheduleBlastReportListEmtyAndScheduleBlastIsTrue_Test()
        {
            // Arrange
            SetUpFakesForSaveScheduleReportMethod(blastReportId: OtherReportId);
            SetWizardSchedulePageProperties();
            var newBlastList = new List<BlastAbstract> { new BlastChampion { BlastID = 1 } };
            var campaignItemId = 1;
            _testEntity.scheduleBlastReport = true;

            // Act
            _privateTestObject.Invoke(SaveScheduleReportMethodName, newBlastList);

            // Assert
            _isReportQueueCleared.ShouldBeTrue();
            _isReportScheduleDeleted.ShouldBeTrue();
            _deletedReportQueueId.ShouldSatisfyAllConditions(
                () => _deletedReportQueueId.ShouldNotBeEmpty(),
                () => _deletedReportQueueId.Count.ShouldBe(1),
                () => _deletedReportQueueId.ShouldContain(FtpReportId));
            _deletedReportScheduleId.ShouldSatisfyAllConditions(
                () => _deletedReportScheduleId.ShouldNotBeEmpty(),
                () => _deletedReportScheduleId.Count.ShouldBe(1),
                () => _deletedReportScheduleId.ShouldContain(FtpReportId));
            _savedReportSchedule.ShouldNotBeNull();
            _savedReportSchedule.ShouldSatisfyAllConditions(
                () => _savedReportSchedule.ReportScheduleID.ShouldBe(-1),
                () => _savedReportSchedule.StartTime.ShouldBe(ReportTime),
                () => _savedReportSchedule.StartDate.ShouldContain(DateTime.UtcNow.ToString("MM/dd/yyyy")),
                () => _savedReportSchedule.FromEmail.ShouldBe(FromEmail),
                () => _savedReportSchedule.ToEmail.ShouldBe(ToEmail),
                () => _savedReportSchedule.ReportID.ShouldBe(1),
                () => _savedReportSchedule.ReportParameters.ShouldContain(CcList));
        }

        [Test]
        public void SaveScheduleReport_WhenScheduleFtpReportListAndscheduleFtpExportTrue_Test()
        {
            // Arrange
            SetUpFakesForSaveScheduleReportMethod(blastReportId: OtherReportId);
            SetWizardSchedulePageProperties();
            var newBlastList = new List<BlastAbstract> { new BlastChampion { BlastID = 1 } };
            var campaignItemId = 1;
            _testEntity.scheduleFtpExport = true;

            // Act
            _privateTestObject.Invoke(SaveScheduleReportMethodName, newBlastList);

            // Assert
            _isReportQueueCleared.ShouldBeTrue();
            _isReportScheduleDeleted.ShouldBeFalse();
            _deletedReportQueueId.ShouldSatisfyAllConditions(
                () => _deletedReportQueueId.ShouldNotBeEmpty(),
                () => _deletedReportQueueId.Count.ShouldBe(1),
                () => _deletedReportQueueId.ShouldContain(FtpReportId));
            _deletedReportScheduleId.ShouldSatisfyAllConditions(
                () => _deletedReportScheduleId.ShouldBeEmpty());
            _savedReportSchedule.ShouldNotBeNull();
            _savedReportSchedule.ShouldSatisfyAllConditions(
                () => _savedReportSchedule.ReportScheduleID.ShouldBe(FtpReportId),
                () => _savedReportSchedule.StartTime.ShouldBe(ReportTime),
                () => _savedReportSchedule.StartDate.ShouldContain(DateTime.UtcNow.ToString("MM/dd/yyyy")),
                () => _savedReportSchedule.FromEmail.ShouldBe(FromEmail),
                () => _savedReportSchedule.ToEmail.ShouldBe(ToEmail),
                () => _savedReportSchedule.ReportID.ShouldBe(1),
                () => _savedReportSchedule.ReportParameters.ShouldContain(CcList));
        }

        [Test]
        public void SaveScheduleReport_WhenScheduleFtpReportListEmty_Test()
        {
            // Arrange
            SetUpFakesForSaveScheduleReportMethod(blastReportId: OtherReportId, ftpReportId: OtherReportId);
            SetWizardSchedulePageProperties();
            var newBlastList = new List<BlastAbstract> { new BlastChampion { BlastID = 1 } };
            var campaignItemId = 1;
            _testEntity.scheduleFtpExport = true;

            // Act
            _privateTestObject.Invoke(SaveScheduleReportMethodName, newBlastList);

            // Assert
            _isReportQueueCleared.ShouldBeFalse();
            _isReportScheduleDeleted.ShouldBeFalse();
            _deletedReportQueueId.ShouldSatisfyAllConditions(
                () => _deletedReportQueueId.ShouldBeEmpty());
            _deletedReportScheduleId.ShouldSatisfyAllConditions(
                () => _deletedReportScheduleId.ShouldBeEmpty());
            _savedReportSchedule.ShouldNotBeNull();
            _savedReportSchedule.ShouldSatisfyAllConditions(
                () => _savedReportSchedule.ReportScheduleID.ShouldBe(-1),
                () => _savedReportSchedule.StartTime.ShouldBe(ReportTime),
                () => _savedReportSchedule.StartDate.ShouldContain(DateTime.UtcNow.ToString("MM/dd/yyyy")),
                () => _savedReportSchedule.FromEmail.ShouldBe(FromEmail),
                () => _savedReportSchedule.ToEmail.ShouldBe(ToEmail),
                () => _savedReportSchedule.ReportID.ShouldBe(1),
                () => _savedReportSchedule.ReportParameters.ShouldContain(CcList));
        }

        private void SetUpFakesForSaveScheduleReportMethod(int blastReportId = BlastReportId, int ftpReportId = FtpReportId)
        {
            ResetAssignments();
            ShimReportSchedule.GetByBlastIdInt32 = (bid) => new List<ReportSchedule>
            {
                new ReportSchedule { BlastID = 1 },
                new ReportSchedule { BlastID = 1 , ReportID = blastReportId, ReportScheduleID = blastReportId }, // Blast Report
                new ReportSchedule { BlastID = 1 , ReportID = ftpReportId, ReportScheduleID = ftpReportId }, // FTP Report
            };

            ShimReports.GetByReportNameStringUser = (name, user) => new CommunicatorEntities.Reports
            {
                ReportID = 1,
                ReportName = "BlastDetailsReport"
            };

            // Delete n Save Fakes section
            ShimReportQueue.Delete_ReportScheduleIDInt32 = (rptSchId) =>
            {
                _deletedReportQueueId.Add(rptSchId);
                _isReportQueueCleared = true;
            };
            ShimReportSchedule.DeleteInt32User = (bid, user) =>
            {
                _deletedReportScheduleId.Add(bid);
                _isReportScheduleDeleted = true;
            };
            ShimReportSchedule.SaveReportScheduleUser = (rpt, user) =>
            {
                _savedReportSchedule = rpt;
                return _savedReportSchedule.ReportScheduleID;
            };
        }

        private void SetWizardSchedulePageProperties()
        {
            _testEntity.reportTime = "01:00";
            _testEntity.reportDate = DateTime.UtcNow;
            _testEntity.fromEmail = "test@test.com";
            _testEntity.toEmail = "abc@abc.com";
            _testEntity.ccList = new List<string> { "sample@sample.com" };
            _testEntity.ftpExports = new List<string> { "SampleFtpExport" };
            _testEntity.ftpUrl = "ftp://sample.ftp.com";
            _testEntity.ftpUsername = "SampleFtpUserName";
            _testEntity.ftpPassword = "SampleFtpPassword";
            _testEntity.ftpExportFormat = "pdf";
        }

        private void ResetAssignments()
        {
            _isReportQueueCleared = false;
            _isReportScheduleDeleted = false;
            _deletedReportQueueId.Clear();
            _deletedReportScheduleId.Clear();
            _savedReportSchedule = null;
        }
    }
}
