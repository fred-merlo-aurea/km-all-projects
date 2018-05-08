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
    public class ReportBillingNotesTest : AbstractGenericTest
    {
        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_rpt_BillingNotes_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstrptBillingNotes = new rpt_BillingNotes();
            var secondrptBillingNotes = new rpt_BillingNotes();
            var thirdrptBillingNotes = new rpt_BillingNotes();
            var fourthrptBillingNotes = new rpt_BillingNotes();
            var fifthrptBillingNotes = new rpt_BillingNotes();
            var sixthrptBillingNotes = new rpt_BillingNotes();

            // Act, Assert
            firstrptBillingNotes.ShouldNotBeNull();
            secondrptBillingNotes.ShouldNotBeNull();
            thirdrptBillingNotes.ShouldNotBeNull();
            fourthrptBillingNotes.ShouldNotBeNull();
            fifthrptBillingNotes.ShouldNotBeNull();
            sixthrptBillingNotes.ShouldNotBeNull();
            firstrptBillingNotes.ShouldNotBeSameAs(secondrptBillingNotes);
            thirdrptBillingNotes.ShouldNotBeSameAs(firstrptBillingNotes);
            fourthrptBillingNotes.ShouldNotBeSameAs(firstrptBillingNotes);
            fifthrptBillingNotes.ShouldNotBeSameAs(firstrptBillingNotes);
            sixthrptBillingNotes.ShouldNotBeSameAs(firstrptBillingNotes);
            sixthrptBillingNotes.ShouldNotBeSameAs(fourthrptBillingNotes);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_rpt_BillingNotes_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(action: () => new rpt_BillingNotes());
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingNotes_Class_Invalid_Property_FullResourceNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFullResourceName = "FullResourceNameNotPresent";
            var rptBillingNotes  = new rpt_BillingNotes();

            // Act , Assert
            Should.NotThrow(action: () => rptBillingNotes.GetType().GetProperty(propertyNameFullResourceName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingNotes_Class_Invalid_Property_GroupFooterSection1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupFooterSection1 = "GroupFooterSection1NotPresent";
            var rptBillingNotes  = new rpt_BillingNotes();

            // Act , Assert
            Should.NotThrow(action: () => rptBillingNotes.GetType().GetProperty(propertyNameGroupFooterSection1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingNotes_Class_Invalid_Property_GroupHeaderSection1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupHeaderSection1 = "GroupHeaderSection1NotPresent";
            var rptBillingNotes  = new rpt_BillingNotes();

            // Act , Assert
            Should.NotThrow(action: () => rptBillingNotes.GetType().GetProperty(propertyNameGroupHeaderSection1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingNotes_Class_Invalid_Property_GroupHeaderSection2NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupHeaderSection2 = "GroupHeaderSection2NotPresent";
            var rptBillingNotes  = new rpt_BillingNotes();

            // Act , Assert
            Should.NotThrow(action: () => rptBillingNotes.GetType().GetProperty(propertyNameGroupHeaderSection2));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingNotes_Class_Invalid_Property_NewGeneratorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameNewGenerator = "NewGeneratorNotPresent";
            var rptBillingNotes  = new rpt_BillingNotes();

            // Act , Assert
            Should.NotThrow(action: () => rptBillingNotes.GetType().GetProperty(propertyNameNewGenerator));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingNotes_Class_Invalid_Property_ResourceNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameResourceName = "ResourceNameNotPresent";
            var rptBillingNotes  = new rpt_BillingNotes();

            // Act , Assert
            Should.NotThrow(action: () => rptBillingNotes.GetType().GetProperty(propertyNameResourceName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingNotes_Class_Invalid_Property_Section1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1NotPresent";
            var rptBillingNotes  = new rpt_BillingNotes();

            // Act , Assert
            Should.NotThrow(action: () => rptBillingNotes.GetType().GetProperty(propertyNameSection1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingNotes_Class_Invalid_Property_Section2NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2NotPresent";
            var rptBillingNotes  = new rpt_BillingNotes();

            // Act , Assert
            Should.NotThrow(action: () => rptBillingNotes.GetType().GetProperty(propertyNameSection2));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingNotes_Class_Invalid_Property_Section3NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3NotPresent";
            var rptBillingNotes  = new rpt_BillingNotes();

            // Act , Assert
            Should.NotThrow(action: () => rptBillingNotes.GetType().GetProperty(propertyNameSection3));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingNotes_Class_Invalid_Property_Section4NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4NotPresent";
            var rptBillingNotes  = new rpt_BillingNotes();

            // Act , Assert
            Should.NotThrow(action: () => rptBillingNotes.GetType().GetProperty(propertyNameSection4));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingNotes_Class_Invalid_Property_Section5NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5NotPresent";
            var rptBillingNotes  = new rpt_BillingNotes();

            // Act , Assert
            Should.NotThrow(action: () => rptBillingNotes.GetType().GetProperty(propertyNameSection5));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingNotes_FullResourceName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFullResourceName = "FullResourceName";
            var rptBillingNotes  = new rpt_BillingNotes();
            var propertyInfo  = rptBillingNotes.GetType().GetProperty(propertyNameFullResourceName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingNotes_FullResourceName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var rptBillingNotes = new rpt_BillingNotes();
            rptBillingNotes.FullResourceName = Fixture.Create<string>();
            var stringType = rptBillingNotes.FullResourceName.GetType();

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
        public void rpt_BillingNotes_GroupFooterSection1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupFooterSection1 = "GroupFooterSection1";
            var rptBillingNotes  = new rpt_BillingNotes();
            var propertyInfo  = rptBillingNotes.GetType().GetProperty(propertyNameGroupFooterSection1);

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
        public void rpt_BillingNotes_GroupFooterSection1_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupFooterSection1 = "GroupFooterSection1";
            var rptBillingNotes = new rpt_BillingNotes();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptBillingNotes.GetType().GetProperty(propertyNameGroupFooterSection1);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptBillingNotes, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingNotes_GroupHeaderSection1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupHeaderSection1 = "GroupHeaderSection1";
            var rptBillingNotes  = new rpt_BillingNotes();
            var propertyInfo  = rptBillingNotes.GetType().GetProperty(propertyNameGroupHeaderSection1);

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
        public void rpt_BillingNotes_GroupHeaderSection1_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupHeaderSection1 = "GroupHeaderSection1";
            var rptBillingNotes = new rpt_BillingNotes();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptBillingNotes.GetType().GetProperty(propertyNameGroupHeaderSection1);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptBillingNotes, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingNotes_GroupHeaderSection2_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupHeaderSection2 = "GroupHeaderSection2";
            var rptBillingNotes  = new rpt_BillingNotes();
            var propertyInfo  = rptBillingNotes.GetType().GetProperty(propertyNameGroupHeaderSection2);

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
        public void rpt_BillingNotes_GroupHeaderSection2_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupHeaderSection2 = "GroupHeaderSection2";
            var rptBillingNotes = new rpt_BillingNotes();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptBillingNotes.GetType().GetProperty(propertyNameGroupHeaderSection2);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptBillingNotes, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingNotes_NewGenerator_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var rptBillingNotes = new rpt_BillingNotes();
            rptBillingNotes.NewGenerator = Fixture.Create<bool>();
            var boolType = rptBillingNotes.NewGenerator.GetType();

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
        public void rpt_BillingNotes_NewGenerator_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameNewGenerator = "NewGenerator";
            var rptBillingNotes  = new rpt_BillingNotes();
            var propertyInfo  = rptBillingNotes.GetType().GetProperty(propertyNameNewGenerator);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingNotes_ResourceName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameResourceName = "ResourceName";
            var rptBillingNotes  = new rpt_BillingNotes();
            var propertyInfo  = rptBillingNotes.GetType().GetProperty(propertyNameResourceName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingNotes_ResourceName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var rptBillingNotes = new rpt_BillingNotes();
            rptBillingNotes.ResourceName = Fixture.Create<string>();
            var stringType = rptBillingNotes.ResourceName.GetType();

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
        public void rpt_BillingNotes_Section1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1";
            var rptBillingNotes  = new rpt_BillingNotes();
            var propertyInfo  = rptBillingNotes.GetType().GetProperty(propertyNameSection1);

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
        public void rpt_BillingNotes_Section1_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1";
            var rptBillingNotes = new rpt_BillingNotes();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptBillingNotes.GetType().GetProperty(propertyNameSection1);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptBillingNotes, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingNotes_Section2_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2";
            var rptBillingNotes  = new rpt_BillingNotes();
            var propertyInfo  = rptBillingNotes.GetType().GetProperty(propertyNameSection2);

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
        public void rpt_BillingNotes_Section2_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2";
            var rptBillingNotes = new rpt_BillingNotes();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptBillingNotes.GetType().GetProperty(propertyNameSection2);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptBillingNotes, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingNotes_Section3_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3";
            var rptBillingNotes  = new rpt_BillingNotes();
            var propertyInfo  = rptBillingNotes.GetType().GetProperty(propertyNameSection3);

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
        public void rpt_BillingNotes_Section3_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3";
            var rptBillingNotes = new rpt_BillingNotes();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptBillingNotes.GetType().GetProperty(propertyNameSection3);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptBillingNotes, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingNotes_Section4_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4";
            var rptBillingNotes  = new rpt_BillingNotes();
            var propertyInfo  = rptBillingNotes.GetType().GetProperty(propertyNameSection4);

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
        public void rpt_BillingNotes_Section4_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4";
            var rptBillingNotes = new rpt_BillingNotes();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptBillingNotes.GetType().GetProperty(propertyNameSection4);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptBillingNotes, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_BillingNotes_Section5_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5";
            var rptBillingNotes  = new rpt_BillingNotes();
            var propertyInfo  = rptBillingNotes.GetType().GetProperty(propertyNameSection5);

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
        public void rpt_BillingNotes_Section5_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5";
            var rptBillingNotes = new rpt_BillingNotes();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptBillingNotes.GetType().GetProperty(propertyNameSection5);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptBillingNotes, randomString, null));
        }
    }
}