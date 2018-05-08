using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks.Fakes;
using System.Web.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using KM.Integration.Marketo.Process.Fakes;
using KM.Integration.OAuth2;
using KM.Integration.OAuth2.Fakes;
using KM.Platform.Fakes;
using KMPlatform.Object.Fakes;
using KMPS.MD.Main;
using KMPS.MD.Main.Fakes;
using KMPS.MD.MasterPages.Fakes;
using KMPS.MD.Objects;
using KMPS.MD.Objects.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using TestCommonHelpers;
using static KMPlatform.Enums;
using FrameworkUADShim = FrameworkUAD.BusinessLogic.Fakes;

namespace KMPS.MD.Tests.Main
{
    /// <summary>
    /// Unit test for <see cref="FilterExport"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class FilterExportTest : BasePageTests
    {
        private const string ButtonSaveClick = "btnSave_Click";
        private const string DefaultText = "Unit Test";
        private const string ListSelectedFilters = "lstSelectedFilters";
        private const string ListSuppressedFilters = "lstSuppressedFilters";
        private const string RadioButtonListSelectedFiltersOperation = "rblSelectedFiltersOperation";
        private const string RadioButtonListSuppressedFiltersOperation = "rblSuppressedFiltersOperation";
        private const string GridViewHttpPost = "gvHttpPost";
        private const string Marketo = "Marketo";
        private const string HiddenFiledPubID = "hfPubID";
        private const string HiddenFiledViewType = "hfViewType";
        private const string HiddenFiledBrandID = "hfBrandID";
        private const string PanleFiltersSelect = "PlFiltersSelect";
        private const string TextBoxRecurringStartDate = "txtRecurringStartDate";
        private const string TextBoxMonth = "txtMonth";
        private const string TextBoxStartDate = "txtStartDate";
        private const string CheckBoxLastDay = "cbLastDay";
        private const string CheckBoxShowHeader = "cbShowHeader";
        private const string HiddenFielddFilterScheduleID = "hdFilterScheduleID";
        private const string TextBoxExportName = "txtExportName";
        private const string TextBoxExportNotes = "txtExportNotes";
        private const string TextBoxEmailAddress = "txtEmailAddress";
        private const string HiddenFieldGroupID = "hfGroupID";
        private const string HiddenFieldFilterID = "hdFilterID";
        private const string DefaultEmialAddres = "admin@unittest.com";
        private const string DropDownScheduleTypeScheduleType = "ddlScheduleType";
        private const string CheckBoxListWeekDays = "cbDays";
        private const string DropDownListStartTime = "ddlStartTime";
        private const string DropDownListRecurrence = "ddlRecurrence";
        private const string DropDownListRecurringStartTime = "ddlRecurringStartTime";
        private const string ListBoxSelectedFields = "lstSelectedFields";
        private const string DropDownListSiteType = "ddlSiteType";
        private const string DropDownListExport = "ddlExport";
        private const string DropDownListDownloadTemplate = "drpDownloadTemplate";
        private const string GridViewFilterSegmentationCounts = "grdFilterSegmentationCounts";
        private const string TestOne = "1";
        private const string Test = "Test";
        private const string FilterObject = "fc";
        private const string ExportTypeFTP = "ExportTypeFTP";
        private const string ExportTypeECN = "ExportTypeECN";
        private const string Sftp = "sftp";
        private const string SftpURL = "sftp://";
        private const string Ftps = "ftps";
        private const string FtpsURL = "ftps://";
        private const string Ftp = "ftp";
        private const string FtpURL = "ftp://";
        private const string MethodLoadFilter = "LoadFilterScheduleDetails";
        private const string TextFileName = "txtFileName";
        private const string Error = "divError";
        private const string ErrorMessage = "lblErrorMessage";
        private const string FileName = "FileName";
        private const string Name = "name";
        private const string Notes = "notes";
        private const string Value = "Value";
        private const string Operation = "opp";
        private const string CustomValue = "CustomValue";
        private const string Union = "Union";
        private const string Field = "field";
        private const int Ten = 10;
        private const int One = 1;
        private const int Two = 2;
        private const int Zero = 0;

        private FilterExport _filterExport;
        private PrivateObject _privateObject;
        private Filters _filters;
        private string _url = string.Empty;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _filterExport = new FilterExport();
            _privateObject = new PrivateObject(_filterExport);
            InitializePage(_filterExport);
        }

