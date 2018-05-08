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
    public class BlastReportDetailTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastReportDetail) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReportDetail_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastReportDetail = Fixture.Create<BlastReportDetail>();
            var blastId = Fixture.Create<int>();
            var unique_Total = Fixture.Create<int>();
            var total = Fixture.Create<int>();
            var actionTypeCode = Fixture.Create<string>();

            // Act
            blastReportDetail.BlastID = blastId;
            blastReportDetail.Unique_Total = unique_Total;
            blastReportDetail.Total = total;
            blastReportDetail.ActionTypeCode = actionTypeCode;

            // Assert
            blastReportDetail.BlastID.ShouldBe(blastId);
            blastReportDetail.Unique_Total.ShouldBe(unique_Total);
            blastReportDetail.Total.ShouldBe(total);
            blastReportDetail.ActionTypeCode.ShouldBe(actionTypeCode);
        }

        #endregion

        #region General Getters/Setters : Class (BlastReportDetail) => Property (ActionTypeCode) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReportDetail_ActionTypeCode_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastReportDetail = Fixture.Create<BlastReportDetail>();
            blastReportDetail.ActionTypeCode = Fixture.Create<string>();
            var stringType = blastReportDetail.ActionTypeCode.GetType();

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

        #region General Getters/Setters : Class (BlastReportDetail) => Property (ActionTypeCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReportDetail_Class_Invalid_Property_ActionTypeCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActionTypeCode = "ActionTypeCodeNotPresent";
            var blastReportDetail  = Fixture.Create<BlastReportDetail>();

            // Act , Assert
            Should.NotThrow(() => blastReportDetail.GetType().GetProperty(propertyNameActionTypeCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReportDetail_ActionTypeCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActionTypeCode = "ActionTypeCode";
            var blastReportDetail  = Fixture.Create<BlastReportDetail>();
            var propertyInfo  = blastReportDetail.GetType().GetProperty(propertyNameActionTypeCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastReportDetail) => Property (BlastID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReportDetail_BlastID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastReportDetail = Fixture.Create<BlastReportDetail>();
            blastReportDetail.BlastID = Fixture.Create<int>();
            var intType = blastReportDetail.BlastID.GetType();

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

        #region General Getters/Setters : Class (BlastReportDetail) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReportDetail_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var blastReportDetail  = Fixture.Create<BlastReportDetail>();

            // Act , Assert
            Should.NotThrow(() => blastReportDetail.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReportDetail_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var blastReportDetail  = Fixture.Create<BlastReportDetail>();
            var propertyInfo  = blastReportDetail.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastReportDetail) => Property (Total) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReportDetail_Total_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastReportDetail = Fixture.Create<BlastReportDetail>();
            blastReportDetail.Total = Fixture.Create<int>();
            var intType = blastReportDetail.Total.GetType();

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

        #region General Getters/Setters : Class (BlastReportDetail) => Property (Total) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReportDetail_Class_Invalid_Property_TotalNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotal = "TotalNotPresent";
            var blastReportDetail  = Fixture.Create<BlastReportDetail>();

            // Act , Assert
            Should.NotThrow(() => blastReportDetail.GetType().GetProperty(propertyNameTotal));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReportDetail_Total_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotal = "Total";
            var blastReportDetail  = Fixture.Create<BlastReportDetail>();
            var propertyInfo  = blastReportDetail.GetType().GetProperty(propertyNameTotal);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastReportDetail) => Property (Unique_Total) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReportDetail_Unique_Total_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastReportDetail = Fixture.Create<BlastReportDetail>();
            blastReportDetail.Unique_Total = Fixture.Create<int>();
            var intType = blastReportDetail.Unique_Total.GetType();

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

        #region General Getters/Setters : Class (BlastReportDetail) => Property (Unique_Total) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReportDetail_Class_Invalid_Property_Unique_TotalNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUnique_Total = "Unique_TotalNotPresent";
            var blastReportDetail  = Fixture.Create<BlastReportDetail>();

            // Act , Assert
            Should.NotThrow(() => blastReportDetail.GetType().GetProperty(propertyNameUnique_Total));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReportDetail_Unique_Total_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUnique_Total = "Unique_Total";
            var blastReportDetail  = Fixture.Create<BlastReportDetail>();
            var propertyInfo  = blastReportDetail.GetType().GetProperty(propertyNameUnique_Total);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastReportDetail) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastReportDetail_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastReportDetail());
        }

        #endregion

        #region General Constructor : Class (BlastReportDetail) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastReportDetail_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastReportDetail = Fixture.CreateMany<BlastReportDetail>(2).ToList();
            var firstBlastReportDetail = instancesOfBlastReportDetail.FirstOrDefault();
            var lastBlastReportDetail = instancesOfBlastReportDetail.Last();

            // Act, Assert
            firstBlastReportDetail.ShouldNotBeNull();
            lastBlastReportDetail.ShouldNotBeNull();
            firstBlastReportDetail.ShouldNotBeSameAs(lastBlastReportDetail);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastReportDetail_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastReportDetail = new BlastReportDetail();
            var secondBlastReportDetail = new BlastReportDetail();
            var thirdBlastReportDetail = new BlastReportDetail();
            var fourthBlastReportDetail = new BlastReportDetail();
            var fifthBlastReportDetail = new BlastReportDetail();
            var sixthBlastReportDetail = new BlastReportDetail();

            // Act, Assert
            firstBlastReportDetail.ShouldNotBeNull();
            secondBlastReportDetail.ShouldNotBeNull();
            thirdBlastReportDetail.ShouldNotBeNull();
            fourthBlastReportDetail.ShouldNotBeNull();
            fifthBlastReportDetail.ShouldNotBeNull();
            sixthBlastReportDetail.ShouldNotBeNull();
            firstBlastReportDetail.ShouldNotBeSameAs(secondBlastReportDetail);
            thirdBlastReportDetail.ShouldNotBeSameAs(firstBlastReportDetail);
            fourthBlastReportDetail.ShouldNotBeSameAs(firstBlastReportDetail);
            fifthBlastReportDetail.ShouldNotBeSameAs(firstBlastReportDetail);
            sixthBlastReportDetail.ShouldNotBeSameAs(firstBlastReportDetail);
            sixthBlastReportDetail.ShouldNotBeSameAs(fourthBlastReportDetail);
        }

        #endregion

        #endregion

        #endregion
    }
}