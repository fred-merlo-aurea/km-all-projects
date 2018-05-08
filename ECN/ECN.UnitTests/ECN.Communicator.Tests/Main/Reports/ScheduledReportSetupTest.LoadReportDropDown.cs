using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using NUnit.Framework;
using Shouldly;
using static KMPlatform.Enums;
using commEntities = ECN_Framework_Entities.Communicator;

namespace ECN.Communicator.Tests.Main.Reports
{
    public partial class ScheduledReportSetupTest
    {
        private const string LRD_DataTextField = "ReportName";
        private const string LRD_DataValueField = "ReportID";
        private DropDownList _drpReports;

        [TestCase(1, ServiceFeatures.BlastDeliveryReport, Access.Download)]
        [TestCase(2, ServiceFeatures.EmailPreviewUsageReport, Access.View)]
        [TestCase(3, ServiceFeatures.EmailPerformanceByDomainReport, Access.Download)]
        [TestCase(4, ServiceFeatures.GroupStatisticsReport, Access.Download)]
        [TestCase(5, ServiceFeatures.AudienceEngagementReport, Access.View)]
        [TestCase(6, ServiceFeatures.AdvertiserClickReport, Access.DownloadDetails)]
        [TestCase(8, ServiceFeatures.GroupExport, Access.FullAccess)]
        [TestCase(11, ServiceFeatures.GroupAttributeReport, Access.Download)]
        [TestCase(12, ServiceFeatures.UnsubscribeReasonReport, Access.DownloadDetails)]
        [TestCase(20, null, null)]
        public void LoadReportDropDown_ReportsLoaded_HasAccessCalledWithProperArguments(int reportId, ServiceFeatures? serviceFeatureEnum, Access? accessEnum)
        {
            // Arrange
            Init_LoadReportDropDownTest();
            ServiceFeatures? calledServiceFeature = null;
            Access? calledAccess = null;
            var report = CreateReports_LoadReportDropDownTest(reportId, true, "reportName");
            ShimReports.GetUser = (u) => new List<commEntities.Reports>() { report };
            KM.Platform.Fakes.ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (user, service, feature, access) =>
            {
                calledServiceFeature = feature;
                calledAccess = access;
                return true;
            };

            // Act
            _privateTestObject.Invoke("LoadReportDropDown");

            // Assert
            calledServiceFeature.ShouldSatisfyAllConditions(
                () => calledServiceFeature.ShouldBe(calledServiceFeature),
                () => calledAccess.ShouldBe(accessEnum));
        }

        [Test]
        public void LoadReportDropDown_ReportsLoaded_DropdownListInitialized()
        {
            // Arrange
            var reports = CreateAllReports_LoadReportDropDownTest();
            ShimReports.GetUser = (u) => reports;
            KM.Platform.Fakes.ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (user, service, feature, access) => true;
            Init_LoadReportDropDownTest();

            // Act
            _privateTestObject.Invoke("LoadReportDropDown");

            // Assert
            _drpReports.ShouldSatisfyAllConditions(
                 () => _drpReports.DataTextField.ShouldBe(LRD_DataTextField),
                 () => _drpReports.DataValueField.ShouldBe(LRD_DataValueField),
                 () => _drpReports.Items.Count.ShouldBe(reports.Count + 1),
                 () => _drpReports.Items[0].ShouldBe(new ListItem("-Select-", "0")));
            reports = reports.OrderBy(x => x.ReportName).ToList();
            for (int i = 0; i < reports.Count; i++)
            {
                var lstItem = _drpReports.Items[i + 1];
                var report = reports[i];
                lstItem.Attributes["OptGroup"].ShouldBe(report.IsExport ? "Exports" : "Reports");
                lstItem.Text.ShouldBe(report.ReportName);
                lstItem.Value.ShouldBe(report.ReportID.ToString());
            }
        }

        private void Init_LoadReportDropDownTest()
        {
            _drpReports = new DropDownList();
            _privateTestObject.SetField("drpReports", BindingFlags.NonPublic | BindingFlags.Instance, _drpReports);
        }

        private commEntities.Reports CreateReports_LoadReportDropDownTest(int reportId, bool isExported, string reportName)
        {
            return new commEntities.Reports()
            {
                ShowInSetup = true,
                ReportID = reportId,
                IsExport = isExported,
                ReportName = reportName,
            };
        }

        private List<commEntities.Reports> CreateAllReports_LoadReportDropDownTest()
        {
            var reports = new List<commEntities.Reports>();
            for (int i = 1; i < 13; i++)
            {
                reports.Add(CreateReports_LoadReportDropDownTest(i, false, $"0-{i}-reportName"));
                reports.Add(CreateReports_LoadReportDropDownTest(i, true, $"1-{i}-reportName"));
            }
            return reports.OrderByDescending(x => x.ReportName).ToList();
        }
    }
}
