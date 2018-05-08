using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Accounts
{
    [TestFixture]
    public class BaseChannelTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BaseChannel) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            var baseChannelId = Fixture.Create<int>();
            var baseChannelName = Fixture.Create<string>();
            var channelURL = Fixture.Create<string>();
            var bounceDomain = Fixture.Create<string>();
            var displayAddress = Fixture.Create<string>();
            var address = Fixture.Create<string>();
            var city = Fixture.Create<string>();
            var state = Fixture.Create<string>();
            var country = Fixture.Create<string>();
            var zip = Fixture.Create<string>();
            var salutation = Fixture.Create<ECN_Framework_Common.Objects.Accounts.Enums.Salutation>();
            var contactName = Fixture.Create<string>();
            var contactTitle = Fixture.Create<string>();
            var phone = Fixture.Create<string>();
            var fax = Fixture.Create<string>();
            var email = Fixture.Create<string>();
            var webAddress = Fixture.Create<string>();
            var infoContentId = Fixture.Create<int?>();
            var masterCustomerId = Fixture.Create<int?>();
            var channelPartnerType = Fixture.Create<ECN_Framework_Common.Objects.Accounts.Enums.ChannelPartnerType>();
            var ratesXml = Fixture.Create<string>();
            var channelType = Fixture.Create<string>();
            var isBranding = Fixture.Create<bool?>();
            var emailThreshold = Fixture.Create<int?>();
            var bounceThreshold = Fixture.Create<int?>();
            var softBounceThreshold = Fixture.Create<int?>();
            var headerSource = Fixture.Create<string>();
            var footerSource = Fixture.Create<string>();
            var brandLogo = Fixture.Create<string>();
            var brandSubDomain = Fixture.Create<string>();
            var createdUserId = Fixture.Create<int?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();
            var channelTypeCode = Fixture.Create<ECN_Framework_Common.Objects.Accounts.Enums.ChannelType>();
            var mSCustomerId = Fixture.Create<int?>();
            var baseChannelGuid = Fixture.Create<Guid?>();
            var isPublisher = Fixture.Create<bool?>();
            var testBlastLimit = Fixture.Create<int?>();
            var accessKey = Fixture.Create<Guid?>();
            var platformClientGroupId = Fixture.Create<int>();

            // Act
            baseChannel.BaseChannelID = baseChannelId;
            baseChannel.BaseChannelName = baseChannelName;
            baseChannel.ChannelURL = channelURL;
            baseChannel.BounceDomain = bounceDomain;
            baseChannel.DisplayAddress = displayAddress;
            baseChannel.Address = address;
            baseChannel.City = city;
            baseChannel.State = state;
            baseChannel.Country = country;
            baseChannel.Zip = zip;
            baseChannel.Salutation = salutation;
            baseChannel.ContactName = contactName;
            baseChannel.ContactTitle = contactTitle;
            baseChannel.Phone = phone;
            baseChannel.Fax = fax;
            baseChannel.Email = email;
            baseChannel.WebAddress = webAddress;
            baseChannel.InfoContentID = infoContentId;
            baseChannel.MasterCustomerID = masterCustomerId;
            baseChannel.ChannelPartnerType = channelPartnerType;
            baseChannel.RatesXml = ratesXml;
            baseChannel.ChannelType = channelType;
            baseChannel.IsBranding = isBranding;
            baseChannel.EmailThreshold = emailThreshold;
            baseChannel.BounceThreshold = bounceThreshold;
            baseChannel.SoftBounceThreshold = softBounceThreshold;
            baseChannel.HeaderSource = headerSource;
            baseChannel.FooterSource = footerSource;
            baseChannel.BrandLogo = brandLogo;
            baseChannel.BrandSubDomain = brandSubDomain;
            baseChannel.CreatedUserID = createdUserId;
            baseChannel.UpdatedUserID = updatedUserId;
            baseChannel.IsDeleted = isDeleted;
            baseChannel.ChannelTypeCode = channelTypeCode;
            baseChannel.MSCustomerID = mSCustomerId;
            baseChannel.BaseChannelGuid = baseChannelGuid;
            baseChannel.IsPublisher = isPublisher;
            baseChannel.TestBlastLimit = testBlastLimit;
            baseChannel.AccessKey = accessKey;
            baseChannel.PlatformClientGroupID = platformClientGroupId;

            // Assert
            baseChannel.BaseChannelID.ShouldBe(baseChannelId);
            baseChannel.BaseChannelName.ShouldBe(baseChannelName);
            baseChannel.ChannelURL.ShouldBe(channelURL);
            baseChannel.BounceDomain.ShouldBe(bounceDomain);
            baseChannel.DisplayAddress.ShouldBe(displayAddress);
            baseChannel.Address.ShouldBe(address);
            baseChannel.City.ShouldBe(city);
            baseChannel.State.ShouldBe(state);
            baseChannel.Country.ShouldBe(country);
            baseChannel.Zip.ShouldBe(zip);
            baseChannel.Salutation.ShouldBe(salutation);
            baseChannel.ContactName.ShouldBe(contactName);
            baseChannel.ContactTitle.ShouldBe(contactTitle);
            baseChannel.Phone.ShouldBe(phone);
            baseChannel.Fax.ShouldBe(fax);
            baseChannel.Email.ShouldBe(email);
            baseChannel.WebAddress.ShouldBe(webAddress);
            baseChannel.InfoContentID.ShouldBe(infoContentId);
            baseChannel.MasterCustomerID.ShouldBe(masterCustomerId);
            baseChannel.ChannelPartnerType.ShouldBe(channelPartnerType);
            baseChannel.RatesXml.ShouldBe(ratesXml);
            baseChannel.ChannelType.ShouldBe(channelType);
            baseChannel.IsBranding.ShouldBe(isBranding);
            baseChannel.EmailThreshold.ShouldBe(emailThreshold);
            baseChannel.BounceThreshold.ShouldBe(bounceThreshold);
            baseChannel.SoftBounceThreshold.ShouldBe(softBounceThreshold);
            baseChannel.HeaderSource.ShouldBe(headerSource);
            baseChannel.FooterSource.ShouldBe(footerSource);
            baseChannel.BrandLogo.ShouldBe(brandLogo);
            baseChannel.BrandSubDomain.ShouldBe(brandSubDomain);
            baseChannel.CreatedUserID.ShouldBe(createdUserId);
            baseChannel.CreatedDate.ShouldBeNull();
            baseChannel.UpdatedUserID.ShouldBe(updatedUserId);
            baseChannel.UpdatedDate.ShouldBeNull();
            baseChannel.IsDeleted.ShouldBe(isDeleted);
            baseChannel.ChannelTypeCode.ShouldBe(channelTypeCode);
            baseChannel.MSCustomerID.ShouldBe(mSCustomerId);
            baseChannel.BaseChannelGuid.ShouldBe(baseChannelGuid);
            baseChannel.IsPublisher.ShouldBe(isPublisher);
            baseChannel.TestBlastLimit.ShouldBe(testBlastLimit);
            baseChannel.AccessKey.ShouldBe(accessKey);
            baseChannel.PlatformClientGroupID.ShouldBe(platformClientGroupId);
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (AccessKey) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_AccessKey_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameAccessKey = "AccessKey";
            var baseChannel = Fixture.Create<BaseChannel>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = baseChannel.GetType().GetProperty(propertyNameAccessKey);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(baseChannel, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (AccessKey) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_AccessKeyNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAccessKey = "AccessKeyNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameAccessKey));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_AccessKey_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAccessKey = "AccessKey";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameAccessKey);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (Address) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Address_Property_String_Type_Verify_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            baseChannel.Address = Fixture.Create<string>();
            var stringType = baseChannel.Address.GetType();

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

        #region General Getters/Setters : Class (BaseChannel) => Property (Address) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_AddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAddress = "AddressNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Address_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAddress = "Address";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (BaseChannelGuid) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_BaseChannelGuid_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameBaseChannelGuid = "BaseChannelGuid";
            var baseChannel = Fixture.Create<BaseChannel>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = baseChannel.GetType().GetProperty(propertyNameBaseChannelGuid);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(baseChannel, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (BaseChannelGuid) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_BaseChannelGuidNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBaseChannelGuid = "BaseChannelGuidNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameBaseChannelGuid));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_BaseChannelGuid_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBaseChannelGuid = "BaseChannelGuid";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameBaseChannelGuid);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (BaseChannelID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_BaseChannelID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            baseChannel.BaseChannelID = Fixture.Create<int>();
            var intType = baseChannel.BaseChannelID.GetType();

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

        #region General Getters/Setters : Class (BaseChannel) => Property (BaseChannelID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_BaseChannelIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelIDNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameBaseChannelID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_BaseChannelID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelID";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameBaseChannelID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (BaseChannelName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_BaseChannelName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            baseChannel.BaseChannelName = Fixture.Create<string>();
            var stringType = baseChannel.BaseChannelName.GetType();

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

        #region General Getters/Setters : Class (BaseChannel) => Property (BaseChannelName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_BaseChannelNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBaseChannelName = "BaseChannelNameNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameBaseChannelName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_BaseChannelName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBaseChannelName = "BaseChannelName";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameBaseChannelName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (BounceDomain) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_BounceDomain_Property_String_Type_Verify_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            baseChannel.BounceDomain = Fixture.Create<string>();
            var stringType = baseChannel.BounceDomain.GetType();

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

        #region General Getters/Setters : Class (BaseChannel) => Property (BounceDomain) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_BounceDomainNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBounceDomain = "BounceDomainNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameBounceDomain));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_BounceDomain_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBounceDomain = "BounceDomain";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameBounceDomain);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (BounceThreshold) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_BounceThreshold_Property_Data_Without_Null_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            var random = Fixture.Create<int>();

            // Act , Set
            baseChannel.BounceThreshold = random;

            // Assert
            baseChannel.BounceThreshold.ShouldBe(random);
            baseChannel.BounceThreshold.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_BounceThreshold_Property_Only_Null_Data_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();    

            // Act , Set
            baseChannel.BounceThreshold = null;

            // Assert
            baseChannel.BounceThreshold.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_BounceThreshold_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBounceThreshold = "BounceThreshold";
            var baseChannel = Fixture.Create<BaseChannel>();
            var propertyInfo = baseChannel.GetType().GetProperty(propertyNameBounceThreshold);

            // Act , Set
            propertyInfo.SetValue(baseChannel, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            baseChannel.BounceThreshold.ShouldBeNull();
            baseChannel.BounceThreshold.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (BounceThreshold) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_BounceThresholdNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBounceThreshold = "BounceThresholdNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameBounceThreshold));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_BounceThreshold_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBounceThreshold = "BounceThreshold";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameBounceThreshold);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (BrandLogo) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_BrandLogo_Property_String_Type_Verify_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            baseChannel.BrandLogo = Fixture.Create<string>();
            var stringType = baseChannel.BrandLogo.GetType();

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

        #region General Getters/Setters : Class (BaseChannel) => Property (BrandLogo) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_BrandLogoNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBrandLogo = "BrandLogoNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameBrandLogo));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_BrandLogo_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBrandLogo = "BrandLogo";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameBrandLogo);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (BrandSubDomain) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_BrandSubDomain_Property_String_Type_Verify_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            baseChannel.BrandSubDomain = Fixture.Create<string>();
            var stringType = baseChannel.BrandSubDomain.GetType();

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

        #region General Getters/Setters : Class (BaseChannel) => Property (BrandSubDomain) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_BrandSubDomainNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBrandSubDomain = "BrandSubDomainNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameBrandSubDomain));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_BrandSubDomain_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBrandSubDomain = "BrandSubDomain";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameBrandSubDomain);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (ChannelPartnerType) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_ChannelPartnerType_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameChannelPartnerType = "ChannelPartnerType";
            var baseChannel = Fixture.Create<BaseChannel>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = baseChannel.GetType().GetProperty(propertyNameChannelPartnerType);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(baseChannel, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (ChannelPartnerType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_ChannelPartnerTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameChannelPartnerType = "ChannelPartnerTypeNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameChannelPartnerType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_ChannelPartnerType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameChannelPartnerType = "ChannelPartnerType";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameChannelPartnerType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (ChannelType) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_ChannelType_Property_String_Type_Verify_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            baseChannel.ChannelType = Fixture.Create<string>();
            var stringType = baseChannel.ChannelType.GetType();

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

        #region General Getters/Setters : Class (BaseChannel) => Property (ChannelType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_ChannelTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameChannelType = "ChannelTypeNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameChannelType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_ChannelType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameChannelType = "ChannelType";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameChannelType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (ChannelTypeCode) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_ChannelTypeCode_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameChannelTypeCode = "ChannelTypeCode";
            var baseChannel = Fixture.Create<BaseChannel>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = baseChannel.GetType().GetProperty(propertyNameChannelTypeCode);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(baseChannel, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (ChannelTypeCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_ChannelTypeCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameChannelTypeCode = "ChannelTypeCodeNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameChannelTypeCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_ChannelTypeCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameChannelTypeCode = "ChannelTypeCode";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameChannelTypeCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (ChannelURL) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_ChannelURL_Property_String_Type_Verify_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            baseChannel.ChannelURL = Fixture.Create<string>();
            var stringType = baseChannel.ChannelURL.GetType();

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

        #region General Getters/Setters : Class (BaseChannel) => Property (ChannelURL) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_ChannelURLNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameChannelURL = "ChannelURLNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameChannelURL));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_ChannelURL_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameChannelURL = "ChannelURL";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameChannelURL);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (City) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_City_Property_String_Type_Verify_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            baseChannel.City = Fixture.Create<string>();
            var stringType = baseChannel.City.GetType();

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

        #region General Getters/Setters : Class (BaseChannel) => Property (City) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_CityNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCity = "CityNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameCity));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_City_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCity = "City";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameCity);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (ContactName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_ContactName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            baseChannel.ContactName = Fixture.Create<string>();
            var stringType = baseChannel.ContactName.GetType();

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

        #region General Getters/Setters : Class (BaseChannel) => Property (ContactName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_ContactNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContactName = "ContactNameNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameContactName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_ContactName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContactName = "ContactName";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameContactName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (ContactTitle) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_ContactTitle_Property_String_Type_Verify_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            baseChannel.ContactTitle = Fixture.Create<string>();
            var stringType = baseChannel.ContactTitle.GetType();

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

        #region General Getters/Setters : Class (BaseChannel) => Property (ContactTitle) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_ContactTitleNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContactTitle = "ContactTitleNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameContactTitle));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_ContactTitle_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContactTitle = "ContactTitle";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameContactTitle);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (Country) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Country_Property_String_Type_Verify_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            baseChannel.Country = Fixture.Create<string>();
            var stringType = baseChannel.Country.GetType();

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

        #region General Getters/Setters : Class (BaseChannel) => Property (Country) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_CountryNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCountry = "CountryNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameCountry));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Country_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCountry = "Country";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameCountry);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var baseChannel = Fixture.Create<BaseChannel>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = baseChannel.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(baseChannel, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            var random = Fixture.Create<int>();

            // Act , Set
            baseChannel.CreatedUserID = random;

            // Assert
            baseChannel.CreatedUserID.ShouldBe(random);
            baseChannel.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();    

            // Act , Set
            baseChannel.CreatedUserID = null;

            // Assert
            baseChannel.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var baseChannel = Fixture.Create<BaseChannel>();
            var propertyInfo = baseChannel.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(baseChannel, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            baseChannel.CreatedUserID.ShouldBeNull();
            baseChannel.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (DisplayAddress) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_DisplayAddress_Property_String_Type_Verify_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            baseChannel.DisplayAddress = Fixture.Create<string>();
            var stringType = baseChannel.DisplayAddress.GetType();

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

        #region General Getters/Setters : Class (BaseChannel) => Property (DisplayAddress) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_DisplayAddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDisplayAddress = "DisplayAddressNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameDisplayAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_DisplayAddress_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDisplayAddress = "DisplayAddress";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameDisplayAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (Email) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Email_Property_String_Type_Verify_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            baseChannel.Email = Fixture.Create<string>();
            var stringType = baseChannel.Email.GetType();

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

        #region General Getters/Setters : Class (BaseChannel) => Property (Email) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_EmailNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmail = "EmailNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameEmail));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Email_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmail = "Email";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameEmail);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (EmailThreshold) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_EmailThreshold_Property_Data_Without_Null_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            var random = Fixture.Create<int>();

            // Act , Set
            baseChannel.EmailThreshold = random;

            // Assert
            baseChannel.EmailThreshold.ShouldBe(random);
            baseChannel.EmailThreshold.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_EmailThreshold_Property_Only_Null_Data_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();    

            // Act , Set
            baseChannel.EmailThreshold = null;

            // Assert
            baseChannel.EmailThreshold.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_EmailThreshold_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameEmailThreshold = "EmailThreshold";
            var baseChannel = Fixture.Create<BaseChannel>();
            var propertyInfo = baseChannel.GetType().GetProperty(propertyNameEmailThreshold);

            // Act , Set
            propertyInfo.SetValue(baseChannel, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            baseChannel.EmailThreshold.ShouldBeNull();
            baseChannel.EmailThreshold.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (EmailThreshold) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_EmailThresholdNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailThreshold = "EmailThresholdNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameEmailThreshold));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_EmailThreshold_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailThreshold = "EmailThreshold";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameEmailThreshold);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (Fax) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Fax_Property_String_Type_Verify_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            baseChannel.Fax = Fixture.Create<string>();
            var stringType = baseChannel.Fax.GetType();

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

        #region General Getters/Setters : Class (BaseChannel) => Property (Fax) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_FaxNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFax = "FaxNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameFax));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Fax_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFax = "Fax";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameFax);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (FooterSource) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_FooterSource_Property_String_Type_Verify_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            baseChannel.FooterSource = Fixture.Create<string>();
            var stringType = baseChannel.FooterSource.GetType();

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

        #region General Getters/Setters : Class (BaseChannel) => Property (FooterSource) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_FooterSourceNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFooterSource = "FooterSourceNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameFooterSource));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_FooterSource_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFooterSource = "FooterSource";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameFooterSource);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (HeaderSource) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_HeaderSource_Property_String_Type_Verify_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            baseChannel.HeaderSource = Fixture.Create<string>();
            var stringType = baseChannel.HeaderSource.GetType();

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

        #region General Getters/Setters : Class (BaseChannel) => Property (HeaderSource) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_HeaderSourceNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameHeaderSource = "HeaderSourceNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameHeaderSource));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_HeaderSource_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameHeaderSource = "HeaderSource";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameHeaderSource);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (InfoContentID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_InfoContentID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            var random = Fixture.Create<int>();

            // Act , Set
            baseChannel.InfoContentID = random;

            // Assert
            baseChannel.InfoContentID.ShouldBe(random);
            baseChannel.InfoContentID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_InfoContentID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();    

            // Act , Set
            baseChannel.InfoContentID = null;

            // Assert
            baseChannel.InfoContentID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_InfoContentID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameInfoContentID = "InfoContentID";
            var baseChannel = Fixture.Create<BaseChannel>();
            var propertyInfo = baseChannel.GetType().GetProperty(propertyNameInfoContentID);

            // Act , Set
            propertyInfo.SetValue(baseChannel, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            baseChannel.InfoContentID.ShouldBeNull();
            baseChannel.InfoContentID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (InfoContentID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_InfoContentIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameInfoContentID = "InfoContentIDNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameInfoContentID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_InfoContentID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameInfoContentID = "InfoContentID";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameInfoContentID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (IsBranding) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_IsBranding_Property_Data_Without_Null_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            var random = Fixture.Create<bool>();

            // Act , Set
            baseChannel.IsBranding = random;

            // Assert
            baseChannel.IsBranding.ShouldBe(random);
            baseChannel.IsBranding.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_IsBranding_Property_Only_Null_Data_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();    

            // Act , Set
            baseChannel.IsBranding = null;

            // Assert
            baseChannel.IsBranding.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_IsBranding_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsBranding = "IsBranding";
            var baseChannel = Fixture.Create<BaseChannel>();
            var propertyInfo = baseChannel.GetType().GetProperty(propertyNameIsBranding);

            // Act , Set
            propertyInfo.SetValue(baseChannel, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            baseChannel.IsBranding.ShouldBeNull();
            baseChannel.IsBranding.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (IsBranding) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_IsBrandingNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsBranding = "IsBrandingNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameIsBranding));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_IsBranding_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsBranding = "IsBranding";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameIsBranding);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            var random = Fixture.Create<bool>();

            // Act , Set
            baseChannel.IsDeleted = random;

            // Assert
            baseChannel.IsDeleted.ShouldBe(random);
            baseChannel.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();    

            // Act , Set
            baseChannel.IsDeleted = null;

            // Assert
            baseChannel.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var baseChannel = Fixture.Create<BaseChannel>();
            var propertyInfo = baseChannel.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(baseChannel, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            baseChannel.IsDeleted.ShouldBeNull();
            baseChannel.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (IsPublisher) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_IsPublisher_Property_Data_Without_Null_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            var random = Fixture.Create<bool>();

            // Act , Set
            baseChannel.IsPublisher = random;

            // Assert
            baseChannel.IsPublisher.ShouldBe(random);
            baseChannel.IsPublisher.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_IsPublisher_Property_Only_Null_Data_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();    

            // Act , Set
            baseChannel.IsPublisher = null;

            // Assert
            baseChannel.IsPublisher.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_IsPublisher_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsPublisher = "IsPublisher";
            var baseChannel = Fixture.Create<BaseChannel>();
            var propertyInfo = baseChannel.GetType().GetProperty(propertyNameIsPublisher);

            // Act , Set
            propertyInfo.SetValue(baseChannel, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            baseChannel.IsPublisher.ShouldBeNull();
            baseChannel.IsPublisher.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (IsPublisher) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_IsPublisherNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsPublisher = "IsPublisherNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameIsPublisher));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_IsPublisher_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsPublisher = "IsPublisher";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameIsPublisher);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (MasterCustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_MasterCustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            var random = Fixture.Create<int>();

            // Act , Set
            baseChannel.MasterCustomerID = random;

            // Assert
            baseChannel.MasterCustomerID.ShouldBe(random);
            baseChannel.MasterCustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_MasterCustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();    

            // Act , Set
            baseChannel.MasterCustomerID = null;

            // Assert
            baseChannel.MasterCustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_MasterCustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameMasterCustomerID = "MasterCustomerID";
            var baseChannel = Fixture.Create<BaseChannel>();
            var propertyInfo = baseChannel.GetType().GetProperty(propertyNameMasterCustomerID);

            // Act , Set
            propertyInfo.SetValue(baseChannel, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            baseChannel.MasterCustomerID.ShouldBeNull();
            baseChannel.MasterCustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (MasterCustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_MasterCustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMasterCustomerID = "MasterCustomerIDNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameMasterCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_MasterCustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMasterCustomerID = "MasterCustomerID";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameMasterCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (MSCustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_MSCustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            var random = Fixture.Create<int>();

            // Act , Set
            baseChannel.MSCustomerID = random;

            // Assert
            baseChannel.MSCustomerID.ShouldBe(random);
            baseChannel.MSCustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_MSCustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();    

            // Act , Set
            baseChannel.MSCustomerID = null;

            // Assert
            baseChannel.MSCustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_MSCustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameMSCustomerID = "MSCustomerID";
            var baseChannel = Fixture.Create<BaseChannel>();
            var propertyInfo = baseChannel.GetType().GetProperty(propertyNameMSCustomerID);

            // Act , Set
            propertyInfo.SetValue(baseChannel, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            baseChannel.MSCustomerID.ShouldBeNull();
            baseChannel.MSCustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (MSCustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_MSCustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMSCustomerID = "MSCustomerIDNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameMSCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_MSCustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMSCustomerID = "MSCustomerID";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameMSCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (Phone) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Phone_Property_String_Type_Verify_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            baseChannel.Phone = Fixture.Create<string>();
            var stringType = baseChannel.Phone.GetType();

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

        #region General Getters/Setters : Class (BaseChannel) => Property (Phone) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_PhoneNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePhone = "PhoneNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNamePhone));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Phone_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePhone = "Phone";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNamePhone);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (PlatformClientGroupID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_PlatformClientGroupID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            baseChannel.PlatformClientGroupID = Fixture.Create<int>();
            var intType = baseChannel.PlatformClientGroupID.GetType();

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

        #region General Getters/Setters : Class (BaseChannel) => Property (PlatformClientGroupID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_PlatformClientGroupIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePlatformClientGroupID = "PlatformClientGroupIDNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNamePlatformClientGroupID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_PlatformClientGroupID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePlatformClientGroupID = "PlatformClientGroupID";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNamePlatformClientGroupID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (RatesXml) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_RatesXml_Property_String_Type_Verify_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            baseChannel.RatesXml = Fixture.Create<string>();
            var stringType = baseChannel.RatesXml.GetType();

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

        #region General Getters/Setters : Class (BaseChannel) => Property (RatesXml) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_RatesXmlNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRatesXml = "RatesXmlNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameRatesXml));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_RatesXml_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRatesXml = "RatesXml";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameRatesXml);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (Salutation) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Salutation_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSalutation = "Salutation";
            var baseChannel = Fixture.Create<BaseChannel>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = baseChannel.GetType().GetProperty(propertyNameSalutation);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(baseChannel, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (Salutation) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_SalutationNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSalutation = "SalutationNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameSalutation));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Salutation_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSalutation = "Salutation";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameSalutation);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (SoftBounceThreshold) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_SoftBounceThreshold_Property_Data_Without_Null_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            var random = Fixture.Create<int>();

            // Act , Set
            baseChannel.SoftBounceThreshold = random;

            // Assert
            baseChannel.SoftBounceThreshold.ShouldBe(random);
            baseChannel.SoftBounceThreshold.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_SoftBounceThreshold_Property_Only_Null_Data_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();    

            // Act , Set
            baseChannel.SoftBounceThreshold = null;

            // Assert
            baseChannel.SoftBounceThreshold.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_SoftBounceThreshold_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameSoftBounceThreshold = "SoftBounceThreshold";
            var baseChannel = Fixture.Create<BaseChannel>();
            var propertyInfo = baseChannel.GetType().GetProperty(propertyNameSoftBounceThreshold);

            // Act , Set
            propertyInfo.SetValue(baseChannel, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            baseChannel.SoftBounceThreshold.ShouldBeNull();
            baseChannel.SoftBounceThreshold.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (SoftBounceThreshold) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_SoftBounceThresholdNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSoftBounceThreshold = "SoftBounceThresholdNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameSoftBounceThreshold));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_SoftBounceThreshold_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSoftBounceThreshold = "SoftBounceThreshold";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameSoftBounceThreshold);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (State) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_State_Property_String_Type_Verify_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            baseChannel.State = Fixture.Create<string>();
            var stringType = baseChannel.State.GetType();

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

        #region General Getters/Setters : Class (BaseChannel) => Property (State) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_StateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameState = "StateNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameState));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_State_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameState = "State";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameState);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (TestBlastLimit) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_TestBlastLimit_Property_Data_Without_Null_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            var random = Fixture.Create<int>();

            // Act , Set
            baseChannel.TestBlastLimit = random;

            // Assert
            baseChannel.TestBlastLimit.ShouldBe(random);
            baseChannel.TestBlastLimit.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_TestBlastLimit_Property_Only_Null_Data_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();    

            // Act , Set
            baseChannel.TestBlastLimit = null;

            // Assert
            baseChannel.TestBlastLimit.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_TestBlastLimit_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameTestBlastLimit = "TestBlastLimit";
            var baseChannel = Fixture.Create<BaseChannel>();
            var propertyInfo = baseChannel.GetType().GetProperty(propertyNameTestBlastLimit);

            // Act , Set
            propertyInfo.SetValue(baseChannel, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            baseChannel.TestBlastLimit.ShouldBeNull();
            baseChannel.TestBlastLimit.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (TestBlastLimit) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_TestBlastLimitNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTestBlastLimit = "TestBlastLimitNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameTestBlastLimit));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_TestBlastLimit_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTestBlastLimit = "TestBlastLimit";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameTestBlastLimit);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var baseChannel = Fixture.Create<BaseChannel>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = baseChannel.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(baseChannel, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            var random = Fixture.Create<int>();

            // Act , Set
            baseChannel.UpdatedUserID = random;

            // Assert
            baseChannel.UpdatedUserID.ShouldBe(random);
            baseChannel.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();    

            // Act , Set
            baseChannel.UpdatedUserID = null;

            // Assert
            baseChannel.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var baseChannel = Fixture.Create<BaseChannel>();
            var propertyInfo = baseChannel.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(baseChannel, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            baseChannel.UpdatedUserID.ShouldBeNull();
            baseChannel.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (WebAddress) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_WebAddress_Property_String_Type_Verify_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            baseChannel.WebAddress = Fixture.Create<string>();
            var stringType = baseChannel.WebAddress.GetType();

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

        #region General Getters/Setters : Class (BaseChannel) => Property (WebAddress) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_WebAddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameWebAddress = "WebAddressNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameWebAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_WebAddress_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameWebAddress = "WebAddress";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameWebAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BaseChannel) => Property (Zip) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Zip_Property_String_Type_Verify_Test()
        {
            // Arrange
            var baseChannel = Fixture.Create<BaseChannel>();
            baseChannel.Zip = Fixture.Create<string>();
            var stringType = baseChannel.Zip.GetType();

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

        #region General Getters/Setters : Class (BaseChannel) => Property (Zip) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Class_Invalid_Property_ZipNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameZip = "ZipNotPresent";
            var baseChannel  = Fixture.Create<BaseChannel>();

            // Act , Assert
            Should.NotThrow(() => baseChannel.GetType().GetProperty(propertyNameZip));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BaseChannel_Zip_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameZip = "Zip";
            var baseChannel  = Fixture.Create<BaseChannel>();
            var propertyInfo  = baseChannel.GetType().GetProperty(propertyNameZip);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BaseChannel) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BaseChannel_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BaseChannel());
        }

        #endregion

        #region General Constructor : Class (BaseChannel) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BaseChannel_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBaseChannel = Fixture.CreateMany<BaseChannel>(2).ToList();
            var firstBaseChannel = instancesOfBaseChannel.FirstOrDefault();
            var lastBaseChannel = instancesOfBaseChannel.Last();

            // Act, Assert
            firstBaseChannel.ShouldNotBeNull();
            lastBaseChannel.ShouldNotBeNull();
            firstBaseChannel.ShouldNotBeSameAs(lastBaseChannel);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BaseChannel_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBaseChannel = new BaseChannel();
            var secondBaseChannel = new BaseChannel();
            var thirdBaseChannel = new BaseChannel();
            var fourthBaseChannel = new BaseChannel();
            var fifthBaseChannel = new BaseChannel();
            var sixthBaseChannel = new BaseChannel();

            // Act, Assert
            firstBaseChannel.ShouldNotBeNull();
            secondBaseChannel.ShouldNotBeNull();
            thirdBaseChannel.ShouldNotBeNull();
            fourthBaseChannel.ShouldNotBeNull();
            fifthBaseChannel.ShouldNotBeNull();
            sixthBaseChannel.ShouldNotBeNull();
            firstBaseChannel.ShouldNotBeSameAs(secondBaseChannel);
            thirdBaseChannel.ShouldNotBeSameAs(firstBaseChannel);
            fourthBaseChannel.ShouldNotBeSameAs(firstBaseChannel);
            fifthBaseChannel.ShouldNotBeSameAs(firstBaseChannel);
            sixthBaseChannel.ShouldNotBeSameAs(firstBaseChannel);
            sixthBaseChannel.ShouldNotBeSameAs(fourthBaseChannel);
        }

        #endregion

        #region General Constructor : Class (BaseChannel) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BaseChannel_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var baseChannelId = -1;
            var baseChannelName = string.Empty;
            var channelURL = string.Empty;
            var bounceDomain = string.Empty;
            var displayAddress = string.Empty;
            var address = string.Empty;
            var city = string.Empty;
            var state = string.Empty;
            var country = string.Empty;
            var zip = string.Empty;
            var salutation = ECN_Framework_Common.Objects.Accounts.Enums.Salutation.Unknown;
            var contactName = string.Empty;
            var contactTitle = string.Empty;
            var phone = string.Empty;
            var fax = string.Empty;
            var email = string.Empty;
            var webAddress = string.Empty;
            var channelPartnerType = ECN_Framework_Common.Objects.Accounts.Enums.ChannelPartnerType.NotInitialized;
            var ratesXml = string.Empty;
            var channelType = string.Empty;
            var channelTypeCode = ECN_Framework_Common.Objects.Accounts.Enums.ChannelType.Unknown;
            var headerSource = string.Empty;
            var footerSource = string.Empty;
            var brandLogo = string.Empty;
            var brandSubDomain = string.Empty;
            var platformClientGroupId = -1;

            // Act
            var baseChannel = new BaseChannel();

            // Assert
            baseChannel.BaseChannelID.ShouldBe(baseChannelId);
            baseChannel.BaseChannelName.ShouldBe(baseChannelName);
            baseChannel.ChannelURL.ShouldBe(channelURL);
            baseChannel.BounceDomain.ShouldBe(bounceDomain);
            baseChannel.DisplayAddress.ShouldBe(displayAddress);
            baseChannel.Address.ShouldBe(address);
            baseChannel.City.ShouldBe(city);
            baseChannel.State.ShouldBe(state);
            baseChannel.Country.ShouldBe(country);
            baseChannel.Zip.ShouldBe(zip);
            baseChannel.Salutation.ShouldBe(salutation);
            baseChannel.ContactName.ShouldBe(contactName);
            baseChannel.ContactTitle.ShouldBe(contactTitle);
            baseChannel.Phone.ShouldBe(phone);
            baseChannel.Fax.ShouldBe(fax);
            baseChannel.Email.ShouldBe(email);
            baseChannel.WebAddress.ShouldBe(webAddress);
            baseChannel.InfoContentID.ShouldBeNull();
            baseChannel.MasterCustomerID.ShouldBeNull();
            baseChannel.ChannelPartnerType.ShouldBe(channelPartnerType);
            baseChannel.RatesXml.ShouldBe(ratesXml);
            baseChannel.ChannelType.ShouldBe(channelType);
            baseChannel.ChannelTypeCode.ShouldBe(channelTypeCode);
            baseChannel.IsBranding.ShouldBeNull();
            baseChannel.EmailThreshold.ShouldBeNull();
            baseChannel.BounceThreshold.ShouldBeNull();
            baseChannel.SoftBounceThreshold.ShouldBeNull();
            baseChannel.HeaderSource.ShouldBe(headerSource);
            baseChannel.FooterSource.ShouldBe(footerSource);
            baseChannel.CreatedUserID.ShouldBeNull();
            baseChannel.CreatedDate.ShouldBeNull();
            baseChannel.UpdatedUserID.ShouldBeNull();
            baseChannel.UpdatedDate.ShouldBeNull();
            baseChannel.IsDeleted.ShouldBeNull();
            baseChannel.MSCustomerID.ShouldBeNull();
            baseChannel.BrandLogo.ShouldBe(brandLogo);
            baseChannel.BrandSubDomain.ShouldBe(brandSubDomain);
            baseChannel.BaseChannelGuid.ShouldBeNull();
            baseChannel.IsPublisher.ShouldBeNull();
            baseChannel.TestBlastLimit.ShouldBeNull();
            baseChannel.AccessKey.ShouldBeNull();
            baseChannel.PlatformClientGroupID.ShouldBe(platformClientGroupId);
        }

        #endregion

        #endregion

        #endregion
    }
}