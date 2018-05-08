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
    public class BlastReportTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastReport) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastReport = Fixture.Create<BlastReport>();
            var blastId = Fixture.Create<int>();
            var emailSubject = Fixture.Create<string>();
            var emailFromName = Fixture.Create<string>();
            var emailFrom = Fixture.Create<string>();
            var groupName = Fixture.Create<string>();
            var filterName = Fixture.Create<string>();
            var layoutName = Fixture.Create<string>();
            var sendTime = Fixture.Create<DateTime>();
            var finishTime = Fixture.Create<DateTime>();
            var successTotal = Fixture.Create<int>();
            var sendTotal = Fixture.Create<int>();
            var setupCost = Fixture.Create<string>();
            var outboundCost = Fixture.Create<string>();
            var inboundCost = Fixture.Create<string>();
            var designCost = Fixture.Create<string>();
            var otherCost = Fixture.Create<string>();
            var suppressionGroups = Fixture.Create<string>();
            var suppressionGroupFilters = Fixture.Create<string>();

            // Act
            blastReport.BlastID = blastId;
            blastReport.EmailSubject = emailSubject;
            blastReport.EmailFromName = emailFromName;
            blastReport.EmailFrom = emailFrom;
            blastReport.GroupName = groupName;
            blastReport.FilterName = filterName;
            blastReport.LayoutName = layoutName;
            blastReport.SendTime = sendTime;
            blastReport.FinishTime = finishTime;
            blastReport.SuccessTotal = successTotal;
            blastReport.SendTotal = sendTotal;
            blastReport.SetupCost = setupCost;
            blastReport.OutboundCost = outboundCost;
            blastReport.InboundCost = inboundCost;
            blastReport.DesignCost = designCost;
            blastReport.OtherCost = otherCost;
            blastReport.SuppressionGroups = suppressionGroups;
            blastReport.SuppressionGroupFilters = suppressionGroupFilters;

            // Assert
            blastReport.BlastID.ShouldBe(blastId);
            blastReport.EmailSubject.ShouldBe(emailSubject);
            blastReport.EmailFromName.ShouldBe(emailFromName);
            blastReport.EmailFrom.ShouldBe(emailFrom);
            blastReport.GroupName.ShouldBe(groupName);
            blastReport.FilterName.ShouldBe(filterName);
            blastReport.LayoutName.ShouldBe(layoutName);
            blastReport.SendTime.ShouldBe(sendTime);
            blastReport.FinishTime.ShouldBe(finishTime);
            blastReport.SuccessTotal.ShouldBe(successTotal);
            blastReport.SendTotal.ShouldBe(sendTotal);
            blastReport.SetupCost.ShouldBe(setupCost);
            blastReport.OutboundCost.ShouldBe(outboundCost);
            blastReport.InboundCost.ShouldBe(inboundCost);
            blastReport.DesignCost.ShouldBe(designCost);
            blastReport.OtherCost.ShouldBe(otherCost);
            blastReport.SuppressionGroups.ShouldBe(suppressionGroups);
            blastReport.SuppressionGroupFilters.ShouldBe(suppressionGroupFilters);
        }

        #endregion

        #region General Getters/Setters : Class (BlastReport) => Property (BlastID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_BlastID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastReport = Fixture.Create<BlastReport>();
            blastReport.BlastID = Fixture.Create<int>();
            var intType = blastReport.BlastID.GetType();

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

        #region General Getters/Setters : Class (BlastReport) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var blastReport  = Fixture.Create<BlastReport>();

            // Act , Assert
            Should.NotThrow(() => blastReport.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var blastReport  = Fixture.Create<BlastReport>();
            var propertyInfo  = blastReport.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastReport) => Property (DesignCost) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_DesignCost_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastReport = Fixture.Create<BlastReport>();
            blastReport.DesignCost = Fixture.Create<string>();
            var stringType = blastReport.DesignCost.GetType();

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

        #region General Getters/Setters : Class (BlastReport) => Property (DesignCost) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_Class_Invalid_Property_DesignCostNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDesignCost = "DesignCostNotPresent";
            var blastReport  = Fixture.Create<BlastReport>();

            // Act , Assert
            Should.NotThrow(() => blastReport.GetType().GetProperty(propertyNameDesignCost));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_DesignCost_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDesignCost = "DesignCost";
            var blastReport  = Fixture.Create<BlastReport>();
            var propertyInfo  = blastReport.GetType().GetProperty(propertyNameDesignCost);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastReport) => Property (EmailFrom) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_EmailFrom_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastReport = Fixture.Create<BlastReport>();
            blastReport.EmailFrom = Fixture.Create<string>();
            var stringType = blastReport.EmailFrom.GetType();

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

        #region General Getters/Setters : Class (BlastReport) => Property (EmailFrom) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_Class_Invalid_Property_EmailFromNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailFrom = "EmailFromNotPresent";
            var blastReport  = Fixture.Create<BlastReport>();

            // Act , Assert
            Should.NotThrow(() => blastReport.GetType().GetProperty(propertyNameEmailFrom));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_EmailFrom_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailFrom = "EmailFrom";
            var blastReport  = Fixture.Create<BlastReport>();
            var propertyInfo  = blastReport.GetType().GetProperty(propertyNameEmailFrom);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastReport) => Property (EmailFromName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_EmailFromName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastReport = Fixture.Create<BlastReport>();
            blastReport.EmailFromName = Fixture.Create<string>();
            var stringType = blastReport.EmailFromName.GetType();

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

        #region General Getters/Setters : Class (BlastReport) => Property (EmailFromName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_Class_Invalid_Property_EmailFromNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailFromName = "EmailFromNameNotPresent";
            var blastReport  = Fixture.Create<BlastReport>();

            // Act , Assert
            Should.NotThrow(() => blastReport.GetType().GetProperty(propertyNameEmailFromName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_EmailFromName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailFromName = "EmailFromName";
            var blastReport  = Fixture.Create<BlastReport>();
            var propertyInfo  = blastReport.GetType().GetProperty(propertyNameEmailFromName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastReport) => Property (EmailSubject) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_EmailSubject_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastReport = Fixture.Create<BlastReport>();
            blastReport.EmailSubject = Fixture.Create<string>();
            var stringType = blastReport.EmailSubject.GetType();

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

        #region General Getters/Setters : Class (BlastReport) => Property (EmailSubject) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_Class_Invalid_Property_EmailSubjectNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubjectNotPresent";
            var blastReport  = Fixture.Create<BlastReport>();

            // Act , Assert
            Should.NotThrow(() => blastReport.GetType().GetProperty(propertyNameEmailSubject));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_EmailSubject_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubject";
            var blastReport  = Fixture.Create<BlastReport>();
            var propertyInfo  = blastReport.GetType().GetProperty(propertyNameEmailSubject);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastReport) => Property (FilterName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_FilterName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastReport = Fixture.Create<BlastReport>();
            blastReport.FilterName = Fixture.Create<string>();
            var stringType = blastReport.FilterName.GetType();

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

        #region General Getters/Setters : Class (BlastReport) => Property (FilterName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_Class_Invalid_Property_FilterNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFilterName = "FilterNameNotPresent";
            var blastReport  = Fixture.Create<BlastReport>();

            // Act , Assert
            Should.NotThrow(() => blastReport.GetType().GetProperty(propertyNameFilterName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_FilterName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFilterName = "FilterName";
            var blastReport  = Fixture.Create<BlastReport>();
            var propertyInfo  = blastReport.GetType().GetProperty(propertyNameFilterName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastReport) => Property (FinishTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_FinishTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameFinishTime = "FinishTime";
            var blastReport = Fixture.Create<BlastReport>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastReport.GetType().GetProperty(propertyNameFinishTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastReport) => Property (FinishTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_Class_Invalid_Property_FinishTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFinishTime = "FinishTimeNotPresent";
            var blastReport  = Fixture.Create<BlastReport>();

            // Act , Assert
            Should.NotThrow(() => blastReport.GetType().GetProperty(propertyNameFinishTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_FinishTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFinishTime = "FinishTime";
            var blastReport  = Fixture.Create<BlastReport>();
            var propertyInfo  = blastReport.GetType().GetProperty(propertyNameFinishTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastReport) => Property (GroupName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_GroupName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastReport = Fixture.Create<BlastReport>();
            blastReport.GroupName = Fixture.Create<string>();
            var stringType = blastReport.GroupName.GetType();

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

        #region General Getters/Setters : Class (BlastReport) => Property (GroupName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_Class_Invalid_Property_GroupNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupName = "GroupNameNotPresent";
            var blastReport  = Fixture.Create<BlastReport>();

            // Act , Assert
            Should.NotThrow(() => blastReport.GetType().GetProperty(propertyNameGroupName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_GroupName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupName = "GroupName";
            var blastReport  = Fixture.Create<BlastReport>();
            var propertyInfo  = blastReport.GetType().GetProperty(propertyNameGroupName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastReport) => Property (InboundCost) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_InboundCost_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastReport = Fixture.Create<BlastReport>();
            blastReport.InboundCost = Fixture.Create<string>();
            var stringType = blastReport.InboundCost.GetType();

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

        #region General Getters/Setters : Class (BlastReport) => Property (InboundCost) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_Class_Invalid_Property_InboundCostNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameInboundCost = "InboundCostNotPresent";
            var blastReport  = Fixture.Create<BlastReport>();

            // Act , Assert
            Should.NotThrow(() => blastReport.GetType().GetProperty(propertyNameInboundCost));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_InboundCost_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameInboundCost = "InboundCost";
            var blastReport  = Fixture.Create<BlastReport>();
            var propertyInfo  = blastReport.GetType().GetProperty(propertyNameInboundCost);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastReport) => Property (LayoutName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_LayoutName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastReport = Fixture.Create<BlastReport>();
            blastReport.LayoutName = Fixture.Create<string>();
            var stringType = blastReport.LayoutName.GetType();

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

        #region General Getters/Setters : Class (BlastReport) => Property (LayoutName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_Class_Invalid_Property_LayoutNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLayoutName = "LayoutNameNotPresent";
            var blastReport  = Fixture.Create<BlastReport>();

            // Act , Assert
            Should.NotThrow(() => blastReport.GetType().GetProperty(propertyNameLayoutName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_LayoutName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLayoutName = "LayoutName";
            var blastReport  = Fixture.Create<BlastReport>();
            var propertyInfo  = blastReport.GetType().GetProperty(propertyNameLayoutName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastReport) => Property (OtherCost) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_OtherCost_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastReport = Fixture.Create<BlastReport>();
            blastReport.OtherCost = Fixture.Create<string>();
            var stringType = blastReport.OtherCost.GetType();

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

        #region General Getters/Setters : Class (BlastReport) => Property (OtherCost) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_Class_Invalid_Property_OtherCostNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOtherCost = "OtherCostNotPresent";
            var blastReport  = Fixture.Create<BlastReport>();

            // Act , Assert
            Should.NotThrow(() => blastReport.GetType().GetProperty(propertyNameOtherCost));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_OtherCost_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOtherCost = "OtherCost";
            var blastReport  = Fixture.Create<BlastReport>();
            var propertyInfo  = blastReport.GetType().GetProperty(propertyNameOtherCost);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastReport) => Property (OutboundCost) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_OutboundCost_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastReport = Fixture.Create<BlastReport>();
            blastReport.OutboundCost = Fixture.Create<string>();
            var stringType = blastReport.OutboundCost.GetType();

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

        #region General Getters/Setters : Class (BlastReport) => Property (OutboundCost) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_Class_Invalid_Property_OutboundCostNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOutboundCost = "OutboundCostNotPresent";
            var blastReport  = Fixture.Create<BlastReport>();

            // Act , Assert
            Should.NotThrow(() => blastReport.GetType().GetProperty(propertyNameOutboundCost));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_OutboundCost_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOutboundCost = "OutboundCost";
            var blastReport  = Fixture.Create<BlastReport>();
            var propertyInfo  = blastReport.GetType().GetProperty(propertyNameOutboundCost);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastReport) => Property (SendTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_SendTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var blastReport = Fixture.Create<BlastReport>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastReport.GetType().GetProperty(propertyNameSendTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastReport) => Property (SendTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_Class_Invalid_Property_SendTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTimeNotPresent";
            var blastReport  = Fixture.Create<BlastReport>();

            // Act , Assert
            Should.NotThrow(() => blastReport.GetType().GetProperty(propertyNameSendTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_SendTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var blastReport  = Fixture.Create<BlastReport>();
            var propertyInfo  = blastReport.GetType().GetProperty(propertyNameSendTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastReport) => Property (SendTotal) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_SendTotal_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastReport = Fixture.Create<BlastReport>();
            blastReport.SendTotal = Fixture.Create<int>();
            var intType = blastReport.SendTotal.GetType();

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

        #region General Getters/Setters : Class (BlastReport) => Property (SendTotal) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_Class_Invalid_Property_SendTotalNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTotal = "SendTotalNotPresent";
            var blastReport  = Fixture.Create<BlastReport>();

            // Act , Assert
            Should.NotThrow(() => blastReport.GetType().GetProperty(propertyNameSendTotal));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_SendTotal_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendTotal = "SendTotal";
            var blastReport  = Fixture.Create<BlastReport>();
            var propertyInfo  = blastReport.GetType().GetProperty(propertyNameSendTotal);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastReport) => Property (SetupCost) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_SetupCost_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastReport = Fixture.Create<BlastReport>();
            blastReport.SetupCost = Fixture.Create<string>();
            var stringType = blastReport.SetupCost.GetType();

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

        #region General Getters/Setters : Class (BlastReport) => Property (SetupCost) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_Class_Invalid_Property_SetupCostNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSetupCost = "SetupCostNotPresent";
            var blastReport  = Fixture.Create<BlastReport>();

            // Act , Assert
            Should.NotThrow(() => blastReport.GetType().GetProperty(propertyNameSetupCost));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_SetupCost_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSetupCost = "SetupCost";
            var blastReport  = Fixture.Create<BlastReport>();
            var propertyInfo  = blastReport.GetType().GetProperty(propertyNameSetupCost);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastReport) => Property (SuccessTotal) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_SuccessTotal_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastReport = Fixture.Create<BlastReport>();
            blastReport.SuccessTotal = Fixture.Create<int>();
            var intType = blastReport.SuccessTotal.GetType();

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

        #region General Getters/Setters : Class (BlastReport) => Property (SuccessTotal) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_Class_Invalid_Property_SuccessTotalNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSuccessTotal = "SuccessTotalNotPresent";
            var blastReport  = Fixture.Create<BlastReport>();

            // Act , Assert
            Should.NotThrow(() => blastReport.GetType().GetProperty(propertyNameSuccessTotal));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_SuccessTotal_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSuccessTotal = "SuccessTotal";
            var blastReport  = Fixture.Create<BlastReport>();
            var propertyInfo  = blastReport.GetType().GetProperty(propertyNameSuccessTotal);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastReport) => Property (SuppressionGroupFilters) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_SuppressionGroupFilters_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastReport = Fixture.Create<BlastReport>();
            blastReport.SuppressionGroupFilters = Fixture.Create<string>();
            var stringType = blastReport.SuppressionGroupFilters.GetType();

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

        #region General Getters/Setters : Class (BlastReport) => Property (SuppressionGroupFilters) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_Class_Invalid_Property_SuppressionGroupFiltersNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSuppressionGroupFilters = "SuppressionGroupFiltersNotPresent";
            var blastReport  = Fixture.Create<BlastReport>();

            // Act , Assert
            Should.NotThrow(() => blastReport.GetType().GetProperty(propertyNameSuppressionGroupFilters));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_SuppressionGroupFilters_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSuppressionGroupFilters = "SuppressionGroupFilters";
            var blastReport  = Fixture.Create<BlastReport>();
            var propertyInfo  = blastReport.GetType().GetProperty(propertyNameSuppressionGroupFilters);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastReport) => Property (SuppressionGroups) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_SuppressionGroups_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastReport = Fixture.Create<BlastReport>();
            blastReport.SuppressionGroups = Fixture.Create<string>();
            var stringType = blastReport.SuppressionGroups.GetType();

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

        #region General Getters/Setters : Class (BlastReport) => Property (SuppressionGroups) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_Class_Invalid_Property_SuppressionGroupsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSuppressionGroups = "SuppressionGroupsNotPresent";
            var blastReport  = Fixture.Create<BlastReport>();

            // Act , Assert
            Should.NotThrow(() => blastReport.GetType().GetProperty(propertyNameSuppressionGroups));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastReport_SuppressionGroups_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSuppressionGroups = "SuppressionGroups";
            var blastReport  = Fixture.Create<BlastReport>();
            var propertyInfo  = blastReport.GetType().GetProperty(propertyNameSuppressionGroups);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastReport) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastReport_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastReport());
        }

        #endregion

        #region General Constructor : Class (BlastReport) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastReport_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastReport = Fixture.CreateMany<BlastReport>(2).ToList();
            var firstBlastReport = instancesOfBlastReport.FirstOrDefault();
            var lastBlastReport = instancesOfBlastReport.Last();

            // Act, Assert
            firstBlastReport.ShouldNotBeNull();
            lastBlastReport.ShouldNotBeNull();
            firstBlastReport.ShouldNotBeSameAs(lastBlastReport);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastReport_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastReport = new BlastReport();
            var secondBlastReport = new BlastReport();
            var thirdBlastReport = new BlastReport();
            var fourthBlastReport = new BlastReport();
            var fifthBlastReport = new BlastReport();
            var sixthBlastReport = new BlastReport();

            // Act, Assert
            firstBlastReport.ShouldNotBeNull();
            secondBlastReport.ShouldNotBeNull();
            thirdBlastReport.ShouldNotBeNull();
            fourthBlastReport.ShouldNotBeNull();
            fifthBlastReport.ShouldNotBeNull();
            sixthBlastReport.ShouldNotBeNull();
            firstBlastReport.ShouldNotBeSameAs(secondBlastReport);
            thirdBlastReport.ShouldNotBeSameAs(firstBlastReport);
            fourthBlastReport.ShouldNotBeSameAs(firstBlastReport);
            fifthBlastReport.ShouldNotBeSameAs(firstBlastReport);
            sixthBlastReport.ShouldNotBeSameAs(firstBlastReport);
            sixthBlastReport.ShouldNotBeSameAs(fourthBlastReport);
        }

        #endregion

        #endregion

        #endregion
    }
}