        [TestCase(1, 1, true, "1", true, true)]
        [TestCase(1, 2, true, "1", true, true)]
        [TestCase(1, 3, true, "1", true, true)]
        [TestCase(1, 2, false, "0", false, false)]
        [TestCase(1, 3, true, "1", true, false)]
        [TestCase(0, 1, true, "1", true, true)]
        [TestCase(0, 2, true, "1", true, true)]
        [TestCase(0, 3, true, "1", true, true)]
        [TestCase(0, 2, false, "0", false, false)]
        [TestCase(0, 3, true, "1", true, false)]
        [TestCase(2, 2, true, "0", false, true)]
        [TestCase(3, 3, false, "1", true, true)]
        [TestCase(3, 1, true, "1", true, true)]
        public void ButtonSaveClick_UserHasAccess_UpdateControlValueToDataBase(
            int exportTypeSelectedIndex,
            int siteTypeSelectedIndex,
            bool scheduleTypeSelected,
            string filterScheduleId,
            bool filtersSelect,
            bool recurrence)
        {
            // Arrange
            var parameters = new object[] { this, EventArgs.Empty };
            CreatePageShimObject(true);
            BindxportDropDown(exportTypeSelectedIndex);
            BindSiteTypeDropDown(siteTypeSelectedIndex);
            var lstSelectedDataSource = CreateSelectedFieldsDataSource();
            BindSelectedFieldsDropDown(lstSelectedDataSource);
            BindListBox(ListSelectedFilters, 0, 10);
            BindListBox(ListSuppressedFilters, 10, 20);
            BindRadioButtonList(RadioButtonListSelectedFiltersOperation, 0, 5, 5);
            BindRadioButtonList(RadioButtonListSuppressedFiltersOperation, 0, 5, 5);
            BingddlRecurrenceDropDown(recurrence);
            BindRecurringStartTimeDropDown();
            BindStartTimeDropDown();
            BindWeekDaysDropDown();
            BindScheduleTypeDropdown(scheduleTypeSelected);
            BindPageInputControl(filterScheduleId, filtersSelect);
            CommonPageFakeObject(lstSelectedDataSource);
            CreateGridFilterSegmentationCounts();
            var marketo = new KMPS.MD.Controls.Marketo();
            InitializeUserControl(marketo);
            var gvHttpPost = (GridView)marketo.FindControl(GridViewHttpPost);
            _privateObject.SetFieldOrProperty(Marketo, marketo);
            // Act
            _privateObject.Invoke(ButtonSaveClick, parameters);

            // Assert
            _url.ShouldNotBeNullOrEmpty();
        }

        [Test]
        [TestCase(Marketo, Sftp)]
        [TestCase(ExportTypeFTP, Sftp)]
        [TestCase(ExportTypeFTP, Ftps)]
        [TestCase(ExportTypeFTP, Ftp)]
        [TestCase(ExportTypeECN, Ftp)]
        public void LoadFilterScheduleDetails_ForDifferentExportTypes_ShouldLoadDetails(string type, string param)
        {
            // Arrange
            SetUpFilter(type, param);
            var MdFilter = new MDFilter();
            var pubIds = new List<int>();
            CreateGridFilterSegmentationCounts();
            pubIds.Add(Ten);

            // Act
            _privateObject.Invoke(MethodLoadFilter, MdFilter, pubIds);

            // Assert
            _filterExport.ShouldSatisfyAllConditions(
                () => GetField<TextBox>(TextFileName).Text.ShouldBe(FileName),
                () => GetField<CheckBox>(CheckBoxShowHeader).Checked.ShouldBeTrue(),
                () => GetField<CheckBox>(CheckBoxLastDay).Checked.ShouldBeTrue());
        }

        [TestCase(Marketo, Ftp)]
        public void LoadFilterScheduleDetails_ForMarketoWithNoUserAccess_ShouldLoadDetails(string type, string param)
        {
            // Arrange
            SetUpFilter(type, param);
            var MdFilter = new MDFilter();
            var pubIds = new List<int>();
            CreateGridFilterSegmentationCounts();
            pubIds.Add(Ten);

            // Act
            _privateObject.Invoke(MethodLoadFilter, MdFilter, pubIds);

            // Assert
            _filterExport.ShouldSatisfyAllConditions(
                () => GetField<TextBox>(TextFileName).Text.ShouldBe(string.Empty),
                () => GetField<HtmlGenericControl>(Error).Visible.ShouldBeTrue(),
                () => GetField<Label>(ErrorMessage).Text.ShouldNotBeEmpty());
        }

