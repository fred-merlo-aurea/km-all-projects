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
    public class SimpleShareDetailTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (SimpleShareDetail) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var simpleShareDetail = Fixture.Create<SimpleShareDetail>();
            var simpleShareDetailId = Fixture.Create<int>();
            var socialMediaId = Fixture.Create<int>();
            var socialMediaAuthId = Fixture.Create<int>();
            var title = Fixture.Create<string>();
            var subTitle = Fixture.Create<string>();
            var imagePath = Fixture.Create<string>();
            var content = Fixture.Create<string>();
            var campaignItemId = Fixture.Create<int>();
            var useThumbnail = Fixture.Create<bool?>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var isDeleted = Fixture.Create<bool?>();
            var pageAccessToken = Fixture.Create<string>();
            var pageId = Fixture.Create<string>();

            // Act
            simpleShareDetail.SimpleShareDetailID = simpleShareDetailId;
            simpleShareDetail.SocialMediaID = socialMediaId;
            simpleShareDetail.SocialMediaAuthID = socialMediaAuthId;
            simpleShareDetail.Title = title;
            simpleShareDetail.SubTitle = subTitle;
            simpleShareDetail.ImagePath = imagePath;
            simpleShareDetail.Content = content;
            simpleShareDetail.CampaignItemID = campaignItemId;
            simpleShareDetail.UseThumbnail = useThumbnail;
            simpleShareDetail.CreatedUserID = createdUserId;
            simpleShareDetail.CreatedDate = createdDate;
            simpleShareDetail.UpdatedUserID = updatedUserId;
            simpleShareDetail.UpdatedDate = updatedDate;
            simpleShareDetail.IsDeleted = isDeleted;
            simpleShareDetail.PageAccessToken = pageAccessToken;
            simpleShareDetail.PageID = pageId;

            // Assert
            simpleShareDetail.SimpleShareDetailID.ShouldBe(simpleShareDetailId);
            simpleShareDetail.SocialMediaID.ShouldBe(socialMediaId);
            simpleShareDetail.SocialMediaAuthID.ShouldBe(socialMediaAuthId);
            simpleShareDetail.Title.ShouldBe(title);
            simpleShareDetail.SubTitle.ShouldBe(subTitle);
            simpleShareDetail.ImagePath.ShouldBe(imagePath);
            simpleShareDetail.Content.ShouldBe(content);
            simpleShareDetail.CampaignItemID.ShouldBe(campaignItemId);
            simpleShareDetail.UseThumbnail.ShouldBe(useThumbnail);
            simpleShareDetail.CreatedUserID.ShouldBe(createdUserId);
            simpleShareDetail.CreatedDate.ShouldBe(createdDate);
            simpleShareDetail.UpdatedUserID.ShouldBe(updatedUserId);
            simpleShareDetail.UpdatedDate.ShouldBe(updatedDate);
            simpleShareDetail.IsDeleted.ShouldBe(isDeleted);
            simpleShareDetail.PageAccessToken.ShouldBe(pageAccessToken);
            simpleShareDetail.PageID.ShouldBe(pageId);
        }

        #endregion

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (CampaignItemID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_CampaignItemID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var simpleShareDetail = Fixture.Create<SimpleShareDetail>();
            simpleShareDetail.CampaignItemID = Fixture.Create<int>();
            var intType = simpleShareDetail.CampaignItemID.GetType();

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

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (CampaignItemID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_Class_Invalid_Property_CampaignItemIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemID = "CampaignItemIDNotPresent";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();

            // Act , Assert
            Should.NotThrow(() => simpleShareDetail.GetType().GetProperty(propertyNameCampaignItemID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_CampaignItemID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemID = "CampaignItemID";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();
            var propertyInfo  = simpleShareDetail.GetType().GetProperty(propertyNameCampaignItemID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (Content) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_Content_Property_String_Type_Verify_Test()
        {
            // Arrange
            var simpleShareDetail = Fixture.Create<SimpleShareDetail>();
            simpleShareDetail.Content = Fixture.Create<string>();
            var stringType = simpleShareDetail.Content.GetType();

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

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (Content) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_Class_Invalid_Property_ContentNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContent = "ContentNotPresent";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();

            // Act , Assert
            Should.NotThrow(() => simpleShareDetail.GetType().GetProperty(propertyNameContent));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_Content_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContent = "Content";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();
            var propertyInfo  = simpleShareDetail.GetType().GetProperty(propertyNameContent);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var simpleShareDetail = Fixture.Create<SimpleShareDetail>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = simpleShareDetail.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(simpleShareDetail, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();

            // Act , Assert
            Should.NotThrow(() => simpleShareDetail.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();
            var propertyInfo  = simpleShareDetail.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var simpleShareDetail = Fixture.Create<SimpleShareDetail>();
            var random = Fixture.Create<int>();

            // Act , Set
            simpleShareDetail.CreatedUserID = random;

            // Assert
            simpleShareDetail.CreatedUserID.ShouldBe(random);
            simpleShareDetail.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var simpleShareDetail = Fixture.Create<SimpleShareDetail>();

            // Act , Set
            simpleShareDetail.CreatedUserID = null;

            // Assert
            simpleShareDetail.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var simpleShareDetail = Fixture.Create<SimpleShareDetail>();
            var propertyInfo = simpleShareDetail.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(simpleShareDetail, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            simpleShareDetail.CreatedUserID.ShouldBeNull();
            simpleShareDetail.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();

            // Act , Assert
            Should.NotThrow(() => simpleShareDetail.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();
            var propertyInfo  = simpleShareDetail.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (ImagePath) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_ImagePath_Property_String_Type_Verify_Test()
        {
            // Arrange
            var simpleShareDetail = Fixture.Create<SimpleShareDetail>();
            simpleShareDetail.ImagePath = Fixture.Create<string>();
            var stringType = simpleShareDetail.ImagePath.GetType();

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

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (ImagePath) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_Class_Invalid_Property_ImagePathNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameImagePath = "ImagePathNotPresent";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();

            // Act , Assert
            Should.NotThrow(() => simpleShareDetail.GetType().GetProperty(propertyNameImagePath));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_ImagePath_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameImagePath = "ImagePath";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();
            var propertyInfo  = simpleShareDetail.GetType().GetProperty(propertyNameImagePath);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var simpleShareDetail = Fixture.Create<SimpleShareDetail>();
            var random = Fixture.Create<bool>();

            // Act , Set
            simpleShareDetail.IsDeleted = random;

            // Assert
            simpleShareDetail.IsDeleted.ShouldBe(random);
            simpleShareDetail.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var simpleShareDetail = Fixture.Create<SimpleShareDetail>();

            // Act , Set
            simpleShareDetail.IsDeleted = null;

            // Assert
            simpleShareDetail.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var simpleShareDetail = Fixture.Create<SimpleShareDetail>();
            var propertyInfo = simpleShareDetail.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(simpleShareDetail, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            simpleShareDetail.IsDeleted.ShouldBeNull();
            simpleShareDetail.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();

            // Act , Assert
            Should.NotThrow(() => simpleShareDetail.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();
            var propertyInfo  = simpleShareDetail.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (PageAccessToken) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_PageAccessToken_Property_String_Type_Verify_Test()
        {
            // Arrange
            var simpleShareDetail = Fixture.Create<SimpleShareDetail>();
            simpleShareDetail.PageAccessToken = Fixture.Create<string>();
            var stringType = simpleShareDetail.PageAccessToken.GetType();

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

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (PageAccessToken) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_Class_Invalid_Property_PageAccessTokenNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePageAccessToken = "PageAccessTokenNotPresent";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();

            // Act , Assert
            Should.NotThrow(() => simpleShareDetail.GetType().GetProperty(propertyNamePageAccessToken));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_PageAccessToken_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePageAccessToken = "PageAccessToken";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();
            var propertyInfo  = simpleShareDetail.GetType().GetProperty(propertyNamePageAccessToken);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (PageID) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_PageID_Property_String_Type_Verify_Test()
        {
            // Arrange
            var simpleShareDetail = Fixture.Create<SimpleShareDetail>();
            simpleShareDetail.PageID = Fixture.Create<string>();
            var stringType = simpleShareDetail.PageID.GetType();

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

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (PageID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_Class_Invalid_Property_PageIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePageID = "PageIDNotPresent";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();

            // Act , Assert
            Should.NotThrow(() => simpleShareDetail.GetType().GetProperty(propertyNamePageID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_PageID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePageID = "PageID";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();
            var propertyInfo  = simpleShareDetail.GetType().GetProperty(propertyNamePageID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (SimpleShareDetailID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_SimpleShareDetailID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var simpleShareDetail = Fixture.Create<SimpleShareDetail>();
            simpleShareDetail.SimpleShareDetailID = Fixture.Create<int>();
            var intType = simpleShareDetail.SimpleShareDetailID.GetType();

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

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (SimpleShareDetailID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_Class_Invalid_Property_SimpleShareDetailIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSimpleShareDetailID = "SimpleShareDetailIDNotPresent";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();

            // Act , Assert
            Should.NotThrow(() => simpleShareDetail.GetType().GetProperty(propertyNameSimpleShareDetailID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_SimpleShareDetailID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSimpleShareDetailID = "SimpleShareDetailID";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();
            var propertyInfo  = simpleShareDetail.GetType().GetProperty(propertyNameSimpleShareDetailID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (SocialMediaAuthID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_SocialMediaAuthID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var simpleShareDetail = Fixture.Create<SimpleShareDetail>();
            simpleShareDetail.SocialMediaAuthID = Fixture.Create<int>();
            var intType = simpleShareDetail.SocialMediaAuthID.GetType();

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

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (SocialMediaAuthID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_Class_Invalid_Property_SocialMediaAuthIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSocialMediaAuthID = "SocialMediaAuthIDNotPresent";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();

            // Act , Assert
            Should.NotThrow(() => simpleShareDetail.GetType().GetProperty(propertyNameSocialMediaAuthID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_SocialMediaAuthID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSocialMediaAuthID = "SocialMediaAuthID";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();
            var propertyInfo  = simpleShareDetail.GetType().GetProperty(propertyNameSocialMediaAuthID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (SocialMediaID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_SocialMediaID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var simpleShareDetail = Fixture.Create<SimpleShareDetail>();
            simpleShareDetail.SocialMediaID = Fixture.Create<int>();
            var intType = simpleShareDetail.SocialMediaID.GetType();

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

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (SocialMediaID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_Class_Invalid_Property_SocialMediaIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSocialMediaID = "SocialMediaIDNotPresent";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();

            // Act , Assert
            Should.NotThrow(() => simpleShareDetail.GetType().GetProperty(propertyNameSocialMediaID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_SocialMediaID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSocialMediaID = "SocialMediaID";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();
            var propertyInfo  = simpleShareDetail.GetType().GetProperty(propertyNameSocialMediaID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (SubTitle) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_SubTitle_Property_String_Type_Verify_Test()
        {
            // Arrange
            var simpleShareDetail = Fixture.Create<SimpleShareDetail>();
            simpleShareDetail.SubTitle = Fixture.Create<string>();
            var stringType = simpleShareDetail.SubTitle.GetType();

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

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (SubTitle) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_Class_Invalid_Property_SubTitleNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSubTitle = "SubTitleNotPresent";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();

            // Act , Assert
            Should.NotThrow(() => simpleShareDetail.GetType().GetProperty(propertyNameSubTitle));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_SubTitle_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSubTitle = "SubTitle";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();
            var propertyInfo  = simpleShareDetail.GetType().GetProperty(propertyNameSubTitle);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (Title) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_Title_Property_String_Type_Verify_Test()
        {
            // Arrange
            var simpleShareDetail = Fixture.Create<SimpleShareDetail>();
            simpleShareDetail.Title = Fixture.Create<string>();
            var stringType = simpleShareDetail.Title.GetType();

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

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (Title) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_Class_Invalid_Property_TitleNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTitle = "TitleNotPresent";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();

            // Act , Assert
            Should.NotThrow(() => simpleShareDetail.GetType().GetProperty(propertyNameTitle));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_Title_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTitle = "Title";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();
            var propertyInfo  = simpleShareDetail.GetType().GetProperty(propertyNameTitle);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var simpleShareDetail = Fixture.Create<SimpleShareDetail>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = simpleShareDetail.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(simpleShareDetail, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();

            // Act , Assert
            Should.NotThrow(() => simpleShareDetail.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();
            var propertyInfo  = simpleShareDetail.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var simpleShareDetail = Fixture.Create<SimpleShareDetail>();
            var random = Fixture.Create<int>();

            // Act , Set
            simpleShareDetail.UpdatedUserID = random;

            // Assert
            simpleShareDetail.UpdatedUserID.ShouldBe(random);
            simpleShareDetail.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var simpleShareDetail = Fixture.Create<SimpleShareDetail>();

            // Act , Set
            simpleShareDetail.UpdatedUserID = null;

            // Assert
            simpleShareDetail.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var simpleShareDetail = Fixture.Create<SimpleShareDetail>();
            var propertyInfo = simpleShareDetail.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(simpleShareDetail, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            simpleShareDetail.UpdatedUserID.ShouldBeNull();
            simpleShareDetail.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();

            // Act , Assert
            Should.NotThrow(() => simpleShareDetail.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();
            var propertyInfo  = simpleShareDetail.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (UseThumbnail) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_UseThumbnail_Property_Data_Without_Null_Test()
        {
            // Arrange
            var simpleShareDetail = Fixture.Create<SimpleShareDetail>();
            var random = Fixture.Create<bool>();

            // Act , Set
            simpleShareDetail.UseThumbnail = random;

            // Assert
            simpleShareDetail.UseThumbnail.ShouldBe(random);
            simpleShareDetail.UseThumbnail.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_UseThumbnail_Property_Only_Null_Data_Test()
        {
            // Arrange
            var simpleShareDetail = Fixture.Create<SimpleShareDetail>();

            // Act , Set
            simpleShareDetail.UseThumbnail = null;

            // Assert
            simpleShareDetail.UseThumbnail.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_UseThumbnail_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUseThumbnail = "UseThumbnail";
            var simpleShareDetail = Fixture.Create<SimpleShareDetail>();
            var propertyInfo = simpleShareDetail.GetType().GetProperty(propertyNameUseThumbnail);

            // Act , Set
            propertyInfo.SetValue(simpleShareDetail, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            simpleShareDetail.UseThumbnail.ShouldBeNull();
            simpleShareDetail.UseThumbnail.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (SimpleShareDetail) => Property (UseThumbnail) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_Class_Invalid_Property_UseThumbnailNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUseThumbnail = "UseThumbnailNotPresent";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();

            // Act , Assert
            Should.NotThrow(() => simpleShareDetail.GetType().GetProperty(propertyNameUseThumbnail));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SimpleShareDetail_UseThumbnail_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUseThumbnail = "UseThumbnail";
            var simpleShareDetail  = Fixture.Create<SimpleShareDetail>();
            var propertyInfo  = simpleShareDetail.GetType().GetProperty(propertyNameUseThumbnail);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (SimpleShareDetail) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_SimpleShareDetail_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new SimpleShareDetail());
        }

        #endregion

        #region General Constructor : Class (SimpleShareDetail) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_SimpleShareDetail_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfSimpleShareDetail = Fixture.CreateMany<SimpleShareDetail>(2).ToList();
            var firstSimpleShareDetail = instancesOfSimpleShareDetail.FirstOrDefault();
            var lastSimpleShareDetail = instancesOfSimpleShareDetail.Last();

            // Act, Assert
            firstSimpleShareDetail.ShouldNotBeNull();
            lastSimpleShareDetail.ShouldNotBeNull();
            firstSimpleShareDetail.ShouldNotBeSameAs(lastSimpleShareDetail);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_SimpleShareDetail_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstSimpleShareDetail = new SimpleShareDetail();
            var secondSimpleShareDetail = new SimpleShareDetail();
            var thirdSimpleShareDetail = new SimpleShareDetail();
            var fourthSimpleShareDetail = new SimpleShareDetail();
            var fifthSimpleShareDetail = new SimpleShareDetail();
            var sixthSimpleShareDetail = new SimpleShareDetail();

            // Act, Assert
            firstSimpleShareDetail.ShouldNotBeNull();
            secondSimpleShareDetail.ShouldNotBeNull();
            thirdSimpleShareDetail.ShouldNotBeNull();
            fourthSimpleShareDetail.ShouldNotBeNull();
            fifthSimpleShareDetail.ShouldNotBeNull();
            sixthSimpleShareDetail.ShouldNotBeNull();
            firstSimpleShareDetail.ShouldNotBeSameAs(secondSimpleShareDetail);
            thirdSimpleShareDetail.ShouldNotBeSameAs(firstSimpleShareDetail);
            fourthSimpleShareDetail.ShouldNotBeSameAs(firstSimpleShareDetail);
            fifthSimpleShareDetail.ShouldNotBeSameAs(firstSimpleShareDetail);
            sixthSimpleShareDetail.ShouldNotBeSameAs(firstSimpleShareDetail);
            sixthSimpleShareDetail.ShouldNotBeSameAs(fourthSimpleShareDetail);
        }

        #endregion

        #region General Constructor : Class (SimpleShareDetail) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_SimpleShareDetail_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var simpleShareDetailId = -1;
            var socialMediaId = -1;
            var socialMediaAuthId = -1;
            var title = string.Empty;
            var subTitle = string.Empty;
            var imagePath = string.Empty;
            var content = string.Empty;
            var campaignItemId = -1;
            var useThumbnail = false;
            var isDeleted = false;
            var pageAccessToken = string.Empty;
            var pageId = string.Empty;

            // Act
            var simpleShareDetail = new SimpleShareDetail();

            // Assert
            simpleShareDetail.SimpleShareDetailID.ShouldBe(simpleShareDetailId);
            simpleShareDetail.SocialMediaID.ShouldBe(socialMediaId);
            simpleShareDetail.SocialMediaAuthID.ShouldBe(socialMediaAuthId);
            simpleShareDetail.Title.ShouldBe(title);
            simpleShareDetail.SubTitle.ShouldBe(subTitle);
            simpleShareDetail.ImagePath.ShouldBe(imagePath);
            simpleShareDetail.Content.ShouldBe(content);
            simpleShareDetail.CampaignItemID.ShouldBe(campaignItemId);
            simpleShareDetail.UseThumbnail.ShouldBe(useThumbnail);
            simpleShareDetail.CreatedUserID.ShouldBeNull();
            simpleShareDetail.CreatedDate.ShouldBeNull();
            simpleShareDetail.UpdatedDate.ShouldBeNull();
            simpleShareDetail.UpdatedUserID.ShouldBeNull();
            simpleShareDetail.IsDeleted.ShouldBe(isDeleted);
            simpleShareDetail.PageAccessToken.ShouldBe(pageAccessToken);
            simpleShareDetail.PageID.ShouldBe(pageId);
        }

        #endregion

        #endregion

        #endregion
    }
}