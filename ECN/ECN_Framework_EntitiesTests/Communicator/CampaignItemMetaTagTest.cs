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
    public class CampaignItemMetaTagTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (CampaignItemMetaTag) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var campaignItemMetaTag = Fixture.Create<CampaignItemMetaTag>();
            var campaignItemMetaTagId = Fixture.Create<int>();
            var campaignItemId = Fixture.Create<int>();
            var socialMediaId = Fixture.Create<int>();
            var property = Fixture.Create<string>();
            var content = Fixture.Create<string>();
            var isDeleted = Fixture.Create<bool>();
            var createdDate = Fixture.Create<DateTime?>();
            var createdUserId = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();

            // Act
            campaignItemMetaTag.CampaignItemMetaTagID = campaignItemMetaTagId;
            campaignItemMetaTag.CampaignItemID = campaignItemId;
            campaignItemMetaTag.SocialMediaID = socialMediaId;
            campaignItemMetaTag.Property = property;
            campaignItemMetaTag.Content = content;
            campaignItemMetaTag.IsDeleted = isDeleted;
            campaignItemMetaTag.CreatedDate = createdDate;
            campaignItemMetaTag.CreatedUserID = createdUserId;
            campaignItemMetaTag.UpdatedDate = updatedDate;
            campaignItemMetaTag.UpdatedUserID = updatedUserId;

            // Assert
            campaignItemMetaTag.CampaignItemMetaTagID.ShouldBe(campaignItemMetaTagId);
            campaignItemMetaTag.CampaignItemID.ShouldBe(campaignItemId);
            campaignItemMetaTag.SocialMediaID.ShouldBe(socialMediaId);
            campaignItemMetaTag.Property.ShouldBe(property);
            campaignItemMetaTag.Content.ShouldBe(content);
            campaignItemMetaTag.IsDeleted.ShouldBe(isDeleted);
            campaignItemMetaTag.CreatedDate.ShouldBe(createdDate);
            campaignItemMetaTag.CreatedUserID.ShouldBe(createdUserId);
            campaignItemMetaTag.UpdatedDate.ShouldBe(updatedDate);
            campaignItemMetaTag.UpdatedUserID.ShouldBe(updatedUserId);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemMetaTag) => Property (CampaignItemID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_CampaignItemID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignItemMetaTag = Fixture.Create<CampaignItemMetaTag>();
            campaignItemMetaTag.CampaignItemID = Fixture.Create<int>();
            var intType = campaignItemMetaTag.CampaignItemID.GetType();

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

        #region General Getters/Setters : Class (CampaignItemMetaTag) => Property (CampaignItemID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_Class_Invalid_Property_CampaignItemIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemID = "CampaignItemIDNotPresent";
            var campaignItemMetaTag  = Fixture.Create<CampaignItemMetaTag>();

            // Act , Assert
            Should.NotThrow(() => campaignItemMetaTag.GetType().GetProperty(propertyNameCampaignItemID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_CampaignItemID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemID = "CampaignItemID";
            var campaignItemMetaTag  = Fixture.Create<CampaignItemMetaTag>();
            var propertyInfo  = campaignItemMetaTag.GetType().GetProperty(propertyNameCampaignItemID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemMetaTag) => Property (CampaignItemMetaTagID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_CampaignItemMetaTagID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignItemMetaTag = Fixture.Create<CampaignItemMetaTag>();
            campaignItemMetaTag.CampaignItemMetaTagID = Fixture.Create<int>();
            var intType = campaignItemMetaTag.CampaignItemMetaTagID.GetType();

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

        #region General Getters/Setters : Class (CampaignItemMetaTag) => Property (CampaignItemMetaTagID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_Class_Invalid_Property_CampaignItemMetaTagIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemMetaTagID = "CampaignItemMetaTagIDNotPresent";
            var campaignItemMetaTag  = Fixture.Create<CampaignItemMetaTag>();

            // Act , Assert
            Should.NotThrow(() => campaignItemMetaTag.GetType().GetProperty(propertyNameCampaignItemMetaTagID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_CampaignItemMetaTagID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemMetaTagID = "CampaignItemMetaTagID";
            var campaignItemMetaTag  = Fixture.Create<CampaignItemMetaTag>();
            var propertyInfo  = campaignItemMetaTag.GetType().GetProperty(propertyNameCampaignItemMetaTagID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemMetaTag) => Property (Content) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_Content_Property_String_Type_Verify_Test()
        {
            // Arrange
            var campaignItemMetaTag = Fixture.Create<CampaignItemMetaTag>();
            campaignItemMetaTag.Content = Fixture.Create<string>();
            var stringType = campaignItemMetaTag.Content.GetType();

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

        #region General Getters/Setters : Class (CampaignItemMetaTag) => Property (Content) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_Class_Invalid_Property_ContentNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContent = "ContentNotPresent";
            var campaignItemMetaTag  = Fixture.Create<CampaignItemMetaTag>();

            // Act , Assert
            Should.NotThrow(() => campaignItemMetaTag.GetType().GetProperty(propertyNameContent));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_Content_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContent = "Content";
            var campaignItemMetaTag  = Fixture.Create<CampaignItemMetaTag>();
            var propertyInfo  = campaignItemMetaTag.GetType().GetProperty(propertyNameContent);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemMetaTag) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var campaignItemMetaTag = Fixture.Create<CampaignItemMetaTag>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = campaignItemMetaTag.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(campaignItemMetaTag, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemMetaTag) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var campaignItemMetaTag  = Fixture.Create<CampaignItemMetaTag>();

            // Act , Assert
            Should.NotThrow(() => campaignItemMetaTag.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var campaignItemMetaTag  = Fixture.Create<CampaignItemMetaTag>();
            var propertyInfo  = campaignItemMetaTag.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemMetaTag) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemMetaTag = Fixture.Create<CampaignItemMetaTag>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemMetaTag.CreatedUserID = random;

            // Assert
            campaignItemMetaTag.CreatedUserID.ShouldBe(random);
            campaignItemMetaTag.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemMetaTag = Fixture.Create<CampaignItemMetaTag>();    

            // Act , Set
            campaignItemMetaTag.CreatedUserID = null;

            // Assert
            campaignItemMetaTag.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var campaignItemMetaTag = Fixture.Create<CampaignItemMetaTag>();
            var propertyInfo = campaignItemMetaTag.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(campaignItemMetaTag, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemMetaTag.CreatedUserID.ShouldBeNull();
            campaignItemMetaTag.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemMetaTag) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var campaignItemMetaTag  = Fixture.Create<CampaignItemMetaTag>();

            // Act , Assert
            Should.NotThrow(() => campaignItemMetaTag.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var campaignItemMetaTag  = Fixture.Create<CampaignItemMetaTag>();
            var propertyInfo  = campaignItemMetaTag.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemMetaTag) => Property (IsDeleted) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_IsDeleted_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var campaignItemMetaTag = Fixture.Create<CampaignItemMetaTag>();
            campaignItemMetaTag.IsDeleted = Fixture.Create<bool>();
            var boolType = campaignItemMetaTag.IsDeleted.GetType();

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

        #region General Getters/Setters : Class (CampaignItemMetaTag) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var campaignItemMetaTag  = Fixture.Create<CampaignItemMetaTag>();

            // Act , Assert
            Should.NotThrow(() => campaignItemMetaTag.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var campaignItemMetaTag  = Fixture.Create<CampaignItemMetaTag>();
            var propertyInfo  = campaignItemMetaTag.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemMetaTag) => Property (Property) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_Property_Property_String_Type_Verify_Test()
        {
            // Arrange
            var campaignItemMetaTag = Fixture.Create<CampaignItemMetaTag>();
            campaignItemMetaTag.Property = Fixture.Create<string>();
            var stringType = campaignItemMetaTag.Property.GetType();

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

        #region General Getters/Setters : Class (CampaignItemMetaTag) => Property (Property) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_Class_Invalid_Property_PropertyNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameProperty = "PropertyNotPresent";
            var campaignItemMetaTag  = Fixture.Create<CampaignItemMetaTag>();

            // Act , Assert
            Should.NotThrow(() => campaignItemMetaTag.GetType().GetProperty(propertyNameProperty));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_Property_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameProperty = "Property";
            var campaignItemMetaTag  = Fixture.Create<CampaignItemMetaTag>();
            var propertyInfo  = campaignItemMetaTag.GetType().GetProperty(propertyNameProperty);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemMetaTag) => Property (SocialMediaID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_SocialMediaID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignItemMetaTag = Fixture.Create<CampaignItemMetaTag>();
            campaignItemMetaTag.SocialMediaID = Fixture.Create<int>();
            var intType = campaignItemMetaTag.SocialMediaID.GetType();

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

        #region General Getters/Setters : Class (CampaignItemMetaTag) => Property (SocialMediaID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_Class_Invalid_Property_SocialMediaIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSocialMediaID = "SocialMediaIDNotPresent";
            var campaignItemMetaTag  = Fixture.Create<CampaignItemMetaTag>();

            // Act , Assert
            Should.NotThrow(() => campaignItemMetaTag.GetType().GetProperty(propertyNameSocialMediaID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_SocialMediaID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSocialMediaID = "SocialMediaID";
            var campaignItemMetaTag  = Fixture.Create<CampaignItemMetaTag>();
            var propertyInfo  = campaignItemMetaTag.GetType().GetProperty(propertyNameSocialMediaID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemMetaTag) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var campaignItemMetaTag = Fixture.Create<CampaignItemMetaTag>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = campaignItemMetaTag.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(campaignItemMetaTag, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemMetaTag) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var campaignItemMetaTag  = Fixture.Create<CampaignItemMetaTag>();

            // Act , Assert
            Should.NotThrow(() => campaignItemMetaTag.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var campaignItemMetaTag  = Fixture.Create<CampaignItemMetaTag>();
            var propertyInfo  = campaignItemMetaTag.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemMetaTag) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemMetaTag = Fixture.Create<CampaignItemMetaTag>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemMetaTag.UpdatedUserID = random;

            // Assert
            campaignItemMetaTag.UpdatedUserID.ShouldBe(random);
            campaignItemMetaTag.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemMetaTag = Fixture.Create<CampaignItemMetaTag>();    

            // Act , Set
            campaignItemMetaTag.UpdatedUserID = null;

            // Assert
            campaignItemMetaTag.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var campaignItemMetaTag = Fixture.Create<CampaignItemMetaTag>();
            var propertyInfo = campaignItemMetaTag.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(campaignItemMetaTag, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemMetaTag.UpdatedUserID.ShouldBeNull();
            campaignItemMetaTag.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemMetaTag) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var campaignItemMetaTag  = Fixture.Create<CampaignItemMetaTag>();

            // Act , Assert
            Should.NotThrow(() => campaignItemMetaTag.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemMetaTag_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var campaignItemMetaTag  = Fixture.Create<CampaignItemMetaTag>();
            var propertyInfo  = campaignItemMetaTag.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (CampaignItemMetaTag) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemMetaTag_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new CampaignItemMetaTag());
        }

        #endregion

        #region General Constructor : Class (CampaignItemMetaTag) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemMetaTag_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfCampaignItemMetaTag = Fixture.CreateMany<CampaignItemMetaTag>(2).ToList();
            var firstCampaignItemMetaTag = instancesOfCampaignItemMetaTag.FirstOrDefault();
            var lastCampaignItemMetaTag = instancesOfCampaignItemMetaTag.Last();

            // Act, Assert
            firstCampaignItemMetaTag.ShouldNotBeNull();
            lastCampaignItemMetaTag.ShouldNotBeNull();
            firstCampaignItemMetaTag.ShouldNotBeSameAs(lastCampaignItemMetaTag);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemMetaTag_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstCampaignItemMetaTag = new CampaignItemMetaTag();
            var secondCampaignItemMetaTag = new CampaignItemMetaTag();
            var thirdCampaignItemMetaTag = new CampaignItemMetaTag();
            var fourthCampaignItemMetaTag = new CampaignItemMetaTag();
            var fifthCampaignItemMetaTag = new CampaignItemMetaTag();
            var sixthCampaignItemMetaTag = new CampaignItemMetaTag();

            // Act, Assert
            firstCampaignItemMetaTag.ShouldNotBeNull();
            secondCampaignItemMetaTag.ShouldNotBeNull();
            thirdCampaignItemMetaTag.ShouldNotBeNull();
            fourthCampaignItemMetaTag.ShouldNotBeNull();
            fifthCampaignItemMetaTag.ShouldNotBeNull();
            sixthCampaignItemMetaTag.ShouldNotBeNull();
            firstCampaignItemMetaTag.ShouldNotBeSameAs(secondCampaignItemMetaTag);
            thirdCampaignItemMetaTag.ShouldNotBeSameAs(firstCampaignItemMetaTag);
            fourthCampaignItemMetaTag.ShouldNotBeSameAs(firstCampaignItemMetaTag);
            fifthCampaignItemMetaTag.ShouldNotBeSameAs(firstCampaignItemMetaTag);
            sixthCampaignItemMetaTag.ShouldNotBeSameAs(firstCampaignItemMetaTag);
            sixthCampaignItemMetaTag.ShouldNotBeSameAs(fourthCampaignItemMetaTag);
        }

        #endregion

        #region General Constructor : Class (CampaignItemMetaTag) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemMetaTag_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var campaignItemMetaTagId = -1;
            var campaignItemId = -1;
            var socialMediaId = -1;
            var property = string.Empty;
            var content = string.Empty;
            var isDeleted = false;

            // Act
            var campaignItemMetaTag = new CampaignItemMetaTag();

            // Assert
            campaignItemMetaTag.CampaignItemMetaTagID.ShouldBe(campaignItemMetaTagId);
            campaignItemMetaTag.CampaignItemID.ShouldBe(campaignItemId);
            campaignItemMetaTag.SocialMediaID.ShouldBe(socialMediaId);
            campaignItemMetaTag.Property.ShouldBe(property);
            campaignItemMetaTag.Content.ShouldBe(content);
            campaignItemMetaTag.IsDeleted.ShouldBe(isDeleted);
            campaignItemMetaTag.CreatedDate.ShouldBeNull();
            campaignItemMetaTag.CreatedUserID.ShouldBeNull();
            campaignItemMetaTag.UpdatedDate.ShouldBeNull();
            campaignItemMetaTag.UpdatedUserID.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}