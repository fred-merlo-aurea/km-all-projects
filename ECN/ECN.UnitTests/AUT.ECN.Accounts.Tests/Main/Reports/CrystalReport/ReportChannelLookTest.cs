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
    public class ReportChannelLookTest : AbstractGenericTest
    {
        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_rpt_ChannelLook_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstrptChannelLook = new rpt_ChannelLook();
            var secondrptChannelLook = new rpt_ChannelLook();
            var thirdrptChannelLook = new rpt_ChannelLook();
            var fourthrptChannelLook = new rpt_ChannelLook();
            var fifthrptChannelLook = new rpt_ChannelLook();
            var sixthrptChannelLook = new rpt_ChannelLook();

            // Act, Assert
            firstrptChannelLook.ShouldNotBeNull();
            secondrptChannelLook.ShouldNotBeNull();
            thirdrptChannelLook.ShouldNotBeNull();
            fourthrptChannelLook.ShouldNotBeNull();
            fifthrptChannelLook.ShouldNotBeNull();
            sixthrptChannelLook.ShouldNotBeNull();
            firstrptChannelLook.ShouldNotBeSameAs(secondrptChannelLook);
            thirdrptChannelLook.ShouldNotBeSameAs(firstrptChannelLook);
            fourthrptChannelLook.ShouldNotBeSameAs(firstrptChannelLook);
            fifthrptChannelLook.ShouldNotBeSameAs(firstrptChannelLook);
            sixthrptChannelLook.ShouldNotBeSameAs(firstrptChannelLook);
            sixthrptChannelLook.ShouldNotBeSameAs(fourthrptChannelLook);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_rpt_ChannelLook_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(action: () => new rpt_ChannelLook());
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Class_Invalid_Property_FullResourceNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFullResourceName = "FullResourceNameNotPresent";
            var rptChannelLook  = new rpt_ChannelLook();

            // Act , Assert
            Should.NotThrow(action: () => rptChannelLook.GetType().GetProperty(propertyNameFullResourceName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Class_Invalid_Property_NewGeneratorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameNewGenerator = "NewGeneratorNotPresent";
            var rptChannelLook  = new rpt_ChannelLook();

            // Act , Assert
            Should.NotThrow(action: () => rptChannelLook.GetType().GetProperty(propertyNameNewGenerator));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Class_Invalid_Property_Parameter_channelIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterChannelId = "Parameter_channelIDNotPresent";
            var rptChannelLook  = new rpt_ChannelLook();

            // Act , Assert
            Should.NotThrow(action: () => rptChannelLook.GetType().GetProperty(propertyNameParameterChannelId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Class_Invalid_Property_Parameter_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterCustomerId = "Parameter_CustomerIDNotPresent";
            var rptChannelLook  = new rpt_ChannelLook();

            // Act , Assert
            Should.NotThrow(action: () => rptChannelLook.GetType().GetProperty(propertyNameParameterCustomerId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Class_Invalid_Property_Parameter_detailsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterDetails = "Parameter_detailsNotPresent";
            var rptChannelLook  = new rpt_ChannelLook();

            // Act , Assert
            Should.NotThrow(action: () => rptChannelLook.GetType().GetProperty(propertyNameParameterDetails));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Class_Invalid_Property_Parameter_fromdtNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterFromdt = "Parameter_fromdtNotPresent";
            var rptChannelLook  = new rpt_ChannelLook();

            // Act , Assert
            Should.NotThrow(action: () => rptChannelLook.GetType().GetProperty(propertyNameParameterFromdt));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Class_Invalid_Property_Parameter_testblastNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterTestblast = "Parameter_testblastNotPresent";
            var rptChannelLook  = new rpt_ChannelLook();

            // Act , Assert
            Should.NotThrow(action: () => rptChannelLook.GetType().GetProperty(propertyNameParameterTestblast));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Class_Invalid_Property_Parameter_todtNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterTodt = "Parameter_todtNotPresent";
            var rptChannelLook  = new rpt_ChannelLook();

            // Act , Assert
            Should.NotThrow(action: () => rptChannelLook.GetType().GetProperty(propertyNameParameterTodt));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Class_Invalid_Property_ResourceNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameResourceName = "ResourceNameNotPresent";
            var rptChannelLook  = new rpt_ChannelLook();

            // Act , Assert
            Should.NotThrow(action: () => rptChannelLook.GetType().GetProperty(propertyNameResourceName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Class_Invalid_Property_Section1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1NotPresent";
            var rptChannelLook  = new rpt_ChannelLook();

            // Act , Assert
            Should.NotThrow(action: () => rptChannelLook.GetType().GetProperty(propertyNameSection1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Class_Invalid_Property_Section2NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2NotPresent";
            var rptChannelLook  = new rpt_ChannelLook();

            // Act , Assert
            Should.NotThrow(action: () => rptChannelLook.GetType().GetProperty(propertyNameSection2));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Class_Invalid_Property_Section3NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3NotPresent";
            var rptChannelLook  = new rpt_ChannelLook();

            // Act , Assert
            Should.NotThrow(action: () => rptChannelLook.GetType().GetProperty(propertyNameSection3));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Class_Invalid_Property_Section4NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4NotPresent";
            var rptChannelLook  = new rpt_ChannelLook();

            // Act , Assert
            Should.NotThrow(action: () => rptChannelLook.GetType().GetProperty(propertyNameSection4));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Class_Invalid_Property_Section5NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5NotPresent";
            var rptChannelLook  = new rpt_ChannelLook();

            // Act , Assert
            Should.NotThrow(action: () => rptChannelLook.GetType().GetProperty(propertyNameSection5));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Class_Invalid_Property_Section6NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection6 = "Section6NotPresent";
            var rptChannelLook  = new rpt_ChannelLook();

            // Act , Assert
            Should.NotThrow(action: () => rptChannelLook.GetType().GetProperty(propertyNameSection6));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Class_Invalid_Property_Section7NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection7 = "Section7NotPresent";
            var rptChannelLook  = new rpt_ChannelLook();

            // Act , Assert
            Should.NotThrow(action: () => rptChannelLook.GetType().GetProperty(propertyNameSection7));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Class_Invalid_Property_Section8NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection8 = "Section8NotPresent";
            var rptChannelLook  = new rpt_ChannelLook();

            // Act , Assert
            Should.NotThrow(action: () => rptChannelLook.GetType().GetProperty(propertyNameSection8));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_FullResourceName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFullResourceName = "FullResourceName";
            var rptChannelLook  = new rpt_ChannelLook();
            var propertyInfo  = rptChannelLook.GetType().GetProperty(propertyNameFullResourceName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_FullResourceName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var rptChannelLook = new rpt_ChannelLook();
            rptChannelLook.FullResourceName = Fixture.Create<string>();
            var stringType = rptChannelLook.FullResourceName.GetType();

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
        public void rpt_ChannelLook_NewGenerator_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var rptChannelLook = new rpt_ChannelLook();
            rptChannelLook.NewGenerator = Fixture.Create<bool>();
            var boolType = rptChannelLook.NewGenerator.GetType();

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
        public void rpt_ChannelLook_NewGenerator_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameNewGenerator = "NewGenerator";
            var rptChannelLook  = new rpt_ChannelLook();
            var propertyInfo  = rptChannelLook.GetType().GetProperty(propertyNameNewGenerator);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Parameter_channelID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterChannelId = "Parameter_channelID";
            var rptChannelLook  = new rpt_ChannelLook();
            var propertyInfo  = rptChannelLook.GetType().GetProperty(propertyNameParameterChannelId);

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
        public void rpt_ChannelLook_Parameter_channelID_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterChannelId = "Parameter_channelID";
            var rptChannelLook = new rpt_ChannelLook();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptChannelLook.GetType().GetProperty(propertyNameParameterChannelId);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptChannelLook, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Parameter_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterCustomerId = "Parameter_CustomerID";
            var rptChannelLook  = new rpt_ChannelLook();
            var propertyInfo  = rptChannelLook.GetType().GetProperty(propertyNameParameterCustomerId);

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
        public void rpt_ChannelLook_Parameter_CustomerID_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterCustomerId = "Parameter_CustomerID";
            var rptChannelLook = new rpt_ChannelLook();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptChannelLook.GetType().GetProperty(propertyNameParameterCustomerId);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptChannelLook, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Parameter_details_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterDetails = "Parameter_details";
            var rptChannelLook  = new rpt_ChannelLook();
            var propertyInfo  = rptChannelLook.GetType().GetProperty(propertyNameParameterDetails);

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
        public void rpt_ChannelLook_Parameter_details_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterDetails = "Parameter_details";
            var rptChannelLook = new rpt_ChannelLook();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptChannelLook.GetType().GetProperty(propertyNameParameterDetails);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptChannelLook, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Parameter_fromdt_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterFromdt = "Parameter_fromdt";
            var rptChannelLook  = new rpt_ChannelLook();
            var propertyInfo  = rptChannelLook.GetType().GetProperty(propertyNameParameterFromdt);

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
        public void rpt_ChannelLook_Parameter_fromdt_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterFromdt = "Parameter_fromdt";
            var rptChannelLook = new rpt_ChannelLook();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptChannelLook.GetType().GetProperty(propertyNameParameterFromdt);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptChannelLook, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Parameter_testblast_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterTestblast = "Parameter_testblast";
            var rptChannelLook  = new rpt_ChannelLook();
            var propertyInfo  = rptChannelLook.GetType().GetProperty(propertyNameParameterTestblast);

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
        public void rpt_ChannelLook_Parameter_testblast_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterTestblast = "Parameter_testblast";
            var rptChannelLook = new rpt_ChannelLook();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptChannelLook.GetType().GetProperty(propertyNameParameterTestblast);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptChannelLook, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Parameter_todt_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterTodt = "Parameter_todt";
            var rptChannelLook  = new rpt_ChannelLook();
            var propertyInfo  = rptChannelLook.GetType().GetProperty(propertyNameParameterTodt);

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
        public void rpt_ChannelLook_Parameter_todt_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterTodt = "Parameter_todt";
            var rptChannelLook = new rpt_ChannelLook();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptChannelLook.GetType().GetProperty(propertyNameParameterTodt);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptChannelLook, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_ResourceName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameResourceName = "ResourceName";
            var rptChannelLook  = new rpt_ChannelLook();
            var propertyInfo  = rptChannelLook.GetType().GetProperty(propertyNameResourceName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_ResourceName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var rptChannelLook = new rpt_ChannelLook();
            rptChannelLook.ResourceName = Fixture.Create<string>();
            var stringType = rptChannelLook.ResourceName.GetType();

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
        public void rpt_ChannelLook_Section1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1";
            var rptChannelLook  = new rpt_ChannelLook();
            var propertyInfo  = rptChannelLook.GetType().GetProperty(propertyNameSection1);

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
        public void rpt_ChannelLook_Section1_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1";
            var rptChannelLook = new rpt_ChannelLook();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptChannelLook.GetType().GetProperty(propertyNameSection1);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptChannelLook, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Section2_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2";
            var rptChannelLook  = new rpt_ChannelLook();
            var propertyInfo  = rptChannelLook.GetType().GetProperty(propertyNameSection2);

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
        public void rpt_ChannelLook_Section2_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2";
            var rptChannelLook = new rpt_ChannelLook();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptChannelLook.GetType().GetProperty(propertyNameSection2);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptChannelLook, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Section3_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3";
            var rptChannelLook  = new rpt_ChannelLook();
            var propertyInfo  = rptChannelLook.GetType().GetProperty(propertyNameSection3);

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
        public void rpt_ChannelLook_Section3_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3";
            var rptChannelLook = new rpt_ChannelLook();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptChannelLook.GetType().GetProperty(propertyNameSection3);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptChannelLook, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Section4_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4";
            var rptChannelLook  = new rpt_ChannelLook();
            var propertyInfo  = rptChannelLook.GetType().GetProperty(propertyNameSection4);

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
        public void rpt_ChannelLook_Section4_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4";
            var rptChannelLook = new rpt_ChannelLook();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptChannelLook.GetType().GetProperty(propertyNameSection4);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptChannelLook, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Section5_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5";
            var rptChannelLook  = new rpt_ChannelLook();
            var propertyInfo  = rptChannelLook.GetType().GetProperty(propertyNameSection5);

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
        public void rpt_ChannelLook_Section5_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5";
            var rptChannelLook = new rpt_ChannelLook();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptChannelLook.GetType().GetProperty(propertyNameSection5);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptChannelLook, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Section6_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection6 = "Section6";
            var rptChannelLook  = new rpt_ChannelLook();
            var propertyInfo  = rptChannelLook.GetType().GetProperty(propertyNameSection6);

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
        public void rpt_ChannelLook_Section6_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection6 = "Section6";
            var rptChannelLook = new rpt_ChannelLook();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptChannelLook.GetType().GetProperty(propertyNameSection6);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptChannelLook, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Section7_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection7 = "Section7";
            var rptChannelLook  = new rpt_ChannelLook();
            var propertyInfo  = rptChannelLook.GetType().GetProperty(propertyNameSection7);

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
        public void rpt_ChannelLook_Section7_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection7 = "Section7";
            var rptChannelLook = new rpt_ChannelLook();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptChannelLook.GetType().GetProperty(propertyNameSection7);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptChannelLook, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_ChannelLook_Section8_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection8 = "Section8";
            var rptChannelLook  = new rpt_ChannelLook();
            var propertyInfo  = rptChannelLook.GetType().GetProperty(propertyNameSection8);

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
        public void rpt_ChannelLook_Section8_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection8 = "Section8";
            var rptChannelLook = new rpt_ChannelLook();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptChannelLook.GetType().GetProperty(propertyNameSection8);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptChannelLook, randomString, null));
        }
    }
}