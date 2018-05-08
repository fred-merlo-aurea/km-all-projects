using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Fakes;
using System.Reflection;
using System.Web;
using System.Web.Fakes;
using System.Web.UI.HtmlControls.Fakes;
using System.Web.UI.WebControls;
using ecn.publisher.main.Publication.Fakes;
using ecn.publisher.main.Publication;
using ecn.publisher.MasterPages.Fakes;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Publisher.Fakes;
using ECN.Common.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EntitiesPublication = ECN_Framework_Entities.Publisher.Publication;
using MasterPage = ecn.publisher.MasterPages.Publisher;
using PublicationFakes = ECN_Framework_BusinessLayer.Publisher.Fakes;
using NUnit.Framework;
using Shouldly;

namespace Ecn.Publisher.Tests.Publication
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class SetupPublicationTest : PageHelper
    {
        private const string MethodButtonNextClick = "lbtnNext_Click";
        private const string MethodPageLoad = "Page_Load";
        private const string TestUser = "TestUser";
        private const string SelectedOption = "rbSubOptn5";
        private const string NextButtonText = "Finish&nbsp;&raquo;";
        private const string DummyString = "dummystring";
        private const string DummyGif = "dummystring.gif";
        private const string LayoutValueString = "1";
        private const string One = "1";
        private const string DummyErrorMessage = "Invalid Image format. Select Only gif or jpg image.";
        private const string DummyLogoUrl = "LogoUrl";
        private const int LayoutId = 1;
        private const int SecondIndex = 2;
        private SetupPublication _testEntity;
        private PrivateObject _privateTestObject;
        private EntitiesPublication _publication;
        private IDisposable _context;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            _context = ShimsContext.Create();
            base.SetPageSessionContext();
            _testEntity = new SetupPublication { };
            _privateTestObject = new PrivateObject(_testEntity);
            InitializeAllControls(_testEntity);
            InitializeSessionFakes();
        }

        [TearDown]
        public void TearDwond()
        {
            _context.Dispose();
        }

        [TestCase("rbSubOptn1")]
        [TestCase("rbSubOptn2")]
        [TestCase("rbSubOptn3")]
        [TestCase("rbSubOptn4")]
        [TestCase("rbSubOptn5")]
        [TestCase("rbSubOptn6")]
        public void lbtnNext_Click_WhenFileNameIsEmpty_LogoURLIsSetToLabelLogoURL(string selectedOption)
        {
            // Arrange 
            Initialize(selectedOption);
            CreateShims();
            var PublicationSaved = false;
            ECN_Framework_BusinessLayer.Publisher.Fakes.ShimPublication.SavePublicationRefUser = (ref EntitiesPublication publication, User y) =>
            {
                _publication = publication;
                PublicationSaved = true;
            };

            // Act
            _privateTestObject.Invoke(MethodButtonNextClick, null, EventArgs.Empty);

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => _publication.LogoURL.ShouldNotBeNullOrWhiteSpace(),
                () => PublicationSaved.ShouldBeTrue(),
                () => _publication.LogoURL.ShouldBe(DummyLogoUrl));
        }

        [Test]
        public void lbtnNext_Click_WhenFileNameDoesNotEndsWithGIF_ErrorMessageIsDisplayed()
        {
            // Arrange 
            Initialize(SelectedOption);
            CreateShims();
            ShimHttpPostedFile.AllInstances.FileNameGet = (x) => DummyString;

            // Act
            _privateTestObject.Invoke(MethodButtonNextClick, null, EventArgs.Empty);

            // Asert
            var labelErrorMessage = ReflectionHelper.GetFieldValue(_testEntity, "lblErrorMessage") as Label;
            _testEntity.ShouldSatisfyAllConditions(
                () => labelErrorMessage.ShouldNotBeNull(),
                () => labelErrorMessage.Text.ShouldBe(DummyErrorMessage));
        }

        [Test]
        public void lbtnNext_Click_WhenFileNameEndsWithGIF_LogoURLContainsDummyString()
        {
            // Arrange 
            Initialize(SelectedOption);
            CreateShims();
            var publicationSaved = false;
            PublicationFakes.ShimPublication.SavePublicationRefUser = (ref EntitiesPublication publication, User y) =>
            {
                _publication = publication;
                publicationSaved = true;
            };
            ShimHttpPostedFile.AllInstances.FileNameGet = (x) => DummyGif;
            ShimDirectory.ExistsString = (x) => false;
            ShimDirectory.CreateDirectoryString = (x) => new DirectoryInfo(DummyString);
            ShimHttpPostedFile.AllInstances.SaveAsString = (x, y) => { };
            ShimPublication.AllInstances.PublicationIDGet = (x) => 0;

            // Act
            _privateTestObject.Invoke(MethodButtonNextClick, null, EventArgs.Empty);

            // Asert
            _testEntity.ShouldSatisfyAllConditions(
                () => publicationSaved.ShouldBeTrue(),
                () => _publication.LogoURL.Contains(DummyGif));
        }

        [Test]
        public void lbtnNext_Click_WhenErrorInSavingPublication_ErrorIsDisplayed()
        {
            // Arrange 
            Initialize(SelectedOption);
            CreateShims();
            ShimHttpPostedFile.AllInstances.FileNameGet = (x) => DummyGif;
            ShimDirectory.ExistsString = (x) => false;
            ShimDirectory.CreateDirectoryString = (x) => new DirectoryInfo(DummyString);
            ShimHttpPostedFile.AllInstances.SaveAsString = (x, y) => { };
            ShimPublication.AllInstances.PublicationIDGet = (x) => 0;
            var ecnError = new ECNError();
            var errorList = new List<ECNError> { ecnError };
            PublicationFakes.ShimPublication.SavePublicationRefUser = (ref EntitiesPublication publication, User y) => throw new ECNException(errorList);

            // Act
            _privateTestObject.Invoke(MethodButtonNextClick, null, EventArgs.Empty);

            // Asert
            var phError = ReflectionHelper.GetFieldValue(_testEntity, "phError") as PlaceHolder;
            phError.Visible.ShouldBeTrue();
        }

        [Test]
        public void lbtnNext_Click_WhenPublicationAlreadyExits_ErrorIsDisplayed()
        {
            // Arrange 
            Initialize(SelectedOption);
            CreateShims();
            ReflectionHelper.SetField(_testEntity, "lbtnNext1", new LinkButton { Text = DummyString });
            ShimSetupPublication.AllInstances.StepIndexGet = (x) => 1;
            PublicationFakes.ShimPublication.ExistsInt32StringInt32 = (a, b, c) => true;

            // Act
            _privateTestObject.Invoke(MethodButtonNextClick, null, EventArgs.Empty);

            // Asert
            var phError = ReflectionHelper.GetFieldValue(_testEntity, "phError") as PlaceHolder;
            phError.Visible.ShouldBeTrue();
        }

        [Test]
        public void lbtnNext_Click_WhenPublicationURLAlreadyExits_ErrorIsDisplayed()
        {
            // Arrange 
            Initialize(SelectedOption);
            CreateShims();
            ReflectionHelper.SetField(_testEntity, "lbtnNext1", new LinkButton { Text = DummyString });
            ShimSetupPublication.AllInstances.StepIndexGet = (x) => 1;
            PublicationFakes.ShimPublication.ExistsInt32StringInt32 = (a, b, c) => false;
            PublicationFakes.ShimPublication.ExistsAliasInt32String = (a, b) => true;
            ReflectionHelper.SetField(_testEntity, "tbPublicationAlias", new TextBox { Text = DummyString });

            // Act
            _privateTestObject.Invoke(MethodButtonNextClick, null, EventArgs.Empty);

            // Asert
            var phError = ReflectionHelper.GetFieldValue(_testEntity, "phError") as PlaceHolder;
            phError.Visible.ShouldBeTrue();
        }

        [Test]
        public void lbtnNext_Click_WhenStepIndexIsGreaterThanCompletedStep_CompletedStepIsSetToStepIndex()
        {
            // Arrange 
            Initialize(SelectedOption);
            CreateShims();
            ReflectionHelper.SetField(_testEntity, "lbtnNext1", new LinkButton { Text = DummyString });
            ShimSetupPublication.AllInstances.StepIndexGet = (x) => SecondIndex;

            // Act
            _privateTestObject.Invoke(MethodButtonNextClick, null, EventArgs.Empty);

            // Asert
            var completedIndex = (int)ReflectionHelper.GetProperty(_testEntity, "CompletedStep");
            completedIndex.ShouldBe(SecondIndex);
        }

        [TestCase(1, false, "pnl1")]
        [TestCase(2, true, "pnl2")]
        [TestCase(3, true, "pnl3")]
        public void lbtnNext_Click_WhenStepIndexChanges_PropertiesAreInitialized(int stepIndex, bool visiblility, string panelName)
        {
            // Arrange 
            Initialize(SelectedOption);
            CreateShims();
            ReflectionHelper.SetField(_testEntity, "lbtnNext1", new LinkButton { Text = DummyString });
            ShimSetupPublication.AllInstances.StepIndexGet = (x) => stepIndex;
            PublicationFakes.ShimPublication.ExistsInt32StringInt32 = (a, b, c) => false;
            ReflectionHelper.SetField(_testEntity, "tbPublicationAlias", new TextBox { Text = string.Empty });

            // Act
            _privateTestObject.Invoke(MethodButtonNextClick, null, EventArgs.Empty);

            // Asert
            var stepsPanel = ReflectionHelper.GetFieldValue(_testEntity, panelName) as Panel;
            var buttonPrevious1 = ReflectionHelper.GetFieldValue(_testEntity, "lbtnPrevious1") as LinkButton;
            var buttonPrevious2 = ReflectionHelper.GetFieldValue(_testEntity, "lbtnPrevious2") as LinkButton;
            _testEntity.ShouldSatisfyAllConditions(
                () => stepsPanel.ShouldNotBeNull(),
                () => buttonPrevious1.ShouldNotBeNull(),
                () => buttonPrevious2.ShouldNotBeNull(),
                () => stepsPanel.Visible.ShouldBeTrue(),
                () => buttonPrevious1.Visible.ShouldBe(visiblility),
                () => buttonPrevious2.Visible.ShouldBe(visiblility));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void Page_Load_Success_ControlsAreInitialized(int subscriptionOption)
        {
            // Arrange 
            Initialize(SelectedOption);
            CreateShims();
            CreateShimsForPageLoad();
            var dummyPublication = ReflectionHelper.CreateInstance(typeof(EntitiesPublication));
            dummyPublication.SubscriptionOption = subscriptionOption;
            PublicationFakes.ShimPublication.GetByPublicationIDInt32User = (x, y) => dummyPublication;

            // Act
            _privateTestObject.Invoke(MethodPageLoad, null, EventArgs.Empty);

            // Asert
            var address = ReflectionHelper.GetFieldValue(_testEntity, "tbContactAddress1") as TextBox;
            var email = ReflectionHelper.GetFieldValue(_testEntity, "tbContactEmail") as TextBox;
            var phone = ReflectionHelper.GetFieldValue(_testEntity, "tbContactPhone") as TextBox;
            _testEntity.ShouldSatisfyAllConditions(
                () => address.ShouldNotBeNull(),
                () => email.ShouldNotBeNull(),
                () => phone.ShouldNotBeNull(),
                () => address.Text.ShouldNotBeNullOrWhiteSpace(),
                () => email.Text.ShouldNotBeNullOrWhiteSpace(),
                () => phone.Text.ShouldNotBeNullOrWhiteSpace());
        }

        private void CreateShimsForPageLoad()
        {
            ShimSetupPublication.AllInstances.MasterGet = (x) => new MasterPage();
            ShimPublisher.AllInstances.CurrentMenuCodeSetEnumsMenuCode = (x, y) => { };
            ShimMasterPageEx.AllInstances.SubMenuSetString = (x, y) => { };
            ShimMasterPageEx.AllInstances.HelpContentSetString = (x, y) => { };
            ShimMasterPageEx.AllInstances.HelpTitleSetString = (x, y) => { };
            ShimMasterPageEx.AllInstances.HeadingSetString = (x, y) => { };
            var category = new ECN_Framework_Entities.Publisher.Category();
            var categoryList = new List<ECN_Framework_Entities.Publisher.Category> { category };
            PublicationFakes.ShimCategory.GetAll = () => categoryList;
            var frequency = new ECN_Framework_Entities.Publisher.Frequency();
            var frequencyList = new List<ECN_Framework_Entities.Publisher.Frequency> { frequency };
            PublicationFakes.ShimFrequency.GetAll = () => frequencyList;
            ShimSetupPublication.AllInstances.PublicationID = (x) => 1;
        }
        private void Initialize(string selectedOption)
        {
            ReflectionHelper.SetField(_testEntity, "lbtnNext1", new LinkButton { Text = NextButtonText });
            ReflectionHelper.SetField(_testEntity, "lblGroupID", new Label { Text = One });
            ReflectionHelper.SetField(_testEntity, selectedOption, new RadioButton { Checked = true });
            ReflectionHelper.SetField(_testEntity, "lblogoURL", new Label { Text = DummyLogoUrl });
            ReflectionHelper.SetField(_testEntity, "ddlPublicationType", new DropDownList
            {
                Items =
                {
                    new ListItem
                    {
                        Selected = true,
                        Value = DummyString
                    }
                }
            });
            ReflectionHelper.SetField(_testEntity, "ddlCategory", new DropDownList
            {
                Items =
                {
                    new ListItem
                    {
                        Selected = true,
                        Value = One
                    }
                }
            });
            ReflectionHelper.SetField(_testEntity, "ddlFrequency", new DropDownList
            {
                Items =
                {
                    new ListItem
                    {
                        Selected = true,
                        Value = One
                    }
                }
            });
        }

        private void CreateShims()
        {
            System.Web.UI.Fakes.ShimPage.AllInstances.IsValidGet = (x) => true;
            ShimHttpPostedFile.AllInstances.FileNameGet = (x) => string.Empty;
            ShimPublication.AllInstances.PublicationIDGet = (x) => 1;
            ShimSetupPublication.AllInstances.MasterGet = (x) => new MasterPage();
            var constructorInfo = typeof(HttpPostedFile).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)[0];
            var dummyPostedFile = (HttpPostedFile)constructorInfo
                      .Invoke(new object[] { "filename", "image/jpeg", null });
            ShimHtmlInputFile.AllInstances.PostedFileGet = (x) => dummyPostedFile;
        }

        private void InitializeSessionFakes()
        {
            var shimSession = new ShimECNSession();
            var client = ReflectionHelper.CreateInstance(typeof(Client));
            shimSession.Instance.CurrentUser = new User()
            {
                UserID = 1,
                UserName = TestUser,
                IsActive = true,
                CurrentClient = client
            };
            shimSession.Instance.CurrentBaseChannel = new BaseChannel { BaseChannelID = 1 };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
        }
    }
}