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
    public class BlastOpensByTimeTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastOpensByTime) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastOpensByTime_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastOpensByTime = Fixture.Create<BlastOpensByTime>();
            var hour = Fixture.Create<string>();
            var opens = Fixture.Create<int>();
            var opensPerc = Fixture.Create<string>();

            // Act
            blastOpensByTime.Hour = hour;
            blastOpensByTime.Opens = opens;
            blastOpensByTime.OpensPerc = opensPerc;

            // Assert
            blastOpensByTime.Hour.ShouldBe(hour);
            blastOpensByTime.Opens.ShouldBe(opens);
            blastOpensByTime.OpensPerc.ShouldBe(opensPerc);
        }

        #endregion

        #region General Getters/Setters : Class (BlastOpensByTime) => Property (Hour) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastOpensByTime_Hour_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastOpensByTime = Fixture.Create<BlastOpensByTime>();
            blastOpensByTime.Hour = Fixture.Create<string>();
            var stringType = blastOpensByTime.Hour.GetType();

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

        #region General Getters/Setters : Class (BlastOpensByTime) => Property (Hour) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastOpensByTime_Class_Invalid_Property_HourNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameHour = "HourNotPresent";
            var blastOpensByTime  = Fixture.Create<BlastOpensByTime>();

            // Act , Assert
            Should.NotThrow(() => blastOpensByTime.GetType().GetProperty(propertyNameHour));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastOpensByTime_Hour_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameHour = "Hour";
            var blastOpensByTime  = Fixture.Create<BlastOpensByTime>();
            var propertyInfo  = blastOpensByTime.GetType().GetProperty(propertyNameHour);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastOpensByTime) => Property (Opens) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastOpensByTime_Opens_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastOpensByTime = Fixture.Create<BlastOpensByTime>();
            blastOpensByTime.Opens = Fixture.Create<int>();
            var intType = blastOpensByTime.Opens.GetType();

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

        #region General Getters/Setters : Class (BlastOpensByTime) => Property (Opens) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastOpensByTime_Class_Invalid_Property_OpensNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOpens = "OpensNotPresent";
            var blastOpensByTime  = Fixture.Create<BlastOpensByTime>();

            // Act , Assert
            Should.NotThrow(() => blastOpensByTime.GetType().GetProperty(propertyNameOpens));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastOpensByTime_Opens_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOpens = "Opens";
            var blastOpensByTime  = Fixture.Create<BlastOpensByTime>();
            var propertyInfo  = blastOpensByTime.GetType().GetProperty(propertyNameOpens);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastOpensByTime) => Property (OpensPerc) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastOpensByTime_OpensPerc_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastOpensByTime = Fixture.Create<BlastOpensByTime>();
            blastOpensByTime.OpensPerc = Fixture.Create<string>();
            var stringType = blastOpensByTime.OpensPerc.GetType();

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

        #region General Getters/Setters : Class (BlastOpensByTime) => Property (OpensPerc) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastOpensByTime_Class_Invalid_Property_OpensPercNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOpensPerc = "OpensPercNotPresent";
            var blastOpensByTime  = Fixture.Create<BlastOpensByTime>();

            // Act , Assert
            Should.NotThrow(() => blastOpensByTime.GetType().GetProperty(propertyNameOpensPerc));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastOpensByTime_OpensPerc_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOpensPerc = "OpensPerc";
            var blastOpensByTime  = Fixture.Create<BlastOpensByTime>();
            var propertyInfo  = blastOpensByTime.GetType().GetProperty(propertyNameOpensPerc);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastOpensByTime) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastOpensByTime_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastOpensByTime());
        }

        #endregion

        #region General Constructor : Class (BlastOpensByTime) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastOpensByTime_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastOpensByTime = Fixture.CreateMany<BlastOpensByTime>(2).ToList();
            var firstBlastOpensByTime = instancesOfBlastOpensByTime.FirstOrDefault();
            var lastBlastOpensByTime = instancesOfBlastOpensByTime.Last();

            // Act, Assert
            firstBlastOpensByTime.ShouldNotBeNull();
            lastBlastOpensByTime.ShouldNotBeNull();
            firstBlastOpensByTime.ShouldNotBeSameAs(lastBlastOpensByTime);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastOpensByTime_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastOpensByTime = new BlastOpensByTime();
            var secondBlastOpensByTime = new BlastOpensByTime();
            var thirdBlastOpensByTime = new BlastOpensByTime();
            var fourthBlastOpensByTime = new BlastOpensByTime();
            var fifthBlastOpensByTime = new BlastOpensByTime();
            var sixthBlastOpensByTime = new BlastOpensByTime();

            // Act, Assert
            firstBlastOpensByTime.ShouldNotBeNull();
            secondBlastOpensByTime.ShouldNotBeNull();
            thirdBlastOpensByTime.ShouldNotBeNull();
            fourthBlastOpensByTime.ShouldNotBeNull();
            fifthBlastOpensByTime.ShouldNotBeNull();
            sixthBlastOpensByTime.ShouldNotBeNull();
            firstBlastOpensByTime.ShouldNotBeSameAs(secondBlastOpensByTime);
            thirdBlastOpensByTime.ShouldNotBeSameAs(firstBlastOpensByTime);
            fourthBlastOpensByTime.ShouldNotBeSameAs(firstBlastOpensByTime);
            fifthBlastOpensByTime.ShouldNotBeSameAs(firstBlastOpensByTime);
            sixthBlastOpensByTime.ShouldNotBeSameAs(firstBlastOpensByTime);
            sixthBlastOpensByTime.ShouldNotBeSameAs(fourthBlastOpensByTime);
        }

        #endregion

        #endregion

        #endregion
    }
}