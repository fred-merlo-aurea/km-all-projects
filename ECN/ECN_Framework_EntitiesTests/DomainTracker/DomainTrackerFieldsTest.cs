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
    public class DomainTrackerFieldsTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (DomainTrackerFields) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var domainTrackerFields = Fixture.Create<DomainTrackerFields>();
            var domainTrackerFieldsId = Fixture.Create<int>();
            var domainTrackerId = Fixture.Create<int>();
            var fieldName = Fixture.Create<string>();
            var source = Fixture.Create<string>();
            var sourceId = Fixture.Create<string>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();

            // Act
            domainTrackerFields.DomainTrackerFieldsID = domainTrackerFieldsId;
            domainTrackerFields.DomainTrackerID = domainTrackerId;
            domainTrackerFields.FieldName = fieldName;
            domainTrackerFields.Source = source;
            domainTrackerFields.SourceID = sourceId;
            domainTrackerFields.CreatedUserID = createdUserId;
            domainTrackerFields.CreatedDate = createdDate;
            domainTrackerFields.UpdatedUserID = updatedUserId;
            domainTrackerFields.IsDeleted = isDeleted;

            // Assert
            domainTrackerFields.DomainTrackerFieldsID.ShouldBe(domainTrackerFieldsId);
            domainTrackerFields.DomainTrackerID.ShouldBe(domainTrackerId);
            domainTrackerFields.FieldName.ShouldBe(fieldName);
            domainTrackerFields.Source.ShouldBe(source);
            domainTrackerFields.SourceID.ShouldBe(sourceId);
            domainTrackerFields.CreatedUserID.ShouldBe(createdUserId);
            domainTrackerFields.CreatedDate.ShouldBe(createdDate);
            domainTrackerFields.UpdatedUserID.ShouldBe(updatedUserId);
            domainTrackerFields.UpdatedDate.ShouldBeNull();
            domainTrackerFields.IsDeleted.ShouldBe(isDeleted);
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerFields) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var domainTrackerFields = Fixture.Create<DomainTrackerFields>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = domainTrackerFields.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(domainTrackerFields, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerFields) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var domainTrackerFields  = Fixture.Create<DomainTrackerFields>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerFields.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var domainTrackerFields  = Fixture.Create<DomainTrackerFields>();
            var propertyInfo  = domainTrackerFields.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerFields) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var domainTrackerFields = Fixture.Create<DomainTrackerFields>();
            var random = Fixture.Create<int>();

            // Act , Set
            domainTrackerFields.CreatedUserID = random;

            // Assert
            domainTrackerFields.CreatedUserID.ShouldBe(random);
            domainTrackerFields.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var domainTrackerFields = Fixture.Create<DomainTrackerFields>();

            // Act , Set
            domainTrackerFields.CreatedUserID = null;

            // Assert
            domainTrackerFields.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var domainTrackerFields = Fixture.Create<DomainTrackerFields>();
            var propertyInfo = domainTrackerFields.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(domainTrackerFields, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            domainTrackerFields.CreatedUserID.ShouldBeNull();
            domainTrackerFields.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerFields) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var domainTrackerFields  = Fixture.Create<DomainTrackerFields>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerFields.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var domainTrackerFields  = Fixture.Create<DomainTrackerFields>();
            var propertyInfo  = domainTrackerFields.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerFields) => Property (DomainTrackerFieldsID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_DomainTrackerFieldsID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var domainTrackerFields = Fixture.Create<DomainTrackerFields>();
            domainTrackerFields.DomainTrackerFieldsID = Fixture.Create<int>();
            var intType = domainTrackerFields.DomainTrackerFieldsID.GetType();

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

        #region General Getters/Setters : Class (DomainTrackerFields) => Property (DomainTrackerFieldsID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_Class_Invalid_Property_DomainTrackerFieldsIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDomainTrackerFieldsID = "DomainTrackerFieldsIDNotPresent";
            var domainTrackerFields  = Fixture.Create<DomainTrackerFields>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerFields.GetType().GetProperty(propertyNameDomainTrackerFieldsID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_DomainTrackerFieldsID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDomainTrackerFieldsID = "DomainTrackerFieldsID";
            var domainTrackerFields  = Fixture.Create<DomainTrackerFields>();
            var propertyInfo  = domainTrackerFields.GetType().GetProperty(propertyNameDomainTrackerFieldsID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerFields) => Property (DomainTrackerID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_DomainTrackerID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var domainTrackerFields = Fixture.Create<DomainTrackerFields>();
            domainTrackerFields.DomainTrackerID = Fixture.Create<int>();
            var intType = domainTrackerFields.DomainTrackerID.GetType();

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

        #region General Getters/Setters : Class (DomainTrackerFields) => Property (DomainTrackerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_Class_Invalid_Property_DomainTrackerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDomainTrackerID = "DomainTrackerIDNotPresent";
            var domainTrackerFields  = Fixture.Create<DomainTrackerFields>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerFields.GetType().GetProperty(propertyNameDomainTrackerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_DomainTrackerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDomainTrackerID = "DomainTrackerID";
            var domainTrackerFields  = Fixture.Create<DomainTrackerFields>();
            var propertyInfo  = domainTrackerFields.GetType().GetProperty(propertyNameDomainTrackerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerFields) => Property (FieldName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_FieldName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var domainTrackerFields = Fixture.Create<DomainTrackerFields>();
            domainTrackerFields.FieldName = Fixture.Create<string>();
            var stringType = domainTrackerFields.FieldName.GetType();

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

        #region General Getters/Setters : Class (DomainTrackerFields) => Property (FieldName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_Class_Invalid_Property_FieldNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFieldName = "FieldNameNotPresent";
            var domainTrackerFields  = Fixture.Create<DomainTrackerFields>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerFields.GetType().GetProperty(propertyNameFieldName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_FieldName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFieldName = "FieldName";
            var domainTrackerFields  = Fixture.Create<DomainTrackerFields>();
            var propertyInfo  = domainTrackerFields.GetType().GetProperty(propertyNameFieldName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerFields) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var domainTrackerFields = Fixture.Create<DomainTrackerFields>();
            var random = Fixture.Create<bool>();

            // Act , Set
            domainTrackerFields.IsDeleted = random;

            // Assert
            domainTrackerFields.IsDeleted.ShouldBe(random);
            domainTrackerFields.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var domainTrackerFields = Fixture.Create<DomainTrackerFields>();

            // Act , Set
            domainTrackerFields.IsDeleted = null;

            // Assert
            domainTrackerFields.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var domainTrackerFields = Fixture.Create<DomainTrackerFields>();
            var propertyInfo = domainTrackerFields.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(domainTrackerFields, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            domainTrackerFields.IsDeleted.ShouldBeNull();
            domainTrackerFields.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerFields) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var domainTrackerFields  = Fixture.Create<DomainTrackerFields>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerFields.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var domainTrackerFields  = Fixture.Create<DomainTrackerFields>();
            var propertyInfo  = domainTrackerFields.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerFields) => Property (Source) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_Source_Property_String_Type_Verify_Test()
        {
            // Arrange
            var domainTrackerFields = Fixture.Create<DomainTrackerFields>();
            domainTrackerFields.Source = Fixture.Create<string>();
            var stringType = domainTrackerFields.Source.GetType();

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

        #region General Getters/Setters : Class (DomainTrackerFields) => Property (Source) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_Class_Invalid_Property_SourceNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSource = "SourceNotPresent";
            var domainTrackerFields  = Fixture.Create<DomainTrackerFields>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerFields.GetType().GetProperty(propertyNameSource));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_Source_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSource = "Source";
            var domainTrackerFields  = Fixture.Create<DomainTrackerFields>();
            var propertyInfo  = domainTrackerFields.GetType().GetProperty(propertyNameSource);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerFields) => Property (SourceID) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_SourceID_Property_String_Type_Verify_Test()
        {
            // Arrange
            var domainTrackerFields = Fixture.Create<DomainTrackerFields>();
            domainTrackerFields.SourceID = Fixture.Create<string>();
            var stringType = domainTrackerFields.SourceID.GetType();

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

        #region General Getters/Setters : Class (DomainTrackerFields) => Property (SourceID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_Class_Invalid_Property_SourceIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSourceID = "SourceIDNotPresent";
            var domainTrackerFields  = Fixture.Create<DomainTrackerFields>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerFields.GetType().GetProperty(propertyNameSourceID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_SourceID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSourceID = "SourceID";
            var domainTrackerFields  = Fixture.Create<DomainTrackerFields>();
            var propertyInfo  = domainTrackerFields.GetType().GetProperty(propertyNameSourceID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerFields) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var domainTrackerFields = Fixture.Create<DomainTrackerFields>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = domainTrackerFields.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(domainTrackerFields, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerFields) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var domainTrackerFields  = Fixture.Create<DomainTrackerFields>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerFields.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var domainTrackerFields  = Fixture.Create<DomainTrackerFields>();
            var propertyInfo  = domainTrackerFields.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerFields) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var domainTrackerFields = Fixture.Create<DomainTrackerFields>();
            var random = Fixture.Create<int>();

            // Act , Set
            domainTrackerFields.UpdatedUserID = random;

            // Assert
            domainTrackerFields.UpdatedUserID.ShouldBe(random);
            domainTrackerFields.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var domainTrackerFields = Fixture.Create<DomainTrackerFields>();

            // Act , Set
            domainTrackerFields.UpdatedUserID = null;

            // Assert
            domainTrackerFields.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var domainTrackerFields = Fixture.Create<DomainTrackerFields>();
            var propertyInfo = domainTrackerFields.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(domainTrackerFields, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            domainTrackerFields.UpdatedUserID.ShouldBeNull();
            domainTrackerFields.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerFields) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var domainTrackerFields  = Fixture.Create<DomainTrackerFields>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerFields.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerFields_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var domainTrackerFields  = Fixture.Create<DomainTrackerFields>();
            var propertyInfo  = domainTrackerFields.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (DomainTrackerFields) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DomainTrackerFields_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new DomainTrackerFields());
        }

        #endregion

        #region General Constructor : Class (DomainTrackerFields) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DomainTrackerFields_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfDomainTrackerFields = Fixture.CreateMany<DomainTrackerFields>(2).ToList();
            var firstDomainTrackerFields = instancesOfDomainTrackerFields.FirstOrDefault();
            var lastDomainTrackerFields = instancesOfDomainTrackerFields.Last();

            // Act, Assert
            firstDomainTrackerFields.ShouldNotBeNull();
            lastDomainTrackerFields.ShouldNotBeNull();
            firstDomainTrackerFields.ShouldNotBeSameAs(lastDomainTrackerFields);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DomainTrackerFields_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstDomainTrackerFields = new DomainTrackerFields();
            var secondDomainTrackerFields = new DomainTrackerFields();
            var thirdDomainTrackerFields = new DomainTrackerFields();
            var fourthDomainTrackerFields = new DomainTrackerFields();
            var fifthDomainTrackerFields = new DomainTrackerFields();
            var sixthDomainTrackerFields = new DomainTrackerFields();

            // Act, Assert
            firstDomainTrackerFields.ShouldNotBeNull();
            secondDomainTrackerFields.ShouldNotBeNull();
            thirdDomainTrackerFields.ShouldNotBeNull();
            fourthDomainTrackerFields.ShouldNotBeNull();
            fifthDomainTrackerFields.ShouldNotBeNull();
            sixthDomainTrackerFields.ShouldNotBeNull();
            firstDomainTrackerFields.ShouldNotBeSameAs(secondDomainTrackerFields);
            thirdDomainTrackerFields.ShouldNotBeSameAs(firstDomainTrackerFields);
            fourthDomainTrackerFields.ShouldNotBeSameAs(firstDomainTrackerFields);
            fifthDomainTrackerFields.ShouldNotBeSameAs(firstDomainTrackerFields);
            sixthDomainTrackerFields.ShouldNotBeSameAs(firstDomainTrackerFields);
            sixthDomainTrackerFields.ShouldNotBeSameAs(fourthDomainTrackerFields);
        }

        #endregion

        #region General Constructor : Class (DomainTrackerFields) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DomainTrackerFields_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var domainTrackerFieldsId = -1;
            var domainTrackerId = -1;
            var fieldName = string.Empty;
            var source = string.Empty;
            var sourceId = string.Empty;

            // Act
            var domainTrackerFields = new DomainTrackerFields();

            // Assert
            domainTrackerFields.DomainTrackerFieldsID.ShouldBe(domainTrackerFieldsId);
            domainTrackerFields.DomainTrackerID.ShouldBe(domainTrackerId);
            domainTrackerFields.FieldName.ShouldBe(fieldName);
            domainTrackerFields.Source.ShouldBe(source);
            domainTrackerFields.SourceID.ShouldBe(sourceId);
            domainTrackerFields.CreatedUserID.ShouldBeNull();
            domainTrackerFields.CreatedDate.ShouldBeNull();
            domainTrackerFields.UpdatedUserID.ShouldBeNull();
            domainTrackerFields.UpdatedDate.ShouldBeNull();
            domainTrackerFields.IsDeleted.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}