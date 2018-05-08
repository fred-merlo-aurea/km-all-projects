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
    public class MessageTypeTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (MessageType) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var messageType = Fixture.Create<MessageType>();
            var messageTypeId = Fixture.Create<int>();
            var name = Fixture.Create<string>();
            var description = Fixture.Create<string>();
            var threshold = Fixture.Create<bool>();
            var priority = Fixture.Create<bool>();
            var priorityNumber = Fixture.Create<int?>();
            var baseChannelId = Fixture.Create<int>();
            var sortOrder = Fixture.Create<int?>();
            var isActive = Fixture.Create<bool>();
            var customerId = Fixture.Create<int?>();
            var createdUserId = Fixture.Create<int?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();

            // Act
            messageType.MessageTypeID = messageTypeId;
            messageType.Name = name;
            messageType.Description = description;
            messageType.Threshold = threshold;
            messageType.Priority = priority;
            messageType.PriorityNumber = priorityNumber;
            messageType.BaseChannelID = baseChannelId;
            messageType.SortOrder = sortOrder;
            messageType.IsActive = isActive;
            messageType.CustomerID = customerId;
            messageType.CreatedUserID = createdUserId;
            messageType.UpdatedUserID = updatedUserId;
            messageType.IsDeleted = isDeleted;

            // Assert
            messageType.MessageTypeID.ShouldBe(messageTypeId);
            messageType.Name.ShouldBe(name);
            messageType.Description.ShouldBe(description);
            messageType.Threshold.ShouldBe(threshold);
            messageType.Priority.ShouldBe(priority);
            messageType.PriorityNumber.ShouldBe(priorityNumber);
            messageType.BaseChannelID.ShouldBe(baseChannelId);
            messageType.SortOrder.ShouldBe(sortOrder);
            messageType.IsActive.ShouldBe(isActive);
            messageType.CustomerID.ShouldBe(customerId);
            messageType.CreatedUserID.ShouldBe(createdUserId);
            messageType.CreatedDate.ShouldBeNull();
            messageType.UpdatedUserID.ShouldBe(updatedUserId);
            messageType.UpdatedDate.ShouldBeNull();
            messageType.IsDeleted.ShouldBe(isDeleted);
        }

        #endregion

        #region General Getters/Setters : Class (MessageType) => Property (BaseChannelID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_BaseChannelID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var messageType = Fixture.Create<MessageType>();
            messageType.BaseChannelID = Fixture.Create<int>();
            var intType = messageType.BaseChannelID.GetType();

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

        #region General Getters/Setters : Class (MessageType) => Property (BaseChannelID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_Class_Invalid_Property_BaseChannelIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelIDNotPresent";
            var messageType  = Fixture.Create<MessageType>();

            // Act , Assert
            Should.NotThrow(() => messageType.GetType().GetProperty(propertyNameBaseChannelID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_BaseChannelID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelID";
            var messageType  = Fixture.Create<MessageType>();
            var propertyInfo  = messageType.GetType().GetProperty(propertyNameBaseChannelID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MessageType) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var messageType = Fixture.Create<MessageType>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = messageType.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(messageType, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (MessageType) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var messageType  = Fixture.Create<MessageType>();

            // Act , Assert
            Should.NotThrow(() => messageType.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var messageType  = Fixture.Create<MessageType>();
            var propertyInfo  = messageType.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MessageType) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var messageType = Fixture.Create<MessageType>();
            var random = Fixture.Create<int>();

            // Act , Set
            messageType.CreatedUserID = random;

            // Assert
            messageType.CreatedUserID.ShouldBe(random);
            messageType.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var messageType = Fixture.Create<MessageType>();

            // Act , Set
            messageType.CreatedUserID = null;

            // Assert
            messageType.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var messageType = Fixture.Create<MessageType>();
            var propertyInfo = messageType.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(messageType, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            messageType.CreatedUserID.ShouldBeNull();
            messageType.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (MessageType) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var messageType  = Fixture.Create<MessageType>();

            // Act , Assert
            Should.NotThrow(() => messageType.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var messageType  = Fixture.Create<MessageType>();
            var propertyInfo  = messageType.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MessageType) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var messageType = Fixture.Create<MessageType>();
            var random = Fixture.Create<int>();

            // Act , Set
            messageType.CustomerID = random;

            // Assert
            messageType.CustomerID.ShouldBe(random);
            messageType.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var messageType = Fixture.Create<MessageType>();

            // Act , Set
            messageType.CustomerID = null;

            // Assert
            messageType.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var messageType = Fixture.Create<MessageType>();
            var propertyInfo = messageType.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(messageType, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            messageType.CustomerID.ShouldBeNull();
            messageType.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (MessageType) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var messageType  = Fixture.Create<MessageType>();

            // Act , Assert
            Should.NotThrow(() => messageType.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var messageType  = Fixture.Create<MessageType>();
            var propertyInfo  = messageType.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MessageType) => Property (Description) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_Description_Property_String_Type_Verify_Test()
        {
            // Arrange
            var messageType = Fixture.Create<MessageType>();
            messageType.Description = Fixture.Create<string>();
            var stringType = messageType.Description.GetType();

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

        #region General Getters/Setters : Class (MessageType) => Property (Description) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_Class_Invalid_Property_DescriptionNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDescription = "DescriptionNotPresent";
            var messageType  = Fixture.Create<MessageType>();

            // Act , Assert
            Should.NotThrow(() => messageType.GetType().GetProperty(propertyNameDescription));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_Description_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDescription = "Description";
            var messageType  = Fixture.Create<MessageType>();
            var propertyInfo  = messageType.GetType().GetProperty(propertyNameDescription);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MessageType) => Property (IsActive) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_IsActive_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var messageType = Fixture.Create<MessageType>();
            messageType.IsActive = Fixture.Create<bool>();
            var boolType = messageType.IsActive.GetType();

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

        #region General Getters/Setters : Class (MessageType) => Property (IsActive) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_Class_Invalid_Property_IsActiveNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsActive = "IsActiveNotPresent";
            var messageType  = Fixture.Create<MessageType>();

            // Act , Assert
            Should.NotThrow(() => messageType.GetType().GetProperty(propertyNameIsActive));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_IsActive_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsActive = "IsActive";
            var messageType  = Fixture.Create<MessageType>();
            var propertyInfo  = messageType.GetType().GetProperty(propertyNameIsActive);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MessageType) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var messageType = Fixture.Create<MessageType>();
            var random = Fixture.Create<bool>();

            // Act , Set
            messageType.IsDeleted = random;

            // Assert
            messageType.IsDeleted.ShouldBe(random);
            messageType.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var messageType = Fixture.Create<MessageType>();

            // Act , Set
            messageType.IsDeleted = null;

            // Assert
            messageType.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var messageType = Fixture.Create<MessageType>();
            var propertyInfo = messageType.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(messageType, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            messageType.IsDeleted.ShouldBeNull();
            messageType.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (MessageType) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var messageType  = Fixture.Create<MessageType>();

            // Act , Assert
            Should.NotThrow(() => messageType.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var messageType  = Fixture.Create<MessageType>();
            var propertyInfo  = messageType.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MessageType) => Property (MessageTypeID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_MessageTypeID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var messageType = Fixture.Create<MessageType>();
            messageType.MessageTypeID = Fixture.Create<int>();
            var intType = messageType.MessageTypeID.GetType();

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

        #region General Getters/Setters : Class (MessageType) => Property (MessageTypeID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_Class_Invalid_Property_MessageTypeIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMessageTypeID = "MessageTypeIDNotPresent";
            var messageType  = Fixture.Create<MessageType>();

            // Act , Assert
            Should.NotThrow(() => messageType.GetType().GetProperty(propertyNameMessageTypeID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_MessageTypeID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMessageTypeID = "MessageTypeID";
            var messageType  = Fixture.Create<MessageType>();
            var propertyInfo  = messageType.GetType().GetProperty(propertyNameMessageTypeID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MessageType) => Property (Name) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_Name_Property_String_Type_Verify_Test()
        {
            // Arrange
            var messageType = Fixture.Create<MessageType>();
            messageType.Name = Fixture.Create<string>();
            var stringType = messageType.Name.GetType();

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

        #region General Getters/Setters : Class (MessageType) => Property (Name) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_Class_Invalid_Property_NameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameName = "NameNotPresent";
            var messageType  = Fixture.Create<MessageType>();

            // Act , Assert
            Should.NotThrow(() => messageType.GetType().GetProperty(propertyNameName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_Name_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameName = "Name";
            var messageType  = Fixture.Create<MessageType>();
            var propertyInfo  = messageType.GetType().GetProperty(propertyNameName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MessageType) => Property (Priority) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_Priority_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var messageType = Fixture.Create<MessageType>();
            messageType.Priority = Fixture.Create<bool>();
            var boolType = messageType.Priority.GetType();

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

        #region General Getters/Setters : Class (MessageType) => Property (Priority) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_Class_Invalid_Property_PriorityNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePriority = "PriorityNotPresent";
            var messageType  = Fixture.Create<MessageType>();

            // Act , Assert
            Should.NotThrow(() => messageType.GetType().GetProperty(propertyNamePriority));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_Priority_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePriority = "Priority";
            var messageType  = Fixture.Create<MessageType>();
            var propertyInfo  = messageType.GetType().GetProperty(propertyNamePriority);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MessageType) => Property (PriorityNumber) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_PriorityNumber_Property_Data_Without_Null_Test()
        {
            // Arrange
            var messageType = Fixture.Create<MessageType>();
            var random = Fixture.Create<int>();

            // Act , Set
            messageType.PriorityNumber = random;

            // Assert
            messageType.PriorityNumber.ShouldBe(random);
            messageType.PriorityNumber.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_PriorityNumber_Property_Only_Null_Data_Test()
        {
            // Arrange
            var messageType = Fixture.Create<MessageType>();

            // Act , Set
            messageType.PriorityNumber = null;

            // Assert
            messageType.PriorityNumber.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_PriorityNumber_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNamePriorityNumber = "PriorityNumber";
            var messageType = Fixture.Create<MessageType>();
            var propertyInfo = messageType.GetType().GetProperty(propertyNamePriorityNumber);

            // Act , Set
            propertyInfo.SetValue(messageType, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            messageType.PriorityNumber.ShouldBeNull();
            messageType.PriorityNumber.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (MessageType) => Property (PriorityNumber) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_Class_Invalid_Property_PriorityNumberNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePriorityNumber = "PriorityNumberNotPresent";
            var messageType  = Fixture.Create<MessageType>();

            // Act , Assert
            Should.NotThrow(() => messageType.GetType().GetProperty(propertyNamePriorityNumber));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_PriorityNumber_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePriorityNumber = "PriorityNumber";
            var messageType  = Fixture.Create<MessageType>();
            var propertyInfo  = messageType.GetType().GetProperty(propertyNamePriorityNumber);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MessageType) => Property (SortOrder) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_SortOrder_Property_Data_Without_Null_Test()
        {
            // Arrange
            var messageType = Fixture.Create<MessageType>();
            var random = Fixture.Create<int>();

            // Act , Set
            messageType.SortOrder = random;

            // Assert
            messageType.SortOrder.ShouldBe(random);
            messageType.SortOrder.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_SortOrder_Property_Only_Null_Data_Test()
        {
            // Arrange
            var messageType = Fixture.Create<MessageType>();

            // Act , Set
            messageType.SortOrder = null;

            // Assert
            messageType.SortOrder.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_SortOrder_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameSortOrder = "SortOrder";
            var messageType = Fixture.Create<MessageType>();
            var propertyInfo = messageType.GetType().GetProperty(propertyNameSortOrder);

            // Act , Set
            propertyInfo.SetValue(messageType, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            messageType.SortOrder.ShouldBeNull();
            messageType.SortOrder.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (MessageType) => Property (SortOrder) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_Class_Invalid_Property_SortOrderNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSortOrder = "SortOrderNotPresent";
            var messageType  = Fixture.Create<MessageType>();

            // Act , Assert
            Should.NotThrow(() => messageType.GetType().GetProperty(propertyNameSortOrder));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_SortOrder_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSortOrder = "SortOrder";
            var messageType  = Fixture.Create<MessageType>();
            var propertyInfo  = messageType.GetType().GetProperty(propertyNameSortOrder);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MessageType) => Property (Threshold) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_Threshold_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var messageType = Fixture.Create<MessageType>();
            messageType.Threshold = Fixture.Create<bool>();
            var boolType = messageType.Threshold.GetType();

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

        #region General Getters/Setters : Class (MessageType) => Property (Threshold) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_Class_Invalid_Property_ThresholdNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameThreshold = "ThresholdNotPresent";
            var messageType  = Fixture.Create<MessageType>();

            // Act , Assert
            Should.NotThrow(() => messageType.GetType().GetProperty(propertyNameThreshold));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_Threshold_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameThreshold = "Threshold";
            var messageType  = Fixture.Create<MessageType>();
            var propertyInfo  = messageType.GetType().GetProperty(propertyNameThreshold);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MessageType) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var messageType = Fixture.Create<MessageType>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = messageType.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(messageType, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (MessageType) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var messageType  = Fixture.Create<MessageType>();

            // Act , Assert
            Should.NotThrow(() => messageType.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var messageType  = Fixture.Create<MessageType>();
            var propertyInfo  = messageType.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MessageType) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var messageType = Fixture.Create<MessageType>();
            var random = Fixture.Create<int>();

            // Act , Set
            messageType.UpdatedUserID = random;

            // Assert
            messageType.UpdatedUserID.ShouldBe(random);
            messageType.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var messageType = Fixture.Create<MessageType>();

            // Act , Set
            messageType.UpdatedUserID = null;

            // Assert
            messageType.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var messageType = Fixture.Create<MessageType>();
            var propertyInfo = messageType.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(messageType, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            messageType.UpdatedUserID.ShouldBeNull();
            messageType.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (MessageType) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var messageType  = Fixture.Create<MessageType>();

            // Act , Assert
            Should.NotThrow(() => messageType.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MessageType_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var messageType  = Fixture.Create<MessageType>();
            var propertyInfo  = messageType.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (MessageType) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_MessageType_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new MessageType());
        }

        #endregion

        #region General Constructor : Class (MessageType) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_MessageType_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfMessageType = Fixture.CreateMany<MessageType>(2).ToList();
            var firstMessageType = instancesOfMessageType.FirstOrDefault();
            var lastMessageType = instancesOfMessageType.Last();

            // Act, Assert
            firstMessageType.ShouldNotBeNull();
            lastMessageType.ShouldNotBeNull();
            firstMessageType.ShouldNotBeSameAs(lastMessageType);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_MessageType_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstMessageType = new MessageType();
            var secondMessageType = new MessageType();
            var thirdMessageType = new MessageType();
            var fourthMessageType = new MessageType();
            var fifthMessageType = new MessageType();
            var sixthMessageType = new MessageType();

            // Act, Assert
            firstMessageType.ShouldNotBeNull();
            secondMessageType.ShouldNotBeNull();
            thirdMessageType.ShouldNotBeNull();
            fourthMessageType.ShouldNotBeNull();
            fifthMessageType.ShouldNotBeNull();
            sixthMessageType.ShouldNotBeNull();
            firstMessageType.ShouldNotBeSameAs(secondMessageType);
            thirdMessageType.ShouldNotBeSameAs(firstMessageType);
            fourthMessageType.ShouldNotBeSameAs(firstMessageType);
            fifthMessageType.ShouldNotBeSameAs(firstMessageType);
            sixthMessageType.ShouldNotBeSameAs(firstMessageType);
            sixthMessageType.ShouldNotBeSameAs(fourthMessageType);
        }

        #endregion

        #region General Constructor : Class (MessageType) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_MessageType_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var messageTypeId = -1;
            var name = string.Empty;
            var description = string.Empty;
            var threshold = false;
            var priority = false;
            var baseChannelId = -1;
            var isActive = false;

            // Act
            var messageType = new MessageType();

            // Assert
            messageType.MessageTypeID.ShouldBe(messageTypeId);
            messageType.Name.ShouldBe(name);
            messageType.Description.ShouldBe(description);
            messageType.Threshold.ShouldBeFalse();
            messageType.Priority.ShouldBeFalse();
            messageType.PriorityNumber.ShouldBeNull();
            messageType.BaseChannelID.ShouldBe(baseChannelId);
            messageType.SortOrder.ShouldBeNull();
            messageType.IsActive.ShouldBeFalse();
            messageType.CustomerID.ShouldBeNull();
            messageType.CreatedUserID.ShouldBeNull();
            messageType.CreatedDate.ShouldBeNull();
            messageType.UpdatedUserID.ShouldBeNull();
            messageType.UpdatedDate.ShouldBeNull();
            messageType.IsDeleted.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}