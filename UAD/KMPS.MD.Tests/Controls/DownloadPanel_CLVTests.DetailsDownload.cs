using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Fakes;
using System.Web.Fakes;
using System.Web.SessionState.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using FrameworkUAS.Entity;
using KMPlatform.Entity;
using KMPlatform.Object;
using KMPS.MD.Controls.Fakes;
using KMPS.MD.Objects;
using KMPS.MD.Objects.Fakes;
using NUnit.Framework;
using Shouldly;
using static KMPS.MD.Objects.Enums;
using BusinessLogicFakes = FrameworkUAS.BusinessLogic.Fakes;
using KM.Platform.Fakes;
using UADBusinessLogicFakes = FrameworkUAD_Lookup.BusinessLogic.Fakes;
using Entity = FrameworkUAD_Lookup.Entity;
using Enums = FrameworkUAD_Lookup.Enums;

namespace KMPS.MD.Tests.Controls
{
    public partial class DownloadPanel_CLVTests
    {
        private const string MethodDetailsDownload = "DetailsDownload";
        private const string DelMethod = "DelMethod";
        private const string DummyString = "DummyString";
        private const string LblErrorMessage = "lblErrorMessage";
        private const string LstSelectedFields = "lstSelectedFields";
        private const string ViewTypeProperty = "ViewType";
        private const string PubIDs = "PubIDs";
        private const int DummyId = 1;
        private const string RbDownloadAll = "rbDownloadAll";
        private const string RbDownload = "rbDownload";
        private const string SubscriptionId = "SubscriptionID";
        private const string FilterCombination = "filterCombination";
        private const string Matched = "Matched";
        private const string DcTypeCodeId = "dcTypeCodeID";
        private const string DcRunId = "dcRunID";
        private const string BrandId = "BrandID";
        private const string DrpIsBillable = "drpIsBillable";
        private const string TxtDownloadCount = "txtDownloadCount";
        private const string TxtDownloadUniqueCount = "txtDownloadUniqueCount";
        private const string CgrpNoColumnName = "CGRP_NO";
        private bool _filterExecute;
        private bool _downloadCalled;

        private static readonly string[] _columnNames =
        {
            "ADDRESS1",
            "REGIONCODE",
            "ZIPCODE",
            "PUBTRANSACTIONDATE",
            "QUALIFICATIONDATE",
            DummyString,
            "FNAME",
            "LNAME",
            "ISLATLONVALID"
        };
        private static readonly string[] _equivalentColumnNames =
        {
            "Address",
            "State",
            "Zip",
            "TransactionDate",
            "QDate",
            DummyString,
            "FirstName",
            "LastName",
            "GeoLocated"
        };

        [Test]
        [TestCase(ViewType.ConsensusView)]
        [TestCase(ViewType.ProductView)]
        public void DetailsDownload_RbDownloadAllIsNotChecked_DisplayErrorMessage(ViewType viewType)
        {
            // Arrange
            SetupShimsForSession();
            PrivateControl.SetProperty(ViewTypeProperty, viewType);
            SetupForDetailsDownload(DummyId, false);
            SetupShimsForUtilities(DummyString);
            const string ExpectedMessage = "Select Download All or Download one record per location";

            // Act	
            PrivateControl.Invoke(MethodDetailsDownload);
            var lblErrorMessage = PrivateControl.GetField(LblErrorMessage) as Label;

            // Assert
            lblErrorMessage.ShouldSatisfyAllConditions(
                () => lblErrorMessage.ShouldNotBeNull(),
                () => lblErrorMessage.Text.ShouldContain(ExpectedMessage)
            );
        }

