using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator
{
    [TestFixture]
    public class EmailPreviewTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (EmailPreview) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreview_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var emailPreview = Fixture.Create<EmailPreview>();
            var emailTestId = Fixture.Create<int>();
            var blastId = Fixture.Create<int>();
            var customerId = Fixture.Create<int>();
            var zipFile = Fixture.Create<string>();
            var createdById = Fixture.Create<int>();
            var dateCreated = Fixture.Create<DateTime>();
            var timeCreated = Fixture.Create<TimeSpan>();
            var linkTestId = Fixture.Create<int>();
            var baseChannelGUId = Fixture.Create<Guid>();

            // Act
            emailPreview.EmailTestID = emailTestId;
            emailPreview.BlastID = blastId;
            emailPreview.CustomerID = customerId;
            emailPreview.ZipFile = zipFile;
            emailPreview.CreatedByID = createdById;
            emailPreview.DateCreated = dateCreated;
            emailPreview.TimeCreated = timeCreated;
            emailPreview.LinkTestID = linkTestId;
            emailPreview.BaseChannelGUID = baseChannelGUId;

            // Assert
            emailPreview.EmailTestID.ShouldBe(emailTestId);
            emailPreview.BlastID.ShouldBe(blastId);
            emailPreview.CustomerID.ShouldBe(customerId);
            emailPreview.ZipFile.ShouldBe(zipFile);
            emailPreview.CreatedByID.ShouldBe(createdById);
            emailPreview.DateCreated.ShouldBe(dateCreated);
            emailPreview.TimeCreated.ShouldBe(timeCreated);
            emailPreview.LinkTestID.ShouldBe(linkTestId);
            emailPreview.BaseChannelGUID.ShouldBe(baseChannelGUId);
        }

        #endregion

        #region General Getters/Setters : Class (EmailPreview) => Property (BaseChannelGUID) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreview_BaseChannelGUID_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameBaseChannelGUID = "BaseChannelGUID";
            var emailPreview = Fixture.Create<EmailPreview>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = emailPreview.GetType().GetProperty(propertyNameBaseChannelGUID);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(emailPreview, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (EmailPreview) => Property (BaseChannelGUID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreview_Class_Invalid_Property_BaseChannelGUIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBaseChannelGUID = "BaseChannelGUIDNotPresent";
            var emailPreview  = Fixture.Create<EmailPreview>();

            // Act , Assert
            Should.NotThrow(() => emailPreview.GetType().GetProperty(propertyNameBaseChannelGUID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreview_BaseChannelGUID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBaseChannelGUID = "BaseChannelGUID";
            var emailPreview  = Fixture.Create<EmailPreview>();
            var propertyInfo  = emailPreview.GetType().GetProperty(propertyNameBaseChannelGUID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailPreview) => Property (BlastID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreview_BlastID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailPreview = Fixture.Create<EmailPreview>();
            emailPreview.BlastID = Fixture.Create<int>();
            var intType = emailPreview.BlastID.GetType();

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

        #region General Getters/Setters : Class (EmailPreview) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreview_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var emailPreview  = Fixture.Create<EmailPreview>();

            // Act , Assert
            Should.NotThrow(() => emailPreview.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreview_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var emailPreview  = Fixture.Create<EmailPreview>();
            var propertyInfo  = emailPreview.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailPreview) => Property (CreatedByID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreview_CreatedByID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailPreview = Fixture.Create<EmailPreview>();
            emailPreview.CreatedByID = Fixture.Create<int>();
            var intType = emailPreview.CreatedByID.GetType();

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

        #region General Getters/Setters : Class (EmailPreview) => Property (CreatedByID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreview_Class_Invalid_Property_CreatedByIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedByID = "CreatedByIDNotPresent";
            var emailPreview  = Fixture.Create<EmailPreview>();

            // Act , Assert
            Should.NotThrow(() => emailPreview.GetType().GetProperty(propertyNameCreatedByID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreview_CreatedByID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedByID = "CreatedByID";
            var emailPreview  = Fixture.Create<EmailPreview>();
            var propertyInfo  = emailPreview.GetType().GetProperty(propertyNameCreatedByID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailPreview) => Property (CustomerID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreview_CustomerID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailPreview = Fixture.Create<EmailPreview>();
            emailPreview.CustomerID = Fixture.Create<int>();
            var intType = emailPreview.CustomerID.GetType();

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

        #region General Getters/Setters : Class (EmailPreview) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreview_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var emailPreview  = Fixture.Create<EmailPreview>();

            // Act , Assert
            Should.NotThrow(() => emailPreview.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreview_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var emailPreview  = Fixture.Create<EmailPreview>();
            var propertyInfo  = emailPreview.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailPreview) => Property (DateCreated) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreview_DateCreated_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameDateCreated = "DateCreated";
            var emailPreview = Fixture.Create<EmailPreview>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = emailPreview.GetType().GetProperty(propertyNameDateCreated);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(emailPreview, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (EmailPreview) => Property (DateCreated) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreview_Class_Invalid_Property_DateCreatedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDateCreated = "DateCreatedNotPresent";
            var emailPreview  = Fixture.Create<EmailPreview>();

            // Act , Assert
            Should.NotThrow(() => emailPreview.GetType().GetProperty(propertyNameDateCreated));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreview_DateCreated_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDateCreated = "DateCreated";
            var emailPreview  = Fixture.Create<EmailPreview>();
            var propertyInfo  = emailPreview.GetType().GetProperty(propertyNameDateCreated);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailPreview) => Property (EmailTestID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreview_EmailTestID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailPreview = Fixture.Create<EmailPreview>();
            emailPreview.EmailTestID = Fixture.Create<int>();
            var intType = emailPreview.EmailTestID.GetType();

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

        #region General Getters/Setters : Class (EmailPreview) => Property (EmailTestID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreview_Class_Invalid_Property_EmailTestIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailTestID = "EmailTestIDNotPresent";
            var emailPreview  = Fixture.Create<EmailPreview>();

            // Act , Assert
            Should.NotThrow(() => emailPreview.GetType().GetProperty(propertyNameEmailTestID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreview_EmailTestID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailTestID = "EmailTestID";
            var emailPreview  = Fixture.Create<EmailPreview>();
            var propertyInfo  = emailPreview.GetType().GetProperty(propertyNameEmailTestID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailPreview) => Property (LinkTestID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreview_LinkTestID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailPreview = Fixture.Create<EmailPreview>();
            emailPreview.LinkTestID = Fixture.Create<int>();
            var intType = emailPreview.LinkTestID.GetType();

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

        #region General Getters/Setters : Class (EmailPreview) => Property (LinkTestID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreview_Class_Invalid_Property_LinkTestIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLinkTestID = "LinkTestIDNotPresent";
            var emailPreview  = Fixture.Create<EmailPreview>();

            // Act , Assert
            Should.NotThrow(() => emailPreview.GetType().GetProperty(propertyNameLinkTestID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreview_LinkTestID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLinkTestID = "LinkTestID";
            var emailPreview  = Fixture.Create<EmailPreview>();
            var propertyInfo  = emailPreview.GetType().GetProperty(propertyNameLinkTestID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailPreview) => Property (TimeCreated) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreview_TimeCreated_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameTimeCreated = "TimeCreated";
            var emailPreview = Fixture.Create<EmailPreview>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = emailPreview.GetType().GetProperty(propertyNameTimeCreated);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(emailPreview, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (EmailPreview) => Property (TimeCreated) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreview_Class_Invalid_Property_TimeCreatedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTimeCreated = "TimeCreatedNotPresent";
            var emailPreview  = Fixture.Create<EmailPreview>();

            // Act , Assert
            Should.NotThrow(() => emailPreview.GetType().GetProperty(propertyNameTimeCreated));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreview_TimeCreated_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTimeCreated = "TimeCreated";
            var emailPreview  = Fixture.Create<EmailPreview>();
            var propertyInfo  = emailPreview.GetType().GetProperty(propertyNameTimeCreated);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailPreview) => Property (ZipFile) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreview_ZipFile_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailPreview = Fixture.Create<EmailPreview>();
            emailPreview.ZipFile = Fixture.Create<string>();
            var stringType = emailPreview.ZipFile.GetType();

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

        #region General Getters/Setters : Class (EmailPreview) => Property (ZipFile) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreview_Class_Invalid_Property_ZipFileNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameZipFile = "ZipFileNotPresent";
            var emailPreview  = Fixture.Create<EmailPreview>();

            // Act , Assert
            Should.NotThrow(() => emailPreview.GetType().GetProperty(propertyNameZipFile));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreview_ZipFile_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameZipFile = "ZipFile";
            var emailPreview  = Fixture.Create<EmailPreview>();
            var propertyInfo  = emailPreview.GetType().GetProperty(propertyNameZipFile);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (EmailPreview) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailPreview_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new EmailPreview());
        }

        #endregion

        #region General Constructor : Class (EmailPreview) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailPreview_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfEmailPreview = Fixture.CreateMany<EmailPreview>(2).ToList();
            var firstEmailPreview = instancesOfEmailPreview.FirstOrDefault();
            var lastEmailPreview = instancesOfEmailPreview.Last();

            // Act, Assert
            firstEmailPreview.ShouldNotBeNull();
            lastEmailPreview.ShouldNotBeNull();
            firstEmailPreview.ShouldNotBeSameAs(lastEmailPreview);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailPreview_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstEmailPreview = new EmailPreview();
            var secondEmailPreview = new EmailPreview();
            var thirdEmailPreview = new EmailPreview();
            var fourthEmailPreview = new EmailPreview();
            var fifthEmailPreview = new EmailPreview();
            var sixthEmailPreview = new EmailPreview();

            // Act, Assert
            firstEmailPreview.ShouldNotBeNull();
            secondEmailPreview.ShouldNotBeNull();
            thirdEmailPreview.ShouldNotBeNull();
            fourthEmailPreview.ShouldNotBeNull();
            fifthEmailPreview.ShouldNotBeNull();
            sixthEmailPreview.ShouldNotBeNull();
            firstEmailPreview.ShouldNotBeSameAs(secondEmailPreview);
            thirdEmailPreview.ShouldNotBeSameAs(firstEmailPreview);
            fourthEmailPreview.ShouldNotBeSameAs(firstEmailPreview);
            fifthEmailPreview.ShouldNotBeSameAs(firstEmailPreview);
            sixthEmailPreview.ShouldNotBeSameAs(firstEmailPreview);
            sixthEmailPreview.ShouldNotBeSameAs(fourthEmailPreview);
        }

        #endregion

        #endregion

        #endregion
    }
}