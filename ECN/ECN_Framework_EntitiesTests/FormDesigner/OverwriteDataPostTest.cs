using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.FormDesigner;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.FormDesigner
{
    [TestFixture]
    public class OverwriteDataPostTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (OverwriteDataPost) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void OverwriteDataPost_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var overwriteDataPost = Fixture.Create<OverwriteDataPost>();
            var overwritedataValue_Seq_Id = Fixture.Create<int>();
            var control_Id = Fixture.Create<int>();
            var rule_Seq_Id = Fixture.Create<int>();
            var value = Fixture.Create<string>();

            // Act
            overwriteDataPost.OverwritedataValue_Seq_ID = overwritedataValue_Seq_Id;
            overwriteDataPost.Control_ID = control_Id;
            overwriteDataPost.Rule_Seq_ID = rule_Seq_Id;
            overwriteDataPost.Value = value;

            // Assert
            overwriteDataPost.OverwritedataValue_Seq_ID.ShouldBe(overwritedataValue_Seq_Id);
            overwriteDataPost.Control_ID.ShouldBe(control_Id);
            overwriteDataPost.Rule_Seq_ID.ShouldBe(rule_Seq_Id);
            overwriteDataPost.Value.ShouldBe(value);
        }

        #endregion

        #region General Getters/Setters : Class (OverwriteDataPost) => Property (Control_ID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void OverwriteDataPost_Control_ID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var overwriteDataPost = Fixture.Create<OverwriteDataPost>();
            overwriteDataPost.Control_ID = Fixture.Create<int>();
            var intType = overwriteDataPost.Control_ID.GetType();

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

        #region General Getters/Setters : Class (OverwriteDataPost) => Property (Control_ID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void OverwriteDataPost_Class_Invalid_Property_Control_IDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameControl_ID = "Control_IDNotPresent";
            var overwriteDataPost  = Fixture.Create<OverwriteDataPost>();

            // Act , Assert
            Should.NotThrow(() => overwriteDataPost.GetType().GetProperty(propertyNameControl_ID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void OverwriteDataPost_Control_ID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameControl_ID = "Control_ID";
            var overwriteDataPost  = Fixture.Create<OverwriteDataPost>();
            var propertyInfo  = overwriteDataPost.GetType().GetProperty(propertyNameControl_ID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (OverwriteDataPost) => Property (OverwritedataValue_Seq_ID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void OverwriteDataPost_OverwritedataValue_Seq_ID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var overwriteDataPost = Fixture.Create<OverwriteDataPost>();
            overwriteDataPost.OverwritedataValue_Seq_ID = Fixture.Create<int>();
            var intType = overwriteDataPost.OverwritedataValue_Seq_ID.GetType();

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

        #region General Getters/Setters : Class (OverwriteDataPost) => Property (OverwritedataValue_Seq_ID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void OverwriteDataPost_Class_Invalid_Property_OverwritedataValue_Seq_IDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOverwritedataValue_Seq_ID = "OverwritedataValue_Seq_IDNotPresent";
            var overwriteDataPost  = Fixture.Create<OverwriteDataPost>();

            // Act , Assert
            Should.NotThrow(() => overwriteDataPost.GetType().GetProperty(propertyNameOverwritedataValue_Seq_ID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void OverwriteDataPost_OverwritedataValue_Seq_ID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOverwritedataValue_Seq_ID = "OverwritedataValue_Seq_ID";
            var overwriteDataPost  = Fixture.Create<OverwriteDataPost>();
            var propertyInfo  = overwriteDataPost.GetType().GetProperty(propertyNameOverwritedataValue_Seq_ID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (OverwriteDataPost) => Property (Rule_Seq_ID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void OverwriteDataPost_Rule_Seq_ID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var overwriteDataPost = Fixture.Create<OverwriteDataPost>();
            overwriteDataPost.Rule_Seq_ID = Fixture.Create<int>();
            var intType = overwriteDataPost.Rule_Seq_ID.GetType();

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

        #region General Getters/Setters : Class (OverwriteDataPost) => Property (Rule_Seq_ID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void OverwriteDataPost_Class_Invalid_Property_Rule_Seq_IDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRule_Seq_ID = "Rule_Seq_IDNotPresent";
            var overwriteDataPost  = Fixture.Create<OverwriteDataPost>();

            // Act , Assert
            Should.NotThrow(() => overwriteDataPost.GetType().GetProperty(propertyNameRule_Seq_ID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void OverwriteDataPost_Rule_Seq_ID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRule_Seq_ID = "Rule_Seq_ID";
            var overwriteDataPost  = Fixture.Create<OverwriteDataPost>();
            var propertyInfo  = overwriteDataPost.GetType().GetProperty(propertyNameRule_Seq_ID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (OverwriteDataPost) => Property (Value) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void OverwriteDataPost_Value_Property_String_Type_Verify_Test()
        {
            // Arrange
            var overwriteDataPost = Fixture.Create<OverwriteDataPost>();
            overwriteDataPost.Value = Fixture.Create<string>();
            var stringType = overwriteDataPost.Value.GetType();

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

        #region General Getters/Setters : Class (OverwriteDataPost) => Property (Value) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void OverwriteDataPost_Class_Invalid_Property_ValueNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameValue = "ValueNotPresent";
            var overwriteDataPost  = Fixture.Create<OverwriteDataPost>();

            // Act , Assert
            Should.NotThrow(() => overwriteDataPost.GetType().GetProperty(propertyNameValue));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void OverwriteDataPost_Value_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameValue = "Value";
            var overwriteDataPost  = Fixture.Create<OverwriteDataPost>();
            var propertyInfo  = overwriteDataPost.GetType().GetProperty(propertyNameValue);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (OverwriteDataPost) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_OverwriteDataPost_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new OverwriteDataPost());
        }

        #endregion

        #region General Constructor : Class (OverwriteDataPost) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_OverwriteDataPost_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfOverwriteDataPost = Fixture.CreateMany<OverwriteDataPost>(2).ToList();
            var firstOverwriteDataPost = instancesOfOverwriteDataPost.FirstOrDefault();
            var lastOverwriteDataPost = instancesOfOverwriteDataPost.Last();

            // Act, Assert
            firstOverwriteDataPost.ShouldNotBeNull();
            lastOverwriteDataPost.ShouldNotBeNull();
            firstOverwriteDataPost.ShouldNotBeSameAs(lastOverwriteDataPost);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_OverwriteDataPost_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstOverwriteDataPost = new OverwriteDataPost();
            var secondOverwriteDataPost = new OverwriteDataPost();
            var thirdOverwriteDataPost = new OverwriteDataPost();
            var fourthOverwriteDataPost = new OverwriteDataPost();
            var fifthOverwriteDataPost = new OverwriteDataPost();
            var sixthOverwriteDataPost = new OverwriteDataPost();

            // Act, Assert
            firstOverwriteDataPost.ShouldNotBeNull();
            secondOverwriteDataPost.ShouldNotBeNull();
            thirdOverwriteDataPost.ShouldNotBeNull();
            fourthOverwriteDataPost.ShouldNotBeNull();
            fifthOverwriteDataPost.ShouldNotBeNull();
            sixthOverwriteDataPost.ShouldNotBeNull();
            firstOverwriteDataPost.ShouldNotBeSameAs(secondOverwriteDataPost);
            thirdOverwriteDataPost.ShouldNotBeSameAs(firstOverwriteDataPost);
            fourthOverwriteDataPost.ShouldNotBeSameAs(firstOverwriteDataPost);
            fifthOverwriteDataPost.ShouldNotBeSameAs(firstOverwriteDataPost);
            sixthOverwriteDataPost.ShouldNotBeSameAs(firstOverwriteDataPost);
            sixthOverwriteDataPost.ShouldNotBeSameAs(fourthOverwriteDataPost);
        }

        #endregion

        #endregion

        #endregion
    }
}