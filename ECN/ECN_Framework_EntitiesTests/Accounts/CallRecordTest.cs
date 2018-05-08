using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Shouldly;
using NUnit.Framework;
using AutoFixture;
using ECN_Framework_EntitiesTests.ConfigureProject;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_EntitiesTests.Accounts
{
    [TestFixture]
    public class CallRecordTest : AbstractGenericTest
    {
        #region General Category : General

        #region Category : GetterSetter

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CallRecord_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var callRecord  = new CallRecord();
            var callRecordID = Fixture.Create<int>();
            var staffID = Fixture.Create<int>();
            var callDate = Fixture.Create<DateTime>();
            var callCount = Fixture.Create<int>();

            // Act
            callRecord.CallRecordID = callRecordID;
            callRecord.StaffID = staffID;
            callRecord.CallDate = callDate;
            callRecord.CallCount = callCount;

            // Assert
            callRecord.CallRecordID.ShouldBe(callRecordID);
            callRecord.StaffID.ShouldBe(staffID);
            callRecord.CallDate.ShouldBe(callDate);
            callRecord.CallCount.ShouldBe(callCount);
            callRecord.ErrorList.ShouldBeEmpty();
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region Non-String Property Type Test : CallRecord => CallDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CallDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constCallDate = "CallDate";
            var callRecord = Fixture.Create<CallRecord>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = callRecord.GetType().GetProperty(constCallDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(callRecord, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CallRecord_Class_Invalid_Property_CallDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCallDate = "CallDate";
            var callRecord  = Fixture.Create<CallRecord>();

            // Act , Assert
            Should.NotThrow(() => callRecord.GetType().GetProperty(constCallDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CallDate_Is_Present_In_CallRecord_Class_As_Public_Test()
        {
            // Arrange
            const string constCallDate = "CallDate";
            var callRecord  = Fixture.Create<CallRecord>();
            var propertyInfo  = callRecord.GetType().GetProperty(constCallDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : CallRecord => CallCount

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CallCount_Int_Type_Verify_Test()
        {
            // Arrange
            var callRecord = Fixture.Create<CallRecord>();
            var intType = callRecord.CallCount.GetType();

            // Act
            var isTypeInt = typeof(int).Equals(intType);    
            var isTypeNullableInt = typeof(int?).Equals(intType);
            var isTypeString = typeof(string).Equals(intType);
            var isTypeDecimal = typeof(decimal).Equals(intType);
            var isTypeLong = typeof(long).Equals(intType);
            var isTypeBool = typeof(bool).Equals(intType);
            var isTypeDouble = typeof(double).Equals(intType);
            var isTypeFloat = typeof(float).Equals(intType);
            var isTypeDecimalNullable = typeof(decimal?).Equals(intType);
            var isTypeLongNullable = typeof(long?).Equals(intType);
            var isTypeBoolNullable = typeof(bool?).Equals(intType);
            var isTypeDoubleNullable = typeof(double?).Equals(intType);
            var isTypeFloatNullable = typeof(float?).Equals(intType);


            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CallRecord_Class_Invalid_Property_CallCount_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCallCount = "CallCount";
            var callRecord  = Fixture.Create<CallRecord>();

            // Act , Assert
            Should.NotThrow(() => callRecord.GetType().GetProperty(constCallCount));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CallCount_Is_Present_In_CallRecord_Class_As_Public_Test()
        {
            // Arrange
            const string constCallCount = "CallCount";
            var callRecord  = Fixture.Create<CallRecord>();
            var propertyInfo  = callRecord.GetType().GetProperty(constCallCount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : CallRecord => CallRecordID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CallRecordID_Int_Type_Verify_Test()
        {
            // Arrange
            var callRecord = Fixture.Create<CallRecord>();
            var intType = callRecord.CallRecordID.GetType();

            // Act
            var isTypeInt = typeof(int).Equals(intType);    
            var isTypeNullableInt = typeof(int?).Equals(intType);
            var isTypeString = typeof(string).Equals(intType);
            var isTypeDecimal = typeof(decimal).Equals(intType);
            var isTypeLong = typeof(long).Equals(intType);
            var isTypeBool = typeof(bool).Equals(intType);
            var isTypeDouble = typeof(double).Equals(intType);
            var isTypeFloat = typeof(float).Equals(intType);
            var isTypeDecimalNullable = typeof(decimal?).Equals(intType);
            var isTypeLongNullable = typeof(long?).Equals(intType);
            var isTypeBoolNullable = typeof(bool?).Equals(intType);
            var isTypeDoubleNullable = typeof(double?).Equals(intType);
            var isTypeFloatNullable = typeof(float?).Equals(intType);


            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CallRecord_Class_Invalid_Property_CallRecordID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCallRecordID = "CallRecordID";
            var callRecord  = Fixture.Create<CallRecord>();

            // Act , Assert
            Should.NotThrow(() => callRecord.GetType().GetProperty(constCallRecordID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CallRecordID_Is_Present_In_CallRecord_Class_As_Public_Test()
        {
            // Arrange
            const string constCallRecordID = "CallRecordID";
            var callRecord  = Fixture.Create<CallRecord>();
            var propertyInfo  = callRecord.GetType().GetProperty(constCallRecordID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : CallRecord => StaffID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_StaffID_Int_Type_Verify_Test()
        {
            // Arrange
            var callRecord = Fixture.Create<CallRecord>();
            var intType = callRecord.StaffID.GetType();

            // Act
            var isTypeInt = typeof(int).Equals(intType);    
            var isTypeNullableInt = typeof(int?).Equals(intType);
            var isTypeString = typeof(string).Equals(intType);
            var isTypeDecimal = typeof(decimal).Equals(intType);
            var isTypeLong = typeof(long).Equals(intType);
            var isTypeBool = typeof(bool).Equals(intType);
            var isTypeDouble = typeof(double).Equals(intType);
            var isTypeFloat = typeof(float).Equals(intType);
            var isTypeDecimalNullable = typeof(decimal?).Equals(intType);
            var isTypeLongNullable = typeof(long?).Equals(intType);
            var isTypeBoolNullable = typeof(bool?).Equals(intType);
            var isTypeDoubleNullable = typeof(double?).Equals(intType);
            var isTypeFloatNullable = typeof(float?).Equals(intType);


            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CallRecord_Class_Invalid_Property_StaffID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constStaffID = "StaffID";
            var callRecord  = Fixture.Create<CallRecord>();

            // Act , Assert
            Should.NotThrow(() => callRecord.GetType().GetProperty(constStaffID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_StaffID_Is_Present_In_CallRecord_Class_As_Public_Test()
        {
            // Arrange
            const string constStaffID = "StaffID";
            var callRecord  = Fixture.Create<CallRecord>();
            var propertyInfo  = callRecord.GetType().GetProperty(constStaffID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CallRecord_Class_Invalid_Property_ErrorList_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constErrorList = "ErrorList";
            var callRecord  = Fixture.Create<CallRecord>();

            // Act , Assert
            Should.NotThrow(() => callRecord.GetType().GetProperty(constErrorList));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ErrorList_Is_Present_In_CallRecord_Class_As_Public_Test()
        {
            // Arrange
            const string constErrorList = "ErrorList";
            var callRecord  = Fixture.Create<CallRecord>();
            var propertyInfo  = callRecord.GetType().GetProperty(constErrorList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #endregion

        #endregion

        #region General Category : General

        #region Category : Contructor

        #region General Constructor Pattern : create and expect no exception.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new CallRecord());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<CallRecord>(2).ToList();
            var first = myInstances.FirstOrDefault();
            var last = myInstances.Last();

            // Act, Assert
            first.ShouldNotBeNull();
            last.ShouldNotBeNull();
            first.ShouldNotBeSameAs(last);
        }

        #endregion

        #region General Constructor Pattern : Default Assignment Test

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Instantiated_With_Default_Assignments_NoChange_DefaultValues()
        {
            // Arrange
            var callRecordID = -1;
            var staffID = -1;
            var callCount = -1;
            var errorList = new List<ECNError>();    

            // Act
            var callRecord = new CallRecord();    

            // Assert
            callRecord.CallRecordID.ShouldBe(callRecordID);
            callRecord.StaffID.ShouldBe(staffID);
            callRecord.CallCount.ShouldBe(callCount);
            callRecord.ErrorList.ShouldBeEmpty();
        }

        #endregion

        #endregion

        #endregion
    }
}