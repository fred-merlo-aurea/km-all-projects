using System;
using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator.View;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator.View
{
    [TestFixture]
    public class LayoutPlanSummaryTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (LayoutPlanSummary) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlanSummary_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var layoutPlanSummary = Fixture.Create<LayoutPlanSummary>();
            var id = Fixture.Create<int>();
            var name = Fixture.Create<string>();
            var triggerCount = Fixture.Create<int>();

            // Act
            layoutPlanSummary.ID = id;
            layoutPlanSummary.Name = name;
            layoutPlanSummary.TriggerCount = triggerCount;

            // Assert
            layoutPlanSummary.ID.ShouldBe(id);
            layoutPlanSummary.Name.ShouldBe(name);
            layoutPlanSummary.TriggerCount.ShouldBe(triggerCount);
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlanSummary) => Property (ID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlanSummary_ID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var layoutPlanSummary = Fixture.Create<LayoutPlanSummary>();
            layoutPlanSummary.ID = Fixture.Create<int>();
            var intType = layoutPlanSummary.ID.GetType();

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

        #region General Getters/Setters : Class (LayoutPlanSummary) => Property (ID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlanSummary_Class_Invalid_Property_IDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameID = "IDNotPresent";
            var layoutPlanSummary  = Fixture.Create<LayoutPlanSummary>();

            // Act , Assert
            Should.NotThrow(() => layoutPlanSummary.GetType().GetProperty(propertyNameID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlanSummary_ID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameID = "ID";
            var layoutPlanSummary  = Fixture.Create<LayoutPlanSummary>();
            var propertyInfo  = layoutPlanSummary.GetType().GetProperty(propertyNameID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlanSummary) => Property (Name) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlanSummary_Name_Property_String_Type_Verify_Test()
        {
            // Arrange
            var layoutPlanSummary = Fixture.Create<LayoutPlanSummary>();
            layoutPlanSummary.Name = Fixture.Create<string>();
            var stringType = layoutPlanSummary.Name.GetType();

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

        #region General Getters/Setters : Class (LayoutPlanSummary) => Property (Name) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlanSummary_Class_Invalid_Property_NameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameName = "NameNotPresent";
            var layoutPlanSummary  = Fixture.Create<LayoutPlanSummary>();

            // Act , Assert
            Should.NotThrow(() => layoutPlanSummary.GetType().GetProperty(propertyNameName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlanSummary_Name_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameName = "Name";
            var layoutPlanSummary  = Fixture.Create<LayoutPlanSummary>();
            var propertyInfo  = layoutPlanSummary.GetType().GetProperty(propertyNameName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LayoutPlanSummary) => Property (TriggerCount) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlanSummary_TriggerCount_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var layoutPlanSummary = Fixture.Create<LayoutPlanSummary>();
            layoutPlanSummary.TriggerCount = Fixture.Create<int>();
            var intType = layoutPlanSummary.TriggerCount.GetType();

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

        #region General Getters/Setters : Class (LayoutPlanSummary) => Property (TriggerCount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlanSummary_Class_Invalid_Property_TriggerCountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTriggerCount = "TriggerCountNotPresent";
            var layoutPlanSummary  = Fixture.Create<LayoutPlanSummary>();

            // Act , Assert
            Should.NotThrow(() => layoutPlanSummary.GetType().GetProperty(propertyNameTriggerCount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LayoutPlanSummary_TriggerCount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTriggerCount = "TriggerCount";
            var layoutPlanSummary  = Fixture.Create<LayoutPlanSummary>();
            var propertyInfo  = layoutPlanSummary.GetType().GetProperty(propertyNameTriggerCount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (LayoutPlanSummary) with Parameter Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LayoutPlanSummary_Instantiated_With_Parameter_No_Exception_Thrown_Test()
        {
            // Arrange
            var id = Fixture.Create<int>();
            var name = Fixture.Create<string>();
            var triggerCount = Fixture.Create<int>();

            // Act, Assert
            Should.NotThrow(() => new LayoutPlanSummary(id, name, triggerCount));
        }

        #endregion

        #region General Constructor : Class (LayoutPlanSummary) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LayoutPlanSummary_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfLayoutPlanSummary = Fixture.CreateMany<LayoutPlanSummary>(2).ToList();
            var firstLayoutPlanSummary = instancesOfLayoutPlanSummary.FirstOrDefault();
            var lastLayoutPlanSummary = instancesOfLayoutPlanSummary.Last();

            // Act, Assert
            firstLayoutPlanSummary.ShouldNotBeNull();
            lastLayoutPlanSummary.ShouldNotBeNull();
            firstLayoutPlanSummary.ShouldNotBeSameAs(lastLayoutPlanSummary);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LayoutPlanSummary_5_Objects_Creation_3_Paramters_Test()
        {
            // Arrange
        	var id = Fixture.Create<int>();
        	var name = Fixture.Create<string>();
        	var triggerCount = Fixture.Create<int>();
            var firstLayoutPlanSummary = new LayoutPlanSummary(id, name, triggerCount);
            var secondLayoutPlanSummary = new LayoutPlanSummary(id, name, triggerCount);
            var thirdLayoutPlanSummary = new LayoutPlanSummary(id, name, triggerCount);
            var fourthLayoutPlanSummary = new LayoutPlanSummary(id, name, triggerCount);
            var fifthLayoutPlanSummary = new LayoutPlanSummary(id, name, triggerCount);
            var sixthLayoutPlanSummary = new LayoutPlanSummary(id, name, triggerCount);

            // Act, Assert
            firstLayoutPlanSummary.ShouldNotBeNull();
            secondLayoutPlanSummary.ShouldNotBeNull();
            thirdLayoutPlanSummary.ShouldNotBeNull();
            fourthLayoutPlanSummary.ShouldNotBeNull();
            fifthLayoutPlanSummary.ShouldNotBeNull();
            sixthLayoutPlanSummary.ShouldNotBeNull();
            firstLayoutPlanSummary.ShouldNotBeSameAs(secondLayoutPlanSummary);
            thirdLayoutPlanSummary.ShouldNotBeSameAs(firstLayoutPlanSummary);
            fourthLayoutPlanSummary.ShouldNotBeSameAs(firstLayoutPlanSummary);
            fifthLayoutPlanSummary.ShouldNotBeSameAs(firstLayoutPlanSummary);
            sixthLayoutPlanSummary.ShouldNotBeSameAs(firstLayoutPlanSummary);
            sixthLayoutPlanSummary.ShouldNotBeSameAs(fourthLayoutPlanSummary);
        }

        #endregion

        #endregion

        #endregion
    }
}