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
    public class GroupConfigTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (GroupConfig) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var groupConfig = Fixture.Create<GroupConfig>();
            var groupConfigId = Fixture.Create<int>();
            var customerId = Fixture.Create<int>();
            var shortName = Fixture.Create<string>();
            var isPublic = Fixture.Create<string>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var isDeleted = Fixture.Create<bool?>();

            // Act
            groupConfig.GroupConfigID = groupConfigId;
            groupConfig.CustomerID = customerId;
            groupConfig.ShortName = shortName;
            groupConfig.IsPublic = isPublic;
            groupConfig.CreatedUserID = createdUserId;
            groupConfig.CreatedDate = createdDate;
            groupConfig.UpdatedUserID = updatedUserId;
            groupConfig.UpdatedDate = updatedDate;
            groupConfig.IsDeleted = isDeleted;

            // Assert
            groupConfig.GroupConfigID.ShouldBe(groupConfigId);
            groupConfig.CustomerID.ShouldBe(customerId);
            groupConfig.ShortName.ShouldBe(shortName);
            groupConfig.IsPublic.ShouldBe(isPublic);
            groupConfig.CreatedUserID.ShouldBe(createdUserId);
            groupConfig.CreatedDate.ShouldBe(createdDate);
            groupConfig.UpdatedUserID.ShouldBe(updatedUserId);
            groupConfig.UpdatedDate.ShouldBe(updatedDate);
            groupConfig.IsDeleted.ShouldBe(isDeleted);
        }

        #endregion

        #region General Getters/Setters : Class (GroupConfig) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var groupConfig = Fixture.Create<GroupConfig>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = groupConfig.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(groupConfig, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (GroupConfig) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var groupConfig  = Fixture.Create<GroupConfig>();

            // Act , Assert
            Should.NotThrow(() => groupConfig.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var groupConfig  = Fixture.Create<GroupConfig>();
            var propertyInfo  = groupConfig.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupConfig) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var groupConfig = Fixture.Create<GroupConfig>();
            var random = Fixture.Create<int>();

            // Act , Set
            groupConfig.CreatedUserID = random;

            // Assert
            groupConfig.CreatedUserID.ShouldBe(random);
            groupConfig.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var groupConfig = Fixture.Create<GroupConfig>();

            // Act , Set
            groupConfig.CreatedUserID = null;

            // Assert
            groupConfig.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var groupConfig = Fixture.Create<GroupConfig>();
            var propertyInfo = groupConfig.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(groupConfig, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            groupConfig.CreatedUserID.ShouldBeNull();
            groupConfig.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (GroupConfig) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var groupConfig  = Fixture.Create<GroupConfig>();

            // Act , Assert
            Should.NotThrow(() => groupConfig.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var groupConfig  = Fixture.Create<GroupConfig>();
            var propertyInfo  = groupConfig.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupConfig) => Property (CustomerID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_CustomerID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var groupConfig = Fixture.Create<GroupConfig>();
            groupConfig.CustomerID = Fixture.Create<int>();
            var intType = groupConfig.CustomerID.GetType();

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

        #region General Getters/Setters : Class (GroupConfig) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var groupConfig  = Fixture.Create<GroupConfig>();

            // Act , Assert
            Should.NotThrow(() => groupConfig.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var groupConfig  = Fixture.Create<GroupConfig>();
            var propertyInfo  = groupConfig.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupConfig) => Property (GroupConfigID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_GroupConfigID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var groupConfig = Fixture.Create<GroupConfig>();
            groupConfig.GroupConfigID = Fixture.Create<int>();
            var intType = groupConfig.GroupConfigID.GetType();

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

        #region General Getters/Setters : Class (GroupConfig) => Property (GroupConfigID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_Class_Invalid_Property_GroupConfigIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupConfigID = "GroupConfigIDNotPresent";
            var groupConfig  = Fixture.Create<GroupConfig>();

            // Act , Assert
            Should.NotThrow(() => groupConfig.GetType().GetProperty(propertyNameGroupConfigID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_GroupConfigID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupConfigID = "GroupConfigID";
            var groupConfig  = Fixture.Create<GroupConfig>();
            var propertyInfo  = groupConfig.GetType().GetProperty(propertyNameGroupConfigID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupConfig) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var groupConfig = Fixture.Create<GroupConfig>();
            var random = Fixture.Create<bool>();

            // Act , Set
            groupConfig.IsDeleted = random;

            // Assert
            groupConfig.IsDeleted.ShouldBe(random);
            groupConfig.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var groupConfig = Fixture.Create<GroupConfig>();

            // Act , Set
            groupConfig.IsDeleted = null;

            // Assert
            groupConfig.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var groupConfig = Fixture.Create<GroupConfig>();
            var propertyInfo = groupConfig.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(groupConfig, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            groupConfig.IsDeleted.ShouldBeNull();
            groupConfig.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (GroupConfig) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var groupConfig  = Fixture.Create<GroupConfig>();

            // Act , Assert
            Should.NotThrow(() => groupConfig.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var groupConfig  = Fixture.Create<GroupConfig>();
            var propertyInfo  = groupConfig.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupConfig) => Property (IsPublic) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_IsPublic_Property_String_Type_Verify_Test()
        {
            // Arrange
            var groupConfig = Fixture.Create<GroupConfig>();
            groupConfig.IsPublic = Fixture.Create<string>();
            var stringType = groupConfig.IsPublic.GetType();

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

        #region General Getters/Setters : Class (GroupConfig) => Property (IsPublic) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_Class_Invalid_Property_IsPublicNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsPublic = "IsPublicNotPresent";
            var groupConfig  = Fixture.Create<GroupConfig>();

            // Act , Assert
            Should.NotThrow(() => groupConfig.GetType().GetProperty(propertyNameIsPublic));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_IsPublic_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsPublic = "IsPublic";
            var groupConfig  = Fixture.Create<GroupConfig>();
            var propertyInfo  = groupConfig.GetType().GetProperty(propertyNameIsPublic);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupConfig) => Property (ShortName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_ShortName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var groupConfig = Fixture.Create<GroupConfig>();
            groupConfig.ShortName = Fixture.Create<string>();
            var stringType = groupConfig.ShortName.GetType();

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

        #region General Getters/Setters : Class (GroupConfig) => Property (ShortName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_Class_Invalid_Property_ShortNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameShortName = "ShortNameNotPresent";
            var groupConfig  = Fixture.Create<GroupConfig>();

            // Act , Assert
            Should.NotThrow(() => groupConfig.GetType().GetProperty(propertyNameShortName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_ShortName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameShortName = "ShortName";
            var groupConfig  = Fixture.Create<GroupConfig>();
            var propertyInfo  = groupConfig.GetType().GetProperty(propertyNameShortName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupConfig) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var groupConfig = Fixture.Create<GroupConfig>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = groupConfig.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(groupConfig, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (GroupConfig) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var groupConfig  = Fixture.Create<GroupConfig>();

            // Act , Assert
            Should.NotThrow(() => groupConfig.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var groupConfig  = Fixture.Create<GroupConfig>();
            var propertyInfo  = groupConfig.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupConfig) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var groupConfig = Fixture.Create<GroupConfig>();
            var random = Fixture.Create<int>();

            // Act , Set
            groupConfig.UpdatedUserID = random;

            // Assert
            groupConfig.UpdatedUserID.ShouldBe(random);
            groupConfig.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var groupConfig = Fixture.Create<GroupConfig>();

            // Act , Set
            groupConfig.UpdatedUserID = null;

            // Assert
            groupConfig.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var groupConfig = Fixture.Create<GroupConfig>();
            var propertyInfo = groupConfig.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(groupConfig, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            groupConfig.UpdatedUserID.ShouldBeNull();
            groupConfig.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (GroupConfig) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var groupConfig  = Fixture.Create<GroupConfig>();

            // Act , Assert
            Should.NotThrow(() => groupConfig.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupConfig_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var groupConfig  = Fixture.Create<GroupConfig>();
            var propertyInfo  = groupConfig.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (GroupConfig) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_GroupConfig_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new GroupConfig());
        }

        #endregion

        #region General Constructor : Class (GroupConfig) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_GroupConfig_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfGroupConfig = Fixture.CreateMany<GroupConfig>(2).ToList();
            var firstGroupConfig = instancesOfGroupConfig.FirstOrDefault();
            var lastGroupConfig = instancesOfGroupConfig.Last();

            // Act, Assert
            firstGroupConfig.ShouldNotBeNull();
            lastGroupConfig.ShouldNotBeNull();
            firstGroupConfig.ShouldNotBeSameAs(lastGroupConfig);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_GroupConfig_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstGroupConfig = new GroupConfig();
            var secondGroupConfig = new GroupConfig();
            var thirdGroupConfig = new GroupConfig();
            var fourthGroupConfig = new GroupConfig();
            var fifthGroupConfig = new GroupConfig();
            var sixthGroupConfig = new GroupConfig();

            // Act, Assert
            firstGroupConfig.ShouldNotBeNull();
            secondGroupConfig.ShouldNotBeNull();
            thirdGroupConfig.ShouldNotBeNull();
            fourthGroupConfig.ShouldNotBeNull();
            fifthGroupConfig.ShouldNotBeNull();
            sixthGroupConfig.ShouldNotBeNull();
            firstGroupConfig.ShouldNotBeSameAs(secondGroupConfig);
            thirdGroupConfig.ShouldNotBeSameAs(firstGroupConfig);
            fourthGroupConfig.ShouldNotBeSameAs(firstGroupConfig);
            fifthGroupConfig.ShouldNotBeSameAs(firstGroupConfig);
            sixthGroupConfig.ShouldNotBeSameAs(firstGroupConfig);
            sixthGroupConfig.ShouldNotBeSameAs(fourthGroupConfig);
        }

        #endregion

        #region General Constructor : Class (GroupConfig) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_GroupConfig_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var groupConfigId = -1;
            var customerId = -1;

            // Act
            var groupConfig = new GroupConfig();

            // Assert
            groupConfig.GroupConfigID.ShouldBe(groupConfigId);
            groupConfig.CustomerID.ShouldBe(customerId);
            groupConfig.ShortName.ShouldBeNull();
            groupConfig.IsPublic.ShouldBeNull();
            groupConfig.CreatedUserID.ShouldBeNull();
            groupConfig.CreatedDate.ShouldBeNull();
            groupConfig.UpdatedUserID.ShouldBeNull();
            groupConfig.UpdatedDate.ShouldBeNull();
            groupConfig.IsDeleted.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}