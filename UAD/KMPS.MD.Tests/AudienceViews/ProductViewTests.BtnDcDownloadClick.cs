using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Fakes;
using System.Linq;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAD_Lookup.Entity;
using FrameworkUAS.BusinessLogic.Fakes;
using FrameworkUAS.Entity;
using KMPS.MD.MasterPages.Fakes;
using KMPS.MD.Objects.Fakes;
using NUnit.Framework;
using Shouldly;
using Telerik.Web.UI;
using static KMPS.MD.Objects.Enums;
using ShimFilter = KMPS.MD.Objects.Fakes.ShimFilter;
using ShimFrameworkUADLookUp = FrameworkUAD_Lookup.BusinessLogic.Fakes;
using ShimProductView = KMPS.MD.Main.Fakes.ShimProductView;

namespace KMPS.MD.Tests.AudienceViews
{
    public partial class ProductViewTests
    {
        private const string TestTwo = "2";
        private const string TestTrueString = "true";
        private const string TestFalseString = "false";
        private const string BtnDCDownloadClick = "btnDCDownload_Click";
        private const string HiddenFiledDcTargetCodeId = "hfDcTargetCodeID";
        private const string HiddenFiledDataCompareProcessCode = "hfDataCompareProcessCode";
        private const string LinkNonMatchedRecords = "lnkNonMatchedRecords";
        private const string PlaceHolderKmStaff = "plKmStaff";
        private const string DropDownListDataCompareSourceFile = "drpDataCompareSourceFile";
        private const string LinkMatchedRecords = "lnkMatchedRecords";
        private const string PlaceholderNotes = "plNotes";
        private const int DummyId = 1;
        private bool _saveDataCompareDownload;
        private bool _selectCodeName;
        private bool _selectForRun;
        private bool _getDataCompareCost;
        private bool _downloadDataCompare;
        private bool _saveDataCompareView;

        [Test]
        public void BtnDCDownloadClick_PageIsInvalid_ReachEnd()
        {
            // Arrange
            ShimPage.AllInstances.IsValidGet = (sender) => false;
            var parameters = new object[] { this, EventArgs.Empty };

            // Act, Assert
            PrivatePage.Invoke(BtnDCDownloadClick, parameters);
        }

        [TestCase(TestOne)]
        [TestCase(TestTwo)]
        [TestCase(TestZero)]
        public void BtnDCDownloadClick_PageIsValidAndSourceFileDropDownHaveSelectedValue_UpdateControlValues(string brandId)
        {
            // Arrange
            CreatePageShimObject(true, int.Parse(brandId));
            SetPagePrivateObject(brandId);
            var drpIsBillable = GetField<DropDownList>(DropDownListIsBillable);
            drpIsBillable.DataSource = new[] { TestTrueString, TestFalseString };
            drpIsBillable.SelectedValue = TestTrueString;
            drpIsBillable.DataBind();
            var parameters = new object[] { this, EventArgs.Empty };

            // Act
            PrivatePage.Invoke(BtnDCDownloadClick, parameters);

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => _selectCodeName.ShouldBeTrue(),
                () => _selectForRun.ShouldBeTrue(),
                () => _downloadDataCompare.ShouldBeTrue(),
                () => _saveDataCompareView.ShouldBeTrue());
        }

