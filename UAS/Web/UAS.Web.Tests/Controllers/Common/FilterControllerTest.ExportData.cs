using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO.Fakes;
using System.Text;
using System.Web.Mvc;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using FrameworkUAD.Object;
using FrameworkUAD.Object.Fakes;
using Kendo.Mvc.Extensions;
using NUnit.Framework;
using Shouldly;
using UAS.Web.Controllers.Common.Fakes;
using UAS.Web.Helpers.Fakes;
using UAS.Web.Models.Common;
using Enums = FrameworkUAD.BusinessLogic.Enums;
using ShimFilterMVC = FrameworkUAD.BusinessLogic.Fakes.ShimFilterMVC;
using ShimSubscriber = FrameworkUAD.BusinessLogic.Fakes.ShimSubscriber;

namespace UAS.Web.Tests.Controllers.Common
{
    public partial class FilterControllerTest
    {
        private const string KeySubscriberIds = "SubscriberIds";
        private const string KeyCurrentFilters = "CurrentFilters";
        private const string PrefixDemo = "demo";
        private const string PrefixAdhoc = "adhoc";
        private const string OptionDownload = "Download";
        private const string OptionExport = "Export";
        private const string MessageNoFieldSelectedForDownload = "Please select atleast one field for download or export.";
        private const string MessageDemoFieldShouldNotMoreThan5 = "Demofields should not be more than 5.";
        private const string MessageAdhocFieldShouldNotMoreThan5 = "AdhocFields should not be more than 5.";
        private const string MessageSelectGroupForExportData = "<font color='red'>Please select New Group or Existing group.</font>";
        private const string MessageGroupNameExists = "<font color='#000000'>\"{0}\"</font> listname already exists. Please enter a different name.";
        private const int ErrorFieldCount = 6;
        private const string GroupName = "group1";
        private const string ColumnNameFirstName = "FirstName";
        private const string ColumnNameSubscriptionId = "SUBSCRIPTIONID";
        private const string ColumnNameDummy = "DummyColumn";
        private const string ColumnNameSortOrder = "SortOrder";
        private const string ExportResultPartialView = "Partials/Common/_exportResult";
        private const string DownloadRootPath = "../main/temp/";
        private const string DownloadAddRemoveRootPath = "../addkilldownloads/main/";
        private const string PromoteCode = "1234";
        private const string OptionSaveToCampaign = "SaveToCampaign";
        private const string PropertySuccess = "success";
        private const string PropertySuccessMessage = "successmessge";
        private const int SampleCount = 3;
        private const string MessageTotalCampaignSubscriber = "Total subscriber in the campaign : ";

