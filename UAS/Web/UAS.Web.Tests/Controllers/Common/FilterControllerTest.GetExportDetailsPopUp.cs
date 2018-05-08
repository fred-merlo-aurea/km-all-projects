using System.Collections.Generic;
using System.Web.Mvc;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAD.Object;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;
using UAS.Web.Controllers.Common.Fakes;
using UAS.Web.Models.Common;

namespace UAS.Web.Tests.Controllers.Common
{
    public partial class FilterControllerTest
    {
        private const string ExportDetailsPopupPartialView = "Partials/Common/_exportDetailsPopup";
        private const string DownloadForReport = "Report";
        private const string DownloadForAddRemove = "AddRemove";
        private const string DownloadForIssueSplit = "IssueSplit";
        private const string DownloadForRecordUpdate = "RecordUpdate";
        private const string KeyAddRemoveSubscriberIds = "AddRemoveSubscriberIds";

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetExportDetailsPopUp_DownloadForReport_ReturnsPartialView(bool isArchived)
        {
            // Arrange
            var idList = new List<int> { SampleId };
            var model = new DownloadViewModel
            {
                IssueID = SampleId,
                DownloadFor = DownloadForReport,
                FilterList = new List<FilterMVC>{ new FilterMVC() }
            };
            ShimForIssueList(isArchived);
            ShimFilterMVC.getProductFilterQueryFilterMVCClientConnectionsBoolean = (a, b, c) => string.Empty;
            ShimFilterMVC.getProductArchiveFilterQueryFilterMVCInt32ClientConnections = (a, b, c) => string.Empty;
            ShimReport.AllInstances.SelectSubscriberCountMVCStringClientConnections = (a, b, c) => idList;
            ShimClient.AllInstances.SelectActiveForClientGroupInt32 = (a, b) => new List<Client>{ new Client() };
            ShimDownloadTemplate.GetByPubIDBrandIDClientConnectionsInt32Int32 = (a, b, c) => new List<DownloadTemplate>{ new DownloadTemplate() };
            ShimFilterController.AllInstances.ExportFieldsDownloadViewModel = (a, viewModel) => viewModel;

            // Act
            var result = _testEntity.GetExportDetailsPopUp(model) as PartialViewResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ViewName.ShouldBe(ExportDetailsPopupPartialView));

            model.ShouldSatisfyAllConditions(
                () => model.IsArchived.ShouldBe(isArchived),
                () => model.DownloadVisible.ShouldBeTrue(),
                () => model.PromoCodeVisible.ShouldBeTrue(),
                () => model.QueryDetailsCheckboxVisible.ShouldBeTrue(),
                () => model.ExportToGroupVisible.ShouldBe(!isArchived),
                () => model.SaveToCampaignVisible.ShouldBe(!isArchived),
                () => model.DisplayDownLoad.ShouldBe(!isArchived),
                () => model.IsQueryDetailsIncluded.ShouldBeFalse(),
                () => model.DownloadCount.ShouldBe(idList.Count),
                () => model.TotalCount.ShouldBe(idList.Count),
                () => model.DownLoadTemplates.Count.ShouldBe(1),
                () => model.Customers.Count.ShouldBe(1));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetExportDetailsPopUp_DownloadForAddRemove_ReturnsPartialView(bool isArchived)
        {
            // Arrange
            var idList = new List<int> { SampleId };
            var model = new DownloadViewModel
            {
                IssueID = SampleId,
                DownloadFor = DownloadForAddRemove,
                FilterList = new List<FilterMVC>{ new FilterMVC() }
            };
            ShimForIssueList(isArchived);
            ShimProductSubscription.AllInstances.SelectProductIDInt32ClientConnections = (a, b, c) => new List<ActionProductSubscription>
            {
                new ActionProductSubscription{ PubSubscriptionID = SampleId }
            };
            ShimDownloadTemplate.GetByPubIDBrandIDClientConnectionsInt32Int32 = (a, b, c) => new List<DownloadTemplate>{ new DownloadTemplate() };
            ShimFilterController.AllInstances.ExportFieldsDownloadViewModel = (a, viewModel) => viewModel;
            SessionMock.Setup(s => s[KeyAddRemoveSubscriberIds]).Returns(idList);

            // Act
            var result = _testEntity.GetExportDetailsPopUp(model) as PartialViewResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ViewName.ShouldBe(ExportDetailsPopupPartialView));

            model.ShouldSatisfyAllConditions(
                () => model.IsArchived.ShouldBe(isArchived),
                () => model.DownloadVisible.ShouldBeFalse(),
                () => model.PromoCodeVisible.ShouldBeFalse(),
                () => model.QueryDetailsCheckboxVisible.ShouldBeFalse(),
                () => model.ExportToGroupVisible.ShouldBeFalse(),
                () => model.SaveToCampaignVisible.ShouldBeFalse(),
                () => model.DisplayDownLoad.ShouldBeTrue(),
                () => model.IsQueryDetailsIncluded.ShouldBeFalse(),
                () => model.DownloadCount.ShouldBe(idList.Count),
                () => model.TotalCount.ShouldBe(idList.Count),
                () => model.DownLoadTemplates.Count.ShouldBe(1),
                () => model.Customers.Count.ShouldBe(0));
        }

        [Test]
        [TestCase(DownloadForIssueSplit)]
        [TestCase(DownloadForRecordUpdate)]
        public void GetExportDetailsPopUp_DownloadForOtherTypes_ReturnsPartialView(string type)
        {
            // Arrange
            var model = new DownloadViewModel
            {
                IssueID = 0,
                DownloadFor = type,
                FilterList = new List<FilterMVC>{ new FilterMVC() }
            };
            ShimFilterController.AllInstances.ExportFieldsDownloadViewModel = (a, viewModel) => viewModel;

            // Act
            var result = _testEntity.GetExportDetailsPopUp(model) as PartialViewResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ViewName.ShouldBe(ExportDetailsPopupPartialView));

            model.ShouldSatisfyAllConditions(
                () => model.IsArchived.ShouldBeFalse(),
                () => model.DownloadVisible.ShouldBeTrue(),
                () => model.PromoCodeVisible.ShouldBeFalse(),
                () => model.QueryDetailsCheckboxVisible.ShouldBeFalse(),
                () => model.ExportToGroupVisible.ShouldBeFalse(),
                () => model.SaveToCampaignVisible.ShouldBeFalse(),
                () => model.DisplayDownLoad.ShouldBeTrue(),
                () => model.IsQueryDetailsIncluded.ShouldBeFalse(),
                () => model.DownloadCount.ShouldBe(0),
                () => model.TotalCount.ShouldBe(0),
                () => model.DownLoadTemplates.Count.ShouldBe(0),
                () => model.Customers.Count.ShouldBe(0));
        }

        private void ShimForIssueList(bool isArchived)
        {
            var issueList = new List<Issue>
            {
                new Issue
                {
                    IssueId = SampleId,
                    IsComplete = isArchived
                }
            };

            ShimIssue.AllInstances.SelectClientConnections = (a, b) => issueList;
        }
    }
}
