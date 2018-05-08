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
    public class BlastReportPerformanceTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastReportPerformance) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReportPerformance_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastReportPerformance = Fixture.Create<BlastReportPerformance>();
            var blastId = Fixture.Create<int>();
            var total = Fixture.Create<int>();
            var actionTypeCode = Fixture.Create<string>();

            // Act
            blastReportPerformance.BlastID = blastId;
            blastReportPerformance.Total = total;
            blastReportPerformance.ActionTypeCode = actionTypeCode;

            // Assert
            blastReportPerformance.BlastID.ShouldBe(blastId);
            blastReportPerformance.Total.ShouldBe(total);
            blastReportPerformance.ActionTypeCode.ShouldBe(actionTypeCode);
        }

        #endregion

        #region General Getters/Setters : Class (BlastReportPerformance) => Property (ActionTypeCode) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReportPerformance_ActionTypeCode_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastReportPerformance = Fixture.Create<BlastReportPerformance>();
            blastReportPerformance.ActionTypeCode = Fixture.Create<string>();
            var stringType = blastReportPerformance.ActionTypeCode.GetType();

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

        #region General Getters/Setters : Class (BlastReportPerformance) => Property (ActionTypeCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReportPerformance_Class_Invalid_Property_ActionTypeCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActionTypeCode = "ActionTypeCodeNotPresent";
            var blastReportPerformance  = Fixture.Create<BlastReportPerformance>();

            // Act , Assert
            Should.NotThrow(() => blastReportPerformance.GetType().GetProperty(propertyNameActionTypeCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReportPerformance_ActionTypeCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActionTypeCode = "ActionTypeCode";
            var blastReportPerformance  = Fixture.Create<BlastReportPerformance>();
            var propertyInfo  = blastReportPerformance.GetType().GetProperty(propertyNameActionTypeCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastReportPerformance) => Property (BlastID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReportPerformance_BlastID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastReportPerformance = Fixture.Create<BlastReportPerformance>();
            blastReportPerformance.BlastID = Fixture.Create<int>();
            var intType = blastReportPerformance.BlastID.GetType();

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

        #region General Getters/Setters : Class (BlastReportPerformance) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReportPerformance_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var blastReportPerformance  = Fixture.Create<BlastReportPerformance>();

            // Act , Assert
            Should.NotThrow(() => blastReportPerformance.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReportPerformance_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var blastReportPerformance  = Fixture.Create<BlastReportPerformance>();
            var propertyInfo  = blastReportPerformance.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastReportPerformance) => Property (Total) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReportPerformance_Total_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastReportPerformance = Fixture.Create<BlastReportPerformance>();
            blastReportPerformance.Total = Fixture.Create<int>();
            var intType = blastReportPerformance.Total.GetType();

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

        #region General Getters/Setters : Class (BlastReportPerformance) => Property (Total) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReportPerformance_Class_Invalid_Property_TotalNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotal = "TotalNotPresent";
            var blastReportPerformance  = Fixture.Create<BlastReportPerformance>();

            // Act , Assert
            Should.NotThrow(() => blastReportPerformance.GetType().GetProperty(propertyNameTotal));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReportPerformance_Total_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotal = "Total";
            var blastReportPerformance  = Fixture.Create<BlastReportPerformance>();
            var propertyInfo  = blastReportPerformance.GetType().GetProperty(propertyNameTotal);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastReportPerformance) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastReportPerformance_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastReportPerformance());
        }

        #endregion

        #region General Constructor : Class (BlastReportPerformance) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastReportPerformance_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastReportPerformance = Fixture.CreateMany<BlastReportPerformance>(2).ToList();
            var firstBlastReportPerformance = instancesOfBlastReportPerformance.FirstOrDefault();
            var lastBlastReportPerformance = instancesOfBlastReportPerformance.Last();

            // Act, Assert
            firstBlastReportPerformance.ShouldNotBeNull();
            lastBlastReportPerformance.ShouldNotBeNull();
            firstBlastReportPerformance.ShouldNotBeSameAs(lastBlastReportPerformance);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastReportPerformance_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastReportPerformance = new BlastReportPerformance();
            var secondBlastReportPerformance = new BlastReportPerformance();
            var thirdBlastReportPerformance = new BlastReportPerformance();
            var fourthBlastReportPerformance = new BlastReportPerformance();
            var fifthBlastReportPerformance = new BlastReportPerformance();
            var sixthBlastReportPerformance = new BlastReportPerformance();

            // Act, Assert
            firstBlastReportPerformance.ShouldNotBeNull();
            secondBlastReportPerformance.ShouldNotBeNull();
            thirdBlastReportPerformance.ShouldNotBeNull();
            fourthBlastReportPerformance.ShouldNotBeNull();
            fifthBlastReportPerformance.ShouldNotBeNull();
            sixthBlastReportPerformance.ShouldNotBeNull();
            firstBlastReportPerformance.ShouldNotBeSameAs(secondBlastReportPerformance);
            thirdBlastReportPerformance.ShouldNotBeSameAs(firstBlastReportPerformance);
            fourthBlastReportPerformance.ShouldNotBeSameAs(firstBlastReportPerformance);
            fifthBlastReportPerformance.ShouldNotBeSameAs(firstBlastReportPerformance);
            sixthBlastReportPerformance.ShouldNotBeSameAs(firstBlastReportPerformance);
            sixthBlastReportPerformance.ShouldNotBeSameAs(fourthBlastReportPerformance);
        }

        #endregion

        #endregion

        #endregion
    }
}