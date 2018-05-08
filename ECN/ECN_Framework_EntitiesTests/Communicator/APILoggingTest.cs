using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Reflection;
using Shouldly;
using AutoFixture;
using NUnit.Framework;
using Moq;
using ECN_Framework_EntitiesTests.ConfigureProject;
using ECN_Framework_Entities.Communicator;

namespace ECN_Framework_Entities.Communicator
{
    [TestFixture]
    public class APILoggingTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : Constructor

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void APILogging_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var aPILogging  = new APILogging();
            var aPILogID = Fixture.Create<int>();
            var accessKey = Fixture.Create<string>();
            var aPIMethod = Fixture.Create<string>();
            var input = Fixture.Create<string>();
            var startTime = Fixture.Create<DateTime?>();
            var logID = Fixture.Create<int?>();
            var endTime = Fixture.Create<DateTime?>();

            // Act
            aPILogging.APILogID = aPILogID;
            aPILogging.AccessKey = accessKey;
            aPILogging.APIMethod = aPIMethod;
            aPILogging.Input = input;
            aPILogging.StartTime = startTime;
            aPILogging.LogID = logID;
            aPILogging.EndTime = endTime;

            // Assert
            aPILogging.APILogID.ShouldBe(aPILogID);
            aPILogging.AccessKey.ShouldBe(accessKey);
            aPILogging.APIMethod.ShouldBe(aPIMethod);
            aPILogging.Input.ShouldBe(input);
            aPILogging.StartTime.ShouldBe(startTime);
            aPILogging.LogID.ShouldBe(logID);
            aPILogging.EndTime.ShouldBe(endTime);   
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region string property type test : APILogging => AccessKey

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void APILogging_AccessKey_Property_String_Type_Verify_Test()
        {
            // Arrange
            var aPILogging = Fixture.Create<APILogging>();
            var stringType = aPILogging.AccessKey.GetType();

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void APILogging_Class_Invalid_Property_AccessKeyNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAccessKey = "AccessKeyNotPresent";
            var aPILogging  = Fixture.Create<APILogging>();

            // Act , Assert
            Should.NotThrow(() => aPILogging.GetType().GetProperty(propertyNameAccessKey));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void APILogging_AccessKey_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAccessKey = "AccessKey";
            var aPILogging  = Fixture.Create<APILogging>();
            var propertyInfo  = aPILogging.GetType().GetProperty(propertyNameAccessKey);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : APILogging => APILogID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void APILogging_APILogID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var aPILogging = Fixture.Create<APILogging>();
            var intType = aPILogging.APILogID.GetType();

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void APILogging_Class_Invalid_Property_APILogIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAPILogID = "APILogIDNotPresent";
            var aPILogging  = Fixture.Create<APILogging>();

            // Act , Assert
            Should.NotThrow(() => aPILogging.GetType().GetProperty(propertyNameAPILogID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void APILogging_APILogID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAPILogID = "APILogID";
            var aPILogging  = Fixture.Create<APILogging>();
            var propertyInfo  = aPILogging.GetType().GetProperty(propertyNameAPILogID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : APILogging => APIMethod

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void APILogging_APIMethod_Property_String_Type_Verify_Test()
        {
            // Arrange
            var aPILogging = Fixture.Create<APILogging>();
            var stringType = aPILogging.APIMethod.GetType();

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void APILogging_Class_Invalid_Property_APIMethodNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAPIMethod = "APIMethodNotPresent";
            var aPILogging  = Fixture.Create<APILogging>();

            // Act , Assert
            Should.NotThrow(() => aPILogging.GetType().GetProperty(propertyNameAPIMethod));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void APILogging_APIMethod_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAPIMethod = "APIMethod";
            var aPILogging  = Fixture.Create<APILogging>();
            var propertyInfo  = aPILogging.GetType().GetProperty(propertyNameAPIMethod);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : APILogging => EndTime

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void APILogging_EndTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameEndTime = "EndTime";
            var aPILogging = Fixture.Create<APILogging>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = aPILogging.GetType().GetProperty(propertyNameEndTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(aPILogging, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void APILogging_Class_Invalid_Property_EndTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEndTime = "EndTimeNotPresent";
            var aPILogging  = Fixture.Create<APILogging>();

            // Act , Assert
            Should.NotThrow(() => aPILogging.GetType().GetProperty(propertyNameEndTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void APILogging_EndTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEndTime = "EndTime";
            var aPILogging  = Fixture.Create<APILogging>();
            var propertyInfo  = aPILogging.GetType().GetProperty(propertyNameEndTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : APILogging => Input

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void APILogging_Input_Property_String_Type_Verify_Test()
        {
            // Arrange
            var aPILogging = Fixture.Create<APILogging>();
            var stringType = aPILogging.Input.GetType();

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void APILogging_Class_Invalid_Property_InputNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameInput = "InputNotPresent";
            var aPILogging  = Fixture.Create<APILogging>();

            // Act , Assert
            Should.NotThrow(() => aPILogging.GetType().GetProperty(propertyNameInput));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void APILogging_Input_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameInput = "Input";
            var aPILogging  = Fixture.Create<APILogging>();
            var propertyInfo  = aPILogging.GetType().GetProperty(propertyNameInput);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : APILogging => LogID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void APILogging_LogID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var aPILogging = Fixture.Create<APILogging>();
            var random = Fixture.Create<int>();

            // Act , Set
            aPILogging.LogID = random;

            // Assert
            aPILogging.LogID.ShouldBe(random);
            aPILogging.LogID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void APILogging_LogID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var aPILogging = Fixture.Create<APILogging>();    

            // Act , Set
            aPILogging.LogID = null;

            // Assert
            aPILogging.LogID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void APILogging_LogID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameLogID = "LogID";
            var aPILogging = Fixture.Create<APILogging>();
            var propertyInfo = aPILogging.GetType().GetProperty(propertyNameLogID);

            // Act , Set
            propertyInfo.SetValue(aPILogging, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            aPILogging.LogID.ShouldBeNull();
            aPILogging.LogID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void APILogging_Class_Invalid_Property_LogIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLogID = "LogIDNotPresent";
            var aPILogging  = Fixture.Create<APILogging>();

            // Act , Assert
            Should.NotThrow(() => aPILogging.GetType().GetProperty(propertyNameLogID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void APILogging_LogID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLogID = "LogID";
            var aPILogging  = Fixture.Create<APILogging>();
            var propertyInfo  = aPILogging.GetType().GetProperty(propertyNameLogID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : APILogging => StartTime

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void APILogging_StartTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameStartTime = "StartTime";
            var aPILogging = Fixture.Create<APILogging>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = aPILogging.GetType().GetProperty(propertyNameStartTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(aPILogging, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void APILogging_Class_Invalid_Property_StartTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameStartTime = "StartTimeNotPresent";
            var aPILogging  = Fixture.Create<APILogging>();

            // Act , Assert
            Should.NotThrow(() => aPILogging.GetType().GetProperty(propertyNameStartTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void APILogging_StartTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameStartTime = "StartTime";
            var aPILogging  = Fixture.Create<APILogging>();
            var propertyInfo  = aPILogging.GetType().GetProperty(propertyNameStartTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion


        #endregion
        #region Category : Constructor

        #region General Constructor Pattern : create and expect no exception.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_APILogging_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new APILogging());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<APILogging>(2).ToList();
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
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_APILogging_Instantiated_With_Default_Assignments_No_Change_Test()
        {
            // Arrange
            var aPILogID = -1;
            var accessKey = string.Empty;
            var aPIMethod = string.Empty;
            var input = string.Empty;
            DateTime? startTime = null;
            int? logID = null;
            DateTime? endTime = null;

            // Act
            var aPILogging = new APILogging();

            // Assert
            aPILogging.APILogID.ShouldBe(aPILogID);
            aPILogging.AccessKey.ShouldBe(accessKey);
            aPILogging.APIMethod.ShouldBe(aPIMethod);
            aPILogging.Input.ShouldBe(input);
            aPILogging.StartTime.ShouldBeNull();
            aPILogging.LogID.ShouldBeNull();
            aPILogging.EndTime.ShouldBeNull();
        }

        #endregion


        #endregion


        #endregion
    }
}