using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Linq;
using System.Web.UI.WebControls;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using KM.Common.Entity;
using KM.Common.Entity.Fakes;
using NUnit.Framework;
using Shouldly;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;

namespace ecn.activityengines.Tests.Engines
{
    public partial class UpdateEmailAddressTest
    {
        private const string SampleEmailAddress = "test@test.com";
        private const string KeyBCId = "bcid";
        private const string SampleDisplay = "SampleDisplay";
        private const string KeyActivity_DomainPath = "Activity_DomainPath";
        private const string MvcActivityDomainPathKeyValue = "MVCActivity_DomainPath";
        private const string KeyKMCommon_Application = "KMCommon_Application";
        private const string TxtOldEmailControl = "txtOldEmail";
        private const string TxtReEnterControl = "txtReEnter";
        private const string TxtNewEmailControl = "txtNewEmail";
        private const string LiteralConfirmation = "litConfirmation";
        private const string ConfirmationPanel = "pnlConfirmation";
        private const string MainPanel = "pnlMain";
        private const string PanelReEnter = "pnlReEnter";
        private const string ErrorPlaceHolder = "phError";
        private const string ErrorMessageLabel = "lblErrorMessage";
        private const string CustomerIDField = "CustomerID";
        private const string UTException= "UT Exception";
        private const string HardErrorExceptionMessage = "LandingPage: Sorry! We're having trouble processing your request right now.";
        private const string BtnSubmitClickMethodName = "btnSubmit_Click";
        private string _exceptionLogText;
        private bool _isExceptionLogged;
        private bool _isEmailDirectSaved;
        private CommunicatorEntities.EmailDirect _savedEmailDirect;

        [Test]
        public void btnSubmit_Click_WhenOldEmailIsNotValid_LogECNException()
        {
            // Arrange
            SetFakesForBtnSubmitMethod();
            SetUpPageControls();
            ShimEmail.IsValidEmailAddressString = _ => false;
            
            // Act
            _privateTestObject.Invoke(BtnSubmitClickMethodName, this, EventArgs.Empty);

            // Assert
            _privateTestObject.ShouldSatisfyAllConditions(
                () => Get<PlaceHolder>(_privateTestObject,ErrorPlaceHolder).Visible.ShouldBeTrue(),
                () => Get<Label>( _privateTestObject, ErrorMessageLabel).Text.ShouldContain("Old Email address is not a valid email"));
        }

        [Test]
        public void btnSubmit_Click_WhenNewEmailIsNotValid_LogECNException()
        {
            // Arrange
            const string SampleNewEmail = "new@test.com";
            SetFakesForBtnSubmitMethod();
            SetUpPageControls();
            Get<TextBox>(_privateTestObject, TxtNewEmailControl).Text = SampleNewEmail;
            ShimEmail.IsValidEmailAddressString = email => 
            {
                return email.Equals(SampleNewEmail) ? false : true;
            };

            // Act
            _privateTestObject.Invoke(BtnSubmitClickMethodName, this, EventArgs.Empty);

            // Assert
            _privateTestObject.ShouldSatisfyAllConditions(
                () => Get<PlaceHolder>(_privateTestObject, ErrorPlaceHolder).Visible.ShouldBeTrue(),
                () => Get<Label>(_privateTestObject, ErrorMessageLabel).Text.ShouldContain("New Email address is not a valid email"));
        }

        [Test]
        public void btnSubmit_Click_WhenOldEmailDoesNotExist_LogECNException()
        {
            // Arrange
            SetFakesForBtnSubmitMethod();
            SetUpPageControls();
            ShimEmail.IsValidEmailAddressString = _ => true;
            ShimEmail.Exists_BaseChannelStringInt32 = (_,__) => false;

            // Act
            _privateTestObject.Invoke(BtnSubmitClickMethodName, this, EventArgs.Empty);

            // Assert
            _privateTestObject.ShouldSatisfyAllConditions(
                () => Get<PlaceHolder>(_privateTestObject, ErrorPlaceHolder).Visible.ShouldBeTrue(),
                () => Get<Label>(_privateTestObject, ErrorMessageLabel).Text.ShouldContain("Old email address doesn't exist"));
        }

        [Test]
        public void btnSubmit_Click_WhenNewEmailDoesNotMatchReEnter_LogECNException()
        {
            // Arrange
            SetFakesForBtnSubmitMethod();
            SetUpPageControls();
            Get<TextBox>(_privateTestObject, TxtReEnterControl).Text = string.Empty;

            // Act
            _privateTestObject.Invoke(BtnSubmitClickMethodName, this, EventArgs.Empty);

            // Assert
            _privateTestObject.ShouldSatisfyAllConditions(
                () => Get<PlaceHolder>(_privateTestObject, ErrorPlaceHolder).Visible.ShouldBeTrue(),
                () => Get<Label>(_privateTestObject, ErrorMessageLabel).Text.ShouldContain("New email addresses don't match"));
        }

