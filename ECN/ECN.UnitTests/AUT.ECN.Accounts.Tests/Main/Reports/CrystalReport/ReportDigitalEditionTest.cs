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
    public class ReportDigitalEditionTest : AbstractGenericTest
    {
        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_rpt_DigitalEdition_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstrptDigitalEdition = new rpt_DigitalEdition();
            var secondrptDigitalEdition = new rpt_DigitalEdition();
            var thirdrptDigitalEdition = new rpt_DigitalEdition();
            var fourthrptDigitalEdition = new rpt_DigitalEdition();
            var fifthrptDigitalEdition = new rpt_DigitalEdition();
            var sixthrptDigitalEdition = new rpt_DigitalEdition();

            // Act, Assert
            firstrptDigitalEdition.ShouldNotBeNull();
            secondrptDigitalEdition.ShouldNotBeNull();
            thirdrptDigitalEdition.ShouldNotBeNull();
            fourthrptDigitalEdition.ShouldNotBeNull();
            fifthrptDigitalEdition.ShouldNotBeNull();
            sixthrptDigitalEdition.ShouldNotBeNull();
            firstrptDigitalEdition.ShouldNotBeSameAs(secondrptDigitalEdition);
            thirdrptDigitalEdition.ShouldNotBeSameAs(firstrptDigitalEdition);
            fourthrptDigitalEdition.ShouldNotBeSameAs(firstrptDigitalEdition);
            fifthrptDigitalEdition.ShouldNotBeSameAs(firstrptDigitalEdition);
            sixthrptDigitalEdition.ShouldNotBeSameAs(firstrptDigitalEdition);
            sixthrptDigitalEdition.ShouldNotBeSameAs(fourthrptDigitalEdition);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_rpt_DigitalEdition_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(action: () => new rpt_DigitalEdition());
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_DigitalEdition_Class_Invalid_Property_FullResourceNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFullResourceName = "FullResourceNameNotPresent";
            var rptDigitalEdition  = new rpt_DigitalEdition();

            // Act , Assert
            Should.NotThrow(action: () => rptDigitalEdition.GetType().GetProperty(propertyNameFullResourceName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_DigitalEdition_Class_Invalid_Property_GroupFooterSection1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupFooterSection1 = "GroupFooterSection1NotPresent";
            var rptDigitalEdition  = new rpt_DigitalEdition();

            // Act , Assert
            Should.NotThrow(action: () => rptDigitalEdition.GetType().GetProperty(propertyNameGroupFooterSection1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_DigitalEdition_Class_Invalid_Property_GroupHeaderSection1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupHeaderSection1 = "GroupHeaderSection1NotPresent";
            var rptDigitalEdition  = new rpt_DigitalEdition();

            // Act , Assert
            Should.NotThrow(action: () => rptDigitalEdition.GetType().GetProperty(propertyNameGroupHeaderSection1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_DigitalEdition_Class_Invalid_Property_GroupHeaderSection2NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupHeaderSection2 = "GroupHeaderSection2NotPresent";
            var rptDigitalEdition  = new rpt_DigitalEdition();

            // Act , Assert
            Should.NotThrow(action: () => rptDigitalEdition.GetType().GetProperty(propertyNameGroupHeaderSection2));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_DigitalEdition_Class_Invalid_Property_NewGeneratorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameNewGenerator = "NewGeneratorNotPresent";
            var rptDigitalEdition  = new rpt_DigitalEdition();

            // Act , Assert
            Should.NotThrow(action: () => rptDigitalEdition.GetType().GetProperty(propertyNameNewGenerator));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_DigitalEdition_Class_Invalid_Property_Parameter_MonthNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterMonth = "Parameter_MonthNotPresent";
            var rptDigitalEdition  = new rpt_DigitalEdition();

            // Act , Assert
            Should.NotThrow(action: () => rptDigitalEdition.GetType().GetProperty(propertyNameParameterMonth));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_DigitalEdition_Class_Invalid_Property_Parameter_yearNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterYear = "Parameter_yearNotPresent";
            var rptDigitalEdition  = new rpt_DigitalEdition();

            // Act , Assert
            Should.NotThrow(action: () => rptDigitalEdition.GetType().GetProperty(propertyNameParameterYear));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_DigitalEdition_Class_Invalid_Property_ResourceNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameResourceName = "ResourceNameNotPresent";
            var rptDigitalEdition  = new rpt_DigitalEdition();

            // Act , Assert
            Should.NotThrow(action: () => rptDigitalEdition.GetType().GetProperty(propertyNameResourceName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_DigitalEdition_Class_Invalid_Property_Section1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1NotPresent";
            var rptDigitalEdition  = new rpt_DigitalEdition();

            // Act , Assert
            Should.NotThrow(action: () => rptDigitalEdition.GetType().GetProperty(propertyNameSection1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_DigitalEdition_Class_Invalid_Property_Section2NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2NotPresent";
            var rptDigitalEdition  = new rpt_DigitalEdition();

            // Act , Assert
            Should.NotThrow(action: () => rptDigitalEdition.GetType().GetProperty(propertyNameSection2));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_DigitalEdition_Class_Invalid_Property_Section3NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3NotPresent";
            var rptDigitalEdition  = new rpt_DigitalEdition();

            // Act , Assert
            Should.NotThrow(action: () => rptDigitalEdition.GetType().GetProperty(propertyNameSection3));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_DigitalEdition_Class_Invalid_Property_Section4NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4NotPresent";
            var rptDigitalEdition  = new rpt_DigitalEdition();

            // Act , Assert
            Should.NotThrow(action: () => rptDigitalEdition.GetType().GetProperty(propertyNameSection4));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_DigitalEdition_Class_Invalid_Property_Section5NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5NotPresent";
            var rptDigitalEdition  = new rpt_DigitalEdition();

            // Act , Assert
            Should.NotThrow(action: () => rptDigitalEdition.GetType().GetProperty(propertyNameSection5));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_DigitalEdition_FullResourceName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFullResourceName = "FullResourceName";
            var rptDigitalEdition  = new rpt_DigitalEdition();
            var propertyInfo  = rptDigitalEdition.GetType().GetProperty(propertyNameFullResourceName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_DigitalEdition_FullResourceName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var rptDigitalEdition = new rpt_DigitalEdition();
            rptDigitalEdition.FullResourceName = Fixture.Create<string>();
            var stringType = rptDigitalEdition.FullResourceName.GetType();

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
        public void rpt_DigitalEdition_GroupFooterSection1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupFooterSection1 = "GroupFooterSection1";
            var rptDigitalEdition  = new rpt_DigitalEdition();
            var propertyInfo  = rptDigitalEdition.GetType().GetProperty(propertyNameGroupFooterSection1);

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
        public void rpt_DigitalEdition_GroupFooterSection1_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupFooterSection1 = "GroupFooterSection1";
            var rptDigitalEdition = new rpt_DigitalEdition();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptDigitalEdition.GetType().GetProperty(propertyNameGroupFooterSection1);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptDigitalEdition, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_DigitalEdition_GroupHeaderSection1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupHeaderSection1 = "GroupHeaderSection1";
            var rptDigitalEdition  = new rpt_DigitalEdition();
            var propertyInfo  = rptDigitalEdition.GetType().GetProperty(propertyNameGroupHeaderSection1);

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
        public void rpt_DigitalEdition_GroupHeaderSection1_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupHeaderSection1 = "GroupHeaderSection1";
            var rptDigitalEdition = new rpt_DigitalEdition();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptDigitalEdition.GetType().GetProperty(propertyNameGroupHeaderSection1);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptDigitalEdition, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_DigitalEdition_GroupHeaderSection2_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupHeaderSection2 = "GroupHeaderSection2";
            var rptDigitalEdition  = new rpt_DigitalEdition();
            var propertyInfo  = rptDigitalEdition.GetType().GetProperty(propertyNameGroupHeaderSection2);

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
        public void rpt_DigitalEdition_GroupHeaderSection2_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupHeaderSection2 = "GroupHeaderSection2";
            var rptDigitalEdition = new rpt_DigitalEdition();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptDigitalEdition.GetType().GetProperty(propertyNameGroupHeaderSection2);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptDigitalEdition, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_DigitalEdition_NewGenerator_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var rptDigitalEdition = new rpt_DigitalEdition();
            rptDigitalEdition.NewGenerator = Fixture.Create<bool>();
            var boolType = rptDigitalEdition.NewGenerator.GetType();

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
        public void rpt_DigitalEdition_NewGenerator_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameNewGenerator = "NewGenerator";
            var rptDigitalEdition  = new rpt_DigitalEdition();
            var propertyInfo  = rptDigitalEdition.GetType().GetProperty(propertyNameNewGenerator);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_DigitalEdition_Parameter_Month_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterMonth = "Parameter_Month";
            var rptDigitalEdition  = new rpt_DigitalEdition();
            var propertyInfo  = rptDigitalEdition.GetType().GetProperty(propertyNameParameterMonth);

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
        public void rpt_DigitalEdition_Parameter_Month_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterMonth = "Parameter_Month";
            var rptDigitalEdition = new rpt_DigitalEdition();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptDigitalEdition.GetType().GetProperty(propertyNameParameterMonth);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptDigitalEdition, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_DigitalEdition_Parameter_year_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterYear = "Parameter_year";
            var rptDigitalEdition  = new rpt_DigitalEdition();
            var propertyInfo  = rptDigitalEdition.GetType().GetProperty(propertyNameParameterYear);

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
        public void rpt_DigitalEdition_Parameter_year_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterYear = "Parameter_year";
            var rptDigitalEdition = new rpt_DigitalEdition();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptDigitalEdition.GetType().GetProperty(propertyNameParameterYear);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptDigitalEdition, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_DigitalEdition_ResourceName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameResourceName = "ResourceName";
            var rptDigitalEdition  = new rpt_DigitalEdition();
            var propertyInfo  = rptDigitalEdition.GetType().GetProperty(propertyNameResourceName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_DigitalEdition_ResourceName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var rptDigitalEdition = new rpt_DigitalEdition();
            rptDigitalEdition.ResourceName = Fixture.Create<string>();
            var stringType = rptDigitalEdition.ResourceName.GetType();

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
        public void rpt_DigitalEdition_Section1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1";
            var rptDigitalEdition  = new rpt_DigitalEdition();
            var propertyInfo  = rptDigitalEdition.GetType().GetProperty(propertyNameSection1);

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
        public void rpt_DigitalEdition_Section1_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1";
            var rptDigitalEdition = new rpt_DigitalEdition();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptDigitalEdition.GetType().GetProperty(propertyNameSection1);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptDigitalEdition, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_DigitalEdition_Section2_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2";
            var rptDigitalEdition  = new rpt_DigitalEdition();
            var propertyInfo  = rptDigitalEdition.GetType().GetProperty(propertyNameSection2);

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
        public void rpt_DigitalEdition_Section2_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2";
            var rptDigitalEdition = new rpt_DigitalEdition();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptDigitalEdition.GetType().GetProperty(propertyNameSection2);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptDigitalEdition, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_DigitalEdition_Section3_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3";
            var rptDigitalEdition  = new rpt_DigitalEdition();
            var propertyInfo  = rptDigitalEdition.GetType().GetProperty(propertyNameSection3);

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
        public void rpt_DigitalEdition_Section3_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3";
            var rptDigitalEdition = new rpt_DigitalEdition();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptDigitalEdition.GetType().GetProperty(propertyNameSection3);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptDigitalEdition, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_DigitalEdition_Section4_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4";
            var rptDigitalEdition  = new rpt_DigitalEdition();
            var propertyInfo  = rptDigitalEdition.GetType().GetProperty(propertyNameSection4);

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
        public void rpt_DigitalEdition_Section4_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4";
            var rptDigitalEdition = new rpt_DigitalEdition();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptDigitalEdition.GetType().GetProperty(propertyNameSection4);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptDigitalEdition, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_DigitalEdition_Section5_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5";
            var rptDigitalEdition  = new rpt_DigitalEdition();
            var propertyInfo  = rptDigitalEdition.GetType().GetProperty(propertyNameSection5);

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
        public void rpt_DigitalEdition_Section5_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5";
            var rptDigitalEdition = new rpt_DigitalEdition();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptDigitalEdition.GetType().GetProperty(propertyNameSection5);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptDigitalEdition, randomString, null));
        }
    }
}