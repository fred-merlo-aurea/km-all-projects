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
    public class BlastActivityUnSubscribesTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastActivityUnSubscribes) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityUnSubscribes_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastActivityUnSubscribes = Fixture.Create<BlastActivityUnSubscribes>();
            var unsubscribeId = Fixture.Create<int>();
            var blastId = Fixture.Create<int>();
            var emailId = Fixture.Create<int>();
            var unsubscribeTime = Fixture.Create<DateTime>();
            var unsubscribeCodeId = Fixture.Create<int>();
            var comments = Fixture.Create<string>();
            var eAId = Fixture.Create<int>();

            // Act
            blastActivityUnSubscribes.UnsubscribeID = unsubscribeId;
            blastActivityUnSubscribes.BlastID = blastId;
            blastActivityUnSubscribes.EmailID = emailId;
            blastActivityUnSubscribes.UnsubscribeTime = unsubscribeTime;
            blastActivityUnSubscribes.UnsubscribeCodeID = unsubscribeCodeId;
            blastActivityUnSubscribes.Comments = comments;
            blastActivityUnSubscribes.EAID = eAId;

            // Assert
            blastActivityUnSubscribes.UnsubscribeID.ShouldBe(unsubscribeId);
            blastActivityUnSubscribes.BlastID.ShouldBe(blastId);
            blastActivityUnSubscribes.EmailID.ShouldBe(emailId);
            blastActivityUnSubscribes.UnsubscribeTime.ShouldBe(unsubscribeTime);
            blastActivityUnSubscribes.UnsubscribeCodeID.ShouldBe(unsubscribeCodeId);
            blastActivityUnSubscribes.Comments.ShouldBe(comments);
            blastActivityUnSubscribes.EAID.ShouldBe(eAId);
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityUnSubscribes) => Property (BlastID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityUnSubscribes_BlastID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityUnSubscribes = Fixture.Create<BlastActivityUnSubscribes>();
            blastActivityUnSubscribes.BlastID = Fixture.Create<int>();
            var intType = blastActivityUnSubscribes.BlastID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityUnSubscribes) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityUnSubscribes_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var blastActivityUnSubscribes  = Fixture.Create<BlastActivityUnSubscribes>();

            // Act , Assert
            Should.NotThrow(() => blastActivityUnSubscribes.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityUnSubscribes_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var blastActivityUnSubscribes  = Fixture.Create<BlastActivityUnSubscribes>();
            var propertyInfo  = blastActivityUnSubscribes.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityUnSubscribes) => Property (Comments) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityUnSubscribes_Comments_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastActivityUnSubscribes = Fixture.Create<BlastActivityUnSubscribes>();
            blastActivityUnSubscribes.Comments = Fixture.Create<string>();
            var stringType = blastActivityUnSubscribes.Comments.GetType();

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

        #region General Getters/Setters : Class (BlastActivityUnSubscribes) => Property (Comments) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityUnSubscribes_Class_Invalid_Property_CommentsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameComments = "CommentsNotPresent";
            var blastActivityUnSubscribes  = Fixture.Create<BlastActivityUnSubscribes>();

            // Act , Assert
            Should.NotThrow(() => blastActivityUnSubscribes.GetType().GetProperty(propertyNameComments));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityUnSubscribes_Comments_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameComments = "Comments";
            var blastActivityUnSubscribes  = Fixture.Create<BlastActivityUnSubscribes>();
            var propertyInfo  = blastActivityUnSubscribes.GetType().GetProperty(propertyNameComments);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityUnSubscribes) => Property (EAID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityUnSubscribes_EAID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityUnSubscribes = Fixture.Create<BlastActivityUnSubscribes>();
            blastActivityUnSubscribes.EAID = Fixture.Create<int>();
            var intType = blastActivityUnSubscribes.EAID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityUnSubscribes) => Property (EAID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityUnSubscribes_Class_Invalid_Property_EAIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEAID = "EAIDNotPresent";
            var blastActivityUnSubscribes  = Fixture.Create<BlastActivityUnSubscribes>();

            // Act , Assert
            Should.NotThrow(() => blastActivityUnSubscribes.GetType().GetProperty(propertyNameEAID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityUnSubscribes_EAID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEAID = "EAID";
            var blastActivityUnSubscribes  = Fixture.Create<BlastActivityUnSubscribes>();
            var propertyInfo  = blastActivityUnSubscribes.GetType().GetProperty(propertyNameEAID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityUnSubscribes) => Property (EmailID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityUnSubscribes_EmailID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityUnSubscribes = Fixture.Create<BlastActivityUnSubscribes>();
            blastActivityUnSubscribes.EmailID = Fixture.Create<int>();
            var intType = blastActivityUnSubscribes.EmailID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityUnSubscribes) => Property (EmailID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityUnSubscribes_Class_Invalid_Property_EmailIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailIDNotPresent";
            var blastActivityUnSubscribes  = Fixture.Create<BlastActivityUnSubscribes>();

            // Act , Assert
            Should.NotThrow(() => blastActivityUnSubscribes.GetType().GetProperty(propertyNameEmailID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityUnSubscribes_EmailID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailID";
            var blastActivityUnSubscribes  = Fixture.Create<BlastActivityUnSubscribes>();
            var propertyInfo  = blastActivityUnSubscribes.GetType().GetProperty(propertyNameEmailID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityUnSubscribes) => Property (UnsubscribeCodeID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityUnSubscribes_UnsubscribeCodeID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityUnSubscribes = Fixture.Create<BlastActivityUnSubscribes>();
            blastActivityUnSubscribes.UnsubscribeCodeID = Fixture.Create<int>();
            var intType = blastActivityUnSubscribes.UnsubscribeCodeID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityUnSubscribes) => Property (UnsubscribeCodeID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityUnSubscribes_Class_Invalid_Property_UnsubscribeCodeIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUnsubscribeCodeID = "UnsubscribeCodeIDNotPresent";
            var blastActivityUnSubscribes  = Fixture.Create<BlastActivityUnSubscribes>();

            // Act , Assert
            Should.NotThrow(() => blastActivityUnSubscribes.GetType().GetProperty(propertyNameUnsubscribeCodeID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityUnSubscribes_UnsubscribeCodeID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUnsubscribeCodeID = "UnsubscribeCodeID";
            var blastActivityUnSubscribes  = Fixture.Create<BlastActivityUnSubscribes>();
            var propertyInfo  = blastActivityUnSubscribes.GetType().GetProperty(propertyNameUnsubscribeCodeID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityUnSubscribes) => Property (UnsubscribeID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityUnSubscribes_UnsubscribeID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityUnSubscribes = Fixture.Create<BlastActivityUnSubscribes>();
            blastActivityUnSubscribes.UnsubscribeID = Fixture.Create<int>();
            var intType = blastActivityUnSubscribes.UnsubscribeID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityUnSubscribes) => Property (UnsubscribeID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityUnSubscribes_Class_Invalid_Property_UnsubscribeIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUnsubscribeID = "UnsubscribeIDNotPresent";
            var blastActivityUnSubscribes  = Fixture.Create<BlastActivityUnSubscribes>();

            // Act , Assert
            Should.NotThrow(() => blastActivityUnSubscribes.GetType().GetProperty(propertyNameUnsubscribeID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityUnSubscribes_UnsubscribeID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUnsubscribeID = "UnsubscribeID";
            var blastActivityUnSubscribes  = Fixture.Create<BlastActivityUnSubscribes>();
            var propertyInfo  = blastActivityUnSubscribes.GetType().GetProperty(propertyNameUnsubscribeID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityUnSubscribes) => Property (UnsubscribeTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityUnSubscribes_UnsubscribeTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUnsubscribeTime = "UnsubscribeTime";
            var blastActivityUnSubscribes = Fixture.Create<BlastActivityUnSubscribes>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastActivityUnSubscribes.GetType().GetProperty(propertyNameUnsubscribeTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastActivityUnSubscribes, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityUnSubscribes) => Property (UnsubscribeTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityUnSubscribes_Class_Invalid_Property_UnsubscribeTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUnsubscribeTime = "UnsubscribeTimeNotPresent";
            var blastActivityUnSubscribes  = Fixture.Create<BlastActivityUnSubscribes>();

            // Act , Assert
            Should.NotThrow(() => blastActivityUnSubscribes.GetType().GetProperty(propertyNameUnsubscribeTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityUnSubscribes_UnsubscribeTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUnsubscribeTime = "UnsubscribeTime";
            var blastActivityUnSubscribes  = Fixture.Create<BlastActivityUnSubscribes>();
            var propertyInfo  = blastActivityUnSubscribes.GetType().GetProperty(propertyNameUnsubscribeTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastActivityUnSubscribes) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivityUnSubscribes_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastActivityUnSubscribes());
        }

        #endregion

        #region General Constructor : Class (BlastActivityUnSubscribes) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivityUnSubscribes_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastActivityUnSubscribes = Fixture.CreateMany<BlastActivityUnSubscribes>(2).ToList();
            var firstBlastActivityUnSubscribes = instancesOfBlastActivityUnSubscribes.FirstOrDefault();
            var lastBlastActivityUnSubscribes = instancesOfBlastActivityUnSubscribes.Last();

            // Act, Assert
            firstBlastActivityUnSubscribes.ShouldNotBeNull();
            lastBlastActivityUnSubscribes.ShouldNotBeNull();
            firstBlastActivityUnSubscribes.ShouldNotBeSameAs(lastBlastActivityUnSubscribes);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivityUnSubscribes_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastActivityUnSubscribes = new BlastActivityUnSubscribes();
            var secondBlastActivityUnSubscribes = new BlastActivityUnSubscribes();
            var thirdBlastActivityUnSubscribes = new BlastActivityUnSubscribes();
            var fourthBlastActivityUnSubscribes = new BlastActivityUnSubscribes();
            var fifthBlastActivityUnSubscribes = new BlastActivityUnSubscribes();
            var sixthBlastActivityUnSubscribes = new BlastActivityUnSubscribes();

            // Act, Assert
            firstBlastActivityUnSubscribes.ShouldNotBeNull();
            secondBlastActivityUnSubscribes.ShouldNotBeNull();
            thirdBlastActivityUnSubscribes.ShouldNotBeNull();
            fourthBlastActivityUnSubscribes.ShouldNotBeNull();
            fifthBlastActivityUnSubscribes.ShouldNotBeNull();
            sixthBlastActivityUnSubscribes.ShouldNotBeNull();
            firstBlastActivityUnSubscribes.ShouldNotBeSameAs(secondBlastActivityUnSubscribes);
            thirdBlastActivityUnSubscribes.ShouldNotBeSameAs(firstBlastActivityUnSubscribes);
            fourthBlastActivityUnSubscribes.ShouldNotBeSameAs(firstBlastActivityUnSubscribes);
            fifthBlastActivityUnSubscribes.ShouldNotBeSameAs(firstBlastActivityUnSubscribes);
            sixthBlastActivityUnSubscribes.ShouldNotBeSameAs(firstBlastActivityUnSubscribes);
            sixthBlastActivityUnSubscribes.ShouldNotBeSameAs(fourthBlastActivityUnSubscribes);
        }

        #endregion

        #endregion

        #endregion
    }
}