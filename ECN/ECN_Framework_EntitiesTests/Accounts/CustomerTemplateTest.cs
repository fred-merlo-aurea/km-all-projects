using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Shouldly;
using NUnit.Framework;
using AutoFixture;
using ECN_Framework_EntitiesTests.ConfigureProject;
using ECN_Framework_Entities.Accounts;

namespace ECN_Framework_EntitiesTests.Accounts
{
    [TestFixture]
    public class CustomerTemplateTest : AbstractGenericTest
    {
        #region General Category : General

        #region Category : GetterSetter

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerTemplate_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var customerTemplate  = new CustomerTemplate();
            var cTID = Fixture.Create<int>();
            var customerID = Fixture.Create<int?>();
            var templateTypeCode = Fixture.Create<string>();
            var headerSource = Fixture.Create<string>();
            var footerSource = Fixture.Create<string>();
            var cCNotifyEmail = Fixture.Create<string>();
            var isActive = Fixture.Create<bool?>();
            var createdUserID = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserID = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var isDeleted = Fixture.Create<bool?>();

            // Act
            customerTemplate.CTID = cTID;
            customerTemplate.CustomerID = customerID;
            customerTemplate.TemplateTypeCode = templateTypeCode;
            customerTemplate.HeaderSource = headerSource;
            customerTemplate.FooterSource = footerSource;
            customerTemplate.CCNotifyEmail = cCNotifyEmail;
            customerTemplate.IsActive = isActive;
            customerTemplate.CreatedUserID = createdUserID;
            customerTemplate.CreatedDate = createdDate;
            customerTemplate.UpdatedUserID = updatedUserID;
            customerTemplate.UpdatedDate = updatedDate;
            customerTemplate.IsDeleted = isDeleted;

