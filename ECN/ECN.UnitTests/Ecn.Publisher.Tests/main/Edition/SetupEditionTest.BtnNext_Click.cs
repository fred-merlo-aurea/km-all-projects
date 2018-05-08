using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.IO;
using System.Web.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.publisher.main.Edition.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_Common.Objects;
using KM.Common.Utilities.Email.Fakes;
using NUnit.Framework;
using Shouldly;
using PublisherFakes = ECN_Framework_BusinessLayer.Publisher.Fakes;

namespace Ecn.Publisher.Tests.main.Edition
{
    public partial class SetupEditionTest
    {
        private const string SampleFileName = "Sample.pdf";
        private const string LblErrorMessage = "lblErrorMessage";
        private const string ErrorPlaceHolder= "phError";
        private const string BtnNextClickMethodName = "btnNext_Click";
        private const string btnNextText = "Finish&nbsp;&raquo;";
        private const string SampleTestpath = "SampleTestpath";
        private const string SampleEdition = "SampleEdition";
        private const string SampleTotalPages = "5";
        private const string StepIndexKey = "StepIndex";
        private const string ViewStateProperty = "ViewState";
        private const string ImagesVirtualPathKey = "Images_VirtualPath";
        private const string ImageDomainPathKey = "Image_DomainPath";
        private const string AdminToEmailConfigurationKey = "DE_NOTIFICATION_TO_EMAIL";
        private const string AdminFromEmailConfigurationKey = "DE_NOTIFICATION_FROM_EMAIL";
        private const string SampleFromEmail = "test@test.com";
        private const string SampleToEmail = "sample@sample.com";
        private const string AdminEmailSubject = "Digital Edition has been uploaded.";

        [Test]
        public void BtnNext_Click_WhenNoEditionIDFileNameIsNotPdf_SetsErrorLabelAndReturns()
        {
            // Arrange
            SetFakesForBtnNextClickMethod(editionID: 0);
            SetPageControls();
            Get<Label>(_privateTestObject, "lbluploadedfile").Text = "SampleFileName";

            // Act
            _privateTestObject.Invoke(BtnNextClickMethodName, this, EventArgs.Empty);

            // Assert
            Get<PlaceHolder>(_privateTestObject, ErrorPlaceHolder).Visible.ShouldBeTrue();
            Get<Label>(_privateTestObject, LblErrorMessage).Text.ShouldContain("Invalid File Format. Please upload PDF file.");
        }

        [Test]
        public void BtnNext_Click_WhenNoEditionIDFileNameIsEmpty_SetsErrorLabelAndReturns()
        {
            // Arrange
            SetFakesForBtnNextClickMethod(editionID: 0);
            SetPageControls();
            Get<Label>(_privateTestObject, "lbluploadedfile").Text = string.Empty;

            // Act
            _privateTestObject.Invoke(BtnNextClickMethodName, this, EventArgs.Empty);

            // Assert
            Get<PlaceHolder>(_privateTestObject, ErrorPlaceHolder).Visible.ShouldBeTrue();
            Get<Label>(_privateTestObject, LblErrorMessage).Text.ShouldContain("Please upload the PDF file.");
        }
        
        [Test]
        public void BtnNext_Click_WhenEditionIDFileNameIsValid_SavesEditionAndSendsEmail()
        {
            // Arrange
            SetFakesForBtnNextClickMethod();
            SetPageControls();
            CreateTempPublisherDirectory();

            // Act
            _privateTestObject.Invoke(BtnNextClickMethodName, this, EventArgs.Empty);

            // Assert
            _isPdfConverted.ShouldBeTrue();
            _isStepLoaded.ShouldBeFalse();
            _savedEdition.ShouldNotBeNull();
            _savedEdition.ShouldSatisfyAllConditions(
                () => _savedEdition.EditionID.ShouldBe(TestEditionID),
                () => _savedEdition.EditionName.ShouldBe(SampleEdition),
                () => _savedEdition.Pages.ShouldBe(5),
                () => _savedEdition.PublicationID.ShouldBe(1),
                () => _savedEdition.FileName.ShouldBe(SampleFileName));
            _sendEmailMessage.ShouldNotBeNull();
            _sendEmailMessage.ShouldSatisfyAllConditions(
                () => _sendEmailMessage.From.ShouldBe(SampleFromEmail),
                () => _sendEmailMessage.Subject.ShouldBe(AdminEmailSubject),
                () => _sendEmailMessage.Body.ShouldContain(SampleBaseChannel),
                () => _sendEmailMessage.Body.ShouldContain(SampleEdition),
                () => _sendEmailMessage.Body.ShouldContain(SampleFileName));
        }