        private void SetUpFilter(string type, string param)
        {
            ShimFilterExport.AllInstances.FilterScheduleIDGet = (x) => Ten;
            var groupId = new List<int>();
            groupId.Add(Ten);
            if (type.Equals(Marketo) && param.Equals(Sftp))
            {
                var filterSchedule = new FilterSchedule()
                {
                    ExportName = Name,
                    ExportNotes = Notes,
                    ExportTypeID = Enums.ExportType.Marketo,
                    GroupID = Ten,
                    FilterGroupID_Selected = groupId,
                    FilterGroupID_Suppressed = groupId,
                    SelectedOperation = Operation,
                    SuppressedOperation = Operation
                };
                ShimFilterSchedule.AllInstances.IsRecurringGet = (x) => true;
                ShimFilterSchedule.GetByIDClientConnectionsInt32 = (x, y) => filterSchedule;
                ShimFilterSchedule.AllInstances.FilterGroupID_SelectedGet = (x) => groupId;
                CreateFiltersObject(true);
                ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (_, __, ___, ____) => true;
            }
            else if(type.Equals(Marketo) && param.Equals(Ftp))
            {
                var filterSchedule = new FilterSchedule()
                {
                    ExportName = Name,
                    ExportNotes = Notes,
                    ExportTypeID = Enums.ExportType.Marketo,
                    GroupID = Ten,
                    FilterGroupID_Selected = groupId,
                    FilterGroupID_Suppressed = groupId,
                    SelectedOperation = Operation,
                    SuppressedOperation = Operation
                };
                ShimFilterSchedule.GetByIDClientConnectionsInt32 = (x, y) => filterSchedule;
                ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (_, __, ___, ____) => false;
            }
            else if(type.Equals(ExportTypeFTP) && param.Equals(Sftp))
            {
                var filterSchedule = new FilterSchedule()
                {
                    ExportName = Name,
                    ExportNotes = Notes,
                    ExportTypeID = Enums.ExportType.FTP,
                    GroupID = Ten,
                    Server = SftpURL,
                    FilterGroupID_Selected = groupId,
                    FilterGroupID_Suppressed = groupId,
                    SelectedOperation = Operation,
                    SuppressedOperation = Operation
                };
                ShimFilterSchedule.AllInstances.IsRecurringGet = (x) => true;
                ShimFilterSchedule.GetByIDClientConnectionsInt32 = (x, y) => filterSchedule;
                ShimFilterSchedule.AllInstances.FilterGroupID_SelectedGet = (x) => groupId;
                CreateFiltersObject(true);
                BindxportDropDown(Zero);
                GetField<DropDownList>(DropDownListExport).SelectedValue = Enums.ExportType.FTP.ToString();
            }
            else if (type.Equals(ExportTypeFTP) && param.Equals(Ftps))
            {
                var filterSchedule = new FilterSchedule()
                {
                    ExportName = Name,
                    ExportNotes = Notes,
                    ExportTypeID = Enums.ExportType.FTP,
                    GroupID = Ten,
                    Server = FtpsURL,
                    FilterGroupID_Selected = groupId,
                    FilterGroupID_Suppressed = groupId,
                    SelectedOperation = Operation,
                    SuppressedOperation = Operation
                };
                CreateFiltersObject(true);
                ShimFilterSchedule.AllInstances.IsRecurringGet = (x) => true;
                BindxportDropDown(Zero);
                ShimFilterSchedule.GetByIDClientConnectionsInt32 = (x, y) => filterSchedule;
                ShimFilterSchedule.AllInstances.FilterGroupID_SelectedGet = (x) => groupId;
                GetField<DropDownList>(DropDownListExport).SelectedValue = Enums.ExportType.FTP.ToString();
            }
            else if (type.Equals(ExportTypeFTP) && param.Equals(Ftp))
            {
                var filterSchedule = new FilterSchedule()
                {
                    ExportName = Name,
                    ExportNotes = Notes,
                    ExportTypeID = Enums.ExportType.FTP,
                    GroupID = Ten,
                    Server = FtpURL,
                    FilterGroupID_Selected = groupId,
                    FilterGroupID_Suppressed = groupId,
                    SelectedOperation = Operation,
                    SuppressedOperation = Operation
                };
                CreateFiltersObject(true);
                ShimFilterSchedule.AllInstances.IsRecurringGet = (x) => false;
                ShimFilterSchedule.GetByIDClientConnectionsInt32 = (x, y) => filterSchedule;
                BindxportDropDown(Zero);
                GetField<DropDownList>(DropDownListExport).SelectedValue = Enums.ExportType.FTP.ToString();
            }
            else if (type.Equals(ExportTypeECN) && param.Equals(Ftp))
            {
                var filterSchedule = new FilterSchedule()
                {
                    ExportName = Name,
                    ExportNotes = Notes,
                    ExportTypeID = Enums.ExportType.ECN,
                    GroupID = Ten,
                    Server = SftpURL,
                    FilterGroupID_Selected = groupId,
                    FilterGroupID_Suppressed = groupId,
                    SelectedOperation = Operation,
                    SuppressedOperation = Operation
                };
                CreateFiltersObject(false);
                ShimFilterSchedule.AllInstances.IsRecurringGet = (x) => true;
                ShimFilterSchedule.AllInstances.FilterGroupID_SelectedGet = (x) => groupId;
                ShimFilterSchedule.GetByIDClientConnectionsInt32 = (x, y) => filterSchedule;
                BindxportDropDown(Zero);
                GetField<DropDownList>(DropDownListExport).SelectedValue = Enums.ExportType.ECN.ToString();
                ShimGroup.GetByGroupIDInt32User = (x, y) => new Group()
                {
                    GroupName = Name
                };
            }
            var selectedHiddenValue = string.Join(", ", Enumerable.Range(One, Ten).ToList());
            ShimFilterExport.AllInstances.FilterSegmentationIDGet = (x) => Ten;
            ShimControl.AllInstances.FindControlString = (sender, controlId) =>
            {
                return GetControlById(controlId, selectedHiddenValue);
            };
            var filterScheduleIntegration1 = new FilterScheduleIntegration()
            {
                IntegrationParamName = Enums.Marketo.BaseURL.ToString(),
                IntegrationParamValue = Value
            };
            var filterScheduleIntegration2 = new FilterScheduleIntegration()
            {
                IntegrationParamName = Enums.Marketo.ClientID.ToString(),
                IntegrationParamValue = Value
            };
            var filterScheduleIntegration3 = new FilterScheduleIntegration()
            {
                IntegrationParamName = Enums.Marketo.ClientSecret.ToString(),
                IntegrationParamValue = Value
            };
            var filterScheduleIntegration4 = new FilterScheduleIntegration()
            {
                IntegrationParamName = Enums.Marketo.Partition.ToString(),
                IntegrationParamValue = Value
            };
            var filterScheduleIntegrationList = new List<FilterScheduleIntegration>();
            filterScheduleIntegrationList.Add(filterScheduleIntegration1);
            filterScheduleIntegrationList.Add(filterScheduleIntegration2);
            filterScheduleIntegrationList.Add(filterScheduleIntegration3);
            filterScheduleIntegrationList.Add(filterScheduleIntegration4);
            ShimFilterScheduleIntegration.getByFilterScheduleIDClientConnectionsInt32 = (x, y) => filterScheduleIntegrationList;
            ShimAuthentication.AllInstances.getToken = (x) => new Token();
            var marketoListResponse = new KM.Integration.Marketo.MarketoListResponse();
            var marketoListResponseList = new List<KM.Integration.Marketo.MarketoListResponse>();
            marketoListResponseList.Add(marketoListResponse);
            ShimMarketoRestAPIProcess.AllInstances.GetMarketoListsTokenStringArrayStringArrayInt32String = 
                (x, y, z, v, m, n) => marketoListResponseList;
            ShimUtilities.GetExportFieldsClientConnectionsEnumsViewTypeInt32IListOfInt32EnumsExportTypeInt32EnumsExportFieldTypeBoolean = 
                (x, y, z, c, v, b, n, m) => {
                    return new Dictionary<string, string>
                    {
                        {CustomValue, Value}
                    };
                };
            var filterExportField = new FilterExportField()
            {
                MappingField = Field,
                ExportColumn = CustomValue,
                CustomValue = Value
            };
            var filterExportFieldList = new List<FilterExportField>();
            filterExportFieldList.Add(filterExportField);
            ShimFilterExportField.getByFilterScheduleIDClientConnectionsInt32 = (x, y) => filterExportFieldList;
            ShimFilterSchedule.AllInstances.SelectedOperationGet = (x) => Union;
            ShimFilterSchedule.AllInstances.SuppressedOperationGet = (x) => Union;
            ShimFilterSchedule.AllInstances.FilterSegmentationIDGet = (x) => Ten;
            ShimFilterSchedule.AllInstances.FilterGroupID_SuppressedGet = (x) => groupId;
            ShimFilterSchedule.AllInstances.SelectedOperationGet = (x) => Operation;
            ShimFilterSchedule.AllInstances.SuppressedOperationGet = (x) => Operation;
            var filterSegmentationGroup = new FrameworkUAD.Entity.FilterSegmentationGroup()
            {
                FilterGroupID_Selected = groupId,
                FilterGroupID_Suppressed = groupId,
                SelectedOperation = Operation,
                SuppressedOperation = Operation
            };
            var filterList = new List<FrameworkUAD.Entity.FilterSegmentationGroup>();
            filterList.Add(filterSegmentationGroup);
            FrameworkUADShim.ShimFilterSegmentationGroup.AllInstances.SelectByFilterSegmentationIDInt32ClientConnections =
                (x, y, z) => filterList;
            BindListBox(ListSelectedFilters, Zero, One);
            BindListBox(ListSuppressedFilters, Zero, One);
            ShimFilterSchedule.AllInstances.RecurrenceTypeIDGet = (x) => 2;
            BindWeekDaysDropDown();
            BingddlRecurrenceDropDown(true);
            ShimFilterSchedule.AllInstances.FileNameGet = (x) => FileName;
            ShimFilterSchedule.AllInstances.ShowHeaderGet = (x) => true;
            ShimFilterSchedule.AllInstances.MonthLastDayGet = (x) => true;
        }

