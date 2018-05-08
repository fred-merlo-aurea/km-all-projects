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
    public class GlobalMasterSuppressionListTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (GlobalMasterSuppressionList) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var globalMasterSuppressionList = Fixture.Create<GlobalMasterSuppressionList>();
            var gSId = Fixture.Create<int>();
            var emailAddress = Fixture.Create<string>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();
            var totalCount = Fixture.Create<int>();

            // Act
            globalMasterSuppressionList.GSID = gSId;
            globalMasterSuppressionList.EmailAddress = emailAddress;
            globalMasterSuppressionList.CreatedUserID = createdUserId;
            globalMasterSuppressionList.CreatedDate = createdDate;
            globalMasterSuppressionList.UpdatedUserID = updatedUserId;
            globalMasterSuppressionList.IsDeleted = isDeleted;
            globalMasterSuppressionList.TotalCount = totalCount;

            // Assert
            globalMasterSuppressionList.GSID.ShouldBe(gSId);
            globalMasterSuppressionList.EmailAddress.ShouldBe(emailAddress);
            globalMasterSuppressionList.CreatedUserID.ShouldBe(createdUserId);
            globalMasterSuppressionList.CreatedDate.ShouldBe(createdDate);
            globalMasterSuppressionList.UpdatedUserID.ShouldBe(updatedUserId);
            globalMasterSuppressionList.UpdatedDate.ShouldBeNull();
            globalMasterSuppressionList.IsDeleted.ShouldBe(isDeleted);
            globalMasterSuppressionList.TotalCount.ShouldBe(totalCount);
        }

        #endregion

        #region General Getters/Setters : Class (GlobalMasterSuppressionList) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var globalMasterSuppressionList = Fixture.Create<GlobalMasterSuppressionList>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = globalMasterSuppressionList.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(globalMasterSuppressionList, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (GlobalMasterSuppressionList) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var globalMasterSuppressionList  = Fixture.Create<GlobalMasterSuppressionList>();

            // Act , Assert
            Should.NotThrow(() => globalMasterSuppressionList.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var globalMasterSuppressionList  = Fixture.Create<GlobalMasterSuppressionList>();
            var propertyInfo  = globalMasterSuppressionList.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GlobalMasterSuppressionList) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var globalMasterSuppressionList = Fixture.Create<GlobalMasterSuppressionList>();
            var random = Fixture.Create<int>();

            // Act , Set
            globalMasterSuppressionList.CreatedUserID = random;

            // Assert
            globalMasterSuppressionList.CreatedUserID.ShouldBe(random);
            globalMasterSuppressionList.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var globalMasterSuppressionList = Fixture.Create<GlobalMasterSuppressionList>();

            // Act , Set
            globalMasterSuppressionList.CreatedUserID = null;

            // Assert
            globalMasterSuppressionList.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var globalMasterSuppressionList = Fixture.Create<GlobalMasterSuppressionList>();
            var propertyInfo = globalMasterSuppressionList.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(globalMasterSuppressionList, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            globalMasterSuppressionList.CreatedUserID.ShouldBeNull();
            globalMasterSuppressionList.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (GlobalMasterSuppressionList) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var globalMasterSuppressionList  = Fixture.Create<GlobalMasterSuppressionList>();

            // Act , Assert
            Should.NotThrow(() => globalMasterSuppressionList.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var globalMasterSuppressionList  = Fixture.Create<GlobalMasterSuppressionList>();
            var propertyInfo  = globalMasterSuppressionList.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GlobalMasterSuppressionList) => Property (EmailAddress) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_EmailAddress_Property_String_Type_Verify_Test()
        {
            // Arrange
            var globalMasterSuppressionList = Fixture.Create<GlobalMasterSuppressionList>();
            globalMasterSuppressionList.EmailAddress = Fixture.Create<string>();
            var stringType = globalMasterSuppressionList.EmailAddress.GetType();

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

        #region General Getters/Setters : Class (GlobalMasterSuppressionList) => Property (EmailAddress) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_Class_Invalid_Property_EmailAddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddressNotPresent";
            var globalMasterSuppressionList  = Fixture.Create<GlobalMasterSuppressionList>();

            // Act , Assert
            Should.NotThrow(() => globalMasterSuppressionList.GetType().GetProperty(propertyNameEmailAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_EmailAddress_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddress";
            var globalMasterSuppressionList  = Fixture.Create<GlobalMasterSuppressionList>();
            var propertyInfo  = globalMasterSuppressionList.GetType().GetProperty(propertyNameEmailAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GlobalMasterSuppressionList) => Property (GSID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_GSID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var globalMasterSuppressionList = Fixture.Create<GlobalMasterSuppressionList>();
            globalMasterSuppressionList.GSID = Fixture.Create<int>();
            var intType = globalMasterSuppressionList.GSID.GetType();

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

        #region General Getters/Setters : Class (GlobalMasterSuppressionList) => Property (GSID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_Class_Invalid_Property_GSIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGSID = "GSIDNotPresent";
            var globalMasterSuppressionList  = Fixture.Create<GlobalMasterSuppressionList>();

            // Act , Assert
            Should.NotThrow(() => globalMasterSuppressionList.GetType().GetProperty(propertyNameGSID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_GSID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGSID = "GSID";
            var globalMasterSuppressionList  = Fixture.Create<GlobalMasterSuppressionList>();
            var propertyInfo  = globalMasterSuppressionList.GetType().GetProperty(propertyNameGSID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GlobalMasterSuppressionList) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var globalMasterSuppressionList = Fixture.Create<GlobalMasterSuppressionList>();
            var random = Fixture.Create<bool>();

            // Act , Set
            globalMasterSuppressionList.IsDeleted = random;

            // Assert
            globalMasterSuppressionList.IsDeleted.ShouldBe(random);
            globalMasterSuppressionList.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var globalMasterSuppressionList = Fixture.Create<GlobalMasterSuppressionList>();

            // Act , Set
            globalMasterSuppressionList.IsDeleted = null;

            // Assert
            globalMasterSuppressionList.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var globalMasterSuppressionList = Fixture.Create<GlobalMasterSuppressionList>();
            var propertyInfo = globalMasterSuppressionList.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(globalMasterSuppressionList, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            globalMasterSuppressionList.IsDeleted.ShouldBeNull();
            globalMasterSuppressionList.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (GlobalMasterSuppressionList) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var globalMasterSuppressionList  = Fixture.Create<GlobalMasterSuppressionList>();

            // Act , Assert
            Should.NotThrow(() => globalMasterSuppressionList.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var globalMasterSuppressionList  = Fixture.Create<GlobalMasterSuppressionList>();
            var propertyInfo  = globalMasterSuppressionList.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GlobalMasterSuppressionList) => Property (TotalCount) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_TotalCount_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var globalMasterSuppressionList = Fixture.Create<GlobalMasterSuppressionList>();
            globalMasterSuppressionList.TotalCount = Fixture.Create<int>();
            var intType = globalMasterSuppressionList.TotalCount.GetType();

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

        #region General Getters/Setters : Class (GlobalMasterSuppressionList) => Property (TotalCount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_Class_Invalid_Property_TotalCountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalCount = "TotalCountNotPresent";
            var globalMasterSuppressionList  = Fixture.Create<GlobalMasterSuppressionList>();

            // Act , Assert
            Should.NotThrow(() => globalMasterSuppressionList.GetType().GetProperty(propertyNameTotalCount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_TotalCount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotalCount = "TotalCount";
            var globalMasterSuppressionList  = Fixture.Create<GlobalMasterSuppressionList>();
            var propertyInfo  = globalMasterSuppressionList.GetType().GetProperty(propertyNameTotalCount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GlobalMasterSuppressionList) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var globalMasterSuppressionList = Fixture.Create<GlobalMasterSuppressionList>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = globalMasterSuppressionList.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(globalMasterSuppressionList, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (GlobalMasterSuppressionList) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var globalMasterSuppressionList  = Fixture.Create<GlobalMasterSuppressionList>();

            // Act , Assert
            Should.NotThrow(() => globalMasterSuppressionList.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var globalMasterSuppressionList  = Fixture.Create<GlobalMasterSuppressionList>();
            var propertyInfo  = globalMasterSuppressionList.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GlobalMasterSuppressionList) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var globalMasterSuppressionList = Fixture.Create<GlobalMasterSuppressionList>();
            var random = Fixture.Create<int>();

            // Act , Set
            globalMasterSuppressionList.UpdatedUserID = random;

            // Assert
            globalMasterSuppressionList.UpdatedUserID.ShouldBe(random);
            globalMasterSuppressionList.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var globalMasterSuppressionList = Fixture.Create<GlobalMasterSuppressionList>();

            // Act , Set
            globalMasterSuppressionList.UpdatedUserID = null;

            // Assert
            globalMasterSuppressionList.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var globalMasterSuppressionList = Fixture.Create<GlobalMasterSuppressionList>();
            var propertyInfo = globalMasterSuppressionList.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(globalMasterSuppressionList, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            globalMasterSuppressionList.UpdatedUserID.ShouldBeNull();
            globalMasterSuppressionList.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (GlobalMasterSuppressionList) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var globalMasterSuppressionList  = Fixture.Create<GlobalMasterSuppressionList>();

            // Act , Assert
            Should.NotThrow(() => globalMasterSuppressionList.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GlobalMasterSuppressionList_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var globalMasterSuppressionList  = Fixture.Create<GlobalMasterSuppressionList>();
            var propertyInfo  = globalMasterSuppressionList.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (GlobalMasterSuppressionList) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_GlobalMasterSuppressionList_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new GlobalMasterSuppressionList());
        }

        #endregion

        #region General Constructor : Class (GlobalMasterSuppressionList) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_GlobalMasterSuppressionList_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfGlobalMasterSuppressionList = Fixture.CreateMany<GlobalMasterSuppressionList>(2).ToList();
            var firstGlobalMasterSuppressionList = instancesOfGlobalMasterSuppressionList.FirstOrDefault();
            var lastGlobalMasterSuppressionList = instancesOfGlobalMasterSuppressionList.Last();

            // Act, Assert
            firstGlobalMasterSuppressionList.ShouldNotBeNull();
            lastGlobalMasterSuppressionList.ShouldNotBeNull();
            firstGlobalMasterSuppressionList.ShouldNotBeSameAs(lastGlobalMasterSuppressionList);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_GlobalMasterSuppressionList_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstGlobalMasterSuppressionList = new GlobalMasterSuppressionList();
            var secondGlobalMasterSuppressionList = new GlobalMasterSuppressionList();
            var thirdGlobalMasterSuppressionList = new GlobalMasterSuppressionList();
            var fourthGlobalMasterSuppressionList = new GlobalMasterSuppressionList();
            var fifthGlobalMasterSuppressionList = new GlobalMasterSuppressionList();
            var sixthGlobalMasterSuppressionList = new GlobalMasterSuppressionList();

            // Act, Assert
            firstGlobalMasterSuppressionList.ShouldNotBeNull();
            secondGlobalMasterSuppressionList.ShouldNotBeNull();
            thirdGlobalMasterSuppressionList.ShouldNotBeNull();
            fourthGlobalMasterSuppressionList.ShouldNotBeNull();
            fifthGlobalMasterSuppressionList.ShouldNotBeNull();
            sixthGlobalMasterSuppressionList.ShouldNotBeNull();
            firstGlobalMasterSuppressionList.ShouldNotBeSameAs(secondGlobalMasterSuppressionList);
            thirdGlobalMasterSuppressionList.ShouldNotBeSameAs(firstGlobalMasterSuppressionList);
            fourthGlobalMasterSuppressionList.ShouldNotBeSameAs(firstGlobalMasterSuppressionList);
            fifthGlobalMasterSuppressionList.ShouldNotBeSameAs(firstGlobalMasterSuppressionList);
            sixthGlobalMasterSuppressionList.ShouldNotBeSameAs(firstGlobalMasterSuppressionList);
            sixthGlobalMasterSuppressionList.ShouldNotBeSameAs(fourthGlobalMasterSuppressionList);
        }

        #endregion

        #region General Constructor : Class (GlobalMasterSuppressionList) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_GlobalMasterSuppressionList_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var gSId = -1;
            var emailAddress = string.Empty;
            var totalCount = 0;

            // Act
            var globalMasterSuppressionList = new GlobalMasterSuppressionList();

            // Assert
            globalMasterSuppressionList.GSID.ShouldBe(gSId);
            globalMasterSuppressionList.EmailAddress.ShouldBe(emailAddress);
            globalMasterSuppressionList.CreatedUserID.ShouldBeNull();
            globalMasterSuppressionList.CreatedDate.ShouldBeNull();
            globalMasterSuppressionList.UpdatedUserID.ShouldBeNull();
            globalMasterSuppressionList.UpdatedDate.ShouldBeNull();
            globalMasterSuppressionList.IsDeleted.ShouldBeNull();
            globalMasterSuppressionList.TotalCount.ShouldBe(totalCount);
        }

        #endregion

        #endregion

        #endregion
    }
}