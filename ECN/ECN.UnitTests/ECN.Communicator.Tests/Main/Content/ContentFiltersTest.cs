using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Fakes;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.QualityTools.Testing.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using ECN.Common.Fakes;
using ecn.communicator.contentmanager;
using ecn.communicator.includes.Fakes;
using ecn.communicator.contentmanager.Fakes;
using ecn.communicator.MasterPages.Fakes;
using ECN.Tests.Helpers;
using EntitiesContent = ECN_Framework_Entities.Communicator.Content;
using EntitiesGroup = ECN_Framework_Entities.Communicator.Group;
using MasterPage = ecn.communicator.MasterPages;
using NUnit.Framework;
using Shouldly;
using KM.Framework.Web.WebForms.FolderSystem.Fakes;

namespace ECN.Communicator.Tests.Main.Content
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class ContentFiltersTest : BasePageTests
    {
        private const string MasterPageIdName = "_master";
        private const string ViewStateName = "_viewState";
        private const string MethodAddFilter = "AddFilter";
        private const string MethodPageLoad = "Page_Load"; 
        private const string MethodConvertToDataTypeChanged = "ConvertToDataType_SelectedIndexChanged"; 
        private const string DummyString = "dummyString"; 
        private const string SelectedFolder = "selectedFldr";
        private const string Action = "action"; 
        private const string DeleteFilter = "deleteFilter"; 
        private const string EditFilter = "editFilter"; 
        private const string DeleteDetail = "deleteFilterDetail";
        private const string Folder = "fd";
        private const string TrueString = "true";
        private const string EmptyBracketsString = "[]";
        private const string Zero = "0"; 
        private const string One = "1";
        private const string Two = "2"; 
        private const string Three = "3"; 
        private const string Four = "4";
        private string _contentFilterFieldName;
        private string _contentFilterCompareValue;
        private NameValueCollection _queryString;
        private contentfilters _page;
        private bool _filtersGridLoaded;
        private IDisposable _shimsContext;
        private FakeHttpContext.FakeHttpContext _fakeHttpContext;

        [SetUp]
        public void Setup()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(DefaultCulture);
            _shimsContext = ShimsContext.Create();
            _fakeHttpContext = new FakeHttpContext.FakeHttpContext();
            _page = new contentfilters();
            InitializePage(_page);
            SetCurrentUser();
            ReflectionHelper.SetValue(_page, typeof(Page), MasterPageIdName, new MasterPage.Communicator());
            _filtersGridLoaded = false;
        }

        [TearDown]
        public void TearDown()
        {
            ShimsContext.Reset();
            _shimsContext.Dispose();
            _fakeHttpContext.Dispose();
        }

        [TestCase(Zero, 1)]
        [TestCase(One, 1)]
        [TestCase(Four, 1)]
        public void ConvertToDataType_SelectedIndexChanged_WhenSelectedIndexIsNotTwo_CompValueNumberValidatorIsDisabled(string dataTypeIndex, int numberOfItems)
        {
            // Arrange
            Initialize();
            SetCompFieldName(Three, numberOfItems);
            SetDataType(dataTypeIndex);
            var methodArgs = new object[] { null, EventArgs.Empty };

            // Act
            ReflectionHelper.CallMethod(typeof(contentfilters), MethodConvertToDataTypeChanged, methodArgs, _page);

            // Assert
            var numberValidator = ReflectionHelper.GetFieldValue(_page, "CompValueNumberValidator") as CustomValidator;
            _page.ShouldSatisfyAllConditions(
                () => numberValidator.ShouldNotBeNull(),
                () => numberValidator.Enabled.ShouldBeFalse());
        }

        [TestCase(Two, 1)]
        public void ConvertToDataType_SelectedIndexChanged_WhenSelectedIndexIsTwo_CompValueNumberValidatorIsEnabled(string dataTypeIndex, int numberOfItems)
        {
            // Arrange
            Initialize();
            SetCompFieldName(Three, numberOfItems);
            SetDataType(dataTypeIndex);
            var methodArgs = new object[] { null, EventArgs.Empty };

            // Act
            ReflectionHelper.CallMethod(typeof(contentfilters), MethodConvertToDataTypeChanged, methodArgs, _page);

            // Assert
            var numberValidator = ReflectionHelper.GetFieldValue(_page, "CompValueNumberValidator") as CustomValidator;
            _page.ShouldSatisfyAllConditions(
                () => numberValidator.ShouldNotBeNull(),
                () => numberValidator.Enabled.ShouldBeTrue());
        }

        [TestCase(Three, 25, false, "")]
        [TestCase(Three, 20, true, "ERROR: Compare Field")]
        public void ConvertToDataType_SelectedIndexChanged_WhenSelectedIndexIsChanged_ErrorMessageToggles(string dataTypeIndex, int numberOfItems, bool errorDisplayed, string errorText)
        {
            // Arrange
            Initialize();
            SetCompFieldName(Three, numberOfItems);
            SetDataType(dataTypeIndex);
            var methodArgs = new object[] { null, EventArgs.Empty };

            // Act
            ReflectionHelper.CallMethod(typeof(contentfilters), MethodConvertToDataTypeChanged, methodArgs, _page);

            // Assert
            var errorMessage = ReflectionHelper.GetFieldValue(_page, "ErrorLabel") as Label;
            _page.ShouldSatisfyAllConditions(
                () => errorMessage.ShouldNotBeNull(),
                () => errorMessage.Visible.ShouldBe(errorDisplayed),
                () => errorMessage.Text.ShouldContain(errorText));
        }

        [TestCase(DummyString)]
        [TestCase(DeleteFilter)]
        [TestCase(EditFilter)]
        [TestCase(DeleteDetail)]
        public void Page_Load_Success_SessionContainsSelectedFolder(string action)
        {
            // Arrange
            Initialize();
            _queryString.Add(Folder, TrueString);
            _queryString.Add(Action, action);
            CreatePageLoadShims();
            var methodArgs = new object[] { null, EventArgs.Empty };
            
            // Act
            ReflectionHelper.CallMethod(typeof(contentfilters), MethodPageLoad, methodArgs, _page);

            // Assert
            var pageSession = _page.Session;
            _page.ShouldSatisfyAllConditions(
                () => pageSession.ShouldNotBeNull(),
                () => pageSession[SelectedFolder].ShouldNotBeNull());
        }

        [TestCase("ending")]
        [TestCase("starting")]
        [TestCase("contains")]
        [TestCase("not ending")]
        [TestCase("not starting")]
        [TestCase("not contains")]
        [TestCase("between")]
        [TestCase("equals")]
        [TestCase("less than")]
        [TestCase("greater than")]
        [TestCase("NOT equals")]
        [TestCase("NOT greater than")]
        [TestCase("NOT less than")]
        [TestCase("NOT between")]
        [TestCase("NOT contains")]
        [TestCase("NOT starting")]
        [TestCase("NOT ending")]
        public void Page_Load_WithDeleteFilterDetailsActionAndDifferentComparator_FilterIsUpdatedAndSaved(string comparator)
        {
            // Arrange
            Initialize();
            _queryString.Add(Folder, TrueString);
            _queryString.Add(Action, DeleteDetail);
            CreatePageLoadShims();
            var contentFilterUpdated = false;
            var contentFilterDetailObject = ReflectionHelper.CreateInstance(typeof(ContentFilterDetail));
            contentFilterDetailObject.Comparator = comparator;
            ShimContentFilterDetail.GetByFilterIDInt32User = (x, y) => new List<ContentFilterDetail> { contentFilterDetailObject };
            ShimContentFilter.SaveContentFilterUser = (x, y) => 
            {
                contentFilterUpdated = true;
            };
            var methodArgs = new object[] { null, EventArgs.Empty };

            // Act
            ReflectionHelper.CallMethod(typeof(contentfilters), MethodPageLoad, methodArgs, _page);

            // Assert

            var pageSession = _page.Session;
            _page.ShouldSatisfyAllConditions(
                () => pageSession.ShouldNotBeNull(),
                () => pageSession[SelectedFolder].ShouldNotBeNull(),
                () => contentFilterUpdated.ShouldBeTrue());
        }

        private void CreatePageLoadShims()
        {
            ShimCommunicator.AllInstances.CurrentMenuCodeSetEnumsMenuCode = (x, y) => { };
            ShimMasterPageEx.AllInstances.HeadingSetString = (x, y) => { };
            ShimMasterPageEx.AllInstances.HelpContentSetString = (x, y) => { };
            ShimMasterPageEx.AllInstances.HelpTitleSetString = (x, y) => { };
            ShimContentFilter.HasPermissionEnumsAccessUser = (x, y) => true;
            ShimFolderSystemBase.AllInstances.LoadFolderTree = (x) => { };
            ShimContent.GetByFolderIDCustomerIDInt32UserBooleanString = (a, b, c, d) => new List<EntitiesContent> { ReflectionHelper.CreateInstance(typeof(EntitiesContent)) };
            ShimGroup.GetByCustomerID_NoAccessCheckInt32String = (x, y) => new List<EntitiesGroup> { ReflectionHelper.CreateInstance(typeof(EntitiesGroup)) };
            ShimHttpRequest.AllInstances.QueryStringGet = (x) => _queryString;
            var contentFilterDetailTable = new DataTable();
            contentFilterDetailTable.Columns.Add("FilterName");
            contentFilterDetailTable.Rows.Add(DummyString);
            ShimContentFilterDetail.GetByContentIDFilterIDInt32User = (x, y) => contentFilterDetailTable;
            ShimContentFilter.GetByLayoutIDSlotNumberInt32Int32UserBoolean = (a, b, c, d) => new List<ContentFilter> { ReflectionHelper.CreateInstance(typeof(ContentFilter)) };
            ShimContentFilter.GetByFilterIDInt32UserBoolean = (x, y, z) => ReflectionHelper.CreateInstance(typeof(ContentFilter));
            ShimContentFilter.DeleteInt32Int32User = (x, y, z) => { };
            ShimHttpResponse.AllInstances.RedirectString = (x, y) => { };
            ShimContent.GetByContentIDInt32UserBoolean = (x, y, z) => ReflectionHelper.CreateInstance(typeof(EntitiesContent));
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (x) => ReflectionHelper.CreateInstance(typeof(EntitiesGroup));
            ShimGroupDataFields.GetByGroupID_NoAccessCheckInt32 = (x) => new List<GroupDataFields> { ReflectionHelper.CreateInstance(typeof(GroupDataFields)) };
            ShimContentFilterDetail.GetByFDIDInt32User = (x, y) => ReflectionHelper.CreateInstance(typeof(ContentFilterDetail));
            ShimContentFilterDetail.DeleteInt32Int32User = (x, y, z) => { };
            ShimContentFilterDetail.GetByFilterIDInt32User = (x, y) => new List<ContentFilterDetail> { ReflectionHelper.CreateInstance(typeof(ContentFilterDetail)) }; // casee here
            ShimContentFilter.SaveContentFilterUser = (x, y) => { };
        }

        [TestCase(DummyString)]
        public void AddFilters_WhenFieldTypeIndexIsZero_CompareValueIsEmptyString(string comparator)
        {
            // Arrange
            ReflectionHelper.SetField(_page, "FilterIDValue", new TextBox { Text = One });
            SetDataType(Zero);
            SetComparator(comparator);
            CreateShims();
            ShimContentFilterDetail.SaveContentFilterDetailUser = (contentFilterDetail, user) =>
            {
                _contentFilterFieldName = contentFilterDetail.FieldName;
                _contentFilterCompareValue = contentFilterDetail.CompareValue;
            };
            var methodArgs = new object[] { null, EventArgs.Empty };

            // Act
            ReflectionHelper.CallMethod(typeof(contentfilters), MethodAddFilter, methodArgs, _page);

            // Assert
            _page.ShouldSatisfyAllConditions(
                () => _filtersGridLoaded.ShouldBeTrue(),
                () => _contentFilterFieldName.ShouldBe(EmptyBracketsString),
                () => _contentFilterCompareValue.ShouldBeEmpty());
        }

        [TestCase("between")]
        [TestCase(DummyString)]
        public void AddFilters_WhenFieldTypeIndexIsThree_CompareValueIsNotEmptyString(string comparator)
        {
            // Arrange
            ReflectionHelper.SetField(_page, "FilterIDValue", new TextBox { Text = One });
            SetDataType(Three);
            SetComparator(comparator);
            CreateShims();
            ShimContentFilterDetail.SaveContentFilterDetailUser = (contentFilterDetail, user) =>
            {
                _contentFilterFieldName = contentFilterDetail.FieldName;
                _contentFilterCompareValue = contentFilterDetail.CompareValue;
            };
            var methodArgs = new object[] { null, EventArgs.Empty };

            // Act
            ReflectionHelper.CallMethod(typeof(contentfilters), MethodAddFilter, methodArgs, _page);

            // Assert
            _page.ShouldSatisfyAllConditions(
                () => _filtersGridLoaded.ShouldBeTrue(),
                () => _contentFilterFieldName.ShouldNotBeNullOrWhiteSpace(),
                () => _contentFilterCompareValue.ShouldNotBeNullOrWhiteSpace());
        }

        [TestCase("ending")]
        [TestCase("starting")]
        [TestCase("contains")]
        [TestCase("not ending")]
        [TestCase("not starting")]
        [TestCase("not contains")]
        [TestCase("between")]
        [TestCase("equals")]
        [TestCase("less than")]
        [TestCase("greater than")]
        [TestCase("NOT equals")]
        [TestCase("NOT greater than")]
        [TestCase("NOT less than")]
        [TestCase("NOT between")]
        [TestCase("NOT contains")]
        [TestCase("NOT starting")]
        [TestCase("NOT ending")]
        public void AddFilters_WhenFieldTypeIndexIsGreaterThanZero_CompareValueIsNotEmptyString(string comparator)
        {
            // Arrange
            ReflectionHelper.SetField(_page, "FilterIDValue", new TextBox { Text = One });
            SetDataType(Two);
            SetComparator(comparator);
            SetCompFieldName(comparator);
            CreateShims();
            var contentFilterDetailObject = ReflectionHelper.CreateInstance(typeof(ContentFilterDetail));
            contentFilterDetailObject.Comparator = comparator;
            ShimContentFilterDetail.GetByFilterIDInt32User = (x, y) => new List<ContentFilterDetail> { contentFilterDetailObject };
            ShimContentFilterDetail.SaveContentFilterDetailUser = (contentFilterDetail, user) =>
            {
                _contentFilterFieldName = contentFilterDetail.FieldName;
                _contentFilterCompareValue = contentFilterDetail.CompareValue;
            };
            var methodArgs = new object[] { null, EventArgs.Empty };

            // Act
            ReflectionHelper.CallMethod(typeof(contentfilters), MethodAddFilter, methodArgs, _page);

            // Assert
            _page.ShouldSatisfyAllConditions(
                () => _filtersGridLoaded.ShouldBeTrue(),
                () => _contentFilterFieldName.ShouldNotBeNullOrWhiteSpace(),
                () => _contentFilterFieldName.Contains(comparator),
                () => _contentFilterCompareValue.ShouldNotBeNullOrWhiteSpace());
        }

        [TestCase(DummyString)]
        public void AddFilters_WhenFieldTypeIndexIsFour_CompareValueIsNotEmptyString(string comparator)
        {
            // Arrange
            ReflectionHelper.SetField(_page, "FilterIDValue", new TextBox { Text = One });
            SetDataType(Four);
            SetComparator(comparator);
            CreateShims();
            ShimContentFilterDetail.SaveContentFilterDetailUser = (contentFilterDetail, user) =>
            {
                _contentFilterFieldName = contentFilterDetail.FieldName;
                _contentFilterCompareValue = contentFilterDetail.CompareValue;
            };
            var methodArgs = new object[] { null, EventArgs.Empty };

            // Act
            ReflectionHelper.CallMethod(typeof(contentfilters), MethodAddFilter, methodArgs, _page);

            // Assert
            _page.ShouldSatisfyAllConditions(
                () => _filtersGridLoaded.ShouldBeTrue(),
                () => _contentFilterFieldName.ShouldNotBeNullOrWhiteSpace(),
                () => _contentFilterCompareValue.ShouldNotBeNullOrWhiteSpace());
        }

        private void Initialize()
        {
            _contentFilterFieldName = string.Empty;
            _contentFilterCompareValue = string.Empty;
            _queryString = new NameValueCollection();
            _queryString.Add("LayoutID", One);
            _queryString.Add("SlotNumber", One);
            _queryString.Add("FilterID", One);
            _queryString.Add("FilterDetailID", One);
        }

        private void CreateShims()
        {
            var contentFilterDetailObject = ReflectionHelper.CreateInstance(typeof(ContentFilterDetail));
            ShimContentFilterDetail.GetByFilterIDInt32User = (x, y) => new List<ContentFilterDetail> { contentFilterDetailObject };
            ShimContentFilterDetail.SaveContentFilterDetailUser = (x, y) => { };
            var contentFilter = ReflectionHelper.CreateInstance(typeof(ContentFilter));
            ShimContentFilter.GetByFilterIDInt32UserBoolean = (x, y, z) => contentFilter;
            ShimContentFilter.SaveContentFilterUser = (x, y) => { };
            Shimcontentfilters.AllInstances.loadFiltersGridInt32Int32 = (x, y, z) =>
            {
                _filtersGridLoaded = true;
            };
        }

        private void SetDataType(string type)
        {
            var typeIndex = Convert.ToInt32(type);
            var dataTypeDropDown = new DropDownList();
            dataTypeDropDown.Items.Add(new ListItem { Value = type });
            dataTypeDropDown.Items.Add(new ListItem { Value = type });
            dataTypeDropDown.Items.Add(new ListItem { Value = type });
            dataTypeDropDown.Items.Add(new ListItem { Value = type });
            dataTypeDropDown.Items.Add(new ListItem { Value = type });
            dataTypeDropDown.Items[typeIndex].Selected = true;
            ReflectionHelper.SetField(_page, "ConvertToDataType", dataTypeDropDown);
        }

        private void SetComparator(string type)
        {
            ReflectionHelper.SetField(_page, "Comparator", new DropDownList
            {
                Items =
                {
                    new ListItem
                    {
                        Selected = true,
                        Value = type
                    }
                }
            });
        }

        private void SetCompFieldName(string type)
        {
            ReflectionHelper.SetField(_page, "CompFieldName", new DropDownList
            {
                Items =
                {
                    new ListItem
                    {
                        Selected = true,
                        Value = type
                    }
                }
            });
        }

        private void SetCompFieldName(string typeValue, int numberOfItems)
        {
            var compFiledNameDropDown = new DropDownList();
            var compFiledNameListItem = new ListItem()
            {
                Value = typeValue,
                Selected = false
            };
            for (var i = 0; i < numberOfItems; i++)
            {
                compFiledNameDropDown.Items.Add(compFiledNameListItem);
            }
            compFiledNameDropDown.Items.Add(new ListItem
            {
                Selected = true,
                Value = typeValue
            });
            ReflectionHelper.SetField(_page, "CompFieldName", compFiledNameDropDown);
        }

        private void SetCurrentUser()
        {
            const string authData = "1,1,1,1,0D5D37DB-ED27-456A-8C0B-062F9F37983B";
            ShimECNSession.AllInstances.RefreshSession = session => { session.CurrentUser = CurrentUserMock; };
            var formsAuthenticationTicket = new FormsAuthenticationTicket(1, CurrentUserMock.UserID.ToString(),
                DateTime.Now, DateTime.MaxValue, true, authData);
            var formsIdentity = new FormsIdentity(formsAuthenticationTicket);
            HttpContext.Current.User = new GenericPrincipal(formsIdentity, null);
        }
    }
}