        [Test]
        public void btnSubmit_Click_WhenNewEmailIsSuppressed_LogECNException()
        {
            // Arrange
            SetFakesForBtnSubmitMethod();
            SetUpPageControls();
            ShimChannelMasterSuppressionList.GetByEmailAddressInt32StringUser = (_, __, ___) => 
            new List<CommunicatorEntities.ChannelMasterSuppressionList>
            {
                new CommunicatorEntities.ChannelMasterSuppressionList()
            };

            // Act
            _privateTestObject.Invoke(BtnSubmitClickMethodName, this, EventArgs.Empty);

            // Assert
            _privateTestObject.ShouldSatisfyAllConditions(
                () => Get<PlaceHolder>(_privateTestObject, ErrorPlaceHolder).Visible.ShouldBeTrue(),
                () => Get<Label>(_privateTestObject, ErrorMessageLabel).Text.
                            ShouldContain("New email address is Channel Master Suppressed. Updating is not allowed."));
        }

        [Test]
        public void btnSubmit_Click_WhenNewEmailIsValidAndSaved_SavesNewEmail()
        {
            // Arrange
            SetFakesForBtnSubmitMethod();
            SetUpPageControls();
            _privateTestObject.SetFieldOrProperty(CustomerIDField, 1);

            // Act
            _privateTestObject.Invoke(BtnSubmitClickMethodName, this, EventArgs.Empty);

            // Assert
            _privateTestObject.ShouldSatisfyAllConditions(
                () => _isEmailDirectSaved.ShouldBeTrue(),
                () => _savedEmailDirect.ShouldNotBeNull(),
                () => Get<Literal>(_privateTestObject, LiteralConfirmation).Text.ShouldBe(
                         GetDefaultAssignContentList().FirstOrDefault(x => x.LPOID == 24).Display),
                () => Get<Panel>(_privateTestObject, ConfirmationPanel).Visible.ShouldBeTrue(),
                () => Get<Panel>(_privateTestObject, MainPanel).Visible.ShouldBeFalse(),
                () => Get<PlaceHolder>(_privateTestObject, ErrorPlaceHolder).Visible.ShouldBeFalse());
        }

        [Test]
        public void btnSubmit_Click_WhenGetLPAThrowsException_LogsException()
        {
            // Arrange
            SetFakesForBtnSubmitMethod();
            SetUpPageControls();
            ShimLandingPageAssign.GetByBaseChannelIDInt32 = (_) => throw new InvalidOperationException(UTException);

            // Act
            _privateTestObject.Invoke(BtnSubmitClickMethodName, this, EventArgs.Empty);

            // Assert
            _privateTestObject.ShouldSatisfyAllConditions(
                () => _isExceptionLogged.ShouldBeTrue(),
                () => _exceptionLogText.ShouldNotBeNullOrWhiteSpace(),
                () => _exceptionLogText.ShouldContain(UTException),
                () => Get<Panel>(_privateTestObject, ConfirmationPanel).Visible.ShouldBeFalse(),
                () => Get<Panel>(_privateTestObject, MainPanel).Visible.ShouldBeFalse(),
                () => Get<PlaceHolder>(_privateTestObject, ErrorPlaceHolder).Visible.ShouldBeTrue(),
                () => Get<Label>(_privateTestObject, ErrorMessageLabel).Text.ShouldContain(HardErrorExceptionMessage));
        }

        [Test]
        public void btnSubmit_Click_WhenGetEncryptionThrowsException_LogsException()
        {
            // Arrange
            SetFakesForBtnSubmitMethod();
            SetUpPageControls();
            ShimEncryption.GetCurrentByApplicationIDInt32 = (appId) => throw new InvalidOperationException(UTException);

            // Act
            _privateTestObject.Invoke(BtnSubmitClickMethodName, this, EventArgs.Empty);

            // Assert
            _privateTestObject.ShouldSatisfyAllConditions(
                () => _isExceptionLogged.ShouldBeTrue(),
                () => _exceptionLogText.ShouldNotBeNullOrWhiteSpace(),
                () => _exceptionLogText.ShouldContain(UTException),
                () => Get<Panel>(_privateTestObject, ConfirmationPanel).Visible.ShouldBeFalse(),
                () => Get<Panel>(_privateTestObject, MainPanel).Visible.ShouldBeFalse(),
                () => Get<PlaceHolder>(_privateTestObject, ErrorPlaceHolder).Visible.ShouldBeTrue(),
                () => Get<Label>(_privateTestObject, ErrorMessageLabel).Text.ShouldContain(HardErrorExceptionMessage));
        }

