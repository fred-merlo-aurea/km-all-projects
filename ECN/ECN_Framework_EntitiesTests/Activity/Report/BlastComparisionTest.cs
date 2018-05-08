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
    public class BlastComparisionTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastComparision) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastComparision = Fixture.Create<BlastComparision>();
            var actionTypeCode = Fixture.Create<string>();
            var blastId = Fixture.Create<string>();
            var distinctCount = Fixture.Create<int>();
            var totalCount = Fixture.Create<int>();
            var sendTime = Fixture.Create<DateTime>();
            var emailSubject = Fixture.Create<string>();
            var totalSent = Fixture.Create<int>();
            var month = Fixture.Create<string>();
            var year = Fixture.Create<int>();
            var perc = Fixture.Create<float>();

            // Act
            blastComparision.ActionTypeCode = actionTypeCode;
            blastComparision.BlastID = blastId;
            blastComparision.DistinctCount = distinctCount;
            blastComparision.TotalCount = totalCount;
            blastComparision.SendTime = sendTime;
            blastComparision.EmailSubject = emailSubject;
            blastComparision.TotalSent = totalSent;
            blastComparision.Month = month;
            blastComparision.Year = year;
            blastComparision.Perc = perc;

            // Assert
            blastComparision.ActionTypeCode.ShouldBe(actionTypeCode);
            blastComparision.BlastID.ShouldBe(blastId);
            blastComparision.DistinctCount.ShouldBe(distinctCount);
            blastComparision.TotalCount.ShouldBe(totalCount);
            blastComparision.SendTime.ShouldBe(sendTime);
            blastComparision.EmailSubject.ShouldBe(emailSubject);
            blastComparision.TotalSent.ShouldBe(totalSent);
            blastComparision.Month.ShouldBe(month);
            blastComparision.Year.ShouldBe(year);
            blastComparision.Perc.ShouldBe(perc);
        }

        #endregion

        #region General Getters/Setters : Class (BlastComparision) => Property (ActionTypeCode) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_ActionTypeCode_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastComparision = Fixture.Create<BlastComparision>();
            blastComparision.ActionTypeCode = Fixture.Create<string>();
            var stringType = blastComparision.ActionTypeCode.GetType();

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

        #region General Getters/Setters : Class (BlastComparision) => Property (ActionTypeCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_Class_Invalid_Property_ActionTypeCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActionTypeCode = "ActionTypeCodeNotPresent";
            var blastComparision  = Fixture.Create<BlastComparision>();

            // Act , Assert
            Should.NotThrow(() => blastComparision.GetType().GetProperty(propertyNameActionTypeCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_ActionTypeCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActionTypeCode = "ActionTypeCode";
            var blastComparision  = Fixture.Create<BlastComparision>();
            var propertyInfo  = blastComparision.GetType().GetProperty(propertyNameActionTypeCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastComparision) => Property (BlastID) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_BlastID_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastComparision = Fixture.Create<BlastComparision>();
            blastComparision.BlastID = Fixture.Create<string>();
            var stringType = blastComparision.BlastID.GetType();

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

        #region General Getters/Setters : Class (BlastComparision) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var blastComparision  = Fixture.Create<BlastComparision>();

            // Act , Assert
            Should.NotThrow(() => blastComparision.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var blastComparision  = Fixture.Create<BlastComparision>();
            var propertyInfo  = blastComparision.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastComparision) => Property (DistinctCount) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_DistinctCount_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastComparision = Fixture.Create<BlastComparision>();
            blastComparision.DistinctCount = Fixture.Create<int>();
            var intType = blastComparision.DistinctCount.GetType();

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

        #region General Getters/Setters : Class (BlastComparision) => Property (DistinctCount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_Class_Invalid_Property_DistinctCountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDistinctCount = "DistinctCountNotPresent";
            var blastComparision  = Fixture.Create<BlastComparision>();

            // Act , Assert
            Should.NotThrow(() => blastComparision.GetType().GetProperty(propertyNameDistinctCount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_DistinctCount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDistinctCount = "DistinctCount";
            var blastComparision  = Fixture.Create<BlastComparision>();
            var propertyInfo  = blastComparision.GetType().GetProperty(propertyNameDistinctCount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastComparision) => Property (EmailSubject) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_EmailSubject_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastComparision = Fixture.Create<BlastComparision>();
            blastComparision.EmailSubject = Fixture.Create<string>();
            var stringType = blastComparision.EmailSubject.GetType();

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

        #region General Getters/Setters : Class (BlastComparision) => Property (EmailSubject) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_Class_Invalid_Property_EmailSubjectNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubjectNotPresent";
            var blastComparision  = Fixture.Create<BlastComparision>();

            // Act , Assert
            Should.NotThrow(() => blastComparision.GetType().GetProperty(propertyNameEmailSubject));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_EmailSubject_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubject";
            var blastComparision  = Fixture.Create<BlastComparision>();
            var propertyInfo  = blastComparision.GetType().GetProperty(propertyNameEmailSubject);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastComparision) => Property (Month) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_Month_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastComparision = Fixture.Create<BlastComparision>();
            blastComparision.Month = Fixture.Create<string>();
            var stringType = blastComparision.Month.GetType();

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

        #region General Getters/Setters : Class (BlastComparision) => Property (Month) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_Class_Invalid_Property_MonthNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMonth = "MonthNotPresent";
            var blastComparision  = Fixture.Create<BlastComparision>();

            // Act , Assert
            Should.NotThrow(() => blastComparision.GetType().GetProperty(propertyNameMonth));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_Month_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMonth = "Month";
            var blastComparision  = Fixture.Create<BlastComparision>();
            var propertyInfo  = blastComparision.GetType().GetProperty(propertyNameMonth);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastComparision) => Property (Perc) (Type : float) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_Perc_Property_Float_Type_Verify_Test()
        {
            // Arrange
            var blastComparision = Fixture.Create<BlastComparision>();
            blastComparision.Perc = Fixture.Create<float>();
            var floatType = blastComparision.Perc.GetType();

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

        #region General Getters/Setters : Class (BlastComparision) => Property (Perc) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_Class_Invalid_Property_PercNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePerc = "PercNotPresent";
            var blastComparision  = Fixture.Create<BlastComparision>();

            // Act , Assert
            Should.NotThrow(() => blastComparision.GetType().GetProperty(propertyNamePerc));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_Perc_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePerc = "Perc";
            var blastComparision  = Fixture.Create<BlastComparision>();
            var propertyInfo  = blastComparision.GetType().GetProperty(propertyNamePerc);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastComparision) => Property (SendTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_SendTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var blastComparision = Fixture.Create<BlastComparision>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastComparision.GetType().GetProperty(propertyNameSendTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastComparision, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastComparision) => Property (SendTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_Class_Invalid_Property_SendTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTimeNotPresent";
            var blastComparision  = Fixture.Create<BlastComparision>();

            // Act , Assert
            Should.NotThrow(() => blastComparision.GetType().GetProperty(propertyNameSendTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_SendTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var blastComparision  = Fixture.Create<BlastComparision>();
            var propertyInfo  = blastComparision.GetType().GetProperty(propertyNameSendTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastComparision) => Property (TotalCount) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_TotalCount_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastComparision = Fixture.Create<BlastComparision>();
            blastComparision.TotalCount = Fixture.Create<int>();
            var intType = blastComparision.TotalCount.GetType();

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

        #region General Getters/Setters : Class (BlastComparision) => Property (TotalCount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_Class_Invalid_Property_TotalCountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalCount = "TotalCountNotPresent";
            var blastComparision  = Fixture.Create<BlastComparision>();

            // Act , Assert
            Should.NotThrow(() => blastComparision.GetType().GetProperty(propertyNameTotalCount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_TotalCount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotalCount = "TotalCount";
            var blastComparision  = Fixture.Create<BlastComparision>();
            var propertyInfo  = blastComparision.GetType().GetProperty(propertyNameTotalCount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastComparision) => Property (TotalSent) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_TotalSent_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastComparision = Fixture.Create<BlastComparision>();
            blastComparision.TotalSent = Fixture.Create<int>();
            var intType = blastComparision.TotalSent.GetType();

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

        #region General Getters/Setters : Class (BlastComparision) => Property (TotalSent) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_Class_Invalid_Property_TotalSentNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalSent = "TotalSentNotPresent";
            var blastComparision  = Fixture.Create<BlastComparision>();

            // Act , Assert
            Should.NotThrow(() => blastComparision.GetType().GetProperty(propertyNameTotalSent));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_TotalSent_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotalSent = "TotalSent";
            var blastComparision  = Fixture.Create<BlastComparision>();
            var propertyInfo  = blastComparision.GetType().GetProperty(propertyNameTotalSent);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastComparision) => Property (Year) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_Year_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastComparision = Fixture.Create<BlastComparision>();
            blastComparision.Year = Fixture.Create<int>();
            var intType = blastComparision.Year.GetType();

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

        #region General Getters/Setters : Class (BlastComparision) => Property (Year) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_Class_Invalid_Property_YearNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameYear = "YearNotPresent";
            var blastComparision  = Fixture.Create<BlastComparision>();

            // Act , Assert
            Should.NotThrow(() => blastComparision.GetType().GetProperty(propertyNameYear));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastComparision_Year_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameYear = "Year";
            var blastComparision  = Fixture.Create<BlastComparision>();
            var propertyInfo  = blastComparision.GetType().GetProperty(propertyNameYear);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastComparision) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastComparision_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastComparision());
        }

        #endregion

        #region General Constructor : Class (BlastComparision) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastComparision_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastComparision = Fixture.CreateMany<BlastComparision>(2).ToList();
            var firstBlastComparision = instancesOfBlastComparision.FirstOrDefault();
            var lastBlastComparision = instancesOfBlastComparision.Last();

            // Act, Assert
            firstBlastComparision.ShouldNotBeNull();
            lastBlastComparision.ShouldNotBeNull();
            firstBlastComparision.ShouldNotBeSameAs(lastBlastComparision);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastComparision_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastComparision = new BlastComparision();
            var secondBlastComparision = new BlastComparision();
            var thirdBlastComparision = new BlastComparision();
            var fourthBlastComparision = new BlastComparision();
            var fifthBlastComparision = new BlastComparision();
            var sixthBlastComparision = new BlastComparision();

            // Act, Assert
            firstBlastComparision.ShouldNotBeNull();
            secondBlastComparision.ShouldNotBeNull();
            thirdBlastComparision.ShouldNotBeNull();
            fourthBlastComparision.ShouldNotBeNull();
            fifthBlastComparision.ShouldNotBeNull();
            sixthBlastComparision.ShouldNotBeNull();
            firstBlastComparision.ShouldNotBeSameAs(secondBlastComparision);
            thirdBlastComparision.ShouldNotBeSameAs(firstBlastComparision);
            fourthBlastComparision.ShouldNotBeSameAs(firstBlastComparision);
            fifthBlastComparision.ShouldNotBeSameAs(firstBlastComparision);
            sixthBlastComparision.ShouldNotBeSameAs(firstBlastComparision);
            sixthBlastComparision.ShouldNotBeSameAs(fourthBlastComparision);
        }

        #endregion

        #endregion

        #endregion
    }
}