        [TestCase(true, DummyId)]
        [TestCase(false, default(int))]
        public void BtnDCDownloadClick_PlKmStaffIsVisibile_UpdateControlValues(bool isBillable, int dcViewId)
        {
            // Arrange
            CreatePageShimObject(true, int.Parse(TestOne));
            SetPagePrivateObject(TestOne);
            var plKmStaff = GetField<PlaceHolder>(PlaceHolderKmStaff);
            plKmStaff.Visible = true;
            var drpIsBillable = GetField<DropDownList>(DropDownListIsBillable);
            drpIsBillable.DataSource = new[] { TestTrueString, TestFalseString };
            drpIsBillable.SelectedValue = TestTrueString;
            drpIsBillable.DataBind();
            var rblDataCompareOperation = GetField<RadioButtonList>(RblDataCompareOperation);
            rblDataCompareOperation.Items.Add(new ListItem(DummyString, DummyId.ToString()));
            rblDataCompareOperation.SelectedIndex = 0;
            ShimDataCompareView.AllInstances.SelectForRunInt32 = (sender, dcRunId) =>
            {
                _selectForRun = true;
                return new List<DataCompareView>
                {
                    new DataCompareView {
                        DcTargetCodeId = DummyId,
                        PaymentStatusId = int.Parse(TestZero),
                        DcTargetIdUad= DummyId,
                        DcTypeCodeId = DummyId,
                        IsBillable= isBillable,
                        DcViewId = dcViewId
                    }
                };
            };
            ShimDataCompareView.AllInstances.SaveDataCompareView = (sender, dataObject) => DummyId;
            var parameters = new object[] { this, EventArgs.Empty };
            ShimDirectory.ExistsString = _ => false;
            ShimDirectory.CreateDirectoryString = _ => new ShimDirectoryInfo();
            ShimDataCompareDownloadCostDetail.AllInstances.CreateCostDetailInt32Int32StringStringStringInt32 =
                (_, _2, _3, _4, _5, _6, _7) => new List<DataCompareDownloadCostDetail>();

            // Act
            PrivatePage.Invoke(BtnDCDownloadClick, parameters);

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => _selectCodeName.ShouldBeTrue(),
                () => _selectForRun.ShouldBeTrue(),
                () => _downloadDataCompare.ShouldBeTrue(),
                () => 
                {
                    if (isBillable)
                    {
                        _saveDataCompareView.ShouldBeFalse();
                        _saveDataCompareDownload.ShouldBeTrue();
                        _getDataCompareCost.ShouldBeTrue();
                    }
                    else
                    {
                        _saveDataCompareView.ShouldBeFalse();
                        _saveDataCompareDownload.ShouldBeFalse();
                        _getDataCompareCost.ShouldBeTrue();
                    }
                });
        }

        [TestCase(TestTwo, TestTrueString)]
        [TestCase(TestTwo, TestFalseString)]
        public void BtnDCDownloadClick_PageIsValidAndIdNotExistInDataCompareView_UpdateControlValues(
            string brandId, 
            string selectedValue)
        {
            // Arrange
            var id = int.Parse(brandId);
            CreatePageShimObject(true, id);
            SetPagePrivateObject(brandId);
            var drpIsBillable = GetField<DropDownList>(DropDownListIsBillable);
            drpIsBillable.DataSource = new string[] { TestTrueString, TestFalseString };
            drpIsBillable.SelectedValue = selectedValue;
            drpIsBillable.DataBind();
            ShimDataCompareView.AllInstances.SelectForRunInt32 = (sender, dcRunId) =>
            {
                _selectForRun = true;
                return new List<DataCompareView>
                {
                    new DataCompareView {
                        DcTargetCodeId = id,
                        PaymentStatusId = id,
                        DcTargetIdUad= id,
                        DcTypeCodeId = DummyId,
                        IsBillable= bool.Parse(selectedValue),
                        DcViewId = id
                    }
                };
            };
            var rblDataCompareOperation = GetField<RadioButtonList>(RblDataCompareOperation);
            rblDataCompareOperation.Items.Add(new ListItem(DummyString, DummyId.ToString()));
            rblDataCompareOperation.SelectedIndex = 0;
            ShimDirectory.ExistsString = _ => false;
            ShimDirectory.CreateDirectoryString = _ => new ShimDirectoryInfo();
            ShimDataCompareDownloadCostDetail.AllInstances.CreateCostDetailInt32Int32StringStringStringInt32 =
                (_, _2, _3, _4, _5, _6, _7) => new List<DataCompareDownloadCostDetail>();
            ShimDataCompareView.AllInstances.SaveDataCompareView = (sender, dataObject) => DummyId;
            var parameters = new object[] { this, EventArgs.Empty };

            // Act
            PrivatePage.Invoke(BtnDCDownloadClick, parameters);

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => _selectCodeName.ShouldBeTrue(),
                () => _selectForRun.ShouldBeTrue(),
                () => _downloadDataCompare.ShouldBeTrue());
        }

        private void SetPagePrivateObject(string brandId)
        {
            GetField<HiddenField>(HiddenFiledBrandId).Value = brandId;
            GetField<HiddenField>(HiddenFiledDcTargetCodeId).Value = brandId;
            GetField<HiddenField>(HiddenFiledDataCompareProcessCode).Value = brandId;
            GetField<HiddenField>(HiddenFiledDcRunId).Value = brandId;
            GetField<HiddenField>(HfProductID).Value = brandId;
            GetField<LinkButton>(LinkNonMatchedRecords).Text = brandId;
            GetField<LinkButton>(LinkMatchedRecords).Text = brandId;
            GetField<PlaceHolder>(PlaceHolderKmStaff).Visible = true;
            GetField<PlaceHolder>(PlaceholderNotes).Visible = true;
            GetField<Label>(LabelTotalFileRecords).Text = brandId;
            var drpDataCompareSourceFile = GetField<DropDownList>(DropDownListDataCompareSourceFile);
            drpDataCompareSourceFile.DataSource = Enumerable.Range(0, 10).ToArray();
            drpDataCompareSourceFile.SelectedValue = brandId;
            drpDataCompareSourceFile.DataBind();
            var rcbProduct = GetField<RadComboBox>(RcbProduct);
            rcbProduct.Items.Add(new RadComboBoxItem(DummyString, DummyString));
            rcbProduct.SelectedIndex = 0;
        }

        private void CreatePageShimObject(bool isActive, int brandId)
        {
            _saveDataCompareDownload = false;
            _selectCodeName = false;
            _selectForRun = false;
            _getDataCompareCost = false;
            _downloadDataCompare = false;
            _saveDataCompareView = false;
            var shimSession = CreateShimECNSessionObject(isActive);
            ShimECNSession.CurrentSession = () => shimSession.Instance;
            CreateMasterPageShimObject(shimSession);
            ShimDataCompareProfile.AllInstances.GetDataCompareDataClientConnectionsStringInt32StringInt32 =
                (sender, client, processCode, dcTargetCodeId, matchType, id) => new DataTable();

            ShimFrameworkUADLookUp::ShimCode.AllInstances.SelectCodeNameEnumsCodeTypeString = (sender, codeType, codeName) =>
            {
                _selectCodeName = true;
                return new Code
                {
                    CodeId = DummyId,
                    CodeValue = TestOne
                };
            };
            ShimFilter.AllInstances.ExecuteClientConnectionsString = (sender, clientconnections, filter) => { };
            ShimCode.GetDataCompareTarget = () =>
            {
               return new List<Objects.Code> 
               { 
                   new Objects.Code { CodeName = DataCompareViewType.Consensus.ToString(), CodeID = DummyId }, 
                   new Objects.Code { CodeName = DataCompareViewType.Consensus.ToString(), CodeID = int.Parse(TestTwo) }
               }; 
            };
            ShimDataCompareView.AllInstances.SelectForRunInt32 = (sender, dcRunId) =>
            {
                _selectForRun = true;
                return new List<DataCompareView>
                {
                    new DataCompareView 
                    {
                        DcTargetCodeId = brandId,
                        PaymentStatusId = brandId
                    }
                };
            };
            ShimDataCompareView.AllInstances.GetDataCompareCostInt32Int32Int32EnumsDataCompareTypeEnumsDataCompareCost =
                (sender, userId, clientId, count, dcType, mergePurgeorDownload) =>
                {
                    _getDataCompareCost = true;
                    return 100;
                };
            ShimDataCompareDownload.AllInstances.SaveDataCompareDownload = (sender, dataCompareDownloadObject) =>
            {
                _saveDataCompareDownload = true;
                return DummyId;
            };
            ShimProductView.AllInstances.saveDataCompareViewInt32NullableOfInt32Int32Int32 = (x, y, z, v, b) =>
            {
                _saveDataCompareView = true;
                return DummyId;
            };
            ShimUtilities.DownloadDataCompareInt32DataTableString = (x, y, z) => _downloadDataCompare = true;
            ShimDataCompareView.AllInstances.DeleteInt32 = (sender, id) => true;
            ShimPage.AllInstances.IsValidGet = (sender) => true;
            ShimPage.AllInstances.ServerGet = (sender) =>
                new ShimHttpServerUtility
                {
                    MapPathString = (path) => path
                };
        }

        private static void CreateMasterPageShimObject(ShimECNSession shimSession)
        {
            ShimProductView.AllInstances.MasterGet = (x) =>
            {
                MasterPages.Site site = new ShimSite
                {
                    clientconnectionsGet = () => new KMPlatform.Object.ClientConnections
                    {
                        ClientLiveDBConnectionString = string.Empty,
                        ClientTestDBConnectionString = string.Empty
                    },
                    UserSessionGet = () => shimSession.Instance,
                    LoggedInUserGet = () => DummyId
                };
                return site;
            };
        }

        private static ShimECNSession CreateShimECNSessionObject(bool isActive)
        {
            ShimECNSession.Constructor = (instance) => { };
            var shimSession = new ShimECNSession
            {
                ClearSession = () => { },
                ClientIDGet = () => DummyId,
                UserIDGet = () => DummyId,
                BaseChannelIDGet = () => DummyId,
                ClientGroupIDGet = () => DummyId,
                CustomerIDGet = () => DummyId,
            };
            shimSession.Instance.CurrentUser = CreateUserObject(isActive, shimSession);
            return shimSession;
        }
    }
}
