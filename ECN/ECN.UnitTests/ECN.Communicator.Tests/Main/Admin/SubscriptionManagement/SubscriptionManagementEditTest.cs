using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using AjaxControlToolkit;
using AjaxControlToolkit.Fakes;
using ecn.communicator.main.admin.SubscriptionManagement;
using ecn.communicator.MasterPages.Fakes;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using static ECN_Framework_Common.Objects.Enums;
using CommunicatorMasterPage = ecn.communicator.MasterPages.Communicator;
using SubscriptionManagementEntity = ECN_Framework_Entities.Accounts.SubscriptionManagement;

namespace ECN.Communicator.Tests.Main.Admin.SubscriptionManagement
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class SubscriptionManagementEditTest
    {
        private const int Zero = 0;
        private const int AboveZero = 100;
        private const string EmptyString = "";
        private const string No = "no";
        private const string Yes = "yes";
        private const string AnyString = "77C5B6AE-BD1A-486B-8D75-49C57FA911C4";
        private const string DropDown = "Drop Down";
        private const string CurrentPageFieldName = "currentPage";
        private const string SubscriptionManageMentFroupFieldName = "listSMGroups";
        private const string ThankYouLabel = "ThankYouLabel";
        private const string RedirectUrlText = "RedirectUrlText";
        private const string IsDeletedColumn = FieldReasonIsDeleted;
        private const int RedirectDelay = 200;
        private const int AnyNumber = 100;
        private const int AnyOtherNumber = AnyNumber + 1;
        private const string SubscriptionManageMentGroupsFieldName = "dtSMGroups";
        private const string SubscriptionManagementGroupsUDFsFieldName = "dtSMGUDFs";
        private const string ReasonDataTableName = "dtReason";
        private const string FieldReasonName = "Reason";
        private const string FieldReasonID = "ID";
        private const string FieldReasonIsDeleted = "IsDeleted";
        private const string FieldReasonSortOrder = "SortOrder";
        private const string EditReasonCommandName = "EditReason";
        private const string DeleteReasonCommandName = "DeleteReason";
        private const string ReasonCommandArgument = "ReasonID";
        private const string NonEmptyText = "Non Empty Text";
        private const string RblReasonControlType = "rblReasonControlType";
        private const string RblVisibilityReason = "rblVisibilityReason";
        private const string DropDownText = "Drop Down";
        private const string FieldRlReasonDropDown = "rlReasonDropDown";
        private const string SubscriptionManagementQueryStringID = "smid";
        private const int SubscriptionManagementID = 5;
        private const int ReasonSortOrder = 4;
        private IDisposable _shimsContext;
        private SubscriptionManagementEdit _page;
        private PrivateObject _pagePrivate;
        private int _currentUserBaseChannelId;
        private RadioButtonList _rblVisibilityReason;
        private RadioButtonList _rblReasonControlType;
        private RadioButtonList _rblRedirectThankYou;
        private CheckBox _chkIncludeMasterSuppressed;
        private TextBox _txtMSMessage;
        private TextBox _txtName;
        private TextBox _txtAdminEmail;
        private TextBox _txtEmailFooter;
        private TextBox _txtEmailHeader;
        private TextBox _txtPageFooter;
        private TextBox _txtPageHeader;
        private TextBox _txtReasonLabel;
        private TextBox _txtThankYouMessage;
        private TextBox _txtRedirectURL;
        private Label _lblErrorMessage;
        private PlaceHolder _phError;
        private DropDownList _ddlRedirectDelay;
        private Dictionary<string, object> _staticFields;
        private MemoryStream _responseStream;
        private StreamWriter _responseStreamWriter;
        private string _responseRedirectUrl;
        private Random _random = new Random();
        private BindingFlags _staticFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        private int _subscriptionManagementSaveResult;
        private ECNException _subscriptionManagementSaveException;
        private ECNException _groupSaveException;
        private int _groupSaveResult;
        private SubscriptionManagementGroup _groupSaveInput;
        private ECNException _groupDeleteException;
        private int _groupDeleteSMId;
        private User _currentUser;
        private ECNException _subscriptionManagementUDFDeleteException;
        private int _subscriptionManagementUDFDeleteId;
        private SubsriptionManagementUDF _subscriptionManagementUDFSaveInput;
        private ECNException _subscriptionManagementUDFSaveException;
        private DataTable _dtReason;

        [SetUp]
        public void Setup()
        {
            _shimsContext = ShimsContext.Create();
            _page = new SubscriptionManagementEdit();
            _pagePrivate = new PrivateObject(_page);
            _staticFields = new Dictionary<string, object>();
            _responseStream = new MemoryStream();
            _responseStreamWriter = new StreamWriter(_responseStream);
            _dtReason = new DataTable();
            InitializePageFields();
            CommonShims();
        }

        [TearDown]
        public void TearDown()
        {
            _responseStreamWriter.Dispose();
            _responseStream.Dispose();
            _page.Dispose();
            _shimsContext.Dispose();
        }

        [Test]
        [TestCase(AboveZero, true, AnyString, AnyString, Yes, AnyString, AnyString, DropDown, true)]
        [TestCase(AboveZero, true, AnyString, AnyString, Yes, AnyString, AnyString, AnyString, false)]
        [TestCase(AboveZero, false, AnyString, EmptyString, No, AnyString, EmptyString, AnyString, null)]
        [TestCase(Zero, true, AnyString, AnyString, Yes, AnyString, AnyString, DropDown, true)]
        [TestCase(Zero, true, AnyString, AnyString, Yes, AnyString, AnyString, AnyString, false)]
        [TestCase(Zero, false, AnyString, EmptyString, No, AnyString, EmptyString, AnyString, null)]
        public void btnSavePage_Click_currentPage_ShouldUpdateCurrentPage(
            int currentPageSubscriptionId,
            bool includeMasterSuppressed,
            string txtMSMessage,
            string expectedMsMessage,
            string visibilityReason,
            string reason,
            string expectedReason,
            string userControlType,
            bool? useReasonDropDown)
        {
            //Arrange
            var currentPage = new SubscriptionManagementEntity
            {
                SubscriptionManagementID = currentPageSubscriptionId
            };
            _chkIncludeMasterSuppressed.Checked = includeMasterSuppressed;
            _txtMSMessage.Text = txtMSMessage;
            _rblVisibilityReason.Items.Add(visibilityReason);
            _rblVisibilityReason.SelectedIndex = 0;
            _txtReasonLabel.Text = reason;
            _rblReasonControlType.Items.Add(userControlType);
            _rblReasonControlType.SelectedIndex = 0;
            var listSMGroups = new List<SubscriptionManagementGroup>();
            _staticFields[CurrentPageFieldName] = currentPage;
            _staticFields[SubscriptionManageMentFroupFieldName] = listSMGroups;
            SetStaticFields();
            SetReasonTable(GetReasonTable());
            //Act
            CallbtnSavePage_Click();
            //Assert
            if (currentPageSubscriptionId == Zero)
            {
                currentPage = _pagePrivate.GetField(CurrentPageFieldName, _staticFlags) as SubscriptionManagementEntity;
                currentPage.ShouldNotBeNull();
            }
            _responseRedirectUrl.ShouldBe("SubscriptionManagementList.aspx");
            currentPage.ShouldSatisfyAllConditions(
                () => currentPage.MSMessage.ShouldBe(expectedMsMessage),
                () => currentPage.ReasonLabel.ShouldBe(expectedReason),
                () => currentPage.IncludeMSGroups.ShouldBe(includeMasterSuppressed),
                () => currentPage.UseReasonDropDown.ShouldBe(useReasonDropDown));
        }

        [Test]
        [TestCase(AboveZero, "thankyou", true, false, ThankYouLabel, EmptyString, Zero)]
        [TestCase(AboveZero, "redirect", false, true, EmptyString, RedirectUrlText, Zero)]
        [TestCase(AboveZero, "both", true, true, ThankYouLabel, RedirectUrlText, RedirectDelay)]
        [TestCase(AboveZero, AnyString, false, false, EmptyString, EmptyString, Zero)]
        [TestCase(Zero, "thankyou", true, false, ThankYouLabel, EmptyString, Zero)]
        [TestCase(Zero, "redirect", false, true, EmptyString, RedirectUrlText, Zero)]
        [TestCase(Zero, "both", true, true, ThankYouLabel, RedirectUrlText, RedirectDelay)]
        [TestCase(Zero, AnyString, false, false, EmptyString, EmptyString, Zero)]
        public void btnSavePage_Click_currentPageRedirects_ShouldUpdateCurrentPage(
            int currentPageSubscriptionId,
            string redirectThankYou,
            bool? expectedUseThankYou,
            bool? expectedUseRedirect,
            string expectedThankYouLabel,
            string expectedRedirectUrl,
            int expectedRedirectDelay)
        {
            //Arrange
            _txtThankYouMessage.Text = ThankYouLabel;
            _txtRedirectURL.Text = RedirectUrlText;
            _ddlRedirectDelay.Items.Add(RedirectDelay.ToString());
            _ddlRedirectDelay.SelectedIndex = 0;
            _rblRedirectThankYou.Items.Add(redirectThankYou);
            _rblRedirectThankYou.SelectedIndex = 0;
            var currentPage = new SubscriptionManagementEntity
            {
                SubscriptionManagementID = currentPageSubscriptionId
            };
            var listSMGroups = new List<SubscriptionManagementGroup>();
            var dtSMGroups = new List<TempSMG>();
            var dtSMGUDFs = new List<TempSMGUDF>();
            _staticFields[CurrentPageFieldName] = currentPage;
            _staticFields[SubscriptionManageMentFroupFieldName] = listSMGroups;
            _staticFields[SubscriptionManageMentGroupsFieldName] = dtSMGroups;
            _staticFields[SubscriptionManagementGroupsUDFsFieldName] = dtSMGUDFs;
            SetStaticFields();
            SetReasonTable(GetReasonTable());
            _subscriptionManagementSaveResult = GetAnyNumber();
            //Act
            CallbtnSavePage_Click();
            //Assert
            if (currentPageSubscriptionId == Zero)
            {
                currentPage = _pagePrivate.GetField(CurrentPageFieldName, _staticFlags) as SubscriptionManagementEntity;
                currentPage.ShouldNotBeNull();
            }
            _responseRedirectUrl.ShouldBe("SubscriptionManagementList.aspx");
            currentPage.ShouldSatisfyAllConditions(
                () => currentPage.UseThankYou.ShouldBe(expectedUseThankYou),
                () => currentPage.UseRedirect.ShouldBe(expectedUseRedirect),
                () => currentPage.ThankYouLabel.ShouldBe(expectedThankYouLabel),
                () => currentPage.RedirectURL.ShouldBe(expectedRedirectUrl),
                () => currentPage.RedirectDelay.ShouldBe(expectedRedirectDelay),
                () => currentPage.SubscriptionManagementID.ShouldBe(_subscriptionManagementSaveResult));
        }

        [Test]
        [TestCase(Zero)]
        [TestCase(AboveZero)]
        public void btnSavePage_Click_WhenCurrentPageSaveThrowException_Should(int currentPageSubscriptionId)
        {
            //Arrange
            var currentPage = new SubscriptionManagementEntity
            {
                SubscriptionManagementID = currentPageSubscriptionId
            };
            _txtThankYouMessage.Text = ThankYouLabel;
            var listSMGroups = new List<SubscriptionManagementGroup>();
            _staticFields[CurrentPageFieldName] = currentPage;
            _staticFields[SubscriptionManageMentFroupFieldName] = listSMGroups;
            SetStaticFields();
            SetReasonTable(GetReasonTable());
            var error = new ECNError
            {
                ErrorMessage = GetAnyString(),
                Entity = Entity.APILogging
            };
            var errors = new List<ECNError>
            {
                error
            };
            _subscriptionManagementSaveException = new ECNException(errors);
            var expectedErrorMessage = GetExpectedErrorMessage(error);
            //Act
            CallbtnSavePage_Click();
            //Assert
            _phError.Visible.ShouldBeTrue();
            _lblErrorMessage.Text.ShouldBe(expectedErrorMessage);
        }

        [Test]
        public void btnSavePage_Click_WhenNoReasons_ShouldShowError()
        {
            //Arrange
            _rblVisibilityReason.Items.Add(Yes);
            _rblVisibilityReason.SelectedIndex = 0;
            _rblReasonControlType.Items.Add(DropDown);
            _rblReasonControlType.SelectedIndex = 0;
            var currentPage = new SubscriptionManagementEntity
            {
                SubscriptionManagementID = GetAnyNumber()
            };
            _staticFields[CurrentPageFieldName] = currentPage;
            var table = GetReasonTable();
            foreach (DataRow row in table.Rows)
            {
                row[IsDeletedColumn] = true;
            }
            SetReasonTable(table);
            SetStaticFields();
            var expecedErrorMessage = "Please enter at least one Reason";
            //Act
            CallbtnSavePage_Click();
            //Assert
            _lblErrorMessage.Text.ShouldContain(expecedErrorMessage);
        }

        [Test]
        public void btnSavePage_Click_ECNExceptionThrown_ShouldShowErrors()
        {
            //Arrange
            var textGetCalledBefore = false;
            var error = new ECNError
            {
                ErrorMessage = GetAnyString(),
                Entity = Entity.APILogging
            };
            var errors = new List<ECNError>
            {
                error
            };
            ShimTextBox.AllInstances.TextGet = textBox =>
            {
                if (textGetCalledBefore)
                {
                    return ShimsContext.ExecuteWithoutShims(() => textBox.Text);
                }
                else
                {
                    textGetCalledBefore = true;
                    throw new ECNException(errors);
                }
            };
            var currentPage = new SubscriptionManagementEntity
            {
                SubscriptionManagementID = GetAnyNumber()
            };
            _staticFields[CurrentPageFieldName] = currentPage;
            SetStaticFields();
            SetReasonTable(GetReasonTable());
            var expectedErrorMessage = GetExpectedErrorMessage(error);
            //Act
            CallbtnSavePage_Click();
            //Assert
            _phError.Visible.ShouldBeTrue();
            _lblErrorMessage.Text.ShouldBe(expectedErrorMessage);
        }

        [Test]
        public void btnSavePage_Click_GroupIdContainsDash_ShouldUpdateAndSaveSave()
        {
            //Arrange
            var currentPage = new SubscriptionManagementEntity
            {
                SubscriptionManagementID = GetAnyNumber()
            };
            var listSMGroups = new List<SubscriptionManagementGroup>();
            var tempSMG = new TempSMG
            {
                SMGID = GetAnyString(),
                GroupID = GetAnyNumber(),
                Label = GetAnyString()
            };
            var dtSMGroups = new List<TempSMG>
            {
                tempSMG
            };
            var dtSMGUDFs = new List<TempSMGUDF>();
            _staticFields[CurrentPageFieldName] = currentPage;
            _staticFields[SubscriptionManageMentFroupFieldName] = listSMGroups;
            _staticFields[SubscriptionManageMentGroupsFieldName] = dtSMGroups;
            _staticFields[SubscriptionManagementGroupsUDFsFieldName] = dtSMGUDFs;
            SetStaticFields();
            SetReasonTable(GetReasonTable());
            _groupSaveResult = GetAnyNumber();
            _subscriptionManagementSaveResult = GetAnyNumber();
            //Act
            CallbtnSavePage_Click();
            //Assert
            tempSMG.DBSMGroupID.ShouldBe(_groupSaveResult);
            _groupSaveInput.ShouldSatisfyAllConditions(
                () => _groupSaveInput.ShouldNotBeNull(),
                () => _groupSaveInput.Label.ShouldBe(tempSMG.Label),
                () => _groupSaveInput.GroupID.ShouldBe(tempSMG.GroupID));
        }

        [Test]
        public void btnSavePage_Click_GroupIdContainsDashWhenException_ShouldShowError()
        {
            //Arrange
            var currentPage = new SubscriptionManagementEntity
            {
                SubscriptionManagementID = GetAnyNumber()
            };
            var listSMGroups = new List<SubscriptionManagementGroup>();
            var tempSMG = new TempSMG
            {
                SMGID = GetAnyString(),
                GroupID = GetAnyNumber(),
                Label = GetAnyString(),

            };
            var dtSMGroups = new List<TempSMG>
            {
                tempSMG
            };
            var dtSMGUDFs = new List<TempSMGUDF>();
            _staticFields[CurrentPageFieldName] = currentPage;
            _staticFields[SubscriptionManageMentFroupFieldName] = listSMGroups;
            _staticFields[SubscriptionManageMentGroupsFieldName] = dtSMGroups;
            _staticFields[SubscriptionManagementGroupsUDFsFieldName] = dtSMGUDFs;
            SetStaticFields();
            SetReasonTable(GetReasonTable());
            var error = new ECNError
            {
                ErrorMessage = GetAnyString(),
                Entity = Entity.BlastOptOutGroup
            };
            var errors = new List<ECNError>
            {
                error
            };
            _groupSaveException = new ECNException(errors);
            _subscriptionManagementSaveResult = GetAnyNumber();
            //Act
            CallbtnSavePage_Click();
            //Assert
            _lblErrorMessage.Text.ShouldBe(GetExpectedErrorMessage(error));
        }

        [Test]
        public void btnSavePage_Click_GroupIdNotContainingDashAndIsDeleted_ShouldCallDelete()
        {
            //Arrange
            var currentPage = new SubscriptionManagementEntity
            {
                SubscriptionManagementID = GetAnyNumber()
            };
            var listSMGroups = new List<SubscriptionManagementGroup>();
            var tempSMG = new TempSMG
            {
                SMID = GetAnyNumber(),
                SMGID = GetAnyNumber().ToString(),
                GroupID = GetAnyNumber(),
                Label = GetAnyString(),
                IsDeleted = true
            };
            var dtSMGroups = new List<TempSMG>
            {
                tempSMG
            };
            var dtSMGUDFs = new List<TempSMGUDF>();
            _staticFields[CurrentPageFieldName] = currentPage;
            _staticFields[SubscriptionManageMentFroupFieldName] = listSMGroups;
            _staticFields[SubscriptionManageMentGroupsFieldName] = dtSMGroups;
            _staticFields[SubscriptionManagementGroupsUDFsFieldName] = dtSMGUDFs;
            SetStaticFields();
            SetReasonTable(GetReasonTable());
            _subscriptionManagementSaveResult = GetAnyNumber();
            //Act
            CallbtnSavePage_Click();
            //Assert
            _groupDeleteSMId.ShouldBe(tempSMG.SMID);
        }

        [Test]
        public void btnSavePage_Click_GroupIdNotContainingDashAndIsDeletedWhenExceptionThrown_ShouldShowError()
        {
            //Arrange
            var currentPage = new SubscriptionManagementEntity
            {
                SubscriptionManagementID = GetAnyNumber()
            };
            var listSMGroups = new List<SubscriptionManagementGroup>();
            var tempSMG = new TempSMG
            {
                SMID = GetAnyNumber(),
                SMGID = GetAnyNumber().ToString(),
                GroupID = GetAnyNumber(),
                Label = GetAnyString(),
                IsDeleted = true
            };
            var dtSMGroups = new List<TempSMG>
            {
                tempSMG
            };
            var dtSMGUDFs = new List<TempSMGUDF>();
            _staticFields[CurrentPageFieldName] = currentPage;
            _staticFields[SubscriptionManageMentFroupFieldName] = listSMGroups;
            _staticFields[SubscriptionManageMentGroupsFieldName] = dtSMGroups;
            _staticFields[SubscriptionManagementGroupsUDFsFieldName] = dtSMGUDFs;
            SetStaticFields();
            SetReasonTable(GetReasonTable());
            _subscriptionManagementSaveResult = GetAnyNumber();
            var error = new ECNError
            {
                ErrorMessage = GetAnyString(),
                Entity = Entity.BlastChampion
            };
            var errors = new List<ECNError> { error };
            _groupDeleteException = new ECNException(errors);
            //Act
            CallbtnSavePage_Click();
            //Assert
            _lblErrorMessage.Text.ShouldBe(GetExpectedErrorMessage(error));
        }

        [Test]
        public void btnSavePage_Click_GroupIdNotContainingDashAndNotIsDeleted_ShouldUpdateAndSaveGroup()
        {
            //Arrange
            var currentPage = new SubscriptionManagementEntity
            {
                SubscriptionManagementID = GetAnyNumber()
            };
            var smgIdNumber = GetAnyNumber();
            var tempGroup = new TempSMG
            {
                SMID = GetAnyNumber(),
                SMGID = smgIdNumber.ToString(),
                GroupID = GetAnyNumber(),
                Label = GetAnyString(),
                IsDeleted = false
            };
            var dtSMGroups = new List<TempSMG>
            {
                tempGroup
            };
            var group = new SubscriptionManagementGroup
            {
                SubscriptionManagementGroupID = smgIdNumber
            };
            var listSMGroups = new List<SubscriptionManagementGroup>
            {
                group
            };
            var dtSMGUDFs = new List<TempSMGUDF>();
            _staticFields[CurrentPageFieldName] = currentPage;
            _staticFields[SubscriptionManageMentFroupFieldName] = listSMGroups;
            _staticFields[SubscriptionManageMentGroupsFieldName] = dtSMGroups;
            _staticFields[SubscriptionManagementGroupsUDFsFieldName] = dtSMGUDFs;
            SetStaticFields();
            SetReasonTable(GetReasonTable());
            _subscriptionManagementSaveResult = GetAnyNumber();
            _currentUser.UserID = GetAnyNumber();
            //Act
            CallbtnSavePage_Click();
            //Assert
            _groupSaveInput.ShouldSatisfyAllConditions(
                () => _groupSaveInput.ShouldNotBeNull(),
                () => _groupSaveInput.ShouldBe(group),
                () => _groupSaveInput.UpdatedUserID.ShouldBe(_currentUser.UserID),
                () => _groupSaveInput.Label.ShouldBe(tempGroup.Label));
        }

        [Test]
        public void btnSavePage_Click_SMUDFIDContainsDash_ShouldUpdateAndSaveSave()
        {
            //Arrange
            var currentPage = new SubscriptionManagementEntity
            {
                SubscriptionManagementID = GetAnyNumber()
            };
            var listSMGroups = new List<SubscriptionManagementGroup>();
            var smgId = GetAnyString();
            var tempGroup = new TempSMG
            {
                DBSMGroupID = GetAnyNumber(),
                SMGID = smgId
            };
            var dtSMGroups = new List<TempSMG>
            {
                tempGroup
            };
            var smgUDF = new TempSMGUDF
            {
                SMUDFID = GetAnyString(),
                GroupDataFieldsID = GetAnyNumber(),
                StaticValue = GetAnyString(),
                SMGID = smgId,
                IsDeleted = false
            };
            var dtSMGUDFs = new List<TempSMGUDF>
            {
                smgUDF
            };
            _staticFields[CurrentPageFieldName] = currentPage;
            _staticFields[SubscriptionManageMentFroupFieldName] = listSMGroups;
            _staticFields[SubscriptionManageMentGroupsFieldName] = dtSMGroups;
            _staticFields[SubscriptionManagementGroupsUDFsFieldName] = dtSMGUDFs;
            SetStaticFields();
            SetReasonTable(GetReasonTable());
            _groupSaveResult = GetAnyNumber();
            _subscriptionManagementSaveResult = GetAnyNumber();
            _currentUser.UserID = GetAnyNumber();
            //Act
            CallbtnSavePage_Click();
            //Assert
            _subscriptionManagementUDFSaveInput.ShouldSatisfyAllConditions(
                () => _subscriptionManagementUDFSaveInput.ShouldNotBeNull(),
                () => _subscriptionManagementUDFSaveInput.IsDeleted.ShouldBeFalse(),
                () => _subscriptionManagementUDFSaveInput.StaticValue.ShouldBe(smgUDF.StaticValue),
                () => _subscriptionManagementUDFSaveInput.GroupDataFieldsID.ShouldBe(smgUDF.GroupDataFieldsID),
                () => _subscriptionManagementUDFSaveInput.SubscriptionManagementGroupID.ShouldBe(tempGroup.DBSMGroupID));
        }

        [Test]
        public void btnSavePage_Click_SMUDFIDContainsDashWhenException_ShouldShowError()
        {
            //Arrange
            var currentPage = new SubscriptionManagementEntity
            {
                SubscriptionManagementID = GetAnyNumber()
            };
            var listSMGroups = new List<SubscriptionManagementGroup>();
            var smgId = GetAnyString();
            var tempGroup = new TempSMG
            {
                DBSMGroupID = GetAnyNumber(),
                SMGID = smgId
            };
            var dtSMGroups = new List<TempSMG>
            {
                tempGroup
            };
            var smgUDF = new TempSMGUDF
            {
                SMUDFID = GetAnyString(),
                GroupDataFieldsID = GetAnyNumber(),
                StaticValue = GetAnyString(),
                SMGID = smgId,
                IsDeleted = false
            };
            var dtSMGUDFs = new List<TempSMGUDF>
            {
                smgUDF
            };
            _staticFields[CurrentPageFieldName] = currentPage;
            _staticFields[SubscriptionManageMentFroupFieldName] = listSMGroups;
            _staticFields[SubscriptionManageMentGroupsFieldName] = dtSMGroups;
            _staticFields[SubscriptionManagementGroupsUDFsFieldName] = dtSMGUDFs;
            SetStaticFields();
            SetReasonTable(GetReasonTable());
            _groupSaveResult = GetAnyNumber();
            _subscriptionManagementSaveResult = GetAnyNumber();
            _currentUser.UserID = GetAnyNumber();
            var error = new ECNError
            {
                Entity = Entity.APILogging,
                ErrorMessage = GetAnyString()
            };
            var errors = new List<ECNError> {error };
            _subscriptionManagementUDFSaveException = new ECNException(errors);
            //Act
            CallbtnSavePage_Click();
            //Assert
            _lblErrorMessage.Text.ShouldBe(GetExpectedErrorMessage(error));
        }

        [Test]
        public void btnSavePage_Click_SMUDFIDNotContainingDashAndIsDelete_ShouldDelete()
        {
            //Arrange
            var currentPage = new SubscriptionManagementEntity
            {
                SubscriptionManagementID = GetAnyNumber()
            };
            var smgId = GetAnyNumber();
            var listSMGroups = new List<SubscriptionManagementGroup>
            {
                new SubscriptionManagementGroup
                {
                    SubscriptionManagementGroupID = smgId
                }
            };
            var tempGroup = new TempSMG
            {
                DBSMGroupID = GetAnyNumber(),
                SMGID = smgId.ToString()
            };
            var dtSMGroups = new List<TempSMG>
            {
                tempGroup
            };
            var smgUDF = new TempSMGUDF
            {
                SMUDFID = GetAnyNumber().ToString(),
                GroupDataFieldsID = GetAnyNumber(),
                StaticValue = GetAnyString(),
                SMGID = smgId.ToString(),
                IsDeleted = true
            };
            var dtSMGUDFs = new List<TempSMGUDF>
            {
                smgUDF
            };
            _staticFields[CurrentPageFieldName] = currentPage;
            _staticFields[SubscriptionManageMentFroupFieldName] = listSMGroups;
            _staticFields[SubscriptionManageMentGroupsFieldName] = dtSMGroups;
            _staticFields[SubscriptionManagementGroupsUDFsFieldName] = dtSMGUDFs;
            SetStaticFields();
            SetReasonTable(GetReasonTable());
            _groupSaveResult = GetAnyNumber();
            _subscriptionManagementSaveResult = GetAnyNumber();
            _currentUser.UserID = GetAnyNumber();
           
            //Act
            CallbtnSavePage_Click();
            //Assert
            _subscriptionManagementUDFDeleteId.ShouldBe(smgId);
        }

        [Test]
        public void btnSavePage_Click_SMUDFIDNotContainingDashAndIsDeleteWhenExceptionThrown_ShouldShowError()
        {
            //Arrange
            var currentPage = new SubscriptionManagementEntity
            {
                SubscriptionManagementID = GetAnyNumber()
            };
            var smgId = GetAnyNumber();
            var listSMGroups = new List<SubscriptionManagementGroup>
            {
                new SubscriptionManagementGroup
                {
                    SubscriptionManagementGroupID = smgId
                }
            };
            var tempGroup = new TempSMG
            {
                DBSMGroupID = GetAnyNumber(),
                SMGID = smgId.ToString()
            };
            var dtSMGroups = new List<TempSMG>
            {
                tempGroup
            };
            var smgUDF = new TempSMGUDF
            {
                SMUDFID = GetAnyNumber().ToString(),
                GroupDataFieldsID = GetAnyNumber(),
                StaticValue = GetAnyString(),
                SMGID = smgId.ToString(),
                IsDeleted = true
            };
            var dtSMGUDFs = new List<TempSMGUDF>
            {
                smgUDF
            };
            _staticFields[CurrentPageFieldName] = currentPage;
            _staticFields[SubscriptionManageMentFroupFieldName] = listSMGroups;
            _staticFields[SubscriptionManageMentGroupsFieldName] = dtSMGroups;
            _staticFields[SubscriptionManagementGroupsUDFsFieldName] = dtSMGUDFs;
            SetStaticFields();
            SetReasonTable(GetReasonTable());
            _groupSaveResult = GetAnyNumber();
            _subscriptionManagementSaveResult = GetAnyNumber();
            _currentUser.UserID = GetAnyNumber();
            var error = new ECNError
            {
                ErrorMessage = GetAnyString(),
                Entity = Entity.BlastAB
            };
            var errors = new List<ECNError> {error };
            _subscriptionManagementUDFDeleteException = new ECNException(errors);
            //Act
            CallbtnSavePage_Click();
            //Assert
            _lblErrorMessage.Text.ShouldBe(GetExpectedErrorMessage(error));
        }

        [Test]
        public void RlReasonDropDownItemCommand_EditReasonCommand_UpdateControls()
        {
            // Arrange
            InitializeReasonControls();
            InitializeReasonTable(ReasonCommandArgument);

            // Act
            var reorderListCommandEventArgs = ReflectionHelper.CreateInstance<ReorderListCommandEventArgs>(
                new object[] { new CommandEventArgs(EditReasonCommandName, ReasonCommandArgument), this, null });
            var parameters = new object[] { null, reorderListCommandEventArgs };
            ReflectionHelper.ExecuteMethod(_page, "rlReasonDropDown_ItemCommand", parameters);

            // Assert
            AssertTextBox("txtReasonLabelEdit", FieldReasonName);
            AssertButtonCommand("btnSaveReason", ReasonCommandArgument);
        }

        [Test]
        public void RlReasonDropDownItemCommand_DeleteReasonCommand_SetsIsDeletedAndDecrementSortOrderAndCallsLoadReasonData()
        {
            // Arrange
            InitializePageFields();
            InitializeReasonTable(ReasonCommandArgument);

            // Act
            var reorderListCommandEventArgs = ReflectionHelper.CreateInstance<ReorderListCommandEventArgs>(
                new object[] { new CommandEventArgs(DeleteReasonCommandName, ReasonCommandArgument), this, null });
            var parameters = new object[] { null, reorderListCommandEventArgs };
            ReflectionHelper.ExecuteMethod(_page, "rlReasonDropDown_ItemCommand", parameters);

            // Assert
            AssertDataTableDataLength($"{FieldReasonIsDeleted} = false", 1);
            AssertDataTableDataLength($"{FieldReasonIsDeleted} = true", 1);
            AssertDataTableDataLength($"{FieldReasonSortOrder} = {ReasonSortOrder}", 2);
        }

        [Test]
        [TestCase(ReasonSortOrder, ReasonSortOrder + 1, ReasonSortOrder + 2)]
        [TestCase(ReasonSortOrder, ReasonSortOrder - 1, ReasonSortOrder)]
        public void RlReasonDropDownItemReorder_OldIndex_NewIndex(int oldIndex, int newIndex, int expectedResult)
        {
            // Arrange
            InitializePageFields();
            InitializeReasonTable(ReasonCommandArgument);
            ShimReorderList.AllInstances.PerformDataBindingIEnumerable = (_, __) => { };

            // Act
            var reorderListItemReorderEventArgs = ReflectionHelper.CreateInstance<ReorderListItemReorderEventArgs>(
                new object[] { null, oldIndex, newIndex });
            var parameters = new object[] { null, reorderListItemReorderEventArgs };
            ReflectionHelper.ExecuteMethod(_page, "rlReasonDropDown_ItemReorder", parameters);

            // Assert
            AssertDataTableDataLength($"{FieldReasonSortOrder} = {expectedResult}", 2);
        }

        [Test]
        [TestCase("thankyou", new bool[] { false, false, true })]
        [TestCase("redirect", new bool[] { false, true, false })]
        [TestCase("both", new bool[] { true, true, true })]
        [TestCase("neither", new bool[] { false, false, false })]
        public void RblRedirectThankYouSelectedIndexChanged_SelectedValue_UpdateControlVisibility(string selectedValue, bool[] expectedControlsVisibility)
        {
            // Arrange
            InitializePageFields();
            InitializeRedirectRadioAndDropDownList(selectedValue);

            // Act
            var parameters = new object[] { null, null };
            ReflectionHelper.ExecuteMethod(_page, "rblRedirectThankYou_SelectedIndexChanged", parameters);

            // Assert
            AssertTableVisibility("tblDelay", expectedControlsVisibility[0]);
            AssertTableVisibility("tblRedirect", expectedControlsVisibility[1]);
            AssertTableVisibility("tblThankYou", expectedControlsVisibility[2]);

            AssertTextBox("txtRedirectURL", string.Empty);
            AssertTextBox("txtThankYouMessage", string.Empty);
            AssertDropDownListSelectedIndex("ddlRedirectDelay", 0);
        }

        [Test]
        public void RblReasonControlTypeSelectedIndexChanged_ReasonControlTypeNotTextBox_FillDataTableWithDefaultReasons()
        {
            // Arrange
            InitializePageFields();
            InitializeReasonColumns();
            ReflectionHelper.SetValue(_page, ReasonDataTableName, _dtReason);
            ReflectionHelper.SetValue(_page, RblReasonControlType, GetRadioButtonList(string.Empty));
            ShimReorderList.AllInstances.PerformDataBindingIEnumerable = (_, __) => { };

            // Act
            var parameters = new object[] { null, EventArgs.Empty };
            ReflectionHelper.ExecuteMethod(_page, "rblReasonControlType_SelectedIndexChanged", parameters);

            // Assert
            AssertDefaultReasons();
        }

        [Test]
        public void LoadData_GetByLPOIDReturnEmptyList_FillDataTable()
        {
            // Arrange
            InitializePageFields();
            var queryStrings = new NameValueCollection();
            queryStrings.Add(SubscriptionManagementQueryStringID, SubscriptionManagementID.ToString()); 
            ShimHttpRequest.AllInstances.QueryStringGet = (_) => { return queryStrings; };
            ReflectionHelper.SetValue(_page, RblVisibilityReason, GetRadioButtonList(Yes));
            ReflectionHelper.SetValue(_page, RblReasonControlType, GetRadioButtonList(DropDownText));
            ShimSubscriptionManagement.GetBySubscriptionManagementIDInt32 = (_) => GetSubscriptionManagement();
            ShimSubscriptionManagementReason.GetBySMIDInt32 = (_) => new List<SubscriptionManagementReason>();
            ShimSubscriptionManagementGroup.GetBySMIDInt32 = (_) => new List<SubscriptionManagementGroup>();
            InitializeReasonColumns();
            ReflectionHelper.SetValue(_page, ReasonDataTableName, _dtReason);
            const int LPOID = 3;
            ShimLandingPageAssignContent.GetByLPAIDInt32 = (id) =>
            {
                return new List<LandingPageAssignContent>
                {
                    new LandingPageAssignContent
                    {
                        LPOID = LPOID,
                        Display = NonEmptyText
                    }
                };
            };
            ShimLandingPageAssignContent.GetByLPOIDInt32Int32 = (_, __) =>
            {
                return new List<LandingPageAssignContent>();
            };

            // Act	
            ReflectionHelper.ExecuteMethod(_page, "LoadData", new object[] { });

            // Assert
            AssertDefaultReasons();

            var reasonDropDown = ReflectionHelper.GetFieldInfoFromInstanceByName(_page, FieldRlReasonDropDown)
                .GetValue(_page) as ReorderList;
            reasonDropDown.ShouldSatisfyAllConditions(
                () => reasonDropDown.ShouldNotBeNull(),
                () => reasonDropDown.DataSource.ShouldNotBeNull(),
                () => (reasonDropDown.DataSource as IList).ShouldNotBeNull(),
                () => (reasonDropDown.DataSource as IList).Count.ShouldBe(6));
        }

        private void AssertDefaultReasons()
        {
            var actualResult = ReflectionHelper.GetFieldInfoFromInstanceTypeByName(typeof(SubscriptionManagementEdit), ReasonDataTableName).GetValue(null) as DataTable;
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Rows
                    .Cast<DataRow>()
                    .ShouldNotBeEmpty(),
                () => actualResult.Rows.Count.ShouldBeGreaterThanOrEqualTo(6));

            var reasonNames = new string[]
            {
                "Email Frequency",
                "Email Volume",
                "Content not relevant",
                "Signed up for one-time email",
                "Circumstances changed(moved, married, changed jobs, etc.)",
                "Prefer to get information another way"
            };
            for (var i = 0; i < reasonNames.Length; i++)
            {
                var dataRow = actualResult.Select($"{FieldReasonSortOrder} = {i + 1}");
                dataRow.ShouldSatisfyAllConditions(
                    () => dataRow.ShouldNotBeNull(),
                    () => dataRow.Length.ShouldBeGreaterThanOrEqualTo(1),
                    () => dataRow[0][FieldReasonName].ShouldBe(reasonNames[i]));
            }
        }

        private RadioButtonList GetRadioButtonList(string itemValue)
        {
            var rbList = new RadioButtonList();
            rbList.Items.Add(new ListItem(NonEmptyText, itemValue));
            rbList.SelectedIndex = 0;
            return rbList;
        }

        private SubscriptionManagementEntity GetSubscriptionManagement()
        {
            return new SubscriptionManagementEntity
            {
                Header = NonEmptyText,
                Footer = NonEmptyText,
                EmailFooter = NonEmptyText,
                EmailHeader = NonEmptyText,
                AdminEmail = NonEmptyText,
                MSMessage = NonEmptyText,
                IncludeMSGroups = false,
                ReasonVisible = true,
                ReasonLabel = NonEmptyText,
                UseReasonDropDown = true
            };
        }

        private void AssertDropDownListSelectedIndex(string dropDownListName, int selectedIndex)
        {
            var dropDownList = ReflectionHelper.GetFieldInfoFromInstanceByName(_page, dropDownListName)
                       .GetValue(_page) as DropDownList;
            dropDownList.ShouldSatisfyAllConditions(
                () => dropDownList.ShouldNotBeNull(),
                () => dropDownList.SelectedIndex.ShouldBe(selectedIndex));
        }

        private void InitializeRedirectRadioAndDropDownList(string selectedValue)
        {
            InitializeRadioButton("rblRedirectThankYou", selectedValue);

            var dropDownList = new DropDownList();
            dropDownList.Items.Add(selectedValue);
            ReflectionHelper.SetValue(_page, "ddlRedirectDelay", dropDownList);
        }

        private void InitializeRadioButton(string radioButtonName, string selectedValue)
        {
            var radioButtonList = new RadioButtonList();
            radioButtonList.Items.Add(selectedValue);
            radioButtonList.SelectedValue = selectedValue;
            ReflectionHelper.SetValue(_page, radioButtonName, radioButtonList);
        }

        private void AssertTableVisibility(string tableName, bool isVisible)
        {
            var htmlTable = ReflectionHelper.GetFieldInfoFromInstanceByName(_page, tableName)
                .GetValue(_page) as HtmlTable;
            htmlTable.ShouldSatisfyAllConditions(
                () => htmlTable.ShouldNotBeNull(),
                () => htmlTable.Visible.ShouldBe(isVisible));
        }

        private void AssertDataTableDataLength(string columnfilter, int dataCount)
        {
            var dtReason = ReflectionHelper.GetFieldInfoFromInstanceTypeByName(typeof(SubscriptionManagementEdit), ReasonDataTableName)
                .GetValue(null) as DataTable;
            dtReason.ShouldNotBeNull();

            var columnData = dtReason.Select(columnfilter);
            columnData.ShouldSatisfyAllConditions(
                () => columnData.ShouldNotBeNull(),
                () => columnData.Length.ShouldBe(dataCount));
        }

        private void AssertButtonCommand(string buttonName, string commandArgument)
        {
            var button = ReflectionHelper.GetFieldInfoFromInstanceByName(_page, buttonName)
                       .GetValue(_page) as Button;
            button.ShouldSatisfyAllConditions(
                () => button.ShouldNotBeNull(),
                () => button.CommandArgument.ShouldBe(commandArgument));
        }

        private void AssertTextBox(string textBoxName, string text)
        {
            var textBox = ReflectionHelper.GetFieldInfoFromInstanceByName(_page, textBoxName)
                       .GetValue(_page) as TextBox;
            textBox.ShouldSatisfyAllConditions(
                () => textBox.ShouldNotBeNull(),
                () => textBox.Text.ShouldBe(text));
        }

        private void InitializeReasonTable(string reasonID)
        {
            InitializeReasonColumns();
            AddNewRow(ReasonSortOrder);
            AddNewRow(ReasonSortOrder + 1);
            ReflectionHelper.SetValue(_page, ReasonDataTableName, _dtReason);
        }

        private void InitializeReasonColumns()
        {
            _dtReason.Columns.Add(FieldReasonName, typeof(string));
            _dtReason.Columns.Add(FieldReasonID, typeof(string));
            _dtReason.Columns.Add(FieldReasonSortOrder, typeof(int));
            _dtReason.Columns.Add(FieldReasonIsDeleted, typeof(bool));
        }

        private void AddNewRow(int sortOrder)
        {
            var row = _dtReason.NewRow();
            row[FieldReasonID] = ReasonCommandArgument;
            row[FieldReasonName] = FieldReasonName;
            row[FieldReasonIsDeleted] = false;
            row[FieldReasonSortOrder] = sortOrder;
            _dtReason.Rows.Add(row);
        }

        private void InitializeReasonControls(string reasonText = "", string commandArgument = "")
        {
            ReflectionHelper.SetValue(_page, "txtReasonLabelEdit", new TextBox() { Text = reasonText });
            ReflectionHelper.SetValue(_page, "btnSaveReason", new Button() { CommandArgument = commandArgument });
            ReflectionHelper.SetValue(_page, "mpeEditReason", new ModalPopupExtender());
        }

        private static string GetExpectedErrorMessage(ECNError error)
        {
            return $"<br/>{error.Entity}: {error.ErrorMessage}";
        }

        private void SetStaticFields()
        {
            foreach (var staticField in _staticFields)
            {
                _pagePrivate.SetField(staticField.Key, _staticFlags, staticField.Value);
            }
        }

        private void CommonShims()
        {
            var communicatorMasterPage = new CommunicatorMasterPage();
            ShimPage.AllInstances.MasterGet = page =>
            {
                return communicatorMasterPage;
            };
            var response = new HttpResponse(_responseStreamWriter);
            ShimPage.AllInstances.ResponseGet = page =>
            {
                return response;
            };
            var request = new HttpRequest(string.Empty, "http://invalidurl", string.Empty);
            ShimPage.AllInstances.RequestGet = page =>
            {
                return request;
            };
            ShimHttpResponse.AllInstances.RedirectString = (instance, url) =>
            {
                _responseRedirectUrl = url;
            };
            var ecnSession = CreateECNUserSession();
            ShimCommunicator.AllInstances.UserSessionGet = instance =>
            {
                return ecnSession;
            };
            ShimSubscriptionManagement.SaveSubscriptionManagementUser = (instance, user) =>
            {
                if (_subscriptionManagementSaveException != null)
                {
                    var exception = _subscriptionManagementSaveException;
                    _subscriptionManagementSaveException = null;
                    throw exception;
                }
                return _subscriptionManagementSaveResult;
            };
            ShimSubscriptionManagementReason.SaveSubscriptionManagementReason = reason =>
            {
                return default(int);
            };
            ShimSubscriptionManagementUDF.DeleteInt32UserNullableOfInt32 = (smgId, user, smgUdfId) =>
            {
                if(_subscriptionManagementUDFDeleteException != null)
                {
                    var exception = _subscriptionManagementUDFDeleteException;
                    _subscriptionManagementUDFDeleteException = null;
                    throw exception;
                }
                _subscriptionManagementUDFDeleteId = smgId;
            };
            ShimSubscriptionManagementUDF.SaveSubsriptionManagementUDF = subscriptionManagementUDF =>
            {
                if (_subscriptionManagementUDFSaveException != null)
                {
                    var exception = _subscriptionManagementUDFSaveException;
                    _subscriptionManagementUDFSaveException = null;
                    throw exception;
                }
                _subscriptionManagementUDFSaveInput = subscriptionManagementUDF;
            };
            ShimSubscriptionManagementReason.GetBySMIDInt32 = smgId =>
            {

                return new List<SubscriptionManagementReason>
                {
                    new SubscriptionManagementReason
                    {
                        SubscriptionManagementReasonID = AnyNumber,
                        SubscriptionManagementID = -1,
                        Reason = string.Empty,
                        IsDeleted = false,
                        CreatedUserID = null,
                        CreatedDate = null,
                        UpdatedDate = null,
                        UpdatedUserID = null,
                        SortOrder = null
                    },
                    new SubscriptionManagementReason
                    {
                        SubscriptionManagementReasonID = AnyOtherNumber,
                        SubscriptionManagementID = -1,
                        Reason = string.Empty,
                        IsDeleted = false,
                        CreatedUserID = null,
                        CreatedDate = null,
                        UpdatedDate = null,
                        UpdatedUserID = null,
                        SortOrder = null
                    }
                };
            };
            ShimBaseDataBoundControl.AllInstances.DataBind = instance => { };
            ShimCustomer.GetByBaseChannelIDInt32 = channelId =>
            {
                return new List<Customer>
                {
                    new Customer()
                };
            };
            ShimSubscriptionManagementGroup.SaveSubscriptionManagementGroup = group =>
            {
                if (_groupSaveException != null)
                {
                    var exception = _groupSaveException;
                    _groupSaveException = null;
                    throw exception;
                }
                _groupSaveInput = group;
                return _groupSaveResult;
            };
            ShimSubscriptionManagementGroup.DeleteInt32Int32User = (smId, smgId, user) =>
            {
                if (_groupDeleteException != null)
                {
                    var exception = _groupDeleteException;
                    _groupDeleteException = null;
                    throw exception;
                }
                _groupDeleteSMId = smId;
            };
        }

        private void SetReasonTable(DataTable table)
        {
            _pagePrivate.SetField(ReasonDataTableName, _staticFlags, table);
        }

        private DataTable GetReasonTable()
        {
            var result = new DataTable();
            result.Columns.Add(FieldReasonName, typeof(string));
            result.Columns.Add(FieldReasonID, typeof(string));
            result.Columns.Add(FieldReasonSortOrder, typeof(int));
            result.Columns.Add(FieldReasonIsDeleted, typeof(bool));
            var lines = new[]
            {
                new
                {
                    Reason = "Email Frequency",
                    Id = Guid.NewGuid().ToString(),
                    SortOrder = 1,
                    IsDeleted = false
                },
                new
                {
                    Reason = "Email Volume",
                    Id = Guid.NewGuid().ToString(),
                    SortOrder = 2,
                    IsDeleted = false
                },
                new
                {
                    Reason = "Content not relevant",
                    Id = Guid.NewGuid().ToString(),
                    SortOrder = 3,
                    IsDeleted = false
                },
                new
                {
                    Reason = "Signed up for one-time email",
                    Id = Guid.NewGuid().ToString(),
                    SortOrder = 4,
                    IsDeleted = false
                },
                new
                {
                    Reason = "Circumstances changed(moved, married, changed jobs, etc.)",
                    Id = Guid.NewGuid().ToString(),
                    SortOrder = 5,
                    IsDeleted = false
                },
                new
                {
                    Reason = "Prefer to get information another way",
                    Id = Guid.NewGuid().ToString(),
                    SortOrder = 6,
                    IsDeleted = false
                },
                new
                {
                    Reason = GetAnyString(),
                    Id = AnyNumber.ToString(),
                    SortOrder = 6,
                    IsDeleted = false
                },
                new
                {
                    Reason = GetAnyString(),
                    Id = AnyOtherNumber.ToString(),
                    SortOrder = 6,
                    IsDeleted = true
                }
            };
            foreach (var line in lines)
            {
                var row = result.NewRow();
                row[FieldReasonName] = line.Reason;
                row[FieldReasonID] = line.Id;
                row[FieldReasonSortOrder] = line.SortOrder;
                row[IsDeletedColumn] = line.IsDeleted;
                result.Rows.Add(row);
            }
            return result;
        }

        private ECNSession CreateECNUserSession()
        {
            ShimECNSession.Constructor = instance => { };
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var result = typeof(ECNSession).
                GetConstructor(flags, null, new Type[0], null)
                ?.Invoke(new object[0]) as ECNSession;
            if (result != null)
            {
                _currentUser = new User();
                result.CurrentUser = _currentUser;
                result.CurrentBaseChannel = new BaseChannel
                {
                    BaseChannelID = _currentUserBaseChannelId
                };
            }
            return result;
        }

        private void CallbtnSavePage_Click()
        {
            const string MethodName = "btnSavePage_Click";
            _pagePrivate.Invoke(MethodName, new object[] { null, null });
        }

        private void InitializePageFields()
        {
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic; ;
            var fields = _page.GetType()
                .GetFields(flags)
                .Where(field => field.GetValue(_page) == null)
                .ToList();
            foreach (var field in fields)
            {
                var value = field.FieldType
                    .GetConstructor(flags, null, new Type[0], null)
                    ?.Invoke(new object[0]);
                field.SetValue(_page, value);
            }
            _rblVisibilityReason = GetControl<RadioButtonList>("rblVisibilityReason");
            _rblReasonControlType = GetControl<RadioButtonList>("rblReasonControlType");
            _rblRedirectThankYou = GetControl<RadioButtonList>("rblRedirectThankYou");
            _chkIncludeMasterSuppressed = GetControl<CheckBox>("chkIncludeMasterSuppressed");
            _txtMSMessage = GetControl<TextBox>("txtMSMessage");
            _txtName = GetControl<TextBox>("txtName");
            _txtAdminEmail = GetControl<TextBox>("txtAdminEmail");
            _txtEmailFooter = GetControl<TextBox>("txtEmailFooter");
            _txtEmailHeader = GetControl<TextBox>("txtEmailHeader");
            _txtPageFooter = GetControl<TextBox>("txtPageFooter");
            _txtPageHeader = GetControl<TextBox>("txtPageHeader");
            _txtReasonLabel = GetControl<TextBox>("txtReasonLabel");
            _txtThankYouMessage = GetControl<TextBox>("txtThankYouMessage");
            _txtRedirectURL = GetControl<TextBox>("txtRedirectURL");
            _lblErrorMessage = GetControl<Label>("lblErrorMessage");
            _phError = GetControl<PlaceHolder>("phError");
            _ddlRedirectDelay = GetControl<DropDownList>("ddlRedirectDelay");
        }

        private T GetControl<T>(string name) where T : class
        {
            var result = _pagePrivate.GetField(name) as T;
            result.ShouldNotBeNull();
            return result;
        }

        private string GetAnyString(bool excludeDash = false)
        {
            var result = Guid.NewGuid()
                .ToString();
            if (excludeDash)
            {
                result = result.Replace("-", string.Empty);
            }
            return result;
        }

        private int GetAnyNumber()
        {
            return _random.Next(10, 1000);
        }
    }
}
