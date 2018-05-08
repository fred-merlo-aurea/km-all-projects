using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web;
using System.Web.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ActiveUp.WebControls;
using CKEditor.NET;
using ecn.communicator.listsmanager;
using ecn.communicator.listsmanager.Fakes;
using ecn.communicator.MasterPages.Fakes;
using Ecn.Communicator.Main.Lists.Interfaces;
using ECN.Common.Fakes;
using ECN.Communicator.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Shouldly;
using MsTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using PageCommunicator = ecn.communicator.MasterPages.Communicator;
using WebPanel = System.Web.UI.WebControls.Panel;

namespace ECN.Communicator.Tests.Main.Lists
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class GroupSubscribeTest
    {
        private const int ValueZero = 0;
        private const int ValueNegativeOne = -1;
        private const int One = 1;
        private const int CustomerID = 1;
        private const int UserID = 1;
        private const string DummyText = "Dummy Text";
        private const string SmartFormNameTextBoxName = "smartFormName";
        private const string ResponseUserMsgSubjectTextBoxName = "Response_UserMsgSubject";
        private const string ResponseUserScreenTextBoxName = "Response_UserScreen";
        private const string ResponseFromEmailTextBoxName = "Response_FromEmail";
        private const string ResponseAdminEmailTextBoxName = "Response_AdminEmail";
        private const string ResponseAdminMsgSubjectTextBoxName = "Response_AdminMsgSubject";
        private const string ResponseAdminMsgBodyTextBoxName = "Response_AdminMsgBody";
        private const string ResponseUserMsgBodyTextBoxName = "Response_UserMsgBody";
        private const string SmartFormGridControlName = "SmartFormGrid";
        private const string GridPagerControlName = "GridPager";
        private const string SoOptinFieldSelectionListBoxName = "SO_OptinFieldSelection";
        private const string SoHTMLCodeControlName = "SO_HTMLCode";
        private const string SoRefreshHTMLControlName = "SO_RefreshHTML";
        private const string SoOptinHTMLSaveControlName = "SO_OptinHTMLSave";
        private const string ErrorPlaceHolderControlName = "phError";
        private const string ErrorMessageLabelControlName = "lblErrorMessage";
        private const string SoSmartFormButtonName = "SO_SmartFormButton";
        private const string DoSmartFormButtonName = "DO_SmartFormButton";
        private const string SoOptinHTMLSaveNewControlName = "SO_OptinHTMLSaveNew";
        private const string CreateNewSmartForm = "Create New smartForm";
        private const string SampleHost = "km.com";
        private const string Delete = "delete";
        private const string DummyString = "dummystring";
        private const string SampleHttpHost = "http://km.com";
        private const string SampleHostPath = "http://km.com/addTemplate";
        private const string SampleUserAgent = "http://km.com/addTemplate";
        private const string CampaignItemTemplateID = "1";
        private const string KMCommon_Application = "1";
        private const int GroupId = 10;
        private const string GroupIdQueryStringKey = "GroupID";
        private const string GroupIdPropertyName = "GroupId";
        private const int SFID = 20;
        private const string SFIDPropertyName = "SFID";
        private const string SFIDQueryStringKey = "SFID";
        private const string Action = "Edit";
        private const string ActionPropertyName = "RequestedAction";
        private const string ActionQueryStringKey = "action";

        private groupsubscribe _groupSubscribe;
        private MsTest.PrivateObject _privateObject;
        private NameValueCollection _queryString;
        private IDisposable _context;

        [SetUp]
        public void Setup()
        {
            _context = ShimsContext.Create();
            InitializeHttpContext();
            InitializeQueryString();

            _groupSubscribe = new groupsubscribe();
            _privateObject = new MsTest.PrivateObject(_groupSubscribe);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void Page_Load_WhenRequestActionIsDelete_SmartFormsHistoryIsDeleted()
        {
            // Arrange 
            var groupSubscribe = GetGroupSubscribe();
            InitializeSession();
            CreateShims();
            var smartFormHistoryDeleted = false;
            Shimgroupsubscribe.AllInstances.RequestedActionGet = (x) => Delete;
            ShimSmartFormsHistory.DeleteInt32User = (x, y) =>
            {
                smartFormHistoryDeleted = true;
            };

            // Act
            typeof(groupsubscribe)
                .CallMethod(
                    "Page_Load",
                    new object[]
                    {
                        null,
                        EventArgs.Empty
                    },
                    groupSubscribe);

            // Assert
            smartFormHistoryDeleted.ShouldBeTrue();
        }

        [Test]
        public void Page_Load_WhenRequestActionIsNotDelete_HTMLSaveOptionIsEnabled()
        {
            // Arrange 
            var groupSubscribeObject = GetGroupSubscribe();
            InitializeSession();
            CreateShims();
            Shimgroupsubscribe.AllInstances.RequestedActionGet = (x) => DummyString;
            var newOptionText = "Create as a  New smartFrom";

            // Act
            typeof(groupsubscribe)
                .CallMethod(
                    "Page_Load",
                    new object[]
                    {
                        null,
                        EventArgs.Empty
                    },
                    groupSubscribeObject);

            // Assert
            var htmlSaveOption = ReflectionHelper.GetField(groupSubscribeObject, "SO_OptinHTMLSave") as Button;
            var htmlSaveNewOption = ReflectionHelper.GetField(groupSubscribeObject, "SO_OptinHTMLSaveNew") as Button;
            groupSubscribeObject.ShouldSatisfyAllConditions(
                () => htmlSaveOption.ShouldNotBeNull(),
                () => htmlSaveNewOption.ShouldNotBeNull(),
                () => htmlSaveOption.Enabled.ShouldBeTrue(),
                () => htmlSaveNewOption.Text.ShouldBe(newOptionText));
        }

        [Test]
        public void Page_Load_WhenClientHasService_PanelTextBoxIsHidden()
        {
            // Arrange 
            var groupSubscribeObject = GetGroupSubscribe();
            InitializeSession();
            CreateShims();
            Shimgroupsubscribe.AllInstances.GroupIdGet = (x) => One;
            Shimgroupsubscribe.AllInstances.SFIDGet = (x) => ValueZero;
            ShimClient.HasServiceInt32EnumsServices = (x, y) => true;
            Shimgroupsubscribe.AllInstances.SetHTMLCodeInt32BooleanCKEditorControl = (a, b, c, d) => { };

            // Act
            typeof(groupsubscribe)
                .CallMethod(
                    "Page_Load",
                    new object[]
                    {
                        null,
                        EventArgs.Empty
                    },
                    groupSubscribeObject);

            // Assert
            var textBoxPanel = ReflectionHelper.GetField(groupSubscribeObject, "panelTexbox") as WebPanel;
            var editorPanel = ReflectionHelper.GetField(groupSubscribeObject, "DO_panelFCKEditor") as WebPanel;
            groupSubscribeObject.ShouldSatisfyAllConditions(
                () => textBoxPanel.ShouldNotBeNull(),
                () => editorPanel.ShouldNotBeNull(),
                () => textBoxPanel.Enabled.ShouldBeFalse(),
                () => editorPanel.Enabled.ShouldBeTrue());
        }

        [Test]
        public void Page_Load_WhenClientHasNoService_PanelTextBoxIsShown()
        {
            // Arrange 
            var groupSubscribeObject = GetGroupSubscribe();
            InitializeSession();
            CreateShims();
            Shimgroupsubscribe.AllInstances.GroupIdGet = (x) => One;
            Shimgroupsubscribe.AllInstances.SFIDGet = (x) => ValueZero;
            ShimClient.HasServiceInt32EnumsServices = (x, y) => false;
            Shimgroupsubscribe.AllInstances.SetHTMLCodeInt32BooleanCKEditorControl = (a, b, c, d) => { };

            // Act
            typeof(groupsubscribe)
                .CallMethod(
                    "Page_Load",
                    new object[]
                    {
                        null,
                        EventArgs.Empty
                    },
                    groupSubscribeObject);

            // Assert
            var textBoxPanel = ReflectionHelper.GetField(groupSubscribeObject, "panelTexbox") as WebPanel;
            var editorPanel = ReflectionHelper.GetField(groupSubscribeObject, "DO_panelFCKEditor") as WebPanel;
            groupSubscribeObject.ShouldSatisfyAllConditions(
                () => textBoxPanel.ShouldNotBeNull(),
                () => editorPanel.ShouldNotBeNull(),
                () => textBoxPanel.Enabled.ShouldBeTrue(),
                () => editorPanel.Enabled.ShouldBeFalse());
        }

        [Test]
        public void SoOptinHTMLSaveNewClick_NoException_CallsSmartFormsHistorySave()
        {
            // Arrange
            var masterCommunicator = CreateMasterCommunicator();
            var smartFormsHistory = CreateSmartFormsHistory();
            smartFormsHistory.Setup(x => x.Save(It.IsAny<SmartFormsHistory>(), It.IsAny<User>()));

            var request = CreateRequest();
            var groupSubscribe = new groupsubscribe(masterCommunicator.Object, smartFormsHistory.Object, request.Object);

            var textBoxNames = new string[]
            {
                SmartFormNameTextBoxName,
                ResponseUserMsgSubjectTextBoxName,
                ResponseUserScreenTextBoxName,
                ResponseFromEmailTextBoxName,
                ResponseAdminEmailTextBoxName,
                ResponseAdminMsgSubjectTextBoxName,
                ResponseAdminMsgBodyTextBoxName,
                ResponseUserMsgBodyTextBoxName
            };
            var smartGrid = new DataGrid();
            var pagerBuilder = new PagerBuilder() { RecordCount = ValueNegativeOne };
            var refreshButton = new Button() { Visible = true };
            var optinHtmlSave = new Button() { Visible = true };
            var errorPlaceholder = new PlaceHolder() { Visible = false };
            var errorMessageLaber = new Label() { Text = DummyText };

            InitializeControls(groupSubscribe, textBoxNames, smartGrid, pagerBuilder, refreshButton, optinHtmlSave, errorPlaceholder, errorMessageLaber);

            // Act
            typeof(groupsubscribe)
                .CallMethod(
                    "SO_OptinHTMLSaveNew_Click",
                    new object[]
                    {
                        null,
                        null
                    },
                    groupSubscribe);

            // Assert
            smartFormsHistory.Verify(x => x.Save(It.IsAny<SmartFormsHistory>(), It.IsAny<User>()), Times.Once());
            smartFormsHistory.Verify(x => x.GetByGroupID(It.IsAny<int>(), It.IsAny<User>()), Times.Once());

            Assert.That(refreshButton.Enabled, Is.False);
            Assert.That(optinHtmlSave.Enabled, Is.False);
            Assert.That(smartGrid.DataSource, Is.InstanceOf(typeof(IList)));
            var dataSource = smartGrid.DataSource as IList;
            Assert.That(dataSource, Is.Not.Null);
            Assert.That((dataSource as IList).Count, Is.EqualTo(ValueZero));
            Assert.That(pagerBuilder.RecordCount, Is.EqualTo(ValueZero));
            AssertTextBoxs(groupSubscribe, textBoxNames, Is.Empty);

            Assert.That(errorPlaceholder.Visible, Is.False);
            Assert.That(errorMessageLaber.Text, Is.EqualTo(DummyText));
        }

        [Test]
        public void SoOptinHTMLSaveNewClick_ECNException_CallsSmartFormsHistorySave()
        {
            // Arrange
            var masterCommunicator = CreateMasterCommunicator();
            var smartFormsHistory = CreateSmartFormsHistory();
            smartFormsHistory.Setup(x => x.Save(It.IsAny<SmartFormsHistory>(), It.IsAny<User>()))
                .Throws(new ECNException(new List<ECNError>()));

            var request = CreateRequest();
            var groupSubscribe = new groupsubscribe(masterCommunicator.Object, smartFormsHistory.Object, request.Object);

            var textBoxNames = new string[]
            {
                SmartFormNameTextBoxName,
                ResponseUserMsgSubjectTextBoxName,
                ResponseUserScreenTextBoxName,
                ResponseFromEmailTextBoxName,
                ResponseAdminEmailTextBoxName,
                ResponseAdminMsgSubjectTextBoxName,
                ResponseAdminMsgBodyTextBoxName,
                ResponseUserMsgBodyTextBoxName
            };
            var smartGrid = new DataGrid();
            var pagerBuilder = new PagerBuilder() { RecordCount = ValueNegativeOne };
            var refreshButton = new Button() { Visible = true };
            var optinHtmlSave = new Button() { Visible = true };
            var errorPlaceholder = new PlaceHolder() { Visible = false };
            var errorMessageLaber = new Label() { Text = DummyText };

            InitializeControls(groupSubscribe, textBoxNames, smartGrid, pagerBuilder, refreshButton, optinHtmlSave, errorPlaceholder, errorMessageLaber);

            // Act
            typeof(groupsubscribe)
                .CallMethod(
                    "SO_OptinHTMLSaveNew_Click",
                    new object[]
                    {
                        null,
                        null
                    },
                    groupSubscribe);

            // Assert
            smartFormsHistory.Verify(x => x.Save(It.IsAny<SmartFormsHistory>(), It.IsAny<User>()), Times.Once());
            smartFormsHistory.Verify(x => x.GetByGroupID(It.IsAny<int>(), It.IsAny<User>()), Times.Never());

            Assert.That(refreshButton.Enabled, Is.True);
            Assert.That(optinHtmlSave.Enabled, Is.True);
            Assert.That(smartGrid.DataSource, Is.Null);
            Assert.That(pagerBuilder.RecordCount, Is.EqualTo(ValueNegativeOne));
            AssertTextBoxs(groupSubscribe, textBoxNames, Is.EqualTo(DummyText));

            Assert.That(errorPlaceholder.Visible, Is.True);
            Assert.That(errorMessageLaber.Text, Is.Empty);
        }

        [Test]
        public void SaveOptinHTMLClick_NoException_CallsSmartFormsHistorySave()
        {
            // Arrange
            var masterCommunicator = CreateMasterCommunicator();

            var smartFormsHistory = CreateSmartFormsHistory();
            smartFormsHistory.Setup(x => x.Save(It.IsAny<SmartFormsHistory>(), It.IsAny<User>()));

            var request = new Mock<IRequest>();
            request.Setup(x => x.GetQueryStringValue(It.IsAny<string>())).Returns(string.Empty);

            var groupSubscribe = new groupsubscribe(masterCommunicator.Object, smartFormsHistory.Object, request.Object);

            var textBoxNames = new string[]
            {
                SmartFormNameTextBoxName,
                ResponseUserMsgSubjectTextBoxName,
                ResponseUserScreenTextBoxName,
                ResponseFromEmailTextBoxName,
                ResponseAdminEmailTextBoxName,
                ResponseAdminMsgSubjectTextBoxName,
                ResponseAdminMsgBodyTextBoxName,
                ResponseUserMsgBodyTextBoxName
            };
            var smartGrid = new DataGrid();
            var pagerBuilder = new PagerBuilder() { RecordCount = ValueNegativeOne };
            var refreshButton = new Button() { Visible = true };
            var optinHtmlSave = new Button() { Visible = true };
            var errorPlaceholder = new PlaceHolder() { Visible = false };
            var errorMessageLaber = new Label() { Text = DummyText };

            InitializeControls(groupSubscribe, textBoxNames, smartGrid, pagerBuilder, refreshButton, optinHtmlSave, errorPlaceholder, errorMessageLaber);

            ReflectionHelper.SetField(groupSubscribe, SoSmartFormButtonName, new Button { Enabled = false });
            ReflectionHelper.SetField(groupSubscribe, DoSmartFormButtonName, new Button { Enabled = true });
            var htmlSaveNewButton = new Button() { Text = DummyText };
            ReflectionHelper.SetField(groupSubscribe, SoOptinHTMLSaveNewControlName, htmlSaveNewButton);

            // Act
            typeof(groupsubscribe)
                .CallMethod(
                    "SaveOptinHTML_Click",
                    new object[]
                    {
                        null,
                        null
                    },
                    groupSubscribe);

            // Assert
            smartFormsHistory.Verify(x => x.Save(It.IsAny<SmartFormsHistory>(), It.IsAny<User>()), Times.Once());
            smartFormsHistory.Verify(x => x.GetByGroupID(It.IsAny<int>(), It.IsAny<User>()), Times.Once());
            smartFormsHistory.Verify(x => x.GetBySmartFormID(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<User>()), Times.Once());

            Assert.That(refreshButton.Enabled, Is.False);
            Assert.That(optinHtmlSave.Enabled, Is.False);
            Assert.That(smartGrid.DataSource, Is.InstanceOf(typeof(IList)));
            var dataSource = smartGrid.DataSource as IList;
            Assert.That(dataSource, Is.Not.Null);
            Assert.That(dataSource.Count, Is.EqualTo(ValueZero));
            Assert.That(pagerBuilder.RecordCount, Is.EqualTo(ValueZero));
            AssertTextBoxs(groupSubscribe, textBoxNames, Is.Empty);

            Assert.That(errorPlaceholder.Visible, Is.False);
            Assert.That(errorMessageLaber.Text, Is.EqualTo(DummyText));

            Assert.That(htmlSaveNewButton.Text, Is.EqualTo(CreateNewSmartForm));
        }

        [Test]
        public void SaveOptinHTMLClick_ECNException_CallsSmartFormsHistorySave()
        {
            // Arrange
            var masterCommunicator = CreateMasterCommunicator();

            var smartFormsHistory = CreateSmartFormsHistory();
            smartFormsHistory.Setup(x => x.GetBySmartFormID(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<User>()))
                .Throws(new ECNException(new List<ECNError>()));

            var request = new Mock<IRequest>();
            request.Setup(x => x.GetQueryStringValue(It.IsAny<string>())).Returns(string.Empty);

            var groupSubscribe = new groupsubscribe(masterCommunicator.Object, smartFormsHistory.Object, request.Object);

            var textBoxNames = new string[]
            {
                SmartFormNameTextBoxName,
                ResponseUserMsgSubjectTextBoxName,
                ResponseUserScreenTextBoxName,
                ResponseFromEmailTextBoxName,
                ResponseAdminEmailTextBoxName,
                ResponseAdminMsgSubjectTextBoxName,
                ResponseAdminMsgBodyTextBoxName,
                ResponseUserMsgBodyTextBoxName
            };
            var smartGrid = new DataGrid();
            var pagerBuilder = new PagerBuilder() { RecordCount = ValueNegativeOne };
            var refreshButton = new Button() { Visible = true };
            var optinHtmlSave = new Button() { Visible = true };
            var errorPlaceholder = new PlaceHolder() { Visible = false };
            var errorMessageLaber = new Label() { Text = DummyText };

            InitializeControls(groupSubscribe, textBoxNames, smartGrid, pagerBuilder, refreshButton, optinHtmlSave, errorPlaceholder, errorMessageLaber);

            ReflectionHelper.SetField(groupSubscribe, SoSmartFormButtonName, new Button { Enabled = false });
            ReflectionHelper.SetField(groupSubscribe, DoSmartFormButtonName, new Button { Enabled = true });
            var htmlSaveNewButton = new Button() { Text = DummyText };
            ReflectionHelper.SetField(groupSubscribe, SoOptinHTMLSaveNewControlName, htmlSaveNewButton);

            // Act
            typeof(groupsubscribe)
                .CallMethod(
                    "SaveOptinHTML_Click",
                    new object[]
                    {
                        null,
                        null
                    },
                    groupSubscribe);

            // Assert
            smartFormsHistory.Verify(x => x.Save(It.IsAny<SmartFormsHistory>(), It.IsAny<User>()), Times.Never());
            smartFormsHistory.Verify(x => x.GetByGroupID(It.IsAny<int>(), It.IsAny<User>()), Times.Once());
            smartFormsHistory.Verify(x => x.GetBySmartFormID(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<User>()), Times.Once());

            Assert.That(refreshButton.Enabled, Is.False);
            Assert.That(optinHtmlSave.Enabled, Is.False);
            Assert.That(smartGrid.DataSource, Is.InstanceOf(typeof(IList)));
            var dataSource = smartGrid.DataSource as IList;
            Assert.That(dataSource, Is.Not.Null);
            Assert.That(dataSource.Count, Is.EqualTo(ValueZero));
            Assert.That(pagerBuilder.RecordCount, Is.EqualTo(ValueZero));
            AssertTextBoxs(groupSubscribe, textBoxNames, Is.Empty);

            Assert.That(errorPlaceholder.Visible, Is.True);
            Assert.That(errorMessageLaber.Text, Is.Empty);

            Assert.That(htmlSaveNewButton.Text, Is.EqualTo(CreateNewSmartForm));
        }

        [Test]
        public void GroupIdGetter_IfQueryStringContainsGroupId_ReturnsGroupId()
        {
            // Arrange
            _queryString.Add(GroupIdQueryStringKey, GroupId.ToString());

            // Act
            var returnedValue = _privateObject.GetProperty(GroupIdPropertyName) as int?;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBeOfType<int>(),
                () => returnedValue.ShouldBe(GroupId));
        }

        [Test]
        public void GroupIdGetter_IfQueryStringDoesNotContainGroupId_ReturnsDefaultValue()
        {
            // Arrange
            // set no groupId to query string

            // Act
            var returnedValue = _privateObject.GetProperty(GroupIdPropertyName) as int?;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBeOfType<int>(),
                () => returnedValue.ShouldBe(default(int)));
        }

        [Test]
        public void SFIDGetter_IfQueryStringContainsSFID_ReturnsSFID()
        {
            // Arrange
            _queryString.Add(SFIDQueryStringKey, SFID.ToString());

            // Act
            var returnedValue = _privateObject.GetProperty(SFIDPropertyName) as int?;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBeOfType<int>(),
                () => returnedValue.ShouldBe(SFID));
        }

        [Test]
        public void SFIDGetter_IfQueryStringDoesNotContainSFID_ReturnsDefaultValue()
        {
            // Arrange
            // set no sfid to query string

            // Act
            var returnedValue = _privateObject.GetProperty(SFIDPropertyName) as int?;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBeOfType<int>(),
                () => returnedValue.ShouldBe(default(int)));
        }

        [Test]
        public void RequestedActionGetter_IfQueryStringContainsAction_ReturnsAction()
        {
            // Arrange
            _queryString.Add(ActionQueryStringKey, Action);

            // Act
            var returnedValue = _privateObject.GetProperty(ActionPropertyName) as string;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBeOfType<string>(),
                () => returnedValue.ShouldBe(Action));
        }

        [Test]
        public void RequestedActionGetter_IfQueryStringDoesNotContainAction_ReturnsDefaultValue()
        {
            // Arrange
            // set no action to query string

            // Act
            var returnedValue = _privateObject.GetProperty(ActionPropertyName) as string;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBeOfType<string>(),
                () => returnedValue.ShouldBeNull());
        }

        private void InitializeControls(
            groupsubscribe groupSubscribe,
            string[] textBoxNames,
            DataGrid smartGrid,
            PagerBuilder pagerBuilder,
            Button refreshButton,
            Button optinHtmlSave,
            PlaceHolder errorPlaceholder,
            Label errorMessageLaber)
        {
            SetTextBoxFields(groupSubscribe, textBoxNames, new TextBox() { Text = DummyText });

            ReflectionHelper.SetField(groupSubscribe, SoOptinFieldSelectionListBoxName, new ListBox());
            ReflectionHelper.SetField(groupSubscribe, SoHTMLCodeControlName, new CKEditorControl());

            ReflectionHelper.SetField(groupSubscribe, SmartFormGridControlName, smartGrid);
            ReflectionHelper.SetField(groupSubscribe, GridPagerControlName, pagerBuilder);

            ReflectionHelper.SetField(groupSubscribe, SoRefreshHTMLControlName, refreshButton);
            ReflectionHelper.SetField(groupSubscribe, SoOptinHTMLSaveControlName, optinHtmlSave);

            ReflectionHelper.SetField(groupSubscribe, ErrorPlaceHolderControlName, errorPlaceholder);

            ReflectionHelper.SetField(groupSubscribe, ErrorMessageLabelControlName, errorMessageLaber);
        }

        private static Mock<IRequest> CreateRequest()
        {
            var request = new Mock<IRequest>();
            request.Setup(x => x.GetQueryStringValue(It.IsAny<string>())).Returns(string.Empty);
            return request;
        }

        private static Mock<ISmartFormsHistory> CreateSmartFormsHistory()
        {
            var smartFormsHistory = new Mock<ISmartFormsHistory>();
            var historyList = new List<SmartFormsHistory>();
            smartFormsHistory.Setup(x => x.GetByGroupID(It.IsAny<int>(), It.IsAny<User>())).Returns(historyList);
            smartFormsHistory.Setup(x => x.GetBySmartFormID(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<User>())).Returns(new SmartFormsHistory());

            return smartFormsHistory;
        }

        private static Mock<IMasterCommunicator> CreateMasterCommunicator()
        {
            var masterCommunicator = new Mock<IMasterCommunicator>();
            masterCommunicator.Setup(x => x.GetCustomerID()).Returns(ValueZero);
            return masterCommunicator;
        }

        private void AssertTextBoxs(groupsubscribe groupSubscribe, string[] textBoxNames, Constraint constraint)
        {
            foreach (var name in textBoxNames)
            {
                Assert.That((ReflectionHelper.GetField(groupSubscribe, name) as TextBox).Text, constraint);
            }
        }

        private void SetTextBoxFields(groupsubscribe groupSubscribe, string[] textBoxNames, TextBox textBox)
        {
            foreach (var name in textBoxNames)
            {
                ReflectionHelper.SetField(groupSubscribe, name, textBox);
            }
        }

        private void CreateShims()
        {
            var masterPage = new PageCommunicator();
            Shimgroupsubscribe.AllInstances.MasterGet = (x) => masterPage;
            ShimCommunicator.AllInstances.CurrentMenuCodeSetEnumsMenuCode = (x, y) => { };
            ShimMasterPageEx.AllInstances.HeadingSetString = (x, y) => { };
            ShimMasterPageEx.AllInstances.HelpContentSetString = (x, y) => { };
            ShimMasterPageEx.AllInstances.HelpTitleSetString = (x, y) => { };
            ShimUser.IsSystemAdministratorUser = (x) => true;
            Shimgroupsubscribe.AllInstances.SFIDGet = (x) => One;
            Shimgroupsubscribe.AllInstances.RequestedActionGet = (x) => Delete;
            Shimgroupsubscribe.AllInstances.GroupIdGet = (x) => One;
            ShimSmartFormsHistory.DeleteInt32User = (x, y) => { };
            Shimgroupsubscribe.AllInstances.SO_SmartFormButton_ClickObjectEventArgs = (x, y, z) => { };
            var dummyGroupDataField = new GroupDataFields();
            var groupDataFieldsList = new List<GroupDataFields> { dummyGroupDataField };
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (x, y, z) => groupDataFieldsList;
            ShimSmartFormsHistory.GetBySmartFormIDInt32Int32User = (x, y, z) => new SmartFormsHistory();
            ShimClient.HasServiceInt32EnumsServices = (x, y) => true;
        }

        private object GetGroupSubscribe()
        {
            var masterCommunicator = CreateMasterCommunicator();
            var smartFormsHistory = CreateSmartFormsHistory();
            smartFormsHistory.Setup(x => x.Save(It.IsAny<SmartFormsHistory>(), It.IsAny<User>()));
            var request = CreateRequest();
            var groupSubscribe = new groupsubscribe(masterCommunicator.Object, smartFormsHistory.Object, request.Object);
            InitializeAllControls(groupSubscribe);
            return groupSubscribe;
        }

        private void InitializeSession()
        {
            ShimECNSession.AllInstances.RefreshSession = (item) => { };
            ShimECNSession.AllInstances.ClearSession = (itme) => { };
            var config = new NameValueCollection();
            var reqParams = new NameValueCollection();
            var dummyUser = ReflectionHelper.CreateInstance(typeof(User));
            var authTkt = ReflectionHelper.CreateInstance(typeof(ECN_Framework_Entities.Application.AuthenticationTicket));
            var ecnSession = ReflectionHelper.CreateInstance(typeof(ECNSession));
            config.Add("KMCommon_Application", KMCommon_Application);
            _queryString.Add("HTTP_HOST", SampleHttpHost);
            _queryString.Add("CampaignItemTemplateID", CampaignItemTemplateID);
            dummyUser.UserID = UserID;
            ReflectionHelper.SetField(authTkt, "CustomerID", CustomerID);
            ReflectionHelper.SetField(ecnSession, "CurrentUser", dummyUser);
            ShimECNSession.CurrentSession = () => ecnSession;
            ShimAuthenticationTicket.getTicket = () => authTkt;
            ShimConfigurationManager.AppSettingsGet = () => config;
            ShimHttpRequest.AllInstances.UserAgentGet = (h) => SampleUserAgent;
            ShimHttpRequest.AllInstances.UserHostAddressGet = (h) => SampleHost;
            ShimHttpRequest.AllInstances.UrlReferrerGet = (h) => new Uri(SampleHostPath);
            ShimHttpRequest.AllInstances.ParamsGet = (x) => reqParams;
            ShimControl.AllInstances.ParentGet = (control) => new Page();
        }

        protected void InitializeAllControls(object page)
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

        private void InitializeQueryString()
        {
            _queryString = new NameValueCollection();
            ShimHttpRequest.AllInstances.QueryStringGet = (h) => _queryString;
        }

        private void InitializeHttpContext()
        {
            HttpContext.Current = MockHelpers.FakeHttpContext();
            ShimUserControl.AllInstances.RequestGet = (x) => HttpContext.Current.Request;
            ShimUserControl.AllInstances.ResponseGet = (x) => HttpContext.Current.Response;
            ShimPage.AllInstances.SessionGet = x => HttpContext.Current.Session;
            ShimPage.AllInstances.RequestGet = (x) => HttpContext.Current.Request;
        }

        private void TryLinkFieldWithPage(object field, object page)
        {
            if (page is Page)
            {
                var fieldType = field.GetType().GetField("_page", BindingFlags.Public |
                                                                  BindingFlags.NonPublic |
                                                                  BindingFlags.Static |
                                                                  BindingFlags.Instance);
                if (fieldType != null)
                {
                    try
                    {
                        fieldType.SetValue(field, page);
                    }
                    catch (Exception ex)
                    {
                        // ignored
                        Trace.TraceError($"Unable to set value as :{ex}");
                    }
                }
            }
        }
    }
}
