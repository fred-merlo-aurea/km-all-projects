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
    public class CampaignItemOptOutGroupTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (CampaignItemOptOutGroup) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var campaignItemOptOutGroup = Fixture.Create<CampaignItemOptOutGroup>();
            var campaignItemOptOutId = Fixture.Create<int>();
            var campaignItemId = Fixture.Create<int?>();
            var groupId = Fixture.Create<int?>();
            var createdUserId = Fixture.Create<int?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();
            var customerId = Fixture.Create<int?>();

            // Act
            campaignItemOptOutGroup.CampaignItemOptOutID = campaignItemOptOutId;
            campaignItemOptOutGroup.CampaignItemID = campaignItemId;
            campaignItemOptOutGroup.GroupID = groupId;
            campaignItemOptOutGroup.CreatedUserID = createdUserId;
            campaignItemOptOutGroup.UpdatedUserID = updatedUserId;
            campaignItemOptOutGroup.IsDeleted = isDeleted;
            campaignItemOptOutGroup.CustomerID = customerId;

            // Assert
            campaignItemOptOutGroup.CampaignItemOptOutID.ShouldBe(campaignItemOptOutId);
            campaignItemOptOutGroup.CampaignItemID.ShouldBe(campaignItemId);
            campaignItemOptOutGroup.GroupID.ShouldBe(groupId);
            campaignItemOptOutGroup.CreatedUserID.ShouldBe(createdUserId);
            campaignItemOptOutGroup.CreatedDate.ShouldBeNull();
            campaignItemOptOutGroup.UpdatedUserID.ShouldBe(updatedUserId);
            campaignItemOptOutGroup.UpdatedDate.ShouldBeNull();
            campaignItemOptOutGroup.IsDeleted.ShouldBe(isDeleted);
            campaignItemOptOutGroup.CustomerID.ShouldBe(customerId);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemOptOutGroup) => Property (CampaignItemID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_CampaignItemID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemOptOutGroup = Fixture.Create<CampaignItemOptOutGroup>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemOptOutGroup.CampaignItemID = random;

            // Assert
            campaignItemOptOutGroup.CampaignItemID.ShouldBe(random);
            campaignItemOptOutGroup.CampaignItemID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_CampaignItemID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemOptOutGroup = Fixture.Create<CampaignItemOptOutGroup>();    

            // Act , Set
            campaignItemOptOutGroup.CampaignItemID = null;

            // Assert
            campaignItemOptOutGroup.CampaignItemID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_CampaignItemID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCampaignItemID = "CampaignItemID";
            var campaignItemOptOutGroup = Fixture.Create<CampaignItemOptOutGroup>();
            var propertyInfo = campaignItemOptOutGroup.GetType().GetProperty(propertyNameCampaignItemID);

            // Act , Set
            propertyInfo.SetValue(campaignItemOptOutGroup, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemOptOutGroup.CampaignItemID.ShouldBeNull();
            campaignItemOptOutGroup.CampaignItemID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemOptOutGroup) => Property (CampaignItemID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_Class_Invalid_Property_CampaignItemIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemID = "CampaignItemIDNotPresent";
            var campaignItemOptOutGroup  = Fixture.Create<CampaignItemOptOutGroup>();

            // Act , Assert
            Should.NotThrow(() => campaignItemOptOutGroup.GetType().GetProperty(propertyNameCampaignItemID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_CampaignItemID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemID = "CampaignItemID";
            var campaignItemOptOutGroup  = Fixture.Create<CampaignItemOptOutGroup>();
            var propertyInfo  = campaignItemOptOutGroup.GetType().GetProperty(propertyNameCampaignItemID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemOptOutGroup) => Property (CampaignItemOptOutID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_CampaignItemOptOutID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignItemOptOutGroup = Fixture.Create<CampaignItemOptOutGroup>();
            campaignItemOptOutGroup.CampaignItemOptOutID = Fixture.Create<int>();
            var intType = campaignItemOptOutGroup.CampaignItemOptOutID.GetType();

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

        #region General Getters/Setters : Class (CampaignItemOptOutGroup) => Property (CampaignItemOptOutID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_Class_Invalid_Property_CampaignItemOptOutIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemOptOutID = "CampaignItemOptOutIDNotPresent";
            var campaignItemOptOutGroup  = Fixture.Create<CampaignItemOptOutGroup>();

            // Act , Assert
            Should.NotThrow(() => campaignItemOptOutGroup.GetType().GetProperty(propertyNameCampaignItemOptOutID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_CampaignItemOptOutID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemOptOutID = "CampaignItemOptOutID";
            var campaignItemOptOutGroup  = Fixture.Create<CampaignItemOptOutGroup>();
            var propertyInfo  = campaignItemOptOutGroup.GetType().GetProperty(propertyNameCampaignItemOptOutID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemOptOutGroup) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var campaignItemOptOutGroup = Fixture.Create<CampaignItemOptOutGroup>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = campaignItemOptOutGroup.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(campaignItemOptOutGroup, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemOptOutGroup) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var campaignItemOptOutGroup  = Fixture.Create<CampaignItemOptOutGroup>();

            // Act , Assert
            Should.NotThrow(() => campaignItemOptOutGroup.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var campaignItemOptOutGroup  = Fixture.Create<CampaignItemOptOutGroup>();
            var propertyInfo  = campaignItemOptOutGroup.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemOptOutGroup) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemOptOutGroup = Fixture.Create<CampaignItemOptOutGroup>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemOptOutGroup.CreatedUserID = random;

            // Assert
            campaignItemOptOutGroup.CreatedUserID.ShouldBe(random);
            campaignItemOptOutGroup.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemOptOutGroup = Fixture.Create<CampaignItemOptOutGroup>();    

            // Act , Set
            campaignItemOptOutGroup.CreatedUserID = null;

            // Assert
            campaignItemOptOutGroup.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var campaignItemOptOutGroup = Fixture.Create<CampaignItemOptOutGroup>();
            var propertyInfo = campaignItemOptOutGroup.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(campaignItemOptOutGroup, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemOptOutGroup.CreatedUserID.ShouldBeNull();
            campaignItemOptOutGroup.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemOptOutGroup) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var campaignItemOptOutGroup  = Fixture.Create<CampaignItemOptOutGroup>();

            // Act , Assert
            Should.NotThrow(() => campaignItemOptOutGroup.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var campaignItemOptOutGroup  = Fixture.Create<CampaignItemOptOutGroup>();
            var propertyInfo  = campaignItemOptOutGroup.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemOptOutGroup) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemOptOutGroup = Fixture.Create<CampaignItemOptOutGroup>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemOptOutGroup.CustomerID = random;

            // Assert
            campaignItemOptOutGroup.CustomerID.ShouldBe(random);
            campaignItemOptOutGroup.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemOptOutGroup = Fixture.Create<CampaignItemOptOutGroup>();    

            // Act , Set
            campaignItemOptOutGroup.CustomerID = null;

            // Assert
            campaignItemOptOutGroup.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var campaignItemOptOutGroup = Fixture.Create<CampaignItemOptOutGroup>();
            var propertyInfo = campaignItemOptOutGroup.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(campaignItemOptOutGroup, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemOptOutGroup.CustomerID.ShouldBeNull();
            campaignItemOptOutGroup.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemOptOutGroup) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var campaignItemOptOutGroup  = Fixture.Create<CampaignItemOptOutGroup>();

            // Act , Assert
            Should.NotThrow(() => campaignItemOptOutGroup.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var campaignItemOptOutGroup  = Fixture.Create<CampaignItemOptOutGroup>();
            var propertyInfo  = campaignItemOptOutGroup.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemOptOutGroup) => Property (GroupID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_GroupID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemOptOutGroup = Fixture.Create<CampaignItemOptOutGroup>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemOptOutGroup.GroupID = random;

            // Assert
            campaignItemOptOutGroup.GroupID.ShouldBe(random);
            campaignItemOptOutGroup.GroupID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_GroupID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemOptOutGroup = Fixture.Create<CampaignItemOptOutGroup>();    

            // Act , Set
            campaignItemOptOutGroup.GroupID = null;

            // Assert
            campaignItemOptOutGroup.GroupID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_GroupID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameGroupID = "GroupID";
            var campaignItemOptOutGroup = Fixture.Create<CampaignItemOptOutGroup>();
            var propertyInfo = campaignItemOptOutGroup.GetType().GetProperty(propertyNameGroupID);

            // Act , Set
            propertyInfo.SetValue(campaignItemOptOutGroup, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemOptOutGroup.GroupID.ShouldBeNull();
            campaignItemOptOutGroup.GroupID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemOptOutGroup) => Property (GroupID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_Class_Invalid_Property_GroupIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupIDNotPresent";
            var campaignItemOptOutGroup  = Fixture.Create<CampaignItemOptOutGroup>();

            // Act , Assert
            Should.NotThrow(() => campaignItemOptOutGroup.GetType().GetProperty(propertyNameGroupID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_GroupID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupID";
            var campaignItemOptOutGroup  = Fixture.Create<CampaignItemOptOutGroup>();
            var propertyInfo  = campaignItemOptOutGroup.GetType().GetProperty(propertyNameGroupID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemOptOutGroup) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemOptOutGroup = Fixture.Create<CampaignItemOptOutGroup>();
            var random = Fixture.Create<bool>();

            // Act , Set
            campaignItemOptOutGroup.IsDeleted = random;

            // Assert
            campaignItemOptOutGroup.IsDeleted.ShouldBe(random);
            campaignItemOptOutGroup.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemOptOutGroup = Fixture.Create<CampaignItemOptOutGroup>();    

            // Act , Set
            campaignItemOptOutGroup.IsDeleted = null;

            // Assert
            campaignItemOptOutGroup.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var campaignItemOptOutGroup = Fixture.Create<CampaignItemOptOutGroup>();
            var propertyInfo = campaignItemOptOutGroup.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(campaignItemOptOutGroup, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemOptOutGroup.IsDeleted.ShouldBeNull();
            campaignItemOptOutGroup.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemOptOutGroup) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var campaignItemOptOutGroup  = Fixture.Create<CampaignItemOptOutGroup>();

            // Act , Assert
            Should.NotThrow(() => campaignItemOptOutGroup.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var campaignItemOptOutGroup  = Fixture.Create<CampaignItemOptOutGroup>();
            var propertyInfo  = campaignItemOptOutGroup.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemOptOutGroup) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var campaignItemOptOutGroup = Fixture.Create<CampaignItemOptOutGroup>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = campaignItemOptOutGroup.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(campaignItemOptOutGroup, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemOptOutGroup) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var campaignItemOptOutGroup  = Fixture.Create<CampaignItemOptOutGroup>();

            // Act , Assert
            Should.NotThrow(() => campaignItemOptOutGroup.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var campaignItemOptOutGroup  = Fixture.Create<CampaignItemOptOutGroup>();
            var propertyInfo  = campaignItemOptOutGroup.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemOptOutGroup) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemOptOutGroup = Fixture.Create<CampaignItemOptOutGroup>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemOptOutGroup.UpdatedUserID = random;

            // Assert
            campaignItemOptOutGroup.UpdatedUserID.ShouldBe(random);
            campaignItemOptOutGroup.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemOptOutGroup = Fixture.Create<CampaignItemOptOutGroup>();    

            // Act , Set
            campaignItemOptOutGroup.UpdatedUserID = null;

            // Assert
            campaignItemOptOutGroup.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var campaignItemOptOutGroup = Fixture.Create<CampaignItemOptOutGroup>();
            var propertyInfo = campaignItemOptOutGroup.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(campaignItemOptOutGroup, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemOptOutGroup.UpdatedUserID.ShouldBeNull();
            campaignItemOptOutGroup.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemOptOutGroup) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var campaignItemOptOutGroup  = Fixture.Create<CampaignItemOptOutGroup>();

            // Act , Assert
            Should.NotThrow(() => campaignItemOptOutGroup.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemOptOutGroup_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var campaignItemOptOutGroup  = Fixture.Create<CampaignItemOptOutGroup>();
            var propertyInfo  = campaignItemOptOutGroup.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (CampaignItemOptOutGroup) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemOptOutGroup_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new CampaignItemOptOutGroup());
        }

        #endregion

        #region General Constructor : Class (CampaignItemOptOutGroup) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemOptOutGroup_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfCampaignItemOptOutGroup = Fixture.CreateMany<CampaignItemOptOutGroup>(2).ToList();
            var firstCampaignItemOptOutGroup = instancesOfCampaignItemOptOutGroup.FirstOrDefault();
            var lastCampaignItemOptOutGroup = instancesOfCampaignItemOptOutGroup.Last();

            // Act, Assert
            firstCampaignItemOptOutGroup.ShouldNotBeNull();
            lastCampaignItemOptOutGroup.ShouldNotBeNull();
            firstCampaignItemOptOutGroup.ShouldNotBeSameAs(lastCampaignItemOptOutGroup);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemOptOutGroup_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstCampaignItemOptOutGroup = new CampaignItemOptOutGroup();
            var secondCampaignItemOptOutGroup = new CampaignItemOptOutGroup();
            var thirdCampaignItemOptOutGroup = new CampaignItemOptOutGroup();
            var fourthCampaignItemOptOutGroup = new CampaignItemOptOutGroup();
            var fifthCampaignItemOptOutGroup = new CampaignItemOptOutGroup();
            var sixthCampaignItemOptOutGroup = new CampaignItemOptOutGroup();

            // Act, Assert
            firstCampaignItemOptOutGroup.ShouldNotBeNull();
            secondCampaignItemOptOutGroup.ShouldNotBeNull();
            thirdCampaignItemOptOutGroup.ShouldNotBeNull();
            fourthCampaignItemOptOutGroup.ShouldNotBeNull();
            fifthCampaignItemOptOutGroup.ShouldNotBeNull();
            sixthCampaignItemOptOutGroup.ShouldNotBeNull();
            firstCampaignItemOptOutGroup.ShouldNotBeSameAs(secondCampaignItemOptOutGroup);
            thirdCampaignItemOptOutGroup.ShouldNotBeSameAs(firstCampaignItemOptOutGroup);
            fourthCampaignItemOptOutGroup.ShouldNotBeSameAs(firstCampaignItemOptOutGroup);
            fifthCampaignItemOptOutGroup.ShouldNotBeSameAs(firstCampaignItemOptOutGroup);
            sixthCampaignItemOptOutGroup.ShouldNotBeSameAs(firstCampaignItemOptOutGroup);
            sixthCampaignItemOptOutGroup.ShouldNotBeSameAs(fourthCampaignItemOptOutGroup);
        }

        #endregion

        #region General Constructor : Class (CampaignItemOptOutGroup) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemOptOutGroup_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var campaignItemOptOutId = -1;

            // Act
            var campaignItemOptOutGroup = new CampaignItemOptOutGroup();

            // Assert
            campaignItemOptOutGroup.CampaignItemOptOutID.ShouldBe(campaignItemOptOutId);
            campaignItemOptOutGroup.CampaignItemID.ShouldBeNull();
            campaignItemOptOutGroup.GroupID.ShouldBeNull();
            campaignItemOptOutGroup.CreatedUserID.ShouldBeNull();
            campaignItemOptOutGroup.CreatedDate.ShouldBeNull();
            campaignItemOptOutGroup.UpdatedUserID.ShouldBeNull();
            campaignItemOptOutGroup.UpdatedDate.ShouldBeNull();
            campaignItemOptOutGroup.IsDeleted.ShouldBeNull();
            campaignItemOptOutGroup.CustomerID.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}