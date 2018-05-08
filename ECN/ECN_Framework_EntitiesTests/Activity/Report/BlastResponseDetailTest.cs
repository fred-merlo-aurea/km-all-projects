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
    public class BlastResponseDetailTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastResponseDetail) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastResponseDetail_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastResponseDetail = Fixture.Create<BlastResponseDetail>();
            var action = Fixture.Create<string>();
            var actionDate = Fixture.Create<string>();
            var total = Fixture.Create<int>();

            // Act
            blastResponseDetail.Action = action;
            blastResponseDetail.ActionDate = actionDate;
            blastResponseDetail.Total = total;

            // Assert
            blastResponseDetail.Action.ShouldBe(action);
            blastResponseDetail.ActionDate.ShouldBe(actionDate);
            blastResponseDetail.Total.ShouldBe(total);
        }

        #endregion

        #region General Getters/Setters : Class (BlastResponseDetail) => Property (Action) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastResponseDetail_Action_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastResponseDetail = Fixture.Create<BlastResponseDetail>();
            blastResponseDetail.Action = Fixture.Create<string>();
            var stringType = blastResponseDetail.Action.GetType();

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

        #region General Getters/Setters : Class (BlastResponseDetail) => Property (Action) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastResponseDetail_Class_Invalid_Property_ActionNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAction = "ActionNotPresent";
            var blastResponseDetail  = Fixture.Create<BlastResponseDetail>();

            // Act , Assert
            Should.NotThrow(() => blastResponseDetail.GetType().GetProperty(propertyNameAction));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastResponseDetail_Action_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAction = "Action";
            var blastResponseDetail  = Fixture.Create<BlastResponseDetail>();
            var propertyInfo  = blastResponseDetail.GetType().GetProperty(propertyNameAction);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastResponseDetail) => Property (ActionDate) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastResponseDetail_ActionDate_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastResponseDetail = Fixture.Create<BlastResponseDetail>();
            blastResponseDetail.ActionDate = Fixture.Create<string>();
            var stringType = blastResponseDetail.ActionDate.GetType();

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

        #region General Getters/Setters : Class (BlastResponseDetail) => Property (ActionDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastResponseDetail_Class_Invalid_Property_ActionDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActionDate = "ActionDateNotPresent";
            var blastResponseDetail  = Fixture.Create<BlastResponseDetail>();

            // Act , Assert
            Should.NotThrow(() => blastResponseDetail.GetType().GetProperty(propertyNameActionDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastResponseDetail_ActionDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActionDate = "ActionDate";
            var blastResponseDetail  = Fixture.Create<BlastResponseDetail>();
            var propertyInfo  = blastResponseDetail.GetType().GetProperty(propertyNameActionDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastResponseDetail) => Property (Total) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastResponseDetail_Total_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastResponseDetail = Fixture.Create<BlastResponseDetail>();
            blastResponseDetail.Total = Fixture.Create<int>();
            var intType = blastResponseDetail.Total.GetType();

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

        #region General Getters/Setters : Class (BlastResponseDetail) => Property (Total) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastResponseDetail_Class_Invalid_Property_TotalNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotal = "TotalNotPresent";
            var blastResponseDetail  = Fixture.Create<BlastResponseDetail>();

            // Act , Assert
            Should.NotThrow(() => blastResponseDetail.GetType().GetProperty(propertyNameTotal));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastResponseDetail_Total_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotal = "Total";
            var blastResponseDetail  = Fixture.Create<BlastResponseDetail>();
            var propertyInfo  = blastResponseDetail.GetType().GetProperty(propertyNameTotal);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastResponseDetail) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastResponseDetail_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastResponseDetail());
        }

        #endregion

        #region General Constructor : Class (BlastResponseDetail) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastResponseDetail_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastResponseDetail = Fixture.CreateMany<BlastResponseDetail>(2).ToList();
            var firstBlastResponseDetail = instancesOfBlastResponseDetail.FirstOrDefault();
            var lastBlastResponseDetail = instancesOfBlastResponseDetail.Last();

            // Act, Assert
            firstBlastResponseDetail.ShouldNotBeNull();
            lastBlastResponseDetail.ShouldNotBeNull();
            firstBlastResponseDetail.ShouldNotBeSameAs(lastBlastResponseDetail);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastResponseDetail_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastResponseDetail = new BlastResponseDetail();
            var secondBlastResponseDetail = new BlastResponseDetail();
            var thirdBlastResponseDetail = new BlastResponseDetail();
            var fourthBlastResponseDetail = new BlastResponseDetail();
            var fifthBlastResponseDetail = new BlastResponseDetail();
            var sixthBlastResponseDetail = new BlastResponseDetail();

            // Act, Assert
            firstBlastResponseDetail.ShouldNotBeNull();
            secondBlastResponseDetail.ShouldNotBeNull();
            thirdBlastResponseDetail.ShouldNotBeNull();
            fourthBlastResponseDetail.ShouldNotBeNull();
            fifthBlastResponseDetail.ShouldNotBeNull();
            sixthBlastResponseDetail.ShouldNotBeNull();
            firstBlastResponseDetail.ShouldNotBeSameAs(secondBlastResponseDetail);
            thirdBlastResponseDetail.ShouldNotBeSameAs(firstBlastResponseDetail);
            fourthBlastResponseDetail.ShouldNotBeSameAs(firstBlastResponseDetail);
            fifthBlastResponseDetail.ShouldNotBeSameAs(firstBlastResponseDetail);
            sixthBlastResponseDetail.ShouldNotBeSameAs(firstBlastResponseDetail);
            sixthBlastResponseDetail.ShouldNotBeSameAs(fourthBlastResponseDetail);
        }

        #endregion

        #endregion

        #endregion
    }
}