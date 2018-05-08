using System.Web;
using System.Collections.Generic;
using Shouldly;
using NUnit.Framework;
using KMEntities;
using KMManagers.Tests.Helpers;

namespace KMManagers.Tests
{
    [TestFixture]
    public partial class PrepopulatorTest
    {   
        private const string TestedMethodName_Get = "Get";

        [Test]
        public void GetPrepopulator_ControlTypeEmail_WithError()
        {
            //Arrange
            InitializeTestParams(_emailTypeValue);

            var methodArguments = new object[] { _httpRequest };

            //Act
            var returnResult = (string)_prepopulator.Invoke(TestedMethodName_Get, methodArguments);

            //Assert
            returnResult.ShouldBeNullOrEmpty();
        }
        
        [Test]
        public void GetPrepopulator_ControlTypeEmail_WithSuccess()
        {
            //Arrange
            InitializeTestParams(_emailTypeValue);
            _httpRequestCookies.Add(new HttpCookie(DummyGuidStr, FieldLabelValue));
            
            var methodArguments = new object[] { _httpRequest };

            //Act
            var returnResult = (string)_prepopulator.Invoke(TestedMethodName_Get, methodArguments);
            var errorWasLogged = _applicationErrorWasLogged;

            //Assert
            returnResult.ShouldNotBeNullOrEmpty();
            errorWasLogged.ShouldBeFalse();
        }

        [Test]
        public void GetPrepopulator_ControlTypeEmail_WithoutChildAndWithoutDeserialization_WillCatchAndLogException()
        {
            //Arrange
            InitializeTestParams(_emailTypeValue);
            _httpRequestCookies.Add(new HttpCookie(DummyGuidStr, FieldLabelValue));

            _httpRequest = new HttpRequest(FieldLabelValue, BaseRequestUriNoChild, RequestQueryWithChild);
            CreateAPIRunnerBaseShim(SerilalizedValueWrongly);

            var methodArguments = new object[] { _httpRequest };

            //Act
            var returnResult = (string)_prepopulator.Invoke(TestedMethodName_Get, methodArguments);
            var errorWasLogged = _applicationErrorWasLogged;

            //Assert
            returnResult.ShouldNotBeNullOrEmpty();
            errorWasLogged.ShouldBeTrue();
        }

        [Test]
        public void GetPrepopulator_ControlTypeDatabase_WithoutQuery_FieldValueError()
        {
            //Arrange
            InitializeTestParams(_databaseTypeValue, null, GroupFieldIdInvalid);
            _httpRequestCookies.Add(new HttpCookie(DummyGuidStr, FieldLabelValue));
            _httpRequest = new HttpRequest(FieldLabelValue, BaseRequestUriNoQuery, RequestQueryWithChild);
            _groupDataFields.Add(KMManagerTestsHelper.CreateGroupDataField(GroupFieldIdInvalid, GroupFieldShortNameInvalid));

            var methodArguments = new object[] { _httpRequest };

            //Act
            var returnResult = (string)_prepopulator.Invoke(TestedMethodName_Get, methodArguments);
            var errorWasLogged = _applicationErrorWasLogged;

            //Assert
            returnResult.ShouldNotBeNullOrEmpty();
            errorWasLogged.ShouldBeFalse();
        }

        [Test]
        public void GetPrepopulator_ControlTypeDatabase_FieldValueNull()
        {
            //Arrange
            InitializeTestParams(_databaseTypeValue, null, null);
            _httpRequestCookies.Add(new HttpCookie(DummyGuidStr, FieldLabelValue));
            _groupDataFields.Add(KMManagerTestsHelper.CreateGroupDataField(GroupFieldIdPhone, GroupFieldShortNamePhone));

            var methodArguments = new object[] { _httpRequest };

            //Act
            var returnResult = (string)_prepopulator.Invoke(TestedMethodName_Get, methodArguments);
            var errorWasLogged = _applicationErrorWasLogged;

            //Assert
            returnResult.ShouldNotBeNullOrEmpty();
            errorWasLogged.ShouldBeFalse();
        }

        [Test]
        public void GetPrepopulator_ControlTypeDatabase_FieldPhone_WithSuccess()
        {
            //Arrange
            InitializeTestParams(_databaseTypeValue, null, GroupFieldIdPhone);
            _httpRequestCookies.Add(new HttpCookie(DummyGuidStr, FieldLabelValue));
            _groupDataFields.Add(KMManagerTestsHelper.CreateGroupDataField(GroupFieldIdPhone,GroupFieldShortNamePhone));

            var methodArguments = new object[] { _httpRequest };

            //Act
            var returnResult = (string)_prepopulator.Invoke(TestedMethodName_Get, methodArguments);
            var errorWasLogged = _applicationErrorWasLogged;

            //Assert
            returnResult.ShouldNotBeNullOrEmpty();
            errorWasLogged.ShouldBeFalse();
        }

