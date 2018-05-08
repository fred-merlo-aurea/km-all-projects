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
using ECN_Framework_Entities.Activity.Report;

namespace ECN_Framework_Entities.Activity.Report
{
    [TestFixture]
    public class BounceByDomainTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BounceByDomain) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BounceByDomain_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var bounceByDomain = Fixture.Create<BounceByDomain>();
            var domain = Fixture.Create<string>();
            var totalBounces = Fixture.Create<int>();
            var hard = Fixture.Create<int>();
            var soft = Fixture.Create<int>();
            var other = Fixture.Create<int>();
            var messagesSent = Fixture.Create<int>();
            var percBounced = Fixture.Create<float>();

            // Act
            bounceByDomain.Domain = domain;
            bounceByDomain.TotalBounces = totalBounces;
            bounceByDomain.Hard = hard;
            bounceByDomain.Soft = soft;
            bounceByDomain.Other = other;
            bounceByDomain.MessagesSent = messagesSent;
            bounceByDomain.PercBounced = percBounced;

            // Assert
            bounceByDomain.Domain.ShouldBe(domain);
            bounceByDomain.TotalBounces.ShouldBe(totalBounces);
            bounceByDomain.Hard.ShouldBe(hard);
            bounceByDomain.Soft.ShouldBe(soft);
            bounceByDomain.Other.ShouldBe(other);
            bounceByDomain.MessagesSent.ShouldBe(messagesSent);
            bounceByDomain.PercBounced.ShouldBe(percBounced);
        }

        #endregion

        #region General Getters/Setters : Class (BounceByDomain) => Property (Domain) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BounceByDomain_Domain_Property_String_Type_Verify_Test()
        {
            // Arrange
            var bounceByDomain = Fixture.Create<BounceByDomain>();
            bounceByDomain.Domain = Fixture.Create<string>();
            var stringType = bounceByDomain.Domain.GetType();

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

        #region General Getters/Setters : Class (BounceByDomain) => Property (Domain) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BounceByDomain_Class_Invalid_Property_DomainNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDomain = "DomainNotPresent";
            var bounceByDomain  = Fixture.Create<BounceByDomain>();

            // Act , Assert
            Should.NotThrow(() => bounceByDomain.GetType().GetProperty(propertyNameDomain));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BounceByDomain_Domain_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDomain = "Domain";
            var bounceByDomain  = Fixture.Create<BounceByDomain>();
            var propertyInfo  = bounceByDomain.GetType().GetProperty(propertyNameDomain);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BounceByDomain) => Property (Hard) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BounceByDomain_Hard_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var bounceByDomain = Fixture.Create<BounceByDomain>();
            bounceByDomain.Hard = Fixture.Create<int>();
            var intType = bounceByDomain.Hard.GetType();

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

        #region General Getters/Setters : Class (BounceByDomain) => Property (Hard) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BounceByDomain_Class_Invalid_Property_HardNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameHard = "HardNotPresent";
            var bounceByDomain  = Fixture.Create<BounceByDomain>();

            // Act , Assert
            Should.NotThrow(() => bounceByDomain.GetType().GetProperty(propertyNameHard));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BounceByDomain_Hard_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameHard = "Hard";
            var bounceByDomain  = Fixture.Create<BounceByDomain>();
            var propertyInfo  = bounceByDomain.GetType().GetProperty(propertyNameHard);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BounceByDomain) => Property (MessagesSent) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BounceByDomain_MessagesSent_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var bounceByDomain = Fixture.Create<BounceByDomain>();
            bounceByDomain.MessagesSent = Fixture.Create<int>();
            var intType = bounceByDomain.MessagesSent.GetType();

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

        #region General Getters/Setters : Class (BounceByDomain) => Property (MessagesSent) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BounceByDomain_Class_Invalid_Property_MessagesSentNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMessagesSent = "MessagesSentNotPresent";
            var bounceByDomain  = Fixture.Create<BounceByDomain>();

            // Act , Assert
            Should.NotThrow(() => bounceByDomain.GetType().GetProperty(propertyNameMessagesSent));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BounceByDomain_MessagesSent_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMessagesSent = "MessagesSent";
            var bounceByDomain  = Fixture.Create<BounceByDomain>();
            var propertyInfo  = bounceByDomain.GetType().GetProperty(propertyNameMessagesSent);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BounceByDomain) => Property (Other) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BounceByDomain_Other_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var bounceByDomain = Fixture.Create<BounceByDomain>();
            bounceByDomain.Other = Fixture.Create<int>();
            var intType = bounceByDomain.Other.GetType();

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

        #region General Getters/Setters : Class (BounceByDomain) => Property (Other) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BounceByDomain_Class_Invalid_Property_OtherNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOther = "OtherNotPresent";
            var bounceByDomain  = Fixture.Create<BounceByDomain>();

            // Act , Assert
            Should.NotThrow(() => bounceByDomain.GetType().GetProperty(propertyNameOther));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BounceByDomain_Other_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOther = "Other";
            var bounceByDomain  = Fixture.Create<BounceByDomain>();
            var propertyInfo  = bounceByDomain.GetType().GetProperty(propertyNameOther);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BounceByDomain) => Property (PercBounced) (Type : float) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BounceByDomain_PercBounced_Property_Float_Type_Verify_Test()
        {
            // Arrange
            var bounceByDomain = Fixture.Create<BounceByDomain>();
            bounceByDomain.PercBounced = Fixture.Create<float>();
            var floatType = bounceByDomain.PercBounced.GetType();

            // Act
            var isTypeFloat = typeof(float) == (floatType);
            var isTypeNullableFloat = typeof(float?) == (floatType);
            var isTypeString = typeof(string) == (floatType);
            var isTypeInt = typeof(int) == (floatType);
            var isTypeDecimal = typeof(decimal) == (floatType);
            var isTypeLong = typeof(long) == (floatType);
            var isTypeBool = typeof(bool) == (floatType);
            var isTypeDouble = typeof(double) == (floatType);
            var isTypeIntNullable = typeof(int?) == (floatType);
            var isTypeDecimalNullable = typeof(decimal?) == (floatType);
            var isTypeLongNullable = typeof(long?) == (floatType);
            var isTypeBoolNullable = typeof(bool?) == (floatType);
            var isTypeDoubleNullable = typeof(double?) == (floatType);

            // Assert
            isTypeFloat.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableFloat.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (BounceByDomain) => Property (PercBounced) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BounceByDomain_Class_Invalid_Property_PercBouncedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePercBounced = "PercBouncedNotPresent";
            var bounceByDomain  = Fixture.Create<BounceByDomain>();

            // Act , Assert
            Should.NotThrow(() => bounceByDomain.GetType().GetProperty(propertyNamePercBounced));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BounceByDomain_PercBounced_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePercBounced = "PercBounced";
            var bounceByDomain  = Fixture.Create<BounceByDomain>();
            var propertyInfo  = bounceByDomain.GetType().GetProperty(propertyNamePercBounced);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BounceByDomain) => Property (Soft) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BounceByDomain_Soft_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var bounceByDomain = Fixture.Create<BounceByDomain>();
            bounceByDomain.Soft = Fixture.Create<int>();
            var intType = bounceByDomain.Soft.GetType();

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

        #region General Getters/Setters : Class (BounceByDomain) => Property (Soft) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BounceByDomain_Class_Invalid_Property_SoftNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSoft = "SoftNotPresent";
            var bounceByDomain  = Fixture.Create<BounceByDomain>();

            // Act , Assert
            Should.NotThrow(() => bounceByDomain.GetType().GetProperty(propertyNameSoft));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BounceByDomain_Soft_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSoft = "Soft";
            var bounceByDomain  = Fixture.Create<BounceByDomain>();
            var propertyInfo  = bounceByDomain.GetType().GetProperty(propertyNameSoft);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BounceByDomain) => Property (TotalBounces) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BounceByDomain_TotalBounces_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var bounceByDomain = Fixture.Create<BounceByDomain>();
            bounceByDomain.TotalBounces = Fixture.Create<int>();
            var intType = bounceByDomain.TotalBounces.GetType();

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

        #region General Getters/Setters : Class (BounceByDomain) => Property (TotalBounces) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BounceByDomain_Class_Invalid_Property_TotalBouncesNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalBounces = "TotalBouncesNotPresent";
            var bounceByDomain  = Fixture.Create<BounceByDomain>();

            // Act , Assert
            Should.NotThrow(() => bounceByDomain.GetType().GetProperty(propertyNameTotalBounces));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BounceByDomain_TotalBounces_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotalBounces = "TotalBounces";
            var bounceByDomain  = Fixture.Create<BounceByDomain>();
            var propertyInfo  = bounceByDomain.GetType().GetProperty(propertyNameTotalBounces);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BounceByDomain) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BounceByDomain_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BounceByDomain());
        }

        #endregion

        #region General Constructor : Class (BounceByDomain) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BounceByDomain_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBounceByDomain = Fixture.CreateMany<BounceByDomain>(2).ToList();
            var firstBounceByDomain = instancesOfBounceByDomain.FirstOrDefault();
            var lastBounceByDomain = instancesOfBounceByDomain.Last();

            // Act, Assert
            firstBounceByDomain.ShouldNotBeNull();
            lastBounceByDomain.ShouldNotBeNull();
            firstBounceByDomain.ShouldNotBeSameAs(lastBounceByDomain);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BounceByDomain_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBounceByDomain = new BounceByDomain();
            var secondBounceByDomain = new BounceByDomain();
            var thirdBounceByDomain = new BounceByDomain();
            var fourthBounceByDomain = new BounceByDomain();
            var fifthBounceByDomain = new BounceByDomain();
            var sixthBounceByDomain = new BounceByDomain();

            // Act, Assert
            firstBounceByDomain.ShouldNotBeNull();
            secondBounceByDomain.ShouldNotBeNull();
            thirdBounceByDomain.ShouldNotBeNull();
            fourthBounceByDomain.ShouldNotBeNull();
            fifthBounceByDomain.ShouldNotBeNull();
            sixthBounceByDomain.ShouldNotBeNull();
            firstBounceByDomain.ShouldNotBeSameAs(secondBounceByDomain);
            thirdBounceByDomain.ShouldNotBeSameAs(firstBounceByDomain);
            fourthBounceByDomain.ShouldNotBeSameAs(firstBounceByDomain);
            fifthBounceByDomain.ShouldNotBeSameAs(firstBounceByDomain);
            sixthBounceByDomain.ShouldNotBeSameAs(firstBounceByDomain);
            sixthBounceByDomain.ShouldNotBeSameAs(fourthBounceByDomain);
        }

        #endregion

        #endregion

        #endregion
    }
}