        private void CreateFiltersObject(bool param)
        {
            _filters = new Filters(new ShimClientConnections(), One);
            _filters.FilterComboList = new List<FilterCombo>
            {
                new FilterCombo {SelectedFilterNo = TestOne},
            };
            _filters.Add(new KMPS.MD.Objects.Filter
            {
                FilterNo = Ten,
                FilterName = Test,
                FilterGroupID = Ten,
                FilterGroupName = Test
            });
            if (param)
            {
                _filters.Add(new KMPS.MD.Objects.Filter
                {
                    FilterNo = Two,
                    FilterName = Test,
                    FilterGroupID = Two,
                    FilterGroupName = Test
                });
            }
            ReflectionHelper.SetValue(_filterExport, FilterObject, _filters);
        }

        private void BindPageInputControl(string filterScheduleId, bool filtersSelect)
        {
            var hfPubID = GetField<HiddenField>(HiddenFiledPubID);
            hfPubID.Value = "1";
            var hfViewType = GetField<HiddenField>(HiddenFiledViewType);
            hfViewType.Value = Enums.ViewType.RecencyView.ToString();
            var hfBrandID = GetField<HiddenField>(HiddenFiledBrandID);
            hfBrandID.Value = "1";
            var PlFiltersSelect = GetField<PlaceHolder>(PanleFiltersSelect);
            PlFiltersSelect.Visible = filtersSelect;
            var txtRecurringStartDate = GetField<TextBox>(TextBoxRecurringStartDate);
            txtRecurringStartDate.Text = DateTime.Now.ToString();
            var txtMonth = GetField<TextBox>(TextBoxMonth);
            txtMonth.Text = DateTime.Now.Month.ToString();
            var txtStartDate = GetField<TextBox>(TextBoxStartDate);
            txtStartDate.Text = DateTime.Now.ToString();
            var cbLastDay = GetField<CheckBox>(CheckBoxLastDay);
            cbLastDay.Checked = false;
            var cbShowHeader = GetField<CheckBox>(CheckBoxShowHeader);
            cbShowHeader.Checked = true;
            var hdFilterScheduleID = GetField<HiddenField>(HiddenFielddFilterScheduleID);
            hdFilterScheduleID.Value = filterScheduleId;
            var txtExportName = GetField<TextBox>(TextBoxExportName);
            txtExportName.Text = DefaultText;
            var txtExportNotes = GetField<TextBox>(TextBoxExportNotes);
            txtExportNotes.Text = DefaultText;
            var txtEmailAddress = GetField<TextBox>(TextBoxEmailAddress);
            txtEmailAddress.Text = DefaultEmialAddres;
            var hfGroupID = GetField<HiddenField>(HiddenFieldGroupID);
            hfGroupID.Value = "1";
            var hdFilterID = GetField<HiddenField>(HiddenFieldFilterID);
            hdFilterID.Value = "1";
        }

