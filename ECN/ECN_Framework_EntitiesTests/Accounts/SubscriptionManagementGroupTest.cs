using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Accounts
{
    [TestFixture]
    public class SubscriptionManagementGroupTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (SubscriptionManagementGroup) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var subscriptionManagementGroup = Fixture.Create<SubscriptionManagementGroup>();
            var subscriptionManagementGroupId = Fixture.Create<int>();
            var subscriptionManagementPageId = Fixture.Create<int>();
            var groupId = Fixture.Create<int>();
            var label = Fixture.Create<string>();
            var isDeleted = Fixture.Create<bool>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();

            // Act
            subscriptionManagementGroup.SubscriptionManagementGroupID = subscriptionManagementGroupId;
            subscriptionManagementGroup.SubscriptionManagementPageID = subscriptionManagementPageId;
            subscriptionManagementGroup.GroupID = groupId;
            subscriptionManagementGroup.Label = label;
            subscriptionManagementGroup.IsDeleted = isDeleted;
            subscriptionManagementGroup.CreatedUserID = createdUserId;
            subscriptionManagementGroup.CreatedDate = createdDate;
            subscriptionManagementGroup.UpdatedUserID = updatedUserId;
            subscriptionManagementGroup.UpdatedDate = updatedDate;

            // Assert
            subscriptionManagementGroup.SubscriptionManagementGroupID.ShouldBe(subscriptionManagementGroupId);
            subscriptionManagementGroup.SubscriptionManagementPageID.ShouldBe(subscriptionManagementPageId);
            subscriptionManagementGroup.GroupID.ShouldBe(groupId);
            subscriptionManagementGroup.Label.ShouldBe(label);
            subscriptionManagementGroup.IsDeleted.ShouldBe(isDeleted);
            subscriptionManagementGroup.CreatedUserID.ShouldBe(createdUserId);
            subscriptionManagementGroup.CreatedDate.ShouldBe(createdDate);
            subscriptionManagementGroup.UpdatedUserID.ShouldBe(updatedUserId);
            subscriptionManagementGroup.UpdatedDate.ShouldBe(updatedDate);
        }

        #endregion

        #region General Getters/Setters : Class (SubscriptionManagementGroup) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var subscriptionManagementGroup = Fixture.Create<SubscriptionManagementGroup>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = subscriptionManagementGroup.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(subscriptionManagementGroup, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (SubscriptionManagementGroup) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var subscriptionManagementGroup  = Fixture.Create<SubscriptionManagementGroup>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagementGroup.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var subscriptionManagementGroup  = Fixture.Create<SubscriptionManagementGroup>();
            var propertyInfo  = subscriptionManagementGroup.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SubscriptionManagementGroup) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var subscriptionManagementGroup = Fixture.Create<SubscriptionManagementGroup>();
            var random = Fixture.Create<int>();

            // Act , Set
            subscriptionManagementGroup.CreatedUserID = random;

            // Assert
            subscriptionManagementGroup.CreatedUserID.ShouldBe(random);
            subscriptionManagementGroup.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var subscriptionManagementGroup = Fixture.Create<SubscriptionManagementGroup>();    

            // Act , Set
            subscriptionManagementGroup.CreatedUserID = null;

            // Assert
            subscriptionManagementGroup.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var subscriptionManagementGroup = Fixture.Create<SubscriptionManagementGroup>();
            var propertyInfo = subscriptionManagementGroup.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(subscriptionManagementGroup, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            subscriptionManagementGroup.CreatedUserID.ShouldBeNull();
            subscriptionManagementGroup.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (SubscriptionManagementGroup) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var subscriptionManagementGroup  = Fixture.Create<SubscriptionManagementGroup>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagementGroup.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var subscriptionManagementGroup  = Fixture.Create<SubscriptionManagementGroup>();
            var propertyInfo  = subscriptionManagementGroup.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SubscriptionManagementGroup) => Property (GroupID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_GroupID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var subscriptionManagementGroup = Fixture.Create<SubscriptionManagementGroup>();
            subscriptionManagementGroup.GroupID = Fixture.Create<int>();
            var intType = subscriptionManagementGroup.GroupID.GetType();

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

        #region General Getters/Setters : Class (SubscriptionManagementGroup) => Property (GroupID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_Class_Invalid_Property_GroupIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupIDNotPresent";
            var subscriptionManagementGroup  = Fixture.Create<SubscriptionManagementGroup>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagementGroup.GetType().GetProperty(propertyNameGroupID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_GroupID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupID";
            var subscriptionManagementGroup  = Fixture.Create<SubscriptionManagementGroup>();
            var propertyInfo  = subscriptionManagementGroup.GetType().GetProperty(propertyNameGroupID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SubscriptionManagementGroup) => Property (IsDeleted) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_IsDeleted_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var subscriptionManagementGroup = Fixture.Create<SubscriptionManagementGroup>();
            subscriptionManagementGroup.IsDeleted = Fixture.Create<bool>();
            var boolType = subscriptionManagementGroup.IsDeleted.GetType();

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

        #region General Getters/Setters : Class (SubscriptionManagementGroup) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var subscriptionManagementGroup  = Fixture.Create<SubscriptionManagementGroup>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagementGroup.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var subscriptionManagementGroup  = Fixture.Create<SubscriptionManagementGroup>();
            var propertyInfo  = subscriptionManagementGroup.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SubscriptionManagementGroup) => Property (Label) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_Label_Property_String_Type_Verify_Test()
        {
            // Arrange
            var subscriptionManagementGroup = Fixture.Create<SubscriptionManagementGroup>();
            subscriptionManagementGroup.Label = Fixture.Create<string>();
            var stringType = subscriptionManagementGroup.Label.GetType();

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

        #region General Getters/Setters : Class (SubscriptionManagementGroup) => Property (Label) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_Class_Invalid_Property_LabelNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLabel = "LabelNotPresent";
            var subscriptionManagementGroup  = Fixture.Create<SubscriptionManagementGroup>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagementGroup.GetType().GetProperty(propertyNameLabel));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_Label_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLabel = "Label";
            var subscriptionManagementGroup  = Fixture.Create<SubscriptionManagementGroup>();
            var propertyInfo  = subscriptionManagementGroup.GetType().GetProperty(propertyNameLabel);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SubscriptionManagementGroup) => Property (SubscriptionManagementGroupID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_SubscriptionManagementGroupID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var subscriptionManagementGroup = Fixture.Create<SubscriptionManagementGroup>();
            subscriptionManagementGroup.SubscriptionManagementGroupID = Fixture.Create<int>();
            var intType = subscriptionManagementGroup.SubscriptionManagementGroupID.GetType();

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

        #region General Getters/Setters : Class (SubscriptionManagementGroup) => Property (SubscriptionManagementGroupID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_Class_Invalid_Property_SubscriptionManagementGroupIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSubscriptionManagementGroupID = "SubscriptionManagementGroupIDNotPresent";
            var subscriptionManagementGroup  = Fixture.Create<SubscriptionManagementGroup>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagementGroup.GetType().GetProperty(propertyNameSubscriptionManagementGroupID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_SubscriptionManagementGroupID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSubscriptionManagementGroupID = "SubscriptionManagementGroupID";
            var subscriptionManagementGroup  = Fixture.Create<SubscriptionManagementGroup>();
            var propertyInfo  = subscriptionManagementGroup.GetType().GetProperty(propertyNameSubscriptionManagementGroupID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SubscriptionManagementGroup) => Property (SubscriptionManagementPageID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_SubscriptionManagementPageID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var subscriptionManagementGroup = Fixture.Create<SubscriptionManagementGroup>();
            subscriptionManagementGroup.SubscriptionManagementPageID = Fixture.Create<int>();
            var intType = subscriptionManagementGroup.SubscriptionManagementPageID.GetType();

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

        #region General Getters/Setters : Class (SubscriptionManagementGroup) => Property (SubscriptionManagementPageID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_Class_Invalid_Property_SubscriptionManagementPageIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSubscriptionManagementPageID = "SubscriptionManagementPageIDNotPresent";
            var subscriptionManagementGroup  = Fixture.Create<SubscriptionManagementGroup>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagementGroup.GetType().GetProperty(propertyNameSubscriptionManagementPageID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_SubscriptionManagementPageID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSubscriptionManagementPageID = "SubscriptionManagementPageID";
            var subscriptionManagementGroup  = Fixture.Create<SubscriptionManagementGroup>();
            var propertyInfo  = subscriptionManagementGroup.GetType().GetProperty(propertyNameSubscriptionManagementPageID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SubscriptionManagementGroup) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var subscriptionManagementGroup = Fixture.Create<SubscriptionManagementGroup>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = subscriptionManagementGroup.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(subscriptionManagementGroup, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (SubscriptionManagementGroup) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var subscriptionManagementGroup  = Fixture.Create<SubscriptionManagementGroup>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagementGroup.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var subscriptionManagementGroup  = Fixture.Create<SubscriptionManagementGroup>();
            var propertyInfo  = subscriptionManagementGroup.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SubscriptionManagementGroup) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var subscriptionManagementGroup = Fixture.Create<SubscriptionManagementGroup>();
            var random = Fixture.Create<int>();

            // Act , Set
            subscriptionManagementGroup.UpdatedUserID = random;

            // Assert
            subscriptionManagementGroup.UpdatedUserID.ShouldBe(random);
            subscriptionManagementGroup.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var subscriptionManagementGroup = Fixture.Create<SubscriptionManagementGroup>();    

            // Act , Set
            subscriptionManagementGroup.UpdatedUserID = null;

            // Assert
            subscriptionManagementGroup.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var subscriptionManagementGroup = Fixture.Create<SubscriptionManagementGroup>();
            var propertyInfo = subscriptionManagementGroup.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(subscriptionManagementGroup, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            subscriptionManagementGroup.UpdatedUserID.ShouldBeNull();
            subscriptionManagementGroup.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (SubscriptionManagementGroup) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var subscriptionManagementGroup  = Fixture.Create<SubscriptionManagementGroup>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagementGroup.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementGroup_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var subscriptionManagementGroup  = Fixture.Create<SubscriptionManagementGroup>();
            var propertyInfo  = subscriptionManagementGroup.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (SubscriptionManagementGroup) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_SubscriptionManagementGroup_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new SubscriptionManagementGroup());
        }

        #endregion

        #region General Constructor : Class (SubscriptionManagementGroup) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_SubscriptionManagementGroup_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfSubscriptionManagementGroup = Fixture.CreateMany<SubscriptionManagementGroup>(2).ToList();
            var firstSubscriptionManagementGroup = instancesOfSubscriptionManagementGroup.FirstOrDefault();
            var lastSubscriptionManagementGroup = instancesOfSubscriptionManagementGroup.Last();

            // Act, Assert
            firstSubscriptionManagementGroup.ShouldNotBeNull();
            lastSubscriptionManagementGroup.ShouldNotBeNull();
            firstSubscriptionManagementGroup.ShouldNotBeSameAs(lastSubscriptionManagementGroup);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_SubscriptionManagementGroup_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstSubscriptionManagementGroup = new SubscriptionManagementGroup();
            var secondSubscriptionManagementGroup = new SubscriptionManagementGroup();
            var thirdSubscriptionManagementGroup = new SubscriptionManagementGroup();
            var fourthSubscriptionManagementGroup = new SubscriptionManagementGroup();
            var fifthSubscriptionManagementGroup = new SubscriptionManagementGroup();
            var sixthSubscriptionManagementGroup = new SubscriptionManagementGroup();

            // Act, Assert
            firstSubscriptionManagementGroup.ShouldNotBeNull();
            secondSubscriptionManagementGroup.ShouldNotBeNull();
            thirdSubscriptionManagementGroup.ShouldNotBeNull();
            fourthSubscriptionManagementGroup.ShouldNotBeNull();
            fifthSubscriptionManagementGroup.ShouldNotBeNull();
            sixthSubscriptionManagementGroup.ShouldNotBeNull();
            firstSubscriptionManagementGroup.ShouldNotBeSameAs(secondSubscriptionManagementGroup);
            thirdSubscriptionManagementGroup.ShouldNotBeSameAs(firstSubscriptionManagementGroup);
            fourthSubscriptionManagementGroup.ShouldNotBeSameAs(firstSubscriptionManagementGroup);
            fifthSubscriptionManagementGroup.ShouldNotBeSameAs(firstSubscriptionManagementGroup);
            sixthSubscriptionManagementGroup.ShouldNotBeSameAs(firstSubscriptionManagementGroup);
            sixthSubscriptionManagementGroup.ShouldNotBeSameAs(fourthSubscriptionManagementGroup);
        }

        #endregion

        #region General Constructor : Class (SubscriptionManagementGroup) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_SubscriptionManagementGroup_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var subscriptionManagementGroupId = -1;
            var subscriptionManagementPageId = -1;
            var groupId = -1;
            var label = string.Empty;
            var isDeleted = false;

            // Act
            var subscriptionManagementGroup = new SubscriptionManagementGroup();

            // Assert
            subscriptionManagementGroup.SubscriptionManagementGroupID.ShouldBe(subscriptionManagementGroupId);
            subscriptionManagementGroup.SubscriptionManagementPageID.ShouldBe(subscriptionManagementPageId);
            subscriptionManagementGroup.GroupID.ShouldBe(groupId);
            subscriptionManagementGroup.Label.ShouldBe(label);
            subscriptionManagementGroup.IsDeleted.ShouldBeFalse();
            subscriptionManagementGroup.CreatedUserID.ShouldBeNull();
            subscriptionManagementGroup.CreatedDate.ShouldBeNull();
            subscriptionManagementGroup.UpdatedUserID.ShouldBeNull();
            subscriptionManagementGroup.UpdatedDate.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}