        private static readonly SelectListItem[] DownloadFields = {
            new SelectListItem{ Text = "ADDRESS1", Value = "Address" },
            new SelectListItem{ Text = "REGIONCODE", Value = "State" },
            new SelectListItem{ Text = "ZIPCODE", Value = "Zip" },
            new SelectListItem{ Text = "PUBTRANSACTIONDATE", Value = "TransactionDate" },
            new SelectListItem{ Text = "QUALIFICATIONDATE", Value = "QDate" }
        };

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ExportData_NoItemSelected_ReturnsError(bool isArchived)
        {
            // Arrange
            var model = new DownloadViewModel
            {
                IssueID = SampleId,
                MasterOptionSelected = OptionExport
            };
            ShimForIssueList(isArchived);

            // Act
            var result = _testEntity.ExportData(model) as JsonResult;

            // Assert
            VerifySaveFilterJsonResult(result, true, MessageNoFieldSelectedForDownload);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ExportData_ItemSelectedMoreThanLimit_ReturnsError(bool isDemo)
        {
            // Arrange
            var expectedMessage = isDemo ? MessageDemoFieldShouldNotMoreThan5 : MessageAdhocFieldShouldNotMoreThan5;
            var model = new DownloadViewModel
            {
                IssueID = 0,
                MasterOptionSelected = OptionExport,
                SelectedItems = CreateSelectedItems(ErrorFieldCount, isDemo)
            };

            // Act
            var result = _testEntity.ExportData(model) as JsonResult;

            // Assert
            VerifySaveFilterJsonResult(result, true, expectedMessage);
        }

        [Test]
        public void ExportData_DownloadForReportAndMasterOptionSelectedExportAndNoGroupSelected_ReturnsError()
        {
            // Arrange
            var model = new DownloadViewModel
            {
                IssueID = 0,
                DownloadFor = DownloadForReport,
                MasterOptionSelected = OptionExport,
                SelectedItems = CreateSelectedItems(1, true)
            };
            ShimForExportDataCommon();
            ShimForExportDataDownloadForReport();

            // Act
            var result = _testEntity.ExportData(model) as JsonResult;

            // Assert
            VerifySaveFilterJsonResult(result, true, MessageSelectGroupForExportData);
        }

        [Test]
        public void ExportData_DownloadForReportAndMasterOptionSelectedExportAndIsNewGroupChecked_ReturnsError()
        {
            // Arrange
            var model = new DownloadViewModel
            {
                IssueID = 0,
                DownloadFor = DownloadForReport,
                MasterOptionSelected = OptionExport,
                IsNewGroupChecked = true,
                GroupName = GroupName,
                SelectedItems = CreateSelectedItems(1, true)
            };
            ShimForExportDataCommon();
            ShimForExportDataDownloadForReport();
            ShimGroup.ExistsByGroupNameByCustomerIDStringInt32 = (s, i) => true;

            // Act
            var result = _testEntity.ExportData(model) as JsonResult;

            // Assert
            VerifySaveFilterJsonResult(result, true, string.Format(MessageGroupNameExists, GroupName));
        }

        [Test]
        public void ExportData_DownloadForReportAndMasterOptionSelectedExportAndIsExistingGroupChecked_ReturnsExportResultPartialView()
        {
            // Arrange
            var model = new DownloadViewModel
            {
                IssueID = 0,
                DownloadFor = DownloadForReport,
                MasterOptionSelected = OptionExport,
                IsExistingGroupChecked = true,
                GroupName = GroupName,
                SelectedItems = CreateSelectedItems(1, true)
            };
            ShimForExportDataCommon();
            ShimForExportDataDownloadForReport();
            ShimUtilities.ExportToECNInt32StringInt32Int32StringStringListOfExportFieldsDataTableInt32EnumsGroupExportSource
                = (a, b, c, d, e, f, g, h, i, j) => new Hashtable{{ string.Empty, string.Empty }};

            // Act
            var result = _testEntity.ExportData(model) as PartialViewResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ViewName.ShouldBe(ExportResultPartialView));
        }

        [Test]
        public void ExportData_DownloadForReportAndMasterOptionSelectedDownload_ReturnsJsonResultWithFilePath()
        {
            // Arrange
            var model = new DownloadViewModel
            {
                IssueID = 0,
                DownloadFor = DownloadForReport,
                MasterOptionSelected = OptionDownload,
                PromoCode = PromoteCode,
                SelectedItems = CreateSelectedItems(1, true)
            };
            ShimForExportDataCommon();
            ShimForExportDataDownloadForReportDownload(model.SelectedItems);

            // Act
            var result = _testEntity.ExportData(model) as JsonResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Data.ToString().ShouldStartWith(DownloadRootPath));
        }

        [Test]
        public void ExportData_DownloadForReportAndMasterOptionSelectedSaveToCampaign_ReturnsSuccessJsonResult()
        {
            // Arrange
            var model = new DownloadViewModel
            {
                IssueID = 0,
                DownloadFor = DownloadForReport,
                MasterOptionSelected = OptionSaveToCampaign,
                IsNewCampaign = true,
                SelectedItems = CreateSelectedItems(1, true)
            };
            ShimForExportDataCommon();
            ShimForExportDataDownloadForReportSaveToCampaign();

            // Act
            var result = _testEntity.ExportData(model) as JsonResult;

            // Assert
            VerifySuccessJsonResult(result, MessageTotalCampaignSubscriber + SampleCount);
        }

