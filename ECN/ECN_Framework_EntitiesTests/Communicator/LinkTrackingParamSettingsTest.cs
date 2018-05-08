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
    public class LinkTrackingParamSettingsTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (LinkTrackingParamSettings) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var linkTrackingParamSettings = Fixture.Create<LinkTrackingParamSettings>();
            var lTPSId = Fixture.Create<int>();
            var lTPId = Fixture.Create<int>();
            var customerId = Fixture.Create<int?>();
            var baseChannelId = Fixture.Create<int?>();
            var displayName = Fixture.Create<string>();
            var allowCustom = Fixture.Create<bool>();
            var isRequired = Fixture.Create<bool>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var isDeleted = Fixture.Create<bool>();

            // Act
            linkTrackingParamSettings.LTPSID = lTPSId;
            linkTrackingParamSettings.LTPID = lTPId;
            linkTrackingParamSettings.CustomerID = customerId;
            linkTrackingParamSettings.BaseChannelID = baseChannelId;
            linkTrackingParamSettings.DisplayName = displayName;
            linkTrackingParamSettings.AllowCustom = allowCustom;
            linkTrackingParamSettings.IsRequired = isRequired;
            linkTrackingParamSettings.CreatedUserID = createdUserId;
            linkTrackingParamSettings.CreatedDate = createdDate;
            linkTrackingParamSettings.UpdatedUserID = updatedUserId;
            linkTrackingParamSettings.UpdatedDate = updatedDate;
            linkTrackingParamSettings.IsDeleted = isDeleted;

            // Assert
            linkTrackingParamSettings.LTPSID.ShouldBe(lTPSId);
            linkTrackingParamSettings.LTPID.ShouldBe(lTPId);
            linkTrackingParamSettings.CustomerID.ShouldBe(customerId);
            linkTrackingParamSettings.BaseChannelID.ShouldBe(baseChannelId);
            linkTrackingParamSettings.DisplayName.ShouldBe(displayName);
            linkTrackingParamSettings.AllowCustom.ShouldBe(allowCustom);
            linkTrackingParamSettings.IsRequired.ShouldBe(isRequired);
            linkTrackingParamSettings.CreatedUserID.ShouldBe(createdUserId);
            linkTrackingParamSettings.CreatedDate.ShouldBe(createdDate);
            linkTrackingParamSettings.UpdatedUserID.ShouldBe(updatedUserId);
            linkTrackingParamSettings.UpdatedDate.ShouldBe(updatedDate);
            linkTrackingParamSettings.IsDeleted.ShouldBe(isDeleted);
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamSettings) => Property (AllowCustom) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_AllowCustom_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var linkTrackingParamSettings = Fixture.Create<LinkTrackingParamSettings>();
            linkTrackingParamSettings.AllowCustom = Fixture.Create<bool>();
            var boolType = linkTrackingParamSettings.AllowCustom.GetType();

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

        #region General Getters/Setters : Class (LinkTrackingParamSettings) => Property (AllowCustom) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_Class_Invalid_Property_AllowCustomNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAllowCustom = "AllowCustomNotPresent";
            var linkTrackingParamSettings  = Fixture.Create<LinkTrackingParamSettings>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParamSettings.GetType().GetProperty(propertyNameAllowCustom));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_AllowCustom_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAllowCustom = "AllowCustom";
            var linkTrackingParamSettings  = Fixture.Create<LinkTrackingParamSettings>();
            var propertyInfo  = linkTrackingParamSettings.GetType().GetProperty(propertyNameAllowCustom);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamSettings) => Property (BaseChannelID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_BaseChannelID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkTrackingParamSettings = Fixture.Create<LinkTrackingParamSettings>();
            var random = Fixture.Create<int>();

            // Act , Set
            linkTrackingParamSettings.BaseChannelID = random;

            // Assert
            linkTrackingParamSettings.BaseChannelID.ShouldBe(random);
            linkTrackingParamSettings.BaseChannelID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_BaseChannelID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkTrackingParamSettings = Fixture.Create<LinkTrackingParamSettings>();

            // Act , Set
            linkTrackingParamSettings.BaseChannelID = null;

            // Assert
            linkTrackingParamSettings.BaseChannelID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_BaseChannelID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBaseChannelID = "BaseChannelID";
            var linkTrackingParamSettings = Fixture.Create<LinkTrackingParamSettings>();
            var propertyInfo = linkTrackingParamSettings.GetType().GetProperty(propertyNameBaseChannelID);

            // Act , Set
            propertyInfo.SetValue(linkTrackingParamSettings, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkTrackingParamSettings.BaseChannelID.ShouldBeNull();
            linkTrackingParamSettings.BaseChannelID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamSettings) => Property (BaseChannelID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_Class_Invalid_Property_BaseChannelIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelIDNotPresent";
            var linkTrackingParamSettings  = Fixture.Create<LinkTrackingParamSettings>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParamSettings.GetType().GetProperty(propertyNameBaseChannelID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_BaseChannelID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelID";
            var linkTrackingParamSettings  = Fixture.Create<LinkTrackingParamSettings>();
            var propertyInfo  = linkTrackingParamSettings.GetType().GetProperty(propertyNameBaseChannelID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamSettings) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var linkTrackingParamSettings = Fixture.Create<LinkTrackingParamSettings>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = linkTrackingParamSettings.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(linkTrackingParamSettings, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamSettings) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var linkTrackingParamSettings  = Fixture.Create<LinkTrackingParamSettings>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParamSettings.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var linkTrackingParamSettings  = Fixture.Create<LinkTrackingParamSettings>();
            var propertyInfo  = linkTrackingParamSettings.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamSettings) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkTrackingParamSettings = Fixture.Create<LinkTrackingParamSettings>();
            var random = Fixture.Create<int>();

            // Act , Set
            linkTrackingParamSettings.CreatedUserID = random;

            // Assert
            linkTrackingParamSettings.CreatedUserID.ShouldBe(random);
            linkTrackingParamSettings.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkTrackingParamSettings = Fixture.Create<LinkTrackingParamSettings>();

            // Act , Set
            linkTrackingParamSettings.CreatedUserID = null;

            // Assert
            linkTrackingParamSettings.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var linkTrackingParamSettings = Fixture.Create<LinkTrackingParamSettings>();
            var propertyInfo = linkTrackingParamSettings.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(linkTrackingParamSettings, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkTrackingParamSettings.CreatedUserID.ShouldBeNull();
            linkTrackingParamSettings.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamSettings) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var linkTrackingParamSettings  = Fixture.Create<LinkTrackingParamSettings>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParamSettings.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var linkTrackingParamSettings  = Fixture.Create<LinkTrackingParamSettings>();
            var propertyInfo  = linkTrackingParamSettings.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamSettings) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkTrackingParamSettings = Fixture.Create<LinkTrackingParamSettings>();
            var random = Fixture.Create<int>();

            // Act , Set
            linkTrackingParamSettings.CustomerID = random;

            // Assert
            linkTrackingParamSettings.CustomerID.ShouldBe(random);
            linkTrackingParamSettings.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkTrackingParamSettings = Fixture.Create<LinkTrackingParamSettings>();

            // Act , Set
            linkTrackingParamSettings.CustomerID = null;

            // Assert
            linkTrackingParamSettings.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var linkTrackingParamSettings = Fixture.Create<LinkTrackingParamSettings>();
            var propertyInfo = linkTrackingParamSettings.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(linkTrackingParamSettings, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkTrackingParamSettings.CustomerID.ShouldBeNull();
            linkTrackingParamSettings.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamSettings) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var linkTrackingParamSettings  = Fixture.Create<LinkTrackingParamSettings>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParamSettings.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var linkTrackingParamSettings  = Fixture.Create<LinkTrackingParamSettings>();
            var propertyInfo  = linkTrackingParamSettings.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamSettings) => Property (DisplayName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_DisplayName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkTrackingParamSettings = Fixture.Create<LinkTrackingParamSettings>();
            linkTrackingParamSettings.DisplayName = Fixture.Create<string>();
            var stringType = linkTrackingParamSettings.DisplayName.GetType();

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

        #region General Getters/Setters : Class (LinkTrackingParamSettings) => Property (DisplayName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_Class_Invalid_Property_DisplayNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDisplayName = "DisplayNameNotPresent";
            var linkTrackingParamSettings  = Fixture.Create<LinkTrackingParamSettings>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParamSettings.GetType().GetProperty(propertyNameDisplayName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_DisplayName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDisplayName = "DisplayName";
            var linkTrackingParamSettings  = Fixture.Create<LinkTrackingParamSettings>();
            var propertyInfo  = linkTrackingParamSettings.GetType().GetProperty(propertyNameDisplayName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamSettings) => Property (IsDeleted) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_IsDeleted_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var linkTrackingParamSettings = Fixture.Create<LinkTrackingParamSettings>();
            linkTrackingParamSettings.IsDeleted = Fixture.Create<bool>();
            var boolType = linkTrackingParamSettings.IsDeleted.GetType();

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

        #region General Getters/Setters : Class (LinkTrackingParamSettings) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var linkTrackingParamSettings  = Fixture.Create<LinkTrackingParamSettings>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParamSettings.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var linkTrackingParamSettings  = Fixture.Create<LinkTrackingParamSettings>();
            var propertyInfo  = linkTrackingParamSettings.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamSettings) => Property (IsRequired) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_IsRequired_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var linkTrackingParamSettings = Fixture.Create<LinkTrackingParamSettings>();
            linkTrackingParamSettings.IsRequired = Fixture.Create<bool>();
            var boolType = linkTrackingParamSettings.IsRequired.GetType();

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

        #region General Getters/Setters : Class (LinkTrackingParamSettings) => Property (IsRequired) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_Class_Invalid_Property_IsRequiredNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsRequired = "IsRequiredNotPresent";
            var linkTrackingParamSettings  = Fixture.Create<LinkTrackingParamSettings>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParamSettings.GetType().GetProperty(propertyNameIsRequired));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_IsRequired_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsRequired = "IsRequired";
            var linkTrackingParamSettings  = Fixture.Create<LinkTrackingParamSettings>();
            var propertyInfo  = linkTrackingParamSettings.GetType().GetProperty(propertyNameIsRequired);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamSettings) => Property (LTPID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_LTPID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var linkTrackingParamSettings = Fixture.Create<LinkTrackingParamSettings>();
            linkTrackingParamSettings.LTPID = Fixture.Create<int>();
            var intType = linkTrackingParamSettings.LTPID.GetType();

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

        #region General Getters/Setters : Class (LinkTrackingParamSettings) => Property (LTPID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_Class_Invalid_Property_LTPIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLTPID = "LTPIDNotPresent";
            var linkTrackingParamSettings  = Fixture.Create<LinkTrackingParamSettings>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParamSettings.GetType().GetProperty(propertyNameLTPID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_LTPID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLTPID = "LTPID";
            var linkTrackingParamSettings  = Fixture.Create<LinkTrackingParamSettings>();
            var propertyInfo  = linkTrackingParamSettings.GetType().GetProperty(propertyNameLTPID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamSettings) => Property (LTPSID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_LTPSID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var linkTrackingParamSettings = Fixture.Create<LinkTrackingParamSettings>();
            linkTrackingParamSettings.LTPSID = Fixture.Create<int>();
            var intType = linkTrackingParamSettings.LTPSID.GetType();

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

        #region General Getters/Setters : Class (LinkTrackingParamSettings) => Property (LTPSID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_Class_Invalid_Property_LTPSIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLTPSID = "LTPSIDNotPresent";
            var linkTrackingParamSettings  = Fixture.Create<LinkTrackingParamSettings>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParamSettings.GetType().GetProperty(propertyNameLTPSID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_LTPSID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLTPSID = "LTPSID";
            var linkTrackingParamSettings  = Fixture.Create<LinkTrackingParamSettings>();
            var propertyInfo  = linkTrackingParamSettings.GetType().GetProperty(propertyNameLTPSID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamSettings) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var linkTrackingParamSettings = Fixture.Create<LinkTrackingParamSettings>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = linkTrackingParamSettings.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(linkTrackingParamSettings, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamSettings) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var linkTrackingParamSettings  = Fixture.Create<LinkTrackingParamSettings>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParamSettings.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var linkTrackingParamSettings  = Fixture.Create<LinkTrackingParamSettings>();
            var propertyInfo  = linkTrackingParamSettings.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamSettings) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkTrackingParamSettings = Fixture.Create<LinkTrackingParamSettings>();
            var random = Fixture.Create<int>();

            // Act , Set
            linkTrackingParamSettings.UpdatedUserID = random;

            // Assert
            linkTrackingParamSettings.UpdatedUserID.ShouldBe(random);
            linkTrackingParamSettings.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkTrackingParamSettings = Fixture.Create<LinkTrackingParamSettings>();

            // Act , Set
            linkTrackingParamSettings.UpdatedUserID = null;

            // Assert
            linkTrackingParamSettings.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var linkTrackingParamSettings = Fixture.Create<LinkTrackingParamSettings>();
            var propertyInfo = linkTrackingParamSettings.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(linkTrackingParamSettings, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkTrackingParamSettings.UpdatedUserID.ShouldBeNull();
            linkTrackingParamSettings.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingParamSettings) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var linkTrackingParamSettings  = Fixture.Create<LinkTrackingParamSettings>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingParamSettings.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingParamSettings_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var linkTrackingParamSettings  = Fixture.Create<LinkTrackingParamSettings>();
            var propertyInfo  = linkTrackingParamSettings.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (LinkTrackingParamSettings) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkTrackingParamSettings_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new LinkTrackingParamSettings());
        }

        #endregion

        #region General Constructor : Class (LinkTrackingParamSettings) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkTrackingParamSettings_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfLinkTrackingParamSettings = Fixture.CreateMany<LinkTrackingParamSettings>(2).ToList();
            var firstLinkTrackingParamSettings = instancesOfLinkTrackingParamSettings.FirstOrDefault();
            var lastLinkTrackingParamSettings = instancesOfLinkTrackingParamSettings.Last();

            // Act, Assert
            firstLinkTrackingParamSettings.ShouldNotBeNull();
            lastLinkTrackingParamSettings.ShouldNotBeNull();
            firstLinkTrackingParamSettings.ShouldNotBeSameAs(lastLinkTrackingParamSettings);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkTrackingParamSettings_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstLinkTrackingParamSettings = new LinkTrackingParamSettings();
            var secondLinkTrackingParamSettings = new LinkTrackingParamSettings();
            var thirdLinkTrackingParamSettings = new LinkTrackingParamSettings();
            var fourthLinkTrackingParamSettings = new LinkTrackingParamSettings();
            var fifthLinkTrackingParamSettings = new LinkTrackingParamSettings();
            var sixthLinkTrackingParamSettings = new LinkTrackingParamSettings();

            // Act, Assert
            firstLinkTrackingParamSettings.ShouldNotBeNull();
            secondLinkTrackingParamSettings.ShouldNotBeNull();
            thirdLinkTrackingParamSettings.ShouldNotBeNull();
            fourthLinkTrackingParamSettings.ShouldNotBeNull();
            fifthLinkTrackingParamSettings.ShouldNotBeNull();
            sixthLinkTrackingParamSettings.ShouldNotBeNull();
            firstLinkTrackingParamSettings.ShouldNotBeSameAs(secondLinkTrackingParamSettings);
            thirdLinkTrackingParamSettings.ShouldNotBeSameAs(firstLinkTrackingParamSettings);
            fourthLinkTrackingParamSettings.ShouldNotBeSameAs(firstLinkTrackingParamSettings);
            fifthLinkTrackingParamSettings.ShouldNotBeSameAs(firstLinkTrackingParamSettings);
            sixthLinkTrackingParamSettings.ShouldNotBeSameAs(firstLinkTrackingParamSettings);
            sixthLinkTrackingParamSettings.ShouldNotBeSameAs(fourthLinkTrackingParamSettings);
        }

        #endregion

        #region General Constructor : Class (LinkTrackingParamSettings) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkTrackingParamSettings_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var lTPSId = -1;
            var lTPId = -1;
            var displayName = string.Empty;
            var allowCustom = false;
            var isRequired = false;
            var isDeleted = false;

            // Act
            var linkTrackingParamSettings = new LinkTrackingParamSettings();

            // Assert
            linkTrackingParamSettings.LTPSID.ShouldBe(lTPSId);
            linkTrackingParamSettings.LTPID.ShouldBe(lTPId);
            linkTrackingParamSettings.CustomerID.ShouldBeNull();
            linkTrackingParamSettings.BaseChannelID.ShouldBeNull();
            linkTrackingParamSettings.DisplayName.ShouldBe(displayName);
            linkTrackingParamSettings.AllowCustom.ShouldBeFalse();
            linkTrackingParamSettings.IsRequired.ShouldBeFalse();
            linkTrackingParamSettings.CreatedUserID.ShouldBeNull();
            linkTrackingParamSettings.CreatedDate.ShouldBeNull();
            linkTrackingParamSettings.UpdatedUserID.ShouldBeNull();
            linkTrackingParamSettings.UpdatedDate.ShouldBeNull();
            linkTrackingParamSettings.IsDeleted.ShouldBeFalse();
        }

        #endregion

        #endregion

        #endregion
    }
}