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
    public class ReportEcnTodayTest : AbstractGenericTest
    {
        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_rpt_ECNToday_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstrptEcnToday = new rpt_ECNToday();
            var secondrptEcnToday = new rpt_ECNToday();
            var thirdrptEcnToday = new rpt_ECNToday();
            var fourthrptEcnToday = new rpt_ECNToday();
            var fifthrptEcnToday = new rpt_ECNToday();
            var sixthrptEcnToday = new rpt_ECNToday();

            // Act, Assert
            firstrptEcnToday.ShouldNotBeNull();
            secondrptEcnToday.ShouldNotBeNull();
            thirdrptEcnToday.ShouldNotBeNull();
            fourthrptEcnToday.ShouldNotBeNull();
            fifthrptEcnToday.ShouldNotBeNull();
            sixthrptEcnToday.ShouldNotBeNull();
            firstrptEcnToday.ShouldNotBeSameAs(secondrptEcnToday);
            thirdrptEcnToday.ShouldNotBeSameAs(firstrptEcnToday);
            fourthrptEcnToday.ShouldNotBeSameAs(firstrptEcnToday);
            fifthrptEcnToday.ShouldNotBeSameAs(firstrptEcnToday);
            sixthrptEcnToday.ShouldNotBeSameAs(firstrptEcnToday);
            sixthrptEcnToday.ShouldNotBeSameAs(fourthrptEcnToday);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_rpt_ECNToday_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(action: () => new rpt_ECNToday());
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ECNToday_Class_Invalid_Property_FullResourceNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFullResourceName = "FullResourceNameNotPresent";
            var rptEcnToday  = new rpt_ECNToday();

            // Act , Assert
            Should.NotThrow(action: () => rptEcnToday.GetType().GetProperty(propertyNameFullResourceName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ECNToday_Class_Invalid_Property_NewGeneratorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameNewGenerator = "NewGeneratorNotPresent";
            var rptEcnToday  = new rpt_ECNToday();

            // Act , Assert
            Should.NotThrow(action: () => rptEcnToday.GetType().GetProperty(propertyNameNewGenerator));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ECNToday_Class_Invalid_Property_Parameter_MonthNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterMonth = "Parameter_MonthNotPresent";
            var rptEcnToday  = new rpt_ECNToday();

            // Act , Assert
            Should.NotThrow(action: () => rptEcnToday.GetType().GetProperty(propertyNameParameterMonth));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ECNToday_Class_Invalid_Property_Parameter_testblastNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterTestblast = "Parameter_testblastNotPresent";
            var rptEcnToday  = new rpt_ECNToday();

            // Act , Assert
            Should.NotThrow(action: () => rptEcnToday.GetType().GetProperty(propertyNameParameterTestblast));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ECNToday_Class_Invalid_Property_Parameter_yearNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterYear = "Parameter_yearNotPresent";
            var rptEcnToday  = new rpt_ECNToday();

            // Act , Assert
            Should.NotThrow(action: () => rptEcnToday.GetType().GetProperty(propertyNameParameterYear));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ECNToday_Class_Invalid_Property_ResourceNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameResourceName = "ResourceNameNotPresent";
            var rptEcnToday  = new rpt_ECNToday();

            // Act , Assert
            Should.NotThrow(action: () => rptEcnToday.GetType().GetProperty(propertyNameResourceName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ECNToday_Class_Invalid_Property_Section1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1NotPresent";
            var rptEcnToday  = new rpt_ECNToday();

            // Act , Assert
            Should.NotThrow(action: () => rptEcnToday.GetType().GetProperty(propertyNameSection1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ECNToday_Class_Invalid_Property_Section2NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2NotPresent";
            var rptEcnToday  = new rpt_ECNToday();

            // Act , Assert
            Should.NotThrow(action: () => rptEcnToday.GetType().GetProperty(propertyNameSection2));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ECNToday_Class_Invalid_Property_Section3NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3NotPresent";
            var rptEcnToday  = new rpt_ECNToday();

            // Act , Assert
            Should.NotThrow(action: () => rptEcnToday.GetType().GetProperty(propertyNameSection3));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ECNToday_Class_Invalid_Property_Section4NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4NotPresent";
            var rptEcnToday  = new rpt_ECNToday();

            // Act , Assert
            Should.NotThrow(action: () => rptEcnToday.GetType().GetProperty(propertyNameSection4));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ECNToday_Class_Invalid_Property_Section5NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5NotPresent";
            var rptEcnToday  = new rpt_ECNToday();

            // Act , Assert
            Should.NotThrow(action: () => rptEcnToday.GetType().GetProperty(propertyNameSection5));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ECNToday_FullResourceName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFullResourceName = "FullResourceName";
            var rptEcnToday  = new rpt_ECNToday();
            var propertyInfo  = rptEcnToday.GetType().GetProperty(propertyNameFullResourceName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ECNToday_FullResourceName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var rptEcnToday = new rpt_ECNToday();
            rptEcnToday.FullResourceName = Fixture.Create<string>();
            var stringType = rptEcnToday.FullResourceName.GetType();

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
        public void rpt_ECNToday_NewGenerator_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var rptEcnToday = new rpt_ECNToday();
            rptEcnToday.NewGenerator = Fixture.Create<bool>();
            var boolType = rptEcnToday.NewGenerator.GetType();

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
        public void rpt_ECNToday_NewGenerator_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameNewGenerator = "NewGenerator";
            var rptEcnToday  = new rpt_ECNToday();
            var propertyInfo  = rptEcnToday.GetType().GetProperty(propertyNameNewGenerator);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ECNToday_Parameter_Month_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterMonth = "Parameter_Month";
            var rptEcnToday  = new rpt_ECNToday();
            var propertyInfo  = rptEcnToday.GetType().GetProperty(propertyNameParameterMonth);

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
        public void rpt_ECNToday_Parameter_Month_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterMonth = "Parameter_Month";
            var rptEcnToday = new rpt_ECNToday();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptEcnToday.GetType().GetProperty(propertyNameParameterMonth);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptEcnToday, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ECNToday_Parameter_testblast_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterTestblast = "Parameter_testblast";
            var rptEcnToday  = new rpt_ECNToday();
            var propertyInfo  = rptEcnToday.GetType().GetProperty(propertyNameParameterTestblast);

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
        public void rpt_ECNToday_Parameter_testblast_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterTestblast = "Parameter_testblast";
            var rptEcnToday = new rpt_ECNToday();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptEcnToday.GetType().GetProperty(propertyNameParameterTestblast);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptEcnToday, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ECNToday_Parameter_year_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterYear = "Parameter_year";
            var rptEcnToday  = new rpt_ECNToday();
            var propertyInfo  = rptEcnToday.GetType().GetProperty(propertyNameParameterYear);

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
        public void rpt_ECNToday_Parameter_year_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterYear = "Parameter_year";
            var rptEcnToday = new rpt_ECNToday();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptEcnToday.GetType().GetProperty(propertyNameParameterYear);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptEcnToday, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ECNToday_ResourceName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameResourceName = "ResourceName";
            var rptEcnToday  = new rpt_ECNToday();
            var propertyInfo  = rptEcnToday.GetType().GetProperty(propertyNameResourceName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ECNToday_ResourceName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var rptEcnToday = new rpt_ECNToday();
            rptEcnToday.ResourceName = Fixture.Create<string>();
            var stringType = rptEcnToday.ResourceName.GetType();

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
        public void rpt_ECNToday_Section1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1";
            var rptEcnToday  = new rpt_ECNToday();
            var propertyInfo  = rptEcnToday.GetType().GetProperty(propertyNameSection1);

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
        public void rpt_ECNToday_Section1_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1";
            var rptEcnToday = new rpt_ECNToday();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptEcnToday.GetType().GetProperty(propertyNameSection1);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptEcnToday, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ECNToday_Section2_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2";
            var rptEcnToday  = new rpt_ECNToday();
            var propertyInfo  = rptEcnToday.GetType().GetProperty(propertyNameSection2);

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
        public void rpt_ECNToday_Section2_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2";
            var rptEcnToday = new rpt_ECNToday();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptEcnToday.GetType().GetProperty(propertyNameSection2);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptEcnToday, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ECNToday_Section3_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3";
            var rptEcnToday  = new rpt_ECNToday();
            var propertyInfo  = rptEcnToday.GetType().GetProperty(propertyNameSection3);

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
        public void rpt_ECNToday_Section3_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3";
            var rptEcnToday = new rpt_ECNToday();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptEcnToday.GetType().GetProperty(propertyNameSection3);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptEcnToday, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ECNToday_Section4_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4";
            var rptEcnToday  = new rpt_ECNToday();
            var propertyInfo  = rptEcnToday.GetType().GetProperty(propertyNameSection4);

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
        public void rpt_ECNToday_Section4_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4";
            var rptEcnToday = new rpt_ECNToday();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptEcnToday.GetType().GetProperty(propertyNameSection4);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptEcnToday, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ECNToday_Section5_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5";
            var rptEcnToday  = new rpt_ECNToday();
            var propertyInfo  = rptEcnToday.GetType().GetProperty(propertyNameSection5);

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
        public void rpt_ECNToday_Section5_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5";
            var rptEcnToday = new rpt_ECNToday();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptEcnToday.GetType().GetProperty(propertyNameSection5);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptEcnToday, randomString, null));
        }
    }
}