        private void BindScheduleTypeDropdown(bool scheduleTypeSelected)
        {
            var ddlScheduleType = GetField<DropDownList>(DropDownScheduleTypeScheduleType);
            ddlScheduleType.Items.Add(new ListItem { Text = "Recurring", Value = "Recurring", Enabled = true, Selected = scheduleTypeSelected });
            ddlScheduleType.Items.Add(new ListItem { Text = "Daily", Value = "Daily", Enabled = true, Selected = !scheduleTypeSelected });
            ddlScheduleType.DataBind();
        }


        private void BindWeekDaysDropDown()
        {
            var cbDays = GetField<CheckBoxList>(CheckBoxListWeekDays);
            var weekDay = Enum.GetValues(typeof(DayOfWeek));
            foreach (DayOfWeek day in weekDay)
            {
                cbDays.Items.Add(new ListItem
                {
                    Text = day.ToString(),
                    Value = day.ToString(),
                    Enabled = true,
                    Selected = true
                });
            }
            cbDays.DataBind();
        }

        private void BindStartTimeDropDown()
        {
            var ddlStartTime = GetField<DropDownList>(DropDownListStartTime);
            ddlStartTime.DataSource = Enumerable.Range(00, 60);
            ddlStartTime.SelectedValue = DateTime.Now.Minute.ToString();
            ddlStartTime.DataBind();
        }

        private void BindRecurringStartTimeDropDown()
        {
            var ddlRecurringStartTime = GetField<DropDownList>(DropDownListRecurringStartTime);
            ddlRecurringStartTime.Items.Add(new ListItem
            {
                Text = DateTime.Now.TimeOfDay.ToString(),
                Value = DateTime.Now.TimeOfDay.ToString(),
                Enabled = true,
                Selected = true
            });
            ddlRecurringStartTime.DataBind();
        }

        private void BingddlRecurrenceDropDown(bool recurrence)
        {
            var ddlRecurrence = GetField<DropDownList>(DropDownListRecurrence);
            ddlRecurrence.Items.Add(new ListItem { Text = "1", Value = "1", Enabled = true, Selected = false });
            ddlRecurrence.Items.Add(new ListItem { Text = "2", Value = "2", Enabled = true, Selected = recurrence });
            ddlRecurrence.Items.Add(new ListItem { Text = "3", Value = "3", Enabled = true, Selected = !recurrence });
            ddlRecurrence.Items.Add(new ListItem { Text = "4", Value = "4", Enabled = true, Selected = false });
            ddlRecurrence.Items.Add(new ListItem { Text = "5", Value = "5", Enabled = true, Selected = false });
            ddlRecurrence.DataBind();
        }

        private ListBox BindListBox(string id, int start, int stop)
        {
            var control = GetField<ListBox>(id);
            for (int i = start; i < stop; i++)
            {
                var item = i + 1;
                control.Items.Add(new ListItem { Text = item.ToString(), Value = item.ToString(), Enabled = true, Selected = true });
            }
            control.DataBind();
            return control;
        }

        private RadioButtonList BindRadioButtonList(string id, int start, int stop, int selectedVale)
        {
            var control = GetField<RadioButtonList>(id);
            for (int i = start; i < stop; i++)
            {
                var item = i + 1;
                var selected = selectedVale == item;
                control.Items.Add(new ListItem { Text = item.ToString(), Value = item.ToString(), Enabled = true, Selected = selected });

            }
            control.DataBind();
            return control;

        }

