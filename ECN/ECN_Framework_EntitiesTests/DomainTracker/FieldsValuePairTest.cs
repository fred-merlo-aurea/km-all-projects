using System;
using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using ECN_Framework_Entities.DomainTracker;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.DomainTracker
{
    [TestFixture]
    public class FieldsValuePairTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (FieldsValuePair) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FieldsValuePair_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var fieldsValuePair = Fixture.Create<FieldsValuePair>();
            var domainTrackerActivityId = Fixture.Create<int>();
            var fieldName = Fixture.Create<string>();
            var value = Fixture.Create<string>();
            var referralURL = Fixture.Create<string>();
            var createdDate = Fixture.Create<DateTime>();

            // Act
            fieldsValuePair.DomainTrackerActivityID = domainTrackerActivityId;
            fieldsValuePair.FieldName = fieldName;
            fieldsValuePair.Value = value;
            fieldsValuePair.ReferralURL = referralURL;
            fieldsValuePair.CreatedDate = createdDate;

            // Assert
            fieldsValuePair.DomainTrackerActivityID.ShouldBe(domainTrackerActivityId);
            fieldsValuePair.FieldName.ShouldBe(fieldName);
            fieldsValuePair.Value.ShouldBe(value);
            fieldsValuePair.ReferralURL.ShouldBe(referralURL);
            fieldsValuePair.CreatedDate.ShouldBe(createdDate);
        }

        #endregion

        #region General Getters/Setters : Class (FieldsValuePair) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FieldsValuePair_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var fieldsValuePair = Fixture.Create<FieldsValuePair>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = fieldsValuePair.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(fieldsValuePair, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (FieldsValuePair) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FieldsValuePair_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var fieldsValuePair  = Fixture.Create<FieldsValuePair>();

            // Act , Assert
            Should.NotThrow(() => fieldsValuePair.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FieldsValuePair_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var fieldsValuePair  = Fixture.Create<FieldsValuePair>();
            var propertyInfo  = fieldsValuePair.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FieldsValuePair) => Property (DomainTrackerActivityID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FieldsValuePair_DomainTrackerActivityID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var fieldsValuePair = Fixture.Create<FieldsValuePair>();
            fieldsValuePair.DomainTrackerActivityID = Fixture.Create<int>();
            var intType = fieldsValuePair.DomainTrackerActivityID.GetType();

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

        #region General Getters/Setters : Class (FieldsValuePair) => Property (DomainTrackerActivityID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FieldsValuePair_Class_Invalid_Property_DomainTrackerActivityIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDomainTrackerActivityID = "DomainTrackerActivityIDNotPresent";
            var fieldsValuePair  = Fixture.Create<FieldsValuePair>();

            // Act , Assert
            Should.NotThrow(() => fieldsValuePair.GetType().GetProperty(propertyNameDomainTrackerActivityID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FieldsValuePair_DomainTrackerActivityID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDomainTrackerActivityID = "DomainTrackerActivityID";
            var fieldsValuePair  = Fixture.Create<FieldsValuePair>();
            var propertyInfo  = fieldsValuePair.GetType().GetProperty(propertyNameDomainTrackerActivityID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FieldsValuePair) => Property (FieldName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FieldsValuePair_FieldName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var fieldsValuePair = Fixture.Create<FieldsValuePair>();
            fieldsValuePair.FieldName = Fixture.Create<string>();
            var stringType = fieldsValuePair.FieldName.GetType();

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

        #region General Getters/Setters : Class (FieldsValuePair) => Property (FieldName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FieldsValuePair_Class_Invalid_Property_FieldNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFieldName = "FieldNameNotPresent";
            var fieldsValuePair  = Fixture.Create<FieldsValuePair>();

            // Act , Assert
            Should.NotThrow(() => fieldsValuePair.GetType().GetProperty(propertyNameFieldName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FieldsValuePair_FieldName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFieldName = "FieldName";
            var fieldsValuePair  = Fixture.Create<FieldsValuePair>();
            var propertyInfo  = fieldsValuePair.GetType().GetProperty(propertyNameFieldName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FieldsValuePair) => Property (ReferralURL) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FieldsValuePair_ReferralURL_Property_String_Type_Verify_Test()
        {
            // Arrange
            var fieldsValuePair = Fixture.Create<FieldsValuePair>();
            fieldsValuePair.ReferralURL = Fixture.Create<string>();
            var stringType = fieldsValuePair.ReferralURL.GetType();

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

        #region General Getters/Setters : Class (FieldsValuePair) => Property (ReferralURL) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FieldsValuePair_Class_Invalid_Property_ReferralURLNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameReferralURL = "ReferralURLNotPresent";
            var fieldsValuePair  = Fixture.Create<FieldsValuePair>();

            // Act , Assert
            Should.NotThrow(() => fieldsValuePair.GetType().GetProperty(propertyNameReferralURL));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FieldsValuePair_ReferralURL_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameReferralURL = "ReferralURL";
            var fieldsValuePair  = Fixture.Create<FieldsValuePair>();
            var propertyInfo  = fieldsValuePair.GetType().GetProperty(propertyNameReferralURL);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FieldsValuePair) => Property (Value) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FieldsValuePair_Value_Property_String_Type_Verify_Test()
        {
            // Arrange
            var fieldsValuePair = Fixture.Create<FieldsValuePair>();
            fieldsValuePair.Value = Fixture.Create<string>();
            var stringType = fieldsValuePair.Value.GetType();

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

        #region General Getters/Setters : Class (FieldsValuePair) => Property (Value) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FieldsValuePair_Class_Invalid_Property_ValueNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameValue = "ValueNotPresent";
            var fieldsValuePair  = Fixture.Create<FieldsValuePair>();

            // Act , Assert
            Should.NotThrow(() => fieldsValuePair.GetType().GetProperty(propertyNameValue));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FieldsValuePair_Value_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameValue = "Value";
            var fieldsValuePair  = Fixture.Create<FieldsValuePair>();
            var propertyInfo  = fieldsValuePair.GetType().GetProperty(propertyNameValue);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #endregion
    }
}