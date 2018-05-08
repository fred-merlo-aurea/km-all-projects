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
    public class MasterSuppressionSourceReportTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (MasterSuppressionSourceReport) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MasterSuppressionSourceReport_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var masterSuppressionSourceReport = Fixture.Create<MasterSuppressionSourceReport>();
            var groupId = Fixture.Create<int>();
            var unsubscribeCodeId = Fixture.Create<int>();
            var unsubscribeCode = Fixture.Create<string>();
            var count = Fixture.Create<int>();

            // Act
            masterSuppressionSourceReport.GroupID = groupId;
            masterSuppressionSourceReport.UnsubscribeCodeID = unsubscribeCodeId;
            masterSuppressionSourceReport.UnsubscribeCode = unsubscribeCode;
            masterSuppressionSourceReport.Count = count;

            // Assert
            masterSuppressionSourceReport.GroupID.ShouldBe(groupId);
            masterSuppressionSourceReport.UnsubscribeCodeID.ShouldBe(unsubscribeCodeId);
            masterSuppressionSourceReport.UnsubscribeCode.ShouldBe(unsubscribeCode);
            masterSuppressionSourceReport.Count.ShouldBe(count);
        }

        #endregion

        #region General Getters/Setters : Class (MasterSuppressionSourceReport) => Property (Count) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MasterSuppressionSourceReport_Count_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var masterSuppressionSourceReport = Fixture.Create<MasterSuppressionSourceReport>();
            masterSuppressionSourceReport.Count = Fixture.Create<int>();
            var intType = masterSuppressionSourceReport.Count.GetType();

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

        #region General Getters/Setters : Class (MasterSuppressionSourceReport) => Property (Count) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MasterSuppressionSourceReport_Class_Invalid_Property_CountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCount = "CountNotPresent";
            var masterSuppressionSourceReport  = Fixture.Create<MasterSuppressionSourceReport>();

            // Act , Assert
            Should.NotThrow(() => masterSuppressionSourceReport.GetType().GetProperty(propertyNameCount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MasterSuppressionSourceReport_Count_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCount = "Count";
            var masterSuppressionSourceReport  = Fixture.Create<MasterSuppressionSourceReport>();
            var propertyInfo  = masterSuppressionSourceReport.GetType().GetProperty(propertyNameCount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MasterSuppressionSourceReport) => Property (GroupID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MasterSuppressionSourceReport_GroupID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var masterSuppressionSourceReport = Fixture.Create<MasterSuppressionSourceReport>();
            masterSuppressionSourceReport.GroupID = Fixture.Create<int>();
            var intType = masterSuppressionSourceReport.GroupID.GetType();

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

        #region General Getters/Setters : Class (MasterSuppressionSourceReport) => Property (GroupID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MasterSuppressionSourceReport_Class_Invalid_Property_GroupIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupIDNotPresent";
            var masterSuppressionSourceReport  = Fixture.Create<MasterSuppressionSourceReport>();

            // Act , Assert
            Should.NotThrow(() => masterSuppressionSourceReport.GetType().GetProperty(propertyNameGroupID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MasterSuppressionSourceReport_GroupID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupID";
            var masterSuppressionSourceReport  = Fixture.Create<MasterSuppressionSourceReport>();
            var propertyInfo  = masterSuppressionSourceReport.GetType().GetProperty(propertyNameGroupID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MasterSuppressionSourceReport) => Property (UnsubscribeCode) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MasterSuppressionSourceReport_UnsubscribeCode_Property_String_Type_Verify_Test()
        {
            // Arrange
            var masterSuppressionSourceReport = Fixture.Create<MasterSuppressionSourceReport>();
            masterSuppressionSourceReport.UnsubscribeCode = Fixture.Create<string>();
            var stringType = masterSuppressionSourceReport.UnsubscribeCode.GetType();

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

        #region General Getters/Setters : Class (MasterSuppressionSourceReport) => Property (UnsubscribeCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MasterSuppressionSourceReport_Class_Invalid_Property_UnsubscribeCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUnsubscribeCode = "UnsubscribeCodeNotPresent";
            var masterSuppressionSourceReport  = Fixture.Create<MasterSuppressionSourceReport>();

            // Act , Assert
            Should.NotThrow(() => masterSuppressionSourceReport.GetType().GetProperty(propertyNameUnsubscribeCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MasterSuppressionSourceReport_UnsubscribeCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUnsubscribeCode = "UnsubscribeCode";
            var masterSuppressionSourceReport  = Fixture.Create<MasterSuppressionSourceReport>();
            var propertyInfo  = masterSuppressionSourceReport.GetType().GetProperty(propertyNameUnsubscribeCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MasterSuppressionSourceReport) => Property (UnsubscribeCodeID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MasterSuppressionSourceReport_UnsubscribeCodeID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var masterSuppressionSourceReport = Fixture.Create<MasterSuppressionSourceReport>();
            masterSuppressionSourceReport.UnsubscribeCodeID = Fixture.Create<int>();
            var intType = masterSuppressionSourceReport.UnsubscribeCodeID.GetType();

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

        #region General Getters/Setters : Class (MasterSuppressionSourceReport) => Property (UnsubscribeCodeID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MasterSuppressionSourceReport_Class_Invalid_Property_UnsubscribeCodeIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUnsubscribeCodeID = "UnsubscribeCodeIDNotPresent";
            var masterSuppressionSourceReport  = Fixture.Create<MasterSuppressionSourceReport>();

            // Act , Assert
            Should.NotThrow(() => masterSuppressionSourceReport.GetType().GetProperty(propertyNameUnsubscribeCodeID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MasterSuppressionSourceReport_UnsubscribeCodeID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUnsubscribeCodeID = "UnsubscribeCodeID";
            var masterSuppressionSourceReport  = Fixture.Create<MasterSuppressionSourceReport>();
            var propertyInfo  = masterSuppressionSourceReport.GetType().GetProperty(propertyNameUnsubscribeCodeID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #endregion
    }
}