        private void BindSelectedFieldsDropDown(Dictionary<string, string> lstSelectedDataSource)
        {
            var lstSelectedFields = GetField<ListBox>(ListBoxSelectedFields);
            foreach (var item in lstSelectedDataSource)
            {
                lstSelectedFields.Items.Add(new ListItem { Text = item.Value, Value = item.Key, Enabled = true, Selected = true });
            }
            lstSelectedFields.DataBind();
        }

        private Dictionary<string, string> CreateSelectedFieldsDataSource()
        {
            return new Dictionary<string, string>
            {
                ["Test_Description|Demo|None"] = "1",
                ["Test_Description|Demo|None1"] = "2",
                ["Test_Description|Demo|None2"] = "3",
                ["Test_Description1|Demo|Non3"] = "4",
                ["Test_Description1|Demo|None4"] = "5",
                ["Test_Description1|Demo|None5"] = "6",
                ["Test_Description|Demo|None6"] = "7",
                ["Test_Description|Demo|None7"] = "8",
                ["Test_Description|Demo|None8"] = "9",
                ["Test_Description|Demo|None9"] = "10",
                ["Test_Description|Demo|None10"] = "11",
                ["Test_Description|Demo|None11"] = "12",
                ["Test_Description1|Demo|None12"] = "13",
                ["Test_Description1|Demo|None13"] = "14",
            };
        }


        private void BindSiteTypeDropDown(int siteTypeSelectedIndex)
        {
            var ddlSiteType = GetField<DropDownList>(DropDownListSiteType);
            var siteType = Enum.GetValues(typeof(Enums.SiteType));
            foreach (Enums.SiteType site in siteType)
            {
                ddlSiteType.Items.Add(new ListItem(site.ToString(), site.ToString()));
            }
            ddlSiteType.SelectedIndex = siteTypeSelectedIndex;
            ddlSiteType.DataBind();
        }

        private void BindDownloadTemplateDropDown(int downloadTemplateSelectedIndex)
        {
            var drpDownloadTemplate = GetField<DropDownList>(DropDownListDownloadTemplate);
            drpDownloadTemplate.Items.Add(new ListItem { Value = "1" });
            drpDownloadTemplate.SelectedIndex = downloadTemplateSelectedIndex;
            drpDownloadTemplate.DataBind();
        }

        private void BindxportDropDown(int exportTypeSelectedIndex)
        {
            var ddlExport = GetField<DropDownList>(DropDownListExport);
            var exportType = Enum.GetValues(typeof(Enums.ExportType));
            foreach (Enums.ExportType export in exportType)
            {
                ddlExport.Items.Add(new ListItem(export.ToString(), export.ToString()));
            }
            ddlExport.SelectedIndex = exportTypeSelectedIndex;
            ddlExport.DataBind();
        }

        private void CreateGridFilterSegmentationCounts()
        {
            var selectedHiddenValue = string.Join(", ", Enumerable.Range(1, 10).ToList());
            var grdFilterSegmentationCountsdatatable = new DataTable();
            grdFilterSegmentationCountsdatatable.Columns.Add(new DataColumn("Column1", typeof(string)));
            grdFilterSegmentationCountsdatatable.Columns.Add(new DataColumn("Column2", typeof(string)));
            grdFilterSegmentationCountsdatatable.Columns.Add(new DataColumn("Column3", typeof(string)));
            grdFilterSegmentationCountsdatatable.Columns.Add(new DataColumn("Column4", typeof(string)));
            grdFilterSegmentationCountsdatatable.Columns.Add(new DataColumn("Column5", typeof(string)));
            var row = grdFilterSegmentationCountsdatatable.NewRow();
            row["Column1"] = string.Empty;
            row["Column2"] = string.Empty;
            row["Column3"] = string.Empty;
            row["Column4"] = string.Empty;
            row["Column5"] = string.Empty;
            grdFilterSegmentationCountsdatatable.Rows.Add(row);
            var grdFilterSegmentationCounts = _privateObject.GetFieldOrProperty(GridViewFilterSegmentationCounts) as GridView;
            grdFilterSegmentationCounts.DataSource = grdFilterSegmentationCountsdatatable;
            grdFilterSegmentationCounts.DataBind();
            grdFilterSegmentationCounts.Rows[0].Cells[0].Controls.Add(new RadioButton { ID = "rbFSSelect", Checked = true });
            grdFilterSegmentationCounts.Rows[0].Cells[1].Controls.Add(new HiddenField { ID = "hfSelectedFilterNo", Value = selectedHiddenValue });
            grdFilterSegmentationCounts.Rows[0].Cells[2].Controls.Add(new HiddenField { ID = "hfSuppressedFilterNo", Value = selectedHiddenValue });
            grdFilterSegmentationCounts.Rows[0].Cells[3].Controls.Add(new HiddenField { ID = "hfSelectedFilterOperation", Value = selectedHiddenValue });
            grdFilterSegmentationCounts.Rows[0].Cells[4].Controls.Add(new HiddenField { ID = "hfSuppressedFilterOperation", Value = selectedHiddenValue });
            _privateObject.SetFieldOrProperty(GridViewFilterSegmentationCounts, grdFilterSegmentationCounts);
        }

