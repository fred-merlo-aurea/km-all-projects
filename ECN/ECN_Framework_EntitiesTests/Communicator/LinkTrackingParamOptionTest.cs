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
    public class LinkTrackingParamOptionTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (LinkTrackingParamOption) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var linkTrackingParamOption = Fixture.Create<LinkTrackingParamOption>();
            var lTPOId = Fixture.Create<int>();
            var lTPId = Fixture.Create<int>();
            var displayName = Fixture.Create<string>();
            var columnName = Fixture.Create<string>();
            var value = Fixture.Create<string>();
            var isActive = Fixture.Create<bool?>();
            var customerId = Fixture.Create<int?>();
            var baseChannelId = Fixture.Create<int?>();
            var isDynamic = Fixture.Create<bool>();
            var isDefault = Fixture.Create<bool>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var isDeleted = Fixture.Create<bool>();

            // Act
            linkTrackingParamOption.LTPOID = lTPOId;
            linkTrackingParamOption.LTPID = lTPId;
            linkTrackingParamOption.DisplayName = displayName;
            linkTrackingParamOption.ColumnName = columnName;
            linkTrackingParamOption.Value = value;
            linkTrackingParamOption.IsActive = isActive;
            linkTrackingParamOption.CustomerID = customerId;
            linkTrackingParamOption.BaseChannelID = baseChannelId;
            linkTrackingParamOption.IsDynamic = isDynamic;
            linkTrackingParamOption.IsDefault = isDefault;
            linkTrackingParamOption.CreatedUserID = createdUserId;
            linkTrackingParamOption.CreatedDate = createdDate;
            linkTrackingParamOption.UpdatedUserID = updatedUserId;
            linkTrackingParamOption.UpdatedDate = updatedDate;
            linkTrackingParamOption.IsDeleted = isDeleted;

            // Assert
            linkTrackingParamOption.LTPOID.ShouldBe(lTPOId);
            linkTrackingParamOption.LTPID.ShouldBe(lTPId);
            linkTrackingParamOption.DisplayName.ShouldBe(displayName);
            linkTrackingParamOption.ColumnName.ShouldBe(columnName);
            linkTrackingParamOption.Value.ShouldBe(value);
            linkTrackingParamOption.IsActive.ShouldBe(isActive);
            linkTrackingParamOption.CustomerID.ShouldBe(customerId);
            linkTrackingParamOption.BaseChannelID.ShouldBe(baseChannelId);
            linkTrackingParamOption.IsDynamic.ShouldBe(isDynamic);
            linkTrackingParamOption.IsDefault.ShouldBe(isDefault);
            linkTrackingParamOption.CreatedUserID.ShouldBe(createdUserId);
            linkTrackingParamOption.CreatedDate.ShouldBe(createdDate);
            linkTrackingParamOption.UpdatedUserID.ShouldBe(updatedUserId);
            linkTrackingParamOption.UpdatedDate.ShouldBe(updatedDate);
            linkTrackingParamOption.IsDeleted.ShouldBe(isDeleted);
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (BaseChannelID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_BaseChannelID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkTrackingParamOption = Fixture.Create<LinkTrackingParamOption>();
            var random = Fixture.Create<int>();

            // Act , Set
            linkTrackingParamOption.BaseChannelID = random;

            // Assert
            linkTrackingParamOption.BaseChannelID.ShouldBe(random);
            linkTrackingParamOption.BaseChannelID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_BaseChannelID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkTrackingParamOption = Fixture.Create<LinkTrackingParamOption>();

            // Act , Set
            linkTrackingParamOption.BaseChannelID = null;

            // Assert
            linkTrackingParamOption.BaseChannelID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_BaseChannelID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBaseChannelID = "BaseChannelID";
            var linkTrackingParamOption = Fixture.Create<LinkTrackingParamOption>();
            var propertyInfo = linkTrackingParamOption.GetType().GetProperty(propertyNameBaseChannelID);

            // Act , Set
            propertyInfo.SetValue(linkTrackingParamOption, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkTrackingParamOption.BaseChannelID.ShouldBeNull();
            linkTrackingParamOption.BaseChannelID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (BaseChannelID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_Class_Invalid_Property_BaseChannelIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelIDNotPresent";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParamOption.GetType().GetProperty(propertyNameBaseChannelID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_BaseChannelID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelID";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();
            var propertyInfo  = linkTrackingParamOption.GetType().GetProperty(propertyNameBaseChannelID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (ColumnName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_ColumnName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkTrackingParamOption = Fixture.Create<LinkTrackingParamOption>();
            linkTrackingParamOption.ColumnName = Fixture.Create<string>();
            var stringType = linkTrackingParamOption.ColumnName.GetType();

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

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (ColumnName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_Class_Invalid_Property_ColumnNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameColumnName = "ColumnNameNotPresent";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParamOption.GetType().GetProperty(propertyNameColumnName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_ColumnName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameColumnName = "ColumnName";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();
            var propertyInfo  = linkTrackingParamOption.GetType().GetProperty(propertyNameColumnName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var linkTrackingParamOption = Fixture.Create<LinkTrackingParamOption>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = linkTrackingParamOption.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(linkTrackingParamOption, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParamOption.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();
            var propertyInfo  = linkTrackingParamOption.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkTrackingParamOption = Fixture.Create<LinkTrackingParamOption>();
            var random = Fixture.Create<int>();

            // Act , Set
            linkTrackingParamOption.CreatedUserID = random;

            // Assert
            linkTrackingParamOption.CreatedUserID.ShouldBe(random);
            linkTrackingParamOption.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkTrackingParamOption = Fixture.Create<LinkTrackingParamOption>();

            // Act , Set
            linkTrackingParamOption.CreatedUserID = null;

            // Assert
            linkTrackingParamOption.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var linkTrackingParamOption = Fixture.Create<LinkTrackingParamOption>();
            var propertyInfo = linkTrackingParamOption.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(linkTrackingParamOption, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkTrackingParamOption.CreatedUserID.ShouldBeNull();
            linkTrackingParamOption.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParamOption.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();
            var propertyInfo  = linkTrackingParamOption.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkTrackingParamOption = Fixture.Create<LinkTrackingParamOption>();
            var random = Fixture.Create<int>();

            // Act , Set
            linkTrackingParamOption.CustomerID = random;

            // Assert
            linkTrackingParamOption.CustomerID.ShouldBe(random);
            linkTrackingParamOption.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkTrackingParamOption = Fixture.Create<LinkTrackingParamOption>();

            // Act , Set
            linkTrackingParamOption.CustomerID = null;

            // Assert
            linkTrackingParamOption.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var linkTrackingParamOption = Fixture.Create<LinkTrackingParamOption>();
            var propertyInfo = linkTrackingParamOption.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(linkTrackingParamOption, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkTrackingParamOption.CustomerID.ShouldBeNull();
            linkTrackingParamOption.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParamOption.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();
            var propertyInfo  = linkTrackingParamOption.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (DisplayName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_DisplayName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkTrackingParamOption = Fixture.Create<LinkTrackingParamOption>();
            linkTrackingParamOption.DisplayName = Fixture.Create<string>();
            var stringType = linkTrackingParamOption.DisplayName.GetType();

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

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (DisplayName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_Class_Invalid_Property_DisplayNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDisplayName = "DisplayNameNotPresent";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParamOption.GetType().GetProperty(propertyNameDisplayName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_DisplayName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDisplayName = "DisplayName";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();
            var propertyInfo  = linkTrackingParamOption.GetType().GetProperty(propertyNameDisplayName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (IsActive) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_IsActive_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkTrackingParamOption = Fixture.Create<LinkTrackingParamOption>();
            var random = Fixture.Create<bool>();

            // Act , Set
            linkTrackingParamOption.IsActive = random;

            // Assert
            linkTrackingParamOption.IsActive.ShouldBe(random);
            linkTrackingParamOption.IsActive.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_IsActive_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkTrackingParamOption = Fixture.Create<LinkTrackingParamOption>();

            // Act , Set
            linkTrackingParamOption.IsActive = null;

            // Assert
            linkTrackingParamOption.IsActive.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_IsActive_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsActive = "IsActive";
            var linkTrackingParamOption = Fixture.Create<LinkTrackingParamOption>();
            var propertyInfo = linkTrackingParamOption.GetType().GetProperty(propertyNameIsActive);

            // Act , Set
            propertyInfo.SetValue(linkTrackingParamOption, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkTrackingParamOption.IsActive.ShouldBeNull();
            linkTrackingParamOption.IsActive.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (IsActive) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_Class_Invalid_Property_IsActiveNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsActive = "IsActiveNotPresent";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParamOption.GetType().GetProperty(propertyNameIsActive));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_IsActive_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsActive = "IsActive";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();
            var propertyInfo  = linkTrackingParamOption.GetType().GetProperty(propertyNameIsActive);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (IsDefault) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_IsDefault_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var linkTrackingParamOption = Fixture.Create<LinkTrackingParamOption>();
            linkTrackingParamOption.IsDefault = Fixture.Create<bool>();
            var boolType = linkTrackingParamOption.IsDefault.GetType();

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

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (IsDefault) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_Class_Invalid_Property_IsDefaultNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDefault = "IsDefaultNotPresent";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParamOption.GetType().GetProperty(propertyNameIsDefault));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_IsDefault_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDefault = "IsDefault";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();
            var propertyInfo  = linkTrackingParamOption.GetType().GetProperty(propertyNameIsDefault);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (IsDeleted) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_IsDeleted_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var linkTrackingParamOption = Fixture.Create<LinkTrackingParamOption>();
            linkTrackingParamOption.IsDeleted = Fixture.Create<bool>();
            var boolType = linkTrackingParamOption.IsDeleted.GetType();

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

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParamOption.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();
            var propertyInfo  = linkTrackingParamOption.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (IsDynamic) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_IsDynamic_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var linkTrackingParamOption = Fixture.Create<LinkTrackingParamOption>();
            linkTrackingParamOption.IsDynamic = Fixture.Create<bool>();
            var boolType = linkTrackingParamOption.IsDynamic.GetType();

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

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (IsDynamic) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_Class_Invalid_Property_IsDynamicNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDynamic = "IsDynamicNotPresent";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParamOption.GetType().GetProperty(propertyNameIsDynamic));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_IsDynamic_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDynamic = "IsDynamic";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();
            var propertyInfo  = linkTrackingParamOption.GetType().GetProperty(propertyNameIsDynamic);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (LTPID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_LTPID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var linkTrackingParamOption = Fixture.Create<LinkTrackingParamOption>();
            linkTrackingParamOption.LTPID = Fixture.Create<int>();
            var intType = linkTrackingParamOption.LTPID.GetType();

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

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (LTPID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_Class_Invalid_Property_LTPIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLTPID = "LTPIDNotPresent";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParamOption.GetType().GetProperty(propertyNameLTPID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_LTPID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLTPID = "LTPID";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();
            var propertyInfo  = linkTrackingParamOption.GetType().GetProperty(propertyNameLTPID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (LTPOID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_LTPOID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var linkTrackingParamOption = Fixture.Create<LinkTrackingParamOption>();
            linkTrackingParamOption.LTPOID = Fixture.Create<int>();
            var intType = linkTrackingParamOption.LTPOID.GetType();

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

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (LTPOID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_Class_Invalid_Property_LTPOIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLTPOID = "LTPOIDNotPresent";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParamOption.GetType().GetProperty(propertyNameLTPOID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_LTPOID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLTPOID = "LTPOID";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();
            var propertyInfo  = linkTrackingParamOption.GetType().GetProperty(propertyNameLTPOID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var linkTrackingParamOption = Fixture.Create<LinkTrackingParamOption>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = linkTrackingParamOption.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(linkTrackingParamOption, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParamOption.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();
            var propertyInfo  = linkTrackingParamOption.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkTrackingParamOption = Fixture.Create<LinkTrackingParamOption>();
            var random = Fixture.Create<int>();

            // Act , Set
            linkTrackingParamOption.UpdatedUserID = random;

            // Assert
            linkTrackingParamOption.UpdatedUserID.ShouldBe(random);
            linkTrackingParamOption.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkTrackingParamOption = Fixture.Create<LinkTrackingParamOption>();

            // Act , Set
            linkTrackingParamOption.UpdatedUserID = null;

            // Assert
            linkTrackingParamOption.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var linkTrackingParamOption = Fixture.Create<LinkTrackingParamOption>();
            var propertyInfo = linkTrackingParamOption.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(linkTrackingParamOption, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkTrackingParamOption.UpdatedUserID.ShouldBeNull();
            linkTrackingParamOption.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParamOption.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();
            var propertyInfo  = linkTrackingParamOption.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (Value) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_Value_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkTrackingParamOption = Fixture.Create<LinkTrackingParamOption>();
            linkTrackingParamOption.Value = Fixture.Create<string>();
            var stringType = linkTrackingParamOption.Value.GetType();

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

        #region General Getters/Setters : Class (LinkTrackingParamOption) => Property (Value) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_Class_Invalid_Property_ValueNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameValue = "ValueNotPresent";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParamOption.GetType().GetProperty(propertyNameValue));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamOption_Value_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameValue = "Value";
            var linkTrackingParamOption  = Fixture.Create<LinkTrackingParamOption>();
            var propertyInfo  = linkTrackingParamOption.GetType().GetProperty(propertyNameValue);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (LinkTrackingParamOption) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkTrackingParamOption_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new LinkTrackingParamOption());
        }

        #endregion

        #region General Constructor : Class (LinkTrackingParamOption) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkTrackingParamOption_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfLinkTrackingParamOption = Fixture.CreateMany<LinkTrackingParamOption>(2).ToList();
            var firstLinkTrackingParamOption = instancesOfLinkTrackingParamOption.FirstOrDefault();
            var lastLinkTrackingParamOption = instancesOfLinkTrackingParamOption.Last();

            // Act, Assert
            firstLinkTrackingParamOption.ShouldNotBeNull();
            lastLinkTrackingParamOption.ShouldNotBeNull();
            firstLinkTrackingParamOption.ShouldNotBeSameAs(lastLinkTrackingParamOption);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkTrackingParamOption_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstLinkTrackingParamOption = new LinkTrackingParamOption();
            var secondLinkTrackingParamOption = new LinkTrackingParamOption();
            var thirdLinkTrackingParamOption = new LinkTrackingParamOption();
            var fourthLinkTrackingParamOption = new LinkTrackingParamOption();
            var fifthLinkTrackingParamOption = new LinkTrackingParamOption();
            var sixthLinkTrackingParamOption = new LinkTrackingParamOption();

            // Act, Assert
            firstLinkTrackingParamOption.ShouldNotBeNull();
            secondLinkTrackingParamOption.ShouldNotBeNull();
            thirdLinkTrackingParamOption.ShouldNotBeNull();
            fourthLinkTrackingParamOption.ShouldNotBeNull();
            fifthLinkTrackingParamOption.ShouldNotBeNull();
            sixthLinkTrackingParamOption.ShouldNotBeNull();
            firstLinkTrackingParamOption.ShouldNotBeSameAs(secondLinkTrackingParamOption);
            thirdLinkTrackingParamOption.ShouldNotBeSameAs(firstLinkTrackingParamOption);
            fourthLinkTrackingParamOption.ShouldNotBeSameAs(firstLinkTrackingParamOption);
            fifthLinkTrackingParamOption.ShouldNotBeSameAs(firstLinkTrackingParamOption);
            sixthLinkTrackingParamOption.ShouldNotBeSameAs(firstLinkTrackingParamOption);
            sixthLinkTrackingParamOption.ShouldNotBeSameAs(fourthLinkTrackingParamOption);
        }

        #endregion

        #region General Constructor : Class (LinkTrackingParamOption) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkTrackingParamOption_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var lTPOId = -1;
            var displayName = string.Empty;
            var value = string.Empty;
            var isDynamic = false;
            var isDefault = false;
            var isDeleted = false;

            // Act
            var linkTrackingParamOption = new LinkTrackingParamOption();

            // Assert
            linkTrackingParamOption.LTPOID.ShouldBe(lTPOId);
            linkTrackingParamOption.DisplayName.ShouldBe(displayName);
            linkTrackingParamOption.ColumnName.ShouldBeNull();
            linkTrackingParamOption.Value.ShouldBe(value);
            linkTrackingParamOption.IsActive.ShouldBeNull();
            linkTrackingParamOption.CustomerID.ShouldBeNull();
            linkTrackingParamOption.BaseChannelID.ShouldBeNull();
            linkTrackingParamOption.IsDynamic.ShouldBeFalse();
            linkTrackingParamOption.IsDefault.ShouldBeFalse();
            linkTrackingParamOption.CreatedUserID.ShouldBeNull();
            linkTrackingParamOption.CreatedDate.ShouldBeNull();
            linkTrackingParamOption.UpdatedUserID.ShouldBeNull();
            linkTrackingParamOption.UpdatedDate.ShouldBeNull();
            linkTrackingParamOption.IsDeleted.ShouldBeFalse();
        }

        #endregion

        #endregion

        #endregion
    }
}