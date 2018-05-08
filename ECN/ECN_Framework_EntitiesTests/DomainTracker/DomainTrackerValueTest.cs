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
    public class DomainTrackerValueTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (DomainTrackerValue) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var domainTrackerValue = Fixture.Create<DomainTrackerValue>();
            var domainTrackerValueId = Fixture.Create<int>();
            var domainTrackerFieldsId = Fixture.Create<int>();
            var domainTrackerActivityId = Fixture.Create<int>();
            var value = Fixture.Create<string>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();

            // Act
            domainTrackerValue.DomainTrackerValueID = domainTrackerValueId;
            domainTrackerValue.DomainTrackerFieldsID = domainTrackerFieldsId;
            domainTrackerValue.DomainTrackerActivityID = domainTrackerActivityId;
            domainTrackerValue.Value = value;
            domainTrackerValue.CreatedUserID = createdUserId;
            domainTrackerValue.CreatedDate = createdDate;
            domainTrackerValue.UpdatedUserID = updatedUserId;
            domainTrackerValue.IsDeleted = isDeleted;

            // Assert
            domainTrackerValue.DomainTrackerValueID.ShouldBe(domainTrackerValueId);
            domainTrackerValue.DomainTrackerFieldsID.ShouldBe(domainTrackerFieldsId);
            domainTrackerValue.DomainTrackerActivityID.ShouldBe(domainTrackerActivityId);
            domainTrackerValue.Value.ShouldBe(value);
            domainTrackerValue.CreatedUserID.ShouldBe(createdUserId);
            domainTrackerValue.CreatedDate.ShouldBe(createdDate);
            domainTrackerValue.UpdatedUserID.ShouldBe(updatedUserId);
            domainTrackerValue.UpdatedDate.ShouldBeNull();
            domainTrackerValue.IsDeleted.ShouldBe(isDeleted);
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerValue) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var domainTrackerValue = Fixture.Create<DomainTrackerValue>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = domainTrackerValue.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(domainTrackerValue, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerValue) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var domainTrackerValue  = Fixture.Create<DomainTrackerValue>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerValue.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var domainTrackerValue  = Fixture.Create<DomainTrackerValue>();
            var propertyInfo  = domainTrackerValue.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerValue) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var domainTrackerValue = Fixture.Create<DomainTrackerValue>();
            var random = Fixture.Create<int>();

            // Act , Set
            domainTrackerValue.CreatedUserID = random;

            // Assert
            domainTrackerValue.CreatedUserID.ShouldBe(random);
            domainTrackerValue.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var domainTrackerValue = Fixture.Create<DomainTrackerValue>();

            // Act , Set
            domainTrackerValue.CreatedUserID = null;

            // Assert
            domainTrackerValue.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var domainTrackerValue = Fixture.Create<DomainTrackerValue>();
            var propertyInfo = domainTrackerValue.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(domainTrackerValue, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            domainTrackerValue.CreatedUserID.ShouldBeNull();
            domainTrackerValue.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerValue) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var domainTrackerValue  = Fixture.Create<DomainTrackerValue>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerValue.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var domainTrackerValue  = Fixture.Create<DomainTrackerValue>();
            var propertyInfo  = domainTrackerValue.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerValue) => Property (DomainTrackerActivityID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_DomainTrackerActivityID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var domainTrackerValue = Fixture.Create<DomainTrackerValue>();
            domainTrackerValue.DomainTrackerActivityID = Fixture.Create<int>();
            var intType = domainTrackerValue.DomainTrackerActivityID.GetType();

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

        #region General Getters/Setters : Class (DomainTrackerValue) => Property (DomainTrackerActivityID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_Class_Invalid_Property_DomainTrackerActivityIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDomainTrackerActivityID = "DomainTrackerActivityIDNotPresent";
            var domainTrackerValue  = Fixture.Create<DomainTrackerValue>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerValue.GetType().GetProperty(propertyNameDomainTrackerActivityID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_DomainTrackerActivityID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDomainTrackerActivityID = "DomainTrackerActivityID";
            var domainTrackerValue  = Fixture.Create<DomainTrackerValue>();
            var propertyInfo  = domainTrackerValue.GetType().GetProperty(propertyNameDomainTrackerActivityID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerValue) => Property (DomainTrackerFieldsID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_DomainTrackerFieldsID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var domainTrackerValue = Fixture.Create<DomainTrackerValue>();
            domainTrackerValue.DomainTrackerFieldsID = Fixture.Create<int>();
            var intType = domainTrackerValue.DomainTrackerFieldsID.GetType();

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

        #region General Getters/Setters : Class (DomainTrackerValue) => Property (DomainTrackerFieldsID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_Class_Invalid_Property_DomainTrackerFieldsIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDomainTrackerFieldsID = "DomainTrackerFieldsIDNotPresent";
            var domainTrackerValue  = Fixture.Create<DomainTrackerValue>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerValue.GetType().GetProperty(propertyNameDomainTrackerFieldsID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_DomainTrackerFieldsID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDomainTrackerFieldsID = "DomainTrackerFieldsID";
            var domainTrackerValue  = Fixture.Create<DomainTrackerValue>();
            var propertyInfo  = domainTrackerValue.GetType().GetProperty(propertyNameDomainTrackerFieldsID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerValue) => Property (DomainTrackerValueID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_DomainTrackerValueID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var domainTrackerValue = Fixture.Create<DomainTrackerValue>();
            domainTrackerValue.DomainTrackerValueID = Fixture.Create<int>();
            var intType = domainTrackerValue.DomainTrackerValueID.GetType();

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

        #region General Getters/Setters : Class (DomainTrackerValue) => Property (DomainTrackerValueID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_Class_Invalid_Property_DomainTrackerValueIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDomainTrackerValueID = "DomainTrackerValueIDNotPresent";
            var domainTrackerValue  = Fixture.Create<DomainTrackerValue>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerValue.GetType().GetProperty(propertyNameDomainTrackerValueID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_DomainTrackerValueID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDomainTrackerValueID = "DomainTrackerValueID";
            var domainTrackerValue  = Fixture.Create<DomainTrackerValue>();
            var propertyInfo  = domainTrackerValue.GetType().GetProperty(propertyNameDomainTrackerValueID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerValue) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var domainTrackerValue = Fixture.Create<DomainTrackerValue>();
            var random = Fixture.Create<bool>();

            // Act , Set
            domainTrackerValue.IsDeleted = random;

            // Assert
            domainTrackerValue.IsDeleted.ShouldBe(random);
            domainTrackerValue.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var domainTrackerValue = Fixture.Create<DomainTrackerValue>();

            // Act , Set
            domainTrackerValue.IsDeleted = null;

            // Assert
            domainTrackerValue.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var domainTrackerValue = Fixture.Create<DomainTrackerValue>();
            var propertyInfo = domainTrackerValue.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(domainTrackerValue, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            domainTrackerValue.IsDeleted.ShouldBeNull();
            domainTrackerValue.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerValue) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var domainTrackerValue  = Fixture.Create<DomainTrackerValue>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerValue.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var domainTrackerValue  = Fixture.Create<DomainTrackerValue>();
            var propertyInfo  = domainTrackerValue.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerValue) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var domainTrackerValue = Fixture.Create<DomainTrackerValue>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = domainTrackerValue.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(domainTrackerValue, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerValue) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var domainTrackerValue  = Fixture.Create<DomainTrackerValue>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerValue.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var domainTrackerValue  = Fixture.Create<DomainTrackerValue>();
            var propertyInfo  = domainTrackerValue.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerValue) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var domainTrackerValue = Fixture.Create<DomainTrackerValue>();
            var random = Fixture.Create<int>();

            // Act , Set
            domainTrackerValue.UpdatedUserID = random;

            // Assert
            domainTrackerValue.UpdatedUserID.ShouldBe(random);
            domainTrackerValue.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var domainTrackerValue = Fixture.Create<DomainTrackerValue>();

            // Act , Set
            domainTrackerValue.UpdatedUserID = null;

            // Assert
            domainTrackerValue.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var domainTrackerValue = Fixture.Create<DomainTrackerValue>();
            var propertyInfo = domainTrackerValue.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(domainTrackerValue, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            domainTrackerValue.UpdatedUserID.ShouldBeNull();
            domainTrackerValue.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerValue) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var domainTrackerValue  = Fixture.Create<DomainTrackerValue>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerValue.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var domainTrackerValue  = Fixture.Create<DomainTrackerValue>();
            var propertyInfo  = domainTrackerValue.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerValue) => Property (Value) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_Value_Property_String_Type_Verify_Test()
        {
            // Arrange
            var domainTrackerValue = Fixture.Create<DomainTrackerValue>();
            domainTrackerValue.Value = Fixture.Create<string>();
            var stringType = domainTrackerValue.Value.GetType();

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

        #region General Getters/Setters : Class (DomainTrackerValue) => Property (Value) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_Class_Invalid_Property_ValueNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameValue = "ValueNotPresent";
            var domainTrackerValue  = Fixture.Create<DomainTrackerValue>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerValue.GetType().GetProperty(propertyNameValue));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerValue_Value_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameValue = "Value";
            var domainTrackerValue  = Fixture.Create<DomainTrackerValue>();
            var propertyInfo  = domainTrackerValue.GetType().GetProperty(propertyNameValue);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (DomainTrackerValue) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DomainTrackerValue_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new DomainTrackerValue());
        }

        #endregion

        #region General Constructor : Class (DomainTrackerValue) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DomainTrackerValue_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfDomainTrackerValue = Fixture.CreateMany<DomainTrackerValue>(2).ToList();
            var firstDomainTrackerValue = instancesOfDomainTrackerValue.FirstOrDefault();
            var lastDomainTrackerValue = instancesOfDomainTrackerValue.Last();

            // Act, Assert
            firstDomainTrackerValue.ShouldNotBeNull();
            lastDomainTrackerValue.ShouldNotBeNull();
            firstDomainTrackerValue.ShouldNotBeSameAs(lastDomainTrackerValue);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DomainTrackerValue_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstDomainTrackerValue = new DomainTrackerValue();
            var secondDomainTrackerValue = new DomainTrackerValue();
            var thirdDomainTrackerValue = new DomainTrackerValue();
            var fourthDomainTrackerValue = new DomainTrackerValue();
            var fifthDomainTrackerValue = new DomainTrackerValue();
            var sixthDomainTrackerValue = new DomainTrackerValue();

            // Act, Assert
            firstDomainTrackerValue.ShouldNotBeNull();
            secondDomainTrackerValue.ShouldNotBeNull();
            thirdDomainTrackerValue.ShouldNotBeNull();
            fourthDomainTrackerValue.ShouldNotBeNull();
            fifthDomainTrackerValue.ShouldNotBeNull();
            sixthDomainTrackerValue.ShouldNotBeNull();
            firstDomainTrackerValue.ShouldNotBeSameAs(secondDomainTrackerValue);
            thirdDomainTrackerValue.ShouldNotBeSameAs(firstDomainTrackerValue);
            fourthDomainTrackerValue.ShouldNotBeSameAs(firstDomainTrackerValue);
            fifthDomainTrackerValue.ShouldNotBeSameAs(firstDomainTrackerValue);
            sixthDomainTrackerValue.ShouldNotBeSameAs(firstDomainTrackerValue);
            sixthDomainTrackerValue.ShouldNotBeSameAs(fourthDomainTrackerValue);
        }

        #endregion

        #region General Constructor : Class (DomainTrackerValue) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DomainTrackerValue_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var domainTrackerValueId = -1;
            var domainTrackerFieldsId = -1;
            var domainTrackerActivityId = -1;
            var value = string.Empty;

            // Act
            var domainTrackerValue = new DomainTrackerValue();

            // Assert
            domainTrackerValue.DomainTrackerValueID.ShouldBe(domainTrackerValueId);
            domainTrackerValue.DomainTrackerFieldsID.ShouldBe(domainTrackerFieldsId);
            domainTrackerValue.DomainTrackerActivityID.ShouldBe(domainTrackerActivityId);
            domainTrackerValue.Value.ShouldBe(value);
            domainTrackerValue.CreatedUserID.ShouldBeNull();
            domainTrackerValue.CreatedDate.ShouldBeNull();
            domainTrackerValue.UpdatedUserID.ShouldBeNull();
            domainTrackerValue.UpdatedDate.ShouldBeNull();
            domainTrackerValue.IsDeleted.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}