        private void CommonPageFakeObject(Dictionary<string, string> lstSelectedDataSource, string fieldCase = "PROPERCASE")
        {
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (user, serviceCode, servicefeatureCode, accessCode) => { return true; };
            ShimUtilities.GetExportFieldsClientConnectionsEnumsViewTypeInt32IListOfInt32EnumsExportTypeInt32EnumsExportFieldTypeBoolean =
                (clientconnection, ViewType, BrandID, PubIDs, exportTypeValue, userID, downloadFieldType, IsFilterSchedule) =>
                {
                    if (downloadFieldType == Enums.ExportFieldType.Demo)
                    {
                        return lstSelectedDataSource.
                            Where(x => Convert.ToInt32(x.Value) <= 5).
                                Select(t => new { t.Key, t.Value }).
                                    ToDictionary(t => t.Key, t => t.Value);
                    }
                    else if (downloadFieldType == Enums.ExportFieldType.Adhoc)
                    {
                        return lstSelectedDataSource.
                            Where(x => 
                            {
                                int valueAsInt;
                                return int.TryParse(x.Value, out valueAsInt) && valueAsInt > 10;  
                            }).
                                Select(t => new { t.Key, t.Value }).
                                    ToDictionary(t => t.Key, t => t.Value);
                    }
                    else
                    {
                        return lstSelectedDataSource.
                            Where(x => Convert.ToInt32(x.Value) > 5 && Convert.ToUInt32(x.Value) <= 10).
                                Select(t => new { t.Key, t.Value }).
                                    ToDictionary(t => t.Key, t => t.Value);
                    }
                };
            ShimFilterSchedule.SaveClientConnectionsFilterSchedule = (x, y) => { return 1; };
            ShimFilterSchedule.ExistsByFileNameClientConnectionsInt32String = (x, y, z) => { return false; };
            ShimFilterExportField.DeleteClientConnectionsInt32 = (x, y) => { };

            ShimGroup.GetByGroupIDInt32User = (x, y) => { return new Group { GroupID = 1, GroupName = "Unit Test" }; };
            ShimFilterExportField.SaveClientConnectionsFilterExportField = (x, y) => { return 1; };
            ShimHttpRequest.AllInstances.QueryStringGet = (x) => { return CreateAppSettingsKey(); };
            var selectedHiddenValue = string.Join(", ", Enumerable.Range(1, 10).ToList());
            ShimControl.AllInstances.FindControlString = (sender, controlId) =>
            {
                return GetControlById(controlId, selectedHiddenValue);
            };
            FrameworkUADShim.ShimFilterSegmentation.AllInstances.SelectByIDInt32ClientConnectionsBoolean = (x, y, z, m) =>
            {
                return new FrameworkUAD.Entity.FilterSegmentation { };
            };
            ShimMDFilter.LoadFiltersClientConnectionsInt32Int32 = (x, y, z) =>
            {
                return CreateFiltersObject(z);
            };

            ShimPage.AllInstances.ResponseGet = (x) =>
            {
                return new ShimHttpResponse
                {
                    RedirectString = (y) => { _url = y; }
                };
            };

            ShimHttpResponse.AllInstances.RedirectString = (x, y) => { _url = y; };
            ShimFilterScheduleIntegration.DeleteClientConnectionsInt32 = (x, y) => { };
            ShimFilterScheduleIntegration.SaveClientConnectionsFilterScheduleIntegration = (x, y) => { return 1; };

            ShimDownloadTemplateDetails.GetByDownloadTemplateIDClientConnectionsInt32 = (ccon, s) => new List<DownloadTemplateDetails>
            {
                new DownloadTemplateDetails
                {
                    DownloadTemplateID = 1,
                    DownloadTemplateDetailsID = 1,
                    FieldCase = fieldCase,
                    ExportColumn = "Test_Description",
                }
            };
            ShimFilterExportField.getByFilterScheduleIDClientConnectionsInt32 = (ccon, s) => new List<FilterExportField>
            {
                new FilterExportField
                {
                    FieldCase = fieldCase,
                    ExportColumn = "Test_Description",
                }
            };
        }

        private Filters CreateFiltersObject(int z)
        {
            var filter = new Filters(new KMPlatform.Object.ClientConnections(), z);
            for (var i = 1; i < 11; i++)
            {
                filter.Add(new Objects.Filter { FilterNo = i, FilterGroupID = i, BrandID = i, PubID = i });
            }
            return filter;
        }

