using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.WebControls;
using ecn.activityengines.includes;
using ecn.activityengines.Tests.Helpers;
using ecn.activityengines.Tests.Setup.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace ecn.activityengines.Tests.Includes
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class EmailProfileBase : PageHelper
    {
        private const int EmailId = 12345;
        private const int GroupId = 23456;
        private const string ConnectionString = "ConnectionString";
        private SqlConnectionMock _sqlConnectionMock;
        private SqlCommandMock _sqlCommandMock;
        private DataFunctionsMock _dataFunctionsMock;
        private emailProfile_base _control;
        private PrivateObject _controlPrivate;
        private NameValueCollection _appSettings;
        private TextBox _birthDate;
        private TextBox _userEvent1Date;
        private TextBox _userEvent2Date;
        private TextBox _emailAddress;
        private TextBox _title;
        private TextBox _firstName;
        private TextBox _lastName;
        private TextBox _companyName;
        private TextBox _occupation;
        private TextBox _address;
        private TextBox _address2;
        private TextBox _city;
        private TextBox _zip;
        private TextBox _country;
        private TextBox _voice;
        private TextBox _mobile;
        private TextBox _fax;
        private TextBox _website;
        private TextBox _age;
        private TextBox _income;
        private TextBox _user1;
        private TextBox _user2;
        private TextBox _user3;
        private TextBox _user4;
        private TextBox _user5;
        private TextBox _user6;
        private TextBox _userEvent1;
        private TextBox _userEvent2;
        private DropDownList _state;
        private DropDownList _gender;

        [SetUp]
        public void Setup()
        {
            _sqlConnectionMock = new SqlConnectionMock();
            _sqlCommandMock = new SqlCommandMock();

            _dataFunctionsMock = new DataFunctionsMock();
            _control = new emailProfile_base();
            _controlPrivate = new PrivateObject(_control);
            _appSettings = new NameValueCollection();
            InitializeAllControls(_control);
            ReadControlFields();
            ShimConfigurationManager.AppSettingsGet = () => _appSettings;
            _appSettings.Add("connString", ConnectionString);
            QueryString.Clear();
            QueryString.Add("eID", EmailId.ToString());
            QueryString.Add("gID", GroupId.ToString());
        }

        [Test]
        public void UpdateEmail_ShouldCleanStrings()
        {
            // Arrange
            var textBoxes = GetTextBoxes();
            foreach (var textBox in textBoxes)
            {
                textBox.Text = GetString();
            }

            // Act
            _control.UpdateEmail(null, EventArgs.Empty);

            // Assert
            foreach (var textBox in textBoxes)
            {
                _dataFunctionsMock.Verify(functions => functions.CleanString(textBox.Text));
            }
        }

        [Test]
        public void UpdateEmail_WhenExceptionOccursWhileReadingProfileData_ShowsErrorMessage()
        {
            // Arrange
            var messageLabel = GetReferenceField<Label>("MessageLabel");
            _dataFunctionsMock.Setup(functions => functions.CleanString(It.IsAny<string>()))
                .Throws(new Exception());

            // Act
            _control.UpdateEmail(null, EventArgs.Empty);

            // Assert
            messageLabel.ShouldSatisfyAllConditions(
                () => messageLabel.Visible.ShouldBeTrue(),
                () => messageLabel.Text.ShouldBe("ERROR: Error reading your Profile Information"));
            
        }

        [Test]
        public void UpdateEmail_WhenExceptionOccursWhileUpdatingEmailProfile_ShowsErrorMessage()
        {
            // Arrange
            var messageLabel = GetReferenceField<Label>("MessageLabel");
            _sqlConnectionMock.Setup(connection => connection.Constructor(It.IsAny<string>()))
                .Throws(new Exception());

            // Act
            _control.UpdateEmail(null, EventArgs.Empty);

            // Assert
            messageLabel.ShouldSatisfyAllConditions(
                () => messageLabel.Visible.ShouldBeTrue(),
                () => messageLabel.Text.ShouldBe("ERROR: Error Occured Updating your Profile Information."));
        }

        [Test]
        public void UpdateEmail_UpdateDatabaseSuccessfully_ShouldShowMessage()
        {
            // Arrange
            var dateTime = DateTime.Now;
            var sqlDateTime = (SqlDateTime)dateTime;
            var validDateString = dateTime.ToString();
            _birthDate.Text = validDateString;
            _userEvent1Date.Text = validDateString;
            _userEvent2Date.Text = validDateString;
            var parameters = new[]
            {
                new
                {
                    Name = "@birthdate",
                    Value = sqlDateTime
                },
                new
                {
                    Name = "@user_event1_date",
                    Value = sqlDateTime
                },
                new
                {
                    Name = "@user_event2_date",
                    Value = sqlDateTime
                }
            };
            var messageLabel = GetReferenceField<Label>("MessageLabel");

            var updateCommandText = (string)_controlPrivate.Invoke("BuildUpdateEmailProfileSqlCommandText");
            var groupUpdateCommandText = (string)_controlPrivate.Invoke("BuildUpdateEmailGroupsSqlCommandText");

            // Act
            _control.UpdateEmail(null, EventArgs.Empty);

            // Assert
            _sqlCommandMock.VerifySet(command => command.CommandText = updateCommandText);
            _sqlCommandMock.VerifySet(command => command.CommandText = groupUpdateCommandText);
            _sqlCommandMock.VerifySet(command => command.CommandTimeout = 0);
            _sqlCommandMock.Verify(command => command.ExecuteNonQuery(), Times.Exactly(2));
            _sqlConnectionMock.Verify(connection => connection.Open(), Times.Exactly(2));
            _sqlConnectionMock.Verify(connection => connection.Close(), Times.Exactly(2));
            messageLabel.Visible.ShouldBeTrue();
            messageLabel.Text.ShouldBe("Successfully Updated your Profile");
        }

        private T GetReferenceField<T>(string name) where T : class
        {
            var result = _controlPrivate.GetField(name) as T;
            result.ShouldNotBeNull();
            return result;
        }

        private IEnumerable<TextBox> GetTextBoxes()
        {
            return new[]
            {
                _title,
                _firstName,
                _lastName,
                _companyName,
                _occupation,
                _address,
                _address2,
                _city,
                _zip,
                _country,
                _voice,
                _mobile,
                _fax,
                _website,
                _age,
                _income,
                _user1,
                _user2,
                _user3,
                _user4,
                _user5,
                _user6,
                _userEvent1,
                _userEvent2
            };
        }

        private void ReadControlFields()
        {
            _birthDate = GetReferenceField<TextBox>("BirthDate");
            _userEvent1Date = GetReferenceField<TextBox>("UserEvent1Date");
            _userEvent2Date = GetReferenceField<TextBox>("UserEvent2Date");
            _emailAddress = GetReferenceField<TextBox>("EmailAddress");
            _title = GetReferenceField<TextBox>("Title");
            _firstName = GetReferenceField<TextBox>("FirstName");
            _lastName = GetReferenceField<TextBox>("LastName");
            _companyName = GetReferenceField<TextBox>("CompanyName");
            _occupation = GetReferenceField<TextBox>("Occupation");
            _address = GetReferenceField<TextBox>("Address");
            _address2 = GetReferenceField<TextBox>("Address2");
            _city = GetReferenceField<TextBox>("City");
            _zip = GetReferenceField<TextBox>("Zip");
            _country = GetReferenceField<TextBox>("Country");
            _voice = GetReferenceField<TextBox>("Voice");
            _mobile = GetReferenceField<TextBox>("Mobile");
            _fax = GetReferenceField<TextBox>("Fax");
            _website = GetReferenceField<TextBox>("Website");
            _age = GetReferenceField<TextBox>("Age");
            _income = GetReferenceField<TextBox>("Income");
            _user1 = GetReferenceField<TextBox>("User1");
            _user2 = GetReferenceField<TextBox>("User2");
            _user3 = GetReferenceField<TextBox>("User3");
            _user4 = GetReferenceField<TextBox>("User4");
            _user5 = GetReferenceField<TextBox>("User5");
            _user6 = GetReferenceField<TextBox>("User6");
            _userEvent1 = GetReferenceField<TextBox>("UserEvent1");
            _userEvent2 = GetReferenceField<TextBox>("UserEvent2");
            _state = GetReferenceField<DropDownList>("State");
            _gender = GetReferenceField<DropDownList>("Gender");
            _state.Items.Add(GetString());
            _state.SelectedIndex = 0;
            _gender.Items.Add(GetString());
            _state.SelectedIndex = 0;
        }

        private string GetString()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
