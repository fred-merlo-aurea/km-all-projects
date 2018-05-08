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
    public class SurveyBranchingTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyBranching_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var surveyBranching  = new SurveyBranching();
            var surveyID = Fixture.Create<int>();
            var questionID = Fixture.Create<int>();
            var optionID = Fixture.Create<int>();
            var pageID = Fixture.Create<int?>();
            var endSurvey = Fixture.Create<bool>();

            // Act
            surveyBranching.SurveyID = surveyID;
            surveyBranching.QuestionID = questionID;
            surveyBranching.OptionID = optionID;
            surveyBranching.PageID = pageID;
            surveyBranching.EndSurvey = endSurvey;

            // Assert
            surveyBranching.SurveyID.ShouldBe(surveyID);
            surveyBranching.QuestionID.ShouldBe(questionID);
            surveyBranching.OptionID.ShouldBe(optionID);
            surveyBranching.PageID.ShouldBe(pageID);
            surveyBranching.EndSurvey.ShouldBe(endSurvey);   
        }

        #endregion

        #region bool property type test : SurveyBranching => EndSurvey

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyBranching_EndSurvey_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var surveyBranching = Fixture.Create<SurveyBranching>();
            var boolType = surveyBranching.EndSurvey.GetType();

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyBranching_Class_Invalid_Property_EndSurveyNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEndSurvey = "EndSurveyNotPresent";
            var surveyBranching  = Fixture.Create<SurveyBranching>();

            // Act , Assert
            Should.NotThrow(() => surveyBranching.GetType().GetProperty(propertyNameEndSurvey));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyBranching_EndSurvey_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEndSurvey = "EndSurvey";
            var surveyBranching  = Fixture.Create<SurveyBranching>();
            var propertyInfo  = surveyBranching.GetType().GetProperty(propertyNameEndSurvey);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : SurveyBranching => OptionID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyBranching_OptionID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var surveyBranching = Fixture.Create<SurveyBranching>();
            var intType = surveyBranching.OptionID.GetType();

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
        public void SurveyBranching_Class_Invalid_Property_OptionIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOptionID = "OptionIDNotPresent";
            var surveyBranching  = Fixture.Create<SurveyBranching>();

            // Act , Assert
            Should.NotThrow(() => surveyBranching.GetType().GetProperty(propertyNameOptionID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyBranching_OptionID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOptionID = "OptionID";
            var surveyBranching  = Fixture.Create<SurveyBranching>();
            var propertyInfo  = surveyBranching.GetType().GetProperty(propertyNameOptionID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : SurveyBranching => PageID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyBranching_PageID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var surveyBranching = Fixture.Create<SurveyBranching>();
            var random = Fixture.Create<int>();

            // Act , Set
            surveyBranching.PageID = random;

            // Assert
            surveyBranching.PageID.ShouldBe(random);
            surveyBranching.PageID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyBranching_PageID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var surveyBranching = Fixture.Create<SurveyBranching>();    

            // Act , Set
            surveyBranching.PageID = null;

            // Assert
            surveyBranching.PageID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyBranching_PageID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNamePageID = "PageID";
            var surveyBranching = Fixture.Create<SurveyBranching>();
            var propertyInfo = surveyBranching.GetType().GetProperty(propertyNamePageID);

            // Act , Set
            propertyInfo.SetValue(surveyBranching, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            surveyBranching.PageID.ShouldBeNull();
            surveyBranching.PageID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyBranching_Class_Invalid_Property_PageIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePageID = "PageIDNotPresent";
            var surveyBranching  = Fixture.Create<SurveyBranching>();

            // Act , Assert
            Should.NotThrow(() => surveyBranching.GetType().GetProperty(propertyNamePageID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyBranching_PageID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePageID = "PageID";
            var surveyBranching  = Fixture.Create<SurveyBranching>();
            var propertyInfo  = surveyBranching.GetType().GetProperty(propertyNamePageID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : SurveyBranching => QuestionID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyBranching_QuestionID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var surveyBranching = Fixture.Create<SurveyBranching>();
            var intType = surveyBranching.QuestionID.GetType();

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
        public void SurveyBranching_Class_Invalid_Property_QuestionIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameQuestionID = "QuestionIDNotPresent";
            var surveyBranching  = Fixture.Create<SurveyBranching>();

            // Act , Assert
            Should.NotThrow(() => surveyBranching.GetType().GetProperty(propertyNameQuestionID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyBranching_QuestionID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameQuestionID = "QuestionID";
            var surveyBranching  = Fixture.Create<SurveyBranching>();
            var propertyInfo  = surveyBranching.GetType().GetProperty(propertyNameQuestionID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : SurveyBranching => SurveyID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyBranching_SurveyID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var surveyBranching = Fixture.Create<SurveyBranching>();
            var intType = surveyBranching.SurveyID.GetType();

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
        public void SurveyBranching_Class_Invalid_Property_SurveyIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSurveyID = "SurveyIDNotPresent";
            var surveyBranching  = Fixture.Create<SurveyBranching>();

            // Act , Assert
            Should.NotThrow(() => surveyBranching.GetType().GetProperty(propertyNameSurveyID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyBranching_SurveyID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSurveyID = "SurveyID";
            var surveyBranching  = Fixture.Create<SurveyBranching>();
            var propertyInfo  = surveyBranching.GetType().GetProperty(propertyNameSurveyID);

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
        public void Constructor_SurveyBranching_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new SurveyBranching());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<SurveyBranching>(2).ToList();
            var first = myInstances.FirstOrDefault();
            var last = myInstances.Last();

            // Act, Assert
            first.ShouldNotBeNull();
            last.ShouldNotBeNull();
            first.ShouldNotBeSameAs(last);
        }

        #endregion

        #region Contructor Default Assignment Test : SurveyBranching => SurveyBranching()

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_SurveyBranching_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var surveyID = -1;
            var questionID = -1;
            var optionID = -1;
            int? pageID = null;
            var endSurvey = false;

            // Act
            var surveyBranching = new SurveyBranching();

            // Assert
            surveyBranching.SurveyID.ShouldBe(surveyID);
            surveyBranching.QuestionID.ShouldBe(questionID);
            surveyBranching.OptionID.ShouldBe(optionID);
            surveyBranching.PageID.ShouldBeNull();
            surveyBranching.EndSurvey.ShouldBeFalse();
        }

        #endregion


        #endregion


        #endregion
    }
}