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
    public class ReportAccountIntensityTest : AbstractGenericTest
    {
        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_rpt_AccountIntensity_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstrptAccountIntensity = new rpt_AccountIntensity();
            var secondrptAccountIntensity = new rpt_AccountIntensity();
            var thirdrptAccountIntensity = new rpt_AccountIntensity();
            var fourthrptAccountIntensity = new rpt_AccountIntensity();
            var fifthrptAccountIntensity = new rpt_AccountIntensity();
            var sixthrptAccountIntensity = new rpt_AccountIntensity();

            // Act, Assert
            firstrptAccountIntensity.ShouldNotBeNull();
            secondrptAccountIntensity.ShouldNotBeNull();
            thirdrptAccountIntensity.ShouldNotBeNull();
            fourthrptAccountIntensity.ShouldNotBeNull();
            fifthrptAccountIntensity.ShouldNotBeNull();
            sixthrptAccountIntensity.ShouldNotBeNull();
            firstrptAccountIntensity.ShouldNotBeSameAs(secondrptAccountIntensity);
            thirdrptAccountIntensity.ShouldNotBeSameAs(firstrptAccountIntensity);
            fourthrptAccountIntensity.ShouldNotBeSameAs(firstrptAccountIntensity);
            fifthrptAccountIntensity.ShouldNotBeSameAs(firstrptAccountIntensity);
            sixthrptAccountIntensity.ShouldNotBeSameAs(firstrptAccountIntensity);
            sixthrptAccountIntensity.ShouldNotBeSameAs(fourthrptAccountIntensity);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_rpt_AccountIntensity_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(action: () => new rpt_AccountIntensity());
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_AccountIntensity_Class_Invalid_Property_FullResourceNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFullResourceName = "FullResourceNameNotPresent";
            var rptAccountIntensity  = new rpt_AccountIntensity();

            // Act , Assert
            Should.NotThrow(action: () => rptAccountIntensity.GetType().GetProperty(propertyNameFullResourceName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_AccountIntensity_Class_Invalid_Property_NewGeneratorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameNewGenerator = "NewGeneratorNotPresent";
            var rptAccountIntensity  = new rpt_AccountIntensity();

            // Act , Assert
            Should.NotThrow(action: () => rptAccountIntensity.GetType().GetProperty(propertyNameNewGenerator));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_AccountIntensity_Class_Invalid_Property_Parameter_AccountExecutiveIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterAccountExecutiveId = "Parameter_AccountExecutiveIDNotPresent";
            var rptAccountIntensity  = new rpt_AccountIntensity();

            // Act , Assert
            Should.NotThrow(action: () => rptAccountIntensity.GetType().GetProperty(propertyNameParameterAccountExecutiveId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_AccountIntensity_Class_Invalid_Property_Parameter_AccountManagerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterAccountManagerId = "Parameter_AccountManagerIDNotPresent";
            var rptAccountIntensity  = new rpt_AccountIntensity();

            // Act , Assert
            Should.NotThrow(action: () => rptAccountIntensity.GetType().GetProperty(propertyNameParameterAccountManagerId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_AccountIntensity_Class_Invalid_Property_Parameter_channelIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterChannelId = "Parameter_channelIDNotPresent";
            var rptAccountIntensity  = new rpt_AccountIntensity();

            // Act , Assert
            Should.NotThrow(action: () => rptAccountIntensity.GetType().GetProperty(propertyNameParameterChannelId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_AccountIntensity_Class_Invalid_Property_Parameter_customerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterCustomerId = "Parameter_customerIDNotPresent";
            var rptAccountIntensity  = new rpt_AccountIntensity();

            // Act , Assert
            Should.NotThrow(action: () => rptAccountIntensity.GetType().GetProperty(propertyNameParameterCustomerId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_AccountIntensity_Class_Invalid_Property_Parameter_customerTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterCustomerType = "Parameter_customerTypeNotPresent";
            var rptAccountIntensity  = new rpt_AccountIntensity();

            // Act , Assert
            Should.NotThrow(action: () => rptAccountIntensity.GetType().GetProperty(propertyNameParameterCustomerType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_AccountIntensity_Class_Invalid_Property_ResourceNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameResourceName = "ResourceNameNotPresent";
            var rptAccountIntensity  = new rpt_AccountIntensity();

            // Act , Assert
            Should.NotThrow(action: () => rptAccountIntensity.GetType().GetProperty(propertyNameResourceName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_AccountIntensity_Class_Invalid_Property_Section1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1NotPresent";
            var rptAccountIntensity  = new rpt_AccountIntensity();

            // Act , Assert
            Should.NotThrow(action: () => rptAccountIntensity.GetType().GetProperty(propertyNameSection1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_AccountIntensity_Class_Invalid_Property_Section2NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2NotPresent";
            var rptAccountIntensity  = new rpt_AccountIntensity();

            // Act , Assert
            Should.NotThrow(action: () => rptAccountIntensity.GetType().GetProperty(propertyNameSection2));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_AccountIntensity_Class_Invalid_Property_Section3NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3NotPresent";
            var rptAccountIntensity  = new rpt_AccountIntensity();

            // Act , Assert
            Should.NotThrow(action: () => rptAccountIntensity.GetType().GetProperty(propertyNameSection3));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_AccountIntensity_Class_Invalid_Property_Section4NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4NotPresent";
            var rptAccountIntensity  = new rpt_AccountIntensity();

            // Act , Assert
            Should.NotThrow(action: () => rptAccountIntensity.GetType().GetProperty(propertyNameSection4));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_AccountIntensity_Class_Invalid_Property_Section5NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5NotPresent";
            var rptAccountIntensity  = new rpt_AccountIntensity();

            // Act , Assert
            Should.NotThrow(action: () => rptAccountIntensity.GetType().GetProperty(propertyNameSection5));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_AccountIntensity_FullResourceName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFullResourceName = "FullResourceName";
            var rptAccountIntensity  = new rpt_AccountIntensity();
            var propertyInfo  = rptAccountIntensity.GetType().GetProperty(propertyNameFullResourceName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_AccountIntensity_FullResourceName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var rptAccountIntensity = new rpt_AccountIntensity();
            rptAccountIntensity.FullResourceName = Fixture.Create<string>();
            var stringType = rptAccountIntensity.FullResourceName.GetType();

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
        public void rpt_AccountIntensity_NewGenerator_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var rptAccountIntensity = new rpt_AccountIntensity();
            rptAccountIntensity.NewGenerator = Fixture.Create<bool>();
            var boolType = rptAccountIntensity.NewGenerator.GetType();

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
        public void rpt_AccountIntensity_NewGenerator_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameNewGenerator = "NewGenerator";
            var rptAccountIntensity  = new rpt_AccountIntensity();
            var propertyInfo  = rptAccountIntensity.GetType().GetProperty(propertyNameNewGenerator);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_AccountIntensity_Parameter_AccountExecutiveID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterAccountExecutiveId = "Parameter_AccountExecutiveID";
            var rptAccountIntensity  = new rpt_AccountIntensity();
            var propertyInfo  = rptAccountIntensity.GetType().GetProperty(propertyNameParameterAccountExecutiveId);

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
        public void rpt_AccountIntensity_Parameter_AccountExecutiveID_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterAccountExecutiveId = "Parameter_AccountExecutiveID";
            var rptAccountIntensity = new rpt_AccountIntensity();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptAccountIntensity.GetType().GetProperty(propertyNameParameterAccountExecutiveId);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptAccountIntensity, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_AccountIntensity_Parameter_AccountManagerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterAccountManagerId = "Parameter_AccountManagerID";
            var rptAccountIntensity  = new rpt_AccountIntensity();
            var propertyInfo  = rptAccountIntensity.GetType().GetProperty(propertyNameParameterAccountManagerId);

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
        public void rpt_AccountIntensity_Parameter_AccountManagerID_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterAccountManagerId = "Parameter_AccountManagerID";
            var rptAccountIntensity = new rpt_AccountIntensity();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptAccountIntensity.GetType().GetProperty(propertyNameParameterAccountManagerId);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptAccountIntensity, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_AccountIntensity_Parameter_channelID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterChannelId = "Parameter_channelID";
            var rptAccountIntensity  = new rpt_AccountIntensity();
            var propertyInfo  = rptAccountIntensity.GetType().GetProperty(propertyNameParameterChannelId);

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
        public void rpt_AccountIntensity_Parameter_channelID_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterChannelId = "Parameter_channelID";
            var rptAccountIntensity = new rpt_AccountIntensity();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptAccountIntensity.GetType().GetProperty(propertyNameParameterChannelId);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptAccountIntensity, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_AccountIntensity_Parameter_customerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterCustomerId = "Parameter_customerID";
            var rptAccountIntensity  = new rpt_AccountIntensity();
            var propertyInfo  = rptAccountIntensity.GetType().GetProperty(propertyNameParameterCustomerId);

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
        public void rpt_AccountIntensity_Parameter_customerID_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterCustomerId = "Parameter_customerID";
            var rptAccountIntensity = new rpt_AccountIntensity();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptAccountIntensity.GetType().GetProperty(propertyNameParameterCustomerId);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptAccountIntensity, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_AccountIntensity_Parameter_customerType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterCustomerType = "Parameter_customerType";
            var rptAccountIntensity  = new rpt_AccountIntensity();
            var propertyInfo  = rptAccountIntensity.GetType().GetProperty(propertyNameParameterCustomerType);

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
        public void rpt_AccountIntensity_Parameter_customerType_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterCustomerType = "Parameter_customerType";
            var rptAccountIntensity = new rpt_AccountIntensity();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptAccountIntensity.GetType().GetProperty(propertyNameParameterCustomerType);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptAccountIntensity, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_AccountIntensity_ResourceName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameResourceName = "ResourceName";
            var rptAccountIntensity  = new rpt_AccountIntensity();
            var propertyInfo  = rptAccountIntensity.GetType().GetProperty(propertyNameResourceName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_AccountIntensity_ResourceName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var rptAccountIntensity = new rpt_AccountIntensity();
            rptAccountIntensity.ResourceName = Fixture.Create<string>();
            var stringType = rptAccountIntensity.ResourceName.GetType();

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
        public void rpt_AccountIntensity_Section1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1";
            var rptAccountIntensity  = new rpt_AccountIntensity();
            var propertyInfo  = rptAccountIntensity.GetType().GetProperty(propertyNameSection1);

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
        public void rpt_AccountIntensity_Section1_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1";
            var rptAccountIntensity = new rpt_AccountIntensity();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptAccountIntensity.GetType().GetProperty(propertyNameSection1);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptAccountIntensity, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_AccountIntensity_Section2_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2";
            var rptAccountIntensity  = new rpt_AccountIntensity();
            var propertyInfo  = rptAccountIntensity.GetType().GetProperty(propertyNameSection2);

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
        public void rpt_AccountIntensity_Section2_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2";
            var rptAccountIntensity = new rpt_AccountIntensity();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptAccountIntensity.GetType().GetProperty(propertyNameSection2);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptAccountIntensity, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_AccountIntensity_Section3_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3";
            var rptAccountIntensity  = new rpt_AccountIntensity();
            var propertyInfo  = rptAccountIntensity.GetType().GetProperty(propertyNameSection3);

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
        public void rpt_AccountIntensity_Section3_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3";
            var rptAccountIntensity = new rpt_AccountIntensity();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptAccountIntensity.GetType().GetProperty(propertyNameSection3);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptAccountIntensity, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_AccountIntensity_Section4_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4";
            var rptAccountIntensity  = new rpt_AccountIntensity();
            var propertyInfo  = rptAccountIntensity.GetType().GetProperty(propertyNameSection4);

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
        public void rpt_AccountIntensity_Section4_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4";
            var rptAccountIntensity = new rpt_AccountIntensity();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptAccountIntensity.GetType().GetProperty(propertyNameSection4);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptAccountIntensity, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_AccountIntensity_Section5_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5";
            var rptAccountIntensity  = new rpt_AccountIntensity();
            var propertyInfo  = rptAccountIntensity.GetType().GetProperty(propertyNameSection5);

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
        public void rpt_AccountIntensity_Section5_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5";
            var rptAccountIntensity = new rpt_AccountIntensity();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptAccountIntensity.GetType().GetProperty(propertyNameSection5);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptAccountIntensity, randomString, null));
        }
    }
}