        [Test]
        public void ExportData_DownloadForAddRemove_ReturnsJsonResultWithFilePath()
        {
            // Arrange
            var model = new DownloadViewModel
            {
                IssueID = 0,
                DownloadFor = DownloadForAddRemove,
                MasterOptionSelected = OptionSaveToCampaign,
                IsNewCampaign = true,
                SelectedItems = CreateSelectedItems(1, true)
            };
            ShimForExportDataCommon();
            ShimForExportDataDownloadForAddRemove(model.SelectedItems);

            // Act
            var result = _testEntity.ExportData(model) as JsonResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Data.ToString().ShouldStartWith(DownloadAddRemoveRootPath));
        }

        private List<SelectListItem> CreateSelectedItems(int itemCount, bool isDemo)
        {
            var list = new List<SelectListItem>();
            var demoDictionary = new Dictionary<string, string>();
            var adhocDictionary = new Dictionary<string, string>();

            for (var i = itemCount; i > 0; i--)
            {
                var adhocFieldName = $"{PrefixAdhoc}{i}";
                adhocDictionary.Add(adhocFieldName, string.Empty);

                var demoFieldName = $"{PrefixDemo}{i}";
                demoDictionary.Add(demoFieldName, string.Empty);

                var item = new SelectListItem
                {
                    Selected = true,
                    Value = isDemo ? demoFieldName : adhocFieldName
                };
                item.Text = item.Value;
                list.Add(item);
            }

            ShimUtilities.GetExportingFieldsClientConnectionsInt32EnumsExportTypeInt32EnumsExportFieldTypeStringBoolean
                = (a, b, c, d, type, f, g) => type == Enums.ExportFieldType.Adhoc
                    ? adhocDictionary
                    : demoDictionary;

            return list;
        }

        private void ShimForExportDataCommon()
        {
            UserServices.Add(KMPlatform.Enums.Services.UAD);
            UserFeatures.Add(KMPlatform.Enums.ServiceFeatures.UADExport);
            UserAccesses.Add(KMPlatform.Enums.Access.Download);
            UserAccesses.Add(KMPlatform.Enums.Access.ExportToGroup);
            UserAccesses.Add(KMPlatform.Enums.Access.SaveToCampaign);

            var filterMvcList = new List<FilterMVC> { new FilterMVC() };
            SessionMock.Setup(s => s[KeyCurrentFilters]).Returns(filterMvcList);
            SessionMock.Setup(s => s[KeySubscriberIds]).Returns(new List<int>());

            ShimUtilities.GetSelectedPubSubExtMapperExportColumnsClientConnectionsListOfStringInt32
                = (a, b, c) => new List<string>();
            ShimUtilities.GetSelectedResponseGroupStandardExportColumnsClientConnectionsListOfStringInt32BooleanBoolean
                = (a, b, c, d, e) => Tuple.Create(new List<string>(), new List<string>(), new List<string>());
            ShimUtilities.GetStandardExportColumnFieldNameListOfStringEnumsViewTypeInt32BooleanBoolean
                = (a, b, c, d, e) => new List<string>();
            ShimUtilities.GetSelectedCustomExportColumnsListOfString = list => list;
            ShimUtilities.DownloadDataTableStringStringInt32Int32 = (a, b, c, d, e) => { };
            ShimDirectory.ExistsString = s => true;
        }

        private void ShimForExportDataDownloadForReport()
        {
            var subscriptionTable = new DataTable();
            subscriptionTable.Columns.Add(ColumnNameFirstName);
            subscriptionTable.Columns.Add(ColumnNameSubscriptionId);
            subscriptionTable.Columns.Add(ColumnNameDummy);
            ShimForExportDataDownloadForReportCommon(subscriptionTable);
        }

        private void ShimForExportDataDownloadForReportDownload(ICollection<SelectListItem> listItems)
        {
            var subscriptionTable = new DataTable();
            listItems.AddRange(DownloadFields);

            foreach (var selectListItem in listItems)
            {
                subscriptionTable.Columns.Add(selectListItem.Value);
            }

            ShimForExportDataDownloadForReportCommon(subscriptionTable);
            ShimProfileFieldMask.MaskDataClientConnectionsObjectUser = (a, table, c) => table;
        }

        private void ShimForExportDataDownloadForReportSaveToCampaign()
        {
            var subscriptionTable = new DataTable();
            subscriptionTable.Columns.Add(ColumnNameSubscriptionId);
            subscriptionTable.Rows.Add(SampleId);

            ShimForExportDataDownloadForReportCommon(subscriptionTable);
            FrameworkUAD.BusinessLogic.Fakes.ShimCampaign.AllInstances.CampaignExistsClientConnectionsString
                = (a, b, c) => 0;
            FrameworkUAD.BusinessLogic.Fakes.ShimCampaign.AllInstances.SaveCampaignClientConnections
                = (a, b, c) => SampleId;
            FrameworkUAD.BusinessLogic.Fakes.ShimCampaign.AllInstances.GetCountByCampaignIDClientConnectionsInt32
                = (a, b, c) => SampleCount;
            FrameworkUAD.BusinessLogic.Fakes.ShimCampaignFilter.AllInstances
                .CampaignFilterExistsClientConnectionsStringInt32 = (a, b, c, d) => 0;
            FrameworkUAD.BusinessLogic.Fakes.ShimCampaignFilter.AllInstances
                .InsertClientConnectionsStringInt32Int32String = (a, b, c, d, e, f) => SampleId;
            FrameworkUAD.BusinessLogic.Fakes.ShimCampaignFilterDetail.AllInstances
                .saveCampaignDetailsClientConnectionsInt32String = (a, b, c, d) => { };
        }

        private void ShimForExportDataDownloadForReportCommon(DataTable subscriptionTable)
        {
            var importedTable = new DataTable();
            importedTable.Columns.Add(ColumnNameSortOrder);

            ShimFilterMVC.generateCombinationQueryFilterCollectionStringStringStringStringStringInt32Int32ClientConnections
                = (a, b, c, d, e, f, g, h, i) => new StringBuilder();
            ShimFilterMVC.getProductArchiveFilterQueryFilterMVCStringStringInt32ClientConnections
                = (a, b, c, d, e) => string.Empty;
            ShimSubscriber.AllInstances
                    .GetArchivedProductDimensionSubscriberDataClientConnectionsStringListOfStringListOfInt32ListOfStringListOfStringListOfStringListOfStringInt32Int32Int32
                = (a, b, c, d, e, f, g, h, i, j, k, l) => subscriptionTable;
            ShimSubscriber.AllInstances
                    .GetSubscriberDataClientConnectionsStringBuilderListOfStringListOfStringListOfStringListOfStringListOfStringInt32ListOfInt32BooleanInt32String
                = (a, b, c, d, e, f, g, h, i, j, k, l, m) => subscriptionTable;
            ShimSubscriber.AllInstances
                    .GetProductDimensionSubscriberDataClientConnectionsStringBuilderListOfStringListOfInt32ListOfStringListOfStringListOfStringListOfStringInt32Int32BooleanListOfInt32
                = (a, b, c, d, e, f, g, h, i, j, k, l, m) => subscriptionTable;
            ShimCustomer.GetByClientIDInt32Boolean = (i, b) => new ECN_Framework_Entities.Accounts.Customer{ CustomerID = 1 };
            ShimGroup.ExistsByGroupNameByCustomerIDStringInt32 = (s, i) => false;
            ShimGroup.GetByGroupIDInt32User = (i, user) => new Group{ MasterSupression = 1 };
            ShimUtilities.ExportToECNInt32StringInt32Int32StringStringListOfExportFieldsDataTableInt32EnumsGroupExportSource
                = (a, b, c, d, e, f, g, h, i, j) => new Hashtable();
            ShimUtilities.getImportedResultHashtableDateTime = (a, b) => importedTable;
        }

        private void ShimForExportDataDownloadForAddRemove(ICollection<SelectListItem> listItems)
        {
            var dataTable = new DataTable();
            listItems.AddRange(DownloadFields);
            foreach (var selectListItem in listItems)
            {
                dataTable.Columns.Add(selectListItem.Value);
            }
            ShimSubscriber.AllInstances
                    .GetProductDimensionSubscriberDataClientConnectionsStringBuilderListOfStringListOfInt32ListOfStringListOfStringListOfStringListOfStringInt32Int32BooleanListOfInt32
                = (a, b, c, d, e, f, g, h, i, j, k, l, m) => dataTable;
            ShimProfileFieldMask.MaskDataClientConnectionsObjectUser = (a, table, c) => table;
            ShimIEnumerableExtention.DataTableToCSVDataTableChar = (a, b) => string.Empty;
            ShimFilterController.AllInstances.AddRemoveCreateTSVFileStringStringDataTable = (a, b, c, d) => { };
        }

        private void VerifySuccessJsonResult(JsonResult result, string message)
        {
            result.ShouldNotBeNull();

            var jsonObject = result.Data;
            jsonObject.ShouldSatisfyAllConditions(
                () => jsonObject.ShouldNotBeNull(),
                () => VerifyProperty(jsonObject, PropertySuccess, true),
                () => VerifyProperty(jsonObject, PropertySuccessMessage, message));
        }
    }
}