        [Test]
        public void DetailsDownload_RbDownloadAllIsChecked_ReachEnd(
            [Values(ViewType.ConsensusView, ViewType.ProductView)] ViewType viewType,
            [Values(true, false)] bool rbDownloadAll,
            [ValueSource(nameof(_columnNames))] string columnName,
            [Values(DummyId, 0)] int dcTargetCodeId,
            [Values(true, false)] bool isBillable,
            [Values(true, false)] bool drpIsBillable,
            [Range(0, 1)] int brandId)
        {
            // Arrange
            SetupShimsForSession();
            PrivateControl.SetProperty(ViewTypeProperty, viewType);
            PrivateControl.SetField(RbDownloadAll, new RadioButton() { Checked = rbDownloadAll });
            PrivateControl.SetField(RbDownload, new RadioButton() { Checked = true });
            SetupForDetailsDownload(brandId, drpIsBillable);
            SetupShimsForSubscriber(columnName);
            SetupShimsForUtilities(columnName);
            SetupShimForDataCompare(dcTargetCodeId, isBillable);
            SetupShimsForCode();

            // Act	
            PrivateControl.Invoke(MethodDetailsDownload);
            var lblErrorMessage = PrivateControl.GetField(LblErrorMessage) as Label;

            // Assert
            lblErrorMessage.ShouldSatisfyAllConditions(
                () => lblErrorMessage.ShouldNotBeNull(),
                () => lblErrorMessage.Text.ShouldBeNullOrWhiteSpace(),
                () => _filterExecute.ShouldBeTrue(),
                () => _downloadCalled.ShouldBeTrue()
            );
        }

        private void SetupForDetailsDownload(int brandId, bool drpIsBillable)
        {
            _filterExecute = false;
            _downloadCalled = false;
            PrivateControl.SetProperty(PubIDs, new List<int> { DummyId });
            Action delMethodFunc = () => { };
            PrivateControl.SetField(DelMethod, delMethodFunc);
            PrivateControl.SetProperty(FilterCombination, Matched);
            var listBox = new ListBox();
            listBox.Items.Add(new ListItem(DummyString, DummyString));
            PrivateControl.SetField(LstSelectedFields, listBox);
            PrivateControl.SetProperty(DcTypeCodeId, DummyId);
            PrivateControl.SetProperty(DcRunId, DummyId);
            PrivateControl.SetProperty(BrandId, brandId);
            var dropDownList = new DropDownList();
            dropDownList.Items.Add(new ListItem(DummyString, drpIsBillable.ToString()));
            dropDownList.SelectedIndex = 0;
            PrivateControl.SetField(DrpIsBillable, dropDownList);
            PrivateControl.SetField(TxtDownloadCount, new TextBox { Text = DummyId.ToString() });
            PrivateControl.SetField(TxtDownloadUniqueCount, new TextBox { Text = DummyId.ToString() });
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (_, __, ___, ____) => true;
            var shimSession = new ShimECNSession();
            shimSession.Instance.CurrentUser = new User { UserID = DummyId };
            shimSession.UserIDGet = () => DummyId;
            shimSession.ClientIDGet = () => DummyId;
            ShimECNSession.CurrentSession = () => shimSession.Instance;
            ShimProfileFieldMask.MaskDataClientConnectionsObjectUser = (_, __, ___) =>
            {
                var dataTable = new DataTable();
                dataTable.Columns.Add(SubscriptionId, typeof(string));
                var row = dataTable.NewRow();
                row[0] = DummyString;
                dataTable.Rows.Add(row);
                return dataTable;
            };
            ShimDownloadPanel_CLV.AllInstances.saveDataCompareViewInt32NullableOfInt32Int32EnumsDataCompareTypeInt32 =
                (_, __, ___, ____, _____, ______) => DummyId;
            ShimFilter.AllInstances.ExecuteClientConnectionsString = (_, __, ___) => { _filterExecute = true; };
            ShimResponseGroup.GetActiveByPubIDClientConnectionsInt32 = (_, __) => new List<ResponseGroup>
            {
                new ResponseGroup
                {
                    ResponseGroupID = DummyId,
                }
            };
            var masterGroupList = new List<MasterGroup>
            {
                new MasterGroup
                {
                    ColumnReference = DummyString
                }
            };
            ShimMasterGroup.GetActiveByBrandIDClientConnectionsInt32 = (_, __) => masterGroupList;
            ShimMasterGroup.GetActiveMasterGroupsSortedClientConnections = (_) => masterGroupList;
            ShimUserControl.AllInstances.ServerGet = (_) => new ShimHttpServerUtility();
            ShimHttpServerUtility.AllInstances.MapPathString = (_, __) => DummyString;
            ShimDirectory.CreateDirectoryString = (_) => new ShimDirectoryInfo();
            ShimPubSubscriptionsExtensionMapper.GetActiveByPubIDClientConnectionsInt32 = (_, __) =>
                new List<PubSubscriptionsExtensionMapper>
                {
                    new PubSubscriptionsExtensionMapper
                    {
                        CustomField = DummyString,
                        PubSubscriptionsExtensionMapperId = DummyId
                    }
                };
            ShimSubscriptionsExtensionMapper.GetActiveClientConnections = (_) => new List<SubscriptionsExtensionMapper>
            {
                new SubscriptionsExtensionMapper
                {
                    CustomField = DummyString,
                    SubscriptionsExtensionMapperId = DummyId
                }
            };
        }

