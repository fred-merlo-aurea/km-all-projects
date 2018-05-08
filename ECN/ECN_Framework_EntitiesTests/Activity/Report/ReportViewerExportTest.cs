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
using ECN_Framework_Entities.Activity.Report;

namespace ECN_Framework_Entities.Activity.Report
{
    [TestFixture]
    public class ExportAttributeTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (ExportAttribute) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ExportAttribute_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var exportAttribute = Fixture.Create<ExportAttribute>();
            var fieldOrder = Fixture.Create<int>();
            var format = Fixture.Create<FormatType>();
            var total = Fixture.Create<int>();
            var ignore = Fixture.Create<bool>();
            var header = Fixture.Create<string>();

            // Act
            exportAttribute.FieldOrder = fieldOrder;
            exportAttribute.Format = format;
            exportAttribute.Total = total;
            exportAttribute.Ignore = ignore;
            exportAttribute.Header = header;

            // Assert
            exportAttribute.FieldOrder.ShouldBe(fieldOrder);
            exportAttribute.Format.ShouldBe(format);
            exportAttribute.Total.ShouldBe(total);
            exportAttribute.Ignore.ShouldBe(ignore);
            exportAttribute.Header.ShouldBe(header);
        }

        #endregion

        #region General Getters/Setters : Class (ExportAttribute) => Property (FieldOrder) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ExportAttribute_FieldOrder_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var exportAttribute = Fixture.Create<ExportAttribute>();
            exportAttribute.FieldOrder = Fixture.Create<int>();
            var intType = exportAttribute.FieldOrder.GetType();

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

        #region General Getters/Setters : Class (ExportAttribute) => Property (FieldOrder) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ExportAttribute_Class_Invalid_Property_FieldOrderNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFieldOrder = "FieldOrderNotPresent";
            var exportAttribute  = Fixture.Create<ExportAttribute>();

            // Act , Assert
            Should.NotThrow(() => exportAttribute.GetType().GetProperty(propertyNameFieldOrder));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ExportAttribute_FieldOrder_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFieldOrder = "FieldOrder";
            var exportAttribute  = Fixture.Create<ExportAttribute>();
            var propertyInfo  = exportAttribute.GetType().GetProperty(propertyNameFieldOrder);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ExportAttribute) => Property (Format) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ExportAttribute_Format_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameFormat = "Format";
            var exportAttribute = Fixture.Create<ExportAttribute>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = exportAttribute.GetType().GetProperty(propertyNameFormat);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(exportAttribute, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (ExportAttribute) => Property (Format) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ExportAttribute_Class_Invalid_Property_FormatNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFormat = "FormatNotPresent";
            var exportAttribute  = Fixture.Create<ExportAttribute>();

            // Act , Assert
            Should.NotThrow(() => exportAttribute.GetType().GetProperty(propertyNameFormat));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ExportAttribute_Format_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFormat = "Format";
            var exportAttribute  = Fixture.Create<ExportAttribute>();
            var propertyInfo  = exportAttribute.GetType().GetProperty(propertyNameFormat);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ExportAttribute) => Property (Header) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ExportAttribute_Header_Property_String_Type_Verify_Test()
        {
            // Arrange
            var exportAttribute = Fixture.Create<ExportAttribute>();
            exportAttribute.Header = Fixture.Create<string>();
            var stringType = exportAttribute.Header.GetType();

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

        #region General Getters/Setters : Class (ExportAttribute) => Property (Header) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ExportAttribute_Class_Invalid_Property_HeaderNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameHeader = "HeaderNotPresent";
            var exportAttribute  = Fixture.Create<ExportAttribute>();

            // Act , Assert
            Should.NotThrow(() => exportAttribute.GetType().GetProperty(propertyNameHeader));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ExportAttribute_Header_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameHeader = "Header";
            var exportAttribute  = Fixture.Create<ExportAttribute>();
            var propertyInfo  = exportAttribute.GetType().GetProperty(propertyNameHeader);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ExportAttribute) => Property (Ignore) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ExportAttribute_Ignore_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var exportAttribute = Fixture.Create<ExportAttribute>();
            exportAttribute.Ignore = Fixture.Create<bool>();
            var boolType = exportAttribute.Ignore.GetType();

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

        #region General Getters/Setters : Class (ExportAttribute) => Property (Ignore) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ExportAttribute_Class_Invalid_Property_IgnoreNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIgnore = "IgnoreNotPresent";
            var exportAttribute  = Fixture.Create<ExportAttribute>();

            // Act , Assert
            Should.NotThrow(() => exportAttribute.GetType().GetProperty(propertyNameIgnore));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ExportAttribute_Ignore_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIgnore = "Ignore";
            var exportAttribute  = Fixture.Create<ExportAttribute>();
            var propertyInfo  = exportAttribute.GetType().GetProperty(propertyNameIgnore);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ExportAttribute) => Property (Total) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ExportAttribute_Total_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var exportAttribute = Fixture.Create<ExportAttribute>();
            exportAttribute.Total = Fixture.Create<int>();
            var intType = exportAttribute.Total.GetType();

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

        #region General Getters/Setters : Class (ExportAttribute) => Property (Total) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ExportAttribute_Class_Invalid_Property_TotalNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotal = "TotalNotPresent";
            var exportAttribute  = Fixture.Create<ExportAttribute>();

            // Act , Assert
            Should.NotThrow(() => exportAttribute.GetType().GetProperty(propertyNameTotal));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ExportAttribute_Total_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotal = "Total";
            var exportAttribute  = Fixture.Create<ExportAttribute>();
            var propertyInfo  = exportAttribute.GetType().GetProperty(propertyNameTotal);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (ExportAttribute) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ExportAttribute_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new ExportAttribute());
        }

        #endregion

        #region General Constructor : Class (ExportAttribute) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ExportAttribute_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfExportAttribute = Fixture.CreateMany<ExportAttribute>(2).ToList();
            var firstExportAttribute = instancesOfExportAttribute.FirstOrDefault();
            var lastExportAttribute = instancesOfExportAttribute.Last();

            // Act, Assert
            firstExportAttribute.ShouldNotBeNull();
            lastExportAttribute.ShouldNotBeNull();
            firstExportAttribute.ShouldNotBeSameAs(lastExportAttribute);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ExportAttribute_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstExportAttribute = new ExportAttribute();
            var secondExportAttribute = new ExportAttribute();
            var thirdExportAttribute = new ExportAttribute();
            var fourthExportAttribute = new ExportAttribute();
            var fifthExportAttribute = new ExportAttribute();
            var sixthExportAttribute = new ExportAttribute();

            // Act, Assert
            firstExportAttribute.ShouldNotBeNull();
            secondExportAttribute.ShouldNotBeNull();
            thirdExportAttribute.ShouldNotBeNull();
            fourthExportAttribute.ShouldNotBeNull();
            fifthExportAttribute.ShouldNotBeNull();
            sixthExportAttribute.ShouldNotBeNull();
            firstExportAttribute.ShouldNotBeSameAs(secondExportAttribute);
            thirdExportAttribute.ShouldNotBeSameAs(firstExportAttribute);
            fourthExportAttribute.ShouldNotBeSameAs(firstExportAttribute);
            fifthExportAttribute.ShouldNotBeSameAs(firstExportAttribute);
            sixthExportAttribute.ShouldNotBeSameAs(firstExportAttribute);
            sixthExportAttribute.ShouldNotBeSameAs(fourthExportAttribute);
        }

        #endregion

        #region General Constructor : Class (ExportAttribute) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ExportAttribute_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var ignore = false;

            // Act
            var exportAttribute = new ExportAttribute();

            // Assert
            exportAttribute.Ignore.ShouldBeFalse();
        }

        #endregion

        #endregion

        #endregion
    }
}