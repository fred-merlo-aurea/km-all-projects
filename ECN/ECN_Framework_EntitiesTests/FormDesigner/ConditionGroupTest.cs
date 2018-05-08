using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.FormDesigner;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.FormDesigner
{
    [TestFixture]
    public class ConditionGroupTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (ConditionGroup) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConditionGroup_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var conditionGroup = Fixture.Create<ConditionGroup>();
            var conditionGroup_Seq_Id = Fixture.Create<int>();
            var mainGroup_Id = Fixture.Create<int?>();
            var logicGroup = Fixture.Create<bool>();
            var conditions = Fixture.Create<List<Condition>>();
            var conditionGroup1 = Fixture.Create<List<ConditionGroup>>();

            // Act
            conditionGroup.ConditionGroup_Seq_ID = conditionGroup_Seq_Id;
            conditionGroup.MainGroup_ID = mainGroup_Id;
            conditionGroup.LogicGroup = logicGroup;
            conditionGroup.Conditions = conditions;
            conditionGroup.ConditionGroup1 = conditionGroup1;

            // Assert
            conditionGroup.ConditionGroup_Seq_ID.ShouldBe(conditionGroup_Seq_Id);
            conditionGroup.MainGroup_ID.ShouldBe(mainGroup_Id);
            conditionGroup.LogicGroup.ShouldBe(logicGroup);
            conditionGroup.Conditions.ShouldBe(conditions);
            conditionGroup.ConditionGroup1.ShouldBe(conditionGroup1);
        }

        #endregion

        #region General Getters/Setters : Class (ConditionGroup) => Property (ConditionGroup_Seq_ID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConditionGroup_ConditionGroup_Seq_ID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var conditionGroup = Fixture.Create<ConditionGroup>();
            conditionGroup.ConditionGroup_Seq_ID = Fixture.Create<int>();
            var intType = conditionGroup.ConditionGroup_Seq_ID.GetType();

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

        #region General Getters/Setters : Class (ConditionGroup) => Property (ConditionGroup_Seq_ID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConditionGroup_Class_Invalid_Property_ConditionGroup_Seq_IDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameConditionGroup_Seq_ID = "ConditionGroup_Seq_IDNotPresent";
            var conditionGroup  = Fixture.Create<ConditionGroup>();

            // Act , Assert
            Should.NotThrow(() => conditionGroup.GetType().GetProperty(propertyNameConditionGroup_Seq_ID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConditionGroup_ConditionGroup_Seq_ID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameConditionGroup_Seq_ID = "ConditionGroup_Seq_ID";
            var conditionGroup  = Fixture.Create<ConditionGroup>();
            var propertyInfo  = conditionGroup.GetType().GetProperty(propertyNameConditionGroup_Seq_ID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ConditionGroup) => Property (ConditionGroup1) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConditionGroup_Class_Invalid_Property_ConditionGroup1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameConditionGroup1 = "ConditionGroup1NotPresent";
            var conditionGroup  = Fixture.Create<ConditionGroup>();

            // Act , Assert
            Should.NotThrow(() => conditionGroup.GetType().GetProperty(propertyNameConditionGroup1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConditionGroup_ConditionGroup1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameConditionGroup1 = "ConditionGroup1";
            var conditionGroup  = Fixture.Create<ConditionGroup>();
            var propertyInfo  = conditionGroup.GetType().GetProperty(propertyNameConditionGroup1);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ConditionGroup) => Property (Conditions) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConditionGroup_Class_Invalid_Property_ConditionsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameConditions = "ConditionsNotPresent";
            var conditionGroup  = Fixture.Create<ConditionGroup>();

            // Act , Assert
            Should.NotThrow(() => conditionGroup.GetType().GetProperty(propertyNameConditions));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConditionGroup_Conditions_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameConditions = "Conditions";
            var conditionGroup  = Fixture.Create<ConditionGroup>();
            var propertyInfo  = conditionGroup.GetType().GetProperty(propertyNameConditions);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ConditionGroup) => Property (LogicGroup) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConditionGroup_LogicGroup_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var conditionGroup = Fixture.Create<ConditionGroup>();
            conditionGroup.LogicGroup = Fixture.Create<bool>();
            var boolType = conditionGroup.LogicGroup.GetType();

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

        #region General Getters/Setters : Class (ConditionGroup) => Property (LogicGroup) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConditionGroup_Class_Invalid_Property_LogicGroupNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLogicGroup = "LogicGroupNotPresent";
            var conditionGroup  = Fixture.Create<ConditionGroup>();

            // Act , Assert
            Should.NotThrow(() => conditionGroup.GetType().GetProperty(propertyNameLogicGroup));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConditionGroup_LogicGroup_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLogicGroup = "LogicGroup";
            var conditionGroup  = Fixture.Create<ConditionGroup>();
            var propertyInfo  = conditionGroup.GetType().GetProperty(propertyNameLogicGroup);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ConditionGroup) => Property (MainGroup_ID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConditionGroup_MainGroup_ID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var conditionGroup = Fixture.Create<ConditionGroup>();
            var random = Fixture.Create<int>();

            // Act , Set
            conditionGroup.MainGroup_ID = random;

            // Assert
            conditionGroup.MainGroup_ID.ShouldBe(random);
            conditionGroup.MainGroup_ID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConditionGroup_MainGroup_ID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var conditionGroup = Fixture.Create<ConditionGroup>();

            // Act , Set
            conditionGroup.MainGroup_ID = null;

            // Assert
            conditionGroup.MainGroup_ID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConditionGroup_MainGroup_ID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameMainGroup_ID = "MainGroup_ID";
            var conditionGroup = Fixture.Create<ConditionGroup>();
            var propertyInfo = conditionGroup.GetType().GetProperty(propertyNameMainGroup_ID);

            // Act , Set
            propertyInfo.SetValue(conditionGroup, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            conditionGroup.MainGroup_ID.ShouldBeNull();
            conditionGroup.MainGroup_ID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ConditionGroup) => Property (MainGroup_ID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConditionGroup_Class_Invalid_Property_MainGroup_IDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMainGroup_ID = "MainGroup_IDNotPresent";
            var conditionGroup  = Fixture.Create<ConditionGroup>();

            // Act , Assert
            Should.NotThrow(() => conditionGroup.GetType().GetProperty(propertyNameMainGroup_ID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConditionGroup_MainGroup_ID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMainGroup_ID = "MainGroup_ID";
            var conditionGroup  = Fixture.Create<ConditionGroup>();
            var propertyInfo  = conditionGroup.GetType().GetProperty(propertyNameMainGroup_ID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (ConditionGroup) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ConditionGroup_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new ConditionGroup());
        }

        #endregion

        #region General Constructor : Class (ConditionGroup) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ConditionGroup_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfConditionGroup = Fixture.CreateMany<ConditionGroup>(2).ToList();
            var firstConditionGroup = instancesOfConditionGroup.FirstOrDefault();
            var lastConditionGroup = instancesOfConditionGroup.Last();

            // Act, Assert
            firstConditionGroup.ShouldNotBeNull();
            lastConditionGroup.ShouldNotBeNull();
            firstConditionGroup.ShouldNotBeSameAs(lastConditionGroup);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ConditionGroup_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstConditionGroup = new ConditionGroup();
            var secondConditionGroup = new ConditionGroup();
            var thirdConditionGroup = new ConditionGroup();
            var fourthConditionGroup = new ConditionGroup();
            var fifthConditionGroup = new ConditionGroup();
            var sixthConditionGroup = new ConditionGroup();

            // Act, Assert
            firstConditionGroup.ShouldNotBeNull();
            secondConditionGroup.ShouldNotBeNull();
            thirdConditionGroup.ShouldNotBeNull();
            fourthConditionGroup.ShouldNotBeNull();
            fifthConditionGroup.ShouldNotBeNull();
            sixthConditionGroup.ShouldNotBeNull();
            firstConditionGroup.ShouldNotBeSameAs(secondConditionGroup);
            thirdConditionGroup.ShouldNotBeSameAs(firstConditionGroup);
            fourthConditionGroup.ShouldNotBeSameAs(firstConditionGroup);
            fifthConditionGroup.ShouldNotBeSameAs(firstConditionGroup);
            sixthConditionGroup.ShouldNotBeSameAs(firstConditionGroup);
            sixthConditionGroup.ShouldNotBeSameAs(fourthConditionGroup);
        }

        #endregion

        #region General Constructor : Class (ConditionGroup) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ConditionGroup_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var conditions = new List<Condition>();
            var conditionGroup1 = new List<ConditionGroup>();
            var conditionGroup_Seq_Id = -1;

            // Act
            var conditionGroup = new ConditionGroup();

            // Assert
            conditionGroup.Conditions.ShouldBeEmpty();
            conditionGroup.ConditionGroup1.ShouldBeEmpty();
            conditionGroup.ConditionGroup_Seq_ID.ShouldBe(conditionGroup_Seq_Id);
        }

        #endregion

        #endregion

        #endregion
    }
}