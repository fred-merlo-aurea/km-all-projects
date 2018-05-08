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
    public class DynamicTagTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (DynamicTag) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var dynamicTag = Fixture.Create<DynamicTag>();
            var dynamicTagId = Fixture.Create<int>();
            var tag = Fixture.Create<string>();
            var contentId = Fixture.Create<int?>();
            var customerId = Fixture.Create<int?>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var isDeleted = Fixture.Create<bool?>();
            var dynamicTagRulesList = Fixture.Create<List<DynamicTagRule>>();

            // Act
            dynamicTag.DynamicTagID = dynamicTagId;
            dynamicTag.Tag = tag;
            dynamicTag.ContentID = contentId;
            dynamicTag.CustomerID = customerId;
            dynamicTag.CreatedUserID = createdUserId;
            dynamicTag.CreatedDate = createdDate;
            dynamicTag.UpdatedUserID = updatedUserId;
            dynamicTag.UpdatedDate = updatedDate;
            dynamicTag.IsDeleted = isDeleted;
            dynamicTag.DynamicTagRulesList = dynamicTagRulesList;

            // Assert
            dynamicTag.DynamicTagID.ShouldBe(dynamicTagId);
            dynamicTag.Tag.ShouldBe(tag);
            dynamicTag.ContentID.ShouldBe(contentId);
            dynamicTag.CustomerID.ShouldBe(customerId);
            dynamicTag.CreatedUserID.ShouldBe(createdUserId);
            dynamicTag.CreatedDate.ShouldBe(createdDate);
            dynamicTag.UpdatedUserID.ShouldBe(updatedUserId);
            dynamicTag.UpdatedDate.ShouldBe(updatedDate);
            dynamicTag.IsDeleted.ShouldBe(isDeleted);
            dynamicTag.DynamicTagRulesList.ShouldBe(dynamicTagRulesList);
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTag) => Property (ContentID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_ContentID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var dynamicTag = Fixture.Create<DynamicTag>();
            var random = Fixture.Create<int>();

            // Act , Set
            dynamicTag.ContentID = random;

            // Assert
            dynamicTag.ContentID.ShouldBe(random);
            dynamicTag.ContentID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_ContentID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var dynamicTag = Fixture.Create<DynamicTag>();

            // Act , Set
            dynamicTag.ContentID = null;

            // Assert
            dynamicTag.ContentID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_ContentID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameContentID = "ContentID";
            var dynamicTag = Fixture.Create<DynamicTag>();
            var propertyInfo = dynamicTag.GetType().GetProperty(propertyNameContentID);

            // Act , Set
            propertyInfo.SetValue(dynamicTag, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            dynamicTag.ContentID.ShouldBeNull();
            dynamicTag.ContentID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTag) => Property (ContentID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_Class_Invalid_Property_ContentIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContentID = "ContentIDNotPresent";
            var dynamicTag  = Fixture.Create<DynamicTag>();

            // Act , Assert
            Should.NotThrow(() => dynamicTag.GetType().GetProperty(propertyNameContentID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_ContentID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContentID = "ContentID";
            var dynamicTag  = Fixture.Create<DynamicTag>();
            var propertyInfo  = dynamicTag.GetType().GetProperty(propertyNameContentID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTag) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var dynamicTag = Fixture.Create<DynamicTag>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = dynamicTag.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(dynamicTag, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTag) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var dynamicTag  = Fixture.Create<DynamicTag>();

            // Act , Assert
            Should.NotThrow(() => dynamicTag.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var dynamicTag  = Fixture.Create<DynamicTag>();
            var propertyInfo  = dynamicTag.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTag) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var dynamicTag = Fixture.Create<DynamicTag>();
            var random = Fixture.Create<int>();

            // Act , Set
            dynamicTag.CreatedUserID = random;

            // Assert
            dynamicTag.CreatedUserID.ShouldBe(random);
            dynamicTag.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var dynamicTag = Fixture.Create<DynamicTag>();

            // Act , Set
            dynamicTag.CreatedUserID = null;

            // Assert
            dynamicTag.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var dynamicTag = Fixture.Create<DynamicTag>();
            var propertyInfo = dynamicTag.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(dynamicTag, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            dynamicTag.CreatedUserID.ShouldBeNull();
            dynamicTag.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTag) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var dynamicTag  = Fixture.Create<DynamicTag>();

            // Act , Assert
            Should.NotThrow(() => dynamicTag.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var dynamicTag  = Fixture.Create<DynamicTag>();
            var propertyInfo  = dynamicTag.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTag) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var dynamicTag = Fixture.Create<DynamicTag>();
            var random = Fixture.Create<int>();

            // Act , Set
            dynamicTag.CustomerID = random;

            // Assert
            dynamicTag.CustomerID.ShouldBe(random);
            dynamicTag.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var dynamicTag = Fixture.Create<DynamicTag>();

            // Act , Set
            dynamicTag.CustomerID = null;

            // Assert
            dynamicTag.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var dynamicTag = Fixture.Create<DynamicTag>();
            var propertyInfo = dynamicTag.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(dynamicTag, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            dynamicTag.CustomerID.ShouldBeNull();
            dynamicTag.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTag) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var dynamicTag  = Fixture.Create<DynamicTag>();

            // Act , Assert
            Should.NotThrow(() => dynamicTag.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var dynamicTag  = Fixture.Create<DynamicTag>();
            var propertyInfo  = dynamicTag.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTag) => Property (DynamicTagID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_DynamicTagID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var dynamicTag = Fixture.Create<DynamicTag>();
            dynamicTag.DynamicTagID = Fixture.Create<int>();
            var intType = dynamicTag.DynamicTagID.GetType();

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

        #region General Getters/Setters : Class (DynamicTag) => Property (DynamicTagID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_Class_Invalid_Property_DynamicTagIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDynamicTagID = "DynamicTagIDNotPresent";
            var dynamicTag  = Fixture.Create<DynamicTag>();

            // Act , Assert
            Should.NotThrow(() => dynamicTag.GetType().GetProperty(propertyNameDynamicTagID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_DynamicTagID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDynamicTagID = "DynamicTagID";
            var dynamicTag  = Fixture.Create<DynamicTag>();
            var propertyInfo  = dynamicTag.GetType().GetProperty(propertyNameDynamicTagID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTag) => Property (DynamicTagRulesList) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_Class_Invalid_Property_DynamicTagRulesListNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDynamicTagRulesList = "DynamicTagRulesListNotPresent";
            var dynamicTag  = Fixture.Create<DynamicTag>();

            // Act , Assert
            Should.NotThrow(() => dynamicTag.GetType().GetProperty(propertyNameDynamicTagRulesList));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_DynamicTagRulesList_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDynamicTagRulesList = "DynamicTagRulesList";
            var dynamicTag  = Fixture.Create<DynamicTag>();
            var propertyInfo  = dynamicTag.GetType().GetProperty(propertyNameDynamicTagRulesList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTag) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var dynamicTag = Fixture.Create<DynamicTag>();
            var random = Fixture.Create<bool>();

            // Act , Set
            dynamicTag.IsDeleted = random;

            // Assert
            dynamicTag.IsDeleted.ShouldBe(random);
            dynamicTag.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var dynamicTag = Fixture.Create<DynamicTag>();

            // Act , Set
            dynamicTag.IsDeleted = null;

            // Assert
            dynamicTag.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var dynamicTag = Fixture.Create<DynamicTag>();
            var propertyInfo = dynamicTag.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(dynamicTag, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            dynamicTag.IsDeleted.ShouldBeNull();
            dynamicTag.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTag) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var dynamicTag  = Fixture.Create<DynamicTag>();

            // Act , Assert
            Should.NotThrow(() => dynamicTag.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var dynamicTag  = Fixture.Create<DynamicTag>();
            var propertyInfo  = dynamicTag.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTag) => Property (Tag) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_Tag_Property_String_Type_Verify_Test()
        {
            // Arrange
            var dynamicTag = Fixture.Create<DynamicTag>();
            dynamicTag.Tag = Fixture.Create<string>();
            var stringType = dynamicTag.Tag.GetType();

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

        #region General Getters/Setters : Class (DynamicTag) => Property (Tag) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_Class_Invalid_Property_TagNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTag = "TagNotPresent";
            var dynamicTag  = Fixture.Create<DynamicTag>();

            // Act , Assert
            Should.NotThrow(() => dynamicTag.GetType().GetProperty(propertyNameTag));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_Tag_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTag = "Tag";
            var dynamicTag  = Fixture.Create<DynamicTag>();
            var propertyInfo  = dynamicTag.GetType().GetProperty(propertyNameTag);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTag) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var dynamicTag = Fixture.Create<DynamicTag>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = dynamicTag.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(dynamicTag, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTag) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var dynamicTag  = Fixture.Create<DynamicTag>();

            // Act , Assert
            Should.NotThrow(() => dynamicTag.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var dynamicTag  = Fixture.Create<DynamicTag>();
            var propertyInfo  = dynamicTag.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTag) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var dynamicTag = Fixture.Create<DynamicTag>();
            var random = Fixture.Create<int>();

            // Act , Set
            dynamicTag.UpdatedUserID = random;

            // Assert
            dynamicTag.UpdatedUserID.ShouldBe(random);
            dynamicTag.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var dynamicTag = Fixture.Create<DynamicTag>();

            // Act , Set
            dynamicTag.UpdatedUserID = null;

            // Assert
            dynamicTag.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var dynamicTag = Fixture.Create<DynamicTag>();
            var propertyInfo = dynamicTag.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(dynamicTag, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            dynamicTag.UpdatedUserID.ShouldBeNull();
            dynamicTag.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DynamicTag) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var dynamicTag  = Fixture.Create<DynamicTag>();

            // Act , Assert
            Should.NotThrow(() => dynamicTag.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DynamicTag_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var dynamicTag  = Fixture.Create<DynamicTag>();
            var propertyInfo  = dynamicTag.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (DynamicTag) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DynamicTag_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new DynamicTag());
        }

        #endregion

        #region General Constructor : Class (DynamicTag) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DynamicTag_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfDynamicTag = Fixture.CreateMany<DynamicTag>(2).ToList();
            var firstDynamicTag = instancesOfDynamicTag.FirstOrDefault();
            var lastDynamicTag = instancesOfDynamicTag.Last();

            // Act, Assert
            firstDynamicTag.ShouldNotBeNull();
            lastDynamicTag.ShouldNotBeNull();
            firstDynamicTag.ShouldNotBeSameAs(lastDynamicTag);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DynamicTag_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstDynamicTag = new DynamicTag();
            var secondDynamicTag = new DynamicTag();
            var thirdDynamicTag = new DynamicTag();
            var fourthDynamicTag = new DynamicTag();
            var fifthDynamicTag = new DynamicTag();
            var sixthDynamicTag = new DynamicTag();

            // Act, Assert
            firstDynamicTag.ShouldNotBeNull();
            secondDynamicTag.ShouldNotBeNull();
            thirdDynamicTag.ShouldNotBeNull();
            fourthDynamicTag.ShouldNotBeNull();
            fifthDynamicTag.ShouldNotBeNull();
            sixthDynamicTag.ShouldNotBeNull();
            firstDynamicTag.ShouldNotBeSameAs(secondDynamicTag);
            thirdDynamicTag.ShouldNotBeSameAs(firstDynamicTag);
            fourthDynamicTag.ShouldNotBeSameAs(firstDynamicTag);
            fifthDynamicTag.ShouldNotBeSameAs(firstDynamicTag);
            sixthDynamicTag.ShouldNotBeSameAs(firstDynamicTag);
            sixthDynamicTag.ShouldNotBeSameAs(fourthDynamicTag);
        }

        #endregion

        #region General Constructor : Class (DynamicTag) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DynamicTag_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var dynamicTagId = -1;

            // Act
            var dynamicTag = new DynamicTag();

            // Assert
            dynamicTag.DynamicTagID.ShouldBe(dynamicTagId);
            dynamicTag.Tag.ShouldBeNull();
            dynamicTag.ContentID.ShouldBeNull();
            dynamicTag.CustomerID.ShouldBeNull();
            dynamicTag.DynamicTagRulesList.ShouldBeNull();
            dynamicTag.CreatedUserID.ShouldBeNull();
            dynamicTag.CreatedDate.ShouldBeNull();
            dynamicTag.UpdatedUserID.ShouldBeNull();
            dynamicTag.UpdatedDate.ShouldBeNull();
            dynamicTag.IsDeleted.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}