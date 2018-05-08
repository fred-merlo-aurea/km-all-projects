using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.DomainTracker
{
    [TestFixture]
    public class DomainTrackerTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (DomainTracker) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var domainTracker = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();
            var domainTrackerId = Fixture.Create<int>();
            var baseChannelId = Fixture.Create<int>();
            var trackerKey = Fixture.Create<string>();
            var domain = Fixture.Create<string>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();
            var trackAnonymous = Fixture.Create<bool>();

            // Act
            domainTracker.DomainTrackerID = domainTrackerId;
            domainTracker.BaseChannelID = baseChannelId;
            domainTracker.TrackerKey = trackerKey;
            domainTracker.Domain = domain;
            domainTracker.CreatedUserID = createdUserId;
            domainTracker.CreatedDate = createdDate;
            domainTracker.UpdatedUserID = updatedUserId;
            domainTracker.IsDeleted = isDeleted;
            domainTracker.TrackAnonymous = trackAnonymous;

            // Assert
            domainTracker.DomainTrackerID.ShouldBe(domainTrackerId);
            domainTracker.BaseChannelID.ShouldBe(baseChannelId);
            domainTracker.TrackerKey.ShouldBe(trackerKey);
            domainTracker.Domain.ShouldBe(domain);
            domainTracker.CreatedUserID.ShouldBe(createdUserId);
            domainTracker.CreatedDate.ShouldBe(createdDate);
            domainTracker.UpdatedUserID.ShouldBe(updatedUserId);
            domainTracker.UpdatedDate.ShouldBeNull();
            domainTracker.IsDeleted.ShouldBe(isDeleted);
            domainTracker.TrackAnonymous.ShouldBe(trackAnonymous);
        }

        #endregion

        #region General Getters/Setters : Class (DomainTracker) => Property (BaseChannelID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_BaseChannelID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var domainTracker = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();
            domainTracker.BaseChannelID = Fixture.Create<int>();
            var intType = domainTracker.BaseChannelID.GetType();

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

        #region General Getters/Setters : Class (DomainTracker) => Property (BaseChannelID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_Class_Invalid_Property_BaseChannelIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelIDNotPresent";
            var domainTracker  = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();

            // Act , Assert
            Should.NotThrow(() => domainTracker.GetType().GetProperty(propertyNameBaseChannelID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_BaseChannelID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelID";
            var domainTracker  = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();
            var propertyInfo  = domainTracker.GetType().GetProperty(propertyNameBaseChannelID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTracker) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var domainTracker = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = domainTracker.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(domainTracker, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (DomainTracker) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var domainTracker  = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();

            // Act , Assert
            Should.NotThrow(() => domainTracker.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var domainTracker  = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();
            var propertyInfo  = domainTracker.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTracker) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var domainTracker = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();
            var random = Fixture.Create<int>();

            // Act , Set
            domainTracker.CreatedUserID = random;

            // Assert
            domainTracker.CreatedUserID.ShouldBe(random);
            domainTracker.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var domainTracker = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();

            // Act , Set
            domainTracker.CreatedUserID = null;

            // Assert
            domainTracker.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var domainTracker = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();
            var propertyInfo = domainTracker.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(domainTracker, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            domainTracker.CreatedUserID.ShouldBeNull();
            domainTracker.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DomainTracker) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var domainTracker  = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();

            // Act , Assert
            Should.NotThrow(() => domainTracker.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var domainTracker  = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();
            var propertyInfo  = domainTracker.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTracker) => Property (Domain) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_Domain_Property_String_Type_Verify_Test()
        {
            // Arrange
            var domainTracker = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();
            domainTracker.Domain = Fixture.Create<string>();
            var stringType = domainTracker.Domain.GetType();

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

        #region General Getters/Setters : Class (DomainTracker) => Property (Domain) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_Class_Invalid_Property_DomainNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDomain = "DomainNotPresent";
            var domainTracker  = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();

            // Act , Assert
            Should.NotThrow(() => domainTracker.GetType().GetProperty(propertyNameDomain));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_Domain_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDomain = "Domain";
            var domainTracker  = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();
            var propertyInfo  = domainTracker.GetType().GetProperty(propertyNameDomain);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTracker) => Property (DomainTrackerID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_DomainTrackerID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var domainTracker = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();
            domainTracker.DomainTrackerID = Fixture.Create<int>();
            var intType = domainTracker.DomainTrackerID.GetType();

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

        #region General Getters/Setters : Class (DomainTracker) => Property (DomainTrackerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_Class_Invalid_Property_DomainTrackerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDomainTrackerID = "DomainTrackerIDNotPresent";
            var domainTracker  = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();

            // Act , Assert
            Should.NotThrow(() => domainTracker.GetType().GetProperty(propertyNameDomainTrackerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_DomainTrackerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDomainTrackerID = "DomainTrackerID";
            var domainTracker  = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();
            var propertyInfo  = domainTracker.GetType().GetProperty(propertyNameDomainTrackerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTracker) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var domainTracker = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();
            var random = Fixture.Create<bool>();

            // Act , Set
            domainTracker.IsDeleted = random;

            // Assert
            domainTracker.IsDeleted.ShouldBe(random);
            domainTracker.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var domainTracker = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();

            // Act , Set
            domainTracker.IsDeleted = null;

            // Assert
            domainTracker.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var domainTracker = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();
            var propertyInfo = domainTracker.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(domainTracker, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            domainTracker.IsDeleted.ShouldBeNull();
            domainTracker.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DomainTracker) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var domainTracker  = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();

            // Act , Assert
            Should.NotThrow(() => domainTracker.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var domainTracker  = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();
            var propertyInfo  = domainTracker.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTracker) => Property (TrackAnonymous) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_TrackAnonymous_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var domainTracker = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();
            domainTracker.TrackAnonymous = Fixture.Create<bool>();
            var boolType = domainTracker.TrackAnonymous.GetType();

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

        #region General Getters/Setters : Class (DomainTracker) => Property (TrackAnonymous) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_Class_Invalid_Property_TrackAnonymousNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTrackAnonymous = "TrackAnonymousNotPresent";
            var domainTracker  = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();

            // Act , Assert
            Should.NotThrow(() => domainTracker.GetType().GetProperty(propertyNameTrackAnonymous));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_TrackAnonymous_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTrackAnonymous = "TrackAnonymous";
            var domainTracker  = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();
            var propertyInfo  = domainTracker.GetType().GetProperty(propertyNameTrackAnonymous);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTracker) => Property (TrackerKey) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_TrackerKey_Property_String_Type_Verify_Test()
        {
            // Arrange
            var domainTracker = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();
            domainTracker.TrackerKey = Fixture.Create<string>();
            var stringType = domainTracker.TrackerKey.GetType();

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

        #region General Getters/Setters : Class (DomainTracker) => Property (TrackerKey) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_Class_Invalid_Property_TrackerKeyNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTrackerKey = "TrackerKeyNotPresent";
            var domainTracker  = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();

            // Act , Assert
            Should.NotThrow(() => domainTracker.GetType().GetProperty(propertyNameTrackerKey));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_TrackerKey_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTrackerKey = "TrackerKey";
            var domainTracker  = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();
            var propertyInfo  = domainTracker.GetType().GetProperty(propertyNameTrackerKey);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTracker) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var domainTracker = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = domainTracker.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(domainTracker, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (DomainTracker) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var domainTracker  = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();

            // Act , Assert
            Should.NotThrow(() => domainTracker.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var domainTracker  = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();
            var propertyInfo  = domainTracker.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTracker) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var domainTracker = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();
            var random = Fixture.Create<int>();

            // Act , Set
            domainTracker.UpdatedUserID = random;

            // Assert
            domainTracker.UpdatedUserID.ShouldBe(random);
            domainTracker.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var domainTracker = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();

            // Act , Set
            domainTracker.UpdatedUserID = null;

            // Assert
            domainTracker.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var domainTracker = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();
            var propertyInfo = domainTracker.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(domainTracker, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            domainTracker.UpdatedUserID.ShouldBeNull();
            domainTracker.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DomainTracker) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var domainTracker  = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();

            // Act , Assert
            Should.NotThrow(() => domainTracker.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTracker_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var domainTracker  = Fixture.Create<ECN_Framework_Entities.DomainTracker.DomainTracker>();
            var propertyInfo  = domainTracker.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (DomainTracker) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DomainTracker_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new ECN_Framework_Entities.DomainTracker.DomainTracker());
        }

        #endregion

        #region General Constructor : Class (DomainTracker) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DomainTracker_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfDomainTracker = Fixture.CreateMany<ECN_Framework_Entities.DomainTracker.DomainTracker>(2).ToList();
            var firstDomainTracker = instancesOfDomainTracker.FirstOrDefault();
            var lastDomainTracker = instancesOfDomainTracker.Last();

            // Act, Assert
            firstDomainTracker.ShouldNotBeNull();
            lastDomainTracker.ShouldNotBeNull();
            firstDomainTracker.ShouldNotBeSameAs(lastDomainTracker);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DomainTracker_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstDomainTracker = new ECN_Framework_Entities.DomainTracker.DomainTracker();
            var secondDomainTracker = new ECN_Framework_Entities.DomainTracker.DomainTracker();
            var thirdDomainTracker = new ECN_Framework_Entities.DomainTracker.DomainTracker();
            var fourthDomainTracker = new ECN_Framework_Entities.DomainTracker.DomainTracker();
            var fifthDomainTracker = new ECN_Framework_Entities.DomainTracker.DomainTracker();
            var sixthDomainTracker = new ECN_Framework_Entities.DomainTracker.DomainTracker();

            // Act, Assert
            firstDomainTracker.ShouldNotBeNull();
            secondDomainTracker.ShouldNotBeNull();
            thirdDomainTracker.ShouldNotBeNull();
            fourthDomainTracker.ShouldNotBeNull();
            fifthDomainTracker.ShouldNotBeNull();
            sixthDomainTracker.ShouldNotBeNull();
            firstDomainTracker.ShouldNotBeSameAs(secondDomainTracker);
            thirdDomainTracker.ShouldNotBeSameAs(firstDomainTracker);
            fourthDomainTracker.ShouldNotBeSameAs(firstDomainTracker);
            fifthDomainTracker.ShouldNotBeSameAs(firstDomainTracker);
            sixthDomainTracker.ShouldNotBeSameAs(firstDomainTracker);
            sixthDomainTracker.ShouldNotBeSameAs(fourthDomainTracker);
        }

        #endregion

        #region General Constructor : Class (DomainTracker) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DomainTracker_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var domainTrackerId = -1;
            var baseChannelId = -1;
            var trackerKey = string.Empty;
            var domain = string.Empty;
            var trackAnonymous = false;

            // Act
            var domainTracker = new ECN_Framework_Entities.DomainTracker.DomainTracker();

            // Assert
            domainTracker.DomainTrackerID.ShouldBe(domainTrackerId);
            domainTracker.BaseChannelID.ShouldBe(baseChannelId);
            domainTracker.TrackerKey.ShouldBe(trackerKey);
            domainTracker.Domain.ShouldBe(domain);
            domainTracker.CreatedUserID.ShouldBeNull();
            domainTracker.CreatedDate.ShouldBeNull();
            domainTracker.UpdatedUserID.ShouldBeNull();
            domainTracker.UpdatedDate.ShouldBeNull();
            domainTracker.IsDeleted.ShouldBeNull();
            domainTracker.TrackAnonymous.ShouldBeFalse();
        }

        #endregion

        #endregion

        #endregion
    }
}