        [Test]
        public void BtnNext_Click_WhenSaveEditionThrowsException_SetsErrorLabelAndReturns()
        {
            // Arrange
            SetFakesForBtnNextClickMethod();
            SetPageControls();
            CreateTempPublisherDirectory();
            PublisherFakes.ShimEdition.SaveEditionUser = (edition, user) => throw new ECNException(
                new List<ECNError>
                {
                    new ECNError { ErrorMessage = "UT Exception" }
                });

            // Act
            _privateTestObject.Invoke(BtnNextClickMethodName, this, EventArgs.Empty);

            // Assert

            // Assert
            _isPdfConverted.ShouldBeTrue();
            _isStepLoaded.ShouldBeFalse();
            _savedEdition.ShouldBeNull();
            _sendEmailMessage.ShouldNotBeNull();
            _sendEmailMessage.ShouldSatisfyAllConditions(
                () => _sendEmailMessage.From.ShouldBe(SampleFromEmail),
                () => _sendEmailMessage.Subject.ShouldBe(AdminEmailSubject),
                () => _sendEmailMessage.Body.ShouldContain(SampleBaseChannel),
                () => _sendEmailMessage.Body.ShouldContain(SampleEdition),
                () => _sendEmailMessage.Body.ShouldContain(SampleFileName));
            Get<PlaceHolder>(_privateTestObject, ErrorPlaceHolder).Visible.ShouldBeTrue();
            Get<Label>(_privateTestObject, LblErrorMessage).Text.ShouldContain("UT Exception");
        }

        [Test]
        public void BtnNext_Click_WhenEditionIDIsZeroAndHaveFileName_SavesEditionAndSendsEmail()
        {
            // Arrange
            SetFakesForBtnNextClickMethod(editionID: 0);
            SetPageControls();
            CreateTempPublisherDirectory();
            
            // Act
            _privateTestObject.Invoke(BtnNextClickMethodName, this, EventArgs.Empty);

            // Assert
            _isPdfConverted.ShouldBeTrue();
            _isStepLoaded.ShouldBeFalse();
            _savedEdition.ShouldNotBeNull();
            _savedEdition.ShouldSatisfyAllConditions(
                () => _savedEdition.EditionID.ShouldBe(0),
                () => _savedEdition.EditionName.ShouldBe(SampleEdition),
                () => _savedEdition.Pages.ShouldBe(5),
                () => _savedEdition.PublicationID.ShouldBe(1),
                () => _savedEdition.FileName.ShouldBe(SampleFileName));
            _sendEmailMessage.ShouldNotBeNull();
            _sendEmailMessage.ShouldSatisfyAllConditions(
                () => _sendEmailMessage.From.ShouldBe(SampleFromEmail),
                () => _sendEmailMessage.Subject.ShouldBe(AdminEmailSubject),
                () => _sendEmailMessage.Body.ShouldContain(SampleBaseChannel),
                () => _sendEmailMessage.Body.ShouldContain(SampleEdition),
                () => _sendEmailMessage.Body.ShouldContain(SampleFileName));
        }

        [Test]
        public void BtnNext_Click_WhenSendEmailThrowsException_SetsErrorLabelAndReturns()
        {
            // Arrange
            SetFakesForBtnNextClickMethod();
            SetPageControls();
            CreateTempPublisherDirectory();
            ShimEmailService.AllInstances.SendEmailEmailMessageString = (e, emsg, server) => 
                throw new InvalidOperationException("UT Exception");

            // Act
            _privateTestObject.Invoke(BtnNextClickMethodName, this, EventArgs.Empty);

            // Assert
            _isPdfConverted.ShouldBeTrue();
            _isStepLoaded.ShouldBeFalse();
            _savedEdition.ShouldNotBeNull();
            _savedEdition.ShouldSatisfyAllConditions(
                () => _savedEdition.EditionID.ShouldBe(1),
                () => _savedEdition.EditionName.ShouldBe(SampleEdition),
                () => _savedEdition.Pages.ShouldBe(5),
                () => _savedEdition.PublicationID.ShouldBe(1),
                () => _savedEdition.FileName.ShouldBe(SampleFileName));
            _sendEmailMessage.ShouldBeNull();
            Get<PlaceHolder>(_privateTestObject, ErrorPlaceHolder).Visible.ShouldBeTrue();
            Get<Label>(_privateTestObject, LblErrorMessage).Text.ShouldContain("UT Exception");
        }

        [Test]
        public void BtnNext_Click_WhenBtnNextStepTextIsNext_LoadsNextStep()
        {
            // Arrange
            SetFakesForBtnNextClickMethod();
            SetPageControls();
            CreateTempPublisherDirectory();
            Get<LinkButton>(_privateTestObject, "btnNext1").Text = "Next";

            // Act
            _privateTestObject.Invoke(BtnNextClickMethodName, this, EventArgs.Empty);

            // Assert
            _isPdfConverted.ShouldBeFalse();
            _isStepLoaded.ShouldBeTrue();
            _savedEdition.ShouldBeNull();
            _sendEmailMessage.ShouldBeNull();
        }