        private static void SetupShimsForCode()
        {
            UADBusinessLogicFakes::ShimCode.AllInstances.SelectCodeIdInt32 = (_, __) => new Entity::Code
            {
                CodeName = Enums.DataCompareType.Like.ToString()
            };
            UADBusinessLogicFakes::ShimCode.AllInstances.SelectCodeNameEnumsCodeTypeString = (_, __, ___) =>
                new Entity::Code
                {
                    CodeId = DummyId
                };
            ShimCode.GetDataCompareTarget = () => new List<Code>
            {
                new Code
                {
                    CodeName = DataCompareViewType.Consensus.ToString(),
                    CodeID = DummyId
                }
            };
            ShimCode.GetUADFieldType = () => new List<Code>
            {
                new Code
                {
                    CodeName = Enums.UADFieldType.Profile.ToString(),
                    CodeID = DummyId
                },
                new Code
                {
                    CodeName = Enums.UADFieldType.Custom.ToString(),
                    CodeID = DummyId
                },
                new Code
                {
                    CodeName = Enums.UADFieldType.Dimension.ToString(),
                    CodeID = DummyId
                },
                new Code
                {
                    CodeName = Enums.UADFieldType.Adhoc.ToString(),
                    CodeID = DummyId
                }
            };
        }

        private static void SetupShimForDataCompare(int dcTargetCodeId, bool isBillable)
        {
            BusinessLogicFakes::ShimDataCompareDownloadDetail.AllInstances.SaveInt32String = (_, __, ___) => { };
            BusinessLogicFakes::ShimDataCompareDownloadCostDetail.AllInstances
                .CreateCostDetailInt32Int32StringStringStringInt32 = (_, __, ___, ____, _____, ______, _______) =>
                new List<DataCompareDownloadCostDetail>();
            BusinessLogicFakes::ShimDataCompareDownload.AllInstances.SaveDataCompareDownload = (_, __) => DummyId;
            BusinessLogicFakes::ShimDataCompareView.AllInstances.SelectForRunInt32 = (_, __) => new List<DataCompareView>
            {
                new DataCompareView
                {
                    DcTargetCodeId = dcTargetCodeId,
                    IsBillable = isBillable,
                    DcViewId = dcTargetCodeId,
                    PaymentStatusId = dcTargetCodeId,
                    DcTypeCodeId = DummyId,
                    DcTargetIdUad = null
                }
            };
            BusinessLogicFakes::ShimDataCompareView.AllInstances.SaveDataCompareView =
                (_, __) => DummyId;
            BusinessLogicFakes::ShimDataCompareView.AllInstances.DeleteInt32 = (_, __) => true;
            BusinessLogicFakes::ShimDataCompareView.AllInstances
                .GetDataCompareCostInt32Int32Int32EnumsDataCompareTypeEnumsDataCompareCost =
                (_, __, ___, ____, _____, ______) => DummyId;
            BusinessLogicFakes::ShimDataCompareDownloadFilterGroup.AllInstances.SaveDataCompareDownloadFilterGroup =
                (_, __) => DummyId;
            BusinessLogicFakes::ShimDataCompareDownloadFilterDetail.AllInstances.SaveDataCompareDownloadFilterDetail =
                (_, __) => DummyId;
            BusinessLogicFakes::ShimDataCompareDownloadField.AllInstances.SaveDataCompareDownloadField =
                (_, __) => DummyId;
        }