        private Control GetControlById(string controlId, string selectedHiddenValue)
        {
            if (controlId == GridViewHttpPost)
            {
                return CreateGridViewHttpPost();
            }
            else if (controlId == "lblParamValue")
            {
                return new Label { ID = controlId, Text = "Email|Email" };
            }
            else if (controlId == "lblParamName")
            {
                return new Label { ID = controlId, Text = "Email|Email" };
            }
            else if (controlId == "lblCustomValue")
            {
                return new Label { ID = controlId, Text = "Email|Email" };
            }
            else if (controlId == "lblParamDisplayName")
            {
                return new Label { ID = controlId, Text = "Email|Email" };
            }
            else if (controlId == "lblHttpPostParamsID")
            {
                return new Label { ID = controlId, Text = "Email|Email" };
            }
            else if (controlId == "ddlMarketoList")
            {
                return new DropDownList { ID = controlId };
            }
            else if (controlId == "rbFSSelect")
            {
                return new RadioButton { ID = controlId, Checked = true };
            }
            else if (controlId == "hfSelectedFilterNo")
            {
                return new HiddenField { ID = controlId, Value = selectedHiddenValue };
            }
            else if (controlId == "hfSuppressedFilterNo")
            {
                return new HiddenField { ID = controlId, Value = selectedHiddenValue };
            }
            else if (controlId == "hfSelectedFilterOperation")
            {
                return new HiddenField { ID = controlId, Value = selectedHiddenValue };
            }
            else if (controlId == "hfSuppressedFilterOperation")
            {
                return new HiddenField { ID = controlId, Value = selectedHiddenValue };
            }
            else if (controlId == "txtMarketoBaseURL")
            {
                return new TextBox { ID = controlId, Text = "127.0.0.1/" };
            }
            else if (controlId == "txtMarketoClientID")
            {
                return new TextBox { ID = controlId, Text = string.Empty };
            }
            else if (controlId == "txtMarketoClientSecret")
            {
                return new TextBox { ID = controlId, Text = string.Empty };
            }
            else if (controlId == "txtMarketoPartition")
            {
                return new TextBox { ID = controlId, Text = string.Empty };
            }
            return new Control { ID = controlId };
        }

        private Control CreateGridViewHttpPost()
        {
            var lblParamValue = new Label { ID = "lblParamValue", Text = "Email|Email" };
            var lblHttpPostParamsID = new DropDownList { ID = "lblHttpPostParamsID" };
            var lblParamName = new DropDownList { ID = "lblParamName" };
            var lblCustomValue = new DropDownList { ID = "lblCustomValue" };
            var lblParamDisplayName = new DropDownList { ID = "lblParamDisplayName" };
            var datatable = new DataTable();
            datatable.Columns.Add(new DataColumn("RowNumber", typeof(string)));
            datatable.Columns.Add(new DataColumn("Column1", typeof(string)));
            datatable.Columns.Add(new DataColumn("Column2", typeof(string)));
            datatable.Columns.Add(new DataColumn("Column3", typeof(string)));
            datatable.Columns.Add(new DataColumn("Column4", typeof(string)));
            datatable.Columns.Add(new DataColumn("Column5", typeof(string)));
            var dataRow = datatable.NewRow();
            dataRow["RowNumber"] = 1;
            dataRow["Column1"] = string.Empty;
            dataRow["Column2"] = string.Empty;
            dataRow["Column3"] = string.Empty;
            dataRow["Column4"] = string.Empty;
            dataRow["Column5"] = string.Empty;
            datatable.Rows.Add(dataRow);

            var gridView = new GridView();
            gridView.ID = GridViewHttpPost;
            gridView.DataSource = datatable;
            gridView.DataBind();
            gridView.Rows[0].Cells[1].Controls.Add(lblParamValue);
            gridView.Rows[0].Cells[2].Controls.Add(lblHttpPostParamsID);
            gridView.Rows[0].Cells[3].Controls.Add(lblParamName);
            gridView.Rows[0].Cells[4].Controls.Add(lblParamDisplayName);
            gridView.Rows[0].Cells[5].Controls.Add(lblCustomValue);
            return gridView;
        }

        private NameValueCollection CreateAppSettingsKey(string filterScheduleId = "1")
        {
            var appKeys = new NameValueCollection();
            appKeys.Add("FilterSegmentationID", "1");
            appKeys.Add("FilterID", "1");
            appKeys.Add("FilterScheduleID", filterScheduleId);
            return appKeys;
        }

        private void CreatePageShimObject(bool isActive = false)
        {
            ShimECNSession.Constructor = (instance) => { };
            var ecnSession = CreateECNSession();
            var shimSession = new ShimECNSession();
            shimSession.ClearSession = () => { };
            shimSession.Instance.CurrentUser = new KMPlatform.Entity.User
            {
                UserID = 1,
                UserName = DefaultText,
                IsActive = isActive,
                CurrentSecurityGroup = new KMPlatform.Entity.SecurityGroup
                {
                    AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator,
                    IsActive = true
                },
                IsPlatformAdministrator = true,
            };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
            ShimFilterExport.AllInstances.MasterGet = (x) =>
            {
                MasterPages.Site site = new ShimSite
                {
                    clientconnectionsGet = () =>
                    {
                        return new KMPlatform.Object.ClientConnections
                        {
                            ClientLiveDBConnectionString = string.Empty,
                            ClientTestDBConnectionString = string.Empty
                        };
                    },
                    UserSessionGet = () => { return shimSession.Instance; },
                    LoggedInUserGet = () => { return 1; }
                };
                return site;
            };
        }

        private ECNSession CreateECNSession()
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var result = typeof(ECNSession).GetConstructor(flags, null, new Type[0], null)
                ?.Invoke(new object[0]) as ECNSession;
            return result;
        }
    }
}