        [Test]
        public void BtnNext_Click_WhenImagePathDirectoryAlreadyExists_MovesContentToDeletedDirectory()
        {
            // Arrange
            SetFakesForBtnNextClickMethod();
            SetPageControls();
            CreateTempPublisherDirectory();
            var fullpath = TestBasePath + ECNSession.CurrentSession().CurrentCustomer.CustomerID +
                "\\publisher\\" + TestEditionID;
            if (!Directory.Exists(fullpath))
            {
                Directory.CreateDirectory(fullpath);
            }

            // Act
            _privateTestObject.Invoke(BtnNextClickMethodName, this, EventArgs.Empty);

            // Assert
            _isPdfConverted.ShouldBeTrue();
            _isStepLoaded.ShouldBeFalse();
            _savedEdition.ShouldNotBeNull();
            _savedEdition.ShouldSatisfyAllConditions(
                () => _savedEdition.EditionID.ShouldBe(TestEditionID),
                () => _savedEdition.EditionName.ShouldBe(SampleEdition),
                () => _savedEdition.Pages.ShouldBe(5),
                () => _savedEdition.PublicationID.ShouldBe(1),
                () => _savedEdition.FileName.ShouldBe(SampleFileName));
            _sendEmailMessage.ShouldNotBeNull();
            _sendEmailMessage.ShouldSatisfyAllConditions(
                () => _sendEmailMessage.From.ShouldBe(SampleFromEmail),
                () => _sendEmailMessage.Subject.ShouldBe(AdminEmailSubject),
                () => _sendEmailMessage.Body.ShouldContain(SampleBaseChannel),
                () => _sendEmailMessage.Body.ShouldContain(SampleEdition),
                () => _sendEmailMessage.Body.ShouldContain(SampleFileName));
        }

        private void SetFakesForBtnNextClickMethod(int editionID = TestEditionID)
        {
            var viewState = (StateBag)_privateTestObject.GetProperty(ViewStateProperty);
            viewState.Add(EditionIDKey, editionID);
            viewState.Add(StepIndexKey, 1);

            var settings = new NameValueCollection();
            settings.Add(ImagesVirtualPathKey, TestContext.CurrentContext.TestDirectory);
            settings.Add(ImageDomainPathKey, SampleTestpath);
            settings.Add(AdminFromEmailConfigurationKey, SampleFromEmail);
            settings.Add(AdminToEmailConfigurationKey, SampleToEmail);
            ShimConfigurationManager.AppSettingsGet = () => settings;
            ShimPage.AllInstances.IsValidGet = (p) => true;

            ShimHttpServerUtility.AllInstances.MapPathString = (x, path) => path.Replace("/", "\\");

            ShimSetupEdition.AllInstances.ConvertDEString = (s, p) => { _isPdfConverted = true; };
            ShimSetupEdition.AllInstances.LoadStep = (s) => { _isStepLoaded = true; };
            PublisherFakes.ShimEdition.SaveEditionUser = (edition, user) =>
            {
                _savedEdition = edition;
                return _savedEdition.EditionID;
            };
            ShimEmailService.AllInstances.SendEmailEmailMessageString = (e, emsg, server) => 
            {
                _sendEmailMessage = emsg;
            };
        }

        private void SetPageControls()
        {
            Get<Label>(_privateTestObject, "lbluploadedfile").Text = SampleFileName;
            Get<Label>(_privateTestObject, "lblTotalPages").Text = SampleTotalPages;
            Get<TextBox>(_privateTestObject, "tbEditionName").Text = SampleEdition;
            Get<TextBox>(_privateTestObject, "tbActivationDate").Text = DateTime.UtcNow.Date.ToString();
            Get<TextBox>(_privateTestObject, "tbDeActivationDate").Text = DateTime.UtcNow.AddMonths(3).Date.ToString();
            Get<LinkButton>(_privateTestObject, "btnNext1").Text = btnNextText;

            var ddlPublicationList = Get<DropDownList>(_privateTestObject, "ddlPublicationList");
            ddlPublicationList.Items.Add(new ListItem { Value = "1", Selected = true });

            var ddlStatus = Get<DropDownList>(_privateTestObject, "ddlStatus");
            ddlStatus.Items.Add(new ListItem { Value = "1", Selected = true });

            var rbSecured = Get<RadioButtonList>(_privateTestObject, "rbSecured");
            rbSecured.Items.Add(new ListItem { Value = "1", Selected = true });
        }

        private void CreateTempPublisherDirectory()
        {
            var tempPath = TestBasePath +
                ECNSession.CurrentSession().CurrentCustomer.CustomerID + "\\Publisher\\Temp\\";
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }
            if (!File.Exists(Path.Combine(tempPath, SampleFileName)))
            {
                File.Create(Path.Combine(tempPath, SampleFileName)).Dispose();
            }
        }
    }
}
