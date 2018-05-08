using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Fakes;
using System.Web;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.activityengines.includes;
using ecn.common.classes.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ecn.activityengines.Tests.Includes
{
    [TestFixture]
    public partial class EmailProfileBaseTest
    {
        private emailProfile_base _emailProfileBaseObject;
        private PrivateObject _emailProfileBasePrivateObject;
        private IDisposable _shimsContext;
        private ShimHttpRequest _shimHttpRequest;
        private HttpRequest _httpRequest;
        private NameValueCollection _queryStringCollection;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _shimsContext = ShimsContext.Create();
            ShimUserControl.AllInstances.RequestGet = (userControl) => { return _httpRequest; };
            ShimHttpRequest.AllInstances.QueryStringGet = (request) => { return _queryStringCollection; };

            _shimHttpRequest = new ShimHttpRequest();
            _emailProfileBaseObject = new emailProfile_base();
            _httpRequest = _shimHttpRequest.Instance;
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _shimsContext.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            _queryStringCollection = new NameValueCollection();
            _emailProfileBaseObject = new emailProfile_base();
            _emailProfileBasePrivateObject = new PrivateObject(_emailProfileBaseObject);

            InitializePageControls();
        }

        [TearDown]
        public void TearDown()
        {
            DisposePageControls();
        }

        [Test]
        public void GetFromQueryString_IfEmailIdProvidedInQueryString_ReturnsEmailId()
        {
            // Arrange
            _httpRequest.QueryString[EmailIdQueryStringKey] = EmailIdValue;

            // Act
            var returnedEmailId = (string)_emailProfileBasePrivateObject.Invoke(GetFromQueryStringMethodName, EmailIdQueryStringKey, EmailIdErrorMessage);

            // Assert
            returnedEmailId.ShouldBe(EmailIdValue);
        }

        [Test]
        public void GetFromQueryString_IfEmailAddressProvidedInQueryString_ReturnsEmailAddress()
        {
            // Arrange
            _httpRequest.QueryString[EmailAddressQueryStringKey] = EmailAddressValue;

            // Act
            var returnedEmailAddress = (string)_emailProfileBasePrivateObject.Invoke(GetFromQueryStringMethodName, EmailAddressQueryStringKey, EmailAddressErrorMessage);

            // Assert
            returnedEmailAddress.ShouldBe(EmailAddressValue);
        }

        [Test]
        public void GetFromQueryString_IfGroupIdProvidedInQueryString_ReturnsGroupId()
        {
            // Arrange
            _httpRequest.QueryString[GroupIdQueryStringKey] = GroupIdValue;

            // Act
            var returnedGroupId = (string)_emailProfileBasePrivateObject.Invoke(GetFromQueryStringMethodName, GroupIdQueryStringKey, GroupIdErrorMessage);

            // Assert
            returnedGroupId.ShouldBe(GroupIdValue);
        }

        [Test]
        public void GetFromQueryString_IfExpectedKeyDoesNotExistInQueryString_ReturnsEmptyStringAndSetsMessageLabel()
        {
            // Arrange
            // set no query string to get error

            // Act
            var returnedValue = (string)_emailProfileBasePrivateObject.Invoke(GetFromQueryStringMethodName, EmailIdQueryStringKey, EmailIdErrorMessage);

            // Assert
            returnedValue.ShouldBeEmpty();
            lblMessageLabel.ShouldSatisfyAllConditions(
                () => lblMessageLabel.Visible.ShouldBeTrue(),
                () => lblMessageLabel.Text.ShouldEndWith(EmailIdErrorMessage));
            btnEditProfileButton.Enabled.ShouldBeFalse();
        }

        [Test]
        public void LoadProfileData_WhenCalled_FillsForm()
        {
            // Arrange
            var dataRowValues = GetDataRowDictionary();

            var shimDataRow = new ShimDataRow();
            shimDataRow.ItemGetString = (colName) => { return dataRowValues[colName]; };

            var dataRows = new DataRow[1];
            dataRows[0] = shimDataRow.Instance;

            var shimDataRowCollection = new ShimDataRowCollection();
            shimDataRowCollection.GetEnumerator = () => { return dataRows.GetEnumerator(); };

            var shimDataTable = new ShimDataTable();
            shimDataTable.RowsGet = () => { return shimDataRowCollection.Instance; };
            ShimDataFunctions.GetDataTableString = (sqlQuery) => { return shimDataTable.Instance; };

            // Act
            _emailProfileBasePrivateObject.Invoke(LoadProfileDataMethodName);

            // Assert
            AssertPageControlValuesForLoadProfileData();
        }

        private void AssertPageControlValuesForLoadProfileData()
        {
            txtEmailAddress.Text.ShouldBe(EmailAddressValue);
            txtTitle.Text.ShouldBe(TitleValue);
            txtFirstName.Text.ShouldBe(FirstNameValue);
            txtLastName.Text.ShouldBe(LastNameValue);
            txtCompanyName.Text.ShouldBe(CompanyNameValue);
            txtOccupation.Text.ShouldBe(OccupationValue);
            txtAddress.Text.ShouldBe(AddressValue);
            txtAddress2.Text.ShouldBe(Address2Value);
            txtCity.Text.ShouldBe(CityValue);
            ddlState.SelectedItem.Text.ShouldBe(StateValue);
            txtZip.Text.ShouldBe(ZipValue);
            txtCountry.Text.ShouldBe(CountryValue);
            txtVoice.Text.ShouldBe(VoiceValue);
            txtMobile.Text.ShouldBe(MobileValue);
            txtFax.Text.ShouldBe(FaxValue);
            txtIncome.Text.ShouldBe(IncomeValue);
            ddlGender.Text.ShouldBe(GenderValue);
            txtAge.Text.ShouldBe(AgeValue);
            txtWebsite.Text.ShouldBe(WebsiteValue);
            txtBirthDate.Text.ShouldBe(BirthDateValue.ToString("MM/dd/yyyy"));
            txtUser1.Text.ShouldBe(User1Value);
            txtUser2.Text.ShouldBe(User2Value);
            txtUser3.Text.ShouldBe(User3Value);
            txtUser4.Text.ShouldBe(User4Value);
            txtUser5.Text.ShouldBe(User5Value);
            txtUser6.Text.ShouldBe(User6Value);
            txtUserEvent1.Text.ShouldBe(UserEvent1Value);
            txtUserEvent1Date.Text.ShouldBe(UserEvent1DateValue.ToString("MM/dd/yyyy"));
            txtUserEvent2.Text.ShouldBe(UserEvent2Value);
            txtUserEvent2Date.Text.ShouldBe(UserEvent2DateValue.ToString("MM/dd/yyyy"));
            txtPassword.Text.ShouldBe(PasswordValue);
            txtBounceScore.Text.ShouldBe(BounceScoreValue);
            txtSoftBounceScore.Text.ShouldBe(SoftBounceScoreValue);
        }

        private Dictionary<string, object> GetDataRowDictionary()
        {
            var colValueDictionary = new Dictionary<string, object>();

            colValueDictionary.Add(EmailAddressColumnName, EmailAddressValue);
            colValueDictionary.Add(TitleColumnName, TitleValue);
            colValueDictionary.Add(FirstNameColumnName, FirstNameValue);
            colValueDictionary.Add(LastNameColumnName, LastNameValue);
            colValueDictionary.Add(CompanyNameColumnName, CompanyNameValue);
            colValueDictionary.Add(OccupationColumnName, OccupationValue);
            colValueDictionary.Add(AddressColumnName, AddressValue);
            colValueDictionary.Add(Address2ColumnName, Address2Value);
            colValueDictionary.Add(CityColumnName, CityValue);
            colValueDictionary.Add(StateItemColumnName, StateValue);
            colValueDictionary.Add(ZipColumnName, ZipValue);
            colValueDictionary.Add(CountryColumnName, CountryValue);
            colValueDictionary.Add(VoiceColumnName, VoiceValue);
            colValueDictionary.Add(MobileColumnName, MobileValue);
            colValueDictionary.Add(FaxColumnName, FaxValue);
            colValueDictionary.Add(IncomeColumnName, IncomeValue);
            colValueDictionary.Add(GenderColumnName, GenderValue);
            colValueDictionary.Add(AgeColumnName, AgeValue);
            colValueDictionary.Add(WebsiteColumnName, WebsiteValue);
            colValueDictionary.Add(BirthDateColumnName, BirthDateValue);
            colValueDictionary.Add(User2ColumnName, User2Value);
            colValueDictionary.Add(User1ColumnName, User1Value);
            colValueDictionary.Add(User3ColumnName, User3Value);
            colValueDictionary.Add(User4ColumnName, User4Value);
            colValueDictionary.Add(User5ColumnName, User5Value);
            colValueDictionary.Add(User6ColumnName, User6Value);
            colValueDictionary.Add(UserEvent1ColumnName, UserEvent1Value);
            colValueDictionary.Add(UserEvent1DateColumnName, UserEvent1DateValue);
            colValueDictionary.Add(UserEvent2ColumnName, UserEvent2Value);
            colValueDictionary.Add(UserEvent2DateColumnName, UserEvent2DateValue);
            colValueDictionary.Add(BounceScoreColumnName, BounceScoreValue);
            colValueDictionary.Add(SoftBounceScoreColumnName, SoftBounceScoreValue);
            colValueDictionary.Add(PasswordColumnName, PasswordValue);

            return colValueDictionary;
        }

        private void InitializePageControls()
        {
            lblMessageLabel = new Label();
            txtEmailAddress = new TextBox();
            txtTitle = new TextBox();
            txtFirstName = new TextBox();
            txtLastName = new TextBox();
            txtFullName = new TextBox();
            txtCompanyName = new TextBox();
            txtOccupation = new TextBox();
            txtAddress = new TextBox();
            txtAddress2 = new TextBox();
            txtCity = new TextBox();
            ddlState = new DropDownList();
            ddlState.Items.Add(StateControlItem1);
            ddlState.Items.Add(StateControlItem2);
            txtZip = new TextBox();
            txtCountry = new TextBox();
            txtVoice = new TextBox();
            txtMobile = new TextBox();
            txtFax = new TextBox();
            txtIncome = new TextBox();
            ddlGender = new DropDownList();
            ddlGender.Items.Add(GenderControlItem1);
            ddlGender.Items.Add(GenderControlItem2);
            txtAge = new TextBox();
            txtWebsite = new TextBox();
            txtBirthDate = new TextBox();
            txtUser1 = new TextBox();
            txtUser2 = new TextBox();
            txtUser3 = new TextBox();
            txtUser4 = new TextBox();
            txtUser5 = new TextBox();
            txtUser6 = new TextBox();
            txtUserEvent1 = new TextBox();
            txtUserEvent1Date = new TextBox();
            txtUserEvent2 = new TextBox();
            txtUserEvent2Date = new TextBox();
            btnEditProfileButton = new Button();
            txtBounceScore = new TextBox();
            txtSoftBounceScore = new TextBox();
            txtPassword = new TextBox();

            _emailProfileBasePrivateObject.SetField(MessageLabelControlName, lblMessageLabel);
            _emailProfileBasePrivateObject.SetField(EmailAddressControlName, txtEmailAddress);
            _emailProfileBasePrivateObject.SetField(TitleControlName, txtTitle);
            _emailProfileBasePrivateObject.SetField(FirstNameControlName, txtFirstName);
            _emailProfileBasePrivateObject.SetField(LastNameControlName, txtLastName);
            _emailProfileBasePrivateObject.SetField(FullNameControlName, txtFullName);
            _emailProfileBasePrivateObject.SetField(CompanyNameControlName, txtCompanyName);
            _emailProfileBasePrivateObject.SetField(OccupationControlName, txtOccupation);
            _emailProfileBasePrivateObject.SetField(AddressControlName, txtAddress);
            _emailProfileBasePrivateObject.SetField(Address2ControlName, txtAddress2);
            _emailProfileBasePrivateObject.SetField(CityControlName, txtCity);
            _emailProfileBasePrivateObject.SetField(StateControlName, ddlState);
            _emailProfileBasePrivateObject.SetField(ZipControlName, txtZip);
            _emailProfileBasePrivateObject.SetField(CountryControlName, txtCountry);
            _emailProfileBasePrivateObject.SetField(VoiceControlName, txtVoice);
            _emailProfileBasePrivateObject.SetField(MobileControlName, txtMobile);
            _emailProfileBasePrivateObject.SetField(FaxControlName, txtFax);
            _emailProfileBasePrivateObject.SetField(IncomeControlName, txtIncome);
            _emailProfileBasePrivateObject.SetField(GenderControlName, ddlGender);
            _emailProfileBasePrivateObject.SetField(AgeControlName, txtAge);
            _emailProfileBasePrivateObject.SetField(WebsiteControlName, txtWebsite);
            _emailProfileBasePrivateObject.SetField(BirthDateControlName, txtBirthDate);
            _emailProfileBasePrivateObject.SetField(User1ControlName, txtUser1);
            _emailProfileBasePrivateObject.SetField(User2ControlName, txtUser2);
            _emailProfileBasePrivateObject.SetField(User3ControlName, txtUser3);
            _emailProfileBasePrivateObject.SetField(User4ControlName, txtUser4);
            _emailProfileBasePrivateObject.SetField(User5ControlName, txtUser5);
            _emailProfileBasePrivateObject.SetField(User6ControlName, txtUser6);
            _emailProfileBasePrivateObject.SetField(UserEvent1ControlName, txtUserEvent1);
            _emailProfileBasePrivateObject.SetField(UserEvent1DateControlName, txtUserEvent1Date);
            _emailProfileBasePrivateObject.SetField(UserEvent2ControlName, txtUserEvent2);
            _emailProfileBasePrivateObject.SetField(UserEvent2DateControlName, txtUserEvent2Date);
            _emailProfileBasePrivateObject.SetField(EditProfileButtonControlName, btnEditProfileButton);
            _emailProfileBasePrivateObject.SetField(BounceScoreControlName, txtBounceScore);
            _emailProfileBasePrivateObject.SetField(SoftBounceScoreControlName, txtSoftBounceScore);
            _emailProfileBasePrivateObject.SetField(PasswordControlName, txtPassword);
        }

        private void DisposePageControls()
        {
            lblMessageLabel.Dispose();
            txtEmailAddress.Dispose();
            txtTitle.Dispose();
            txtFirstName.Dispose();
            txtLastName.Dispose();
            txtFullName.Dispose();
            txtCompanyName.Dispose();
            txtOccupation.Dispose();
            txtAddress.Dispose();
            txtAddress2.Dispose();
            txtCity.Dispose();
            ddlState.Dispose();
            txtZip.Dispose();
            txtCountry.Dispose();
            txtVoice.Dispose();
            txtMobile.Dispose();
            txtFax.Dispose();
            txtIncome.Dispose();
            ddlGender.Dispose();
            txtAge.Dispose();
            txtWebsite.Dispose();
            txtBirthDate.Dispose();
            txtUser1.Dispose();
            txtUser2.Dispose();
            txtUser3.Dispose();
            txtUser4.Dispose();
            txtUser5.Dispose();
            txtUser6.Dispose();
            txtUserEvent1.Dispose();
            txtUserEvent1Date.Dispose();
            txtUserEvent2.Dispose();
            txtUserEvent2Date.Dispose();
            btnEditProfileButton.Dispose();
            txtBounceScore.Dispose();
            txtSoftBounceScore.Dispose();
            txtPassword.Dispose();
        }
    }
}