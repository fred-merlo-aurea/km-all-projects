using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator
{
    [TestFixture]
    public class DomainSuppressionTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (DomainSuppression) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var domainSuppression = Fixture.Create<DomainSuppression>();
            var domainSuppressionId = Fixture.Create<int>();
            var baseChannelId = Fixture.Create<int?>();
            var customerId = Fixture.Create<int?>();
            var domain = Fixture.Create<string>();
            var isActive = Fixture.Create<bool>();
            var createdUserId = Fixture.Create<int?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();

            // Act
            domainSuppression.DomainSuppressionID = domainSuppressionId;
            domainSuppression.BaseChannelID = baseChannelId;
            domainSuppression.CustomerID = customerId;
            domainSuppression.Domain = domain;
            domainSuppression.IsActive = isActive;
            domainSuppression.CreatedUserID = createdUserId;
            domainSuppression.UpdatedUserID = updatedUserId;
            domainSuppression.IsDeleted = isDeleted;

            // Assert
            domainSuppression.DomainSuppressionID.ShouldBe(domainSuppressionId);
            domainSuppression.BaseChannelID.ShouldBe(baseChannelId);
            domainSuppression.CustomerID.ShouldBe(customerId);
            domainSuppression.Domain.ShouldBe(domain);
            domainSuppression.IsActive.ShouldBe(isActive);
            domainSuppression.CreatedUserID.ShouldBe(createdUserId);
            domainSuppression.CreatedDate.ShouldBeNull();
            domainSuppression.UpdatedUserID.ShouldBe(updatedUserId);
            domainSuppression.UpdatedDate.ShouldBeNull();
            domainSuppression.IsDeleted.ShouldBe(isDeleted);
        }

        #endregion

        #region General Getters/Setters : Class (DomainSuppression) => Property (BaseChannelID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_BaseChannelID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var domainSuppression = Fixture.Create<DomainSuppression>();
            var random = Fixture.Create<int>();

            // Act , Set
            domainSuppression.BaseChannelID = random;

            // Assert
            domainSuppression.BaseChannelID.ShouldBe(random);
            domainSuppression.BaseChannelID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_BaseChannelID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var domainSuppression = Fixture.Create<DomainSuppression>();

            // Act , Set
            domainSuppression.BaseChannelID = null;

            // Assert
            domainSuppression.BaseChannelID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_BaseChannelID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBaseChannelID = "BaseChannelID";
            var domainSuppression = Fixture.Create<DomainSuppression>();
            var propertyInfo = domainSuppression.GetType().GetProperty(propertyNameBaseChannelID);

            // Act , Set
            propertyInfo.SetValue(domainSuppression, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            domainSuppression.BaseChannelID.ShouldBeNull();
            domainSuppression.BaseChannelID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DomainSuppression) => Property (BaseChannelID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_Class_Invalid_Property_BaseChannelIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelIDNotPresent";
            var domainSuppression  = Fixture.Create<DomainSuppression>();

            // Act , Assert
            Should.NotThrow(() => domainSuppression.GetType().GetProperty(propertyNameBaseChannelID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_BaseChannelID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelID";
            var domainSuppression  = Fixture.Create<DomainSuppression>();
            var propertyInfo  = domainSuppression.GetType().GetProperty(propertyNameBaseChannelID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainSuppression) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var domainSuppression = Fixture.Create<DomainSuppression>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = domainSuppression.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(domainSuppression, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (DomainSuppression) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var domainSuppression  = Fixture.Create<DomainSuppression>();

            // Act , Assert
            Should.NotThrow(() => domainSuppression.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var domainSuppression  = Fixture.Create<DomainSuppression>();
            var propertyInfo  = domainSuppression.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainSuppression) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var domainSuppression = Fixture.Create<DomainSuppression>();
            var random = Fixture.Create<int>();

            // Act , Set
            domainSuppression.CreatedUserID = random;

            // Assert
            domainSuppression.CreatedUserID.ShouldBe(random);
            domainSuppression.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var domainSuppression = Fixture.Create<DomainSuppression>();

            // Act , Set
            domainSuppression.CreatedUserID = null;

            // Assert
            domainSuppression.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var domainSuppression = Fixture.Create<DomainSuppression>();
            var propertyInfo = domainSuppression.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(domainSuppression, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            domainSuppression.CreatedUserID.ShouldBeNull();
            domainSuppression.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DomainSuppression) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var domainSuppression  = Fixture.Create<DomainSuppression>();

            // Act , Assert
            Should.NotThrow(() => domainSuppression.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var domainSuppression  = Fixture.Create<DomainSuppression>();
            var propertyInfo  = domainSuppression.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainSuppression) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var domainSuppression = Fixture.Create<DomainSuppression>();
            var random = Fixture.Create<int>();

            // Act , Set
            domainSuppression.CustomerID = random;

            // Assert
            domainSuppression.CustomerID.ShouldBe(random);
            domainSuppression.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var domainSuppression = Fixture.Create<DomainSuppression>();

            // Act , Set
            domainSuppression.CustomerID = null;

            // Assert
            domainSuppression.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var domainSuppression = Fixture.Create<DomainSuppression>();
            var propertyInfo = domainSuppression.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(domainSuppression, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            domainSuppression.CustomerID.ShouldBeNull();
            domainSuppression.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DomainSuppression) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var domainSuppression  = Fixture.Create<DomainSuppression>();

            // Act , Assert
            Should.NotThrow(() => domainSuppression.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var domainSuppression  = Fixture.Create<DomainSuppression>();
            var propertyInfo  = domainSuppression.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainSuppression) => Property (Domain) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_Domain_Property_String_Type_Verify_Test()
        {
            // Arrange
            var domainSuppression = Fixture.Create<DomainSuppression>();
            domainSuppression.Domain = Fixture.Create<string>();
            var stringType = domainSuppression.Domain.GetType();

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

        #region General Getters/Setters : Class (DomainSuppression) => Property (Domain) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_Class_Invalid_Property_DomainNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDomain = "DomainNotPresent";
            var domainSuppression  = Fixture.Create<DomainSuppression>();

            // Act , Assert
            Should.NotThrow(() => domainSuppression.GetType().GetProperty(propertyNameDomain));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_Domain_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDomain = "Domain";
            var domainSuppression  = Fixture.Create<DomainSuppression>();
            var propertyInfo  = domainSuppression.GetType().GetProperty(propertyNameDomain);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainSuppression) => Property (DomainSuppressionID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_DomainSuppressionID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var domainSuppression = Fixture.Create<DomainSuppression>();
            domainSuppression.DomainSuppressionID = Fixture.Create<int>();
            var intType = domainSuppression.DomainSuppressionID.GetType();

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

        #region General Getters/Setters : Class (DomainSuppression) => Property (DomainSuppressionID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_Class_Invalid_Property_DomainSuppressionIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDomainSuppressionID = "DomainSuppressionIDNotPresent";
            var domainSuppression  = Fixture.Create<DomainSuppression>();

            // Act , Assert
            Should.NotThrow(() => domainSuppression.GetType().GetProperty(propertyNameDomainSuppressionID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_DomainSuppressionID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDomainSuppressionID = "DomainSuppressionID";
            var domainSuppression  = Fixture.Create<DomainSuppression>();
            var propertyInfo  = domainSuppression.GetType().GetProperty(propertyNameDomainSuppressionID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainSuppression) => Property (IsActive) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_IsActive_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var domainSuppression = Fixture.Create<DomainSuppression>();
            domainSuppression.IsActive = Fixture.Create<bool>();
            var boolType = domainSuppression.IsActive.GetType();

            // Act
            var isTypeBool = typeof(bool) == (boolType);
            var isTypeNullableBool = typeof(bool?) == (boolType);
            var isTypeString = typeof(string) == (boolType);
            var isTypeInt = typeof(int) == (boolType);
            var isTypeDecimal = typeof(decimal) == (boolType);
            var isTypeLong = typeof(long) == (boolType);
            var isTypeDouble = typeof(double) == (boolType);
            var isTypeFloat = typeof(float) == (boolType);
            var isTypeIntNullable = typeof(int?) == (boolType);
            var isTypeDecimalNullable = typeof(decimal?) == (boolType);
            var isTypeLongNullable = typeof(long?) == (boolType);
            var isTypeDoubleNullable = typeof(double?) == (boolType);
            var isTypeFloatNullable = typeof(float?) == (boolType);

            // Assert
            isTypeBool.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableBool.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (DomainSuppression) => Property (IsActive) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_Class_Invalid_Property_IsActiveNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsActive = "IsActiveNotPresent";
            var domainSuppression  = Fixture.Create<DomainSuppression>();

            // Act , Assert
            Should.NotThrow(() => domainSuppression.GetType().GetProperty(propertyNameIsActive));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_IsActive_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsActive = "IsActive";
            var domainSuppression  = Fixture.Create<DomainSuppression>();
            var propertyInfo  = domainSuppression.GetType().GetProperty(propertyNameIsActive);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainSuppression) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var domainSuppression = Fixture.Create<DomainSuppression>();
            var random = Fixture.Create<bool>();

            // Act , Set
            domainSuppression.IsDeleted = random;

            // Assert
            domainSuppression.IsDeleted.ShouldBe(random);
            domainSuppression.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var domainSuppression = Fixture.Create<DomainSuppression>();

            // Act , Set
            domainSuppression.IsDeleted = null;

            // Assert
            domainSuppression.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var domainSuppression = Fixture.Create<DomainSuppression>();
            var propertyInfo = domainSuppression.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(domainSuppression, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            domainSuppression.IsDeleted.ShouldBeNull();
            domainSuppression.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DomainSuppression) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var domainSuppression  = Fixture.Create<DomainSuppression>();

            // Act , Assert
            Should.NotThrow(() => domainSuppression.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var domainSuppression  = Fixture.Create<DomainSuppression>();
            var propertyInfo  = domainSuppression.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainSuppression) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var domainSuppression = Fixture.Create<DomainSuppression>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = domainSuppression.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(domainSuppression, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (DomainSuppression) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var domainSuppression  = Fixture.Create<DomainSuppression>();

            // Act , Assert
            Should.NotThrow(() => domainSuppression.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var domainSuppression  = Fixture.Create<DomainSuppression>();
            var propertyInfo  = domainSuppression.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainSuppression) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var domainSuppression = Fixture.Create<DomainSuppression>();
            var random = Fixture.Create<int>();

            // Act , Set
            domainSuppression.UpdatedUserID = random;

            // Assert
            domainSuppression.UpdatedUserID.ShouldBe(random);
            domainSuppression.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var domainSuppression = Fixture.Create<DomainSuppression>();

            // Act , Set
            domainSuppression.UpdatedUserID = null;

            // Assert
            domainSuppression.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var domainSuppression = Fixture.Create<DomainSuppression>();
            var propertyInfo = domainSuppression.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(domainSuppression, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            domainSuppression.UpdatedUserID.ShouldBeNull();
            domainSuppression.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DomainSuppression) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var domainSuppression  = Fixture.Create<DomainSuppression>();

            // Act , Assert
            Should.NotThrow(() => domainSuppression.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainSuppression_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var domainSuppression  = Fixture.Create<DomainSuppression>();
            var propertyInfo  = domainSuppression.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (DomainSuppression) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DomainSuppression_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new DomainSuppression());
        }

        #endregion

        #region General Constructor : Class (DomainSuppression) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DomainSuppression_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfDomainSuppression = Fixture.CreateMany<DomainSuppression>(2).ToList();
            var firstDomainSuppression = instancesOfDomainSuppression.FirstOrDefault();
            var lastDomainSuppression = instancesOfDomainSuppression.Last();

            // Act, Assert
            firstDomainSuppression.ShouldNotBeNull();
            lastDomainSuppression.ShouldNotBeNull();
            firstDomainSuppression.ShouldNotBeSameAs(lastDomainSuppression);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DomainSuppression_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstDomainSuppression = new DomainSuppression();
            var secondDomainSuppression = new DomainSuppression();
            var thirdDomainSuppression = new DomainSuppression();
            var fourthDomainSuppression = new DomainSuppression();
            var fifthDomainSuppression = new DomainSuppression();
            var sixthDomainSuppression = new DomainSuppression();

            // Act, Assert
            firstDomainSuppression.ShouldNotBeNull();
            secondDomainSuppression.ShouldNotBeNull();
            thirdDomainSuppression.ShouldNotBeNull();
            fourthDomainSuppression.ShouldNotBeNull();
            fifthDomainSuppression.ShouldNotBeNull();
            sixthDomainSuppression.ShouldNotBeNull();
            firstDomainSuppression.ShouldNotBeSameAs(secondDomainSuppression);
            thirdDomainSuppression.ShouldNotBeSameAs(firstDomainSuppression);
            fourthDomainSuppression.ShouldNotBeSameAs(firstDomainSuppression);
            fifthDomainSuppression.ShouldNotBeSameAs(firstDomainSuppression);
            sixthDomainSuppression.ShouldNotBeSameAs(firstDomainSuppression);
            sixthDomainSuppression.ShouldNotBeSameAs(fourthDomainSuppression);
        }

        #endregion

        #region General Constructor : Class (DomainSuppression) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DomainSuppression_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var domainSuppressionId = -1;
            var domain = string.Empty;
            var isActive = false;

            // Act
            var domainSuppression = new DomainSuppression();

            // Assert
            domainSuppression.DomainSuppressionID.ShouldBe(domainSuppressionId);
            domainSuppression.BaseChannelID.ShouldBeNull();
            domainSuppression.CustomerID.ShouldBeNull();
            domainSuppression.Domain.ShouldBe(domain);
            domainSuppression.IsActive.ShouldBeFalse();
            domainSuppression.CreatedUserID.ShouldBeNull();
            domainSuppression.CreatedDate.ShouldBeNull();
            domainSuppression.UpdatedUserID.ShouldBeNull();
            domainSuppression.UpdatedDate.ShouldBeNull();
            domainSuppression.IsDeleted.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}