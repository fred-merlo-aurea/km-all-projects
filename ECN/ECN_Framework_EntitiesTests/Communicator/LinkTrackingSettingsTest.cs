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
    public class LinkTrackingSettingsTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (LinkTrackingSettings) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var linkTrackingSettings = Fixture.Create<LinkTrackingSettings>();
            var lTSId = Fixture.Create<int>();
            var lTId = Fixture.Create<int>();
            var customerId = Fixture.Create<int?>();
            var baseChannelId = Fixture.Create<int?>();
            var xMLConfig = Fixture.Create<string>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var isDeleted = Fixture.Create<bool>();

            // Act
            linkTrackingSettings.LTSID = lTSId;
            linkTrackingSettings.LTID = lTId;
            linkTrackingSettings.CustomerID = customerId;
            linkTrackingSettings.BaseChannelID = baseChannelId;
            linkTrackingSettings.XMLConfig = xMLConfig;
            linkTrackingSettings.CreatedUserID = createdUserId;
            linkTrackingSettings.CreatedDate = createdDate;
            linkTrackingSettings.UpdatedUserID = updatedUserId;
            linkTrackingSettings.UpdatedDate = updatedDate;
            linkTrackingSettings.IsDeleted = isDeleted;

            // Assert
            linkTrackingSettings.LTSID.ShouldBe(lTSId);
            linkTrackingSettings.LTID.ShouldBe(lTId);
            linkTrackingSettings.CustomerID.ShouldBe(customerId);
            linkTrackingSettings.BaseChannelID.ShouldBe(baseChannelId);
            linkTrackingSettings.XMLConfig.ShouldBe(xMLConfig);
            linkTrackingSettings.CreatedUserID.ShouldBe(createdUserId);
            linkTrackingSettings.CreatedDate.ShouldBe(createdDate);
            linkTrackingSettings.UpdatedUserID.ShouldBe(updatedUserId);
            linkTrackingSettings.UpdatedDate.ShouldBe(updatedDate);
            linkTrackingSettings.IsDeleted.ShouldBe(isDeleted);
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingSettings) => Property (BaseChannelID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_BaseChannelID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkTrackingSettings = Fixture.Create<LinkTrackingSettings>();
            var random = Fixture.Create<int>();

            // Act , Set
            linkTrackingSettings.BaseChannelID = random;

            // Assert
            linkTrackingSettings.BaseChannelID.ShouldBe(random);
            linkTrackingSettings.BaseChannelID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_BaseChannelID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkTrackingSettings = Fixture.Create<LinkTrackingSettings>();

            // Act , Set
            linkTrackingSettings.BaseChannelID = null;

            // Assert
            linkTrackingSettings.BaseChannelID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_BaseChannelID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBaseChannelID = "BaseChannelID";
            var linkTrackingSettings = Fixture.Create<LinkTrackingSettings>();
            var propertyInfo = linkTrackingSettings.GetType().GetProperty(propertyNameBaseChannelID);

            // Act , Set
            propertyInfo.SetValue(linkTrackingSettings, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkTrackingSettings.BaseChannelID.ShouldBeNull();
            linkTrackingSettings.BaseChannelID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingSettings) => Property (BaseChannelID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_Class_Invalid_Property_BaseChannelIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelIDNotPresent";
            var linkTrackingSettings  = Fixture.Create<LinkTrackingSettings>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingSettings.GetType().GetProperty(propertyNameBaseChannelID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_BaseChannelID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelID";
            var linkTrackingSettings  = Fixture.Create<LinkTrackingSettings>();
            var propertyInfo  = linkTrackingSettings.GetType().GetProperty(propertyNameBaseChannelID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingSettings) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var linkTrackingSettings = Fixture.Create<LinkTrackingSettings>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = linkTrackingSettings.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(linkTrackingSettings, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingSettings) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var linkTrackingSettings  = Fixture.Create<LinkTrackingSettings>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingSettings.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var linkTrackingSettings  = Fixture.Create<LinkTrackingSettings>();
            var propertyInfo  = linkTrackingSettings.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingSettings) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkTrackingSettings = Fixture.Create<LinkTrackingSettings>();
            var random = Fixture.Create<int>();

            // Act , Set
            linkTrackingSettings.CreatedUserID = random;

            // Assert
            linkTrackingSettings.CreatedUserID.ShouldBe(random);
            linkTrackingSettings.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkTrackingSettings = Fixture.Create<LinkTrackingSettings>();

            // Act , Set
            linkTrackingSettings.CreatedUserID = null;

            // Assert
            linkTrackingSettings.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var linkTrackingSettings = Fixture.Create<LinkTrackingSettings>();
            var propertyInfo = linkTrackingSettings.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(linkTrackingSettings, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkTrackingSettings.CreatedUserID.ShouldBeNull();
            linkTrackingSettings.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingSettings) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var linkTrackingSettings  = Fixture.Create<LinkTrackingSettings>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingSettings.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var linkTrackingSettings  = Fixture.Create<LinkTrackingSettings>();
            var propertyInfo  = linkTrackingSettings.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingSettings) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkTrackingSettings = Fixture.Create<LinkTrackingSettings>();
            var random = Fixture.Create<int>();

            // Act , Set
            linkTrackingSettings.CustomerID = random;

            // Assert
            linkTrackingSettings.CustomerID.ShouldBe(random);
            linkTrackingSettings.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkTrackingSettings = Fixture.Create<LinkTrackingSettings>();

            // Act , Set
            linkTrackingSettings.CustomerID = null;

            // Assert
            linkTrackingSettings.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var linkTrackingSettings = Fixture.Create<LinkTrackingSettings>();
            var propertyInfo = linkTrackingSettings.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(linkTrackingSettings, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkTrackingSettings.CustomerID.ShouldBeNull();
            linkTrackingSettings.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingSettings) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var linkTrackingSettings  = Fixture.Create<LinkTrackingSettings>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingSettings.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var linkTrackingSettings  = Fixture.Create<LinkTrackingSettings>();
            var propertyInfo  = linkTrackingSettings.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingSettings) => Property (IsDeleted) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_IsDeleted_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var linkTrackingSettings = Fixture.Create<LinkTrackingSettings>();
            linkTrackingSettings.IsDeleted = Fixture.Create<bool>();
            var boolType = linkTrackingSettings.IsDeleted.GetType();

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

        #region General Getters/Setters : Class (LinkTrackingSettings) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var linkTrackingSettings  = Fixture.Create<LinkTrackingSettings>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingSettings.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var linkTrackingSettings  = Fixture.Create<LinkTrackingSettings>();
            var propertyInfo  = linkTrackingSettings.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingSettings) => Property (LTID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_LTID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var linkTrackingSettings = Fixture.Create<LinkTrackingSettings>();
            linkTrackingSettings.LTID = Fixture.Create<int>();
            var intType = linkTrackingSettings.LTID.GetType();

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

        #region General Getters/Setters : Class (LinkTrackingSettings) => Property (LTID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_Class_Invalid_Property_LTIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLTID = "LTIDNotPresent";
            var linkTrackingSettings  = Fixture.Create<LinkTrackingSettings>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingSettings.GetType().GetProperty(propertyNameLTID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_LTID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLTID = "LTID";
            var linkTrackingSettings  = Fixture.Create<LinkTrackingSettings>();
            var propertyInfo  = linkTrackingSettings.GetType().GetProperty(propertyNameLTID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingSettings) => Property (LTSID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_LTSID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var linkTrackingSettings = Fixture.Create<LinkTrackingSettings>();
            linkTrackingSettings.LTSID = Fixture.Create<int>();
            var intType = linkTrackingSettings.LTSID.GetType();

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

        #region General Getters/Setters : Class (LinkTrackingSettings) => Property (LTSID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_Class_Invalid_Property_LTSIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLTSID = "LTSIDNotPresent";
            var linkTrackingSettings  = Fixture.Create<LinkTrackingSettings>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingSettings.GetType().GetProperty(propertyNameLTSID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_LTSID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLTSID = "LTSID";
            var linkTrackingSettings  = Fixture.Create<LinkTrackingSettings>();
            var propertyInfo  = linkTrackingSettings.GetType().GetProperty(propertyNameLTSID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingSettings) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var linkTrackingSettings = Fixture.Create<LinkTrackingSettings>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = linkTrackingSettings.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(linkTrackingSettings, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingSettings) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var linkTrackingSettings  = Fixture.Create<LinkTrackingSettings>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingSettings.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var linkTrackingSettings  = Fixture.Create<LinkTrackingSettings>();
            var propertyInfo  = linkTrackingSettings.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingSettings) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkTrackingSettings = Fixture.Create<LinkTrackingSettings>();
            var random = Fixture.Create<int>();

            // Act , Set
            linkTrackingSettings.UpdatedUserID = random;

            // Assert
            linkTrackingSettings.UpdatedUserID.ShouldBe(random);
            linkTrackingSettings.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkTrackingSettings = Fixture.Create<LinkTrackingSettings>();

            // Act , Set
            linkTrackingSettings.UpdatedUserID = null;

            // Assert
            linkTrackingSettings.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var linkTrackingSettings = Fixture.Create<LinkTrackingSettings>();
            var propertyInfo = linkTrackingSettings.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(linkTrackingSettings, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkTrackingSettings.UpdatedUserID.ShouldBeNull();
            linkTrackingSettings.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingSettings) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var linkTrackingSettings  = Fixture.Create<LinkTrackingSettings>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingSettings.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var linkTrackingSettings  = Fixture.Create<LinkTrackingSettings>();
            var propertyInfo  = linkTrackingSettings.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingSettings) => Property (XMLConfig) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_XMLConfig_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkTrackingSettings = Fixture.Create<LinkTrackingSettings>();
            linkTrackingSettings.XMLConfig = Fixture.Create<string>();
            var stringType = linkTrackingSettings.XMLConfig.GetType();

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

        #region General Getters/Setters : Class (LinkTrackingSettings) => Property (XMLConfig) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_Class_Invalid_Property_XMLConfigNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameXMLConfig = "XMLConfigNotPresent";
            var linkTrackingSettings  = Fixture.Create<LinkTrackingSettings>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingSettings.GetType().GetProperty(propertyNameXMLConfig));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingSettings_XMLConfig_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameXMLConfig = "XMLConfig";
            var linkTrackingSettings  = Fixture.Create<LinkTrackingSettings>();
            var propertyInfo  = linkTrackingSettings.GetType().GetProperty(propertyNameXMLConfig);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (LinkTrackingSettings) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkTrackingSettings_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new LinkTrackingSettings());
        }

        #endregion

        #region General Constructor : Class (LinkTrackingSettings) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkTrackingSettings_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfLinkTrackingSettings = Fixture.CreateMany<LinkTrackingSettings>(2).ToList();
            var firstLinkTrackingSettings = instancesOfLinkTrackingSettings.FirstOrDefault();
            var lastLinkTrackingSettings = instancesOfLinkTrackingSettings.Last();

            // Act, Assert
            firstLinkTrackingSettings.ShouldNotBeNull();
            lastLinkTrackingSettings.ShouldNotBeNull();
            firstLinkTrackingSettings.ShouldNotBeSameAs(lastLinkTrackingSettings);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkTrackingSettings_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstLinkTrackingSettings = new LinkTrackingSettings();
            var secondLinkTrackingSettings = new LinkTrackingSettings();
            var thirdLinkTrackingSettings = new LinkTrackingSettings();
            var fourthLinkTrackingSettings = new LinkTrackingSettings();
            var fifthLinkTrackingSettings = new LinkTrackingSettings();
            var sixthLinkTrackingSettings = new LinkTrackingSettings();

            // Act, Assert
            firstLinkTrackingSettings.ShouldNotBeNull();
            secondLinkTrackingSettings.ShouldNotBeNull();
            thirdLinkTrackingSettings.ShouldNotBeNull();
            fourthLinkTrackingSettings.ShouldNotBeNull();
            fifthLinkTrackingSettings.ShouldNotBeNull();
            sixthLinkTrackingSettings.ShouldNotBeNull();
            firstLinkTrackingSettings.ShouldNotBeSameAs(secondLinkTrackingSettings);
            thirdLinkTrackingSettings.ShouldNotBeSameAs(firstLinkTrackingSettings);
            fourthLinkTrackingSettings.ShouldNotBeSameAs(firstLinkTrackingSettings);
            fifthLinkTrackingSettings.ShouldNotBeSameAs(firstLinkTrackingSettings);
            sixthLinkTrackingSettings.ShouldNotBeSameAs(firstLinkTrackingSettings);
            sixthLinkTrackingSettings.ShouldNotBeSameAs(fourthLinkTrackingSettings);
        }

        #endregion

        #region General Constructor : Class (LinkTrackingSettings) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkTrackingSettings_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var lTSId = -1;
            var lTId = -1;
            var xMLConfig = string.Empty;
            var createdDate = DateTime.MinValue;
            var isDeleted = false;

            // Act
            var linkTrackingSettings = new LinkTrackingSettings();

            // Assert
            linkTrackingSettings.LTSID.ShouldBe(lTSId);
            linkTrackingSettings.LTID.ShouldBe(lTId);
            linkTrackingSettings.CustomerID.ShouldBeNull();
            linkTrackingSettings.BaseChannelID.ShouldBeNull();
            linkTrackingSettings.XMLConfig.ShouldBe(xMLConfig);
            linkTrackingSettings.CreatedUserID.ShouldBeNull();
            linkTrackingSettings.CreatedDate.ShouldBe(createdDate);
            linkTrackingSettings.UpdatedUserID.ShouldBeNull();
            linkTrackingSettings.UpdatedDate.ShouldBeNull();
            linkTrackingSettings.IsDeleted.ShouldBeFalse();
        }

        #endregion

        #endregion

        #endregion
    }
}