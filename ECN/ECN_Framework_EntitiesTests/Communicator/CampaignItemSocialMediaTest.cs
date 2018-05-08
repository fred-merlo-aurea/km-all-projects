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
    public class CampaignItemSocialMediaTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (CampaignItemSocialMedia) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var campaignItemSocialMedia = Fixture.Create<CampaignItemSocialMedia>();
            var campaignItemSocialMediaId = Fixture.Create<int>();
            var campaignItemId = Fixture.Create<int>();
            var socialMediaId = Fixture.Create<int>();
            var simpleShareDetailId = Fixture.Create<int?>();
            var socialMediaAuthId = Fixture.Create<int?>();
            var status = Fixture.Create<string>();
            var statusDate = Fixture.Create<DateTime>();
            var pageId = Fixture.Create<string>();
            var pageAccessToken = Fixture.Create<string>();
            var postId = Fixture.Create<string>();
            var lastErrorCode = Fixture.Create<int?>();

            // Act
            campaignItemSocialMedia.CampaignItemSocialMediaID = campaignItemSocialMediaId;
            campaignItemSocialMedia.CampaignItemID = campaignItemId;
            campaignItemSocialMedia.SocialMediaID = socialMediaId;
            campaignItemSocialMedia.SimpleShareDetailID = simpleShareDetailId;
            campaignItemSocialMedia.SocialMediaAuthID = socialMediaAuthId;
            campaignItemSocialMedia.Status = status;
            campaignItemSocialMedia.StatusDate = statusDate;
            campaignItemSocialMedia.PageID = pageId;
            campaignItemSocialMedia.PageAccessToken = pageAccessToken;
            campaignItemSocialMedia.PostID = postId;
            campaignItemSocialMedia.LastErrorCode = lastErrorCode;

            // Assert
            campaignItemSocialMedia.CampaignItemSocialMediaID.ShouldBe(campaignItemSocialMediaId);
            campaignItemSocialMedia.CampaignItemID.ShouldBe(campaignItemId);
            campaignItemSocialMedia.SocialMediaID.ShouldBe(socialMediaId);
            campaignItemSocialMedia.SimpleShareDetailID.ShouldBe(simpleShareDetailId);
            campaignItemSocialMedia.SocialMediaAuthID.ShouldBe(socialMediaAuthId);
            campaignItemSocialMedia.Status.ShouldBe(status);
            campaignItemSocialMedia.StatusDate.ShouldBe(statusDate);
            campaignItemSocialMedia.PageID.ShouldBe(pageId);
            campaignItemSocialMedia.PageAccessToken.ShouldBe(pageAccessToken);
            campaignItemSocialMedia.PostID.ShouldBe(postId);
            campaignItemSocialMedia.LastErrorCode.ShouldBe(lastErrorCode);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSocialMedia) => Property (CampaignItemID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_CampaignItemID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignItemSocialMedia = Fixture.Create<CampaignItemSocialMedia>();
            campaignItemSocialMedia.CampaignItemID = Fixture.Create<int>();
            var intType = campaignItemSocialMedia.CampaignItemID.GetType();

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

        #region General Getters/Setters : Class (CampaignItemSocialMedia) => Property (CampaignItemID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_Class_Invalid_Property_CampaignItemIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemID = "CampaignItemIDNotPresent";
            var campaignItemSocialMedia  = Fixture.Create<CampaignItemSocialMedia>();

            // Act , Assert
            Should.NotThrow(() => campaignItemSocialMedia.GetType().GetProperty(propertyNameCampaignItemID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_CampaignItemID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemID = "CampaignItemID";
            var campaignItemSocialMedia  = Fixture.Create<CampaignItemSocialMedia>();
            var propertyInfo  = campaignItemSocialMedia.GetType().GetProperty(propertyNameCampaignItemID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSocialMedia) => Property (CampaignItemSocialMediaID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_CampaignItemSocialMediaID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignItemSocialMedia = Fixture.Create<CampaignItemSocialMedia>();
            campaignItemSocialMedia.CampaignItemSocialMediaID = Fixture.Create<int>();
            var intType = campaignItemSocialMedia.CampaignItemSocialMediaID.GetType();

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

        #region General Getters/Setters : Class (CampaignItemSocialMedia) => Property (CampaignItemSocialMediaID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_Class_Invalid_Property_CampaignItemSocialMediaIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemSocialMediaID = "CampaignItemSocialMediaIDNotPresent";
            var campaignItemSocialMedia  = Fixture.Create<CampaignItemSocialMedia>();

            // Act , Assert
            Should.NotThrow(() => campaignItemSocialMedia.GetType().GetProperty(propertyNameCampaignItemSocialMediaID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_CampaignItemSocialMediaID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemSocialMediaID = "CampaignItemSocialMediaID";
            var campaignItemSocialMedia  = Fixture.Create<CampaignItemSocialMedia>();
            var propertyInfo  = campaignItemSocialMedia.GetType().GetProperty(propertyNameCampaignItemSocialMediaID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSocialMedia) => Property (LastErrorCode) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_LastErrorCode_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemSocialMedia = Fixture.Create<CampaignItemSocialMedia>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemSocialMedia.LastErrorCode = random;

            // Assert
            campaignItemSocialMedia.LastErrorCode.ShouldBe(random);
            campaignItemSocialMedia.LastErrorCode.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_LastErrorCode_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemSocialMedia = Fixture.Create<CampaignItemSocialMedia>();    

            // Act , Set
            campaignItemSocialMedia.LastErrorCode = null;

            // Assert
            campaignItemSocialMedia.LastErrorCode.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_LastErrorCode_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameLastErrorCode = "LastErrorCode";
            var campaignItemSocialMedia = Fixture.Create<CampaignItemSocialMedia>();
            var propertyInfo = campaignItemSocialMedia.GetType().GetProperty(propertyNameLastErrorCode);

            // Act , Set
            propertyInfo.SetValue(campaignItemSocialMedia, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemSocialMedia.LastErrorCode.ShouldBeNull();
            campaignItemSocialMedia.LastErrorCode.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSocialMedia) => Property (LastErrorCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_Class_Invalid_Property_LastErrorCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLastErrorCode = "LastErrorCodeNotPresent";
            var campaignItemSocialMedia  = Fixture.Create<CampaignItemSocialMedia>();

            // Act , Assert
            Should.NotThrow(() => campaignItemSocialMedia.GetType().GetProperty(propertyNameLastErrorCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_LastErrorCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLastErrorCode = "LastErrorCode";
            var campaignItemSocialMedia  = Fixture.Create<CampaignItemSocialMedia>();
            var propertyInfo  = campaignItemSocialMedia.GetType().GetProperty(propertyNameLastErrorCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSocialMedia) => Property (PageAccessToken) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_PageAccessToken_Property_String_Type_Verify_Test()
        {
            // Arrange
            var campaignItemSocialMedia = Fixture.Create<CampaignItemSocialMedia>();
            campaignItemSocialMedia.PageAccessToken = Fixture.Create<string>();
            var stringType = campaignItemSocialMedia.PageAccessToken.GetType();

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

        #region General Getters/Setters : Class (CampaignItemSocialMedia) => Property (PageAccessToken) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_Class_Invalid_Property_PageAccessTokenNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePageAccessToken = "PageAccessTokenNotPresent";
            var campaignItemSocialMedia  = Fixture.Create<CampaignItemSocialMedia>();

            // Act , Assert
            Should.NotThrow(() => campaignItemSocialMedia.GetType().GetProperty(propertyNamePageAccessToken));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_PageAccessToken_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePageAccessToken = "PageAccessToken";
            var campaignItemSocialMedia  = Fixture.Create<CampaignItemSocialMedia>();
            var propertyInfo  = campaignItemSocialMedia.GetType().GetProperty(propertyNamePageAccessToken);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSocialMedia) => Property (PageID) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_PageID_Property_String_Type_Verify_Test()
        {
            // Arrange
            var campaignItemSocialMedia = Fixture.Create<CampaignItemSocialMedia>();
            campaignItemSocialMedia.PageID = Fixture.Create<string>();
            var stringType = campaignItemSocialMedia.PageID.GetType();

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

        #region General Getters/Setters : Class (CampaignItemSocialMedia) => Property (PageID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_Class_Invalid_Property_PageIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePageID = "PageIDNotPresent";
            var campaignItemSocialMedia  = Fixture.Create<CampaignItemSocialMedia>();

            // Act , Assert
            Should.NotThrow(() => campaignItemSocialMedia.GetType().GetProperty(propertyNamePageID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_PageID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePageID = "PageID";
            var campaignItemSocialMedia  = Fixture.Create<CampaignItemSocialMedia>();
            var propertyInfo  = campaignItemSocialMedia.GetType().GetProperty(propertyNamePageID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSocialMedia) => Property (PostID) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_PostID_Property_String_Type_Verify_Test()
        {
            // Arrange
            var campaignItemSocialMedia = Fixture.Create<CampaignItemSocialMedia>();
            campaignItemSocialMedia.PostID = Fixture.Create<string>();
            var stringType = campaignItemSocialMedia.PostID.GetType();

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

        #region General Getters/Setters : Class (CampaignItemSocialMedia) => Property (PostID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_Class_Invalid_Property_PostIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePostID = "PostIDNotPresent";
            var campaignItemSocialMedia  = Fixture.Create<CampaignItemSocialMedia>();

            // Act , Assert
            Should.NotThrow(() => campaignItemSocialMedia.GetType().GetProperty(propertyNamePostID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_PostID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePostID = "PostID";
            var campaignItemSocialMedia  = Fixture.Create<CampaignItemSocialMedia>();
            var propertyInfo  = campaignItemSocialMedia.GetType().GetProperty(propertyNamePostID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSocialMedia) => Property (SimpleShareDetailID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_SimpleShareDetailID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemSocialMedia = Fixture.Create<CampaignItemSocialMedia>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemSocialMedia.SimpleShareDetailID = random;

            // Assert
            campaignItemSocialMedia.SimpleShareDetailID.ShouldBe(random);
            campaignItemSocialMedia.SimpleShareDetailID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_SimpleShareDetailID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemSocialMedia = Fixture.Create<CampaignItemSocialMedia>();    

            // Act , Set
            campaignItemSocialMedia.SimpleShareDetailID = null;

            // Assert
            campaignItemSocialMedia.SimpleShareDetailID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_SimpleShareDetailID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameSimpleShareDetailID = "SimpleShareDetailID";
            var campaignItemSocialMedia = Fixture.Create<CampaignItemSocialMedia>();
            var propertyInfo = campaignItemSocialMedia.GetType().GetProperty(propertyNameSimpleShareDetailID);

            // Act , Set
            propertyInfo.SetValue(campaignItemSocialMedia, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemSocialMedia.SimpleShareDetailID.ShouldBeNull();
            campaignItemSocialMedia.SimpleShareDetailID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSocialMedia) => Property (SimpleShareDetailID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_Class_Invalid_Property_SimpleShareDetailIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSimpleShareDetailID = "SimpleShareDetailIDNotPresent";
            var campaignItemSocialMedia  = Fixture.Create<CampaignItemSocialMedia>();

            // Act , Assert
            Should.NotThrow(() => campaignItemSocialMedia.GetType().GetProperty(propertyNameSimpleShareDetailID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_SimpleShareDetailID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSimpleShareDetailID = "SimpleShareDetailID";
            var campaignItemSocialMedia  = Fixture.Create<CampaignItemSocialMedia>();
            var propertyInfo  = campaignItemSocialMedia.GetType().GetProperty(propertyNameSimpleShareDetailID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSocialMedia) => Property (SocialMediaAuthID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_SocialMediaAuthID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemSocialMedia = Fixture.Create<CampaignItemSocialMedia>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemSocialMedia.SocialMediaAuthID = random;

            // Assert
            campaignItemSocialMedia.SocialMediaAuthID.ShouldBe(random);
            campaignItemSocialMedia.SocialMediaAuthID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_SocialMediaAuthID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemSocialMedia = Fixture.Create<CampaignItemSocialMedia>();    

            // Act , Set
            campaignItemSocialMedia.SocialMediaAuthID = null;

            // Assert
            campaignItemSocialMedia.SocialMediaAuthID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_SocialMediaAuthID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameSocialMediaAuthID = "SocialMediaAuthID";
            var campaignItemSocialMedia = Fixture.Create<CampaignItemSocialMedia>();
            var propertyInfo = campaignItemSocialMedia.GetType().GetProperty(propertyNameSocialMediaAuthID);

            // Act , Set
            propertyInfo.SetValue(campaignItemSocialMedia, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemSocialMedia.SocialMediaAuthID.ShouldBeNull();
            campaignItemSocialMedia.SocialMediaAuthID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSocialMedia) => Property (SocialMediaAuthID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_Class_Invalid_Property_SocialMediaAuthIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSocialMediaAuthID = "SocialMediaAuthIDNotPresent";
            var campaignItemSocialMedia  = Fixture.Create<CampaignItemSocialMedia>();

            // Act , Assert
            Should.NotThrow(() => campaignItemSocialMedia.GetType().GetProperty(propertyNameSocialMediaAuthID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_SocialMediaAuthID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSocialMediaAuthID = "SocialMediaAuthID";
            var campaignItemSocialMedia  = Fixture.Create<CampaignItemSocialMedia>();
            var propertyInfo  = campaignItemSocialMedia.GetType().GetProperty(propertyNameSocialMediaAuthID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSocialMedia) => Property (SocialMediaID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_SocialMediaID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignItemSocialMedia = Fixture.Create<CampaignItemSocialMedia>();
            campaignItemSocialMedia.SocialMediaID = Fixture.Create<int>();
            var intType = campaignItemSocialMedia.SocialMediaID.GetType();

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

        #region General Getters/Setters : Class (CampaignItemSocialMedia) => Property (SocialMediaID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_Class_Invalid_Property_SocialMediaIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSocialMediaID = "SocialMediaIDNotPresent";
            var campaignItemSocialMedia  = Fixture.Create<CampaignItemSocialMedia>();

            // Act , Assert
            Should.NotThrow(() => campaignItemSocialMedia.GetType().GetProperty(propertyNameSocialMediaID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_SocialMediaID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSocialMediaID = "SocialMediaID";
            var campaignItemSocialMedia  = Fixture.Create<CampaignItemSocialMedia>();
            var propertyInfo  = campaignItemSocialMedia.GetType().GetProperty(propertyNameSocialMediaID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSocialMedia) => Property (Status) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_Status_Property_String_Type_Verify_Test()
        {
            // Arrange
            var campaignItemSocialMedia = Fixture.Create<CampaignItemSocialMedia>();
            campaignItemSocialMedia.Status = Fixture.Create<string>();
            var stringType = campaignItemSocialMedia.Status.GetType();

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

        #region General Getters/Setters : Class (CampaignItemSocialMedia) => Property (Status) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_Class_Invalid_Property_StatusNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameStatus = "StatusNotPresent";
            var campaignItemSocialMedia  = Fixture.Create<CampaignItemSocialMedia>();

            // Act , Assert
            Should.NotThrow(() => campaignItemSocialMedia.GetType().GetProperty(propertyNameStatus));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_Status_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameStatus = "Status";
            var campaignItemSocialMedia  = Fixture.Create<CampaignItemSocialMedia>();
            var propertyInfo  = campaignItemSocialMedia.GetType().GetProperty(propertyNameStatus);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSocialMedia) => Property (StatusDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_StatusDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameStatusDate = "StatusDate";
            var campaignItemSocialMedia = Fixture.Create<CampaignItemSocialMedia>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = campaignItemSocialMedia.GetType().GetProperty(propertyNameStatusDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(campaignItemSocialMedia, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemSocialMedia) => Property (StatusDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_Class_Invalid_Property_StatusDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameStatusDate = "StatusDateNotPresent";
            var campaignItemSocialMedia  = Fixture.Create<CampaignItemSocialMedia>();

            // Act , Assert
            Should.NotThrow(() => campaignItemSocialMedia.GetType().GetProperty(propertyNameStatusDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemSocialMedia_StatusDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameStatusDate = "StatusDate";
            var campaignItemSocialMedia  = Fixture.Create<CampaignItemSocialMedia>();
            var propertyInfo  = campaignItemSocialMedia.GetType().GetProperty(propertyNameStatusDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (CampaignItemSocialMedia) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemSocialMedia_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new CampaignItemSocialMedia());
        }

        #endregion

        #region General Constructor : Class (CampaignItemSocialMedia) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemSocialMedia_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfCampaignItemSocialMedia = Fixture.CreateMany<CampaignItemSocialMedia>(2).ToList();
            var firstCampaignItemSocialMedia = instancesOfCampaignItemSocialMedia.FirstOrDefault();
            var lastCampaignItemSocialMedia = instancesOfCampaignItemSocialMedia.Last();

            // Act, Assert
            firstCampaignItemSocialMedia.ShouldNotBeNull();
            lastCampaignItemSocialMedia.ShouldNotBeNull();
            firstCampaignItemSocialMedia.ShouldNotBeSameAs(lastCampaignItemSocialMedia);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemSocialMedia_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstCampaignItemSocialMedia = new CampaignItemSocialMedia();
            var secondCampaignItemSocialMedia = new CampaignItemSocialMedia();
            var thirdCampaignItemSocialMedia = new CampaignItemSocialMedia();
            var fourthCampaignItemSocialMedia = new CampaignItemSocialMedia();
            var fifthCampaignItemSocialMedia = new CampaignItemSocialMedia();
            var sixthCampaignItemSocialMedia = new CampaignItemSocialMedia();

            // Act, Assert
            firstCampaignItemSocialMedia.ShouldNotBeNull();
            secondCampaignItemSocialMedia.ShouldNotBeNull();
            thirdCampaignItemSocialMedia.ShouldNotBeNull();
            fourthCampaignItemSocialMedia.ShouldNotBeNull();
            fifthCampaignItemSocialMedia.ShouldNotBeNull();
            sixthCampaignItemSocialMedia.ShouldNotBeNull();
            firstCampaignItemSocialMedia.ShouldNotBeSameAs(secondCampaignItemSocialMedia);
            thirdCampaignItemSocialMedia.ShouldNotBeSameAs(firstCampaignItemSocialMedia);
            fourthCampaignItemSocialMedia.ShouldNotBeSameAs(firstCampaignItemSocialMedia);
            fifthCampaignItemSocialMedia.ShouldNotBeSameAs(firstCampaignItemSocialMedia);
            sixthCampaignItemSocialMedia.ShouldNotBeSameAs(firstCampaignItemSocialMedia);
            sixthCampaignItemSocialMedia.ShouldNotBeSameAs(fourthCampaignItemSocialMedia);
        }

        #endregion

        #endregion

        #endregion
    }
}