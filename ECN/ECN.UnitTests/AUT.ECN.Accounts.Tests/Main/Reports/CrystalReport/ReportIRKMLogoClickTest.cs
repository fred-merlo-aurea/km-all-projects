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
    public class ReportIrkmLogoClickTest : AbstractGenericTest
    {
        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_rpt_IRKMLogoClick_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstrptIrkmLogoClick = new rpt_IRKMLogoClick();
            var secondrptIrkmLogoClick = new rpt_IRKMLogoClick();
            var thirdrptIrkmLogoClick = new rpt_IRKMLogoClick();
            var fourthrptIrkmLogoClick = new rpt_IRKMLogoClick();
            var fifthrptIrkmLogoClick = new rpt_IRKMLogoClick();
            var sixthrptIrkmLogoClick = new rpt_IRKMLogoClick();

            // Act, Assert
            firstrptIrkmLogoClick.ShouldNotBeNull();
            secondrptIrkmLogoClick.ShouldNotBeNull();
            thirdrptIrkmLogoClick.ShouldNotBeNull();
            fourthrptIrkmLogoClick.ShouldNotBeNull();
            fifthrptIrkmLogoClick.ShouldNotBeNull();
            sixthrptIrkmLogoClick.ShouldNotBeNull();
            firstrptIrkmLogoClick.ShouldNotBeSameAs(secondrptIrkmLogoClick);
            thirdrptIrkmLogoClick.ShouldNotBeSameAs(firstrptIrkmLogoClick);
            fourthrptIrkmLogoClick.ShouldNotBeSameAs(firstrptIrkmLogoClick);
            fifthrptIrkmLogoClick.ShouldNotBeSameAs(firstrptIrkmLogoClick);
            sixthrptIrkmLogoClick.ShouldNotBeSameAs(firstrptIrkmLogoClick);
            sixthrptIrkmLogoClick.ShouldNotBeSameAs(fourthrptIrkmLogoClick);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_rpt_IRKMLogoClick_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(action: () => new rpt_IRKMLogoClick());
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Class_Invalid_Property_FullResourceNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFullResourceName = "FullResourceNameNotPresent";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();

            // Act , Assert
            Should.NotThrow(action: () => rptIrkmLogoClick.GetType().GetProperty(propertyNameFullResourceName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Class_Invalid_Property_NewGeneratorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameNewGenerator = "NewGeneratorNotPresent";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();

            // Act , Assert
            Should.NotThrow(action: () => rptIrkmLogoClick.GetType().GetProperty(propertyNameNewGenerator));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Class_Invalid_Property_Parameter_fromdtNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterFromdt = "Parameter_fromdtNotPresent";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();

            // Act , Assert
            Should.NotThrow(action: () => rptIrkmLogoClick.GetType().GetProperty(propertyNameParameterFromdt));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Class_Invalid_Property_Parameter_showdetailsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterShowdetails = "Parameter_showdetailsNotPresent";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();

            // Act , Assert
            Should.NotThrow(action: () => rptIrkmLogoClick.GetType().GetProperty(propertyNameParameterShowdetails));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Class_Invalid_Property_Parameter_todtNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterTodt = "Parameter_todtNotPresent";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();

            // Act , Assert
            Should.NotThrow(action: () => rptIrkmLogoClick.GetType().GetProperty(propertyNameParameterTodt));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Class_Invalid_Property_ResourceNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameResourceName = "ResourceNameNotPresent";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();

            // Act , Assert
            Should.NotThrow(action: () => rptIrkmLogoClick.GetType().GetProperty(propertyNameResourceName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Class_Invalid_Property_Section10NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection10 = "Section10NotPresent";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();

            // Act , Assert
            Should.NotThrow(action: () => rptIrkmLogoClick.GetType().GetProperty(propertyNameSection10));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Class_Invalid_Property_Section11NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection11 = "Section11NotPresent";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();

            // Act , Assert
            Should.NotThrow(action: () => rptIrkmLogoClick.GetType().GetProperty(propertyNameSection11));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Class_Invalid_Property_Section12NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection12 = "Section12NotPresent";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();

            // Act , Assert
            Should.NotThrow(action: () => rptIrkmLogoClick.GetType().GetProperty(propertyNameSection12));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Class_Invalid_Property_Section13NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection13 = "Section13NotPresent";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();

            // Act , Assert
            Should.NotThrow(action: () => rptIrkmLogoClick.GetType().GetProperty(propertyNameSection13));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Class_Invalid_Property_Section15NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection15 = "Section15NotPresent";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();

            // Act , Assert
            Should.NotThrow(action: () => rptIrkmLogoClick.GetType().GetProperty(propertyNameSection15));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Class_Invalid_Property_Section1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1NotPresent";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();

            // Act , Assert
            Should.NotThrow(action: () => rptIrkmLogoClick.GetType().GetProperty(propertyNameSection1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Class_Invalid_Property_Section2NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2NotPresent";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();

            // Act , Assert
            Should.NotThrow(action: () => rptIrkmLogoClick.GetType().GetProperty(propertyNameSection2));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Class_Invalid_Property_Section3NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3NotPresent";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();

            // Act , Assert
            Should.NotThrow(action: () => rptIrkmLogoClick.GetType().GetProperty(propertyNameSection3));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Class_Invalid_Property_Section4NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4NotPresent";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();

            // Act , Assert
            Should.NotThrow(action: () => rptIrkmLogoClick.GetType().GetProperty(propertyNameSection4));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Class_Invalid_Property_Section5NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5NotPresent";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();

            // Act , Assert
            Should.NotThrow(action: () => rptIrkmLogoClick.GetType().GetProperty(propertyNameSection5));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Class_Invalid_Property_Section6NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection6 = "Section6NotPresent";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();

            // Act , Assert
            Should.NotThrow(action: () => rptIrkmLogoClick.GetType().GetProperty(propertyNameSection6));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Class_Invalid_Property_Section7NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection7 = "Section7NotPresent";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();

            // Act , Assert
            Should.NotThrow(action: () => rptIrkmLogoClick.GetType().GetProperty(propertyNameSection7));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Class_Invalid_Property_Section8NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection8 = "Section8NotPresent";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();

            // Act , Assert
            Should.NotThrow(action: () => rptIrkmLogoClick.GetType().GetProperty(propertyNameSection8));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Class_Invalid_Property_Section9NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection9 = "Section9NotPresent";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();

            // Act , Assert
            Should.NotThrow(action: () => rptIrkmLogoClick.GetType().GetProperty(propertyNameSection9));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_FullResourceName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFullResourceName = "FullResourceName";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameFullResourceName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_FullResourceName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            rptIrkmLogoClick.FullResourceName = Fixture.Create<string>();
            var stringType = rptIrkmLogoClick.FullResourceName.GetType();

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
        public void rpt_IRKMLogoClick_NewGenerator_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            rptIrkmLogoClick.NewGenerator = Fixture.Create<bool>();
            var boolType = rptIrkmLogoClick.NewGenerator.GetType();

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
        public void rpt_IRKMLogoClick_NewGenerator_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameNewGenerator = "NewGenerator";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameNewGenerator);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Parameter_fromdt_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterFromdt = "Parameter_fromdt";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameParameterFromdt);

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
        public void rpt_IRKMLogoClick_Parameter_fromdt_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterFromdt = "Parameter_fromdt";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameParameterFromdt);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptIrkmLogoClick, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Parameter_showdetails_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterShowdetails = "Parameter_showdetails";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameParameterShowdetails);

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
        public void rpt_IRKMLogoClick_Parameter_showdetails_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterShowdetails = "Parameter_showdetails";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameParameterShowdetails);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptIrkmLogoClick, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Parameter_todt_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterTodt = "Parameter_todt";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameParameterTodt);

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
        public void rpt_IRKMLogoClick_Parameter_todt_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterTodt = "Parameter_todt";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameParameterTodt);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptIrkmLogoClick, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_ResourceName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameResourceName = "ResourceName";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameResourceName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_ResourceName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            rptIrkmLogoClick.ResourceName = Fixture.Create<string>();
            var stringType = rptIrkmLogoClick.ResourceName.GetType();

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
        public void rpt_IRKMLogoClick_Section1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameSection1);

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
        public void rpt_IRKMLogoClick_Section1_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameSection1);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptIrkmLogoClick, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Section10_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection10 = "Section10";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameSection10);

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
        public void rpt_IRKMLogoClick_Section10_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection10 = "Section10";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameSection10);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptIrkmLogoClick, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Section11_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection11 = "Section11";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameSection11);

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
        public void rpt_IRKMLogoClick_Section11_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection11 = "Section11";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameSection11);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptIrkmLogoClick, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Section12_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection12 = "Section12";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameSection12);

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
        public void rpt_IRKMLogoClick_Section12_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection12 = "Section12";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameSection12);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptIrkmLogoClick, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Section13_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection13 = "Section13";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameSection13);

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
        public void rpt_IRKMLogoClick_Section13_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection13 = "Section13";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameSection13);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptIrkmLogoClick, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Section15_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection15 = "Section15";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameSection15);

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
        public void rpt_IRKMLogoClick_Section15_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection15 = "Section15";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameSection15);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptIrkmLogoClick, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Section2_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameSection2);

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
        public void rpt_IRKMLogoClick_Section2_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameSection2);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptIrkmLogoClick, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Section3_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameSection3);

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
        public void rpt_IRKMLogoClick_Section3_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameSection3);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptIrkmLogoClick, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Section4_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameSection4);

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
        public void rpt_IRKMLogoClick_Section4_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameSection4);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptIrkmLogoClick, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Section5_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameSection5);

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
        public void rpt_IRKMLogoClick_Section5_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameSection5);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptIrkmLogoClick, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Section6_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection6 = "Section6";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameSection6);

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
        public void rpt_IRKMLogoClick_Section6_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection6 = "Section6";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameSection6);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptIrkmLogoClick, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Section7_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection7 = "Section7";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameSection7);

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
        public void rpt_IRKMLogoClick_Section7_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection7 = "Section7";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameSection7);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptIrkmLogoClick, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Section8_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection8 = "Section8";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameSection8);

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
        public void rpt_IRKMLogoClick_Section8_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection8 = "Section8";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameSection8);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptIrkmLogoClick, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_IRKMLogoClick_Section9_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection9 = "Section9";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameSection9);

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
        public void rpt_IRKMLogoClick_Section9_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection9 = "Section9";
            var rptIrkmLogoClick = new rpt_IRKMLogoClick();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptIrkmLogoClick.GetType().GetProperty(propertyNameSection9);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptIrkmLogoClick, randomString, null));
        }
    }
}