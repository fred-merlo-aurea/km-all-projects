using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Reflection;
using Shouldly;
using AutoFixture;
using NUnit.Framework;
using Moq;
using ECN_Framework_EntitiesTests.ConfigureProject;
using ECN_Framework_Entities.Collector;

namespace ECN_Framework_Entities.Collector
{
    [TestFixture]
    public class SurveyStylesTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var surveyStyles  = new SurveyStyles();
            var surveyID = Fixture.Create<int>();
            var pWidth = Fixture.Create<string>();
            var pbgcolor = Fixture.Create<string>();
            var pAlign = Fixture.Create<string>();
            var pBorder = Fixture.Create<bool>();
            var pBordercolor = Fixture.Create<string>();
            var pfontfamily = Fixture.Create<string>();
            var hImage = Fixture.Create<string>();
            var hAlign = Fixture.Create<string>();
            var hMargin = Fixture.Create<string>();
            var hbgcolor = Fixture.Create<string>();
            var phbgcolor = Fixture.Create<string>();
            var phfontsize = Fixture.Create<string>();
            var phcolor = Fixture.Create<string>();
            var phBold = Fixture.Create<bool>();
            var pdbgcolor = Fixture.Create<string>();
            var pdfontsize = Fixture.Create<string>();
            var pdcolor = Fixture.Create<string>();
            var pdbold = Fixture.Create<bool>();
            var bbgcolor = Fixture.Create<string>();
            var qcolor = Fixture.Create<string>();
            var qfontsize = Fixture.Create<string>();
            var qbold = Fixture.Create<bool>();
            var acolor = Fixture.Create<string>();
            var abold = Fixture.Create<bool>();
            var afontsize = Fixture.Create<string>();
            var fImage = Fixture.Create<string>();
            var fAlign = Fixture.Create<string>();
            var fMargin = Fixture.Create<string>();
            var fbgcolor = Fixture.Create<string>();
            var showQuestionNo = Fixture.Create<bool>();

            // Act
            surveyStyles.SurveyID = surveyID;
            surveyStyles.pWidth = pWidth;
            surveyStyles.pbgcolor = pbgcolor;
            surveyStyles.pAlign = pAlign;
            surveyStyles.pBorder = pBorder;
            surveyStyles.pBordercolor = pBordercolor;
            surveyStyles.pfontfamily = pfontfamily;
            surveyStyles.hImage = hImage;
            surveyStyles.hAlign = hAlign;
            surveyStyles.hMargin = hMargin;
            surveyStyles.hbgcolor = hbgcolor;
            surveyStyles.phbgcolor = phbgcolor;
            surveyStyles.phfontsize = phfontsize;
            surveyStyles.phcolor = phcolor;
            surveyStyles.phBold = phBold;
            surveyStyles.pdbgcolor = pdbgcolor;
            surveyStyles.pdfontsize = pdfontsize;
            surveyStyles.pdcolor = pdcolor;
            surveyStyles.pdbold = pdbold;
            surveyStyles.bbgcolor = bbgcolor;
            surveyStyles.qcolor = qcolor;
            surveyStyles.qfontsize = qfontsize;
            surveyStyles.qbold = qbold;
            surveyStyles.acolor = acolor;
            surveyStyles.abold = abold;
            surveyStyles.afontsize = afontsize;
            surveyStyles.fImage = fImage;
            surveyStyles.fAlign = fAlign;
            surveyStyles.fMargin = fMargin;
            surveyStyles.fbgcolor = fbgcolor;
            surveyStyles.ShowQuestionNo = showQuestionNo;

