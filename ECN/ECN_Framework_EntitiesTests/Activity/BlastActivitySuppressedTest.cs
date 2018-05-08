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
using ECN_Framework_Entities.Activity;

namespace ECN_Framework_Entities.Activity
{
    [TestFixture]
    public class BlastActivitySuppressedTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastActivitySuppressed) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySuppressed_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastActivitySuppressed = Fixture.Create<BlastActivitySuppressed>();
            var suppressId = Fixture.Create<int>();
            var blastId = Fixture.Create<int>();
            var emailId = Fixture.Create<int>();
            var suppressedTime = Fixture.Create<DateTime>();
            var suppressedCodeId = Fixture.Create<int>();
            var comments = Fixture.Create<string>();
            var eAId = Fixture.Create<int>();

            // Act
            blastActivitySuppressed.SuppressID = suppressId;
            blastActivitySuppressed.BlastID = blastId;
            blastActivitySuppressed.EmailID = emailId;
            blastActivitySuppressed.SuppressedTime = suppressedTime;
            blastActivitySuppressed.SuppressedCodeID = suppressedCodeId;
            blastActivitySuppressed.Comments = comments;
            blastActivitySuppressed.EAID = eAId;

            // Assert
            blastActivitySuppressed.SuppressID.ShouldBe(suppressId);
            blastActivitySuppressed.BlastID.ShouldBe(blastId);
            blastActivitySuppressed.EmailID.ShouldBe(emailId);
            blastActivitySuppressed.SuppressedTime.ShouldBe(suppressedTime);
            blastActivitySuppressed.SuppressedCodeID.ShouldBe(suppressedCodeId);
            blastActivitySuppressed.Comments.ShouldBe(comments);
            blastActivitySuppressed.EAID.ShouldBe(eAId);
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySuppressed) => Property (BlastID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySuppressed_BlastID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivitySuppressed = Fixture.Create<BlastActivitySuppressed>();
            blastActivitySuppressed.BlastID = Fixture.Create<int>();
            var intType = blastActivitySuppressed.BlastID.GetType();

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

        #region General Getters/Setters : Class (BlastActivitySuppressed) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySuppressed_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var blastActivitySuppressed  = Fixture.Create<BlastActivitySuppressed>();

            // Act , Assert
            Should.NotThrow(() => blastActivitySuppressed.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySuppressed_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var blastActivitySuppressed  = Fixture.Create<BlastActivitySuppressed>();
            var propertyInfo  = blastActivitySuppressed.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySuppressed) => Property (Comments) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySuppressed_Comments_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastActivitySuppressed = Fixture.Create<BlastActivitySuppressed>();
            blastActivitySuppressed.Comments = Fixture.Create<string>();
            var stringType = blastActivitySuppressed.Comments.GetType();

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

        #region General Getters/Setters : Class (BlastActivitySuppressed) => Property (Comments) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySuppressed_Class_Invalid_Property_CommentsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameComments = "CommentsNotPresent";
            var blastActivitySuppressed  = Fixture.Create<BlastActivitySuppressed>();

            // Act , Assert
            Should.NotThrow(() => blastActivitySuppressed.GetType().GetProperty(propertyNameComments));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySuppressed_Comments_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameComments = "Comments";
            var blastActivitySuppressed  = Fixture.Create<BlastActivitySuppressed>();
            var propertyInfo  = blastActivitySuppressed.GetType().GetProperty(propertyNameComments);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySuppressed) => Property (EAID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySuppressed_EAID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivitySuppressed = Fixture.Create<BlastActivitySuppressed>();
            blastActivitySuppressed.EAID = Fixture.Create<int>();
            var intType = blastActivitySuppressed.EAID.GetType();

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

        #region General Getters/Setters : Class (BlastActivitySuppressed) => Property (EAID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySuppressed_Class_Invalid_Property_EAIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEAID = "EAIDNotPresent";
            var blastActivitySuppressed  = Fixture.Create<BlastActivitySuppressed>();

            // Act , Assert
            Should.NotThrow(() => blastActivitySuppressed.GetType().GetProperty(propertyNameEAID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySuppressed_EAID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEAID = "EAID";
            var blastActivitySuppressed  = Fixture.Create<BlastActivitySuppressed>();
            var propertyInfo  = blastActivitySuppressed.GetType().GetProperty(propertyNameEAID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySuppressed) => Property (EmailID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySuppressed_EmailID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivitySuppressed = Fixture.Create<BlastActivitySuppressed>();
            blastActivitySuppressed.EmailID = Fixture.Create<int>();
            var intType = blastActivitySuppressed.EmailID.GetType();

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

        #region General Getters/Setters : Class (BlastActivitySuppressed) => Property (EmailID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySuppressed_Class_Invalid_Property_EmailIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailIDNotPresent";
            var blastActivitySuppressed  = Fixture.Create<BlastActivitySuppressed>();

            // Act , Assert
            Should.NotThrow(() => blastActivitySuppressed.GetType().GetProperty(propertyNameEmailID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySuppressed_EmailID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailID";
            var blastActivitySuppressed  = Fixture.Create<BlastActivitySuppressed>();
            var propertyInfo  = blastActivitySuppressed.GetType().GetProperty(propertyNameEmailID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySuppressed) => Property (SuppressedCodeID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySuppressed_SuppressedCodeID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivitySuppressed = Fixture.Create<BlastActivitySuppressed>();
            blastActivitySuppressed.SuppressedCodeID = Fixture.Create<int>();
            var intType = blastActivitySuppressed.SuppressedCodeID.GetType();

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

        #region General Getters/Setters : Class (BlastActivitySuppressed) => Property (SuppressedCodeID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySuppressed_Class_Invalid_Property_SuppressedCodeIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSuppressedCodeID = "SuppressedCodeIDNotPresent";
            var blastActivitySuppressed  = Fixture.Create<BlastActivitySuppressed>();

            // Act , Assert
            Should.NotThrow(() => blastActivitySuppressed.GetType().GetProperty(propertyNameSuppressedCodeID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySuppressed_SuppressedCodeID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSuppressedCodeID = "SuppressedCodeID";
            var blastActivitySuppressed  = Fixture.Create<BlastActivitySuppressed>();
            var propertyInfo  = blastActivitySuppressed.GetType().GetProperty(propertyNameSuppressedCodeID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySuppressed) => Property (SuppressedTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySuppressed_SuppressedTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSuppressedTime = "SuppressedTime";
            var blastActivitySuppressed = Fixture.Create<BlastActivitySuppressed>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastActivitySuppressed.GetType().GetProperty(propertyNameSuppressedTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastActivitySuppressed, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySuppressed) => Property (SuppressedTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySuppressed_Class_Invalid_Property_SuppressedTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSuppressedTime = "SuppressedTimeNotPresent";
            var blastActivitySuppressed  = Fixture.Create<BlastActivitySuppressed>();

            // Act , Assert
            Should.NotThrow(() => blastActivitySuppressed.GetType().GetProperty(propertyNameSuppressedTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySuppressed_SuppressedTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSuppressedTime = "SuppressedTime";
            var blastActivitySuppressed  = Fixture.Create<BlastActivitySuppressed>();
            var propertyInfo  = blastActivitySuppressed.GetType().GetProperty(propertyNameSuppressedTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySuppressed) => Property (SuppressID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySuppressed_SuppressID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivitySuppressed = Fixture.Create<BlastActivitySuppressed>();
            blastActivitySuppressed.SuppressID = Fixture.Create<int>();
            var intType = blastActivitySuppressed.SuppressID.GetType();

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

        #region General Getters/Setters : Class (BlastActivitySuppressed) => Property (SuppressID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySuppressed_Class_Invalid_Property_SuppressIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSuppressID = "SuppressIDNotPresent";
            var blastActivitySuppressed  = Fixture.Create<BlastActivitySuppressed>();

            // Act , Assert
            Should.NotThrow(() => blastActivitySuppressed.GetType().GetProperty(propertyNameSuppressID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySuppressed_SuppressID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSuppressID = "SuppressID";
            var blastActivitySuppressed  = Fixture.Create<BlastActivitySuppressed>();
            var propertyInfo  = blastActivitySuppressed.GetType().GetProperty(propertyNameSuppressID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastActivitySuppressed) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivitySuppressed_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastActivitySuppressed());
        }

        #endregion

        #region General Constructor : Class (BlastActivitySuppressed) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivitySuppressed_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastActivitySuppressed = Fixture.CreateMany<BlastActivitySuppressed>(2).ToList();
            var firstBlastActivitySuppressed = instancesOfBlastActivitySuppressed.FirstOrDefault();
            var lastBlastActivitySuppressed = instancesOfBlastActivitySuppressed.Last();

            // Act, Assert
            firstBlastActivitySuppressed.ShouldNotBeNull();
            lastBlastActivitySuppressed.ShouldNotBeNull();
            firstBlastActivitySuppressed.ShouldNotBeSameAs(lastBlastActivitySuppressed);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivitySuppressed_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastActivitySuppressed = new BlastActivitySuppressed();
            var secondBlastActivitySuppressed = new BlastActivitySuppressed();
            var thirdBlastActivitySuppressed = new BlastActivitySuppressed();
            var fourthBlastActivitySuppressed = new BlastActivitySuppressed();
            var fifthBlastActivitySuppressed = new BlastActivitySuppressed();
            var sixthBlastActivitySuppressed = new BlastActivitySuppressed();

            // Act, Assert
            firstBlastActivitySuppressed.ShouldNotBeNull();
            secondBlastActivitySuppressed.ShouldNotBeNull();
            thirdBlastActivitySuppressed.ShouldNotBeNull();
            fourthBlastActivitySuppressed.ShouldNotBeNull();
            fifthBlastActivitySuppressed.ShouldNotBeNull();
            sixthBlastActivitySuppressed.ShouldNotBeNull();
            firstBlastActivitySuppressed.ShouldNotBeSameAs(secondBlastActivitySuppressed);
            thirdBlastActivitySuppressed.ShouldNotBeSameAs(firstBlastActivitySuppressed);
            fourthBlastActivitySuppressed.ShouldNotBeSameAs(firstBlastActivitySuppressed);
            fifthBlastActivitySuppressed.ShouldNotBeSameAs(firstBlastActivitySuppressed);
            sixthBlastActivitySuppressed.ShouldNotBeSameAs(firstBlastActivitySuppressed);
            sixthBlastActivitySuppressed.ShouldNotBeSameAs(fourthBlastActivitySuppressed);
        }

        #endregion

        #endregion

        #endregion
    }
}