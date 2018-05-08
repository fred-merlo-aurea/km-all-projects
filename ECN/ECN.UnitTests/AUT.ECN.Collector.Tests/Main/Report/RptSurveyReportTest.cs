using System;
using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using AUT.ConfigureTestProjects;
using ecn.collector.main.report;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Collector.Tests.Main.Report
{
    [TestFixture]
    public class RptSurveyReportTest : AbstractGenericTest
    {
        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_rpt_SurveyReport_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstrptSurveyReport = new rpt_SurveyReport();
            var secondrptSurveyReport = new rpt_SurveyReport();
            var thirdrptSurveyReport = new rpt_SurveyReport();
            var fourthrptSurveyReport = new rpt_SurveyReport();
            var fifthrptSurveyReport = new rpt_SurveyReport();
            var sixthrptSurveyReport = new rpt_SurveyReport();

            // Act, Assert
            firstrptSurveyReport.ShouldNotBeNull();
            secondrptSurveyReport.ShouldNotBeNull();
            thirdrptSurveyReport.ShouldNotBeNull();
            fourthrptSurveyReport.ShouldNotBeNull();
            fifthrptSurveyReport.ShouldNotBeNull();
            sixthrptSurveyReport.ShouldNotBeNull();
            firstrptSurveyReport.ShouldNotBeSameAs(secondrptSurveyReport);
            thirdrptSurveyReport.ShouldNotBeSameAs(firstrptSurveyReport);
            fourthrptSurveyReport.ShouldNotBeSameAs(firstrptSurveyReport);
            fifthrptSurveyReport.ShouldNotBeSameAs(firstrptSurveyReport);
            sixthrptSurveyReport.ShouldNotBeSameAs(firstrptSurveyReport);
            sixthrptSurveyReport.ShouldNotBeSameAs(fourthrptSurveyReport);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_rpt_SurveyReport_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(action: () => new rpt_SurveyReport());
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var rptSurveyReport = new rpt_SurveyReport();
            var resourceName = Fixture.Create<string>();
            var newGenerator = true;
            var fullResourceName = Fixture.Create<string>();

            // Act
            rptSurveyReport.ResourceName = resourceName;
            rptSurveyReport.NewGenerator = newGenerator;
            rptSurveyReport.FullResourceName = fullResourceName;

            // Assert
            rptSurveyReport.ResourceName.ShouldNotBeNull();
            rptSurveyReport.NewGenerator.ShouldBe(newGenerator);
            rptSurveyReport.FullResourceName.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Class_Invalid_Property_DetailSection2NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDetailSection2 = "DetailSection2NotPresent";
            var rptSurveyReport  = new rpt_SurveyReport();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyReport.GetType().GetProperty(propertyNameDetailSection2));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Class_Invalid_Property_DetailSection3NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDetailSection3 = "DetailSection3NotPresent";
            var rptSurveyReport  = new rpt_SurveyReport();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyReport.GetType().GetProperty(propertyNameDetailSection3));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Class_Invalid_Property_FullResourceNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFullResourceName = "FullResourceNameNotPresent";
            var rptSurveyReport  = new rpt_SurveyReport();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyReport.GetType().GetProperty(propertyNameFullResourceName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Class_Invalid_Property_GroupFooterSection1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupFooterSection1 = "GroupFooterSection1NotPresent";
            var rptSurveyReport  = new rpt_SurveyReport();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyReport.GetType().GetProperty(propertyNameGroupFooterSection1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Class_Invalid_Property_GroupHeaderSection1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupHeaderSection1 = "GroupHeaderSection1NotPresent";
            var rptSurveyReport  = new rpt_SurveyReport();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyReport.GetType().GetProperty(propertyNameGroupHeaderSection1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Class_Invalid_Property_NewGeneratorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameNewGenerator = "NewGeneratorNotPresent";
            var rptSurveyReport  = new rpt_SurveyReport();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyReport.GetType().GetProperty(propertyNameNewGenerator));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Class_Invalid_Property_Parameter_EmailIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterEmailId = "Parameter_EmailIDNotPresent";
            var rptSurveyReport  = new rpt_SurveyReport();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyReport.GetType().GetProperty(propertyNameParameterEmailId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Class_Invalid_Property_Parameter_ExportFormatNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterExportFormat = "Parameter_ExportFormatNotPresent";
            var rptSurveyReport  = new rpt_SurveyReport();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyReport.GetType().GetProperty(propertyNameParameterExportFormat));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Class_Invalid_Property_Parameter_FilterstrNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterFilterstr = "Parameter_FilterstrNotPresent";
            var rptSurveyReport  = new rpt_SurveyReport();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyReport.GetType().GetProperty(propertyNameParameterFilterstr));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Class_Invalid_Property_Parameter_percentusingNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterPercentusing = "Parameter_percentusingNotPresent";
            var rptSurveyReport  = new rpt_SurveyReport();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyReport.GetType().GetProperty(propertyNameParameterPercentusing));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Class_Invalid_Property_Parameter_SurveyIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterSurveyId = "Parameter_SurveyIDNotPresent";
            var rptSurveyReport  = new rpt_SurveyReport();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyReport.GetType().GetProperty(propertyNameParameterSurveyId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Class_Invalid_Property_Parameter_titleNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterTitle = "Parameter_titleNotPresent";
            var rptSurveyReport  = new rpt_SurveyReport();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyReport.GetType().GetProperty(propertyNameParameterTitle));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Class_Invalid_Property_Parameter_totalRespondentNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterTotalRespondent = "Parameter_totalRespondentNotPresent";
            var rptSurveyReport  = new rpt_SurveyReport();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyReport.GetType().GetProperty(propertyNameParameterTotalRespondent));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Class_Invalid_Property_ReportHeaderSection2NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameReportHeaderSection2 = "ReportHeaderSection2NotPresent";
            var rptSurveyReport  = new rpt_SurveyReport();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyReport.GetType().GetProperty(propertyNameReportHeaderSection2));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Class_Invalid_Property_ResourceNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameResourceName = "ResourceNameNotPresent";
            var rptSurveyReport  = new rpt_SurveyReport();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyReport.GetType().GetProperty(propertyNameResourceName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Class_Invalid_Property_Section1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1NotPresent";
            var rptSurveyReport  = new rpt_SurveyReport();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyReport.GetType().GetProperty(propertyNameSection1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Class_Invalid_Property_Section2NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2NotPresent";
            var rptSurveyReport  = new rpt_SurveyReport();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyReport.GetType().GetProperty(propertyNameSection2));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Class_Invalid_Property_Section3NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3NotPresent";
            var rptSurveyReport  = new rpt_SurveyReport();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyReport.GetType().GetProperty(propertyNameSection3));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Class_Invalid_Property_Section4NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4NotPresent";
            var rptSurveyReport  = new rpt_SurveyReport();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyReport.GetType().GetProperty(propertyNameSection4));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Class_Invalid_Property_Section5NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5NotPresent";
            var rptSurveyReport  = new rpt_SurveyReport();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyReport.GetType().GetProperty(propertyNameSection5));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_DetailSection2_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDetailSection2 = "DetailSection2";
            var rptSurveyReport  = new rpt_SurveyReport();
            var propertyInfo  = rptSurveyReport.GetType().GetProperty(propertyNameDetailSection2);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_DetailSection2_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameDetailSection2 = "DetailSection2";
            var rptSurveyReport = new rpt_SurveyReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyReport.GetType().GetProperty(propertyNameDetailSection2);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyReport, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_DetailSection3_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDetailSection3 = "DetailSection3";
            var rptSurveyReport  = new rpt_SurveyReport();
            var propertyInfo  = rptSurveyReport.GetType().GetProperty(propertyNameDetailSection3);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_DetailSection3_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameDetailSection3 = "DetailSection3";
            var rptSurveyReport = new rpt_SurveyReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyReport.GetType().GetProperty(propertyNameDetailSection3);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyReport, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_FullResourceName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFullResourceName = "FullResourceName";
            var rptSurveyReport  = new rpt_SurveyReport();
            var propertyInfo  = rptSurveyReport.GetType().GetProperty(propertyNameFullResourceName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_FullResourceName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var rptSurveyReport = new rpt_SurveyReport();
            rptSurveyReport.FullResourceName = Fixture.Create<string>();
            var stringType = rptSurveyReport.FullResourceName.GetType();

            // Act
            var isTypeString = typeof(string) == stringType;
            var isTypeInt = typeof(int) == stringType;
            var isTypeDecimal = typeof(decimal) == stringType;
            var isTypeLong = typeof(long) == stringType;
            var isTypeBool = typeof(bool) == stringType;
            var isTypeDouble = typeof(double) == stringType;
            var isTypeFloat = typeof(float) == stringType;

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_GroupFooterSection1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupFooterSection1 = "GroupFooterSection1";
            var rptSurveyReport  = new rpt_SurveyReport();
            var propertyInfo  = rptSurveyReport.GetType().GetProperty(propertyNameGroupFooterSection1);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_GroupFooterSection1_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupFooterSection1 = "GroupFooterSection1";
            var rptSurveyReport = new rpt_SurveyReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyReport.GetType().GetProperty(propertyNameGroupFooterSection1);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyReport, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_GroupHeaderSection1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupHeaderSection1 = "GroupHeaderSection1";
            var rptSurveyReport  = new rpt_SurveyReport();
            var propertyInfo  = rptSurveyReport.GetType().GetProperty(propertyNameGroupHeaderSection1);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_GroupHeaderSection1_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupHeaderSection1 = "GroupHeaderSection1";
            var rptSurveyReport = new rpt_SurveyReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyReport.GetType().GetProperty(propertyNameGroupHeaderSection1);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyReport, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_NewGenerator_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var rptSurveyReport = new rpt_SurveyReport();
            rptSurveyReport.NewGenerator = Fixture.Create<bool>();
            var boolType = rptSurveyReport.NewGenerator.GetType();

            // Act
            var isTypeBool = typeof(bool) == boolType;
            var isTypeNullableBool = typeof(bool?) == boolType;
            var isTypeString = typeof(string) == boolType;
            var isTypeInt = typeof(int) == boolType;
            var isTypeDecimal = typeof(decimal) == boolType;
            var isTypeLong = typeof(long) == boolType;
            var isTypeDouble = typeof(double) == boolType;
            var isTypeFloat = typeof(float) == boolType;
            var isTypeIntNullable = typeof(int?) == boolType;
            var isTypeDecimalNullable = typeof(decimal?) == boolType;
            var isTypeLongNullable = typeof(long?) == boolType;
            var isTypeDoubleNullable = typeof(double?) == boolType;
            var isTypeFloatNullable = typeof(float?) == boolType;

            // Assert
            isTypeBool.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableBool.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_NewGenerator_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameNewGenerator = "NewGenerator";
            var rptSurveyReport  = new rpt_SurveyReport();
            var propertyInfo  = rptSurveyReport.GetType().GetProperty(propertyNameNewGenerator);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Parameter_EmailID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterEmailId = "Parameter_EmailID";
            var rptSurveyReport  = new rpt_SurveyReport();
            var propertyInfo  = rptSurveyReport.GetType().GetProperty(propertyNameParameterEmailId);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Parameter_EmailID_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterEmailId = "Parameter_EmailID";
            var rptSurveyReport = new rpt_SurveyReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyReport.GetType().GetProperty(propertyNameParameterEmailId);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyReport, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Parameter_ExportFormat_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterExportFormat = "Parameter_ExportFormat";
            var rptSurveyReport  = new rpt_SurveyReport();
            var propertyInfo  = rptSurveyReport.GetType().GetProperty(propertyNameParameterExportFormat);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Parameter_ExportFormat_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterExportFormat = "Parameter_ExportFormat";
            var rptSurveyReport = new rpt_SurveyReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyReport.GetType().GetProperty(propertyNameParameterExportFormat);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyReport, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Parameter_Filterstr_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterFilterstr = "Parameter_Filterstr";
            var rptSurveyReport  = new rpt_SurveyReport();
            var propertyInfo  = rptSurveyReport.GetType().GetProperty(propertyNameParameterFilterstr);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Parameter_Filterstr_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterFilterstr = "Parameter_Filterstr";
            var rptSurveyReport = new rpt_SurveyReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyReport.GetType().GetProperty(propertyNameParameterFilterstr);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyReport, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Parameter_percentusing_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterPercentusing = "Parameter_percentusing";
            var rptSurveyReport  = new rpt_SurveyReport();
            var propertyInfo  = rptSurveyReport.GetType().GetProperty(propertyNameParameterPercentusing);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Parameter_percentusing_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterPercentusing = "Parameter_percentusing";
            var rptSurveyReport = new rpt_SurveyReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyReport.GetType().GetProperty(propertyNameParameterPercentusing);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyReport, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Parameter_SurveyID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterSurveyId = "Parameter_SurveyID";
            var rptSurveyReport  = new rpt_SurveyReport();
            var propertyInfo  = rptSurveyReport.GetType().GetProperty(propertyNameParameterSurveyId);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Parameter_SurveyID_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterSurveyId = "Parameter_SurveyID";
            var rptSurveyReport = new rpt_SurveyReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyReport.GetType().GetProperty(propertyNameParameterSurveyId);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyReport, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Parameter_title_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterTitle = "Parameter_title";
            var rptSurveyReport  = new rpt_SurveyReport();
            var propertyInfo  = rptSurveyReport.GetType().GetProperty(propertyNameParameterTitle);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Parameter_title_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterTitle = "Parameter_title";
            var rptSurveyReport = new rpt_SurveyReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyReport.GetType().GetProperty(propertyNameParameterTitle);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyReport, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Parameter_totalRespondent_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterTotalRespondent = "Parameter_totalRespondent";
            var rptSurveyReport  = new rpt_SurveyReport();
            var propertyInfo  = rptSurveyReport.GetType().GetProperty(propertyNameParameterTotalRespondent);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Parameter_totalRespondent_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterTotalRespondent = "Parameter_totalRespondent";
            var rptSurveyReport = new rpt_SurveyReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyReport.GetType().GetProperty(propertyNameParameterTotalRespondent);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyReport, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_ReportHeaderSection2_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameReportHeaderSection2 = "ReportHeaderSection2";
            var rptSurveyReport  = new rpt_SurveyReport();
            var propertyInfo  = rptSurveyReport.GetType().GetProperty(propertyNameReportHeaderSection2);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_ReportHeaderSection2_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameReportHeaderSection2 = "ReportHeaderSection2";
            var rptSurveyReport = new rpt_SurveyReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyReport.GetType().GetProperty(propertyNameReportHeaderSection2);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyReport, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_ResourceName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameResourceName = "ResourceName";
            var rptSurveyReport  = new rpt_SurveyReport();
            var propertyInfo  = rptSurveyReport.GetType().GetProperty(propertyNameResourceName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_ResourceName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var rptSurveyReport = new rpt_SurveyReport();
            rptSurveyReport.ResourceName = Fixture.Create<string>();
            var stringType = rptSurveyReport.ResourceName.GetType();

            // Act
            var isTypeString = typeof(string) == stringType;
            var isTypeInt = typeof(int) == stringType;
            var isTypeDecimal = typeof(decimal) == stringType;
            var isTypeLong = typeof(long) == stringType;
            var isTypeBool = typeof(bool) == stringType;
            var isTypeDouble = typeof(double) == stringType;
            var isTypeFloat = typeof(float) == stringType;

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Section1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1";
            var rptSurveyReport  = new rpt_SurveyReport();
            var propertyInfo  = rptSurveyReport.GetType().GetProperty(propertyNameSection1);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Section1_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1";
            var rptSurveyReport = new rpt_SurveyReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyReport.GetType().GetProperty(propertyNameSection1);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyReport, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Section2_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2";
            var rptSurveyReport  = new rpt_SurveyReport();
            var propertyInfo  = rptSurveyReport.GetType().GetProperty(propertyNameSection2);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Section2_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2";
            var rptSurveyReport = new rpt_SurveyReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyReport.GetType().GetProperty(propertyNameSection2);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyReport, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Section3_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3";
            var rptSurveyReport  = new rpt_SurveyReport();
            var propertyInfo  = rptSurveyReport.GetType().GetProperty(propertyNameSection3);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Section3_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3";
            var rptSurveyReport = new rpt_SurveyReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyReport.GetType().GetProperty(propertyNameSection3);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyReport, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Section4_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4";
            var rptSurveyReport  = new rpt_SurveyReport();
            var propertyInfo  = rptSurveyReport.GetType().GetProperty(propertyNameSection4);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Section4_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4";
            var rptSurveyReport = new rpt_SurveyReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyReport.GetType().GetProperty(propertyNameSection4);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyReport, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Section5_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5";
            var rptSurveyReport  = new rpt_SurveyReport();
            var propertyInfo  = rptSurveyReport.GetType().GetProperty(propertyNameSection5);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyReport_Section5_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5";
            var rptSurveyReport = new rpt_SurveyReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyReport.GetType().GetProperty(propertyNameSection5);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyReport, randomString, null));
        }
    }
}