            // Assert
            surveyStyles.SurveyID.ShouldBe(surveyID);
            surveyStyles.pWidth.ShouldBe(pWidth);
            surveyStyles.pbgcolor.ShouldBe(pbgcolor);
            surveyStyles.pAlign.ShouldBe(pAlign);
            surveyStyles.pBorder.ShouldBe(pBorder);
            surveyStyles.pBordercolor.ShouldBe(pBordercolor);
            surveyStyles.pfontfamily.ShouldBe(pfontfamily);
            surveyStyles.hImage.ShouldBe(hImage);
            surveyStyles.hAlign.ShouldBe(hAlign);
            surveyStyles.hMargin.ShouldBe(hMargin);
            surveyStyles.hbgcolor.ShouldBe(hbgcolor);
            surveyStyles.phbgcolor.ShouldBe(phbgcolor);
            surveyStyles.phfontsize.ShouldBe(phfontsize);
            surveyStyles.phcolor.ShouldBe(phcolor);
            surveyStyles.phBold.ShouldBe(phBold);
            surveyStyles.pdbgcolor.ShouldBe(pdbgcolor);
            surveyStyles.pdfontsize.ShouldBe(pdfontsize);
            surveyStyles.pdcolor.ShouldBe(pdcolor);
            surveyStyles.pdbold.ShouldBe(pdbold);
            surveyStyles.bbgcolor.ShouldBe(bbgcolor);
            surveyStyles.qcolor.ShouldBe(qcolor);
            surveyStyles.qfontsize.ShouldBe(qfontsize);
            surveyStyles.qbold.ShouldBe(qbold);
            surveyStyles.acolor.ShouldBe(acolor);
            surveyStyles.abold.ShouldBe(abold);
            surveyStyles.afontsize.ShouldBe(afontsize);
            surveyStyles.fImage.ShouldBe(fImage);
            surveyStyles.fAlign.ShouldBe(fAlign);
            surveyStyles.fMargin.ShouldBe(fMargin);
            surveyStyles.fbgcolor.ShouldBe(fbgcolor);
            surveyStyles.ShowQuestionNo.ShouldBe(showQuestionNo);   
        }

        #endregion

        #region bool property type test : SurveyStyles => abold

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_abold_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var boolType = surveyStyles.abold.GetType();

            // Act
            var isTypeBool = typeof(bool) == (boolType);    
            var isTypeNullableBool = typeof(bool?) == (boolType);
            var isTypeString = typeof(string) == (boolType);
            var isTypeInt = typeof(int) == (boolType);
            var isTypeDecimal = typeof(decimal) == (boolType);
            var isTypeLong = typeof(long) == (boolType);
            var isTypeDouble = typeof(double) == (boolType);
            var isTypeFloat = typeof(float) == (boolType);
            var isTypeIntNullable = typeof(int?) == (boolType);
            var isTypeDecimalNullable = typeof(decimal?) == (boolType);
            var isTypeLongNullable = typeof(long?) == (boolType);
            var isTypeDoubleNullable = typeof(double?) == (boolType);
            var isTypeFloatNullable = typeof(float?) == (boolType);

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

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_aboldNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameabold = "aboldNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNameabold));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_abold_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameabold = "abold";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNameabold);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SurveyStyles => acolor

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_acolor_Property_String_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var stringType = surveyStyles.acolor.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);    
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_acolorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameacolor = "acolorNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNameacolor));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_acolor_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameacolor = "acolor";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNameacolor);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SurveyStyles => afontsize

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_afontsize_Property_String_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var stringType = surveyStyles.afontsize.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);    
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_afontsizeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameafontsize = "afontsizeNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNameafontsize));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_afontsize_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameafontsize = "afontsize";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNameafontsize);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SurveyStyles => bbgcolor

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_bbgcolor_Property_String_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var stringType = surveyStyles.bbgcolor.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);    
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_bbgcolorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamebbgcolor = "bbgcolorNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNamebbgcolor));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_bbgcolor_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamebbgcolor = "bbgcolor";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNamebbgcolor);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SurveyStyles => fAlign

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_fAlign_Property_String_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var stringType = surveyStyles.fAlign.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);    
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_fAlignNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamefAlign = "fAlignNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNamefAlign));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_fAlign_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamefAlign = "fAlign";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNamefAlign);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SurveyStyles => fbgcolor

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_fbgcolor_Property_String_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var stringType = surveyStyles.fbgcolor.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);    
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_fbgcolorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamefbgcolor = "fbgcolorNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNamefbgcolor));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_fbgcolor_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamefbgcolor = "fbgcolor";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNamefbgcolor);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SurveyStyles => fImage

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_fImage_Property_String_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var stringType = surveyStyles.fImage.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);    
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_fImageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamefImage = "fImageNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNamefImage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_fImage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamefImage = "fImage";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNamefImage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SurveyStyles => fMargin

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_fMargin_Property_String_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var stringType = surveyStyles.fMargin.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);    
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_fMarginNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamefMargin = "fMarginNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNamefMargin));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_fMargin_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamefMargin = "fMargin";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNamefMargin);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SurveyStyles => hAlign

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_hAlign_Property_String_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var stringType = surveyStyles.hAlign.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);    
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_hAlignNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamehAlign = "hAlignNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNamehAlign));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_hAlign_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamehAlign = "hAlign";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNamehAlign);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SurveyStyles => hbgcolor

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_hbgcolor_Property_String_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var stringType = surveyStyles.hbgcolor.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);    
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_hbgcolorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamehbgcolor = "hbgcolorNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNamehbgcolor));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_hbgcolor_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamehbgcolor = "hbgcolor";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNamehbgcolor);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SurveyStyles => hImage

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_hImage_Property_String_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var stringType = surveyStyles.hImage.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);    
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_hImageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamehImage = "hImageNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNamehImage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_hImage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamehImage = "hImage";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNamehImage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SurveyStyles => hMargin

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_hMargin_Property_String_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var stringType = surveyStyles.hMargin.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);    
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_hMarginNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamehMargin = "hMarginNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNamehMargin));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_hMargin_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamehMargin = "hMargin";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNamehMargin);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SurveyStyles => pAlign

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_pAlign_Property_String_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var stringType = surveyStyles.pAlign.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);    
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_pAlignNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamepAlign = "pAlignNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNamepAlign));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_pAlign_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamepAlign = "pAlign";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNamepAlign);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SurveyStyles => pbgcolor

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_pbgcolor_Property_String_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var stringType = surveyStyles.pbgcolor.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);    
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_pbgcolorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamepbgcolor = "pbgcolorNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNamepbgcolor));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_pbgcolor_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamepbgcolor = "pbgcolor";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNamepbgcolor);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region bool property type test : SurveyStyles => pBorder

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_pBorder_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var boolType = surveyStyles.pBorder.GetType();

            // Act
            var isTypeBool = typeof(bool) == (boolType);    
            var isTypeNullableBool = typeof(bool?) == (boolType);
            var isTypeString = typeof(string) == (boolType);
            var isTypeInt = typeof(int) == (boolType);
            var isTypeDecimal = typeof(decimal) == (boolType);
            var isTypeLong = typeof(long) == (boolType);
            var isTypeDouble = typeof(double) == (boolType);
            var isTypeFloat = typeof(float) == (boolType);
            var isTypeIntNullable = typeof(int?) == (boolType);
            var isTypeDecimalNullable = typeof(decimal?) == (boolType);
            var isTypeLongNullable = typeof(long?) == (boolType);
            var isTypeDoubleNullable = typeof(double?) == (boolType);
            var isTypeFloatNullable = typeof(float?) == (boolType);

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

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_pBorderNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamepBorder = "pBorderNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNamepBorder));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_pBorder_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamepBorder = "pBorder";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNamepBorder);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SurveyStyles => pBordercolor

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_pBordercolor_Property_String_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var stringType = surveyStyles.pBordercolor.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);    
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_pBordercolorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamepBordercolor = "pBordercolorNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNamepBordercolor));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_pBordercolor_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamepBordercolor = "pBordercolor";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNamepBordercolor);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SurveyStyles => pdbgcolor

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_pdbgcolor_Property_String_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var stringType = surveyStyles.pdbgcolor.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);    
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_pdbgcolorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamepdbgcolor = "pdbgcolorNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNamepdbgcolor));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_pdbgcolor_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamepdbgcolor = "pdbgcolor";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNamepdbgcolor);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region bool property type test : SurveyStyles => pdbold

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_pdbold_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var boolType = surveyStyles.pdbold.GetType();

            // Act
            var isTypeBool = typeof(bool) == (boolType);    
            var isTypeNullableBool = typeof(bool?) == (boolType);
            var isTypeString = typeof(string) == (boolType);
            var isTypeInt = typeof(int) == (boolType);
            var isTypeDecimal = typeof(decimal) == (boolType);
            var isTypeLong = typeof(long) == (boolType);
            var isTypeDouble = typeof(double) == (boolType);
            var isTypeFloat = typeof(float) == (boolType);
            var isTypeIntNullable = typeof(int?) == (boolType);
            var isTypeDecimalNullable = typeof(decimal?) == (boolType);
            var isTypeLongNullable = typeof(long?) == (boolType);
            var isTypeDoubleNullable = typeof(double?) == (boolType);
            var isTypeFloatNullable = typeof(float?) == (boolType);

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

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_pdboldNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamepdbold = "pdboldNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNamepdbold));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_pdbold_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamepdbold = "pdbold";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNamepdbold);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SurveyStyles => pdcolor

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_pdcolor_Property_String_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var stringType = surveyStyles.pdcolor.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);    
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_pdcolorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamepdcolor = "pdcolorNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNamepdcolor));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_pdcolor_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamepdcolor = "pdcolor";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNamepdcolor);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SurveyStyles => pdfontsize

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_pdfontsize_Property_String_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var stringType = surveyStyles.pdfontsize.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);    
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_pdfontsizeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamepdfontsize = "pdfontsizeNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNamepdfontsize));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_pdfontsize_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamepdfontsize = "pdfontsize";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNamepdfontsize);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SurveyStyles => pfontfamily

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_pfontfamily_Property_String_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var stringType = surveyStyles.pfontfamily.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);    
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_pfontfamilyNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamepfontfamily = "pfontfamilyNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNamepfontfamily));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_pfontfamily_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamepfontfamily = "pfontfamily";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNamepfontfamily);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SurveyStyles => phbgcolor

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_phbgcolor_Property_String_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var stringType = surveyStyles.phbgcolor.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);    
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_phbgcolorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamephbgcolor = "phbgcolorNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNamephbgcolor));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_phbgcolor_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamephbgcolor = "phbgcolor";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNamephbgcolor);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region bool property type test : SurveyStyles => phBold

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_phBold_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var boolType = surveyStyles.phBold.GetType();

            // Act
            var isTypeBool = typeof(bool) == (boolType);    
            var isTypeNullableBool = typeof(bool?) == (boolType);
            var isTypeString = typeof(string) == (boolType);
            var isTypeInt = typeof(int) == (boolType);
            var isTypeDecimal = typeof(decimal) == (boolType);
            var isTypeLong = typeof(long) == (boolType);
            var isTypeDouble = typeof(double) == (boolType);
            var isTypeFloat = typeof(float) == (boolType);
            var isTypeIntNullable = typeof(int?) == (boolType);
            var isTypeDecimalNullable = typeof(decimal?) == (boolType);
            var isTypeLongNullable = typeof(long?) == (boolType);
            var isTypeDoubleNullable = typeof(double?) == (boolType);
            var isTypeFloatNullable = typeof(float?) == (boolType);

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

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_phBoldNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamephBold = "phBoldNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNamephBold));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_phBold_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamephBold = "phBold";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNamephBold);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SurveyStyles => phcolor

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_phcolor_Property_String_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var stringType = surveyStyles.phcolor.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);    
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_phcolorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamephcolor = "phcolorNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNamephcolor));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_phcolor_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamephcolor = "phcolor";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNamephcolor);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SurveyStyles => phfontsize

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_phfontsize_Property_String_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var stringType = surveyStyles.phfontsize.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);    
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_phfontsizeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamephfontsize = "phfontsizeNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNamephfontsize));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_phfontsize_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamephfontsize = "phfontsize";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNamephfontsize);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SurveyStyles => pWidth

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_pWidth_Property_String_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var stringType = surveyStyles.pWidth.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);    
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_pWidthNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamepWidth = "pWidthNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNamepWidth));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_pWidth_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamepWidth = "pWidth";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNamepWidth);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region bool property type test : SurveyStyles => qbold

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_qbold_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var boolType = surveyStyles.qbold.GetType();

            // Act
            var isTypeBool = typeof(bool) == (boolType);    
            var isTypeNullableBool = typeof(bool?) == (boolType);
            var isTypeString = typeof(string) == (boolType);
            var isTypeInt = typeof(int) == (boolType);
            var isTypeDecimal = typeof(decimal) == (boolType);
            var isTypeLong = typeof(long) == (boolType);
            var isTypeDouble = typeof(double) == (boolType);
            var isTypeFloat = typeof(float) == (boolType);
            var isTypeIntNullable = typeof(int?) == (boolType);
            var isTypeDecimalNullable = typeof(decimal?) == (boolType);
            var isTypeLongNullable = typeof(long?) == (boolType);
            var isTypeDoubleNullable = typeof(double?) == (boolType);
            var isTypeFloatNullable = typeof(float?) == (boolType);

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

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_qboldNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameqbold = "qboldNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNameqbold));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_qbold_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameqbold = "qbold";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNameqbold);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SurveyStyles => qcolor

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_qcolor_Property_String_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var stringType = surveyStyles.qcolor.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);    
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_qcolorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameqcolor = "qcolorNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNameqcolor));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_qcolor_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameqcolor = "qcolor";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNameqcolor);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SurveyStyles => qfontsize

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_qfontsize_Property_String_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var stringType = surveyStyles.qfontsize.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);    
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_qfontsizeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameqfontsize = "qfontsizeNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNameqfontsize));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_qfontsize_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameqfontsize = "qfontsize";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNameqfontsize);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region bool property type test : SurveyStyles => ShowQuestionNo

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_ShowQuestionNo_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var boolType = surveyStyles.ShowQuestionNo.GetType();

            // Act
            var isTypeBool = typeof(bool) == (boolType);    
            var isTypeNullableBool = typeof(bool?) == (boolType);
            var isTypeString = typeof(string) == (boolType);
            var isTypeInt = typeof(int) == (boolType);
            var isTypeDecimal = typeof(decimal) == (boolType);
            var isTypeLong = typeof(long) == (boolType);
            var isTypeDouble = typeof(double) == (boolType);
            var isTypeFloat = typeof(float) == (boolType);
            var isTypeIntNullable = typeof(int?) == (boolType);
            var isTypeDecimalNullable = typeof(decimal?) == (boolType);
            var isTypeLongNullable = typeof(long?) == (boolType);
            var isTypeDoubleNullable = typeof(double?) == (boolType);
            var isTypeFloatNullable = typeof(float?) == (boolType);

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

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_ShowQuestionNoNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameShowQuestionNo = "ShowQuestionNoNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNameShowQuestionNo));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_ShowQuestionNo_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameShowQuestionNo = "ShowQuestionNo";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNameShowQuestionNo);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : SurveyStyles => SurveyID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_SurveyID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var surveyStyles = Fixture.Create<SurveyStyles>();
            var intType = surveyStyles.SurveyID.GetType();

            // Act
            var isTypeInt = typeof(int) == (intType);    
            var isTypeNullableInt = typeof(int?) == (intType);
            var isTypeString = typeof(string) == (intType);
            var isTypeDecimal = typeof(decimal) == (intType);
            var isTypeLong = typeof(long) == (intType);
            var isTypeBool = typeof(bool) == (intType);
            var isTypeDouble = typeof(double) == (intType);
            var isTypeFloat = typeof(float) == (intType);
            var isTypeDecimalNullable = typeof(decimal?) == (intType);
            var isTypeLongNullable = typeof(long?) == (intType);
            var isTypeBoolNullable = typeof(bool?) == (intType);
            var isTypeDoubleNullable = typeof(double?) == (intType);
            var isTypeFloatNullable = typeof(float?) == (intType);

            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_Class_Invalid_Property_SurveyIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSurveyID = "SurveyIDNotPresent";
            var surveyStyles  = Fixture.Create<SurveyStyles>();

            // Act , Assert
            Should.NotThrow(() => surveyStyles.GetType().GetProperty(propertyNameSurveyID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyStyles_SurveyID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSurveyID = "SurveyID";
            var surveyStyles  = Fixture.Create<SurveyStyles>();
            var propertyInfo  = surveyStyles.GetType().GetProperty(propertyNameSurveyID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion


        #endregion
        #region Category : Constructor

        #region General Constructor Pattern : create and expect no exception.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_SurveyStyles_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new SurveyStyles());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<SurveyStyles>(2).ToList();
            var first = myInstances.FirstOrDefault();
            var last = myInstances.Last();

            // Act, Assert
            first.ShouldNotBeNull();
            last.ShouldNotBeNull();
            first.ShouldNotBeSameAs(last);
        }

        #endregion

        #region Contructor Default Assignment Test : SurveyStyles => SurveyStyles()

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_SurveyStyles_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var surveyID = -1;
            var pWidth = string.Empty;
            var pbgcolor = string.Empty;
            var pAlign = string.Empty;
            var pBorder = false;
            var pBordercolor = string.Empty;
            var pfontfamily = string.Empty;
            var hImage = string.Empty;
            var hAlign = string.Empty;
            var hMargin = string.Empty;
            var hbgcolor = string.Empty;
            var phbgcolor = string.Empty;
            var phfontsize = string.Empty;
            var phcolor = string.Empty;
            var phBold = false;
            var pdbgcolor = string.Empty;
            var pdfontsize = string.Empty;
            var pdcolor = string.Empty;
            var pdbold = false;
            var bbgcolor = string.Empty;
            var qcolor = string.Empty;
            var qfontsize = string.Empty;
            var qbold = false;
            var acolor = string.Empty;
            var abold = false;
            var afontsize = string.Empty;
            var fImage = string.Empty;
            var fAlign = string.Empty;
            var fMargin = string.Empty;
            var fbgcolor = string.Empty;
            var showQuestionNo = false;

            // Act
            var surveyStyles = new SurveyStyles();

            // Assert
            surveyStyles.SurveyID.ShouldBe(surveyID);
            surveyStyles.pWidth.ShouldBe(pWidth);
            surveyStyles.pbgcolor.ShouldBe(pbgcolor);
            surveyStyles.pAlign.ShouldBe(pAlign);
            surveyStyles.pBorder.ShouldBeFalse();
            surveyStyles.pBordercolor.ShouldBe(pBordercolor);
            surveyStyles.pfontfamily.ShouldBe(pfontfamily);
            surveyStyles.hImage.ShouldBe(hImage);
            surveyStyles.hAlign.ShouldBe(hAlign);
            surveyStyles.hMargin.ShouldBe(hMargin);
            surveyStyles.hbgcolor.ShouldBe(hbgcolor);
            surveyStyles.phbgcolor.ShouldBe(phbgcolor);
            surveyStyles.phfontsize.ShouldBe(phfontsize);
            surveyStyles.phcolor.ShouldBe(phcolor);
            surveyStyles.phBold.ShouldBeFalse();
            surveyStyles.pdbgcolor.ShouldBe(pdbgcolor);
            surveyStyles.pdfontsize.ShouldBe(pdfontsize);
            surveyStyles.pdcolor.ShouldBe(pdcolor);
            surveyStyles.pdbold.ShouldBeFalse();
            surveyStyles.bbgcolor.ShouldBe(bbgcolor);
            surveyStyles.qcolor.ShouldBe(qcolor);
            surveyStyles.qfontsize.ShouldBe(qfontsize);
            surveyStyles.qbold.ShouldBeFalse();
            surveyStyles.acolor.ShouldBe(acolor);
            surveyStyles.abold.ShouldBeFalse();
            surveyStyles.afontsize.ShouldBe(afontsize);
            surveyStyles.fImage.ShouldBe(fImage);
            surveyStyles.fAlign.ShouldBe(fAlign);
            surveyStyles.fMargin.ShouldBe(fMargin);
            surveyStyles.fbgcolor.ShouldBe(fbgcolor);
            surveyStyles.ShowQuestionNo.ShouldBeFalse();
        }

        #endregion


        #endregion


        #endregion
    }
}