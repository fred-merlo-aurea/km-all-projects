using System;
using System.Reflection;
using AutoFixture;
using AUT.ConfigureTestProjects;
using ecn.collector.main.survey.UserControls;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Collector.Tests.Main.Survey.UserControls
{
    [TestFixture]
    public class DefineIntroTest : AbstractGenericTest
    {
        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DefineIntro_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var defineIntro = new DefineIntro();
            var surveyId = Fixture.Create<int>();
            var errorMessage = Fixture.Create<string>();

            // Act
            defineIntro.SurveyID = surveyId;
            defineIntro.ErrorMessage = errorMessage;

            // Assert
            defineIntro.SurveyID.ShouldBe(surveyId);
            defineIntro.ErrorMessage.ShouldBe(errorMessage);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DefineIntro_Class_Invalid_Property_ErrorMessageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameErrorMessage = "ErrorMessageNotPresent";
            var defineIntro  = new DefineIntro();

            // Act , Assert
            Should.NotThrow(action: () => defineIntro.GetType().GetProperty(propertyNameErrorMessage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DefineIntro_Class_Invalid_Property_SurveyIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSurveyId = "SurveyIDNotPresent";
            var defineIntro  = new DefineIntro();

            // Act , Assert
            Should.NotThrow(action: () => defineIntro.GetType().GetProperty(propertyNameSurveyId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DefineIntro_ErrorMessage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameErrorMessage = "ErrorMessage";
            var defineIntro  = new DefineIntro();
            var propertyInfo  = defineIntro.GetType().GetProperty(propertyNameErrorMessage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DefineIntro_ErrorMessage_Property_String_Type_Verify_Test()
        {
            // Arrange
            var defineIntro = new DefineIntro();
            defineIntro.ErrorMessage = Fixture.Create<string>();
            var stringType = defineIntro.ErrorMessage.GetType();

            // Act
            var isTypeString = typeof(string) == stringType;
            var isTypeInt = typeof(int) == stringType;
            var isTypeDecimal = typeof(decimal) == stringType;
            var isTypeLong = typeof(long) == stringType;
            var isTypeBool = typeof(bool) == stringType;
            var isTypeDouble = typeof(double) == stringType;
            var isTypeFloat = typeof(float) == stringType;

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void DefineIntro_Initialize_Method_No_Parameters_2_Calls_Test()
        {
            // Arrange
            var defineIntro  = new DefineIntro();

            // Act
            Action initialize = () => defineIntro.Initialize();

            // Assert
            Should.NotThrow(action: () => initialize.Invoke());
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void DefineIntro_Initialize_Method_No_Parameters_Simple_Call_Test()
        {
            // Arrange
            var defineIntro  = new DefineIntro();

            // Act, Assert
            Should.NotThrow(action: () => defineIntro.Initialize());
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void DefineIntro_Initialize_Method_With_No_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            object[] parametersOutRanged = {null, null};
            var defineIntro  = new DefineIntro();
            var methodName = "Initialize";

            // Act
            var initializeMethodInfo1 = defineIntro.GetType().GetMethod(methodName);
            var initializeMethodInfo2 = defineIntro.GetType().GetMethod(methodName);
            var returnType1 = initializeMethodInfo1.ReturnType;
            var returnType2 = initializeMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            defineIntro.ShouldNotBeNull();
            initializeMethodInfo1.ShouldNotBeNull();
            initializeMethodInfo2.ShouldNotBeNull();
            initializeMethodInfo1.ShouldBe(initializeMethodInfo2);
            Should.Throw<Exception>(actual: () => initializeMethodInfo1.Invoke(defineIntro, parametersOutRanged));
            Should.Throw<Exception>(actual: () => initializeMethodInfo2.Invoke(defineIntro, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(actual: () => initializeMethodInfo1.Invoke(defineIntro, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(actual: () => initializeMethodInfo2.Invoke(defineIntro, parametersOutRanged));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void DefineIntro_Initialize_Method_With_No_Parameters_Call_With_Reflection_Test()
        {
            // Arrange
            var defineIntro = new DefineIntro();
            var methodName = "Initialize";

            // Act
            var initializeMethodInfo1 = defineIntro.GetType().GetMethod(methodName);
            var initializeMethodInfo2 = defineIntro.GetType().GetMethod(methodName);
            var returnType1 = initializeMethodInfo1.ReturnType;
            var returnType2 = initializeMethodInfo2.ReturnType;

            // Assert
            defineIntro.ShouldNotBeNull();
            initializeMethodInfo1.ShouldNotBeNull();
            initializeMethodInfo2.ShouldNotBeNull();
            initializeMethodInfo1.ShouldBe(initializeMethodInfo2);
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            Should.NotThrow(action: () => initializeMethodInfo1.Invoke(defineIntro, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void DefineIntro_Save_Method_With_No_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            object[] parametersOutRanged = {null, null};
            var defineIntro  = new DefineIntro();
            var methodName = "Save";

            // Act
            var saveMethodInfo1 = defineIntro.GetType().GetMethod(methodName);
            var saveMethodInfo2 = defineIntro.GetType().GetMethod(methodName);
            var returnType1 = saveMethodInfo1.ReturnType;
            var returnType2 = saveMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            defineIntro.ShouldNotBeNull();
            saveMethodInfo1.ShouldNotBeNull();
            saveMethodInfo2.ShouldNotBeNull();
            saveMethodInfo1.ShouldBe(saveMethodInfo2);
            Should.Throw<Exception>(actual: () => saveMethodInfo1.Invoke(defineIntro, parametersOutRanged));
            Should.Throw<Exception>(actual: () => saveMethodInfo2.Invoke(defineIntro, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(actual: () => saveMethodInfo1.Invoke(defineIntro, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(actual: () => saveMethodInfo2.Invoke(defineIntro, parametersOutRanged));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DefineIntro_SurveyID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var defineIntro = new DefineIntro();
            defineIntro.SurveyID = Fixture.Create<int>();
            var intType = defineIntro.SurveyID.GetType();

            // Act
            var isTypeInt = typeof(int) == intType;
            var isTypeNullableInt = typeof(int?) == intType;
            var isTypeString = typeof(string) == intType;
            var isTypeDecimal = typeof(decimal) == intType;
            var isTypeLong = typeof(long) == intType;
            var isTypeBool = typeof(bool) == intType;
            var isTypeDouble = typeof(double) == intType;
            var isTypeFloat = typeof(float) == intType;
            var isTypeDecimalNullable = typeof(decimal?) == intType;
            var isTypeLongNullable = typeof(long?) == intType;
            var isTypeBoolNullable = typeof(bool?) == intType;
            var isTypeDoubleNullable = typeof(double?) == intType;
            var isTypeFloatNullable = typeof(float?) == intType;

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

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DefineIntro_SurveyID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSurveyId = "SurveyID";
            var defineIntro  = new DefineIntro();
            var propertyInfo  = defineIntro.GetType().GetProperty(propertyNameSurveyId);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }
    }
}