            // Assert
            customerTemplate.CTID.ShouldBe(cTID);
            customerTemplate.CustomerID.ShouldBe(customerID);
            customerTemplate.TemplateTypeCode.ShouldBe(templateTypeCode);
            customerTemplate.HeaderSource.ShouldBe(headerSource);
            customerTemplate.FooterSource.ShouldBe(footerSource);
            customerTemplate.CCNotifyEmail.ShouldBe(cCNotifyEmail);
            customerTemplate.IsActive.ShouldBe(isActive);
            customerTemplate.CreatedUserID.ShouldBe(createdUserID);
            customerTemplate.CreatedDate.ShouldBe(createdDate);
            customerTemplate.UpdatedUserID.ShouldBe(updatedUserID);
            customerTemplate.UpdatedDate.ShouldBe(updatedDate);
            customerTemplate.IsDeleted.ShouldBe(isDeleted);   
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region Nullable Property Test : CustomerTemplate => IsActive

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsActive_Data_Without_Null_Test()
        {
            // Arrange
            var customerTemplate = Fixture.Create<CustomerTemplate>();
            var random = Fixture.Create<bool>();

            // Act , Set
            customerTemplate.IsActive = random;

            // Assert
            customerTemplate.IsActive.ShouldBe(random);
            customerTemplate.IsActive.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsActive_Only_Null_Data_Test()
        {
            // Arrange
            var customerTemplate = Fixture.Create<CustomerTemplate>();    

            // Act , Set
            customerTemplate.IsActive = null;

            // Assert
            customerTemplate.IsActive.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsActive_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constIsActive = "IsActive";
            var customerTemplate = Fixture.Create<CustomerTemplate>();
            var propertyInfo = customerTemplate.GetType().GetProperty(constIsActive);

            // Act , Set
            propertyInfo.SetValue(customerTemplate, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerTemplate.IsActive.ShouldBeNull();
            customerTemplate.IsActive.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerTemplate_Class_Invalid_Property_IsActive_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constIsActive = "IsActive";
            var customerTemplate  = Fixture.Create<CustomerTemplate>();

            // Act , Assert
            Should.NotThrow(() => customerTemplate.GetType().GetProperty(constIsActive));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsActive_Is_Present_In_CustomerTemplate_Class_As_Public_Test()
        {
            // Arrange
            const string constIsActive = "IsActive";
            var customerTemplate  = Fixture.Create<CustomerTemplate>();
            var propertyInfo  = customerTemplate.GetType().GetProperty(constIsActive);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : CustomerTemplate => IsDeleted

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Data_Without_Null_Test()
        {
            // Arrange
            var customerTemplate = Fixture.Create<CustomerTemplate>();
            var random = Fixture.Create<bool>();

            // Act , Set
            customerTemplate.IsDeleted = random;

            // Assert
            customerTemplate.IsDeleted.ShouldBe(random);
            customerTemplate.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Only_Null_Data_Test()
        {
            // Arrange
            var customerTemplate = Fixture.Create<CustomerTemplate>();    

            // Act , Set
            customerTemplate.IsDeleted = null;

            // Assert
            customerTemplate.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constIsDeleted = "IsDeleted";
            var customerTemplate = Fixture.Create<CustomerTemplate>();
            var propertyInfo = customerTemplate.GetType().GetProperty(constIsDeleted);

            // Act , Set
            propertyInfo.SetValue(customerTemplate, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerTemplate.IsDeleted.ShouldBeNull();
            customerTemplate.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerTemplate_Class_Invalid_Property_IsDeleted_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var customerTemplate  = Fixture.Create<CustomerTemplate>();

            // Act , Assert
            Should.NotThrow(() => customerTemplate.GetType().GetProperty(constIsDeleted));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Is_Present_In_CustomerTemplate_Class_As_Public_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var customerTemplate  = Fixture.Create<CustomerTemplate>();
            var propertyInfo  = customerTemplate.GetType().GetProperty(constIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : CustomerTemplate => CreatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var customerTemplate = Fixture.Create<CustomerTemplate>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = customerTemplate.GetType().GetProperty(constCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(customerTemplate, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerTemplate_Class_Invalid_Property_CreatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var customerTemplate  = Fixture.Create<CustomerTemplate>();

            // Act , Assert
            Should.NotThrow(() => customerTemplate.GetType().GetProperty(constCreatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Is_Present_In_CustomerTemplate_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var customerTemplate  = Fixture.Create<CustomerTemplate>();
            var propertyInfo  = customerTemplate.GetType().GetProperty(constCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : CustomerTemplate => UpdatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var customerTemplate = Fixture.Create<CustomerTemplate>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = customerTemplate.GetType().GetProperty(constUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(customerTemplate, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerTemplate_Class_Invalid_Property_UpdatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var customerTemplate  = Fixture.Create<CustomerTemplate>();

            // Act , Assert
            Should.NotThrow(() => customerTemplate.GetType().GetProperty(constUpdatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Is_Present_In_CustomerTemplate_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var customerTemplate  = Fixture.Create<CustomerTemplate>();
            var propertyInfo  = customerTemplate.GetType().GetProperty(constUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : CustomerTemplate => CTID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CTID_Int_Type_Verify_Test()
        {
            // Arrange
            var customerTemplate = Fixture.Create<CustomerTemplate>();
            var intType = customerTemplate.CTID.GetType();

            // Act
            var isTypeInt = typeof(int).Equals(intType);    
            var isTypeNullableInt = typeof(int?).Equals(intType);
            var isTypeString = typeof(string).Equals(intType);
            var isTypeDecimal = typeof(decimal).Equals(intType);
            var isTypeLong = typeof(long).Equals(intType);
            var isTypeBool = typeof(bool).Equals(intType);
            var isTypeDouble = typeof(double).Equals(intType);
            var isTypeFloat = typeof(float).Equals(intType);
            var isTypeDecimalNullable = typeof(decimal?).Equals(intType);
            var isTypeLongNullable = typeof(long?).Equals(intType);
            var isTypeBoolNullable = typeof(bool?).Equals(intType);
            var isTypeDoubleNullable = typeof(double?).Equals(intType);
            var isTypeFloatNullable = typeof(float?).Equals(intType);


            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerTemplate_Class_Invalid_Property_CTID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCTID = "CTID";
            var customerTemplate  = Fixture.Create<CustomerTemplate>();

            // Act , Assert
            Should.NotThrow(() => customerTemplate.GetType().GetProperty(constCTID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CTID_Is_Present_In_CustomerTemplate_Class_As_Public_Test()
        {
            // Arrange
            const string constCTID = "CTID";
            var customerTemplate  = Fixture.Create<CustomerTemplate>();
            var propertyInfo  = customerTemplate.GetType().GetProperty(constCTID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : CustomerTemplate => CreatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var customerTemplate = Fixture.Create<CustomerTemplate>();
            var random = Fixture.Create<int>();

            // Act , Set
            customerTemplate.CreatedUserID = random;

            // Assert
            customerTemplate.CreatedUserID.ShouldBe(random);
            customerTemplate.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var customerTemplate = Fixture.Create<CustomerTemplate>();    

            // Act , Set
            customerTemplate.CreatedUserID = null;

            // Assert
            customerTemplate.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constCreatedUserID = "CreatedUserID";
            var customerTemplate = Fixture.Create<CustomerTemplate>();
            var propertyInfo = customerTemplate.GetType().GetProperty(constCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(customerTemplate, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerTemplate.CreatedUserID.ShouldBeNull();
            customerTemplate.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerTemplate_Class_Invalid_Property_CreatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var customerTemplate  = Fixture.Create<CustomerTemplate>();

            // Act , Assert
            Should.NotThrow(() => customerTemplate.GetType().GetProperty(constCreatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Is_Present_In_CustomerTemplate_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var customerTemplate  = Fixture.Create<CustomerTemplate>();
            var propertyInfo  = customerTemplate.GetType().GetProperty(constCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : CustomerTemplate => CustomerID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Data_Without_Null_Test()
        {
            // Arrange
            var customerTemplate = Fixture.Create<CustomerTemplate>();
            var random = Fixture.Create<int>();

            // Act , Set
            customerTemplate.CustomerID = random;

            // Assert
            customerTemplate.CustomerID.ShouldBe(random);
            customerTemplate.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Only_Null_Data_Test()
        {
            // Arrange
            var customerTemplate = Fixture.Create<CustomerTemplate>();    

            // Act , Set
            customerTemplate.CustomerID = null;

            // Assert
            customerTemplate.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constCustomerID = "CustomerID";
            var customerTemplate = Fixture.Create<CustomerTemplate>();
            var propertyInfo = customerTemplate.GetType().GetProperty(constCustomerID);

            // Act , Set
            propertyInfo.SetValue(customerTemplate, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerTemplate.CustomerID.ShouldBeNull();
            customerTemplate.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerTemplate_Class_Invalid_Property_CustomerID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCustomerID = "CustomerID";
            var customerTemplate  = Fixture.Create<CustomerTemplate>();

            // Act , Assert
            Should.NotThrow(() => customerTemplate.GetType().GetProperty(constCustomerID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Is_Present_In_CustomerTemplate_Class_As_Public_Test()
        {
            // Arrange
            const string constCustomerID = "CustomerID";
            var customerTemplate  = Fixture.Create<CustomerTemplate>();
            var propertyInfo  = customerTemplate.GetType().GetProperty(constCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : CustomerTemplate => UpdatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var customerTemplate = Fixture.Create<CustomerTemplate>();
            var random = Fixture.Create<int>();

            // Act , Set
            customerTemplate.UpdatedUserID = random;

            // Assert
            customerTemplate.UpdatedUserID.ShouldBe(random);
            customerTemplate.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var customerTemplate = Fixture.Create<CustomerTemplate>();    

            // Act , Set
            customerTemplate.UpdatedUserID = null;

            // Assert
            customerTemplate.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constUpdatedUserID = "UpdatedUserID";
            var customerTemplate = Fixture.Create<CustomerTemplate>();
            var propertyInfo = customerTemplate.GetType().GetProperty(constUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(customerTemplate, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerTemplate.UpdatedUserID.ShouldBeNull();
            customerTemplate.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerTemplate_Class_Invalid_Property_UpdatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var customerTemplate  = Fixture.Create<CustomerTemplate>();

            // Act , Assert
            Should.NotThrow(() => customerTemplate.GetType().GetProperty(constUpdatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Is_Present_In_CustomerTemplate_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var customerTemplate  = Fixture.Create<CustomerTemplate>();
            var propertyInfo  = customerTemplate.GetType().GetProperty(constUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : CustomerTemplate => CCNotifyEmail

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CCNotifyEmail_String_Type_Verify_Test()
        {
            // Arrange
            var customerTemplate = Fixture.Create<CustomerTemplate>();
            var stringType = customerTemplate.CCNotifyEmail.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerTemplate_Class_Invalid_Property_CCNotifyEmail_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCCNotifyEmail = "CCNotifyEmail";
            var customerTemplate  = Fixture.Create<CustomerTemplate>();

            // Act , Assert
            Should.NotThrow(() => customerTemplate.GetType().GetProperty(constCCNotifyEmail));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CCNotifyEmail_Is_Present_In_CustomerTemplate_Class_As_Public_Test()
        {
            // Arrange
            const string constCCNotifyEmail = "CCNotifyEmail";
            var customerTemplate  = Fixture.Create<CustomerTemplate>();
            var propertyInfo  = customerTemplate.GetType().GetProperty(constCCNotifyEmail);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : CustomerTemplate => FooterSource

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_FooterSource_String_Type_Verify_Test()
        {
            // Arrange
            var customerTemplate = Fixture.Create<CustomerTemplate>();
            var stringType = customerTemplate.FooterSource.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerTemplate_Class_Invalid_Property_FooterSource_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constFooterSource = "FooterSource";
            var customerTemplate  = Fixture.Create<CustomerTemplate>();

            // Act , Assert
            Should.NotThrow(() => customerTemplate.GetType().GetProperty(constFooterSource));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_FooterSource_Is_Present_In_CustomerTemplate_Class_As_Public_Test()
        {
            // Arrange
            const string constFooterSource = "FooterSource";
            var customerTemplate  = Fixture.Create<CustomerTemplate>();
            var propertyInfo  = customerTemplate.GetType().GetProperty(constFooterSource);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : CustomerTemplate => HeaderSource

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_HeaderSource_String_Type_Verify_Test()
        {
            // Arrange
            var customerTemplate = Fixture.Create<CustomerTemplate>();
            var stringType = customerTemplate.HeaderSource.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerTemplate_Class_Invalid_Property_HeaderSource_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constHeaderSource = "HeaderSource";
            var customerTemplate  = Fixture.Create<CustomerTemplate>();

            // Act , Assert
            Should.NotThrow(() => customerTemplate.GetType().GetProperty(constHeaderSource));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_HeaderSource_Is_Present_In_CustomerTemplate_Class_As_Public_Test()
        {
            // Arrange
            const string constHeaderSource = "HeaderSource";
            var customerTemplate  = Fixture.Create<CustomerTemplate>();
            var propertyInfo  = customerTemplate.GetType().GetProperty(constHeaderSource);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : CustomerTemplate => TemplateTypeCode

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_TemplateTypeCode_String_Type_Verify_Test()
        {
            // Arrange
            var customerTemplate = Fixture.Create<CustomerTemplate>();
            var stringType = customerTemplate.TemplateTypeCode.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerTemplate_Class_Invalid_Property_TemplateTypeCode_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constTemplateTypeCode = "TemplateTypeCode";
            var customerTemplate  = Fixture.Create<CustomerTemplate>();

            // Act , Assert
            Should.NotThrow(() => customerTemplate.GetType().GetProperty(constTemplateTypeCode));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_TemplateTypeCode_Is_Present_In_CustomerTemplate_Class_As_Public_Test()
        {
            // Arrange
            const string constTemplateTypeCode = "TemplateTypeCode";
            var customerTemplate  = Fixture.Create<CustomerTemplate>();
            var propertyInfo  = customerTemplate.GetType().GetProperty(constTemplateTypeCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #endregion

        #endregion

        #region General Category : General

        #region Category : Contructor

        #region General Constructor Pattern : create and expect no exception.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new CustomerTemplate());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<CustomerTemplate>(2).ToList();
            var first = myInstances.FirstOrDefault();
            var last = myInstances.Last();

            // Act, Assert
            first.ShouldNotBeNull();
            last.ShouldNotBeNull();
            first.ShouldNotBeSameAs(last);
        }

        #endregion

        #region General Constructor Pattern : Default Assignment Test

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Instantiated_With_Default_Assignments_NoChange_DefaultValues()
        {
            // Arrange
            var cTID = -1;
            int? customerID = null;
            var templateTypeCode = string.Empty;
            var headerSource = string.Empty;
            var footerSource = string.Empty;
            var cCNotifyEmail = string.Empty;
            bool? isActive = null;
            int? createdUserID = null;
            DateTime? createdDate = null;
            int? updatedUserID = null;
            DateTime? updatedDate = null;
            bool? isDeleted = null;    

            // Act
            var customerTemplate = new CustomerTemplate();    

            // Assert
            customerTemplate.CTID.ShouldBe(cTID);
            customerTemplate.CustomerID.ShouldBeNull();
            customerTemplate.TemplateTypeCode.ShouldBe(templateTypeCode);
            customerTemplate.HeaderSource.ShouldBe(headerSource);
            customerTemplate.FooterSource.ShouldBe(footerSource);
            customerTemplate.CCNotifyEmail.ShouldBe(cCNotifyEmail);
            customerTemplate.IsActive.ShouldBeNull();
            customerTemplate.CreatedUserID.ShouldBeNull();
            customerTemplate.CreatedDate.ShouldBeNull();
            customerTemplate.UpdatedUserID.ShouldBeNull();
            customerTemplate.UpdatedDate.ShouldBeNull();
            customerTemplate.IsDeleted.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}