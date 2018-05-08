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
    public class CampaignItemSuppressionTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (CampaignItemSuppression) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var campaignItemSuppression = Fixture.Create<CampaignItemSuppression>();
            var campaignItemSuppressionId = Fixture.Create<int>();
            var campaignItemId = Fixture.Create<int?>();
            var groupId = Fixture.Create<int?>();
            var createdUserId = Fixture.Create<int?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();
            var customerId = Fixture.Create<int?>();
            var filters = Fixture.Create<List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter>>();

            // Act
            campaignItemSuppression.CampaignItemSuppressionID = campaignItemSuppressionId;
            campaignItemSuppression.CampaignItemID = campaignItemId;
            campaignItemSuppression.GroupID = groupId;
            campaignItemSuppression.CreatedUserID = createdUserId;
            campaignItemSuppression.UpdatedUserID = updatedUserId;
            campaignItemSuppression.IsDeleted = isDeleted;
            campaignItemSuppression.CustomerID = customerId;
            campaignItemSuppression.Filters = filters;

            // Assert
            campaignItemSuppression.CampaignItemSuppressionID.ShouldBe(campaignItemSuppressionId);
            campaignItemSuppression.CampaignItemID.ShouldBe(campaignItemId);
            campaignItemSuppression.GroupID.ShouldBe(groupId);
            campaignItemSuppression.CreatedUserID.ShouldBe(createdUserId);
            campaignItemSuppression.CreatedDate.ShouldBeNull();
            campaignItemSuppression.UpdatedUserID.ShouldBe(updatedUserId);
            campaignItemSuppression.UpdatedDate.ShouldBeNull();
            campaignItemSuppression.IsDeleted.ShouldBe(isDeleted);
            campaignItemSuppression.CustomerID.ShouldBe(customerId);
            campaignItemSuppression.Filters.ShouldBe(filters);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSuppression) => Property (CampaignItemID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_CampaignItemID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemSuppression = Fixture.Create<CampaignItemSuppression>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemSuppression.CampaignItemID = random;

            // Assert
            campaignItemSuppression.CampaignItemID.ShouldBe(random);
            campaignItemSuppression.CampaignItemID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_CampaignItemID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemSuppression = Fixture.Create<CampaignItemSuppression>();    

            // Act , Set
            campaignItemSuppression.CampaignItemID = null;

            // Assert
            campaignItemSuppression.CampaignItemID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_CampaignItemID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCampaignItemID = "CampaignItemID";
            var campaignItemSuppression = Fixture.Create<CampaignItemSuppression>();
            var propertyInfo = campaignItemSuppression.GetType().GetProperty(propertyNameCampaignItemID);

            // Act , Set
            propertyInfo.SetValue(campaignItemSuppression, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemSuppression.CampaignItemID.ShouldBeNull();
            campaignItemSuppression.CampaignItemID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSuppression) => Property (CampaignItemID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_Class_Invalid_Property_CampaignItemIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemID = "CampaignItemIDNotPresent";
            var campaignItemSuppression  = Fixture.Create<CampaignItemSuppression>();

            // Act , Assert
            Should.NotThrow(() => campaignItemSuppression.GetType().GetProperty(propertyNameCampaignItemID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_CampaignItemID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemID = "CampaignItemID";
            var campaignItemSuppression  = Fixture.Create<CampaignItemSuppression>();
            var propertyInfo  = campaignItemSuppression.GetType().GetProperty(propertyNameCampaignItemID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSuppression) => Property (CampaignItemSuppressionID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_CampaignItemSuppressionID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignItemSuppression = Fixture.Create<CampaignItemSuppression>();
            campaignItemSuppression.CampaignItemSuppressionID = Fixture.Create<int>();
            var intType = campaignItemSuppression.CampaignItemSuppressionID.GetType();

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

        #region General Getters/Setters : Class (CampaignItemSuppression) => Property (CampaignItemSuppressionID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_Class_Invalid_Property_CampaignItemSuppressionIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemSuppressionID = "CampaignItemSuppressionIDNotPresent";
            var campaignItemSuppression  = Fixture.Create<CampaignItemSuppression>();

            // Act , Assert
            Should.NotThrow(() => campaignItemSuppression.GetType().GetProperty(propertyNameCampaignItemSuppressionID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_CampaignItemSuppressionID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemSuppressionID = "CampaignItemSuppressionID";
            var campaignItemSuppression  = Fixture.Create<CampaignItemSuppression>();
            var propertyInfo  = campaignItemSuppression.GetType().GetProperty(propertyNameCampaignItemSuppressionID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSuppression) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var campaignItemSuppression = Fixture.Create<CampaignItemSuppression>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = campaignItemSuppression.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(campaignItemSuppression, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSuppression) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var campaignItemSuppression  = Fixture.Create<CampaignItemSuppression>();

            // Act , Assert
            Should.NotThrow(() => campaignItemSuppression.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var campaignItemSuppression  = Fixture.Create<CampaignItemSuppression>();
            var propertyInfo  = campaignItemSuppression.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSuppression) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemSuppression = Fixture.Create<CampaignItemSuppression>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemSuppression.CreatedUserID = random;

            // Assert
            campaignItemSuppression.CreatedUserID.ShouldBe(random);
            campaignItemSuppression.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemSuppression = Fixture.Create<CampaignItemSuppression>();    

            // Act , Set
            campaignItemSuppression.CreatedUserID = null;

            // Assert
            campaignItemSuppression.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var campaignItemSuppression = Fixture.Create<CampaignItemSuppression>();
            var propertyInfo = campaignItemSuppression.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(campaignItemSuppression, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemSuppression.CreatedUserID.ShouldBeNull();
            campaignItemSuppression.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSuppression) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var campaignItemSuppression  = Fixture.Create<CampaignItemSuppression>();

            // Act , Assert
            Should.NotThrow(() => campaignItemSuppression.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var campaignItemSuppression  = Fixture.Create<CampaignItemSuppression>();
            var propertyInfo  = campaignItemSuppression.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSuppression) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemSuppression = Fixture.Create<CampaignItemSuppression>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemSuppression.CustomerID = random;

            // Assert
            campaignItemSuppression.CustomerID.ShouldBe(random);
            campaignItemSuppression.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemSuppression = Fixture.Create<CampaignItemSuppression>();    

            // Act , Set
            campaignItemSuppression.CustomerID = null;

            // Assert
            campaignItemSuppression.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var campaignItemSuppression = Fixture.Create<CampaignItemSuppression>();
            var propertyInfo = campaignItemSuppression.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(campaignItemSuppression, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemSuppression.CustomerID.ShouldBeNull();
            campaignItemSuppression.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSuppression) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var campaignItemSuppression  = Fixture.Create<CampaignItemSuppression>();

            // Act , Assert
            Should.NotThrow(() => campaignItemSuppression.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var campaignItemSuppression  = Fixture.Create<CampaignItemSuppression>();
            var propertyInfo  = campaignItemSuppression.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSuppression) => Property (Filters) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_Class_Invalid_Property_FiltersNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFilters = "FiltersNotPresent";
            var campaignItemSuppression  = Fixture.Create<CampaignItemSuppression>();

            // Act , Assert
            Should.NotThrow(() => campaignItemSuppression.GetType().GetProperty(propertyNameFilters));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_Filters_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFilters = "Filters";
            var campaignItemSuppression  = Fixture.Create<CampaignItemSuppression>();
            var propertyInfo  = campaignItemSuppression.GetType().GetProperty(propertyNameFilters);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSuppression) => Property (GroupID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_GroupID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemSuppression = Fixture.Create<CampaignItemSuppression>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemSuppression.GroupID = random;

            // Assert
            campaignItemSuppression.GroupID.ShouldBe(random);
            campaignItemSuppression.GroupID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_GroupID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemSuppression = Fixture.Create<CampaignItemSuppression>();    

            // Act , Set
            campaignItemSuppression.GroupID = null;

            // Assert
            campaignItemSuppression.GroupID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_GroupID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameGroupID = "GroupID";
            var campaignItemSuppression = Fixture.Create<CampaignItemSuppression>();
            var propertyInfo = campaignItemSuppression.GetType().GetProperty(propertyNameGroupID);

            // Act , Set
            propertyInfo.SetValue(campaignItemSuppression, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemSuppression.GroupID.ShouldBeNull();
            campaignItemSuppression.GroupID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSuppression) => Property (GroupID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_Class_Invalid_Property_GroupIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupIDNotPresent";
            var campaignItemSuppression  = Fixture.Create<CampaignItemSuppression>();

            // Act , Assert
            Should.NotThrow(() => campaignItemSuppression.GetType().GetProperty(propertyNameGroupID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_GroupID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupID";
            var campaignItemSuppression  = Fixture.Create<CampaignItemSuppression>();
            var propertyInfo  = campaignItemSuppression.GetType().GetProperty(propertyNameGroupID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSuppression) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemSuppression = Fixture.Create<CampaignItemSuppression>();
            var random = Fixture.Create<bool>();

            // Act , Set
            campaignItemSuppression.IsDeleted = random;

            // Assert
            campaignItemSuppression.IsDeleted.ShouldBe(random);
            campaignItemSuppression.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemSuppression = Fixture.Create<CampaignItemSuppression>();    

            // Act , Set
            campaignItemSuppression.IsDeleted = null;

            // Assert
            campaignItemSuppression.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var campaignItemSuppression = Fixture.Create<CampaignItemSuppression>();
            var propertyInfo = campaignItemSuppression.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(campaignItemSuppression, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemSuppression.IsDeleted.ShouldBeNull();
            campaignItemSuppression.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSuppression) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var campaignItemSuppression  = Fixture.Create<CampaignItemSuppression>();

            // Act , Assert
            Should.NotThrow(() => campaignItemSuppression.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var campaignItemSuppression  = Fixture.Create<CampaignItemSuppression>();
            var propertyInfo  = campaignItemSuppression.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSuppression) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var campaignItemSuppression = Fixture.Create<CampaignItemSuppression>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = campaignItemSuppression.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(campaignItemSuppression, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSuppression) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var campaignItemSuppression  = Fixture.Create<CampaignItemSuppression>();

            // Act , Assert
            Should.NotThrow(() => campaignItemSuppression.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var campaignItemSuppression  = Fixture.Create<CampaignItemSuppression>();
            var propertyInfo  = campaignItemSuppression.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSuppression) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemSuppression = Fixture.Create<CampaignItemSuppression>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemSuppression.UpdatedUserID = random;

            // Assert
            campaignItemSuppression.UpdatedUserID.ShouldBe(random);
            campaignItemSuppression.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemSuppression = Fixture.Create<CampaignItemSuppression>();    

            // Act , Set
            campaignItemSuppression.UpdatedUserID = null;

            // Assert
            campaignItemSuppression.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var campaignItemSuppression = Fixture.Create<CampaignItemSuppression>();
            var propertyInfo = campaignItemSuppression.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(campaignItemSuppression, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemSuppression.UpdatedUserID.ShouldBeNull();
            campaignItemSuppression.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSuppression) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var campaignItemSuppression  = Fixture.Create<CampaignItemSuppression>();

            // Act , Assert
            Should.NotThrow(() => campaignItemSuppression.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSuppression_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var campaignItemSuppression  = Fixture.Create<CampaignItemSuppression>();
            var propertyInfo  = campaignItemSuppression.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (CampaignItemSuppression) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemSuppression_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new CampaignItemSuppression());
        }

        #endregion

        #region General Constructor : Class (CampaignItemSuppression) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemSuppression_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfCampaignItemSuppression = Fixture.CreateMany<CampaignItemSuppression>(2).ToList();
            var firstCampaignItemSuppression = instancesOfCampaignItemSuppression.FirstOrDefault();
            var lastCampaignItemSuppression = instancesOfCampaignItemSuppression.Last();

            // Act, Assert
            firstCampaignItemSuppression.ShouldNotBeNull();
            lastCampaignItemSuppression.ShouldNotBeNull();
            firstCampaignItemSuppression.ShouldNotBeSameAs(lastCampaignItemSuppression);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemSuppression_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstCampaignItemSuppression = new CampaignItemSuppression();
            var secondCampaignItemSuppression = new CampaignItemSuppression();
            var thirdCampaignItemSuppression = new CampaignItemSuppression();
            var fourthCampaignItemSuppression = new CampaignItemSuppression();
            var fifthCampaignItemSuppression = new CampaignItemSuppression();
            var sixthCampaignItemSuppression = new CampaignItemSuppression();

            // Act, Assert
            firstCampaignItemSuppression.ShouldNotBeNull();
            secondCampaignItemSuppression.ShouldNotBeNull();
            thirdCampaignItemSuppression.ShouldNotBeNull();
            fourthCampaignItemSuppression.ShouldNotBeNull();
            fifthCampaignItemSuppression.ShouldNotBeNull();
            sixthCampaignItemSuppression.ShouldNotBeNull();
            firstCampaignItemSuppression.ShouldNotBeSameAs(secondCampaignItemSuppression);
            thirdCampaignItemSuppression.ShouldNotBeSameAs(firstCampaignItemSuppression);
            fourthCampaignItemSuppression.ShouldNotBeSameAs(firstCampaignItemSuppression);
            fifthCampaignItemSuppression.ShouldNotBeSameAs(firstCampaignItemSuppression);
            sixthCampaignItemSuppression.ShouldNotBeSameAs(firstCampaignItemSuppression);
            sixthCampaignItemSuppression.ShouldNotBeSameAs(fourthCampaignItemSuppression);
        }

        #endregion

        #region General Constructor : Class (CampaignItemSuppression) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemSuppression_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var campaignItemSuppressionId = -1;

            // Act
            var campaignItemSuppression = new CampaignItemSuppression();

            // Assert
            campaignItemSuppression.CampaignItemSuppressionID.ShouldBe(campaignItemSuppressionId);
            campaignItemSuppression.CampaignItemID.ShouldBeNull();
            campaignItemSuppression.GroupID.ShouldBeNull();
            campaignItemSuppression.CreatedUserID.ShouldBeNull();
            campaignItemSuppression.CreatedDate.ShouldBeNull();
            campaignItemSuppression.UpdatedUserID.ShouldBeNull();
            campaignItemSuppression.UpdatedDate.ShouldBeNull();
            campaignItemSuppression.IsDeleted.ShouldBeNull();
            campaignItemSuppression.CustomerID.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}