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
    public class ChannelNoThresholdListTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (ChannelNoThresholdList) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var channelNoThresholdList = Fixture.Create<ChannelNoThresholdList>();
            var cNTId = Fixture.Create<int>();
            var baseChannelId = Fixture.Create<int>();
            var emailAddress = Fixture.Create<string>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();
            var totalCount = Fixture.Create<int>();

            // Act
            channelNoThresholdList.CNTID = cNTId;
            channelNoThresholdList.BaseChannelID = baseChannelId;
            channelNoThresholdList.EmailAddress = emailAddress;
            channelNoThresholdList.CreatedUserID = createdUserId;
            channelNoThresholdList.CreatedDate = createdDate;
            channelNoThresholdList.UpdatedUserID = updatedUserId;
            channelNoThresholdList.IsDeleted = isDeleted;
            channelNoThresholdList.TotalCount = totalCount;

            // Assert
            channelNoThresholdList.CNTID.ShouldBe(cNTId);
            channelNoThresholdList.BaseChannelID.ShouldBe(baseChannelId);
            channelNoThresholdList.EmailAddress.ShouldBe(emailAddress);
            channelNoThresholdList.CreatedUserID.ShouldBe(createdUserId);
            channelNoThresholdList.CreatedDate.ShouldBe(createdDate);
            channelNoThresholdList.UpdatedUserID.ShouldBe(updatedUserId);
            channelNoThresholdList.UpdatedDate.ShouldBeNull();
            channelNoThresholdList.IsDeleted.ShouldBe(isDeleted);
            channelNoThresholdList.TotalCount.ShouldBe(totalCount);
        }

        #endregion

        #region General Getters/Setters : Class (ChannelNoThresholdList) => Property (BaseChannelID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_BaseChannelID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var channelNoThresholdList = Fixture.Create<ChannelNoThresholdList>();
            channelNoThresholdList.BaseChannelID = Fixture.Create<int>();
            var intType = channelNoThresholdList.BaseChannelID.GetType();

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

        #region General Getters/Setters : Class (ChannelNoThresholdList) => Property (BaseChannelID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_Class_Invalid_Property_BaseChannelIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelIDNotPresent";
            var channelNoThresholdList  = Fixture.Create<ChannelNoThresholdList>();

            // Act , Assert
            Should.NotThrow(() => channelNoThresholdList.GetType().GetProperty(propertyNameBaseChannelID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_BaseChannelID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelID";
            var channelNoThresholdList  = Fixture.Create<ChannelNoThresholdList>();
            var propertyInfo  = channelNoThresholdList.GetType().GetProperty(propertyNameBaseChannelID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChannelNoThresholdList) => Property (CNTID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_CNTID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var channelNoThresholdList = Fixture.Create<ChannelNoThresholdList>();
            channelNoThresholdList.CNTID = Fixture.Create<int>();
            var intType = channelNoThresholdList.CNTID.GetType();

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

        #region General Getters/Setters : Class (ChannelNoThresholdList) => Property (CNTID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_Class_Invalid_Property_CNTIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCNTID = "CNTIDNotPresent";
            var channelNoThresholdList  = Fixture.Create<ChannelNoThresholdList>();

            // Act , Assert
            Should.NotThrow(() => channelNoThresholdList.GetType().GetProperty(propertyNameCNTID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_CNTID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCNTID = "CNTID";
            var channelNoThresholdList  = Fixture.Create<ChannelNoThresholdList>();
            var propertyInfo  = channelNoThresholdList.GetType().GetProperty(propertyNameCNTID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChannelNoThresholdList) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var channelNoThresholdList = Fixture.Create<ChannelNoThresholdList>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = channelNoThresholdList.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(channelNoThresholdList, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (ChannelNoThresholdList) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var channelNoThresholdList  = Fixture.Create<ChannelNoThresholdList>();

            // Act , Assert
            Should.NotThrow(() => channelNoThresholdList.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var channelNoThresholdList  = Fixture.Create<ChannelNoThresholdList>();
            var propertyInfo  = channelNoThresholdList.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChannelNoThresholdList) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var channelNoThresholdList = Fixture.Create<ChannelNoThresholdList>();
            var random = Fixture.Create<int>();

            // Act , Set
            channelNoThresholdList.CreatedUserID = random;

            // Assert
            channelNoThresholdList.CreatedUserID.ShouldBe(random);
            channelNoThresholdList.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var channelNoThresholdList = Fixture.Create<ChannelNoThresholdList>();    

            // Act , Set
            channelNoThresholdList.CreatedUserID = null;

            // Assert
            channelNoThresholdList.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var channelNoThresholdList = Fixture.Create<ChannelNoThresholdList>();
            var propertyInfo = channelNoThresholdList.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(channelNoThresholdList, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            channelNoThresholdList.CreatedUserID.ShouldBeNull();
            channelNoThresholdList.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ChannelNoThresholdList) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var channelNoThresholdList  = Fixture.Create<ChannelNoThresholdList>();

            // Act , Assert
            Should.NotThrow(() => channelNoThresholdList.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var channelNoThresholdList  = Fixture.Create<ChannelNoThresholdList>();
            var propertyInfo  = channelNoThresholdList.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChannelNoThresholdList) => Property (EmailAddress) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_EmailAddress_Property_String_Type_Verify_Test()
        {
            // Arrange
            var channelNoThresholdList = Fixture.Create<ChannelNoThresholdList>();
            channelNoThresholdList.EmailAddress = Fixture.Create<string>();
            var stringType = channelNoThresholdList.EmailAddress.GetType();

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

        #region General Getters/Setters : Class (ChannelNoThresholdList) => Property (EmailAddress) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_Class_Invalid_Property_EmailAddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddressNotPresent";
            var channelNoThresholdList  = Fixture.Create<ChannelNoThresholdList>();

            // Act , Assert
            Should.NotThrow(() => channelNoThresholdList.GetType().GetProperty(propertyNameEmailAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_EmailAddress_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddress";
            var channelNoThresholdList  = Fixture.Create<ChannelNoThresholdList>();
            var propertyInfo  = channelNoThresholdList.GetType().GetProperty(propertyNameEmailAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChannelNoThresholdList) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var channelNoThresholdList = Fixture.Create<ChannelNoThresholdList>();
            var random = Fixture.Create<bool>();

            // Act , Set
            channelNoThresholdList.IsDeleted = random;

            // Assert
            channelNoThresholdList.IsDeleted.ShouldBe(random);
            channelNoThresholdList.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var channelNoThresholdList = Fixture.Create<ChannelNoThresholdList>();    

            // Act , Set
            channelNoThresholdList.IsDeleted = null;

            // Assert
            channelNoThresholdList.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var channelNoThresholdList = Fixture.Create<ChannelNoThresholdList>();
            var propertyInfo = channelNoThresholdList.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(channelNoThresholdList, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            channelNoThresholdList.IsDeleted.ShouldBeNull();
            channelNoThresholdList.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ChannelNoThresholdList) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var channelNoThresholdList  = Fixture.Create<ChannelNoThresholdList>();

            // Act , Assert
            Should.NotThrow(() => channelNoThresholdList.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var channelNoThresholdList  = Fixture.Create<ChannelNoThresholdList>();
            var propertyInfo  = channelNoThresholdList.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChannelNoThresholdList) => Property (TotalCount) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_TotalCount_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var channelNoThresholdList = Fixture.Create<ChannelNoThresholdList>();
            channelNoThresholdList.TotalCount = Fixture.Create<int>();
            var intType = channelNoThresholdList.TotalCount.GetType();

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

        #region General Getters/Setters : Class (ChannelNoThresholdList) => Property (TotalCount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_Class_Invalid_Property_TotalCountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalCount = "TotalCountNotPresent";
            var channelNoThresholdList  = Fixture.Create<ChannelNoThresholdList>();

            // Act , Assert
            Should.NotThrow(() => channelNoThresholdList.GetType().GetProperty(propertyNameTotalCount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_TotalCount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotalCount = "TotalCount";
            var channelNoThresholdList  = Fixture.Create<ChannelNoThresholdList>();
            var propertyInfo  = channelNoThresholdList.GetType().GetProperty(propertyNameTotalCount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChannelNoThresholdList) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var channelNoThresholdList = Fixture.Create<ChannelNoThresholdList>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = channelNoThresholdList.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(channelNoThresholdList, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (ChannelNoThresholdList) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var channelNoThresholdList  = Fixture.Create<ChannelNoThresholdList>();

            // Act , Assert
            Should.NotThrow(() => channelNoThresholdList.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var channelNoThresholdList  = Fixture.Create<ChannelNoThresholdList>();
            var propertyInfo  = channelNoThresholdList.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChannelNoThresholdList) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var channelNoThresholdList = Fixture.Create<ChannelNoThresholdList>();
            var random = Fixture.Create<int>();

            // Act , Set
            channelNoThresholdList.UpdatedUserID = random;

            // Assert
            channelNoThresholdList.UpdatedUserID.ShouldBe(random);
            channelNoThresholdList.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var channelNoThresholdList = Fixture.Create<ChannelNoThresholdList>();    

            // Act , Set
            channelNoThresholdList.UpdatedUserID = null;

            // Assert
            channelNoThresholdList.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var channelNoThresholdList = Fixture.Create<ChannelNoThresholdList>();
            var propertyInfo = channelNoThresholdList.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(channelNoThresholdList, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            channelNoThresholdList.UpdatedUserID.ShouldBeNull();
            channelNoThresholdList.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ChannelNoThresholdList) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var channelNoThresholdList  = Fixture.Create<ChannelNoThresholdList>();

            // Act , Assert
            Should.NotThrow(() => channelNoThresholdList.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelNoThresholdList_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var channelNoThresholdList  = Fixture.Create<ChannelNoThresholdList>();
            var propertyInfo  = channelNoThresholdList.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (ChannelNoThresholdList) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ChannelNoThresholdList_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new ChannelNoThresholdList());
        }

        #endregion

        #region General Constructor : Class (ChannelNoThresholdList) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ChannelNoThresholdList_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfChannelNoThresholdList = Fixture.CreateMany<ChannelNoThresholdList>(2).ToList();
            var firstChannelNoThresholdList = instancesOfChannelNoThresholdList.FirstOrDefault();
            var lastChannelNoThresholdList = instancesOfChannelNoThresholdList.Last();

            // Act, Assert
            firstChannelNoThresholdList.ShouldNotBeNull();
            lastChannelNoThresholdList.ShouldNotBeNull();
            firstChannelNoThresholdList.ShouldNotBeSameAs(lastChannelNoThresholdList);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ChannelNoThresholdList_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstChannelNoThresholdList = new ChannelNoThresholdList();
            var secondChannelNoThresholdList = new ChannelNoThresholdList();
            var thirdChannelNoThresholdList = new ChannelNoThresholdList();
            var fourthChannelNoThresholdList = new ChannelNoThresholdList();
            var fifthChannelNoThresholdList = new ChannelNoThresholdList();
            var sixthChannelNoThresholdList = new ChannelNoThresholdList();

            // Act, Assert
            firstChannelNoThresholdList.ShouldNotBeNull();
            secondChannelNoThresholdList.ShouldNotBeNull();
            thirdChannelNoThresholdList.ShouldNotBeNull();
            fourthChannelNoThresholdList.ShouldNotBeNull();
            fifthChannelNoThresholdList.ShouldNotBeNull();
            sixthChannelNoThresholdList.ShouldNotBeNull();
            firstChannelNoThresholdList.ShouldNotBeSameAs(secondChannelNoThresholdList);
            thirdChannelNoThresholdList.ShouldNotBeSameAs(firstChannelNoThresholdList);
            fourthChannelNoThresholdList.ShouldNotBeSameAs(firstChannelNoThresholdList);
            fifthChannelNoThresholdList.ShouldNotBeSameAs(firstChannelNoThresholdList);
            sixthChannelNoThresholdList.ShouldNotBeSameAs(firstChannelNoThresholdList);
            sixthChannelNoThresholdList.ShouldNotBeSameAs(fourthChannelNoThresholdList);
        }

        #endregion

        #region General Constructor : Class (ChannelNoThresholdList) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ChannelNoThresholdList_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var cNTId = -1;
            var baseChannelId = -1;
            var emailAddress = string.Empty;
            var totalCount = 0;

            // Act
            var channelNoThresholdList = new ChannelNoThresholdList();

            // Assert
            channelNoThresholdList.CNTID.ShouldBe(cNTId);
            channelNoThresholdList.BaseChannelID.ShouldBe(baseChannelId);
            channelNoThresholdList.EmailAddress.ShouldBe(emailAddress);
            channelNoThresholdList.CreatedUserID.ShouldBeNull();
            channelNoThresholdList.CreatedDate.ShouldBeNull();
            channelNoThresholdList.UpdatedUserID.ShouldBeNull();
            channelNoThresholdList.UpdatedDate.ShouldBeNull();
            channelNoThresholdList.IsDeleted.ShouldBeNull();
            channelNoThresholdList.TotalCount.ShouldBe(totalCount);
        }

        #endregion

        #endregion

        #endregion
    }
}