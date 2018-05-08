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
    public class GridStatementsTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GridStatements_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var gridStatements  = new GridStatements();
            var gridStatementID = Fixture.Create<int>();
            var questionID = Fixture.Create<int>();
            var gridStatement = Fixture.Create<string>();

            // Act
            gridStatements.GridStatementID = gridStatementID;
            gridStatements.QuestionID = questionID;
            gridStatements.GridStatement = gridStatement;

            // Assert
            gridStatements.GridStatementID.ShouldBe(gridStatementID);
            gridStatements.QuestionID.ShouldBe(questionID);
            gridStatements.GridStatement.ShouldBe(gridStatement);   
        }

        #endregion

        #region string property type test : GridStatements => GridStatement

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GridStatements_GridStatement_Property_String_Type_Verify_Test()
        {
            // Arrange
            var gridStatements = Fixture.Create<GridStatements>();
            var stringType = gridStatements.GridStatement.GetType();

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
        public void GridStatements_Class_Invalid_Property_GridStatementNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGridStatement = "GridStatementNotPresent";
            var gridStatements  = Fixture.Create<GridStatements>();

            // Act , Assert
            Should.NotThrow(() => gridStatements.GetType().GetProperty(propertyNameGridStatement));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GridStatements_GridStatement_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGridStatement = "GridStatement";
            var gridStatements  = Fixture.Create<GridStatements>();
            var propertyInfo  = gridStatements.GetType().GetProperty(propertyNameGridStatement);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : GridStatements => GridStatementID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GridStatements_GridStatementID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var gridStatements = Fixture.Create<GridStatements>();
            var intType = gridStatements.GridStatementID.GetType();

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
        public void GridStatements_Class_Invalid_Property_GridStatementIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGridStatementID = "GridStatementIDNotPresent";
            var gridStatements  = Fixture.Create<GridStatements>();

            // Act , Assert
            Should.NotThrow(() => gridStatements.GetType().GetProperty(propertyNameGridStatementID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GridStatements_GridStatementID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGridStatementID = "GridStatementID";
            var gridStatements  = Fixture.Create<GridStatements>();
            var propertyInfo  = gridStatements.GetType().GetProperty(propertyNameGridStatementID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : GridStatements => QuestionID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GridStatements_QuestionID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var gridStatements = Fixture.Create<GridStatements>();
            var intType = gridStatements.QuestionID.GetType();

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
        public void GridStatements_Class_Invalid_Property_QuestionIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameQuestionID = "QuestionIDNotPresent";
            var gridStatements  = Fixture.Create<GridStatements>();

            // Act , Assert
            Should.NotThrow(() => gridStatements.GetType().GetProperty(propertyNameQuestionID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GridStatements_QuestionID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameQuestionID = "QuestionID";
            var gridStatements  = Fixture.Create<GridStatements>();
            var propertyInfo  = gridStatements.GetType().GetProperty(propertyNameQuestionID);

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
        public void Constructor_GridStatements_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new GridStatements());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<GridStatements>(2).ToList();
            var first = myInstances.FirstOrDefault();
            var last = myInstances.Last();

            // Act, Assert
            first.ShouldNotBeNull();
            last.ShouldNotBeNull();
            first.ShouldNotBeSameAs(last);
        }

        #endregion

        #region Contructor Default Assignment Test : GridStatements => GridStatements()

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_GridStatements_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var gridStatementID = -1;
            var questionID = -1;
            var gridStatement = string.Empty;

            // Act
            var gridStatements = new GridStatements();

            // Assert
            gridStatements.GridStatementID.ShouldBe(gridStatementID);
            gridStatements.QuestionID.ShouldBe(questionID);
            gridStatements.GridStatement.ShouldBe(gridStatement);
        }

        #endregion


        #endregion


        #endregion
    }
}