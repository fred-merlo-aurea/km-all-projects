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
    public class ReportNoUsageTest : AbstractGenericTest
    {
        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_rpt_NoUsage_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstrptNoUsage = new rpt_NoUsage();
            var secondrptNoUsage = new rpt_NoUsage();
            var thirdrptNoUsage = new rpt_NoUsage();
            var fourthrptNoUsage = new rpt_NoUsage();
            var fifthrptNoUsage = new rpt_NoUsage();
            var sixthrptNoUsage = new rpt_NoUsage();

            // Act, Assert
            firstrptNoUsage.ShouldNotBeNull();
            secondrptNoUsage.ShouldNotBeNull();
            thirdrptNoUsage.ShouldNotBeNull();
            fourthrptNoUsage.ShouldNotBeNull();
            fifthrptNoUsage.ShouldNotBeNull();
            sixthrptNoUsage.ShouldNotBeNull();
            firstrptNoUsage.ShouldNotBeSameAs(secondrptNoUsage);
            thirdrptNoUsage.ShouldNotBeSameAs(firstrptNoUsage);
            fourthrptNoUsage.ShouldNotBeSameAs(firstrptNoUsage);
            fifthrptNoUsage.ShouldNotBeSameAs(firstrptNoUsage);
            sixthrptNoUsage.ShouldNotBeSameAs(firstrptNoUsage);
            sixthrptNoUsage.ShouldNotBeSameAs(fourthrptNoUsage);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_rpt_NoUsage_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(action: () => new rpt_NoUsage());
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_NoUsage_Class_Invalid_Property_FullResourceNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFullResourceName = "FullResourceNameNotPresent";
            var rptNoUsage  = new rpt_NoUsage();

            // Act , Assert
            Should.NotThrow(action: () => rptNoUsage.GetType().GetProperty(propertyNameFullResourceName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_NoUsage_Class_Invalid_Property_NewGeneratorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameNewGenerator = "NewGeneratorNotPresent";
            var rptNoUsage  = new rpt_NoUsage();

            // Act , Assert
            Should.NotThrow(action: () => rptNoUsage.GetType().GetProperty(propertyNameNewGenerator));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_NoUsage_Class_Invalid_Property_Parameter_channelIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterChannelId = "Parameter_channelIDNotPresent";
            var rptNoUsage  = new rpt_NoUsage();

            // Act , Assert
            Should.NotThrow(action: () => rptNoUsage.GetType().GetProperty(propertyNameParameterChannelId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_NoUsage_Class_Invalid_Property_Parameter_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterCustomerId = "Parameter_CustomerIDNotPresent";
            var rptNoUsage  = new rpt_NoUsage();

            // Act , Assert
            Should.NotThrow(action: () => rptNoUsage.GetType().GetProperty(propertyNameParameterCustomerId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_NoUsage_Class_Invalid_Property_Parameter_fromdtNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterFromdt = "Parameter_fromdtNotPresent";
            var rptNoUsage  = new rpt_NoUsage();

            // Act , Assert
            Should.NotThrow(action: () => rptNoUsage.GetType().GetProperty(propertyNameParameterFromdt));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_NoUsage_Class_Invalid_Property_Parameter_todtNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterTodt = "Parameter_todtNotPresent";
            var rptNoUsage  = new rpt_NoUsage();

            // Act , Assert
            Should.NotThrow(action: () => rptNoUsage.GetType().GetProperty(propertyNameParameterTodt));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_NoUsage_Class_Invalid_Property_ResourceNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameResourceName = "ResourceNameNotPresent";
            var rptNoUsage  = new rpt_NoUsage();

            // Act , Assert
            Should.NotThrow(action: () => rptNoUsage.GetType().GetProperty(propertyNameResourceName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_NoUsage_Class_Invalid_Property_Section1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1NotPresent";
            var rptNoUsage  = new rpt_NoUsage();

            // Act , Assert
            Should.NotThrow(action: () => rptNoUsage.GetType().GetProperty(propertyNameSection1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_NoUsage_Class_Invalid_Property_Section2NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2NotPresent";
            var rptNoUsage  = new rpt_NoUsage();

            // Act , Assert
            Should.NotThrow(action: () => rptNoUsage.GetType().GetProperty(propertyNameSection2));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_NoUsage_Class_Invalid_Property_Section3NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3NotPresent";
            var rptNoUsage  = new rpt_NoUsage();

            // Act , Assert
            Should.NotThrow(action: () => rptNoUsage.GetType().GetProperty(propertyNameSection3));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_NoUsage_Class_Invalid_Property_Section4NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4NotPresent";
            var rptNoUsage  = new rpt_NoUsage();

            // Act , Assert
            Should.NotThrow(action: () => rptNoUsage.GetType().GetProperty(propertyNameSection4));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_NoUsage_Class_Invalid_Property_Section5NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5NotPresent";
            var rptNoUsage  = new rpt_NoUsage();

            // Act , Assert
            Should.NotThrow(action: () => rptNoUsage.GetType().GetProperty(propertyNameSection5));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_NoUsage_Class_Invalid_Property_Section6NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection6 = "Section6NotPresent";
            var rptNoUsage  = new rpt_NoUsage();

            // Act , Assert
            Should.NotThrow(action: () => rptNoUsage.GetType().GetProperty(propertyNameSection6));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_NoUsage_Class_Invalid_Property_Section7NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection7 = "Section7NotPresent";
            var rptNoUsage  = new rpt_NoUsage();

            // Act , Assert
            Should.NotThrow(action: () => rptNoUsage.GetType().GetProperty(propertyNameSection7));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_NoUsage_FullResourceName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFullResourceName = "FullResourceName";
            var rptNoUsage  = new rpt_NoUsage();
            var propertyInfo  = rptNoUsage.GetType().GetProperty(propertyNameFullResourceName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_NoUsage_FullResourceName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var rptNoUsage = new rpt_NoUsage();
            rptNoUsage.FullResourceName = Fixture.Create<string>();
            var stringType = rptNoUsage.FullResourceName.GetType();

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
        public void rpt_NoUsage_NewGenerator_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var rptNoUsage = new rpt_NoUsage();
            rptNoUsage.NewGenerator = Fixture.Create<bool>();
            var boolType = rptNoUsage.NewGenerator.GetType();

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
        public void rpt_NoUsage_NewGenerator_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameNewGenerator = "NewGenerator";
            var rptNoUsage  = new rpt_NoUsage();
            var propertyInfo  = rptNoUsage.GetType().GetProperty(propertyNameNewGenerator);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_NoUsage_Parameter_channelID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterChannelId = "Parameter_channelID";
            var rptNoUsage  = new rpt_NoUsage();
            var propertyInfo  = rptNoUsage.GetType().GetProperty(propertyNameParameterChannelId);

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
        public void rpt_NoUsage_Parameter_channelID_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterChannelId = "Parameter_channelID";
            var rptNoUsage = new rpt_NoUsage();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptNoUsage.GetType().GetProperty(propertyNameParameterChannelId);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptNoUsage, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_NoUsage_Parameter_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterCustomerId = "Parameter_CustomerID";
            var rptNoUsage  = new rpt_NoUsage();
            var propertyInfo  = rptNoUsage.GetType().GetProperty(propertyNameParameterCustomerId);

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
        public void rpt_NoUsage_Parameter_CustomerID_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterCustomerId = "Parameter_CustomerID";
            var rptNoUsage = new rpt_NoUsage();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptNoUsage.GetType().GetProperty(propertyNameParameterCustomerId);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptNoUsage, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_NoUsage_Parameter_fromdt_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterFromdt = "Parameter_fromdt";
            var rptNoUsage  = new rpt_NoUsage();
            var propertyInfo  = rptNoUsage.GetType().GetProperty(propertyNameParameterFromdt);

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
        public void rpt_NoUsage_Parameter_fromdt_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterFromdt = "Parameter_fromdt";
            var rptNoUsage = new rpt_NoUsage();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptNoUsage.GetType().GetProperty(propertyNameParameterFromdt);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptNoUsage, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_NoUsage_Parameter_todt_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterTodt = "Parameter_todt";
            var rptNoUsage  = new rpt_NoUsage();
            var propertyInfo  = rptNoUsage.GetType().GetProperty(propertyNameParameterTodt);

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
        public void rpt_NoUsage_Parameter_todt_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterTodt = "Parameter_todt";
            var rptNoUsage = new rpt_NoUsage();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptNoUsage.GetType().GetProperty(propertyNameParameterTodt);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptNoUsage, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_NoUsage_ResourceName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameResourceName = "ResourceName";
            var rptNoUsage  = new rpt_NoUsage();
            var propertyInfo  = rptNoUsage.GetType().GetProperty(propertyNameResourceName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_NoUsage_ResourceName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var rptNoUsage = new rpt_NoUsage();
            rptNoUsage.ResourceName = Fixture.Create<string>();
            var stringType = rptNoUsage.ResourceName.GetType();

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
        public void rpt_NoUsage_Section1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1";
            var rptNoUsage  = new rpt_NoUsage();
            var propertyInfo  = rptNoUsage.GetType().GetProperty(propertyNameSection1);

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
        public void rpt_NoUsage_Section1_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1";
            var rptNoUsage = new rpt_NoUsage();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptNoUsage.GetType().GetProperty(propertyNameSection1);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptNoUsage, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_NoUsage_Section2_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2";
            var rptNoUsage  = new rpt_NoUsage();
            var propertyInfo  = rptNoUsage.GetType().GetProperty(propertyNameSection2);

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
        public void rpt_NoUsage_Section2_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2";
            var rptNoUsage = new rpt_NoUsage();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptNoUsage.GetType().GetProperty(propertyNameSection2);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptNoUsage, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_NoUsage_Section3_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3";
            var rptNoUsage  = new rpt_NoUsage();
            var propertyInfo  = rptNoUsage.GetType().GetProperty(propertyNameSection3);

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
        public void rpt_NoUsage_Section3_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3";
            var rptNoUsage = new rpt_NoUsage();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptNoUsage.GetType().GetProperty(propertyNameSection3);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptNoUsage, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_NoUsage_Section4_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4";
            var rptNoUsage  = new rpt_NoUsage();
            var propertyInfo  = rptNoUsage.GetType().GetProperty(propertyNameSection4);

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
        public void rpt_NoUsage_Section4_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4";
            var rptNoUsage = new rpt_NoUsage();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptNoUsage.GetType().GetProperty(propertyNameSection4);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptNoUsage, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_NoUsage_Section5_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5";
            var rptNoUsage  = new rpt_NoUsage();
            var propertyInfo  = rptNoUsage.GetType().GetProperty(propertyNameSection5);

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
        public void rpt_NoUsage_Section5_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5";
            var rptNoUsage = new rpt_NoUsage();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptNoUsage.GetType().GetProperty(propertyNameSection5);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptNoUsage, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_NoUsage_Section6_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection6 = "Section6";
            var rptNoUsage  = new rpt_NoUsage();
            var propertyInfo  = rptNoUsage.GetType().GetProperty(propertyNameSection6);

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
        public void rpt_NoUsage_Section6_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection6 = "Section6";
            var rptNoUsage = new rpt_NoUsage();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptNoUsage.GetType().GetProperty(propertyNameSection6);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptNoUsage, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_NoUsage_Section7_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection7 = "Section7";
            var rptNoUsage  = new rpt_NoUsage();
            var propertyInfo  = rptNoUsage.GetType().GetProperty(propertyNameSection7);

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
        public void rpt_NoUsage_Section7_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection7 = "Section7";
            var rptNoUsage = new rpt_NoUsage();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptNoUsage.GetType().GetProperty(propertyNameSection7);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptNoUsage, randomString, null));
        }
    }
}