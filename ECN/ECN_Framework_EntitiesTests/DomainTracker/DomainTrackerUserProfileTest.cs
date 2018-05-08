using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.DomainTracker;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.DomainTracker
{
    [TestFixture]
    public class DomainTrackerUserProfileTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (DomainTrackerUserProfile) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var domainTrackerUserProfile = Fixture.Create<DomainTrackerUserProfile>();
            var profileId = Fixture.Create<int>();
            var baseChannelId = Fixture.Create<int>();
            var emailAddress = Fixture.Create<string>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();
            var profileCount = Fixture.Create<int>();
            var convertedDate = Fixture.Create<DateTime?>();
            var isKnown = Fixture.Create<bool?>();

            // Act
            domainTrackerUserProfile.ProfileID = profileId;
            domainTrackerUserProfile.BaseChannelID = baseChannelId;
            domainTrackerUserProfile.EmailAddress = emailAddress;
            domainTrackerUserProfile.CreatedUserID = createdUserId;
            domainTrackerUserProfile.CreatedDate = createdDate;
            domainTrackerUserProfile.UpdatedUserID = updatedUserId;
            domainTrackerUserProfile.IsDeleted = isDeleted;
            domainTrackerUserProfile.ProfileCount = profileCount;
            domainTrackerUserProfile.ConvertedDate = convertedDate;
            domainTrackerUserProfile.IsKnown = isKnown;

            // Assert
            domainTrackerUserProfile.ProfileID.ShouldBe(profileId);
            domainTrackerUserProfile.BaseChannelID.ShouldBe(baseChannelId);
            domainTrackerUserProfile.EmailAddress.ShouldBe(emailAddress);
            domainTrackerUserProfile.CreatedUserID.ShouldBe(createdUserId);
            domainTrackerUserProfile.CreatedDate.ShouldBe(createdDate);
            domainTrackerUserProfile.UpdatedUserID.ShouldBe(updatedUserId);
            domainTrackerUserProfile.UpdatedDate.ShouldBeNull();
            domainTrackerUserProfile.IsDeleted.ShouldBe(isDeleted);
            domainTrackerUserProfile.ProfileCount.ShouldBe(profileCount);
            domainTrackerUserProfile.ConvertedDate.ShouldBe(convertedDate);
            domainTrackerUserProfile.IsKnown.ShouldBe(isKnown);
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserProfile) => Property (BaseChannelID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_BaseChannelID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var domainTrackerUserProfile = Fixture.Create<DomainTrackerUserProfile>();
            domainTrackerUserProfile.BaseChannelID = Fixture.Create<int>();
            var intType = domainTrackerUserProfile.BaseChannelID.GetType();

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

        #region General Getters/Setters : Class (DomainTrackerUserProfile) => Property (BaseChannelID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_Class_Invalid_Property_BaseChannelIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelIDNotPresent";
            var domainTrackerUserProfile  = Fixture.Create<DomainTrackerUserProfile>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerUserProfile.GetType().GetProperty(propertyNameBaseChannelID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_BaseChannelID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelID";
            var domainTrackerUserProfile  = Fixture.Create<DomainTrackerUserProfile>();
            var propertyInfo  = domainTrackerUserProfile.GetType().GetProperty(propertyNameBaseChannelID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserProfile) => Property (ConvertedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_ConvertedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameConvertedDate = "ConvertedDate";
            var domainTrackerUserProfile = Fixture.Create<DomainTrackerUserProfile>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = domainTrackerUserProfile.GetType().GetProperty(propertyNameConvertedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(domainTrackerUserProfile, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserProfile) => Property (ConvertedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_Class_Invalid_Property_ConvertedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameConvertedDate = "ConvertedDateNotPresent";
            var domainTrackerUserProfile  = Fixture.Create<DomainTrackerUserProfile>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerUserProfile.GetType().GetProperty(propertyNameConvertedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_ConvertedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameConvertedDate = "ConvertedDate";
            var domainTrackerUserProfile  = Fixture.Create<DomainTrackerUserProfile>();
            var propertyInfo  = domainTrackerUserProfile.GetType().GetProperty(propertyNameConvertedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserProfile) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var domainTrackerUserProfile = Fixture.Create<DomainTrackerUserProfile>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = domainTrackerUserProfile.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(domainTrackerUserProfile, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserProfile) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var domainTrackerUserProfile  = Fixture.Create<DomainTrackerUserProfile>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerUserProfile.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var domainTrackerUserProfile  = Fixture.Create<DomainTrackerUserProfile>();
            var propertyInfo  = domainTrackerUserProfile.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserProfile) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var domainTrackerUserProfile = Fixture.Create<DomainTrackerUserProfile>();
            var random = Fixture.Create<int>();

            // Act , Set
            domainTrackerUserProfile.CreatedUserID = random;

            // Assert
            domainTrackerUserProfile.CreatedUserID.ShouldBe(random);
            domainTrackerUserProfile.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var domainTrackerUserProfile = Fixture.Create<DomainTrackerUserProfile>();

            // Act , Set
            domainTrackerUserProfile.CreatedUserID = null;

            // Assert
            domainTrackerUserProfile.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var domainTrackerUserProfile = Fixture.Create<DomainTrackerUserProfile>();
            var propertyInfo = domainTrackerUserProfile.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(domainTrackerUserProfile, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            domainTrackerUserProfile.CreatedUserID.ShouldBeNull();
            domainTrackerUserProfile.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserProfile) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var domainTrackerUserProfile  = Fixture.Create<DomainTrackerUserProfile>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerUserProfile.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var domainTrackerUserProfile  = Fixture.Create<DomainTrackerUserProfile>();
            var propertyInfo  = domainTrackerUserProfile.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserProfile) => Property (EmailAddress) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_EmailAddress_Property_String_Type_Verify_Test()
        {
            // Arrange
            var domainTrackerUserProfile = Fixture.Create<DomainTrackerUserProfile>();
            domainTrackerUserProfile.EmailAddress = Fixture.Create<string>();
            var stringType = domainTrackerUserProfile.EmailAddress.GetType();

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

        #region General Getters/Setters : Class (DomainTrackerUserProfile) => Property (EmailAddress) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_Class_Invalid_Property_EmailAddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddressNotPresent";
            var domainTrackerUserProfile  = Fixture.Create<DomainTrackerUserProfile>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerUserProfile.GetType().GetProperty(propertyNameEmailAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_EmailAddress_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddress";
            var domainTrackerUserProfile  = Fixture.Create<DomainTrackerUserProfile>();
            var propertyInfo  = domainTrackerUserProfile.GetType().GetProperty(propertyNameEmailAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserProfile) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var domainTrackerUserProfile = Fixture.Create<DomainTrackerUserProfile>();
            var random = Fixture.Create<bool>();

            // Act , Set
            domainTrackerUserProfile.IsDeleted = random;

            // Assert
            domainTrackerUserProfile.IsDeleted.ShouldBe(random);
            domainTrackerUserProfile.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var domainTrackerUserProfile = Fixture.Create<DomainTrackerUserProfile>();

            // Act , Set
            domainTrackerUserProfile.IsDeleted = null;

            // Assert
            domainTrackerUserProfile.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var domainTrackerUserProfile = Fixture.Create<DomainTrackerUserProfile>();
            var propertyInfo = domainTrackerUserProfile.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(domainTrackerUserProfile, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            domainTrackerUserProfile.IsDeleted.ShouldBeNull();
            domainTrackerUserProfile.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserProfile) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var domainTrackerUserProfile  = Fixture.Create<DomainTrackerUserProfile>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerUserProfile.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var domainTrackerUserProfile  = Fixture.Create<DomainTrackerUserProfile>();
            var propertyInfo  = domainTrackerUserProfile.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserProfile) => Property (IsKnown) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_IsKnown_Property_Data_Without_Null_Test()
        {
            // Arrange
            var domainTrackerUserProfile = Fixture.Create<DomainTrackerUserProfile>();
            var random = Fixture.Create<bool>();

            // Act , Set
            domainTrackerUserProfile.IsKnown = random;

            // Assert
            domainTrackerUserProfile.IsKnown.ShouldBe(random);
            domainTrackerUserProfile.IsKnown.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_IsKnown_Property_Only_Null_Data_Test()
        {
            // Arrange
            var domainTrackerUserProfile = Fixture.Create<DomainTrackerUserProfile>();

            // Act , Set
            domainTrackerUserProfile.IsKnown = null;

            // Assert
            domainTrackerUserProfile.IsKnown.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_IsKnown_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsKnown = "IsKnown";
            var domainTrackerUserProfile = Fixture.Create<DomainTrackerUserProfile>();
            var propertyInfo = domainTrackerUserProfile.GetType().GetProperty(propertyNameIsKnown);

            // Act , Set
            propertyInfo.SetValue(domainTrackerUserProfile, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            domainTrackerUserProfile.IsKnown.ShouldBeNull();
            domainTrackerUserProfile.IsKnown.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserProfile) => Property (IsKnown) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_Class_Invalid_Property_IsKnownNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsKnown = "IsKnownNotPresent";
            var domainTrackerUserProfile  = Fixture.Create<DomainTrackerUserProfile>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerUserProfile.GetType().GetProperty(propertyNameIsKnown));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_IsKnown_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsKnown = "IsKnown";
            var domainTrackerUserProfile  = Fixture.Create<DomainTrackerUserProfile>();
            var propertyInfo  = domainTrackerUserProfile.GetType().GetProperty(propertyNameIsKnown);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserProfile) => Property (ProfileCount) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_ProfileCount_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var domainTrackerUserProfile = Fixture.Create<DomainTrackerUserProfile>();
            domainTrackerUserProfile.ProfileCount = Fixture.Create<int>();
            var intType = domainTrackerUserProfile.ProfileCount.GetType();

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

        #region General Getters/Setters : Class (DomainTrackerUserProfile) => Property (ProfileCount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_Class_Invalid_Property_ProfileCountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameProfileCount = "ProfileCountNotPresent";
            var domainTrackerUserProfile  = Fixture.Create<DomainTrackerUserProfile>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerUserProfile.GetType().GetProperty(propertyNameProfileCount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_ProfileCount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameProfileCount = "ProfileCount";
            var domainTrackerUserProfile  = Fixture.Create<DomainTrackerUserProfile>();
            var propertyInfo  = domainTrackerUserProfile.GetType().GetProperty(propertyNameProfileCount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserProfile) => Property (ProfileID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_ProfileID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var domainTrackerUserProfile = Fixture.Create<DomainTrackerUserProfile>();
            domainTrackerUserProfile.ProfileID = Fixture.Create<int>();
            var intType = domainTrackerUserProfile.ProfileID.GetType();

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

        #region General Getters/Setters : Class (DomainTrackerUserProfile) => Property (ProfileID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_Class_Invalid_Property_ProfileIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameProfileID = "ProfileIDNotPresent";
            var domainTrackerUserProfile  = Fixture.Create<DomainTrackerUserProfile>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerUserProfile.GetType().GetProperty(propertyNameProfileID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_ProfileID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameProfileID = "ProfileID";
            var domainTrackerUserProfile  = Fixture.Create<DomainTrackerUserProfile>();
            var propertyInfo  = domainTrackerUserProfile.GetType().GetProperty(propertyNameProfileID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserProfile) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var domainTrackerUserProfile = Fixture.Create<DomainTrackerUserProfile>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = domainTrackerUserProfile.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(domainTrackerUserProfile, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserProfile) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var domainTrackerUserProfile  = Fixture.Create<DomainTrackerUserProfile>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerUserProfile.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var domainTrackerUserProfile  = Fixture.Create<DomainTrackerUserProfile>();
            var propertyInfo  = domainTrackerUserProfile.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserProfile) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var domainTrackerUserProfile = Fixture.Create<DomainTrackerUserProfile>();
            var random = Fixture.Create<int>();

            // Act , Set
            domainTrackerUserProfile.UpdatedUserID = random;

            // Assert
            domainTrackerUserProfile.UpdatedUserID.ShouldBe(random);
            domainTrackerUserProfile.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var domainTrackerUserProfile = Fixture.Create<DomainTrackerUserProfile>();

            // Act , Set
            domainTrackerUserProfile.UpdatedUserID = null;

            // Assert
            domainTrackerUserProfile.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var domainTrackerUserProfile = Fixture.Create<DomainTrackerUserProfile>();
            var propertyInfo = domainTrackerUserProfile.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(domainTrackerUserProfile, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            domainTrackerUserProfile.UpdatedUserID.ShouldBeNull();
            domainTrackerUserProfile.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserProfile) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var domainTrackerUserProfile  = Fixture.Create<DomainTrackerUserProfile>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerUserProfile.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserProfile_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var domainTrackerUserProfile  = Fixture.Create<DomainTrackerUserProfile>();
            var propertyInfo  = domainTrackerUserProfile.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (DomainTrackerUserProfile) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DomainTrackerUserProfile_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new DomainTrackerUserProfile());
        }

        #endregion

        #region General Constructor : Class (DomainTrackerUserProfile) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DomainTrackerUserProfile_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfDomainTrackerUserProfile = Fixture.CreateMany<DomainTrackerUserProfile>(2).ToList();
            var firstDomainTrackerUserProfile = instancesOfDomainTrackerUserProfile.FirstOrDefault();
            var lastDomainTrackerUserProfile = instancesOfDomainTrackerUserProfile.Last();

            // Act, Assert
            firstDomainTrackerUserProfile.ShouldNotBeNull();
            lastDomainTrackerUserProfile.ShouldNotBeNull();
            firstDomainTrackerUserProfile.ShouldNotBeSameAs(lastDomainTrackerUserProfile);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DomainTrackerUserProfile_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstDomainTrackerUserProfile = new DomainTrackerUserProfile();
            var secondDomainTrackerUserProfile = new DomainTrackerUserProfile();
            var thirdDomainTrackerUserProfile = new DomainTrackerUserProfile();
            var fourthDomainTrackerUserProfile = new DomainTrackerUserProfile();
            var fifthDomainTrackerUserProfile = new DomainTrackerUserProfile();
            var sixthDomainTrackerUserProfile = new DomainTrackerUserProfile();

            // Act, Assert
            firstDomainTrackerUserProfile.ShouldNotBeNull();
            secondDomainTrackerUserProfile.ShouldNotBeNull();
            thirdDomainTrackerUserProfile.ShouldNotBeNull();
            fourthDomainTrackerUserProfile.ShouldNotBeNull();
            fifthDomainTrackerUserProfile.ShouldNotBeNull();
            sixthDomainTrackerUserProfile.ShouldNotBeNull();
            firstDomainTrackerUserProfile.ShouldNotBeSameAs(secondDomainTrackerUserProfile);
            thirdDomainTrackerUserProfile.ShouldNotBeSameAs(firstDomainTrackerUserProfile);
            fourthDomainTrackerUserProfile.ShouldNotBeSameAs(firstDomainTrackerUserProfile);
            fifthDomainTrackerUserProfile.ShouldNotBeSameAs(firstDomainTrackerUserProfile);
            sixthDomainTrackerUserProfile.ShouldNotBeSameAs(firstDomainTrackerUserProfile);
            sixthDomainTrackerUserProfile.ShouldNotBeSameAs(fourthDomainTrackerUserProfile);
        }

        #endregion

        #region General Constructor : Class (DomainTrackerUserProfile) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DomainTrackerUserProfile_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var profileId = -1;
            var baseChannelId = -1;
            var emailAddress = string.Empty;
            var profileCount = 0;

            // Act
            var domainTrackerUserProfile = new DomainTrackerUserProfile();

            // Assert
            domainTrackerUserProfile.ProfileID.ShouldBe(profileId);
            domainTrackerUserProfile.BaseChannelID.ShouldBe(baseChannelId);
            domainTrackerUserProfile.EmailAddress.ShouldBe(emailAddress);
            domainTrackerUserProfile.CreatedUserID.ShouldBeNull();
            domainTrackerUserProfile.CreatedDate.ShouldBeNull();
            domainTrackerUserProfile.UpdatedUserID.ShouldBeNull();
            domainTrackerUserProfile.UpdatedDate.ShouldBeNull();
            domainTrackerUserProfile.IsDeleted.ShouldBeNull();
            domainTrackerUserProfile.ProfileCount.ShouldBe(profileCount);
        }

        #endregion

        #endregion

        #endregion
    }
}