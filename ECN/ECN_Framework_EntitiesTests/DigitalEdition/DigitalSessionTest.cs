using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.DigitalEdition;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.DigitalEdition
{
    [TestFixture]
    public class DigitalSessionTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (DigitalSession) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var digitalSession = Fixture.Create<DigitalSession>();
            var publicationCode = Fixture.Create<string>();
            var editionId = Fixture.Create<int>();
            var emailId = Fixture.Create<int>();
            var blastId = Fixture.Create<int>();
            var totalpages = Fixture.Create<int>();
            var isLoginRequired = Fixture.Create<bool?>();
            var isAuthenticated = Fixture.Create<bool>();
            var publicationtype = Fixture.Create<string>();
            var imagePath = Fixture.Create<string>();

            // Act
            digitalSession.PublicationCode = publicationCode;
            digitalSession.EditionID = editionId;
            digitalSession.EmailID = emailId;
            digitalSession.BlastID = blastId;
            digitalSession.Totalpages = totalpages;
            digitalSession.IsLoginRequired = isLoginRequired;
            digitalSession.IsAuthenticated = isAuthenticated;
            digitalSession.Publicationtype = publicationtype;
            digitalSession.ImagePath = imagePath;

            // Assert
            digitalSession.PublicationCode.ShouldBe(publicationCode);
            digitalSession.EditionID.ShouldBe(editionId);
            digitalSession.EmailID.ShouldBe(emailId);
            digitalSession.BlastID.ShouldBe(blastId);
            digitalSession.Totalpages.ShouldBe(totalpages);
            digitalSession.IsLoginRequired.ShouldBe(isLoginRequired);
            digitalSession.IsAuthenticated.ShouldBe(isAuthenticated);
            digitalSession.Publicationtype.ShouldBe(publicationtype);
            digitalSession.ImagePath.ShouldBe(imagePath);
        }

        #endregion

        #region General Getters/Setters : Class (DigitalSession) => Property (BlastID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_BlastID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var digitalSession = Fixture.Create<DigitalSession>();
            digitalSession.BlastID = Fixture.Create<int>();
            var intType = digitalSession.BlastID.GetType();

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

        #region General Getters/Setters : Class (DigitalSession) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var digitalSession  = Fixture.Create<DigitalSession>();

            // Act , Assert
            Should.NotThrow(() => digitalSession.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var digitalSession  = Fixture.Create<DigitalSession>();
            var propertyInfo  = digitalSession.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DigitalSession) => Property (EditionID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_EditionID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var digitalSession = Fixture.Create<DigitalSession>();
            digitalSession.EditionID = Fixture.Create<int>();
            var intType = digitalSession.EditionID.GetType();

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

        #region General Getters/Setters : Class (DigitalSession) => Property (EditionID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_Class_Invalid_Property_EditionIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEditionID = "EditionIDNotPresent";
            var digitalSession  = Fixture.Create<DigitalSession>();

            // Act , Assert
            Should.NotThrow(() => digitalSession.GetType().GetProperty(propertyNameEditionID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_EditionID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEditionID = "EditionID";
            var digitalSession  = Fixture.Create<DigitalSession>();
            var propertyInfo  = digitalSession.GetType().GetProperty(propertyNameEditionID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DigitalSession) => Property (EmailID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_EmailID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var digitalSession = Fixture.Create<DigitalSession>();
            digitalSession.EmailID = Fixture.Create<int>();
            var intType = digitalSession.EmailID.GetType();

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

        #region General Getters/Setters : Class (DigitalSession) => Property (EmailID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_Class_Invalid_Property_EmailIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailIDNotPresent";
            var digitalSession  = Fixture.Create<DigitalSession>();

            // Act , Assert
            Should.NotThrow(() => digitalSession.GetType().GetProperty(propertyNameEmailID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_EmailID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailID";
            var digitalSession  = Fixture.Create<DigitalSession>();
            var propertyInfo  = digitalSession.GetType().GetProperty(propertyNameEmailID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DigitalSession) => Property (ImagePath) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_ImagePath_Property_String_Type_Verify_Test()
        {
            // Arrange
            var digitalSession = Fixture.Create<DigitalSession>();
            digitalSession.ImagePath = Fixture.Create<string>();
            var stringType = digitalSession.ImagePath.GetType();

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

        #region General Getters/Setters : Class (DigitalSession) => Property (ImagePath) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_Class_Invalid_Property_ImagePathNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameImagePath = "ImagePathNotPresent";
            var digitalSession  = Fixture.Create<DigitalSession>();

            // Act , Assert
            Should.NotThrow(() => digitalSession.GetType().GetProperty(propertyNameImagePath));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_ImagePath_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameImagePath = "ImagePath";
            var digitalSession  = Fixture.Create<DigitalSession>();
            var propertyInfo  = digitalSession.GetType().GetProperty(propertyNameImagePath);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DigitalSession) => Property (IsAuthenticated) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_IsAuthenticated_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var digitalSession = Fixture.Create<DigitalSession>();
            digitalSession.IsAuthenticated = Fixture.Create<bool>();
            var boolType = digitalSession.IsAuthenticated.GetType();

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

        #region General Getters/Setters : Class (DigitalSession) => Property (IsAuthenticated) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_Class_Invalid_Property_IsAuthenticatedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsAuthenticated = "IsAuthenticatedNotPresent";
            var digitalSession  = Fixture.Create<DigitalSession>();

            // Act , Assert
            Should.NotThrow(() => digitalSession.GetType().GetProperty(propertyNameIsAuthenticated));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_IsAuthenticated_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsAuthenticated = "IsAuthenticated";
            var digitalSession  = Fixture.Create<DigitalSession>();
            var propertyInfo  = digitalSession.GetType().GetProperty(propertyNameIsAuthenticated);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DigitalSession) => Property (IsLoginRequired) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_IsLoginRequired_Property_Data_Without_Null_Test()
        {
            // Arrange
            var digitalSession = Fixture.Create<DigitalSession>();
            var random = Fixture.Create<bool>();

            // Act , Set
            digitalSession.IsLoginRequired = random;

            // Assert
            digitalSession.IsLoginRequired.ShouldBe(random);
            digitalSession.IsLoginRequired.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_IsLoginRequired_Property_Only_Null_Data_Test()
        {
            // Arrange
            var digitalSession = Fixture.Create<DigitalSession>();

            // Act , Set
            digitalSession.IsLoginRequired = null;

            // Assert
            digitalSession.IsLoginRequired.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_IsLoginRequired_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsLoginRequired = "IsLoginRequired";
            var digitalSession = Fixture.Create<DigitalSession>();
            var propertyInfo = digitalSession.GetType().GetProperty(propertyNameIsLoginRequired);

            // Act , Set
            propertyInfo.SetValue(digitalSession, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            digitalSession.IsLoginRequired.ShouldBeNull();
            digitalSession.IsLoginRequired.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DigitalSession) => Property (IsLoginRequired) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_Class_Invalid_Property_IsLoginRequiredNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsLoginRequired = "IsLoginRequiredNotPresent";
            var digitalSession  = Fixture.Create<DigitalSession>();

            // Act , Assert
            Should.NotThrow(() => digitalSession.GetType().GetProperty(propertyNameIsLoginRequired));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_IsLoginRequired_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsLoginRequired = "IsLoginRequired";
            var digitalSession  = Fixture.Create<DigitalSession>();
            var propertyInfo  = digitalSession.GetType().GetProperty(propertyNameIsLoginRequired);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DigitalSession) => Property (PublicationCode) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_PublicationCode_Property_String_Type_Verify_Test()
        {
            // Arrange
            var digitalSession = Fixture.Create<DigitalSession>();
            digitalSession.PublicationCode = Fixture.Create<string>();
            var stringType = digitalSession.PublicationCode.GetType();

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

        #region General Getters/Setters : Class (DigitalSession) => Property (PublicationCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_Class_Invalid_Property_PublicationCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePublicationCode = "PublicationCodeNotPresent";
            var digitalSession  = Fixture.Create<DigitalSession>();

            // Act , Assert
            Should.NotThrow(() => digitalSession.GetType().GetProperty(propertyNamePublicationCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_PublicationCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePublicationCode = "PublicationCode";
            var digitalSession  = Fixture.Create<DigitalSession>();
            var propertyInfo  = digitalSession.GetType().GetProperty(propertyNamePublicationCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DigitalSession) => Property (Publicationtype) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_Publicationtype_Property_String_Type_Verify_Test()
        {
            // Arrange
            var digitalSession = Fixture.Create<DigitalSession>();
            digitalSession.Publicationtype = Fixture.Create<string>();
            var stringType = digitalSession.Publicationtype.GetType();

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

        #region General Getters/Setters : Class (DigitalSession) => Property (Publicationtype) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_Class_Invalid_Property_PublicationtypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePublicationtype = "PublicationtypeNotPresent";
            var digitalSession  = Fixture.Create<DigitalSession>();

            // Act , Assert
            Should.NotThrow(() => digitalSession.GetType().GetProperty(propertyNamePublicationtype));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_Publicationtype_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePublicationtype = "Publicationtype";
            var digitalSession  = Fixture.Create<DigitalSession>();
            var propertyInfo  = digitalSession.GetType().GetProperty(propertyNamePublicationtype);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DigitalSession) => Property (Totalpages) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_Totalpages_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var digitalSession = Fixture.Create<DigitalSession>();
            digitalSession.Totalpages = Fixture.Create<int>();
            var intType = digitalSession.Totalpages.GetType();

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

        #region General Getters/Setters : Class (DigitalSession) => Property (Totalpages) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_Class_Invalid_Property_TotalpagesNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalpages = "TotalpagesNotPresent";
            var digitalSession  = Fixture.Create<DigitalSession>();

            // Act , Assert
            Should.NotThrow(() => digitalSession.GetType().GetProperty(propertyNameTotalpages));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DigitalSession_Totalpages_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotalpages = "Totalpages";
            var digitalSession  = Fixture.Create<DigitalSession>();
            var propertyInfo  = digitalSession.GetType().GetProperty(propertyNameTotalpages);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (DigitalSession) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DigitalSession_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new DigitalSession());
        }

        #endregion

        #region General Constructor : Class (DigitalSession) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DigitalSession_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfDigitalSession = Fixture.CreateMany<DigitalSession>(2).ToList();
            var firstDigitalSession = instancesOfDigitalSession.FirstOrDefault();
            var lastDigitalSession = instancesOfDigitalSession.Last();

            // Act, Assert
            firstDigitalSession.ShouldNotBeNull();
            lastDigitalSession.ShouldNotBeNull();
            firstDigitalSession.ShouldNotBeSameAs(lastDigitalSession);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DigitalSession_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstDigitalSession = new DigitalSession();
            var secondDigitalSession = new DigitalSession();
            var thirdDigitalSession = new DigitalSession();
            var fourthDigitalSession = new DigitalSession();
            var fifthDigitalSession = new DigitalSession();
            var sixthDigitalSession = new DigitalSession();

            // Act, Assert
            firstDigitalSession.ShouldNotBeNull();
            secondDigitalSession.ShouldNotBeNull();
            thirdDigitalSession.ShouldNotBeNull();
            fourthDigitalSession.ShouldNotBeNull();
            fifthDigitalSession.ShouldNotBeNull();
            sixthDigitalSession.ShouldNotBeNull();
            firstDigitalSession.ShouldNotBeSameAs(secondDigitalSession);
            thirdDigitalSession.ShouldNotBeSameAs(firstDigitalSession);
            fourthDigitalSession.ShouldNotBeSameAs(firstDigitalSession);
            fifthDigitalSession.ShouldNotBeSameAs(firstDigitalSession);
            sixthDigitalSession.ShouldNotBeSameAs(firstDigitalSession);
            sixthDigitalSession.ShouldNotBeSameAs(fourthDigitalSession);
        }

        #endregion

        #region General Constructor : Class (DigitalSession) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DigitalSession_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var publicationCode = string.Empty;
            var editionId = -1;
            var emailId = -1;
            var blastId = -1;
            var totalpages = 0;
            var isLoginRequired = false;
            var isAuthenticated = false;
            var publicationtype = string.Empty;
            var imagePath = string.Empty;

            // Act
            var digitalSession = new DigitalSession();

            // Assert
            digitalSession.PublicationCode.ShouldBe(publicationCode);
            digitalSession.EditionID.ShouldBe(editionId);
            digitalSession.EmailID.ShouldBe(emailId);
            digitalSession.BlastID.ShouldBe(blastId);
            digitalSession.Totalpages.ShouldBe(totalpages);
            digitalSession.IsLoginRequired.ShouldBe(isLoginRequired);
            digitalSession.IsAuthenticated.ShouldBeFalse();
            digitalSession.Publicationtype.ShouldBe(publicationtype);
            digitalSession.ImagePath.ShouldBe(imagePath);
        }

        #endregion

        #endregion

        #endregion
    }
}