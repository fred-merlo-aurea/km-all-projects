using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Content;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Content
{
    [TestFixture]
    public class PersonalizedContentTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (PersonalizedContent) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var personalizedContent = Fixture.Create<PersonalizedContent>();
            var personalizedContentId = Fixture.Create<Int64>();
            var blastId = Fixture.Create<Int64>();
            var emailAddress = Fixture.Create<string>();
            var emailSubject = Fixture.Create<string>();
            var hTMLContent = Fixture.Create<string>();
            var tEXTContent = Fixture.Create<string>();
            var isValid = Fixture.Create<bool>();
            var isProcessed = Fixture.Create<bool>();
            var isDeleted = Fixture.Create<bool>();
            var createdUserId = Fixture.Create<int>();
            var updatedUserId = Fixture.Create<int?>();

            // Act
            personalizedContent.PersonalizedContentID = personalizedContentId;
            personalizedContent.BlastID = blastId;
            personalizedContent.EmailAddress = emailAddress;
            personalizedContent.EmailSubject = emailSubject;
            personalizedContent.HTMLContent = hTMLContent;
            personalizedContent.TEXTContent = tEXTContent;
            personalizedContent.IsValid = isValid;
            personalizedContent.IsProcessed = isProcessed;
            personalizedContent.IsDeleted = isDeleted;
            personalizedContent.CreatedUserID = createdUserId;
            personalizedContent.UpdatedUserID = updatedUserId;

            // Assert
            personalizedContent.PersonalizedContentID.ShouldBe(personalizedContentId);
            personalizedContent.BlastID.ShouldBe(blastId);
            personalizedContent.EmailAddress.ShouldBe(emailAddress);
            personalizedContent.EmailSubject.ShouldBe(emailSubject);
            personalizedContent.HTMLContent.ShouldBe(hTMLContent);
            personalizedContent.TEXTContent.ShouldBe(tEXTContent);
            personalizedContent.IsValid.ShouldBe(isValid);
            personalizedContent.IsProcessed.ShouldBe(isProcessed);
            personalizedContent.IsDeleted.ShouldBe(isDeleted);
            personalizedContent.CreatedDate.ShouldNotBeNull();
            personalizedContent.CreatedUserID.ShouldBe(createdUserId);
            personalizedContent.UpdatedDate.ShouldBeNull();
            personalizedContent.UpdatedUserID.ShouldBe(updatedUserId);
        }

        #endregion

        #region General Getters/Setters : Class (PersonalizedContent) => Property (BlastID) (Type : Int64) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_BlastID_Property_Int64_Type_Verify_Test()
        {
            // Arrange
            var personalizedContent = Fixture.Create<PersonalizedContent>();
            personalizedContent.BlastID = Fixture.Create<Int64>();
            var int64Type = personalizedContent.BlastID.GetType();

            // Act
            var isTypeInt64 = typeof(Int64) == (int64Type);
            var isTypeNullableInt64 = typeof(Int64?) == (int64Type);
            var isTypeString = typeof(string) == (int64Type);
            var isTypeInt = typeof(int) == (int64Type);
            var isTypeDecimal = typeof(decimal) == (int64Type);
            var isTypeLong = typeof(long) == (int64Type);
            var isTypeBool = typeof(bool) == (int64Type);
            var isTypeDouble = typeof(double) == (int64Type);
            var isTypeFloat = typeof(float) == (int64Type);
            var isTypeIntNullable = typeof(int?) == (int64Type);
            var isTypeDecimalNullable = typeof(decimal?) == (int64Type);
            var isTypeLongNullable = typeof(long?) == (int64Type);
            var isTypeBoolNullable = typeof(bool?) == (int64Type);
            var isTypeDoubleNullable = typeof(double?) == (int64Type);
            var isTypeFloatNullable = typeof(float?) == (int64Type);

            // Assert
            isTypeInt64.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableInt64.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeTrue();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (PersonalizedContent) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var personalizedContent  = Fixture.Create<PersonalizedContent>();

            // Act , Assert
            Should.NotThrow(() => personalizedContent.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var personalizedContent  = Fixture.Create<PersonalizedContent>();
            var propertyInfo  = personalizedContent.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PersonalizedContent) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var personalizedContent = Fixture.Create<PersonalizedContent>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = personalizedContent.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(personalizedContent, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (PersonalizedContent) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var personalizedContent  = Fixture.Create<PersonalizedContent>();

            // Act , Assert
            Should.NotThrow(() => personalizedContent.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var personalizedContent  = Fixture.Create<PersonalizedContent>();
            var propertyInfo  = personalizedContent.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PersonalizedContent) => Property (CreatedUserID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_CreatedUserID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var personalizedContent = Fixture.Create<PersonalizedContent>();
            personalizedContent.CreatedUserID = Fixture.Create<int>();
            var intType = personalizedContent.CreatedUserID.GetType();

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

        #region General Getters/Setters : Class (PersonalizedContent) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var personalizedContent  = Fixture.Create<PersonalizedContent>();

            // Act , Assert
            Should.NotThrow(() => personalizedContent.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var personalizedContent  = Fixture.Create<PersonalizedContent>();
            var propertyInfo  = personalizedContent.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PersonalizedContent) => Property (EmailAddress) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_EmailAddress_Property_String_Type_Verify_Test()
        {
            // Arrange
            var personalizedContent = Fixture.Create<PersonalizedContent>();
            personalizedContent.EmailAddress = Fixture.Create<string>();
            var stringType = personalizedContent.EmailAddress.GetType();

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

        #region General Getters/Setters : Class (PersonalizedContent) => Property (EmailAddress) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_Class_Invalid_Property_EmailAddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddressNotPresent";
            var personalizedContent  = Fixture.Create<PersonalizedContent>();

            // Act , Assert
            Should.NotThrow(() => personalizedContent.GetType().GetProperty(propertyNameEmailAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_EmailAddress_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddress";
            var personalizedContent  = Fixture.Create<PersonalizedContent>();
            var propertyInfo  = personalizedContent.GetType().GetProperty(propertyNameEmailAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PersonalizedContent) => Property (EmailSubject) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_EmailSubject_Property_String_Type_Verify_Test()
        {
            // Arrange
            var personalizedContent = Fixture.Create<PersonalizedContent>();
            personalizedContent.EmailSubject = Fixture.Create<string>();
            var stringType = personalizedContent.EmailSubject.GetType();

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

        #region General Getters/Setters : Class (PersonalizedContent) => Property (EmailSubject) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_Class_Invalid_Property_EmailSubjectNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubjectNotPresent";
            var personalizedContent  = Fixture.Create<PersonalizedContent>();

            // Act , Assert
            Should.NotThrow(() => personalizedContent.GetType().GetProperty(propertyNameEmailSubject));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_EmailSubject_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubject";
            var personalizedContent  = Fixture.Create<PersonalizedContent>();
            var propertyInfo  = personalizedContent.GetType().GetProperty(propertyNameEmailSubject);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PersonalizedContent) => Property (HTMLContent) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_HTMLContent_Property_String_Type_Verify_Test()
        {
            // Arrange
            var personalizedContent = Fixture.Create<PersonalizedContent>();
            personalizedContent.HTMLContent = Fixture.Create<string>();
            var stringType = personalizedContent.HTMLContent.GetType();

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

        #region General Getters/Setters : Class (PersonalizedContent) => Property (HTMLContent) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_Class_Invalid_Property_HTMLContentNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameHTMLContent = "HTMLContentNotPresent";
            var personalizedContent  = Fixture.Create<PersonalizedContent>();

            // Act , Assert
            Should.NotThrow(() => personalizedContent.GetType().GetProperty(propertyNameHTMLContent));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_HTMLContent_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameHTMLContent = "HTMLContent";
            var personalizedContent  = Fixture.Create<PersonalizedContent>();
            var propertyInfo  = personalizedContent.GetType().GetProperty(propertyNameHTMLContent);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PersonalizedContent) => Property (IsDeleted) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_IsDeleted_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var personalizedContent = Fixture.Create<PersonalizedContent>();
            personalizedContent.IsDeleted = Fixture.Create<bool>();
            var boolType = personalizedContent.IsDeleted.GetType();

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

        #region General Getters/Setters : Class (PersonalizedContent) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var personalizedContent  = Fixture.Create<PersonalizedContent>();

            // Act , Assert
            Should.NotThrow(() => personalizedContent.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var personalizedContent  = Fixture.Create<PersonalizedContent>();
            var propertyInfo  = personalizedContent.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PersonalizedContent) => Property (IsProcessed) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_IsProcessed_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var personalizedContent = Fixture.Create<PersonalizedContent>();
            personalizedContent.IsProcessed = Fixture.Create<bool>();
            var boolType = personalizedContent.IsProcessed.GetType();

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

        #region General Getters/Setters : Class (PersonalizedContent) => Property (IsProcessed) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_Class_Invalid_Property_IsProcessedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsProcessed = "IsProcessedNotPresent";
            var personalizedContent  = Fixture.Create<PersonalizedContent>();

            // Act , Assert
            Should.NotThrow(() => personalizedContent.GetType().GetProperty(propertyNameIsProcessed));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_IsProcessed_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsProcessed = "IsProcessed";
            var personalizedContent  = Fixture.Create<PersonalizedContent>();
            var propertyInfo  = personalizedContent.GetType().GetProperty(propertyNameIsProcessed);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PersonalizedContent) => Property (IsValid) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_IsValid_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var personalizedContent = Fixture.Create<PersonalizedContent>();
            personalizedContent.IsValid = Fixture.Create<bool>();
            var boolType = personalizedContent.IsValid.GetType();

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

        #region General Getters/Setters : Class (PersonalizedContent) => Property (IsValid) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_Class_Invalid_Property_IsValidNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsValid = "IsValidNotPresent";
            var personalizedContent  = Fixture.Create<PersonalizedContent>();

            // Act , Assert
            Should.NotThrow(() => personalizedContent.GetType().GetProperty(propertyNameIsValid));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_IsValid_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsValid = "IsValid";
            var personalizedContent  = Fixture.Create<PersonalizedContent>();
            var propertyInfo  = personalizedContent.GetType().GetProperty(propertyNameIsValid);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PersonalizedContent) => Property (PersonalizedContentID) (Type : Int64) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_PersonalizedContentID_Property_Int64_Type_Verify_Test()
        {
            // Arrange
            var personalizedContent = Fixture.Create<PersonalizedContent>();
            personalizedContent.PersonalizedContentID = Fixture.Create<Int64>();
            var int64Type = personalizedContent.PersonalizedContentID.GetType();

            // Act
            var isTypeInt64 = typeof(Int64) == (int64Type);
            var isTypeNullableInt64 = typeof(Int64?) == (int64Type);
            var isTypeString = typeof(string) == (int64Type);
            var isTypeInt = typeof(int) == (int64Type);
            var isTypeDecimal = typeof(decimal) == (int64Type);
            var isTypeLong = typeof(long) == (int64Type);
            var isTypeBool = typeof(bool) == (int64Type);
            var isTypeDouble = typeof(double) == (int64Type);
            var isTypeFloat = typeof(float) == (int64Type);
            var isTypeIntNullable = typeof(int?) == (int64Type);
            var isTypeDecimalNullable = typeof(decimal?) == (int64Type);
            var isTypeLongNullable = typeof(long?) == (int64Type);
            var isTypeBoolNullable = typeof(bool?) == (int64Type);
            var isTypeDoubleNullable = typeof(double?) == (int64Type);
            var isTypeFloatNullable = typeof(float?) == (int64Type);

            // Assert
            isTypeInt64.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableInt64.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeTrue();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (PersonalizedContent) => Property (PersonalizedContentID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_Class_Invalid_Property_PersonalizedContentIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePersonalizedContentID = "PersonalizedContentIDNotPresent";
            var personalizedContent  = Fixture.Create<PersonalizedContent>();

            // Act , Assert
            Should.NotThrow(() => personalizedContent.GetType().GetProperty(propertyNamePersonalizedContentID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_PersonalizedContentID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePersonalizedContentID = "PersonalizedContentID";
            var personalizedContent  = Fixture.Create<PersonalizedContent>();
            var propertyInfo  = personalizedContent.GetType().GetProperty(propertyNamePersonalizedContentID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PersonalizedContent) => Property (TEXTContent) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_TEXTContent_Property_String_Type_Verify_Test()
        {
            // Arrange
            var personalizedContent = Fixture.Create<PersonalizedContent>();
            personalizedContent.TEXTContent = Fixture.Create<string>();
            var stringType = personalizedContent.TEXTContent.GetType();

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

        #region General Getters/Setters : Class (PersonalizedContent) => Property (TEXTContent) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_Class_Invalid_Property_TEXTContentNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTEXTContent = "TEXTContentNotPresent";
            var personalizedContent  = Fixture.Create<PersonalizedContent>();

            // Act , Assert
            Should.NotThrow(() => personalizedContent.GetType().GetProperty(propertyNameTEXTContent));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_TEXTContent_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTEXTContent = "TEXTContent";
            var personalizedContent  = Fixture.Create<PersonalizedContent>();
            var propertyInfo  = personalizedContent.GetType().GetProperty(propertyNameTEXTContent);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PersonalizedContent) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var personalizedContent = Fixture.Create<PersonalizedContent>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = personalizedContent.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(personalizedContent, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (PersonalizedContent) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var personalizedContent  = Fixture.Create<PersonalizedContent>();

            // Act , Assert
            Should.NotThrow(() => personalizedContent.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var personalizedContent  = Fixture.Create<PersonalizedContent>();
            var propertyInfo  = personalizedContent.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PersonalizedContent) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var personalizedContent = Fixture.Create<PersonalizedContent>();
            var random = Fixture.Create<int>();

            // Act , Set
            personalizedContent.UpdatedUserID = random;

            // Assert
            personalizedContent.UpdatedUserID.ShouldBe(random);
            personalizedContent.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var personalizedContent = Fixture.Create<PersonalizedContent>();

            // Act , Set
            personalizedContent.UpdatedUserID = null;

            // Assert
            personalizedContent.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var personalizedContent = Fixture.Create<PersonalizedContent>();
            var propertyInfo = personalizedContent.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(personalizedContent, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            personalizedContent.UpdatedUserID.ShouldBeNull();
            personalizedContent.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (PersonalizedContent) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var personalizedContent  = Fixture.Create<PersonalizedContent>();

            // Act , Assert
            Should.NotThrow(() => personalizedContent.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PersonalizedContent_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var personalizedContent  = Fixture.Create<PersonalizedContent>();
            var propertyInfo  = personalizedContent.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (PersonalizedContent) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_PersonalizedContent_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new PersonalizedContent());
        }

        #endregion

        #region General Constructor : Class (PersonalizedContent) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_PersonalizedContent_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfPersonalizedContent = Fixture.CreateMany<PersonalizedContent>(2).ToList();
            var firstPersonalizedContent = instancesOfPersonalizedContent.FirstOrDefault();
            var lastPersonalizedContent = instancesOfPersonalizedContent.Last();

            // Act, Assert
            firstPersonalizedContent.ShouldNotBeNull();
            lastPersonalizedContent.ShouldNotBeNull();
            firstPersonalizedContent.ShouldNotBeSameAs(lastPersonalizedContent);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_PersonalizedContent_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstPersonalizedContent = new PersonalizedContent();
            var secondPersonalizedContent = new PersonalizedContent();
            var thirdPersonalizedContent = new PersonalizedContent();
            var fourthPersonalizedContent = new PersonalizedContent();
            var fifthPersonalizedContent = new PersonalizedContent();
            var sixthPersonalizedContent = new PersonalizedContent();

            // Act, Assert
            firstPersonalizedContent.ShouldNotBeNull();
            secondPersonalizedContent.ShouldNotBeNull();
            thirdPersonalizedContent.ShouldNotBeNull();
            fourthPersonalizedContent.ShouldNotBeNull();
            fifthPersonalizedContent.ShouldNotBeNull();
            sixthPersonalizedContent.ShouldNotBeNull();
            firstPersonalizedContent.ShouldNotBeSameAs(secondPersonalizedContent);
            thirdPersonalizedContent.ShouldNotBeSameAs(firstPersonalizedContent);
            fourthPersonalizedContent.ShouldNotBeSameAs(firstPersonalizedContent);
            fifthPersonalizedContent.ShouldNotBeSameAs(firstPersonalizedContent);
            sixthPersonalizedContent.ShouldNotBeSameAs(firstPersonalizedContent);
            sixthPersonalizedContent.ShouldNotBeSameAs(fourthPersonalizedContent);
        }

        #endregion

        #region General Constructor : Class (PersonalizedContent) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_PersonalizedContent_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var personalizedContentId = 0;
            var blastId = 0;
            var emailAddress = string.Empty;
            var emailSubject = string.Empty;
            var hTMLContent = string.Empty;
            var tEXTContent = string.Empty;
            var isValid = true;
            var isDeleted = false;
            var isProcessed = false;
            var createdUserId = 0;

            // Act
            var personalizedContent = new PersonalizedContent();

            // Assert
            personalizedContent.PersonalizedContentID.ShouldBe(personalizedContentId);
            personalizedContent.BlastID.ShouldBe(blastId);
            personalizedContent.EmailAddress.ShouldBe(emailAddress);
            personalizedContent.EmailSubject.ShouldBe(emailSubject);
            personalizedContent.HTMLContent.ShouldBe(hTMLContent);
            personalizedContent.TEXTContent.ShouldBe(tEXTContent);
            personalizedContent.IsValid.ShouldBeTrue();
            personalizedContent.IsDeleted.ShouldBeFalse();
            personalizedContent.IsProcessed.ShouldBeFalse();
            personalizedContent.CreatedDate.ShouldNotBeNull();
            personalizedContent.CreatedUserID.ShouldBe(createdUserId);
            personalizedContent.UpdatedUserID.ShouldBeNull();
            personalizedContent.UpdatedDate.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}