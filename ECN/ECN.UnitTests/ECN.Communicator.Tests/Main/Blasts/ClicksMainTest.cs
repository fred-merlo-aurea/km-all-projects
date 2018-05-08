using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Fakes;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web;
using System.Web.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using ecn.communicator.blastsmanager;
using ecn.communicator.blastsmanager.Fakes;
using ECN.Communicator.Tests.Helpers;
using ECN_Framework_BusinessLayer.Activity.Fakes;
using ECN_Framework_BusinessLayer.Activity.Report.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Activity;
using ECN_Framework_Entities.Activity.Report;
using KM.Platform.Fakes;
using Microsoft.Reporting.WebForms;
using Microsoft.Reporting.WebForms.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using Entities = ECN_Framework_Entities.Communicator;
using ShimBusinessRSSFeed = ECN_Framework_BusinessLayer.Communicator.ContentReplacement.Fakes.ShimRSSFeed;
using StateBag = System.Web.UI.StateBag;

namespace ECN.Communicator.Tests.Main.Blasts
{
    /// <summary>
    /// Unit Tests for <see cref="clicks_main"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ClicksMainTest : BaseBlastsTest<clicks_main>
    {
        private const string ClicksbyURLReport = "Clicks by URL Report";
        private const string HelpContent = "<p><b>Clicks by URL </b><br />Lists all recepients who clicked on the URL links in your email Blast<br />Displays the time clicked, the URL link clicked.<br />Click on the email address to view the profile of that email address.";
        private const string MethodBtnDownloadViewClick = "btnDownloadView_Click";
        private const string MethodLoadGrid = "loadGrid";
        private const string MethodShowHeatMap = "ShowHeatMap";
        private const string ExistingBlastId = "1";
        private const string NewBlastId = "0";
        private const string ExistingCampaignId = "1";
        private const string NewCampaignId = "0";
        private const string DummyString = "dummyString";
        private const string DummyNumberString = "1";
        private const string Summary = "summary";
        private const string Detail = "detail";
        private const string DummyURL = "km.com";
        private string _emailBody = "<h1>KM Email</h1><a href = 'km.com'/>";
        private const int BlastId = 1;
        private const string TvSortField = "TvSortField";
        private const string TvSortFieldKey = "tvSortField";
        private const string TvSortDirection = "tvSortDirection";
        private string _reportType;
        private byte[] _reportData;
        private DataSet _groupClicksDataSet;
        private StateBag _viewState;
        private NameValueCollection _queryString;

        [Test]
        public void GetUdfData_NoException_ReturnsQueryStringValue()
        {
            // Arrange
            QueryString.Add(UDFdata, UDFdataValue);

            // Act, Assert
            testObject.getUDFData().ShouldBe(UDFdataValue);
        }

        [Test]
        public void GetUdfData_ExceptionThrown_ReturnsEmptyString()
        {
            // Arrange
            QueryString = null;

            // Act, Assert
            testObject.getUDFData().ShouldBeEmpty();
        }

        [Test]
        public void PageLoad_UserHasAccessIsPostBack_SetsMaster()
        {
            // Arrange
            ShimPage.AllInstances.IsPostBackGet = (page) => true;

            // Act
            privateObject.Invoke(PageLoad, null, new EventArgs());

            // Assert
            testObject.ShouldSatisfyAllConditions(
                () => currentMenuCode.ShouldBe(MenuCode.REPORTS),
                () => subMenu.ShouldBe(string.Empty),
                () => heading.ShouldBe(ClicksbyURLReport),
                () => helpContent.ShouldBe(HelpContent),
                () => helpTitle.ShouldBe(BlastManager));
        }

        [Test]
        public void BtnDownloadView_Click_WithSummaryViewAndExistingBlast_ReportIsDownloaded()
        {
            // Arrange
            Initialize(MethodBtnDownloadViewClick);
            SetField(testObject, "ddlDownloadView", GetDropDownForDownloadView(Summary));
            _queryString.Add("BlastID", ExistingBlastId);
            var reportDownloaded = false;
            ShimHttpResponse.AllInstances.BinaryWriteByteArray = (x, y) =>
            {
                reportDownloaded = true;
            };

            // Act
            privateObject.Invoke(MethodBtnDownloadViewClick, null, new EventArgs());

            // Assert
            reportDownloaded.ShouldBeTrue();
        }

        [Test]
        public void BtnDownloadView_Click_WithSummaryViewAndNewBlast_ReportIsDownloaded()
        {
            // Arrange
            Initialize(MethodBtnDownloadViewClick);
            SetField(testObject, "ddlDownloadView", GetDropDownForDownloadView(Summary));
            _queryString.Add("BlastID", NewBlastId);
            _queryString.Add("CampaignItemID", NewCampaignId);
            var reportDownloaded = false;
            ShimHttpResponse.AllInstances.BinaryWriteByteArray = (x, y) =>
            {
                reportDownloaded = true;
            };

            // Act
            privateObject.Invoke(MethodBtnDownloadViewClick, null, new EventArgs());

            // Assert
            reportDownloaded.ShouldBeTrue();
        }

        [Test]
        public void BtnDownloadView_Click_WithDetailViewAndExistingBlast_ReportIsDownloaded()
        {
            // Arrange
            Initialize(MethodBtnDownloadViewClick);
            SetField(testObject, "ddlDownloadView", GetDropDownForDownloadView(Detail));
            _queryString.Add("BlastID", ExistingBlastId);
            _queryString.Add("CampaignItemID", ExistingCampaignId);
            var reportDownloaded = false;
            ShimHttpResponse.AllInstances.BinaryWriteByteArray = (x, y) =>
            {
                reportDownloaded = true;
            };

            // Act
            privateObject.Invoke(MethodBtnDownloadViewClick, null, new EventArgs());

            // Assert
            reportDownloaded.ShouldBeTrue();
        }

        [Test]
        public void BtnDownloadView_Click_WithDetailViewAndNewBlast_ReportIsDownloaded()
        {
            // Arrange
            Initialize(MethodBtnDownloadViewClick);
            SetField(testObject, "ddlDownloadView", GetDropDownForDownloadView(Detail));
            _queryString.Add("BlastID", NewBlastId);
            _queryString.Add("CampaignItemID", NewCampaignId);
            var reportDownloaded = false;
            ShimHttpResponse.AllInstances.BinaryWriteByteArray = (x, y) =>
            {
                reportDownloaded = true;
            };

            // Act
            privateObject.Invoke(MethodBtnDownloadViewClick, null, new EventArgs());

            // Assert
            reportDownloaded.ShouldBeTrue();
        }

        [TestCase("topclicks")]
        [TestCase("topvisitors")]
        [TestCase("allclicks")]
        [TestCase("heatmap")]
        public void LoadGrid_WithExistingBlast_GridIsLoadedWithData(string reportType)
        {
            // Arrange
            Initialize(MethodBtnDownloadViewClick);
            Initialize(MethodLoadGrid);
            SetField(testObject, "ddlDownloadView", GetDropDownForDownloadView(Detail));
            _queryString.Add("BlastID", ExistingBlastId);
            _queryString.Add("CampaignItemID", ExistingCampaignId);
            _reportType = reportType;
            var gridLoaded = false;
            ShimBaseDataList.AllInstances.DataBind = (x) =>
            {
                gridLoaded = true;
            };
            Shimclicks_main.AllInstances.ShowHeatMapInt32Boolean = (x, y, z) =>
            {
                gridLoaded = true;
            };

            // Act
            privateObject.Invoke(MethodLoadGrid, _reportType);

            // Assert
            gridLoaded.ShouldBeTrue();
        }

        [TestCase("allclicks", ExistingBlastId, ExistingCampaignId)]
        [TestCase("allclicks", NewBlastId, NewCampaignId)]
        public void LoadGrid_WithInvalidStartEndDate_ErrorIsDisplayed(string reportType, string blastId, string campaignId)
        {
            // Arrange
            Initialize(MethodBtnDownloadViewClick);
            Initialize(MethodLoadGrid);
            SetField(testObject, "ddlDownloadView", GetDropDownForDownloadView(Detail));
            _queryString.Add("BlastID", blastId);
            _queryString.Add("CampaignItemID", campaignId);
            _reportType = reportType;
            SetField(testObject, "txtstartDate", new TextBox());
            SetField(testObject, "txtendDate", new TextBox());

            // Act
            privateObject.Invoke(MethodLoadGrid, _reportType);

            // Assert
            var dateError = (GetField(testObject, "lblDateRangeError") as Label).Visible;
            dateError.ShouldBeTrue();
        }

        [TestCase("allclicks", ExistingBlastId, ExistingCampaignId)]
        [TestCase("allclicks", NewBlastId, NewCampaignId)]
        public void LoadGrid_WithClickTypesSetToAll_AllClickAreSelected(string reportType, string blastId, string campaignId)
        {
            // Arrange
            Initialize(MethodBtnDownloadViewClick);
            Initialize(MethodLoadGrid);
            SetField(testObject, "ddlDownloadView", GetDropDownForDownloadView(Detail));
            _queryString.Add("BlastID", blastId);
            _queryString.Add("CampaignItemID", campaignId);
            _reportType = reportType;
            var allClicksSelected = false;
            SetField(testObject, "ddlClicksType", new DropDownList
            {
                Items =
                {
                    new ListItem
                    {
                        Selected = true,
                        Value = "all"
                    }
                }
            });
            ShimBlast.GetBlastGroupClicksDataNullableOfInt32NullableOfInt32StringStringStringStringInt32Int32StringStringStringStringBoolean = (a, b, c, d, e, f, g, h, i, j, k, l, m) =>
            {
                allClicksSelected = true;
                return _groupClicksDataSet;
            };

            // Act
            privateObject.Invoke(MethodLoadGrid, _reportType);

            // Assert
            allClicksSelected.ShouldBeTrue();
        }

        [TestCase("topclicks")]
        [TestCase("topvisitors")]
        [TestCase("allclicks")]
        [TestCase("heatmap")]
        public void LoadGrid_WithNewBlast_GridIsLoadedWithData(string reportType)
        {
            // Arrange
            Initialize(MethodBtnDownloadViewClick);
            Initialize(MethodLoadGrid);
            SetField(testObject, "ddlDownloadView", GetDropDownForDownloadView(Detail));
            _queryString.Add("BlastID", NewBlastId);
            _queryString.Add("CampaignItemID", NewCampaignId);
            _reportType = reportType;
            var gridLoaded = false;
            ShimBaseDataList.AllInstances.DataBind = (x) =>
            {
                gridLoaded = true;
            };
            Shimclicks_main.AllInstances.ShowHeatMapInt32Boolean = (x, y, z) =>
            {
                gridLoaded = true;
            };

            // Act
            privateObject.Invoke(MethodLoadGrid, _reportType);

            // Assert
            gridLoaded.ShouldBeTrue();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ShowHeatMap_WhenBlastLayoutIsNotNull_PreviewIsShownAndContainsData(bool isBlast)
        {
            // Arrange
            Initialize(MethodBtnDownloadViewClick);
            Initialize(MethodLoadGrid);
            Initialize(MethodShowHeatMap);
            _queryString.Add("BlastID", NewBlastId);
            _queryString.Add("CampaignItemID", NewCampaignId);

            // Act
            privateObject.Invoke(MethodShowHeatMap, BlastId, isBlast);

            // Assert
            var preview = (GetField(testObject, "LabelPreview") as Label).Text;
            testObject.ShouldSatisfyAllConditions(
                () => preview.ShouldNotBeNullOrWhiteSpace(),
                () => preview.ShouldBe(_emailBody));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ShowHeatMap_WhenBlastLayoutIsNotNullAndBlastClickMatchUrl_PreviewContainsClickValue(bool isBlast)
        {
            // Arrange
            Initialize(MethodBtnDownloadViewClick);
            Initialize(MethodLoadGrid);
            Initialize(MethodShowHeatMap);
            _queryString.Add("BlastID", NewBlastId);
            _queryString.Add("CampaignItemID", NewCampaignId);
            var blastActivityClicks = CreateInstance(typeof(BlastActivityClicks));
            blastActivityClicks.URL = DummyURL;
            var blastActivityClicksList = new List<BlastActivityClicks>
            {
                blastActivityClicks
            };
            ShimBlastActivityClicks.GetByBlastIDInt32 = (x) => blastActivityClicksList;
            var clickValue = "clickValue =";

            // Act
            privateObject.Invoke(MethodShowHeatMap, BlastId, isBlast);

            // Assert
            var preview = (GetField(testObject, "LabelPreview") as Label).Text;
            testObject.ShouldSatisfyAllConditions(
                () => preview.ShouldNotBeNullOrWhiteSpace(),
                () => preview.Contains(clickValue));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ShowHeatMap_WhenEmailBodyContainsEcnId_PreviewContainsClickValue(bool isBlast)
        {
            // Arrange
            Initialize(MethodBtnDownloadViewClick);
            Initialize(MethodLoadGrid);
            Initialize(MethodShowHeatMap);
            _queryString.Add("BlastID", NewBlastId);
            _queryString.Add("CampaignItemID", NewCampaignId);
            var uniqueLink = CreateInstance(typeof(Entities.UniqueLink));
            var uniqueLinkList = new List<Entities.UniqueLink>
            {
                uniqueLink
            };
            ShimUniqueLink.GetByBlastIDInt32 = (x) => uniqueLinkList;
            _emailBody = "<h1>KM Email</h1><ecn_id='10' href = 'km.com'/>";
            var clickValue = "clickValue =";

            // Act
            privateObject.Invoke(MethodShowHeatMap, BlastId, isBlast);

            // Assert
            var preview = (GetField(testObject, "LabelPreview") as Label).Text;
            testObject.ShouldSatisfyAllConditions(
                () => preview.ShouldNotBeNullOrWhiteSpace(),
                () => preview.Contains(clickValue));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ShowHeatMap_WhenBlastLayoutIsNull_ErrorIsShown(bool isBlast)
        {
            // Arrange
            Initialize(MethodBtnDownloadViewClick);
            Initialize(MethodLoadGrid);
            Initialize(MethodShowHeatMap);
            _queryString.Add("BlastID", NewBlastId);
            _queryString.Add("CampaignItemID", NewCampaignId);
            var errorMessage = "Associated Layout or Template has been removed.";
            var blast = CreateInstance(typeof(Entities.BlastRegular));
            blast.Layout = null;
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (x, y) => blast;

            // Act
            privateObject.Invoke(MethodShowHeatMap, BlastId, isBlast);

            // Assert
            var preview = (GetField(testObject, "LabelPreview") as Label).Text;
            testObject.ShouldSatisfyAllConditions(
                () => preview.ShouldNotBeNullOrWhiteSpace(),
                () => preview.ShouldBe(errorMessage));
        }

        [TestCase(true, "ASC")]
        [TestCase(true, "DESC")]
        [TestCase(false, "")]
        public void EmailsGridSort_SortField_SetsViewState(bool isSortField, string sortDescription)
        {
            // Arrange
            Shimclicks_main.AllInstances.loadGridString = (obj, reportType) => { };

            using (var testObject1 = new clicks_main())
            {
                ShimDataGridSortCommandEventArgs.AllInstances.SortExpressionGet = (obj) => isSortField
                                                                                               ? TvSortField
                                                                                               : string.Empty;
                privateObject = new PrivateObject(testObject1);
                var viewState = privateObject.GetProperty("ViewState") as StateBag;

                viewState[TvSortFieldKey] = TvSortField;
                viewState[TvSortDirection] = sortDescription;

                // Act
                testObject1.EmailsGrid_Sort(null, new ShimDataGridSortCommandEventArgs().Instance);

                // Assert
                testObject1.ShouldSatisfyAllConditions(
                    () => viewState[TvSortDirection]
                        .ShouldBe(
                            sortDescription == "ASC"
                                ? "DESC"
                                : "ASC"),
                    () => viewState[TvSortFieldKey]
                        .ShouldBe(
                            isSortField
                                ? TvSortField
                                : string.Empty));
            }
        }

        private void Initialize(string method = "Default")
        {
            InitializeAllControls(testObject);
            _queryString = new NameValueCollection();
            if (method == MethodLoadGrid)
            {
                SetField(testObject, "txtstartDate", new TextBox
                {
                    Text = "1/30/2018"
                });
                SetField(testObject, "txtendDate", new TextBox
                {
                    Text = "3/30/2018"
                });
                _reportType = string.Empty;
                _viewState = new StateBag();
                _viewState["tcSortField"] = string.Empty;
                _viewState["tcSortDirection"] = string.Empty;
                _viewState["acSortField"] = string.Empty;
                _viewState["acSortDirection"] = string.Empty;
                _viewState[TvSortFieldKey] = string.Empty;
                _viewState[TvSortDirection] = string.Empty;
                _viewState["_pageSize"] = 1;
                _viewState["_currentIndex"] = 1;
            }
            CreateShims(method);
        }

        private void CreateShims(string method = "Default")
        {
            var blastClickSummaryList = new List<BlastClickSummary>
                {
                    CreateInstance(typeof(BlastClickSummary))
                };
            var sampleBlast = CreateInstance(typeof(Entities.BlastRegular));
            sampleBlast.TestBlast = "N";
            var blastAbstractList = new List<Entities.BlastAbstract>
                {
                    sampleBlast
                };
            var blastClickDetailList = new List<BlastClickDetail>
                {
                    CreateInstance(typeof(BlastClickDetail))
                };
            var groupClicksTable = new DataTable();
            groupClicksTable.Columns.Add(DummyString);
            groupClicksTable.Rows.Add(DummyNumberString);
            var groupClicksTable2 = new DataTable();
            groupClicksTable2.Columns.Add(DummyString);
            groupClicksTable2.Rows.Add(DummyString);
            _groupClicksDataSet = new DataSet();
            _groupClicksDataSet.Tables.Add(groupClicksTable);
            _groupClicksDataSet.Tables.Add(groupClicksTable2);
            if (method == MethodBtnDownloadViewClick)
            {
                ShimPage.AllInstances.IsPostBackGet = (page) => true;
                ShimHttpRequest.AllInstances.QueryStringGet = (h) => _queryString;
                ShimBlastClickSummary.GetInt32 = (x) => blastClickSummaryList;
                ConfigurationManager.AppSettings["ReportPath"] = DummyString;
                ShimLocalReport.AllInstances.SetParametersIEnumerableOfReportParameter = (x, y) => { };
                _reportData = new byte[1];
                ShimLocalReport
                    .AllInstances
                    .RenderStringStringPageCountModeStringOutStringOutStringOutStringArrayOutWarningArrayOut = (
                    LocalReport format,
                    string renderers,
                    string info,
                    PageCountMode mode,
                    out string type,
                    out string encoding,
                    out string extension,
                    out string[] streams,
                    out Warning[] warnings) =>
                    {
                        type = string.Empty;
                        encoding = string.Empty;
                        extension = string.Empty;
                        streams = new string[0];
                        warnings = new Warning[0];
                        return _reportData;
                    };
                ShimHttpResponse.AllInstances.BinaryWriteByteArray = (x, y) => { };
                ShimHttpResponse.AllInstances.End = (x) => { };
                ShimBlast.GetByCampaignItemID_NoAccessCheckInt32Boolean = (x, y) => blastAbstractList;
                ShimBlastClickDetail.GetInt32 = (x) => blastClickDetailList;
            }
            if (method == MethodLoadGrid)
            {
                ShimControl.AllInstances.ViewStateGet = (x) => _viewState;
                ShimDataView.AllInstances.SortSetString = (x, y) => { };
                ShimDataView.AllInstances.SortSetString = (x, y) => { };
                ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (a, b, c, d) => true;
                ShimBlast.GetBlastGroupClicksDataNullableOfInt32NullableOfInt32StringStringStringStringInt32Int32StringStringStringStringBoolean = (a, b, c, d, e, f, g, h, i, j, k, l, m) => _groupClicksDataSet;
            }
            if (method == MethodShowHeatMap)
            {
                var campaignItemBlast = CreateInstance(typeof(Entities.CampaignItemBlast));
                var campaignItemBlastList = new List<Entities.CampaignItemBlast>
                {
                    campaignItemBlast
                };
                var blast = CreateInstance(typeof(Entities.BlastRegular));
                var layout = CreateInstance(typeof(Entities.Layout));
                layout.Template = CreateInstance(typeof(Entities.Template));
                blast.Layout = layout;
                var blastActivityClicks = CreateInstance(typeof(BlastActivityClicks));
                var blastActivityClicksList = new List<BlastActivityClicks>
                {
                    blastActivityClicks
                };
                var uniqueLink = CreateInstance(typeof(Entities.UniqueLink));
                var uniqueLinkList = new List<Entities.UniqueLink>
                {
                    uniqueLink
                };
                ShimCampaignItemBlast.GetByCampaignItemID_NoAccessCheckInt32Boolean = (x, y) => campaignItemBlastList;
                ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (x, y) => blast;
                ShimLayout.EmailBody_NoAccessCheckStringStringStringNullableOfInt32NullableOfInt32NullableOfInt32NullableOfInt32NullableOfInt32NullableOfInt32NullableOfInt32NullableOfInt32NullableOfInt32EnumsContentTypeCodeBooleanNullableOfInt32NullableOfInt32NullableOfInt32 = (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q) => _emailBody;
                ShimBlastActivityClicks.GetByBlastIDInt32 = (x) => blastActivityClicksList;
                ShimUniqueLink.GetByBlastIDInt32 = (x) => uniqueLinkList;
                ShimBusinessRSSFeed.ReplaceStringRefInt32BooleanNullableOfInt32 = (ref string content, int a, bool b, int? c) => { };
                ShimECNSession.AllInstances.RefreshSession = (item) => { };
                ShimECNSession.AllInstances.ClearSession = (itme) => { };
            }
        }

        private DropDownList GetDropDownForDownloadView(string textValue)
        {
            var downloadView = new DropDownList
            {
                Items =
                    {
                        new ListItem
                        {
                            Selected = true,
                            Text = textValue
                        }
                    }
            };
            return downloadView;
        }
        private dynamic CallMethod(Type type, string methodName, object[] parametersValues, object instance = null)
        {
            return ReflectionHelper.CallMethod(type, methodName, parametersValues, instance);
        }

        private dynamic CreateInstance(Type type)
        {
            return ReflectionHelper.CreateInstance(type);
        }

        private dynamic GetField(dynamic obj, string fieldName)
        {
            return ReflectionHelper.GetField(obj, fieldName);
        }

        private void SetField(dynamic obj, string fieldName, dynamic value)
        {
            ReflectionHelper.SetField(obj, fieldName, value);
        }

        private void SetProperty(dynamic obj, string fieldName, dynamic value)
        {
            ReflectionHelper.SetProperty(obj, fieldName, value);
        }

        private dynamic GetProperty(dynamic obj, string fieldName)
        {
            return ReflectionHelper.GetPropertyValue(obj, fieldName);
        }

        private void SetSessionVariable(string name, object value)
        {
            HttpContext.Current.Session.Add(name, value);
        }

        private void InitializeAllControls(object page)
        {
            var fields = page.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
                if (field.GetValue(page) == null)
                {
                    var constructor = field.FieldType.GetConstructor(new Type[0]);
                    if (constructor != null)
                    {
                        var obj = constructor.Invoke(new object[0]);
                        if (obj != null)
                        {
                            field.SetValue(page, obj);
                            TryLinkFieldWithPage(obj, page);
                        }
                    }
                }
            }
        }

        private void TryLinkFieldWithPage(object field, object page)
        {
            if (page is Page)
            {
                var fieldType = field.GetType().GetField("_page", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

                if (fieldType != null)
                {
                    try
                    {
                        fieldType.SetValue(field, page);
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError($"Unable to set value as :{ex}");
                    }
                }
            }
        }
    }
}