        private static void SetupShimsForSubscriber(string columnName)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add(SubscriptionId, typeof(string));
            dataTable.Columns.Add(columnName, typeof(string));
            dataTable.Columns.Add(CgrpNoColumnName, typeof(string));
            foreach (var column in _equivalentColumnNames)
            {
                if (!dataTable.Columns.Contains(column))
                {
                    dataTable.Columns.Add(column, typeof(string));
                }
            }
            ShimSubscriber
                    .GetProductDimensionSubscriberData_CLVClientConnectionsStringBuilderListOfStringListOfInt32ListOfStringListOfStringListOfStringListOfStringInt32Boolean
                = (_, __, ___, ____, _____, ______, _______, ________, _________, __________) => dataTable;
            ShimSubscriber
                    .GetSubscriberData_CLVClientConnectionsStringBuilderListOfStringListOfStringListOfStringListOfStringListOfStringInt32ListOfInt32BooleanBoolean
                = (_, __, ___, ____, _____, ______, _______, ________, _________, __________, ___________) => dataTable;
        }

        private void SetupShimsForUtilities(string columnName)
        {
            ShimUtilities.GetSelectedPubSubExtMapperExportColumnsClientConnectionsListOfStringInt32 = (_, __, ___) =>
                new List<string>() { DummyString };
            ShimBaseControl.AllInstances.clientconnectionsGet = (_) => new ClientConnections(DummyString, DummyString);
            ShimUtilities.GetSelectedStandardExportColumnsClientConnectionsListOfStringInt32 = (_, __, ___) =>
                new List<string>() { DummyString };
            ShimUtilities.GetSelectedSubExtMapperExportColumnsClientConnectionsListOfString = (_, __) =>
                new List<string>() { DummyString };
            ShimUtilities.GetStandardExportColumnFieldNameIListOfStringEnumsViewTypeInt32Boolean = (_, __, ___, ____) =>
                new List<string>();
            ShimUtilities.GetSelectedMasterGroupExportColumnsClientConnectionsListOfStringInt32 = (_, __, ___) =>
                new Tuple<List<string>, List<string>>(
                    new List<string> { DummyString },
                    new List<string> { DummyString }
                );
            ShimUtilities.GetSelectedCustomExportColumnsListOfString = (_) =>
                new List<string> { DummyString };
            ShimUtilities.GetSelectedResponseGroupStandardExportColumnsClientConnectionsListOfStringInt32BooleanBoolean
                = (_, __, ___, ____, _____) => new Tuple<List<string>, List<string>, List<string>>(
                    new List<string> { DummyId.ToString() },
                    new List<string> { DummyId.ToString() },
                    new List<string> { DummyId.ToString() }
                );
            ShimUtilities.getListboxSelectedExportFieldsListBox = (_) =>
                new List<dynamic> { new { text = columnName } };
            ShimUtilities.DownloadDataTableStringStringInt32Int32 = (_, __, ___, ____, _____) =>
            {
                _downloadCalled = true;
            };
        }

        private static void SetupShimsForSession()
        {
            var sessionState = new Dictionary<string, object>();
            ShimHttpContext.CurrentGet = () => new ShimHttpContext();
            ShimHttpContext.AllInstances.SessionGet = (o) => new ShimHttpSessionState
            {
                ItemGetString = (key) =>
                {
                    sessionState.TryGetValue(key, out var result);
                    return result;
                }
            };
        }
    }
}
