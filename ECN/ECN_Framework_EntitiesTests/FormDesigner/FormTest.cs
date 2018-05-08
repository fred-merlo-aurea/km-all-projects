using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.FormDesigner;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.FormDesigner
{
    [TestFixture]
    public class FormTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (Form) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var form = Fixture.Create<Form>();
            var form_Seq_Id = Fixture.Create<int>();
            var tokenUId = Fixture.Create<Guid>();
            var name = Fixture.Create<string>();
            var status = Fixture.Create<String>();
            var activationDateFrom = Fixture.Create<DateTime?>();
            var activationDateTo = Fixture.Create<DateTime?>();
            var lastUpdated = Fixture.Create<DateTime?>();
            var lastPublished = Fixture.Create<DateTime?>();
            var formType = Fixture.Create<String>();
            var optInType = Fixture.Create<int>();
            var cssUri = Fixture.Create<string>();
            var headerHTML = Fixture.Create<string>();
            var footerHTML = Fixture.Create<string>();
            var updatedBy = Fixture.Create<string>();
            var customerName = Fixture.Create<string>();
            var active = Fixture.Create<int>();
            var submitButtonText = Fixture.Create<string>();
            var parentForm_Id = Fixture.Create<int?>();
            var cssFile_Seq_Id = Fixture.Create<int?>();
            var groupId = Fixture.Create<int>();
            var customerId = Fixture.Create<int>();
            var userId = Fixture.Create<int>();
            var stylingType = Fixture.Create<int>();
            var customerAccessKey = Fixture.Create<string>();
            var publishAfter = Fixture.Create<DateTime?>();

            // Act
            form.Form_Seq_ID = form_Seq_Id;
            form.TokenUID = tokenUId;
            form.Name = name;
            form.Status = status;
            form.ActivationDateFrom = activationDateFrom;
            form.ActivationDateTo = activationDateTo;
            form.LastUpdated = lastUpdated;
            form.LastPublished = lastPublished;
            form.FormType = formType;
            form.OptInType = optInType;
            form.CssUri = cssUri;
            form.HeaderHTML = headerHTML;
            form.FooterHTML = footerHTML;
            form.UpdatedBy = updatedBy;
            form.CustomerName = customerName;
            form.Active = active;
            form.SubmitButtonText = submitButtonText;
            form.ParentForm_ID = parentForm_Id;
            form.CssFile_Seq_ID = cssFile_Seq_Id;
            form.GroupID = groupId;
            form.CustomerID = customerId;
            form.UserID = userId;
            form.StylingType = stylingType;
            form.CustomerAccessKey = customerAccessKey;
            form.PublishAfter = publishAfter;

            // Assert
            form.Form_Seq_ID.ShouldBe(form_Seq_Id);
            form.TokenUID.ShouldBe(tokenUId);
            form.Name.ShouldBe(name);
            form.Status.ShouldBe(status);
            form.ActivationDateFrom.ShouldBe(activationDateFrom);
            form.ActivationDateTo.ShouldBe(activationDateTo);
            form.LastUpdated.ShouldBe(lastUpdated);
            form.LastPublished.ShouldBe(lastPublished);
            form.FormType.ShouldBe(formType);
            form.OptInType.ShouldBe(optInType);
            form.CssUri.ShouldBe(cssUri);
            form.HeaderHTML.ShouldBe(headerHTML);
            form.FooterHTML.ShouldBe(footerHTML);
            form.UpdatedBy.ShouldBe(updatedBy);
            form.CustomerName.ShouldBe(customerName);
            form.Active.ShouldBe(active);
            form.SubmitButtonText.ShouldBe(submitButtonText);
            form.ParentForm_ID.ShouldBe(parentForm_Id);
            form.CssFile_Seq_ID.ShouldBe(cssFile_Seq_Id);
            form.GroupID.ShouldBe(groupId);
            form.CustomerID.ShouldBe(customerId);
            form.UserID.ShouldBe(userId);
            form.StylingType.ShouldBe(stylingType);
            form.CustomerAccessKey.ShouldBe(customerAccessKey);
            form.PublishAfter.ShouldBe(publishAfter);
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (ActivationDateFrom) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_ActivationDateFrom_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameActivationDateFrom = "ActivationDateFrom";
            var form = Fixture.Create<Form>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = form.GetType().GetProperty(propertyNameActivationDateFrom);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(form, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (ActivationDateFrom) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Class_Invalid_Property_ActivationDateFromNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActivationDateFrom = "ActivationDateFromNotPresent";
            var form = Fixture.Create<Form>();

            // Act , Assert
            Should.NotThrow(() => form.GetType().GetProperty(propertyNameActivationDateFrom));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_ActivationDateFrom_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActivationDateFrom = "ActivationDateFrom";
            var form = Fixture.Create<Form>();
            var propertyInfo = form.GetType().GetProperty(propertyNameActivationDateFrom);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (ActivationDateTo) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_ActivationDateTo_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameActivationDateTo = "ActivationDateTo";
            var form = Fixture.Create<Form>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = form.GetType().GetProperty(propertyNameActivationDateTo);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(form, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (ActivationDateTo) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Class_Invalid_Property_ActivationDateToNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActivationDateTo = "ActivationDateToNotPresent";
            var form = Fixture.Create<Form>();

            // Act , Assert
            Should.NotThrow(() => form.GetType().GetProperty(propertyNameActivationDateTo));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_ActivationDateTo_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActivationDateTo = "ActivationDateTo";
            var form = Fixture.Create<Form>();
            var propertyInfo = form.GetType().GetProperty(propertyNameActivationDateTo);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (Active) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Active_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var form = Fixture.Create<Form>();
            form.Active = Fixture.Create<int>();
            var intType = form.Active.GetType();

            // Act
            var isTypeInt = typeof(int) == (intType);
            var isTypeNullableInt = typeof(int?) == (intType);
            var isTypeString = typeof(string) == (intType);
            var isTypeDecimal = typeof(decimal) == (intType);
            var isTypeLong = typeof(long) == (intType);
            var isTypeBool = typeof(bool) == (intType);
            var isTypeDouble = typeof(double) == (intType);
            var isTypeFloat = typeof(float) == (intType);
            var isTypeDecimalNullable = typeof(decimal?) == (intType);
            var isTypeLongNullable = typeof(long?) == (intType);
            var isTypeBoolNullable = typeof(bool?) == (intType);
            var isTypeDoubleNullable = typeof(double?) == (intType);
            var isTypeFloatNullable = typeof(float?) == (intType);

            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (Active) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Class_Invalid_Property_ActiveNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActive = "ActiveNotPresent";
            var form = Fixture.Create<Form>();

            // Act , Assert
            Should.NotThrow(() => form.GetType().GetProperty(propertyNameActive));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Active_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActive = "Active";
            var form = Fixture.Create<Form>();
            var propertyInfo = form.GetType().GetProperty(propertyNameActive);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (CssFile_Seq_ID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_CssFile_Seq_ID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var form = Fixture.Create<Form>();
            var random = Fixture.Create<int>();

            // Act , Set
            form.CssFile_Seq_ID = random;

            // Assert
            form.CssFile_Seq_ID.ShouldBe(random);
            form.CssFile_Seq_ID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_CssFile_Seq_ID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var form = Fixture.Create<Form>();

            // Act , Set
            form.CssFile_Seq_ID = null;

            // Assert
            form.CssFile_Seq_ID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_CssFile_Seq_ID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCssFile_Seq_ID = "CssFile_Seq_ID";
            var form = Fixture.Create<Form>();
            var propertyInfo = form.GetType().GetProperty(propertyNameCssFile_Seq_ID);

            // Act , Set
            propertyInfo.SetValue(form, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            form.CssFile_Seq_ID.ShouldBeNull();
            form.CssFile_Seq_ID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (CssFile_Seq_ID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Class_Invalid_Property_CssFile_Seq_IDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCssFile_Seq_ID = "CssFile_Seq_IDNotPresent";
            var form = Fixture.Create<Form>();

            // Act , Assert
            Should.NotThrow(() => form.GetType().GetProperty(propertyNameCssFile_Seq_ID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_CssFile_Seq_ID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCssFile_Seq_ID = "CssFile_Seq_ID";
            var form = Fixture.Create<Form>();
            var propertyInfo = form.GetType().GetProperty(propertyNameCssFile_Seq_ID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (CssUri) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_CssUri_Property_String_Type_Verify_Test()
        {
            // Arrange
            var form = Fixture.Create<Form>();
            form.CssUri = Fixture.Create<string>();
            var stringType = form.CssUri.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (CssUri) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Class_Invalid_Property_CssUriNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCssUri = "CssUriNotPresent";
            var form = Fixture.Create<Form>();

            // Act , Assert
            Should.NotThrow(() => form.GetType().GetProperty(propertyNameCssUri));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_CssUri_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCssUri = "CssUri";
            var form = Fixture.Create<Form>();
            var propertyInfo = form.GetType().GetProperty(propertyNameCssUri);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (CustomerAccessKey) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_CustomerAccessKey_Property_String_Type_Verify_Test()
        {
            // Arrange
            var form = Fixture.Create<Form>();
            form.CustomerAccessKey = Fixture.Create<string>();
            var stringType = form.CustomerAccessKey.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (CustomerAccessKey) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Class_Invalid_Property_CustomerAccessKeyNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerAccessKey = "CustomerAccessKeyNotPresent";
            var form = Fixture.Create<Form>();

            // Act , Assert
            Should.NotThrow(() => form.GetType().GetProperty(propertyNameCustomerAccessKey));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_CustomerAccessKey_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerAccessKey = "CustomerAccessKey";
            var form = Fixture.Create<Form>();
            var propertyInfo = form.GetType().GetProperty(propertyNameCustomerAccessKey);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (CustomerID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_CustomerID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var form = Fixture.Create<Form>();
            form.CustomerID = Fixture.Create<int>();
            var intType = form.CustomerID.GetType();

            // Act
            var isTypeInt = typeof(int) == (intType);
            var isTypeNullableInt = typeof(int?) == (intType);
            var isTypeString = typeof(string) == (intType);
            var isTypeDecimal = typeof(decimal) == (intType);
            var isTypeLong = typeof(long) == (intType);
            var isTypeBool = typeof(bool) == (intType);
            var isTypeDouble = typeof(double) == (intType);
            var isTypeFloat = typeof(float) == (intType);
            var isTypeDecimalNullable = typeof(decimal?) == (intType);
            var isTypeLongNullable = typeof(long?) == (intType);
            var isTypeBoolNullable = typeof(bool?) == (intType);
            var isTypeDoubleNullable = typeof(double?) == (intType);
            var isTypeFloatNullable = typeof(float?) == (intType);

            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var form = Fixture.Create<Form>();

            // Act , Assert
            Should.NotThrow(() => form.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var form = Fixture.Create<Form>();
            var propertyInfo = form.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (CustomerName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_CustomerName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var form = Fixture.Create<Form>();
            form.CustomerName = Fixture.Create<string>();
            var stringType = form.CustomerName.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (CustomerName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Class_Invalid_Property_CustomerNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerName = "CustomerNameNotPresent";
            var form = Fixture.Create<Form>();

            // Act , Assert
            Should.NotThrow(() => form.GetType().GetProperty(propertyNameCustomerName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_CustomerName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerName = "CustomerName";
            var form = Fixture.Create<Form>();
            var propertyInfo = form.GetType().GetProperty(propertyNameCustomerName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (FooterHTML) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_FooterHTML_Property_String_Type_Verify_Test()
        {
            // Arrange
            var form = Fixture.Create<Form>();
            form.FooterHTML = Fixture.Create<string>();
            var stringType = form.FooterHTML.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (FooterHTML) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Class_Invalid_Property_FooterHTMLNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFooterHTML = "FooterHTMLNotPresent";
            var form = Fixture.Create<Form>();

            // Act , Assert
            Should.NotThrow(() => form.GetType().GetProperty(propertyNameFooterHTML));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_FooterHTML_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFooterHTML = "FooterHTML";
            var form = Fixture.Create<Form>();
            var propertyInfo = form.GetType().GetProperty(propertyNameFooterHTML);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (Form_Seq_ID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Form_Seq_ID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var form = Fixture.Create<Form>();
            form.Form_Seq_ID = Fixture.Create<int>();
            var intType = form.Form_Seq_ID.GetType();

            // Act
            var isTypeInt = typeof(int) == (intType);
            var isTypeNullableInt = typeof(int?) == (intType);
            var isTypeString = typeof(string) == (intType);
            var isTypeDecimal = typeof(decimal) == (intType);
            var isTypeLong = typeof(long) == (intType);
            var isTypeBool = typeof(bool) == (intType);
            var isTypeDouble = typeof(double) == (intType);
            var isTypeFloat = typeof(float) == (intType);
            var isTypeDecimalNullable = typeof(decimal?) == (intType);
            var isTypeLongNullable = typeof(long?) == (intType);
            var isTypeBoolNullable = typeof(bool?) == (intType);
            var isTypeDoubleNullable = typeof(double?) == (intType);
            var isTypeFloatNullable = typeof(float?) == (intType);

            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (Form_Seq_ID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Class_Invalid_Property_Form_Seq_IDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameForm_Seq_ID = "Form_Seq_IDNotPresent";
            var form = Fixture.Create<Form>();

            // Act , Assert
            Should.NotThrow(() => form.GetType().GetProperty(propertyNameForm_Seq_ID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Form_Seq_ID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameForm_Seq_ID = "Form_Seq_ID";
            var form = Fixture.Create<Form>();
            var propertyInfo = form.GetType().GetProperty(propertyNameForm_Seq_ID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (FormType) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_FormType_Property_Setting_String_No_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameFormType = "FormType";
            var form = Fixture.Create<Form>();
            var propertyInfo = form.GetType().GetProperty(propertyNameFormType);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.NotThrow(() => propertyInfo.SetValue(form, null));
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (FormType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Class_Invalid_Property_FormTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFormType = "FormTypeNotPresent";
            var form = Fixture.Create<Form>();

            // Act , Assert
            Should.NotThrow(() => form.GetType().GetProperty(propertyNameFormType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_FormType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFormType = "FormType";
            var form = Fixture.Create<Form>();
            var propertyInfo = form.GetType().GetProperty(propertyNameFormType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (GroupID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_GroupID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var form = Fixture.Create<Form>();
            form.GroupID = Fixture.Create<int>();
            var intType = form.GroupID.GetType();

            // Act
            var isTypeInt = typeof(int) == (intType);
            var isTypeNullableInt = typeof(int?) == (intType);
            var isTypeString = typeof(string) == (intType);
            var isTypeDecimal = typeof(decimal) == (intType);
            var isTypeLong = typeof(long) == (intType);
            var isTypeBool = typeof(bool) == (intType);
            var isTypeDouble = typeof(double) == (intType);
            var isTypeFloat = typeof(float) == (intType);
            var isTypeDecimalNullable = typeof(decimal?) == (intType);
            var isTypeLongNullable = typeof(long?) == (intType);
            var isTypeBoolNullable = typeof(bool?) == (intType);
            var isTypeDoubleNullable = typeof(double?) == (intType);
            var isTypeFloatNullable = typeof(float?) == (intType);

            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (GroupID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Class_Invalid_Property_GroupIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupIDNotPresent";
            var form = Fixture.Create<Form>();

            // Act , Assert
            Should.NotThrow(() => form.GetType().GetProperty(propertyNameGroupID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_GroupID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupID";
            var form = Fixture.Create<Form>();
            var propertyInfo = form.GetType().GetProperty(propertyNameGroupID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (HeaderHTML) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_HeaderHTML_Property_String_Type_Verify_Test()
        {
            // Arrange
            var form = Fixture.Create<Form>();
            form.HeaderHTML = Fixture.Create<string>();
            var stringType = form.HeaderHTML.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (HeaderHTML) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Class_Invalid_Property_HeaderHTMLNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameHeaderHTML = "HeaderHTMLNotPresent";
            var form = Fixture.Create<Form>();

            // Act , Assert
            Should.NotThrow(() => form.GetType().GetProperty(propertyNameHeaderHTML));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_HeaderHTML_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameHeaderHTML = "HeaderHTML";
            var form = Fixture.Create<Form>();
            var propertyInfo = form.GetType().GetProperty(propertyNameHeaderHTML);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (LastPublished) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_LastPublished_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameLastPublished = "LastPublished";
            var form = Fixture.Create<Form>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = form.GetType().GetProperty(propertyNameLastPublished);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(form, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (LastPublished) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Class_Invalid_Property_LastPublishedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLastPublished = "LastPublishedNotPresent";
            var form = Fixture.Create<Form>();

            // Act , Assert
            Should.NotThrow(() => form.GetType().GetProperty(propertyNameLastPublished));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_LastPublished_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLastPublished = "LastPublished";
            var form = Fixture.Create<Form>();
            var propertyInfo = form.GetType().GetProperty(propertyNameLastPublished);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (LastUpdated) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_LastUpdated_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameLastUpdated = "LastUpdated";
            var form = Fixture.Create<Form>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = form.GetType().GetProperty(propertyNameLastUpdated);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(form, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (LastUpdated) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Class_Invalid_Property_LastUpdatedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLastUpdated = "LastUpdatedNotPresent";
            var form = Fixture.Create<Form>();

            // Act , Assert
            Should.NotThrow(() => form.GetType().GetProperty(propertyNameLastUpdated));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_LastUpdated_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLastUpdated = "LastUpdated";
            var form = Fixture.Create<Form>();
            var propertyInfo = form.GetType().GetProperty(propertyNameLastUpdated);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (Name) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Name_Property_String_Type_Verify_Test()
        {
            // Arrange
            var form = Fixture.Create<Form>();
            form.Name = Fixture.Create<string>();
            var stringType = form.Name.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (Name) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Class_Invalid_Property_NameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameName = "NameNotPresent";
            var form = Fixture.Create<Form>();

            // Act , Assert
            Should.NotThrow(() => form.GetType().GetProperty(propertyNameName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Name_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameName = "Name";
            var form = Fixture.Create<Form>();
            var propertyInfo = form.GetType().GetProperty(propertyNameName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (OptInType) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_OptInType_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var form = Fixture.Create<Form>();
            form.OptInType = Fixture.Create<int>();
            var intType = form.OptInType.GetType();

            // Act
            var isTypeInt = typeof(int) == (intType);
            var isTypeNullableInt = typeof(int?) == (intType);
            var isTypeString = typeof(string) == (intType);
            var isTypeDecimal = typeof(decimal) == (intType);
            var isTypeLong = typeof(long) == (intType);
            var isTypeBool = typeof(bool) == (intType);
            var isTypeDouble = typeof(double) == (intType);
            var isTypeFloat = typeof(float) == (intType);
            var isTypeDecimalNullable = typeof(decimal?) == (intType);
            var isTypeLongNullable = typeof(long?) == (intType);
            var isTypeBoolNullable = typeof(bool?) == (intType);
            var isTypeDoubleNullable = typeof(double?) == (intType);
            var isTypeFloatNullable = typeof(float?) == (intType);

            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (OptInType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Class_Invalid_Property_OptInTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOptInType = "OptInTypeNotPresent";
            var form = Fixture.Create<Form>();

            // Act , Assert
            Should.NotThrow(() => form.GetType().GetProperty(propertyNameOptInType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_OptInType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOptInType = "OptInType";
            var form = Fixture.Create<Form>();
            var propertyInfo = form.GetType().GetProperty(propertyNameOptInType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (ParentForm_ID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_ParentForm_ID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var form = Fixture.Create<Form>();
            var random = Fixture.Create<int>();

            // Act , Set
            form.ParentForm_ID = random;

            // Assert
            form.ParentForm_ID.ShouldBe(random);
            form.ParentForm_ID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_ParentForm_ID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var form = Fixture.Create<Form>();

            // Act , Set
            form.ParentForm_ID = null;

            // Assert
            form.ParentForm_ID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_ParentForm_ID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameParentForm_ID = "ParentForm_ID";
            var form = Fixture.Create<Form>();
            var propertyInfo = form.GetType().GetProperty(propertyNameParentForm_ID);

            // Act , Set
            propertyInfo.SetValue(form, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            form.ParentForm_ID.ShouldBeNull();
            form.ParentForm_ID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (ParentForm_ID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Class_Invalid_Property_ParentForm_IDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParentForm_ID = "ParentForm_IDNotPresent";
            var form = Fixture.Create<Form>();

            // Act , Assert
            Should.NotThrow(() => form.GetType().GetProperty(propertyNameParentForm_ID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_ParentForm_ID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParentForm_ID = "ParentForm_ID";
            var form = Fixture.Create<Form>();
            var propertyInfo = form.GetType().GetProperty(propertyNameParentForm_ID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (PublishAfter) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_PublishAfter_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNamePublishAfter = "PublishAfter";
            var form = Fixture.Create<Form>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = form.GetType().GetProperty(propertyNamePublishAfter);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(form, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (PublishAfter) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Class_Invalid_Property_PublishAfterNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePublishAfter = "PublishAfterNotPresent";
            var form = Fixture.Create<Form>();

            // Act , Assert
            Should.NotThrow(() => form.GetType().GetProperty(propertyNamePublishAfter));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_PublishAfter_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePublishAfter = "PublishAfter";
            var form = Fixture.Create<Form>();
            var propertyInfo = form.GetType().GetProperty(propertyNamePublishAfter);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (Status) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Status_Property_Setting_String_Not_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameStatus = "Status";
            var form = Fixture.Create<Form>();
            var propertyInfo = form.GetType().GetProperty(propertyNameStatus);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.NotThrow(() => propertyInfo.SetValue(form,  null));
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (Status) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Class_Invalid_Property_StatusNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameStatus = "StatusNotPresent";
            var form = Fixture.Create<Form>();

            // Act , Assert
            Should.NotThrow(() => form.GetType().GetProperty(propertyNameStatus));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Status_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameStatus = "Status";
            var form = Fixture.Create<Form>();
            var propertyInfo = form.GetType().GetProperty(propertyNameStatus);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (StylingType) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_StylingType_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var form = Fixture.Create<Form>();
            form.StylingType = Fixture.Create<int>();
            var intType = form.StylingType.GetType();

            // Act
            var isTypeInt = typeof(int) == (intType);
            var isTypeNullableInt = typeof(int?) == (intType);
            var isTypeString = typeof(string) == (intType);
            var isTypeDecimal = typeof(decimal) == (intType);
            var isTypeLong = typeof(long) == (intType);
            var isTypeBool = typeof(bool) == (intType);
            var isTypeDouble = typeof(double) == (intType);
            var isTypeFloat = typeof(float) == (intType);
            var isTypeDecimalNullable = typeof(decimal?) == (intType);
            var isTypeLongNullable = typeof(long?) == (intType);
            var isTypeBoolNullable = typeof(bool?) == (intType);
            var isTypeDoubleNullable = typeof(double?) == (intType);
            var isTypeFloatNullable = typeof(float?) == (intType);

            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (StylingType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Class_Invalid_Property_StylingTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameStylingType = "StylingTypeNotPresent";
            var form = Fixture.Create<Form>();

            // Act , Assert
            Should.NotThrow(() => form.GetType().GetProperty(propertyNameStylingType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_StylingType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameStylingType = "StylingType";
            var form = Fixture.Create<Form>();
            var propertyInfo = form.GetType().GetProperty(propertyNameStylingType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (SubmitButtonText) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_SubmitButtonText_Property_String_Type_Verify_Test()
        {
            // Arrange
            var form = Fixture.Create<Form>();
            form.SubmitButtonText = Fixture.Create<string>();
            var stringType = form.SubmitButtonText.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (SubmitButtonText) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Class_Invalid_Property_SubmitButtonTextNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSubmitButtonText = "SubmitButtonTextNotPresent";
            var form = Fixture.Create<Form>();

            // Act , Assert
            Should.NotThrow(() => form.GetType().GetProperty(propertyNameSubmitButtonText));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_SubmitButtonText_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSubmitButtonText = "SubmitButtonText";
            var form = Fixture.Create<Form>();
            var propertyInfo = form.GetType().GetProperty(propertyNameSubmitButtonText);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (TokenUID) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_TokenUID_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameTokenUID = "TokenUID";
            var form = Fixture.Create<Form>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = form.GetType().GetProperty(propertyNameTokenUID);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(form, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (TokenUID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Class_Invalid_Property_TokenUIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTokenUID = "TokenUIDNotPresent";
            var form = Fixture.Create<Form>();

            // Act , Assert
            Should.NotThrow(() => form.GetType().GetProperty(propertyNameTokenUID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_TokenUID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTokenUID = "TokenUID";
            var form = Fixture.Create<Form>();
            var propertyInfo = form.GetType().GetProperty(propertyNameTokenUID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (UpdatedBy) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_UpdatedBy_Property_String_Type_Verify_Test()
        {
            // Arrange
            var form = Fixture.Create<Form>();
            form.UpdatedBy = Fixture.Create<string>();
            var stringType = form.UpdatedBy.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (UpdatedBy) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Class_Invalid_Property_UpdatedByNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedBy = "UpdatedByNotPresent";
            var form = Fixture.Create<Form>();

            // Act , Assert
            Should.NotThrow(() => form.GetType().GetProperty(propertyNameUpdatedBy));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_UpdatedBy_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedBy = "UpdatedBy";
            var form = Fixture.Create<Form>();
            var propertyInfo = form.GetType().GetProperty(propertyNameUpdatedBy);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (UserID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_UserID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var form = Fixture.Create<Form>();
            form.UserID = Fixture.Create<int>();
            var intType = form.UserID.GetType();

            // Act
            var isTypeInt = typeof(int) == (intType);
            var isTypeNullableInt = typeof(int?) == (intType);
            var isTypeString = typeof(string) == (intType);
            var isTypeDecimal = typeof(decimal) == (intType);
            var isTypeLong = typeof(long) == (intType);
            var isTypeBool = typeof(bool) == (intType);
            var isTypeDouble = typeof(double) == (intType);
            var isTypeFloat = typeof(float) == (intType);
            var isTypeDecimalNullable = typeof(decimal?) == (intType);
            var isTypeLongNullable = typeof(long?) == (intType);
            var isTypeBoolNullable = typeof(bool?) == (intType);
            var isTypeDoubleNullable = typeof(double?) == (intType);
            var isTypeFloatNullable = typeof(float?) == (intType);

            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (Form) => Property (UserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_Class_Invalid_Property_UserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUserID = "UserIDNotPresent";
            var form = Fixture.Create<Form>();

            // Act , Assert
            Should.NotThrow(() => form.GetType().GetProperty(propertyNameUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Form_UserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUserID = "UserID";
            var form = Fixture.Create<Form>();
            var propertyInfo = form.GetType().GetProperty(propertyNameUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (Form) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Form_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new Form());
        }

        #endregion

        #region General Constructor : Class (Form) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Form_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfForm = Fixture.CreateMany<Form>(2).ToList();
            var firstForm = instancesOfForm.FirstOrDefault();
            var lastForm = instancesOfForm.Last();

            // Act, Assert
            firstForm.ShouldNotBeNull();
            lastForm.ShouldNotBeNull();
            firstForm.ShouldNotBeSameAs(lastForm);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Form_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstForm = new Form();
            var secondForm = new Form();
            var thirdForm = new Form();
            var fourthForm = new Form();
            var fifthForm = new Form();
            var sixthForm = new Form();

            // Act, Assert
            firstForm.ShouldNotBeNull();
            secondForm.ShouldNotBeNull();
            thirdForm.ShouldNotBeNull();
            fourthForm.ShouldNotBeNull();
            fifthForm.ShouldNotBeNull();
            sixthForm.ShouldNotBeNull();
            firstForm.ShouldNotBeSameAs(secondForm);
            thirdForm.ShouldNotBeSameAs(firstForm);
            fourthForm.ShouldNotBeSameAs(firstForm);
            fifthForm.ShouldNotBeSameAs(firstForm);
            sixthForm.ShouldNotBeSameAs(firstForm);
            sixthForm.ShouldNotBeSameAs(fourthForm);
        }

        #endregion

        #region General Constructor : Class (Form) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Form_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var form_Seq_Id = -1;
            var tokenUId = new Guid();
            var name = string.Empty;
            var status = string.Empty;
            var formType = string.Empty;
            var optInType = -1;
            var cssUri = string.Empty;
            var headerHTML = string.Empty;
            var footerHTML = string.Empty;
            var updatedBy = string.Empty;
            var customerName = string.Empty;
            var active = -1;
            var groupId = -1;
            var customerId = -1;
            var userId = -1;
            var stylingType = -1;
            var customerAccessKey = string.Empty;

            // Act
            var form = new Form();

            // Assert
            form.Form_Seq_ID.ShouldBe(form_Seq_Id);
            form.TokenUID.ShouldBe(tokenUId);
            form.Name.ShouldBe(name);
            form.Status.ShouldBe(status);
            form.ActivationDateFrom.ShouldBeNull();
            form.ActivationDateTo.ShouldBeNull();
            form.LastUpdated.ShouldBeNull();
            form.LastPublished.ShouldBeNull();
            form.FormType.ShouldBe(formType);
            form.OptInType.ShouldBe(optInType);
            form.CssUri.ShouldBe(cssUri);
            form.HeaderHTML.ShouldBe(headerHTML);
            form.FooterHTML.ShouldBe(footerHTML);
            form.UpdatedBy.ShouldBe(updatedBy);
            form.CustomerName.ShouldBe(customerName);
            form.Active.ShouldBe(active);
            form.SubmitButtonText.ShouldBeNull();
            form.ParentForm_ID.ShouldBeNull();
            form.CssFile_Seq_ID.ShouldBeNull();
            form.GroupID.ShouldBe(groupId);
            form.CustomerID.ShouldBe(customerId);
            form.UserID.ShouldBe(userId);
            form.StylingType.ShouldBe(stylingType);
            form.CustomerAccessKey.ShouldBe(customerAccessKey);
            form.PublishAfter.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}