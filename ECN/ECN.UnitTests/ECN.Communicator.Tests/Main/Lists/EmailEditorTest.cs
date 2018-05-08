using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web;
using System.Web.UI.WebControls;
using ecn.communicator.listsmanager;
using ecn.communicator.listsmanager.Fakes;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Activity.View.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_Entities.Accounts;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using ActiveUpPagerBuilder = ActiveUp.WebControls.PagerBuilder;
using EntitiesBlastActivity = ECN_Framework_Entities.Activity.View.BlastActivity;
using NUnit.Framework;
using Shouldly;
using ReflectionHelper = ECN.Communicator.Tests.Helpers.ReflectionHelper;
using MasterPages = ecn.communicator.MasterPages;

namespace ECN.Communicator.Tests.Main.Lists
{
    /// <summary>
    ///     Unit tests for <see cref="ecn.communicator.listsmanager.emaileditor"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class emaileditorTest : BasePageTests
    {
        private const int CustomerID = 1;
        private const int UserID = 1;
        private const int SetEmailId = 1;
        private const int GroupId = 1;
        private const string Host = "http://km.com";
        private const string DummyString = "dummyString";
        private const string MethodLoadFormData = "LoadFormData";
        private const string MethodViewNotes = "ViewNotes";
        private const string MethodViewDetails = "ViewDetails";
        private const string MethodViewLog = "ViewLog";
        private const string MethodSetECNError = "setECNError";

        private object[] _methodArgs;
        private emaileditor _testObject;
        private emaileditor _page;

        private IDisposable _shimObject;
        private Email _email;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            _page = new emaileditor();
            InitializePage(_page);

            SetupFakes();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void LoadFormData_Success_RespectiveFieldsContainData()
        {
            // Arrange
            InitializeFakes();
            InitilizeTestObject();
            InitializePropertiesAndFields();
            _methodArgs = new object[] { SetEmailId, GroupId };

            //Act
            CallMethod(typeof(emaileditor), MethodLoadFormData, _methodArgs, _testObject);

            // Assert
            AssertPropertiesAndFields();
        }

        [Test]
        public void LoadFormData_WhenEmailNotExist_ErrorIsShown()
        {
            // Arrange
            InitializeFakes();
            InitilizeTestObject();
            InitializePropertiesAndFields();
            _methodArgs = new object[] { SetEmailId, GroupId };
            ShimEmail.GetByEmailIDGroupIDInt32Int32User = (setEmailID, groupID, user) => null;

            //Act
            CallMethod(typeof(emaileditor), MethodLoadFormData, _methodArgs, _testObject);

            // Assert
            var expectedErrorString = "EmailID: 1 has been removed";
            var errorMessage = (GetField(_testObject, "lblErrorMessage") as Label).Text;
            var errorShown = (GetField(_testObject, "phError") as PlaceHolder).Visible;
            _testObject.ShouldSatisfyAllConditions(
                () => errorMessage.ShouldNotBeNullOrWhiteSpace(),
                () => errorMessage.ShouldContain(expectedErrorString),
                () => errorShown.ShouldBeTrue());
        }

        [Test]
        public void ViewNotes_Success_NotesPanelIsShown()
        {
            // Arrange
            InitializeFakes();
            InitilizeTestObject();
            InitializePropertiesAndFields();
            _methodArgs = new object[] { null, EventArgs.Empty };

            //Act
            CallMethod(typeof(emaileditor), MethodViewNotes, _methodArgs, _testObject);

            // Assert
            var notesPanelShown = (GetField(_testObject, "NotesPanel") as Panel).Visible;
            var logPanelShown = (GetField(_testObject, "LogPanel") as Panel).Visible;
            var detailsPanelShown = (GetField(_testObject, "DetailsPanel") as Panel).Visible;
            _testObject.ShouldSatisfyAllConditions(
                () => logPanelShown.ShouldBeFalse(),
                () => detailsPanelShown.ShouldBeFalse(),
                () => notesPanelShown.ShouldBeTrue());
        }

        [Test]
        public void ViewDetails_Success_DetailsPanelIsShown()
        {
            // Arrange
            InitializeFakes();
            InitilizeTestObject();
            InitializePropertiesAndFields();
            _methodArgs = new object[] { null, EventArgs.Empty };
            Shimemaileditor.AllInstances.LoadFormDataInt32Int32 = (x, y, z) => { };

            //Act
            CallMethod(typeof(emaileditor), MethodViewDetails, _methodArgs, _testObject);

            // Assert
            var notesPanelShown = (GetField(_testObject, "NotesPanel") as Panel).Visible;
            var logPanelShown = (GetField(_testObject, "LogPanel") as Panel).Visible;
            var detailsPanelShown = (GetField(_testObject, "DetailsPanel") as Panel).Visible;
            _testObject.ShouldSatisfyAllConditions(
                () => logPanelShown.ShouldBeFalse(),
                () => detailsPanelShown.ShouldBeTrue(),
                () => notesPanelShown.ShouldBeFalse());
        }

        [Test]
        public void ViewLog_Success_DetailsPanelIsShown()
        {
            // Arrange
            InitializeFakes();
            InitilizeTestObject();
            InitializePropertiesAndFields();
            _methodArgs = new object[] { null, EventArgs.Empty };
            ShimBlastActivity.GetByEmailIDInt32User = (emailId, user) =>
            new List<EntitiesBlastActivity>()
            {
                CreateInstance(typeof(EntitiesBlastActivity))
            };

            //Act
            CallMethod(typeof(emaileditor), MethodViewLog, _methodArgs, _testObject);

            // Assert
            var notesPanelShown = (GetField(_testObject, "NotesPanel") as Panel).Visible;
            var logPanelShown = (GetField(_testObject, "LogPanel") as Panel).Visible;
            var detailsPanelShown = (GetField(_testObject, "DetailsPanel") as Panel).Visible;
            _testObject.ShouldSatisfyAllConditions(
                () => logPanelShown.ShouldBeTrue(),
                () => detailsPanelShown.ShouldBeFalse(),
                () => notesPanelShown.ShouldBeFalse());
        }

        [Test]
        public void SetECNError_Success_DetailsPanelIsShown()
        {
            // Arrange
            InitializeFakes();
            InitilizeTestObject();
            InitializePropertiesAndFields();
            var error = new ECNError
            {
                ErrorMessage = DummyString
            };
            var errorList = new List<ECNError>
            {
                error
            };
            var ecnException = new ECNException(errorList);
            _methodArgs = new object[] { ecnException };

            //Act
            CallMethod(typeof(emaileditor), MethodSetECNError, _methodArgs, _testObject);

            // Assert
            var ecnError = (GetField(_testObject, "lblErrorMessage") as Label).Text;
            var ecnErrorShown = (GetField(_testObject, "phError") as PlaceHolder).Visible;
            _testObject.ShouldSatisfyAllConditions(
                () => ecnError.ShouldContain(DummyString),
                () => ecnErrorShown.ShouldBeTrue());
        }

        private void InitializeFakes()
        {
            var context = new HttpContext(
                new HttpRequest(string.Empty, Host, string.Empty),
                new HttpResponse(TextWriter.Null));
            ShimHttpContext.CurrentGet = () => { return context; };
            ShimPage.AllInstances.RequestGet = (p) =>
            {
                return new HttpRequest(string.Empty, "http://km.com", string.Empty);
            };

            ShimHttpRequest.AllInstances.QueryStringGet = (r) =>
            {
                return new NameValueCollection
                {
                    {"GroupID", string.Format("{0}", GroupId) }
                };
            };
            _email = CreateInstance(typeof(Email));
            ShimEmail.GetByEmailIDGroupIDInt32Int32User = (setEmailID, groupID, user) => _email;
            ShimEmail.GetByEmailIDInt32User = (setEmailID, user) => _email;
            ShimGroup.GetByGroupIDInt32User = (groupID, user) => CreateInstance(typeof(Group));
            Shimemaileditor.AllInstances.MasterGet = (x) => new ecn.communicator.MasterPages.Communicator();
            ShimECNSession.CurrentSession = () =>
            {
                var dummyCustormer = CreateInstance(typeof(Customer));
                dummyCustormer.CustomerID = CustomerID;
                var dummyUser = CreateInstance(typeof(User));
                dummyUser.UserID = UserID;
                var ecnSession = CreateInstance(typeof(ECNSession));
                SetField(ecnSession, "CurrentUser", dummyUser);
                SetField(ecnSession, "CurrentCustomer", dummyCustormer);
                return ecnSession;
            };
            ShimAuthenticationTicket.getTicket = () =>
            {
                var authTkt = CreateInstance(typeof(AuthenticationTicket));
                SetField(authTkt, "CustomerID", CustomerID);
                return authTkt;
            };
            ShimECNSession.AllInstances.RefreshSession = (item) => { };
            ShimECNSession.AllInstances.ClearSession = (itme) => { };
        }

        private void InitializePropertiesAndFields()
        {
            SetField(_testObject, "EmailID", new TextBox
            {
                Text = string.Format("{0}", SetEmailId)
            });
            SetField(_testObject, "EmailAddress", new TextBox());
            SetField(_testObject, "Password", new TextBox());
            SetField(_testObject, "txtTitle", new TextBox());
            SetField(_testObject, "FirstName", new TextBox());
            SetField(_testObject, "LastName", new TextBox());
            SetField(_testObject, "FullName", new TextBox());
            SetField(_testObject, "CompanyName", new TextBox());
            SetField(_testObject, "Occupation", new TextBox());
            SetField(_testObject, "Address", new TextBox());
            SetField(_testObject, "Address2", new TextBox());
            SetField(_testObject, "City", new TextBox());
            SetField(_testObject, "State", new TextBox());
            SetField(_testObject, "Zip", new TextBox());
            SetField(_testObject, "Country", new TextBox());
            SetField(_testObject, "Voice", new TextBox());
            SetField(_testObject, "Mobile", new TextBox());
            SetField(_testObject, "Fax", new TextBox());
            SetField(_testObject, "Website", new TextBox());
            SetField(_testObject, "Age", new TextBox());
            SetField(_testObject, "Income", new TextBox());
            SetField(_testObject, "Gender", new TextBox());
            SetField(_testObject, "User1", new TextBox());
            SetField(_testObject, "User2", new TextBox());
            SetField(_testObject, "User3", new TextBox());
            SetField(_testObject, "User4", new TextBox());
            SetField(_testObject, "User5", new TextBox());
            SetField(_testObject, "User6", new TextBox());
            SetField(_testObject, "UserEvent1", new TextBox());
            SetField(_testObject, "UserEvent2", new TextBox());
            SetField(_testObject, "BounceScore", new TextBox());
            SetField(_testObject, "NotesBox", new TextBox());
            SetField(_testObject, "txtSoftBounceScore", new TextBox());
            SetField(_testObject, "phError", new PlaceHolder());
            SetField(_testObject, "lblErrorMessage", new Label());
            SetField(_testObject, "FormatTypeCode", new DropDownList
            {
                Items =
                {
                    new ListItem
                    {
                        Selected = true,
                        Value = _email.FormatTypeCode.ToLower()
                    }
                }
            });
            SetField(_testObject, "SubscribeTypeCode", new DropDownList
            {
                Items =
                {
                    new ListItem
                    {
                        Selected = true,
                        Value = _email.SubscribeTypeCode.ToUpper()
                    }
                }
            });
            SetField(_testObject, "DetailsPanel", new Panel());
            SetField(_testObject, "NotesPanel", new Panel());
            SetField(_testObject, "LogPanel", new Panel());
            SetField(_testObject, "DetailsButton", new Button());
            SetField(_testObject, "NotesButton", new Button());
            SetField(_testObject, "LogButton", new Button());
            SetField(_testObject, "LogGrid", new DataGrid());
            SetField(_testObject, "LogPager", new ActiveUpPagerBuilder());
            SetField(_testObject, "BirthDate", new TextBox());
        }

        private void AssertPropertiesAndFields()
        {
            var address = (GetField(_testObject, "Address") as TextBox).Text;
            var emailAddress = (GetField(_testObject, "EmailAddress") as TextBox).Text;
            var password = (GetField(_testObject, "Password") as TextBox).Text;
            var firstName = (GetField(_testObject, "FirstName") as TextBox).Text;
            var lastName = (GetField(_testObject, "LastName") as TextBox).Text;
            _testObject.ShouldSatisfyAllConditions(
                () => address.ShouldNotBeNullOrWhiteSpace(),
                () => address.ShouldBe(_email.Address),
                () => emailAddress.ShouldNotBeNullOrWhiteSpace(),
                () => emailAddress.ShouldBe(_email.EmailAddress),
                () => password.ShouldNotBeNullOrWhiteSpace(),
                () => password.ShouldBe(_email.Password),
                () => firstName.ShouldNotBeNullOrWhiteSpace(),
                () => firstName.ShouldBe(_email.FirstName),
                () => lastName.ShouldNotBeNullOrWhiteSpace(),
                () => lastName.ShouldBe(_email.LastName));
        }

        private void InitilizeTestObject()
        {
            _testObject = CreateInstance(typeof(emaileditor));
        }

        private dynamic CreateInstance(Type type)
        {
            return type.CreateInstance();
        }

        private void CallMethod(Type type, string methodName, object[] parametersValues, object instance = null)
        {
            ReflectionHelper.CallMethod(type, methodName, parametersValues, instance);
        }

        private void SetField(dynamic obj, string fieldName, dynamic fieldValue)
        {
            ReflectionHelper.SetField(obj, fieldName, fieldValue);
        }

        private dynamic GetField(dynamic obj, string fieldName)
        {
            return ReflectionHelper.GetField(obj, fieldName);
        }

        private void SetupFakes()
        {
            Shimemaileditor.AllInstances.isEmailSuppressedString = (_, email) =>
             {
                 email.ShouldBe(EmailAddress);
                 return false;
             };

            SetupEmailFakes();
            SetupPageFakes();
        }

        private static void SetupPageFakes()
        {
            ShimECNSession.CurrentSession = () =>
            {
                ECNSession session = typeof(ECNSession).CreateInstance();
                session.CurrentUser = new User
                {
                    CustomerID = CustomerId
                };

                return session;
            };

            ShimPage.AllInstances.MasterGet = _ => new MasterPages.Communicator();
        }
    }
}
