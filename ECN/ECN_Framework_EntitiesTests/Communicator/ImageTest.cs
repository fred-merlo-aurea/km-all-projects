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
    public class ImageTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (Image) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Image_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var image = Fixture.Create<Image>();
            var folderName = Fixture.Create<string>();
            var folderId = Fixture.Create<int>();
            var imageId = Fixture.Create<int>();
            var imageName = Fixture.Create<string>();
            var imageData = Fixture.Create<byte[]>();
            var imageURL = Fixture.Create<string>();

            // Act
            image.FolderName = folderName;
            image.FolderID = folderId;
            image.ImageID = imageId;
            image.ImageName = imageName;
            image.ImageData = imageData;
            image.ImageURL = imageURL;

            // Assert
            image.FolderName.ShouldBe(folderName);
            image.FolderID.ShouldBe(folderId);
            image.ImageID.ShouldBe(imageId);
            image.ImageName.ShouldBe(imageName);
            image.ImageData.ShouldBe(imageData);
            image.ImageURL.ShouldBe(imageURL);
        }

        #endregion

        #region General Getters/Setters : Class (Image) => Property (FolderID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Image_FolderID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var image = Fixture.Create<Image>();
            image.FolderID = Fixture.Create<int>();
            var intType = image.FolderID.GetType();

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

        #region General Getters/Setters : Class (Image) => Property (FolderID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Image_Class_Invalid_Property_FolderIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFolderID = "FolderIDNotPresent";
            var image  = Fixture.Create<Image>();

            // Act , Assert
            Should.NotThrow(() => image.GetType().GetProperty(propertyNameFolderID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Image_FolderID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFolderID = "FolderID";
            var image  = Fixture.Create<Image>();
            var propertyInfo  = image.GetType().GetProperty(propertyNameFolderID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Image) => Property (FolderName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Image_FolderName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var image = Fixture.Create<Image>();
            image.FolderName = Fixture.Create<string>();
            var stringType = image.FolderName.GetType();

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

        #region General Getters/Setters : Class (Image) => Property (FolderName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Image_Class_Invalid_Property_FolderNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFolderName = "FolderNameNotPresent";
            var image  = Fixture.Create<Image>();

            // Act , Assert
            Should.NotThrow(() => image.GetType().GetProperty(propertyNameFolderName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Image_FolderName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFolderName = "FolderName";
            var image  = Fixture.Create<Image>();
            var propertyInfo  = image.GetType().GetProperty(propertyNameFolderName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Image) => Property (ImageData) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Image_ImageData_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameImageData = "ImageData";
            var image = Fixture.Create<Image>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = image.GetType().GetProperty(propertyNameImageData);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(image, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Image) => Property (ImageData) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Image_Class_Invalid_Property_ImageDataNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameImageData = "ImageDataNotPresent";
            var image  = Fixture.Create<Image>();

            // Act , Assert
            Should.NotThrow(() => image.GetType().GetProperty(propertyNameImageData));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Image_ImageData_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameImageData = "ImageData";
            var image  = Fixture.Create<Image>();
            var propertyInfo  = image.GetType().GetProperty(propertyNameImageData);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Image) => Property (ImageID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Image_ImageID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var image = Fixture.Create<Image>();
            image.ImageID = Fixture.Create<int>();
            var intType = image.ImageID.GetType();

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

        #region General Getters/Setters : Class (Image) => Property (ImageID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Image_Class_Invalid_Property_ImageIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameImageID = "ImageIDNotPresent";
            var image  = Fixture.Create<Image>();

            // Act , Assert
            Should.NotThrow(() => image.GetType().GetProperty(propertyNameImageID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Image_ImageID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameImageID = "ImageID";
            var image  = Fixture.Create<Image>();
            var propertyInfo  = image.GetType().GetProperty(propertyNameImageID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Image) => Property (ImageName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Image_ImageName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var image = Fixture.Create<Image>();
            image.ImageName = Fixture.Create<string>();
            var stringType = image.ImageName.GetType();

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

        #region General Getters/Setters : Class (Image) => Property (ImageName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Image_Class_Invalid_Property_ImageNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameImageName = "ImageNameNotPresent";
            var image  = Fixture.Create<Image>();

            // Act , Assert
            Should.NotThrow(() => image.GetType().GetProperty(propertyNameImageName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Image_ImageName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameImageName = "ImageName";
            var image  = Fixture.Create<Image>();
            var propertyInfo  = image.GetType().GetProperty(propertyNameImageName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Image) => Property (ImageURL) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Image_ImageURL_Property_String_Type_Verify_Test()
        {
            // Arrange
            var image = Fixture.Create<Image>();
            image.ImageURL = Fixture.Create<string>();
            var stringType = image.ImageURL.GetType();

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

        #region General Getters/Setters : Class (Image) => Property (ImageURL) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Image_Class_Invalid_Property_ImageURLNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameImageURL = "ImageURLNotPresent";
            var image  = Fixture.Create<Image>();

            // Act , Assert
            Should.NotThrow(() => image.GetType().GetProperty(propertyNameImageURL));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Image_ImageURL_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameImageURL = "ImageURL";
            var image  = Fixture.Create<Image>();
            var propertyInfo  = image.GetType().GetProperty(propertyNameImageURL);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (Image) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Image_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new Image());
        }

        #endregion

        #region General Constructor : Class (Image) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Image_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfImage = Fixture.CreateMany<Image>(2).ToList();
            var firstImage = instancesOfImage.FirstOrDefault();
            var lastImage = instancesOfImage.Last();

            // Act, Assert
            firstImage.ShouldNotBeNull();
            lastImage.ShouldNotBeNull();
            firstImage.ShouldNotBeSameAs(lastImage);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Image_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstImage = new Image();
            var secondImage = new Image();
            var thirdImage = new Image();
            var fourthImage = new Image();
            var fifthImage = new Image();
            var sixthImage = new Image();

            // Act, Assert
            firstImage.ShouldNotBeNull();
            secondImage.ShouldNotBeNull();
            thirdImage.ShouldNotBeNull();
            fourthImage.ShouldNotBeNull();
            fifthImage.ShouldNotBeNull();
            sixthImage.ShouldNotBeNull();
            firstImage.ShouldNotBeSameAs(secondImage);
            thirdImage.ShouldNotBeSameAs(firstImage);
            fourthImage.ShouldNotBeSameAs(firstImage);
            fifthImage.ShouldNotBeSameAs(firstImage);
            sixthImage.ShouldNotBeSameAs(firstImage);
            sixthImage.ShouldNotBeSameAs(fourthImage);
        }

        #endregion

        #region General Constructor : Class (Image) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Image_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var imageName = string.Empty;
            var folderName = string.Empty;
            var imageURL = string.Empty;
            var folderId = -1;
            var imageId = 0;

            // Act
            var image = new Image();

            // Assert
            image.ImageName.ShouldBe(imageName);
            image.FolderName.ShouldBe(folderName);
            image.ImageURL.ShouldBe(imageURL);
            image.FolderID.ShouldBe(folderId);
            image.ImageID.ShouldBe(imageId);
            image.ImageData.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}