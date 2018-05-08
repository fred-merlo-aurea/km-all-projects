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
    public class RptSurveyTextResponseTest : AbstractGenericTest
    {
        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_rpt_SurveyTextResponse_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstrptSurveyTextResponse = new rpt_SurveyTextResponse();
            var secondrptSurveyTextResponse = new rpt_SurveyTextResponse();
            var thirdrptSurveyTextResponse = new rpt_SurveyTextResponse();
            var fourthrptSurveyTextResponse = new rpt_SurveyTextResponse();
            var fifthrptSurveyTextResponse = new rpt_SurveyTextResponse();
            var sixthrptSurveyTextResponse = new rpt_SurveyTextResponse();

            // Act, Assert
            firstrptSurveyTextResponse.ShouldNotBeNull();
            secondrptSurveyTextResponse.ShouldNotBeNull();
            thirdrptSurveyTextResponse.ShouldNotBeNull();
            fourthrptSurveyTextResponse.ShouldNotBeNull();
            fifthrptSurveyTextResponse.ShouldNotBeNull();
            sixthrptSurveyTextResponse.ShouldNotBeNull();
            firstrptSurveyTextResponse.ShouldNotBeSameAs(secondrptSurveyTextResponse);
            thirdrptSurveyTextResponse.ShouldNotBeSameAs(firstrptSurveyTextResponse);
            fourthrptSurveyTextResponse.ShouldNotBeSameAs(firstrptSurveyTextResponse);
            fifthrptSurveyTextResponse.ShouldNotBeSameAs(firstrptSurveyTextResponse);
            sixthrptSurveyTextResponse.ShouldNotBeSameAs(firstrptSurveyTextResponse);
            sixthrptSurveyTextResponse.ShouldNotBeSameAs(fourthrptSurveyTextResponse);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_rpt_SurveyTextResponse_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(action: () => new rpt_SurveyTextResponse());
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyTextResponse_Class_Invalid_Property_FullResourceNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFullResourceName = "FullResourceNameNotPresent";
            var rptSurveyTextResponse  = new rpt_SurveyTextResponse();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyTextResponse.GetType().GetProperty(propertyNameFullResourceName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyTextResponse_Class_Invalid_Property_NewGeneratorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameNewGenerator = "NewGeneratorNotPresent";
            var rptSurveyTextResponse  = new rpt_SurveyTextResponse();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyTextResponse.GetType().GetProperty(propertyNameNewGenerator));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyTextResponse_Class_Invalid_Property_Parameter_filterstrNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterFilterstr = "Parameter_filterstrNotPresent";
            var rptSurveyTextResponse  = new rpt_SurveyTextResponse();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyTextResponse.GetType().GetProperty(propertyNameParameterFilterstr));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyTextResponse_Class_Invalid_Property_Parameter_OtherTextNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterOtherText = "Parameter_OtherTextNotPresent";
            var rptSurveyTextResponse  = new rpt_SurveyTextResponse();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyTextResponse.GetType().GetProperty(propertyNameParameterOtherText));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyTextResponse_Class_Invalid_Property_Parameter_QuestionIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterQuestionId = "Parameter_QuestionIDNotPresent";
            var rptSurveyTextResponse  = new rpt_SurveyTextResponse();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyTextResponse.GetType().GetProperty(propertyNameParameterQuestionId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyTextResponse_Class_Invalid_Property_Parameter_QuestionNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterQuestion = "Parameter_QuestionNotPresent";
            var rptSurveyTextResponse  = new rpt_SurveyTextResponse();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyTextResponse.GetType().GetProperty(propertyNameParameterQuestion));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyTextResponse_Class_Invalid_Property_Parameter_titleNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterTitle = "Parameter_titleNotPresent";
            var rptSurveyTextResponse  = new rpt_SurveyTextResponse();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyTextResponse.GetType().GetProperty(propertyNameParameterTitle));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyTextResponse_Class_Invalid_Property_ResourceNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameResourceName = "ResourceNameNotPresent";
            var rptSurveyTextResponse  = new rpt_SurveyTextResponse();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyTextResponse.GetType().GetProperty(propertyNameResourceName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyTextResponse_Class_Invalid_Property_Section1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1NotPresent";
            var rptSurveyTextResponse  = new rpt_SurveyTextResponse();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyTextResponse.GetType().GetProperty(propertyNameSection1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyTextResponse_Class_Invalid_Property_Section2NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2NotPresent";
            var rptSurveyTextResponse  = new rpt_SurveyTextResponse();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyTextResponse.GetType().GetProperty(propertyNameSection2));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyTextResponse_Class_Invalid_Property_Section3NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3NotPresent";
            var rptSurveyTextResponse  = new rpt_SurveyTextResponse();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyTextResponse.GetType().GetProperty(propertyNameSection3));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyTextResponse_Class_Invalid_Property_Section4NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4NotPresent";
            var rptSurveyTextResponse  = new rpt_SurveyTextResponse();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyTextResponse.GetType().GetProperty(propertyNameSection4));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyTextResponse_Class_Invalid_Property_Section5NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5NotPresent";
            var rptSurveyTextResponse  = new rpt_SurveyTextResponse();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyTextResponse.GetType().GetProperty(propertyNameSection5));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyTextResponse_Class_Invalid_Property_Section6NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSection6 = "Section6NotPresent";
            var rptSurveyTextResponse  = new rpt_SurveyTextResponse();

            // Act , Assert
            Should.NotThrow(action: () => rptSurveyTextResponse.GetType().GetProperty(propertyNameSection6));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyTextResponse_FullResourceName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFullResourceName = "FullResourceName";
            var rptSurveyTextResponse  = new rpt_SurveyTextResponse();
            var propertyInfo  = rptSurveyTextResponse.GetType().GetProperty(propertyNameFullResourceName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyTextResponse_FullResourceName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var rptSurveyTextResponse = new rpt_SurveyTextResponse();
            rptSurveyTextResponse.FullResourceName = Fixture.Create<string>();
            var stringType = rptSurveyTextResponse.FullResourceName.GetType();

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
        public void rpt_SurveyTextResponse_NewGenerator_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var rptSurveyTextResponse = new rpt_SurveyTextResponse();
            rptSurveyTextResponse.NewGenerator = Fixture.Create<bool>();
            var boolType = rptSurveyTextResponse.NewGenerator.GetType();

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
        public void rpt_SurveyTextResponse_NewGenerator_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameNewGenerator = "NewGenerator";
            var rptSurveyTextResponse  = new rpt_SurveyTextResponse();
            var propertyInfo  = rptSurveyTextResponse.GetType().GetProperty(propertyNameNewGenerator);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyTextResponse_Parameter_filterstr_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterFilterstr = "Parameter_filterstr";
            var rptSurveyTextResponse  = new rpt_SurveyTextResponse();
            var propertyInfo  = rptSurveyTextResponse.GetType().GetProperty(propertyNameParameterFilterstr);

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
        public void rpt_SurveyTextResponse_Parameter_filterstr_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterFilterstr = "Parameter_filterstr";
            var rptSurveyTextResponse = new rpt_SurveyTextResponse();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyTextResponse.GetType().GetProperty(propertyNameParameterFilterstr);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyTextResponse, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyTextResponse_Parameter_OtherText_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterOtherText = "Parameter_OtherText";
            var rptSurveyTextResponse  = new rpt_SurveyTextResponse();
            var propertyInfo  = rptSurveyTextResponse.GetType().GetProperty(propertyNameParameterOtherText);

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
        public void rpt_SurveyTextResponse_Parameter_OtherText_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterOtherText = "Parameter_OtherText";
            var rptSurveyTextResponse = new rpt_SurveyTextResponse();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyTextResponse.GetType().GetProperty(propertyNameParameterOtherText);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyTextResponse, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyTextResponse_Parameter_Question_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterQuestion = "Parameter_Question";
            var rptSurveyTextResponse  = new rpt_SurveyTextResponse();
            var propertyInfo  = rptSurveyTextResponse.GetType().GetProperty(propertyNameParameterQuestion);

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
        public void rpt_SurveyTextResponse_Parameter_Question_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterQuestion = "Parameter_Question";
            var rptSurveyTextResponse = new rpt_SurveyTextResponse();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyTextResponse.GetType().GetProperty(propertyNameParameterQuestion);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyTextResponse, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyTextResponse_Parameter_QuestionID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterQuestionId = "Parameter_QuestionID";
            var rptSurveyTextResponse  = new rpt_SurveyTextResponse();
            var propertyInfo  = rptSurveyTextResponse.GetType().GetProperty(propertyNameParameterQuestionId);

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
        public void rpt_SurveyTextResponse_Parameter_QuestionID_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterQuestionId = "Parameter_QuestionID";
            var rptSurveyTextResponse = new rpt_SurveyTextResponse();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyTextResponse.GetType().GetProperty(propertyNameParameterQuestionId);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyTextResponse, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyTextResponse_Parameter_title_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParameterTitle = "Parameter_title";
            var rptSurveyTextResponse  = new rpt_SurveyTextResponse();
            var propertyInfo  = rptSurveyTextResponse.GetType().GetProperty(propertyNameParameterTitle);

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
        public void rpt_SurveyTextResponse_Parameter_title_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameParameterTitle = "Parameter_title";
            var rptSurveyTextResponse = new rpt_SurveyTextResponse();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyTextResponse.GetType().GetProperty(propertyNameParameterTitle);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyTextResponse, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyTextResponse_ResourceName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameResourceName = "ResourceName";
            var rptSurveyTextResponse  = new rpt_SurveyTextResponse();
            var propertyInfo  = rptSurveyTextResponse.GetType().GetProperty(propertyNameResourceName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyTextResponse_ResourceName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var rptSurveyTextResponse = new rpt_SurveyTextResponse();
            rptSurveyTextResponse.ResourceName = Fixture.Create<string>();
            var stringType = rptSurveyTextResponse.ResourceName.GetType();

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
        public void rpt_SurveyTextResponse_Section1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1";
            var rptSurveyTextResponse  = new rpt_SurveyTextResponse();
            var propertyInfo  = rptSurveyTextResponse.GetType().GetProperty(propertyNameSection1);

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
        public void rpt_SurveyTextResponse_Section1_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection1 = "Section1";
            var rptSurveyTextResponse = new rpt_SurveyTextResponse();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyTextResponse.GetType().GetProperty(propertyNameSection1);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyTextResponse, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyTextResponse_Section2_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2";
            var rptSurveyTextResponse  = new rpt_SurveyTextResponse();
            var propertyInfo  = rptSurveyTextResponse.GetType().GetProperty(propertyNameSection2);

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
        public void rpt_SurveyTextResponse_Section2_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection2 = "Section2";
            var rptSurveyTextResponse = new rpt_SurveyTextResponse();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyTextResponse.GetType().GetProperty(propertyNameSection2);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyTextResponse, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyTextResponse_Section3_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3";
            var rptSurveyTextResponse  = new rpt_SurveyTextResponse();
            var propertyInfo  = rptSurveyTextResponse.GetType().GetProperty(propertyNameSection3);

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
        public void rpt_SurveyTextResponse_Section3_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection3 = "Section3";
            var rptSurveyTextResponse = new rpt_SurveyTextResponse();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyTextResponse.GetType().GetProperty(propertyNameSection3);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyTextResponse, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyTextResponse_Section4_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4";
            var rptSurveyTextResponse  = new rpt_SurveyTextResponse();
            var propertyInfo  = rptSurveyTextResponse.GetType().GetProperty(propertyNameSection4);

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
        public void rpt_SurveyTextResponse_Section4_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection4 = "Section4";
            var rptSurveyTextResponse = new rpt_SurveyTextResponse();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyTextResponse.GetType().GetProperty(propertyNameSection4);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyTextResponse, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyTextResponse_Section5_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5";
            var rptSurveyTextResponse  = new rpt_SurveyTextResponse();
            var propertyInfo  = rptSurveyTextResponse.GetType().GetProperty(propertyNameSection5);

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
        public void rpt_SurveyTextResponse_Section5_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection5 = "Section5";
            var rptSurveyTextResponse = new rpt_SurveyTextResponse();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyTextResponse.GetType().GetProperty(propertyNameSection5);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyTextResponse, randomString, null));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void rpt_SurveyTextResponse_Section6_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSection6 = "Section6";
            var rptSurveyTextResponse  = new rpt_SurveyTextResponse();
            var propertyInfo  = rptSurveyTextResponse.GetType().GetProperty(propertyNameSection6);

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
        public void rpt_SurveyTextResponse_Section6_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSection6 = "Section6";
            var rptSurveyTextResponse = new rpt_SurveyTextResponse();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rptSurveyTextResponse.GetType().GetProperty(propertyNameSection6);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(actual: () => propertyInfo.SetValue(rptSurveyTextResponse, randomString, null));
        }
    }
}