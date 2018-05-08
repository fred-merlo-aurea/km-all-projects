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
using ECN_Framework_Entities.Collector;

namespace ECN_Framework_Entities.Collector
{
    [TestFixture]
    public class ResponseOptionsTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ResponseOptions_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var responseOptions  = new ResponseOptions();
            var optionID = Fixture.Create<int>();
            var questionID = Fixture.Create<int>();
            var optionValue = Fixture.Create<string>();
            var score = Fixture.Create<int>();

            // Act
            responseOptions.OptionID = optionID;
            responseOptions.QuestionID = questionID;
            responseOptions.OptionValue = optionValue;
            responseOptions.Score = score;

            // Assert
            responseOptions.OptionID.ShouldBe(optionID);
            responseOptions.QuestionID.ShouldBe(questionID);
            responseOptions.OptionValue.ShouldBe(optionValue);
            responseOptions.Score.ShouldBe(score);   
        }

        #endregion

        #region int property type test : ResponseOptions => OptionID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ResponseOptions_OptionID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var responseOptions = Fixture.Create<ResponseOptions>();
            var intType = responseOptions.OptionID.GetType();

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
        public void ResponseOptions_Class_Invalid_Property_OptionIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOptionID = "OptionIDNotPresent";
            var responseOptions  = Fixture.Create<ResponseOptions>();

            // Act , Assert
            Should.NotThrow(() => responseOptions.GetType().GetProperty(propertyNameOptionID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ResponseOptions_OptionID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOptionID = "OptionID";
            var responseOptions  = Fixture.Create<ResponseOptions>();
            var propertyInfo  = responseOptions.GetType().GetProperty(propertyNameOptionID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : ResponseOptions => OptionValue

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ResponseOptions_OptionValue_Property_String_Type_Verify_Test()
        {
            // Arrange
            var responseOptions = Fixture.Create<ResponseOptions>();
            var stringType = responseOptions.OptionValue.GetType();

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
        public void ResponseOptions_Class_Invalid_Property_OptionValueNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOptionValue = "OptionValueNotPresent";
            var responseOptions  = Fixture.Create<ResponseOptions>();

            // Act , Assert
            Should.NotThrow(() => responseOptions.GetType().GetProperty(propertyNameOptionValue));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ResponseOptions_OptionValue_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOptionValue = "OptionValue";
            var responseOptions  = Fixture.Create<ResponseOptions>();
            var propertyInfo  = responseOptions.GetType().GetProperty(propertyNameOptionValue);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : ResponseOptions => QuestionID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ResponseOptions_QuestionID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var responseOptions = Fixture.Create<ResponseOptions>();
            var intType = responseOptions.QuestionID.GetType();

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
        public void ResponseOptions_Class_Invalid_Property_QuestionIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameQuestionID = "QuestionIDNotPresent";
            var responseOptions  = Fixture.Create<ResponseOptions>();

            // Act , Assert
            Should.NotThrow(() => responseOptions.GetType().GetProperty(propertyNameQuestionID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ResponseOptions_QuestionID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameQuestionID = "QuestionID";
            var responseOptions  = Fixture.Create<ResponseOptions>();
            var propertyInfo  = responseOptions.GetType().GetProperty(propertyNameQuestionID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : ResponseOptions => Score

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ResponseOptions_Score_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var responseOptions = Fixture.Create<ResponseOptions>();
            var intType = responseOptions.Score.GetType();

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
        public void ResponseOptions_Class_Invalid_Property_ScoreNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameScore = "ScoreNotPresent";
            var responseOptions  = Fixture.Create<ResponseOptions>();

            // Act , Assert
            Should.NotThrow(() => responseOptions.GetType().GetProperty(propertyNameScore));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ResponseOptions_Score_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameScore = "Score";
            var responseOptions  = Fixture.Create<ResponseOptions>();
            var propertyInfo  = responseOptions.GetType().GetProperty(propertyNameScore);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion


        #endregion
        #region Category : Constructor

        #region General Constructor Pattern : create and expect no exception.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ResponseOptions_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new ResponseOptions());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<ResponseOptions>(2).ToList();
            var first = myInstances.FirstOrDefault();
            var last = myInstances.Last();

            // Act, Assert
            first.ShouldNotBeNull();
            last.ShouldNotBeNull();
            first.ShouldNotBeSameAs(last);
        }

        #endregion

        #region Contructor Default Assignment Test : ResponseOptions => ResponseOptions()

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ResponseOptions_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var optionID = -1;
            var questionID = -1;
            var optionValue = string.Empty;
            var score = -1;

            // Act
            var responseOptions = new ResponseOptions();

            // Assert
            responseOptions.OptionID.ShouldBe(optionID);
            responseOptions.QuestionID.ShouldBe(questionID);
            responseOptions.OptionValue.ShouldBe(optionValue);
            responseOptions.Score.ShouldBe(score);
        }

        #endregion


        #endregion


        #endregion
    }
}