        [Test]
        public void GetPrepopulator_ControlTypeDatabase_FieldPassword_WithSuccess()
        {
            //Arrange
            InitializeTestParams(_databaseTypeValue, null, GroupFieldIdPassword);
            _httpRequestCookies.Add(new HttpCookie(DummyGuidStr, FieldLabelValue));
            _groupDataFields.Add(KMManagerTestsHelper.CreateGroupDataField(GroupFieldIdPassword, GroupFieldShortNamePassword));

            var methodArguments = new object[] { _httpRequest };

            //Act
            var returnResult = (string)_prepopulator.Invoke(TestedMethodName_Get, methodArguments);
            var errorWasLogged = _applicationErrorWasLogged;

            //Assert
            returnResult.ShouldNotBeNullOrEmpty();
            errorWasLogged.ShouldBeFalse();
        }

        [Test]
        public void GetPrepopulator_ControlTypeDatabase_FieldState_WithSuccess()
        {
            //Arrange
            InitializeTestParams(_databaseTypeValue, null, GroupFieldIdState);
            _httpRequestCookies.Add(new HttpCookie(DummyGuidStr, FieldLabelValue));
            _groupDataFields.Add(KMManagerTestsHelper.CreateGroupDataField(GroupFieldIdState, GroupFieldShortNameState));

            var methodArguments = new object[] { _httpRequest };

            //Act
            var returnResult = (string)_prepopulator.Invoke(TestedMethodName_Get, methodArguments);
            var errorWasLogged = _applicationErrorWasLogged;

            //Assert
            returnResult.ShouldNotBeNullOrEmpty();
            errorWasLogged.ShouldBeFalse();
        }

        [Test]
        public void GetPrepopulator_ControlTypeTextArea_FieldWithError_WithSuccess()
        {
            //Arrange
            InitializeTestParams(FieldLabelValue, TextAreaType);
            _httpRequestCookies.Add(new HttpCookie(DummyGuidStr, FieldLabelValue));

            var methodArguments = new object[] { _httpRequest };

            //Act
            var returnResult = (string)_prepopulator.Invoke(TestedMethodName_Get, methodArguments);
            var errorWasLogged = _applicationErrorWasLogged;

            //Assert
            returnResult.ShouldNotBeNullOrEmpty();
            errorWasLogged.ShouldBeFalse();
        }

        [Test]
        public void GetPrepopulator_ControlTypeNewsLetter_FieldWithError_WithSuccess()
        {
            //Arrange
            InitializeTestParams(FieldLabelValue, NewsLetterType);
            _httpRequestCookies.Add(new HttpCookie(DummyGuidStr, FieldLabelValue));

            var methodArguments = new object[] { _httpRequest };

            //Act
            var returnResult = (string)_prepopulator.Invoke(TestedMethodName_Get, methodArguments);
            var errorWasLogged = _applicationErrorWasLogged;

            //Assert
            returnResult.ShouldNotBeNullOrEmpty();
            errorWasLogged.ShouldBeFalse();
        }

        [Test]
        public void GetPrepopulator_ControlTypeNewsLetter_FieldPhone_WithSuccess()
        {
            //Arrange
            InitializeTestParams(_databaseTypeValue, NewsLetterType, GroupFieldIdPhone);
            _httpRequestCookies.Add(new HttpCookie(DummyGuidStr, FieldLabelValue));
            _groupDataFields.Add(KMManagerTestsHelper.CreateGroupDataField(GroupFieldIdPhone, GroupFieldShortNamePhone));

            var methodArguments = new object[] { _httpRequest };

            //Act
            var returnResult = (string)_prepopulator.Invoke(TestedMethodName_Get, methodArguments);
            var errorWasLogged = _applicationErrorWasLogged;

            //Assert
            returnResult.ShouldNotBeNullOrEmpty();
            errorWasLogged.ShouldBeFalse();
        }

        private void InitializeTestParams(string formControlValue, int? mainTypeID = null, int? fieldId=null)
        {
            var controls = KMManagerTestsHelper.CreateControls(controlID: ControlID,
                                                                  fieldLabelValue: FieldLabelValue,
                                                                  mainTypeId: mainTypeID,
                                                                  typeSeqID: TypeSeqIDForMailingPassword,
                                                                  fieldID: fieldId,
                                                                  htmlIDGuid: DummyGuidStr);
            if (mainTypeID == NewsLetterType)
            {
                KMManagerTestsHelper.ChangeControlToAddNewsletterGroup(controls[0],string.Empty,GroupFieldIdPhone);
            }

            var formControlProperties = new List<FormControlProperty>
                {
                    new FormControlProperty
                    {
                        ControlProperty_ID = ControlPropertySeqID,
                        Value = formControlValue
                    }
                };

            controls[0].FormControlProperties = formControlProperties;

            _form.Controls = controls;
        }
    }
}
