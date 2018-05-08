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
    public class ChannelMasterSuppressionListTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (ChannelMasterSuppressionList) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var channelMasterSuppressionList = Fixture.Create<ChannelMasterSuppressionList>();
            var cMSId = Fixture.Create<int>();
            var baseChannelId = Fixture.Create<int>();
            var emailAddress = Fixture.Create<string>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();
            var totalCount = Fixture.Create<int>();

            // Act
            channelMasterSuppressionList.CMSID = cMSId;
            channelMasterSuppressionList.BaseChannelID = baseChannelId;
            channelMasterSuppressionList.EmailAddress = emailAddress;
            channelMasterSuppressionList.CreatedUserID = createdUserId;
            channelMasterSuppressionList.CreatedDate = createdDate;
            channelMasterSuppressionList.UpdatedUserID = updatedUserId;
            channelMasterSuppressionList.IsDeleted = isDeleted;
            channelMasterSuppressionList.TotalCount = totalCount;

            // Assert
            channelMasterSuppressionList.CMSID.ShouldBe(cMSId);
            channelMasterSuppressionList.BaseChannelID.ShouldBe(baseChannelId);
            channelMasterSuppressionList.EmailAddress.ShouldBe(emailAddress);
            channelMasterSuppressionList.CreatedUserID.ShouldBe(createdUserId);
            channelMasterSuppressionList.CreatedDate.ShouldBe(createdDate);
            channelMasterSuppressionList.UpdatedUserID.ShouldBe(updatedUserId);
            channelMasterSuppressionList.UpdatedDate.ShouldBeNull();
            channelMasterSuppressionList.IsDeleted.ShouldBe(isDeleted);
            channelMasterSuppressionList.TotalCount.ShouldBe(totalCount);
        }

        #endregion

        #region General Getters/Setters : Class (ChannelMasterSuppressionList) => Property (BaseChannelID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_BaseChannelID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var channelMasterSuppressionList = Fixture.Create<ChannelMasterSuppressionList>();
            channelMasterSuppressionList.BaseChannelID = Fixture.Create<int>();
            var intType = channelMasterSuppressionList.BaseChannelID.GetType();

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

        #region General Getters/Setters : Class (ChannelMasterSuppressionList) => Property (BaseChannelID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_Class_Invalid_Property_BaseChannelIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelIDNotPresent";
            var channelMasterSuppressionList  = Fixture.Create<ChannelMasterSuppressionList>();

            // Act , Assert
            Should.NotThrow(() => channelMasterSuppressionList.GetType().GetProperty(propertyNameBaseChannelID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_BaseChannelID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelID";
            var channelMasterSuppressionList  = Fixture.Create<ChannelMasterSuppressionList>();
            var propertyInfo  = channelMasterSuppressionList.GetType().GetProperty(propertyNameBaseChannelID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChannelMasterSuppressionList) => Property (CMSID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_CMSID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var channelMasterSuppressionList = Fixture.Create<ChannelMasterSuppressionList>();
            channelMasterSuppressionList.CMSID = Fixture.Create<int>();
            var intType = channelMasterSuppressionList.CMSID.GetType();

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

        #region General Getters/Setters : Class (ChannelMasterSuppressionList) => Property (CMSID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_Class_Invalid_Property_CMSIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCMSID = "CMSIDNotPresent";
            var channelMasterSuppressionList  = Fixture.Create<ChannelMasterSuppressionList>();

            // Act , Assert
            Should.NotThrow(() => channelMasterSuppressionList.GetType().GetProperty(propertyNameCMSID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_CMSID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCMSID = "CMSID";
            var channelMasterSuppressionList  = Fixture.Create<ChannelMasterSuppressionList>();
            var propertyInfo  = channelMasterSuppressionList.GetType().GetProperty(propertyNameCMSID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChannelMasterSuppressionList) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var channelMasterSuppressionList = Fixture.Create<ChannelMasterSuppressionList>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = channelMasterSuppressionList.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(channelMasterSuppressionList, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (ChannelMasterSuppressionList) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var channelMasterSuppressionList  = Fixture.Create<ChannelMasterSuppressionList>();

            // Act , Assert
            Should.NotThrow(() => channelMasterSuppressionList.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var channelMasterSuppressionList  = Fixture.Create<ChannelMasterSuppressionList>();
            var propertyInfo  = channelMasterSuppressionList.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChannelMasterSuppressionList) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var channelMasterSuppressionList = Fixture.Create<ChannelMasterSuppressionList>();
            var random = Fixture.Create<int>();

            // Act , Set
            channelMasterSuppressionList.CreatedUserID = random;

            // Assert
            channelMasterSuppressionList.CreatedUserID.ShouldBe(random);
            channelMasterSuppressionList.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var channelMasterSuppressionList = Fixture.Create<ChannelMasterSuppressionList>();    

            // Act , Set
            channelMasterSuppressionList.CreatedUserID = null;

            // Assert
            channelMasterSuppressionList.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var channelMasterSuppressionList = Fixture.Create<ChannelMasterSuppressionList>();
            var propertyInfo = channelMasterSuppressionList.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(channelMasterSuppressionList, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            channelMasterSuppressionList.CreatedUserID.ShouldBeNull();
            channelMasterSuppressionList.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ChannelMasterSuppressionList) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var channelMasterSuppressionList  = Fixture.Create<ChannelMasterSuppressionList>();

            // Act , Assert
            Should.NotThrow(() => channelMasterSuppressionList.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var channelMasterSuppressionList  = Fixture.Create<ChannelMasterSuppressionList>();
            var propertyInfo  = channelMasterSuppressionList.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChannelMasterSuppressionList) => Property (EmailAddress) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_EmailAddress_Property_String_Type_Verify_Test()
        {
            // Arrange
            var channelMasterSuppressionList = Fixture.Create<ChannelMasterSuppressionList>();
            channelMasterSuppressionList.EmailAddress = Fixture.Create<string>();
            var stringType = channelMasterSuppressionList.EmailAddress.GetType();

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

        #region General Getters/Setters : Class (ChannelMasterSuppressionList) => Property (EmailAddress) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_Class_Invalid_Property_EmailAddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddressNotPresent";
            var channelMasterSuppressionList  = Fixture.Create<ChannelMasterSuppressionList>();

            // Act , Assert
            Should.NotThrow(() => channelMasterSuppressionList.GetType().GetProperty(propertyNameEmailAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_EmailAddress_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddress";
            var channelMasterSuppressionList  = Fixture.Create<ChannelMasterSuppressionList>();
            var propertyInfo  = channelMasterSuppressionList.GetType().GetProperty(propertyNameEmailAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChannelMasterSuppressionList) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var channelMasterSuppressionList = Fixture.Create<ChannelMasterSuppressionList>();
            var random = Fixture.Create<bool>();

            // Act , Set
            channelMasterSuppressionList.IsDeleted = random;

            // Assert
            channelMasterSuppressionList.IsDeleted.ShouldBe(random);
            channelMasterSuppressionList.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var channelMasterSuppressionList = Fixture.Create<ChannelMasterSuppressionList>();    

            // Act , Set
            channelMasterSuppressionList.IsDeleted = null;

            // Assert
            channelMasterSuppressionList.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var channelMasterSuppressionList = Fixture.Create<ChannelMasterSuppressionList>();
            var propertyInfo = channelMasterSuppressionList.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(channelMasterSuppressionList, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            channelMasterSuppressionList.IsDeleted.ShouldBeNull();
            channelMasterSuppressionList.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ChannelMasterSuppressionList) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var channelMasterSuppressionList  = Fixture.Create<ChannelMasterSuppressionList>();

            // Act , Assert
            Should.NotThrow(() => channelMasterSuppressionList.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var channelMasterSuppressionList  = Fixture.Create<ChannelMasterSuppressionList>();
            var propertyInfo  = channelMasterSuppressionList.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChannelMasterSuppressionList) => Property (TotalCount) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_TotalCount_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var channelMasterSuppressionList = Fixture.Create<ChannelMasterSuppressionList>();
            channelMasterSuppressionList.TotalCount = Fixture.Create<int>();
            var intType = channelMasterSuppressionList.TotalCount.GetType();

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

        #region General Getters/Setters : Class (ChannelMasterSuppressionList) => Property (TotalCount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_Class_Invalid_Property_TotalCountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalCount = "TotalCountNotPresent";
            var channelMasterSuppressionList  = Fixture.Create<ChannelMasterSuppressionList>();

            // Act , Assert
            Should.NotThrow(() => channelMasterSuppressionList.GetType().GetProperty(propertyNameTotalCount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_TotalCount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotalCount = "TotalCount";
            var channelMasterSuppressionList  = Fixture.Create<ChannelMasterSuppressionList>();
            var propertyInfo  = channelMasterSuppressionList.GetType().GetProperty(propertyNameTotalCount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChannelMasterSuppressionList) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var channelMasterSuppressionList = Fixture.Create<ChannelMasterSuppressionList>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = channelMasterSuppressionList.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(channelMasterSuppressionList, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (ChannelMasterSuppressionList) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var channelMasterSuppressionList  = Fixture.Create<ChannelMasterSuppressionList>();

            // Act , Assert
            Should.NotThrow(() => channelMasterSuppressionList.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var channelMasterSuppressionList  = Fixture.Create<ChannelMasterSuppressionList>();
            var propertyInfo  = channelMasterSuppressionList.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChannelMasterSuppressionList) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var channelMasterSuppressionList = Fixture.Create<ChannelMasterSuppressionList>();
            var random = Fixture.Create<int>();

            // Act , Set
            channelMasterSuppressionList.UpdatedUserID = random;

            // Assert
            channelMasterSuppressionList.UpdatedUserID.ShouldBe(random);
            channelMasterSuppressionList.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var channelMasterSuppressionList = Fixture.Create<ChannelMasterSuppressionList>();    

            // Act , Set
            channelMasterSuppressionList.UpdatedUserID = null;

            // Assert
            channelMasterSuppressionList.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var channelMasterSuppressionList = Fixture.Create<ChannelMasterSuppressionList>();
            var propertyInfo = channelMasterSuppressionList.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(channelMasterSuppressionList, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            channelMasterSuppressionList.UpdatedUserID.ShouldBeNull();
            channelMasterSuppressionList.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ChannelMasterSuppressionList) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var channelMasterSuppressionList  = Fixture.Create<ChannelMasterSuppressionList>();

            // Act , Assert
            Should.NotThrow(() => channelMasterSuppressionList.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelMasterSuppressionList_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var channelMasterSuppressionList  = Fixture.Create<ChannelMasterSuppressionList>();
            var propertyInfo  = channelMasterSuppressionList.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (ChannelMasterSuppressionList) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ChannelMasterSuppressionList_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new ChannelMasterSuppressionList());
        }

        #endregion

        #region General Constructor : Class (ChannelMasterSuppressionList) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ChannelMasterSuppressionList_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfChannelMasterSuppressionList = Fixture.CreateMany<ChannelMasterSuppressionList>(2).ToList();
            var firstChannelMasterSuppressionList = instancesOfChannelMasterSuppressionList.FirstOrDefault();
            var lastChannelMasterSuppressionList = instancesOfChannelMasterSuppressionList.Last();

            // Act, Assert
            firstChannelMasterSuppressionList.ShouldNotBeNull();
            lastChannelMasterSuppressionList.ShouldNotBeNull();
            firstChannelMasterSuppressionList.ShouldNotBeSameAs(lastChannelMasterSuppressionList);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ChannelMasterSuppressionList_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstChannelMasterSuppressionList = new ChannelMasterSuppressionList();
            var secondChannelMasterSuppressionList = new ChannelMasterSuppressionList();
            var thirdChannelMasterSuppressionList = new ChannelMasterSuppressionList();
            var fourthChannelMasterSuppressionList = new ChannelMasterSuppressionList();
            var fifthChannelMasterSuppressionList = new ChannelMasterSuppressionList();
            var sixthChannelMasterSuppressionList = new ChannelMasterSuppressionList();

            // Act, Assert
            firstChannelMasterSuppressionList.ShouldNotBeNull();
            secondChannelMasterSuppressionList.ShouldNotBeNull();
            thirdChannelMasterSuppressionList.ShouldNotBeNull();
            fourthChannelMasterSuppressionList.ShouldNotBeNull();
            fifthChannelMasterSuppressionList.ShouldNotBeNull();
            sixthChannelMasterSuppressionList.ShouldNotBeNull();
            firstChannelMasterSuppressionList.ShouldNotBeSameAs(secondChannelMasterSuppressionList);
            thirdChannelMasterSuppressionList.ShouldNotBeSameAs(firstChannelMasterSuppressionList);
            fourthChannelMasterSuppressionList.ShouldNotBeSameAs(firstChannelMasterSuppressionList);
            fifthChannelMasterSuppressionList.ShouldNotBeSameAs(firstChannelMasterSuppressionList);
            sixthChannelMasterSuppressionList.ShouldNotBeSameAs(firstChannelMasterSuppressionList);
            sixthChannelMasterSuppressionList.ShouldNotBeSameAs(fourthChannelMasterSuppressionList);
        }

        #endregion

        #region General Constructor : Class (ChannelMasterSuppressionList) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ChannelMasterSuppressionList_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var cMSId = -1;
            var baseChannelId = -1;
            var emailAddress = string.Empty;

            // Act
            var channelMasterSuppressionList = new ChannelMasterSuppressionList();

            // Assert
            channelMasterSuppressionList.CMSID.ShouldBe(cMSId);
            channelMasterSuppressionList.BaseChannelID.ShouldBe(baseChannelId);
            channelMasterSuppressionList.EmailAddress.ShouldBe(emailAddress);
            channelMasterSuppressionList.CreatedUserID.ShouldBeNull();
            channelMasterSuppressionList.CreatedDate.ShouldBeNull();
            channelMasterSuppressionList.UpdatedUserID.ShouldBeNull();
            channelMasterSuppressionList.UpdatedDate.ShouldBeNull();
            channelMasterSuppressionList.IsDeleted.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}