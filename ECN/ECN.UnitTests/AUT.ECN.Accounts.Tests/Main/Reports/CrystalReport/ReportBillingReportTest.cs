using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using AUT.ConfigureTestProjects;
using ecn.accounts.main.reports.CrystalReport;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests.Main.Reports.CrystalReport
{
    [TestFixture]
    public class ReportBillingReportTest : AbstractGenericTest
    {
        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_rpt_BillingReport_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstrptBillingReport = new rpt_BillingReport();
            var secondrptBillingReport = new rpt_BillingReport();
            var thirdrptBillingReport = new rpt_BillingReport();
            var fourthrptBillingReport = new rpt_BillingReport();
            var fifthrptBillingReport = new rpt_BillingReport();
            var sixthrptBillingReport = new rpt_BillingReport();

            // Act, Assert
            firstrptBillingReport.ShouldNotBeNull();
            secondrptBillingReport.ShouldNotBeNull();
            thirdrptBillingReport.ShouldNotBeNull();
            fourthrptBillingReport.ShouldNotBeNull();
            fifthrptBillingReport.ShouldNotBeNull();
            sixthrptBillingReport.ShouldNotBeNull();
            firstrptBillingReport.ShouldNotBeSameAs(secondrptBillingReport);
            thirdrptBillingReport.ShouldNotBeSameAs(firstrptBillingReport);
            fourthrptBillingReport.ShouldNotBeSameAs(firstrptBillingReport);
            fifthrptBillingReport.ShouldNotBeSameAs(firstrptBillingReport);
            sixthrptBillingReport.ShouldNotBeSameAs(firstrptBillingReport);
            sixthrptBillingReport.ShouldNotBeSameAs(fourthrptBillingReport);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_rpt_BillingReport_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(action: () => new rpt_BillingReport());
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingReport_Class_Invalid_Property_FullResourceNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFullResourceName = "FullResourceNameNotPresent";
            var rptBillingReport  = new rpt_BillingReport();

            // Act , Assert
            Should.NotThrow(action: () => rptBillingReport.GetType().GetProperty(propertyNameFullResourceName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingReport_Class_Invalid_Property_GroupFooterSection1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupFooterSection1 = "GroupFooterSection1NotPresent";
            var rptBillingReport  = new rpt_BillingReport();

            // Act , Assert
            Should.NotThrow(action: () => rptBillingReport.GetType().GetProperty(propertyNameGroupFooterSection1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingReport_Class_Invalid_Property_GroupHeaderSection1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupHeaderSection1 = "GroupHeaderSection1NotPresent";
            var rptBillingReport  = new rpt_BillingReport();

            // Act , Assert
            Should.NotThrow(action: () => rptBillingReport.GetType().GetProperty(propertyNameGroupHeaderSection1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingReport_Class_Invalid_Property_GroupHeaderSection2NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupHeaderSection2 = "GroupHeaderSection2NotPresent";
            var rptBillingReport  = new rpt_BillingReport();

            // Act , Assert
            Should.NotThrow(action: () => rptBillingReport.GetType().GetProperty(propertyNameGroupHeaderSection2));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingReport_Class_Invalid_Property_NewGeneratorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameNewGenerator = "NewGeneratorNotPresent";
            var rptBillingReport  = new rpt_BillingReport();

            // Act , Assert
            Should.NotThrow(action: () => rptBillingReport.GetType().GetProperty(propertyNameNewGenerator));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingReport_Class_Invalid_Property_Parameter_channelIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterChannelId = "Parameter_channelIDNotPresent";
            var rptBillingReport  = new rpt_BillingReport();

            // Act , Assert
            Should.NotThrow(action: () => rptBillingReport.GetType().GetProperty(propertyNameParameterChannelId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingReport_Class_Invalid_Property_Parameter_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterCustomerId = "Parameter_CustomerIDNotPresent";
            var rptBillingReport  = new rpt_BillingReport();

            // Act , Assert
            Should.NotThrow(action: () => rptBillingReport.GetType().GetProperty(propertyNameParameterCustomerId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingReport_Class_Invalid_Property_Parameter_monthNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterMonth = "Parameter_monthNotPresent";
            var rptBillingReport  = new rpt_BillingReport();

            // Act , Assert
            Should.NotThrow(action: () => rptBillingReport.GetType().GetProperty(propertyNameParameterMonth));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingReport_Class_Invalid_Property_Parameter_yearNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterYear = "Parameter_yearNotPresent";
            var rptBillingReport  = new rpt_BillingReport();

            // Act , Assert
            Should.NotThrow(action: () => rptBillingReport.GetType().GetProperty(propertyNameParameterYear));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingReport_Class_Invalid_Property_ResourceNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameResourceName = "ResourceNameNotPresent";
            var rptBillingReport  = new rpt_BillingReport();

            // Act , Assert
            Should.NotThrow(action: () => rptBillingReport.GetType().GetProperty(propertyNameResourceName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingReport_Class_Invalid_Property_Section1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1NotPresent";
            var rptBillingReport  = new rpt_BillingReport();

            // Act , Assert
            Should.NotThrow(action: () => rptBillingReport.GetType().GetProperty(propertyNameSection1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingReport_Class_Invalid_Property_Section2NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2NotPresent";
            var rptBillingReport  = new rpt_BillingReport();

            // Act , Assert
            Should.NotThrow(action: () => rptBillingReport.GetType().GetProperty(propertyNameSection2));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingReport_Class_Invalid_Property_Section3NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3NotPresent";
            var rptBillingReport  = new rpt_BillingReport();

            // Act , Assert
            Should.NotThrow(action: () => rptBillingReport.GetType().GetProperty(propertyNameSection3));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingReport_Class_Invalid_Property_Section4NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4NotPresent";
            var rptBillingReport  = new rpt_BillingReport();

            // Act , Assert
            Should.NotThrow(action: () => rptBillingReport.GetType().GetProperty(propertyNameSection4));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingReport_Class_Invalid_Property_Section5NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5NotPresent";
            var rptBillingReport  = new rpt_BillingReport();

            // Act , Assert
            Should.NotThrow(action: () => rptBillingReport.GetType().GetProperty(propertyNameSection5));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingReport_FullResourceName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFullResourceName = "FullResourceName";
            var rptBillingReport  = new rpt_BillingReport();
            var propertyInfo  = rptBillingReport.GetType().GetProperty(propertyNameFullResourceName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingReport_FullResourceName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var rptBillingReport = new rpt_BillingReport();
            rptBillingReport.FullResourceName = Fixture.Create<string>();
            var stringType = rptBillingReport.FullResourceName.GetType();

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
        public void rpt_BillingReport_GroupFooterSection1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupFooterSection1 = "GroupFooterSection1";
            var rptBillingReport  = new rpt_BillingReport();
            var propertyInfo  = rptBillingReport.GetType().GetProperty(propertyNameGroupFooterSection1);

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
        public void rpt_BillingReport_GroupFooterSection1_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupFooterSection1 = "GroupFooterSection1";
            var rptBillingReport = new rpt_BillingReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptBillingReport.GetType().GetProperty(propertyNameGroupFooterSection1);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptBillingReport, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingReport_GroupHeaderSection1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupHeaderSection1 = "GroupHeaderSection1";
            var rptBillingReport  = new rpt_BillingReport();
            var propertyInfo  = rptBillingReport.GetType().GetProperty(propertyNameGroupHeaderSection1);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingReport_Parameter_month_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterMonth = "Parameter_month";
            var rptBillingReport  = new rpt_BillingReport();
            var propertyInfo  = rptBillingReport.GetType().GetProperty(propertyNameParameterMonth);

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
        public void rpt_BillingReport_Parameter_month_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterMonth = "Parameter_month";
            var rptBillingReport = new rpt_BillingReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptBillingReport.GetType().GetProperty(propertyNameParameterMonth);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptBillingReport, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingReport_Parameter_year_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterYear = "Parameter_year";
            var rptBillingReport  = new rpt_BillingReport();
            var propertyInfo  = rptBillingReport.GetType().GetProperty(propertyNameParameterYear);

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
        public void rpt_BillingReport_Parameter_year_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterYear = "Parameter_year";
            var rptBillingReport = new rpt_BillingReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptBillingReport.GetType().GetProperty(propertyNameParameterYear);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptBillingReport, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingReport_ResourceName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameResourceName = "ResourceName";
            var rptBillingReport  = new rpt_BillingReport();
            var propertyInfo  = rptBillingReport.GetType().GetProperty(propertyNameResourceName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingReport_ResourceName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var rptBillingReport = new rpt_BillingReport();
            rptBillingReport.ResourceName = Fixture.Create<string>();
            var stringType = rptBillingReport.ResourceName.GetType();

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
        public void rpt_BillingReport_Section1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1";
            var rptBillingReport  = new rpt_BillingReport();
            var propertyInfo  = rptBillingReport.GetType().GetProperty(propertyNameSection1);

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
        public void rpt_BillingReport_Section1_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1";
            var rptBillingReport = new rpt_BillingReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptBillingReport.GetType().GetProperty(propertyNameSection1);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptBillingReport, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingReport_Section2_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2";
            var rptBillingReport  = new rpt_BillingReport();
            var propertyInfo  = rptBillingReport.GetType().GetProperty(propertyNameSection2);

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
        public void rpt_BillingReport_Section2_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2";
            var rptBillingReport = new rpt_BillingReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptBillingReport.GetType().GetProperty(propertyNameSection2);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptBillingReport, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingReport_Section3_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3";
            var rptBillingReport  = new rpt_BillingReport();
            var propertyInfo  = rptBillingReport.GetType().GetProperty(propertyNameSection3);

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
        public void rpt_BillingReport_Section3_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3";
            var rptBillingReport = new rpt_BillingReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptBillingReport.GetType().GetProperty(propertyNameSection3);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptBillingReport, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingReport_Section4_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4";
            var rptBillingReport  = new rpt_BillingReport();
            var propertyInfo  = rptBillingReport.GetType().GetProperty(propertyNameSection4);

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
        public void rpt_BillingReport_Section4_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4";
            var rptBillingReport = new rpt_BillingReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptBillingReport.GetType().GetProperty(propertyNameSection4);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptBillingReport, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingReport_Section5_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5";
            var rptBillingReport  = new rpt_BillingReport();
            var propertyInfo  = rptBillingReport.GetType().GetProperty(propertyNameSection5);

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
        public void rpt_BillingReport_Section5_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5";
            var rptBillingReport = new rpt_BillingReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptBillingReport.GetType().GetProperty(propertyNameSection5);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptBillingReport, randomString, null));
        }
    }
}