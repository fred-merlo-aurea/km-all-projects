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
    public class RptSurveyRespondentsTest : AbstractGenericTest
    {
        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_rpt_SurveyRespondents_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstrptSurveyRespondents = new rpt_SurveyRespondents();
            var secondrptSurveyRespondents = new rpt_SurveyRespondents();
            var thirdrptSurveyRespondents = new rpt_SurveyRespondents();
            var fourthrptSurveyRespondents = new rpt_SurveyRespondents();
            var fifthrptSurveyRespondents = new rpt_SurveyRespondents();
            var sixthrptSurveyRespondents = new rpt_SurveyRespondents();

            // Act, Assert
            firstrptSurveyRespondents.ShouldNotBeNull();
            secondrptSurveyRespondents.ShouldNotBeNull();
            thirdrptSurveyRespondents.ShouldNotBeNull();
            fourthrptSurveyRespondents.ShouldNotBeNull();
            fifthrptSurveyRespondents.ShouldNotBeNull();
            sixthrptSurveyRespondents.ShouldNotBeNull();
            firstrptSurveyRespondents.ShouldNotBeSameAs(secondrptSurveyRespondents);
            thirdrptSurveyRespondents.ShouldNotBeSameAs(firstrptSurveyRespondents);
            fourthrptSurveyRespondents.ShouldNotBeSameAs(firstrptSurveyRespondents);
            fifthrptSurveyRespondents.ShouldNotBeSameAs(firstrptSurveyRespondents);
            sixthrptSurveyRespondents.ShouldNotBeSameAs(firstrptSurveyRespondents);
            sixthrptSurveyRespondents.ShouldNotBeSameAs(fourthrptSurveyRespondents);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_rpt_SurveyRespondents_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(action: () => new rpt_SurveyRespondents());
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_Class_Invalid_Property_FullResourceNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFullResourceName = "FullResourceNameNotPresent";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyRespondents.GetType().GetProperty(propertyNameFullResourceName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_Class_Invalid_Property_NewGeneratorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameNewGenerator = "NewGeneratorNotPresent";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyRespondents.GetType().GetProperty(propertyNameNewGenerator));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_Class_Invalid_Property_Parameter_emailIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterEmailId = "Parameter_emailIDNotPresent";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyRespondents.GetType().GetProperty(propertyNameParameterEmailId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_Class_Invalid_Property_Parameter_ExportFormatNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterExportFormat = "Parameter_ExportFormatNotPresent";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyRespondents.GetType().GetProperty(propertyNameParameterExportFormat));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_Class_Invalid_Property_Parameter_filterstrNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterFilterstr = "Parameter_filterstrNotPresent";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyRespondents.GetType().GetProperty(propertyNameParameterFilterstr));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_Class_Invalid_Property_Parameter_surveyIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterSurveyId = "Parameter_surveyIDNotPresent";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyRespondents.GetType().GetProperty(propertyNameParameterSurveyId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_Class_Invalid_Property_Parameter_titleNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterTitle = "Parameter_titleNotPresent";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyRespondents.GetType().GetProperty(propertyNameParameterTitle));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_Class_Invalid_Property_Parameter_TotalRespondentNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterTotalRespondent = "Parameter_TotalRespondentNotPresent";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyRespondents.GetType().GetProperty(propertyNameParameterTotalRespondent));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_Class_Invalid_Property_ResourceNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameResourceName = "ResourceNameNotPresent";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyRespondents.GetType().GetProperty(propertyNameResourceName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_Class_Invalid_Property_Section1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1NotPresent";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyRespondents.GetType().GetProperty(propertyNameSection1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_Class_Invalid_Property_Section2NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2NotPresent";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyRespondents.GetType().GetProperty(propertyNameSection2));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_Class_Invalid_Property_Section3NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3NotPresent";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyRespondents.GetType().GetProperty(propertyNameSection3));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_Class_Invalid_Property_Section4NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4NotPresent";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyRespondents.GetType().GetProperty(propertyNameSection4));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_Class_Invalid_Property_Section5NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5NotPresent";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyRespondents.GetType().GetProperty(propertyNameSection5));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_Class_Invalid_Property_Section6NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection6 = "Section6NotPresent";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyRespondents.GetType().GetProperty(propertyNameSection6));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_FullResourceName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFullResourceName = "FullResourceName";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();
            var propertyInfo  = rptSurveyRespondents.GetType().GetProperty(propertyNameFullResourceName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_FullResourceName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var rptSurveyRespondents = new rpt_SurveyRespondents();
            rptSurveyRespondents.FullResourceName = Fixture.Create<string>();
            var stringType = rptSurveyRespondents.FullResourceName.GetType();

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
        public void rpt_SurveyRespondents_NewGenerator_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var rptSurveyRespondents = new rpt_SurveyRespondents();
            rptSurveyRespondents.NewGenerator = Fixture.Create<bool>();
            var boolType = rptSurveyRespondents.NewGenerator.GetType();

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
        public void rpt_SurveyRespondents_NewGenerator_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameNewGenerator = "NewGenerator";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();
            var propertyInfo  = rptSurveyRespondents.GetType().GetProperty(propertyNameNewGenerator);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_Parameter_emailID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterEmailId = "Parameter_emailID";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();
            var propertyInfo  = rptSurveyRespondents.GetType().GetProperty(propertyNameParameterEmailId);

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
        public void rpt_SurveyRespondents_Parameter_emailID_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterEmailId = "Parameter_emailID";
            var rptSurveyRespondents = new rpt_SurveyRespondents();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyRespondents.GetType().GetProperty(propertyNameParameterEmailId);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyRespondents, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_Parameter_ExportFormat_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterExportFormat = "Parameter_ExportFormat";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();
            var propertyInfo  = rptSurveyRespondents.GetType().GetProperty(propertyNameParameterExportFormat);

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
        public void rpt_SurveyRespondents_Parameter_ExportFormat_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterExportFormat = "Parameter_ExportFormat";
            var rptSurveyRespondents = new rpt_SurveyRespondents();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyRespondents.GetType().GetProperty(propertyNameParameterExportFormat);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyRespondents, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_Parameter_filterstr_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterFilterstr = "Parameter_filterstr";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();
            var propertyInfo  = rptSurveyRespondents.GetType().GetProperty(propertyNameParameterFilterstr);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_Parameter_surveyID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterSurveyId = "Parameter_surveyID";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();
            var propertyInfo  = rptSurveyRespondents.GetType().GetProperty(propertyNameParameterSurveyId);

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
        public void rpt_SurveyRespondents_Parameter_surveyID_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterSurveyId = "Parameter_surveyID";
            var rptSurveyRespondents = new rpt_SurveyRespondents();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyRespondents.GetType().GetProperty(propertyNameParameterSurveyId);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyRespondents, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_Parameter_title_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterTitle = "Parameter_title";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();
            var propertyInfo  = rptSurveyRespondents.GetType().GetProperty(propertyNameParameterTitle);

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
        public void rpt_SurveyRespondents_Parameter_title_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterTitle = "Parameter_title";
            var rptSurveyRespondents = new rpt_SurveyRespondents();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyRespondents.GetType().GetProperty(propertyNameParameterTitle);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyRespondents, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_Parameter_TotalRespondent_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterTotalRespondent = "Parameter_TotalRespondent";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();
            var propertyInfo  = rptSurveyRespondents.GetType().GetProperty(propertyNameParameterTotalRespondent);

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
        public void rpt_SurveyRespondents_Parameter_TotalRespondent_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterTotalRespondent = "Parameter_TotalRespondent";
            var rptSurveyRespondents = new rpt_SurveyRespondents();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyRespondents.GetType().GetProperty(propertyNameParameterTotalRespondent);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyRespondents, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_ResourceName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameResourceName = "ResourceName";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();
            var propertyInfo  = rptSurveyRespondents.GetType().GetProperty(propertyNameResourceName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_ResourceName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var rptSurveyRespondents = new rpt_SurveyRespondents();
            rptSurveyRespondents.ResourceName = Fixture.Create<string>();
            var stringType = rptSurveyRespondents.ResourceName.GetType();

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
        public void rpt_SurveyRespondents_Section1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();
            var propertyInfo  = rptSurveyRespondents.GetType().GetProperty(propertyNameSection1);

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
        public void rpt_SurveyRespondents_Section1_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1";
            var rptSurveyRespondents = new rpt_SurveyRespondents();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyRespondents.GetType().GetProperty(propertyNameSection1);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyRespondents, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_Section2_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();
            var propertyInfo  = rptSurveyRespondents.GetType().GetProperty(propertyNameSection2);

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
        public void rpt_SurveyRespondents_Section2_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2";
            var rptSurveyRespondents = new rpt_SurveyRespondents();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyRespondents.GetType().GetProperty(propertyNameSection2);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyRespondents, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_Section3_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();
            var propertyInfo  = rptSurveyRespondents.GetType().GetProperty(propertyNameSection3);

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
        public void rpt_SurveyRespondents_Section3_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3";
            var rptSurveyRespondents = new rpt_SurveyRespondents();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyRespondents.GetType().GetProperty(propertyNameSection3);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyRespondents, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_Section4_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();
            var propertyInfo  = rptSurveyRespondents.GetType().GetProperty(propertyNameSection4);

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
        public void rpt_SurveyRespondents_Section4_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4";
            var rptSurveyRespondents = new rpt_SurveyRespondents();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyRespondents.GetType().GetProperty(propertyNameSection4);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyRespondents, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_Section5_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();
            var propertyInfo  = rptSurveyRespondents.GetType().GetProperty(propertyNameSection5);

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
        public void rpt_SurveyRespondents_Section5_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5";
            var rptSurveyRespondents = new rpt_SurveyRespondents();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyRespondents.GetType().GetProperty(propertyNameSection5);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyRespondents, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyRespondents_Section6_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection6 = "Section6";
            var rptSurveyRespondents  = new rpt_SurveyRespondents();
            var propertyInfo  = rptSurveyRespondents.GetType().GetProperty(propertyNameSection6);

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
        public void rpt_SurveyRespondents_Section6_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection6 = "Section6";
            var rptSurveyRespondents = new rpt_SurveyRespondents();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyRespondents.GetType().GetProperty(propertyNameSection6);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyRespondents, randomString, null));
        }
    }
}