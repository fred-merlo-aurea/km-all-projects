using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator
{
    [TestFixture]
    public class ImageFolderTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (ImageFolder) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ImageFolder_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var imageFolder = Fixture.Create<ImageFolder>();
            var folderId = Fixture.Create<int>();
            var folderName = Fixture.Create<string>();
            var folderFullName = Fixture.Create<string>();
            var folderUrl = Fixture.Create<string>();

            // Act
            imageFolder.FolderID = folderId;
            imageFolder.FolderName = folderName;
            imageFolder.FolderFullName = folderFullName;
            imageFolder.FolderUrl = folderUrl;

            // Assert
            imageFolder.FolderID.ShouldBe(folderId);
            imageFolder.FolderName.ShouldBe(folderName);
            imageFolder.FolderFullName.ShouldBe(folderFullName);
            imageFolder.FolderUrl.ShouldBe(folderUrl);
        }

        #endregion

        #region General Getters/Setters : Class (ImageFolder) => Property (FolderFullName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ImageFolder_FolderFullName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var imageFolder = Fixture.Create<ImageFolder>();
            imageFolder.FolderFullName = Fixture.Create<string>();
            var stringType = imageFolder.FolderFullName.GetType();

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

        #region General Getters/Setters : Class (ImageFolder) => Property (FolderFullName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ImageFolder_Class_Invalid_Property_FolderFullNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFolderFullName = "FolderFullNameNotPresent";
            var imageFolder  = Fixture.Create<ImageFolder>();

            // Act , Assert
            Should.NotThrow(() => imageFolder.GetType().GetProperty(propertyNameFolderFullName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ImageFolder_FolderFullName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFolderFullName = "FolderFullName";
            var imageFolder  = Fixture.Create<ImageFolder>();
            var propertyInfo  = imageFolder.GetType().GetProperty(propertyNameFolderFullName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ImageFolder) => Property (FolderID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ImageFolder_FolderID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var imageFolder = Fixture.Create<ImageFolder>();
            imageFolder.FolderID = Fixture.Create<int>();
            var intType = imageFolder.FolderID.GetType();

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

        #region General Getters/Setters : Class (ImageFolder) => Property (FolderID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ImageFolder_Class_Invalid_Property_FolderIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFolderID = "FolderIDNotPresent";
            var imageFolder  = Fixture.Create<ImageFolder>();

            // Act , Assert
            Should.NotThrow(() => imageFolder.GetType().GetProperty(propertyNameFolderID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ImageFolder_FolderID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFolderID = "FolderID";
            var imageFolder  = Fixture.Create<ImageFolder>();
            var propertyInfo  = imageFolder.GetType().GetProperty(propertyNameFolderID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ImageFolder) => Property (FolderName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ImageFolder_FolderName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var imageFolder = Fixture.Create<ImageFolder>();
            imageFolder.FolderName = Fixture.Create<string>();
            var stringType = imageFolder.FolderName.GetType();

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

        #region General Getters/Setters : Class (ImageFolder) => Property (FolderName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ImageFolder_Class_Invalid_Property_FolderNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFolderName = "FolderNameNotPresent";
            var imageFolder  = Fixture.Create<ImageFolder>();

            // Act , Assert
            Should.NotThrow(() => imageFolder.GetType().GetProperty(propertyNameFolderName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ImageFolder_FolderName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFolderName = "FolderName";
            var imageFolder  = Fixture.Create<ImageFolder>();
            var propertyInfo  = imageFolder.GetType().GetProperty(propertyNameFolderName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ImageFolder) => Property (FolderUrl) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ImageFolder_FolderUrl_Property_String_Type_Verify_Test()
        {
            // Arrange
            var imageFolder = Fixture.Create<ImageFolder>();
            imageFolder.FolderUrl = Fixture.Create<string>();
            var stringType = imageFolder.FolderUrl.GetType();

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

        #region General Getters/Setters : Class (ImageFolder) => Property (FolderUrl) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ImageFolder_Class_Invalid_Property_FolderUrlNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFolderUrl = "FolderUrlNotPresent";
            var imageFolder  = Fixture.Create<ImageFolder>();

            // Act , Assert
            Should.NotThrow(() => imageFolder.GetType().GetProperty(propertyNameFolderUrl));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ImageFolder_FolderUrl_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFolderUrl = "FolderUrl";
            var imageFolder  = Fixture.Create<ImageFolder>();
            var propertyInfo  = imageFolder.GetType().GetProperty(propertyNameFolderUrl);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #endregion
    }
}