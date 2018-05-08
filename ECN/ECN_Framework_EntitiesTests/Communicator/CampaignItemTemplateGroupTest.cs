using System;
using System.Collections.Generic;
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
    public class CampaignItemTemplateGroupTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (CampaignItemTemplateGroup) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var campaignItemTemplateGroup = Fixture.Create<CampaignItemTemplateGroup>();
            var campaignItemTemplateGroupId = Fixture.Create<int>();
            var campaignItemTemplateId = Fixture.Create<int>();
            var groupId = Fixture.Create<int>();
            var isDeleted = Fixture.Create<bool?>();
            var createdDate = Fixture.Create<DateTime?>();
            var createdUserId = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var filters = Fixture.Create<List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter>>();

            // Act
            campaignItemTemplateGroup.CampaignItemTemplateGroupID = campaignItemTemplateGroupId;
            campaignItemTemplateGroup.CampaignItemTemplateID = campaignItemTemplateId;
            campaignItemTemplateGroup.GroupID = groupId;
            campaignItemTemplateGroup.IsDeleted = isDeleted;
            campaignItemTemplateGroup.CreatedDate = createdDate;
            campaignItemTemplateGroup.CreatedUserID = createdUserId;
            campaignItemTemplateGroup.UpdatedDate = updatedDate;
            campaignItemTemplateGroup.UpdatedUserID = updatedUserId;
            campaignItemTemplateGroup.Filters = filters;

            // Assert
            campaignItemTemplateGroup.CampaignItemTemplateGroupID.ShouldBe(campaignItemTemplateGroupId);
            campaignItemTemplateGroup.CampaignItemTemplateID.ShouldBe(campaignItemTemplateId);
            campaignItemTemplateGroup.GroupID.ShouldBe(groupId);
            campaignItemTemplateGroup.IsDeleted.ShouldBe(isDeleted);
            campaignItemTemplateGroup.CreatedDate.ShouldBe(createdDate);
            campaignItemTemplateGroup.CreatedUserID.ShouldBe(createdUserId);
            campaignItemTemplateGroup.UpdatedDate.ShouldBe(updatedDate);
            campaignItemTemplateGroup.UpdatedUserID.ShouldBe(updatedUserId);
            campaignItemTemplateGroup.Filters.ShouldBe(filters);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemTemplateGroup) => Property (CampaignItemTemplateGroupID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_CampaignItemTemplateGroupID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignItemTemplateGroup = Fixture.Create<CampaignItemTemplateGroup>();
            campaignItemTemplateGroup.CampaignItemTemplateGroupID = Fixture.Create<int>();
            var intType = campaignItemTemplateGroup.CampaignItemTemplateGroupID.GetType();

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

        #region General Getters/Setters : Class (CampaignItemTemplateGroup) => Property (CampaignItemTemplateGroupID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_Class_Invalid_Property_CampaignItemTemplateGroupIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemTemplateGroupID = "CampaignItemTemplateGroupIDNotPresent";
            var campaignItemTemplateGroup  = Fixture.Create<CampaignItemTemplateGroup>();

            // Act , Assert
            Should.NotThrow(() => campaignItemTemplateGroup.GetType().GetProperty(propertyNameCampaignItemTemplateGroupID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_CampaignItemTemplateGroupID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemTemplateGroupID = "CampaignItemTemplateGroupID";
            var campaignItemTemplateGroup  = Fixture.Create<CampaignItemTemplateGroup>();
            var propertyInfo  = campaignItemTemplateGroup.GetType().GetProperty(propertyNameCampaignItemTemplateGroupID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemTemplateGroup) => Property (CampaignItemTemplateID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_CampaignItemTemplateID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignItemTemplateGroup = Fixture.Create<CampaignItemTemplateGroup>();
            campaignItemTemplateGroup.CampaignItemTemplateID = Fixture.Create<int>();
            var intType = campaignItemTemplateGroup.CampaignItemTemplateID.GetType();

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

        #region General Getters/Setters : Class (CampaignItemTemplateGroup) => Property (CampaignItemTemplateID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_Class_Invalid_Property_CampaignItemTemplateIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemTemplateID = "CampaignItemTemplateIDNotPresent";
            var campaignItemTemplateGroup  = Fixture.Create<CampaignItemTemplateGroup>();

            // Act , Assert
            Should.NotThrow(() => campaignItemTemplateGroup.GetType().GetProperty(propertyNameCampaignItemTemplateID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_CampaignItemTemplateID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemTemplateID = "CampaignItemTemplateID";
            var campaignItemTemplateGroup  = Fixture.Create<CampaignItemTemplateGroup>();
            var propertyInfo  = campaignItemTemplateGroup.GetType().GetProperty(propertyNameCampaignItemTemplateID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemTemplateGroup) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var campaignItemTemplateGroup = Fixture.Create<CampaignItemTemplateGroup>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = campaignItemTemplateGroup.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(campaignItemTemplateGroup, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemTemplateGroup) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var campaignItemTemplateGroup  = Fixture.Create<CampaignItemTemplateGroup>();

            // Act , Assert
            Should.NotThrow(() => campaignItemTemplateGroup.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var campaignItemTemplateGroup  = Fixture.Create<CampaignItemTemplateGroup>();
            var propertyInfo  = campaignItemTemplateGroup.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemTemplateGroup) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemTemplateGroup = Fixture.Create<CampaignItemTemplateGroup>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemTemplateGroup.CreatedUserID = random;

            // Assert
            campaignItemTemplateGroup.CreatedUserID.ShouldBe(random);
            campaignItemTemplateGroup.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemTemplateGroup = Fixture.Create<CampaignItemTemplateGroup>();    

            // Act , Set
            campaignItemTemplateGroup.CreatedUserID = null;

            // Assert
            campaignItemTemplateGroup.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var campaignItemTemplateGroup = Fixture.Create<CampaignItemTemplateGroup>();
            var propertyInfo = campaignItemTemplateGroup.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(campaignItemTemplateGroup, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemTemplateGroup.CreatedUserID.ShouldBeNull();
            campaignItemTemplateGroup.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemTemplateGroup) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var campaignItemTemplateGroup  = Fixture.Create<CampaignItemTemplateGroup>();

            // Act , Assert
            Should.NotThrow(() => campaignItemTemplateGroup.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var campaignItemTemplateGroup  = Fixture.Create<CampaignItemTemplateGroup>();
            var propertyInfo  = campaignItemTemplateGroup.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemTemplateGroup) => Property (Filters) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_Class_Invalid_Property_FiltersNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFilters = "FiltersNotPresent";
            var campaignItemTemplateGroup  = Fixture.Create<CampaignItemTemplateGroup>();

            // Act , Assert
            Should.NotThrow(() => campaignItemTemplateGroup.GetType().GetProperty(propertyNameFilters));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_Filters_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFilters = "Filters";
            var campaignItemTemplateGroup  = Fixture.Create<CampaignItemTemplateGroup>();
            var propertyInfo  = campaignItemTemplateGroup.GetType().GetProperty(propertyNameFilters);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemTemplateGroup) => Property (GroupID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_GroupID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignItemTemplateGroup = Fixture.Create<CampaignItemTemplateGroup>();
            campaignItemTemplateGroup.GroupID = Fixture.Create<int>();
            var intType = campaignItemTemplateGroup.GroupID.GetType();

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

        #region General Getters/Setters : Class (CampaignItemTemplateGroup) => Property (GroupID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_Class_Invalid_Property_GroupIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupIDNotPresent";
            var campaignItemTemplateGroup  = Fixture.Create<CampaignItemTemplateGroup>();

            // Act , Assert
            Should.NotThrow(() => campaignItemTemplateGroup.GetType().GetProperty(propertyNameGroupID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_GroupID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupID";
            var campaignItemTemplateGroup  = Fixture.Create<CampaignItemTemplateGroup>();
            var propertyInfo  = campaignItemTemplateGroup.GetType().GetProperty(propertyNameGroupID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemTemplateGroup) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemTemplateGroup = Fixture.Create<CampaignItemTemplateGroup>();
            var random = Fixture.Create<bool>();

            // Act , Set
            campaignItemTemplateGroup.IsDeleted = random;

            // Assert
            campaignItemTemplateGroup.IsDeleted.ShouldBe(random);
            campaignItemTemplateGroup.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemTemplateGroup = Fixture.Create<CampaignItemTemplateGroup>();    

            // Act , Set
            campaignItemTemplateGroup.IsDeleted = null;

            // Assert
            campaignItemTemplateGroup.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var campaignItemTemplateGroup = Fixture.Create<CampaignItemTemplateGroup>();
            var propertyInfo = campaignItemTemplateGroup.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(campaignItemTemplateGroup, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemTemplateGroup.IsDeleted.ShouldBeNull();
            campaignItemTemplateGroup.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemTemplateGroup) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var campaignItemTemplateGroup  = Fixture.Create<CampaignItemTemplateGroup>();

            // Act , Assert
            Should.NotThrow(() => campaignItemTemplateGroup.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var campaignItemTemplateGroup  = Fixture.Create<CampaignItemTemplateGroup>();
            var propertyInfo  = campaignItemTemplateGroup.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemTemplateGroup) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var campaignItemTemplateGroup = Fixture.Create<CampaignItemTemplateGroup>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = campaignItemTemplateGroup.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(campaignItemTemplateGroup, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemTemplateGroup) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var campaignItemTemplateGroup  = Fixture.Create<CampaignItemTemplateGroup>();

            // Act , Assert
            Should.NotThrow(() => campaignItemTemplateGroup.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var campaignItemTemplateGroup  = Fixture.Create<CampaignItemTemplateGroup>();
            var propertyInfo  = campaignItemTemplateGroup.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemTemplateGroup) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemTemplateGroup = Fixture.Create<CampaignItemTemplateGroup>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemTemplateGroup.UpdatedUserID = random;

            // Assert
            campaignItemTemplateGroup.UpdatedUserID.ShouldBe(random);
            campaignItemTemplateGroup.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemTemplateGroup = Fixture.Create<CampaignItemTemplateGroup>();    

            // Act , Set
            campaignItemTemplateGroup.UpdatedUserID = null;

            // Assert
            campaignItemTemplateGroup.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var campaignItemTemplateGroup = Fixture.Create<CampaignItemTemplateGroup>();
            var propertyInfo = campaignItemTemplateGroup.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(campaignItemTemplateGroup, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemTemplateGroup.UpdatedUserID.ShouldBeNull();
            campaignItemTemplateGroup.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemTemplateGroup) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var campaignItemTemplateGroup  = Fixture.Create<CampaignItemTemplateGroup>();

            // Act , Assert
            Should.NotThrow(() => campaignItemTemplateGroup.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemTemplateGroup_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var campaignItemTemplateGroup  = Fixture.Create<CampaignItemTemplateGroup>();
            var propertyInfo  = campaignItemTemplateGroup.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (CampaignItemTemplateGroup) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemTemplateGroup_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new CampaignItemTemplateGroup());
        }

        #endregion

        #region General Constructor : Class (CampaignItemTemplateGroup) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemTemplateGroup_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfCampaignItemTemplateGroup = Fixture.CreateMany<CampaignItemTemplateGroup>(2).ToList();
            var firstCampaignItemTemplateGroup = instancesOfCampaignItemTemplateGroup.FirstOrDefault();
            var lastCampaignItemTemplateGroup = instancesOfCampaignItemTemplateGroup.Last();

            // Act, Assert
            firstCampaignItemTemplateGroup.ShouldNotBeNull();
            lastCampaignItemTemplateGroup.ShouldNotBeNull();
            firstCampaignItemTemplateGroup.ShouldNotBeSameAs(lastCampaignItemTemplateGroup);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemTemplateGroup_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstCampaignItemTemplateGroup = new CampaignItemTemplateGroup();
            var secondCampaignItemTemplateGroup = new CampaignItemTemplateGroup();
            var thirdCampaignItemTemplateGroup = new CampaignItemTemplateGroup();
            var fourthCampaignItemTemplateGroup = new CampaignItemTemplateGroup();
            var fifthCampaignItemTemplateGroup = new CampaignItemTemplateGroup();
            var sixthCampaignItemTemplateGroup = new CampaignItemTemplateGroup();

            // Act, Assert
            firstCampaignItemTemplateGroup.ShouldNotBeNull();
            secondCampaignItemTemplateGroup.ShouldNotBeNull();
            thirdCampaignItemTemplateGroup.ShouldNotBeNull();
            fourthCampaignItemTemplateGroup.ShouldNotBeNull();
            fifthCampaignItemTemplateGroup.ShouldNotBeNull();
            sixthCampaignItemTemplateGroup.ShouldNotBeNull();
            firstCampaignItemTemplateGroup.ShouldNotBeSameAs(secondCampaignItemTemplateGroup);
            thirdCampaignItemTemplateGroup.ShouldNotBeSameAs(firstCampaignItemTemplateGroup);
            fourthCampaignItemTemplateGroup.ShouldNotBeSameAs(firstCampaignItemTemplateGroup);
            fifthCampaignItemTemplateGroup.ShouldNotBeSameAs(firstCampaignItemTemplateGroup);
            sixthCampaignItemTemplateGroup.ShouldNotBeSameAs(firstCampaignItemTemplateGroup);
            sixthCampaignItemTemplateGroup.ShouldNotBeSameAs(fourthCampaignItemTemplateGroup);
        }

        #endregion

        #region General Constructor : Class (CampaignItemTemplateGroup) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemTemplateGroup_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var campaignItemTemplateGroupId = -1;
            var campaignItemTemplateId = -1;
            var groupId = -1;
            var isDeleted = false;
            var filters = new List<CampaignItemBlastFilter>();

            // Act
            var campaignItemTemplateGroup = new CampaignItemTemplateGroup();

            // Assert
            campaignItemTemplateGroup.CampaignItemTemplateGroupID.ShouldBe(campaignItemTemplateGroupId);
            campaignItemTemplateGroup.CampaignItemTemplateID.ShouldBe(campaignItemTemplateId);
            campaignItemTemplateGroup.GroupID.ShouldBe(groupId);
            campaignItemTemplateGroup.IsDeleted.ShouldBe(isDeleted);
            campaignItemTemplateGroup.CreatedDate.ShouldBeNull();
            campaignItemTemplateGroup.CreatedUserID.ShouldBeNull();
            campaignItemTemplateGroup.UpdatedDate.ShouldBeNull();
            campaignItemTemplateGroup.UpdatedUserID.ShouldBeNull();
            campaignItemTemplateGroup.Filters.Count.ShouldBe(0);
            campaignItemTemplateGroup.Filters.ShouldBe(filters);
        }

        #endregion

        #endregion

        #endregion
    }
}