        [Test]
        public void btnSubmit_Click_WhenGetEncryptionThrowsECNException_LogsException()
        {
            // Arrange
            SetFakesForBtnSubmitMethod();
            SetUpPageControls();
            ShimEncryption.GetCurrentByApplicationIDInt32 = (appId) => throw new ECNException(
                new List<ECNError>
                {
                    new ECNError { ErrorMessage = UTException }
                });

            // Act
            _privateTestObject.Invoke(BtnSubmitClickMethodName, this, EventArgs.Empty);

            // Assert
            _privateTestObject.ShouldSatisfyAllConditions(
                () => Get<PlaceHolder>(_privateTestObject, ErrorPlaceHolder).Visible.ShouldBeTrue(),
                () => Get<Label>(_privateTestObject, ErrorMessageLabel).Text.
                                  ShouldContain(UTException));
        }

        [Test]
        public void btnSubmit_Click_WhenEmailSaveThrowsException_LogsException()
        {
            // Arrange
            SetFakesForBtnSubmitMethod();
            SetUpPageControls();
            ShimEmailDirect.SaveEmailDirect = _ => throw new InvalidOperationException(UTException);

            // Act
            _privateTestObject.Invoke(BtnSubmitClickMethodName, this, EventArgs.Empty);

            // Assert
            _privateTestObject.ShouldSatisfyAllConditions(
                () => Get<Panel>(_privateTestObject, ConfirmationPanel).Visible.ShouldBeFalse(),
                () => Get<Panel>(_privateTestObject, MainPanel).Visible.ShouldBeFalse(),
                () => Get<PlaceHolder>(_privateTestObject, ErrorPlaceHolder).Visible.ShouldBeTrue(),
                () => Get<Label>(_privateTestObject, ErrorMessageLabel).Text.ShouldContain(HardErrorExceptionMessage));
        }

        private void SetFakesForBtnSubmitMethod()
        {
            _exceptionLogText = string.Empty;
            _isExceptionLogged = false;
            _isEmailDirectSaved = false;
            _savedEmailDirect = null;

            QueryString.Clear();
            QueryString.Add(KeyBCId, "1");

            var settings = new NameValueCollection();
            settings.Add(KeyKMCommon_Application, "1");
            settings.Add(KeyActivity_DomainPath, MvcActivityDomainPathKeyValue);
            ShimConfigurationManager.AppSettingsGet = () => settings;

            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 = (ex, name, id, _, __, ___) =>
            {
                _isExceptionLogged = true;
                _exceptionLogText = ex.Message;
                return 1;
            };

            ShimEmail.IsValidEmailAddressString = _ => true;
            ShimEmail.Exists_BaseChannelStringInt32 = (_, __) => true;
            ShimLandingPageAssign.GetByBaseChannelIDInt32 = (bcid) => new List<LandingPageAssign>
            {
                new LandingPageAssign { CustomerID = 1 , LPID = 1, LPAID = 1 }
            };

            ShimLandingPageAssign.GetDefault = () => GetDefaultLandingPageAssign();
            ShimLandingPageAssignContent.GetByLPAIDInt32 = (lpaid) => GetDefaultAssignContentList();
            ShimChannelMasterSuppressionList.GetByEmailAddressInt32StringUser = (_, __, ___) =>
                new List<CommunicatorEntities.ChannelMasterSuppressionList>();
            ShimGlobalMasterSuppressionList.GetByEmailAddressStringUser = (_, __) => 
                new List<CommunicatorEntities.GlobalMasterSuppressionList>();
            ShimGroup.GetMasterSuppressionGroup_NoAccessCheckInt32 = _ => new CommunicatorEntities.Group { };
            ShimEmailGroup.GetByEmailAddressGroupID_NoAccessCheckStringInt32 = (_, __) => null;
            
            ShimEncryption.GetCurrentByApplicationIDInt32 = (appId) => new Encryption(true);

            ShimEmailDirect.SaveEmailDirect = (emailDirect) =>
            {
                _savedEmailDirect = emailDirect;
                _isEmailDirectSaved = true;
                return _savedEmailDirect.CustomerID;
            };
        }

        private void SetUpPageControls()
        {
            Get<TextBox>(_privateTestObject, TxtOldEmailControl).Text = SampleEmailAddress;
            Get<TextBox>(_privateTestObject, TxtNewEmailControl).Text = SampleEmailAddress;
            Get<TextBox>(_privateTestObject, TxtReEnterControl).Text = SampleEmailAddress;
            Get<Panel>(_privateTestObject, PanelReEnter).Visible = true;
        }

        private List<LandingPageAssign> GetDefaultLandingPageAssign()
        {
            return new List<LandingPageAssign>
            {
                new LandingPageAssign
                {
                    CustomerID = 1,
                    LPID = 5,
                    LPAID = 1,
                    CreatedUserID = 1
                }
            };
        }

        private List<LandingPageAssignContent> GetDefaultAssignContentList()
        {
            var assignContentList =  new List<LandingPageAssignContent>();

            foreach (var num in Enumerable.Range(24, 6))
            {
                assignContentList.Add(new LandingPageAssignContent { LPOID = num, Display = $"{SampleDisplay}{num}" });
            }
            return assignContentList;
        }
    }
}
