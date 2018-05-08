using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.FormDesigner;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.FormDesigner
{
    [TestFixture]
    public class ConditionTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (Condition) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Condition_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var condition = Fixture.Create<Condition>();
            var condition_Seq_Id = Fixture.Create<int>();
            var control_Id = Fixture.Create<int>();
            var conditionGroup_Seq_Id = Fixture.Create<int>();
            var operation_Id = Fixture.Create<int>();
            var value = Fixture.Create<string>();

            // Act
            condition.Condition_Seq_ID = condition_Seq_Id;
            condition.Control_ID = control_Id;
            condition.ConditionGroup_Seq_ID = conditionGroup_Seq_Id;
            condition.Operation_ID = operation_Id;
            condition.Value = value;

            // Assert
            condition.Condition_Seq_ID.ShouldBe(condition_Seq_Id);
            condition.Control_ID.ShouldBe(control_Id);
            condition.ConditionGroup_Seq_ID.ShouldBe(conditionGroup_Seq_Id);
            condition.Operation_ID.ShouldBe(operation_Id);
            condition.Value.ShouldBe(value);
        }

        #endregion

        #region General Getters/Setters : Class (Condition) => Property (Condition_Seq_ID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Condition_Condition_Seq_ID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var condition = Fixture.Create<Condition>();
            condition.Condition_Seq_ID = Fixture.Create<int>();
            var intType = condition.Condition_Seq_ID.GetType();

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

        #region General Getters/Setters : Class (Condition) => Property (Condition_Seq_ID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Condition_Class_Invalid_Property_Condition_Seq_IDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCondition_Seq_ID = "Condition_Seq_IDNotPresent";
            var condition  = Fixture.Create<Condition>();

            // Act , Assert
            Should.NotThrow(() => condition.GetType().GetProperty(propertyNameCondition_Seq_ID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Condition_Condition_Seq_ID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCondition_Seq_ID = "Condition_Seq_ID";
            var condition  = Fixture.Create<Condition>();
            var propertyInfo  = condition.GetType().GetProperty(propertyNameCondition_Seq_ID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Condition) => Property (ConditionGroup_Seq_ID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Condition_ConditionGroup_Seq_ID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var condition = Fixture.Create<Condition>();
            condition.ConditionGroup_Seq_ID = Fixture.Create<int>();
            var intType = condition.ConditionGroup_Seq_ID.GetType();

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

        #region General Getters/Setters : Class (Condition) => Property (ConditionGroup_Seq_ID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Condition_Class_Invalid_Property_ConditionGroup_Seq_IDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameConditionGroup_Seq_ID = "ConditionGroup_Seq_IDNotPresent";
            var condition  = Fixture.Create<Condition>();

            // Act , Assert
            Should.NotThrow(() => condition.GetType().GetProperty(propertyNameConditionGroup_Seq_ID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Condition_ConditionGroup_Seq_ID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameConditionGroup_Seq_ID = "ConditionGroup_Seq_ID";
            var condition  = Fixture.Create<Condition>();
            var propertyInfo  = condition.GetType().GetProperty(propertyNameConditionGroup_Seq_ID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Condition) => Property (Control_ID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Condition_Control_ID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var condition = Fixture.Create<Condition>();
            condition.Control_ID = Fixture.Create<int>();
            var intType = condition.Control_ID.GetType();

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

        #region General Getters/Setters : Class (Condition) => Property (Control_ID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Condition_Class_Invalid_Property_Control_IDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameControl_ID = "Control_IDNotPresent";
            var condition  = Fixture.Create<Condition>();

            // Act , Assert
            Should.NotThrow(() => condition.GetType().GetProperty(propertyNameControl_ID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Condition_Control_ID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameControl_ID = "Control_ID";
            var condition  = Fixture.Create<Condition>();
            var propertyInfo  = condition.GetType().GetProperty(propertyNameControl_ID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Condition) => Property (Operation_ID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Condition_Operation_ID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var condition = Fixture.Create<Condition>();
            condition.Operation_ID = Fixture.Create<int>();
            var intType = condition.Operation_ID.GetType();

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

        #region General Getters/Setters : Class (Condition) => Property (Operation_ID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Condition_Class_Invalid_Property_Operation_IDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOperation_ID = "Operation_IDNotPresent";
            var condition  = Fixture.Create<Condition>();

            // Act , Assert
            Should.NotThrow(() => condition.GetType().GetProperty(propertyNameOperation_ID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Condition_Operation_ID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOperation_ID = "Operation_ID";
            var condition  = Fixture.Create<Condition>();
            var propertyInfo  = condition.GetType().GetProperty(propertyNameOperation_ID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Condition) => Property (Value) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Condition_Value_Property_String_Type_Verify_Test()
        {
            // Arrange
            var condition = Fixture.Create<Condition>();
            condition.Value = Fixture.Create<string>();
            var stringType = condition.Value.GetType();

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

        #region General Getters/Setters : Class (Condition) => Property (Value) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Condition_Class_Invalid_Property_ValueNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameValue = "ValueNotPresent";
            var condition  = Fixture.Create<Condition>();

            // Act , Assert
            Should.NotThrow(() => condition.GetType().GetProperty(propertyNameValue));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Condition_Value_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameValue = "Value";
            var condition  = Fixture.Create<Condition>();
            var propertyInfo  = condition.GetType().GetProperty(propertyNameValue);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (Condition) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Condition_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new Condition());
        }

        #endregion

        #region General Constructor : Class (Condition) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Condition_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfCondition = Fixture.CreateMany<Condition>(2).ToList();
            var firstCondition = instancesOfCondition.FirstOrDefault();
            var lastCondition = instancesOfCondition.Last();

            // Act, Assert
            firstCondition.ShouldNotBeNull();
            lastCondition.ShouldNotBeNull();
            firstCondition.ShouldNotBeSameAs(lastCondition);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Condition_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstCondition = new Condition();
            var secondCondition = new Condition();
            var thirdCondition = new Condition();
            var fourthCondition = new Condition();
            var fifthCondition = new Condition();
            var sixthCondition = new Condition();

            // Act, Assert
            firstCondition.ShouldNotBeNull();
            secondCondition.ShouldNotBeNull();
            thirdCondition.ShouldNotBeNull();
            fourthCondition.ShouldNotBeNull();
            fifthCondition.ShouldNotBeNull();
            sixthCondition.ShouldNotBeNull();
            firstCondition.ShouldNotBeSameAs(secondCondition);
            thirdCondition.ShouldNotBeSameAs(firstCondition);
            fourthCondition.ShouldNotBeSameAs(firstCondition);
            fifthCondition.ShouldNotBeSameAs(firstCondition);
            sixthCondition.ShouldNotBeSameAs(firstCondition);
            sixthCondition.ShouldNotBeSameAs(fourthCondition);
        }

        #endregion

        #endregion

        #endregion
    }
}