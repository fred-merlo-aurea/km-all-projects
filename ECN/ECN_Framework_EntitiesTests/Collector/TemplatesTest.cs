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
    public class TemplatesTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var templates  = new Templates();
            var templateID = Fixture.Create<int>();
            var customerID = Fixture.Create<int>();
            var templateName = Fixture.Create<string>();
            var templateImage = Fixture.Create<string>();
            var isDefault = Fixture.Create<bool>();
            var pbgcolor = Fixture.Create<string>();
            var pAlign = Fixture.Create<string>();
            var pBorder = Fixture.Create<bool>();
            var pBordercolor = Fixture.Create<string>();
            var pfontfamily = Fixture.Create<string>();
            var pWidth = Fixture.Create<string>();
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
            var showQuestionNo = Fixture.Create<bool>();
            var acolor = Fixture.Create<string>();
            var abold = Fixture.Create<bool>();
            var afontsize = Fixture.Create<string>();
            var fImage = Fixture.Create<string>();
            var fAlign = Fixture.Create<string>();
            var fMargin = Fixture.Create<string>();
            var fbgcolor = Fixture.Create<string>();
            var isActive = Fixture.Create<bool>();
            var createdDate = Fixture.Create<DateTime?>();
            var createdUserID = Fixture.Create<int>();
            var updatedDate = Fixture.Create<DateTime?>();
            var updatedUserID = Fixture.Create<int>();

            // Act
            templates.TemplateID = templateID;
            templates.CustomerID = customerID;
            templates.TemplateName = templateName;
            templates.TemplateImage = templateImage;
            templates.IsDefault = isDefault;
            templates.pbgcolor = pbgcolor;
            templates.pAlign = pAlign;
            templates.pBorder = pBorder;
            templates.pBordercolor = pBordercolor;
            templates.pfontfamily = pfontfamily;
            templates.pWidth = pWidth;
            templates.hImage = hImage;
            templates.hAlign = hAlign;
            templates.hMargin = hMargin;
            templates.hbgcolor = hbgcolor;
            templates.phbgcolor = phbgcolor;
            templates.phfontsize = phfontsize;
            templates.phcolor = phcolor;
            templates.phBold = phBold;
            templates.pdbgcolor = pdbgcolor;
            templates.pdfontsize = pdfontsize;
            templates.pdcolor = pdcolor;
            templates.pdbold = pdbold;
            templates.bbgcolor = bbgcolor;
            templates.qcolor = qcolor;
            templates.qfontsize = qfontsize;
            templates.qbold = qbold;
            templates.ShowQuestionNo = showQuestionNo;
            templates.acolor = acolor;
            templates.abold = abold;
            templates.afontsize = afontsize;
            templates.fImage = fImage;
            templates.fAlign = fAlign;
            templates.fMargin = fMargin;
            templates.fbgcolor = fbgcolor;
            templates.IsActive = isActive;
            templates.CreatedDate = createdDate;
            templates.CreatedUserID = createdUserID;
            templates.UpdatedDate = updatedDate;
            templates.UpdatedUserID = updatedUserID;

            // Assert
            templates.TemplateID.ShouldBe(templateID);
            templates.CustomerID.ShouldBe(customerID);
            templates.TemplateName.ShouldBe(templateName);
            templates.TemplateImage.ShouldBe(templateImage);
            templates.IsDefault.ShouldBe(isDefault);
            templates.pbgcolor.ShouldBe(pbgcolor);
            templates.pAlign.ShouldBe(pAlign);
            templates.pBorder.ShouldBe(pBorder);
            templates.pBordercolor.ShouldBe(pBordercolor);
            templates.pfontfamily.ShouldBe(pfontfamily);
            templates.pWidth.ShouldBe(pWidth);
            templates.hImage.ShouldBe(hImage);
            templates.hAlign.ShouldBe(hAlign);
            templates.hMargin.ShouldBe(hMargin);
            templates.hbgcolor.ShouldBe(hbgcolor);
            templates.phbgcolor.ShouldBe(phbgcolor);
            templates.phfontsize.ShouldBe(phfontsize);
            templates.phcolor.ShouldBe(phcolor);
            templates.phBold.ShouldBe(phBold);
            templates.pdbgcolor.ShouldBe(pdbgcolor);
            templates.pdfontsize.ShouldBe(pdfontsize);
            templates.pdcolor.ShouldBe(pdcolor);
            templates.pdbold.ShouldBe(pdbold);
            templates.bbgcolor.ShouldBe(bbgcolor);
            templates.qcolor.ShouldBe(qcolor);
            templates.qfontsize.ShouldBe(qfontsize);
            templates.qbold.ShouldBe(qbold);
            templates.ShowQuestionNo.ShouldBe(showQuestionNo);
            templates.acolor.ShouldBe(acolor);
            templates.abold.ShouldBe(abold);
            templates.afontsize.ShouldBe(afontsize);
            templates.fImage.ShouldBe(fImage);
            templates.fAlign.ShouldBe(fAlign);
            templates.fMargin.ShouldBe(fMargin);
            templates.fbgcolor.ShouldBe(fbgcolor);
            templates.IsActive.ShouldBe(isActive);
            templates.CreatedDate.ShouldBe(createdDate);
            templates.CreatedUserID.ShouldBe(createdUserID);
            templates.UpdatedDate.ShouldBe(updatedDate);
            templates.UpdatedUserID.ShouldBe(updatedUserID);   
        }

        #endregion

        #region bool property type test : Templates => abold

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_abold_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var boolType = templates.abold.GetType();

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
        public void Templates_Class_Invalid_Property_aboldNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameabold = "aboldNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNameabold));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_abold_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameabold = "abold";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNameabold);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Templates => acolor

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_acolor_Property_String_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var stringType = templates.acolor.GetType();

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
        public void Templates_Class_Invalid_Property_acolorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameacolor = "acolorNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNameacolor));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_acolor_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameacolor = "acolor";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNameacolor);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Templates => afontsize

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_afontsize_Property_String_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var stringType = templates.afontsize.GetType();

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
        public void Templates_Class_Invalid_Property_afontsizeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameafontsize = "afontsizeNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNameafontsize));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_afontsize_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameafontsize = "afontsize";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNameafontsize);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Templates => bbgcolor

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_bbgcolor_Property_String_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var stringType = templates.bbgcolor.GetType();

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
        public void Templates_Class_Invalid_Property_bbgcolorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamebbgcolor = "bbgcolorNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNamebbgcolor));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_bbgcolor_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamebbgcolor = "bbgcolor";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNamebbgcolor);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : Templates => CreatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var templates = Fixture.Create<Templates>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = templates.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(templates, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : Templates => CreatedUserID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_CreatedUserID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var intType = templates.CreatedUserID.GetType();

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
        public void Templates_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : Templates => CustomerID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_CustomerID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var intType = templates.CustomerID.GetType();

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
        public void Templates_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Templates => fAlign

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_fAlign_Property_String_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var stringType = templates.fAlign.GetType();

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
        public void Templates_Class_Invalid_Property_fAlignNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamefAlign = "fAlignNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNamefAlign));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_fAlign_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamefAlign = "fAlign";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNamefAlign);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Templates => fbgcolor

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_fbgcolor_Property_String_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var stringType = templates.fbgcolor.GetType();

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
        public void Templates_Class_Invalid_Property_fbgcolorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamefbgcolor = "fbgcolorNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNamefbgcolor));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_fbgcolor_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamefbgcolor = "fbgcolor";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNamefbgcolor);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Templates => fImage

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_fImage_Property_String_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var stringType = templates.fImage.GetType();

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
        public void Templates_Class_Invalid_Property_fImageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamefImage = "fImageNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNamefImage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_fImage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamefImage = "fImage";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNamefImage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Templates => fMargin

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_fMargin_Property_String_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var stringType = templates.fMargin.GetType();

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
        public void Templates_Class_Invalid_Property_fMarginNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamefMargin = "fMarginNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNamefMargin));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_fMargin_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamefMargin = "fMargin";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNamefMargin);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Templates => hAlign

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_hAlign_Property_String_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var stringType = templates.hAlign.GetType();

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
        public void Templates_Class_Invalid_Property_hAlignNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamehAlign = "hAlignNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNamehAlign));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_hAlign_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamehAlign = "hAlign";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNamehAlign);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Templates => hbgcolor

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_hbgcolor_Property_String_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var stringType = templates.hbgcolor.GetType();

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
        public void Templates_Class_Invalid_Property_hbgcolorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamehbgcolor = "hbgcolorNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNamehbgcolor));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_hbgcolor_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamehbgcolor = "hbgcolor";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNamehbgcolor);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Templates => hImage

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_hImage_Property_String_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var stringType = templates.hImage.GetType();

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
        public void Templates_Class_Invalid_Property_hImageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamehImage = "hImageNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNamehImage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_hImage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamehImage = "hImage";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNamehImage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Templates => hMargin

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_hMargin_Property_String_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var stringType = templates.hMargin.GetType();

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
        public void Templates_Class_Invalid_Property_hMarginNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamehMargin = "hMarginNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNamehMargin));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_hMargin_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamehMargin = "hMargin";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNamehMargin);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region bool property type test : Templates => IsActive

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_IsActive_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var boolType = templates.IsActive.GetType();

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
        public void Templates_Class_Invalid_Property_IsActiveNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsActive = "IsActiveNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNameIsActive));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_IsActive_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsActive = "IsActive";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNameIsActive);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region bool property type test : Templates => IsDefault

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_IsDefault_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var boolType = templates.IsDefault.GetType();

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
        public void Templates_Class_Invalid_Property_IsDefaultNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDefault = "IsDefaultNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNameIsDefault));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_IsDefault_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDefault = "IsDefault";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNameIsDefault);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Templates => pAlign

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_pAlign_Property_String_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var stringType = templates.pAlign.GetType();

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
        public void Templates_Class_Invalid_Property_pAlignNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamepAlign = "pAlignNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNamepAlign));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_pAlign_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamepAlign = "pAlign";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNamepAlign);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Templates => pbgcolor

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_pbgcolor_Property_String_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var stringType = templates.pbgcolor.GetType();

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
        public void Templates_Class_Invalid_Property_pbgcolorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamepbgcolor = "pbgcolorNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNamepbgcolor));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_pbgcolor_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamepbgcolor = "pbgcolor";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNamepbgcolor);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region bool property type test : Templates => pBorder

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_pBorder_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var boolType = templates.pBorder.GetType();

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
        public void Templates_Class_Invalid_Property_pBorderNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamepBorder = "pBorderNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNamepBorder));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_pBorder_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamepBorder = "pBorder";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNamepBorder);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Templates => pBordercolor

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_pBordercolor_Property_String_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var stringType = templates.pBordercolor.GetType();

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
        public void Templates_Class_Invalid_Property_pBordercolorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamepBordercolor = "pBordercolorNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNamepBordercolor));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_pBordercolor_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamepBordercolor = "pBordercolor";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNamepBordercolor);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Templates => pdbgcolor

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_pdbgcolor_Property_String_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var stringType = templates.pdbgcolor.GetType();

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
        public void Templates_Class_Invalid_Property_pdbgcolorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamepdbgcolor = "pdbgcolorNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNamepdbgcolor));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_pdbgcolor_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamepdbgcolor = "pdbgcolor";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNamepdbgcolor);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region bool property type test : Templates => pdbold

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_pdbold_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var boolType = templates.pdbold.GetType();

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
        public void Templates_Class_Invalid_Property_pdboldNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamepdbold = "pdboldNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNamepdbold));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_pdbold_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamepdbold = "pdbold";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNamepdbold);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Templates => pdcolor

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_pdcolor_Property_String_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var stringType = templates.pdcolor.GetType();

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
        public void Templates_Class_Invalid_Property_pdcolorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamepdcolor = "pdcolorNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNamepdcolor));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_pdcolor_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamepdcolor = "pdcolor";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNamepdcolor);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Templates => pdfontsize

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_pdfontsize_Property_String_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var stringType = templates.pdfontsize.GetType();

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
        public void Templates_Class_Invalid_Property_pdfontsizeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamepdfontsize = "pdfontsizeNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNamepdfontsize));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_pdfontsize_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamepdfontsize = "pdfontsize";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNamepdfontsize);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Templates => pfontfamily

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_pfontfamily_Property_String_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var stringType = templates.pfontfamily.GetType();

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
        public void Templates_Class_Invalid_Property_pfontfamilyNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamepfontfamily = "pfontfamilyNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNamepfontfamily));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_pfontfamily_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamepfontfamily = "pfontfamily";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNamepfontfamily);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Templates => phbgcolor

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_phbgcolor_Property_String_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var stringType = templates.phbgcolor.GetType();

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
        public void Templates_Class_Invalid_Property_phbgcolorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamephbgcolor = "phbgcolorNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNamephbgcolor));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_phbgcolor_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamephbgcolor = "phbgcolor";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNamephbgcolor);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region bool property type test : Templates => phBold

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_phBold_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var boolType = templates.phBold.GetType();

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
        public void Templates_Class_Invalid_Property_phBoldNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamephBold = "phBoldNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNamephBold));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_phBold_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamephBold = "phBold";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNamephBold);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Templates => phcolor

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_phcolor_Property_String_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var stringType = templates.phcolor.GetType();

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
        public void Templates_Class_Invalid_Property_phcolorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamephcolor = "phcolorNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNamephcolor));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_phcolor_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamephcolor = "phcolor";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNamephcolor);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Templates => phfontsize

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_phfontsize_Property_String_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var stringType = templates.phfontsize.GetType();

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
        public void Templates_Class_Invalid_Property_phfontsizeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamephfontsize = "phfontsizeNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNamephfontsize));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_phfontsize_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamephfontsize = "phfontsize";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNamephfontsize);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Templates => pWidth

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_pWidth_Property_String_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var stringType = templates.pWidth.GetType();

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
        public void Templates_Class_Invalid_Property_pWidthNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamepWidth = "pWidthNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNamepWidth));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_pWidth_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamepWidth = "pWidth";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNamepWidth);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region bool property type test : Templates => qbold

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_qbold_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var boolType = templates.qbold.GetType();

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
        public void Templates_Class_Invalid_Property_qboldNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameqbold = "qboldNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNameqbold));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_qbold_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameqbold = "qbold";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNameqbold);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Templates => qcolor

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_qcolor_Property_String_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var stringType = templates.qcolor.GetType();

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
        public void Templates_Class_Invalid_Property_qcolorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameqcolor = "qcolorNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNameqcolor));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_qcolor_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameqcolor = "qcolor";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNameqcolor);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Templates => qfontsize

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_qfontsize_Property_String_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var stringType = templates.qfontsize.GetType();

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
        public void Templates_Class_Invalid_Property_qfontsizeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameqfontsize = "qfontsizeNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNameqfontsize));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_qfontsize_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameqfontsize = "qfontsize";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNameqfontsize);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region bool property type test : Templates => ShowQuestionNo

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_ShowQuestionNo_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var boolType = templates.ShowQuestionNo.GetType();

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
        public void Templates_Class_Invalid_Property_ShowQuestionNoNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameShowQuestionNo = "ShowQuestionNoNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNameShowQuestionNo));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_ShowQuestionNo_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameShowQuestionNo = "ShowQuestionNo";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNameShowQuestionNo);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : Templates => TemplateID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_TemplateID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var intType = templates.TemplateID.GetType();

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
        public void Templates_Class_Invalid_Property_TemplateIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTemplateID = "TemplateIDNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNameTemplateID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_TemplateID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTemplateID = "TemplateID";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNameTemplateID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Templates => TemplateImage

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_TemplateImage_Property_String_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var stringType = templates.TemplateImage.GetType();

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
        public void Templates_Class_Invalid_Property_TemplateImageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTemplateImage = "TemplateImageNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNameTemplateImage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_TemplateImage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTemplateImage = "TemplateImage";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNameTemplateImage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Templates => TemplateName

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_TemplateName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var stringType = templates.TemplateName.GetType();

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
        public void Templates_Class_Invalid_Property_TemplateNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTemplateName = "TemplateNameNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNameTemplateName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_TemplateName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTemplateName = "TemplateName";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNameTemplateName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : Templates => UpdatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var templates = Fixture.Create<Templates>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = templates.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(templates, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : Templates => UpdatedUserID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_UpdatedUserID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var templates = Fixture.Create<Templates>();
            var intType = templates.UpdatedUserID.GetType();

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
        public void Templates_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var templates  = Fixture.Create<Templates>();

            // Act , Assert
            Should.NotThrow(() => templates.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Templates_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var templates  = Fixture.Create<Templates>();
            var propertyInfo  = templates.GetType().GetProperty(propertyNameUpdatedUserID);

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
        public void Constructor_Templates_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new Templates());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<Templates>(2).ToList();
            var first = myInstances.FirstOrDefault();
            var last = myInstances.Last();

            // Act, Assert
            first.ShouldNotBeNull();
            last.ShouldNotBeNull();
            first.ShouldNotBeSameAs(last);
        }

        #endregion

        #region Contructor Default Assignment Test : Templates => Templates()

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Templates_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var templateID = -1;
            var customerID = -1;
            var templateName = string.Empty;
            var templateImage = string.Empty;
            var isDefault = false;
            var pbgcolor = string.Empty;
            var pAlign = string.Empty;
            var pBorder = false;
            var pBordercolor = string.Empty;
            var pfontfamily = string.Empty;
            var pWidth = string.Empty;
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
            var showQuestionNo = false;
            var acolor = string.Empty;
            var abold = false;
            var afontsize = string.Empty;
            var fImage = string.Empty;
            var fAlign = string.Empty;
            var fMargin = string.Empty;
            var fbgcolor = string.Empty;
            var isActive = false;
            DateTime? createdDate = null;
            var createdUserID = -1;
            DateTime? updatedDate = null;
            var updatedUserID = -1;

            // Act
            var templates = new Templates();

            // Assert
            templates.TemplateID.ShouldBe(templateID);
            templates.CustomerID.ShouldBe(customerID);
            templates.TemplateName.ShouldBe(templateName);
            templates.TemplateImage.ShouldBe(templateImage);
            templates.IsDefault.ShouldBeFalse();
            templates.pbgcolor.ShouldBe(pbgcolor);
            templates.pAlign.ShouldBe(pAlign);
            templates.pBorder.ShouldBeFalse();
            templates.pBordercolor.ShouldBe(pBordercolor);
            templates.pfontfamily.ShouldBe(pfontfamily);
            templates.pWidth.ShouldBe(pWidth);
            templates.hImage.ShouldBe(hImage);
            templates.hAlign.ShouldBe(hAlign);
            templates.hMargin.ShouldBe(hMargin);
            templates.hbgcolor.ShouldBe(hbgcolor);
            templates.phbgcolor.ShouldBe(phbgcolor);
            templates.phfontsize.ShouldBe(phfontsize);
            templates.phcolor.ShouldBe(phcolor);
            templates.phBold.ShouldBeFalse();
            templates.pdbgcolor.ShouldBe(pdbgcolor);
            templates.pdfontsize.ShouldBe(pdfontsize);
            templates.pdcolor.ShouldBe(pdcolor);
            templates.pdbold.ShouldBeFalse();
            templates.bbgcolor.ShouldBe(bbgcolor);
            templates.qcolor.ShouldBe(qcolor);
            templates.qfontsize.ShouldBe(qfontsize);
            templates.qbold.ShouldBeFalse();
            templates.ShowQuestionNo.ShouldBeFalse();
            templates.acolor.ShouldBe(acolor);
            templates.abold.ShouldBeFalse();
            templates.afontsize.ShouldBe(afontsize);
            templates.fImage.ShouldBe(fImage);
            templates.fAlign.ShouldBe(fAlign);
            templates.fMargin.ShouldBe(fMargin);
            templates.fbgcolor.ShouldBe(fbgcolor);
            templates.IsActive.ShouldBeFalse();
            templates.CreatedDate.ShouldBeNull();
            templates.CreatedUserID.ShouldBe(createdUserID);
            templates.UpdatedDate.ShouldBeNull();
            templates.UpdatedUserID.ShouldBe(updatedUserID);
        }

        #endregion


        #endregion


        #endregion
    }
}