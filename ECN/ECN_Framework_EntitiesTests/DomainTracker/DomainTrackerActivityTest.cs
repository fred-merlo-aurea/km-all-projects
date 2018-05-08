using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.DomainTracker;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.DomainTracker
{
    [TestFixture]
    public class DomainTrackerActivityTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (DomainTrackerActivity) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var domainTrackerActivity = Fixture.Create<DomainTrackerActivity>();
            var domainTrackerActivityId = Fixture.Create<int>();
            var domainTrackerId = Fixture.Create<int>();
            var profileId = Fixture.Create<int?>();
            var pageURL = Fixture.Create<string>();
            var timeStamp = Fixture.Create<DateTime?>();
            var iPAddress = Fixture.Create<string>();
            var userAgent = Fixture.Create<string>();
            var oS = Fixture.Create<string>();
            var browser = Fixture.Create<string>();
            var referralURL = Fixture.Create<string>();
            var sourceBlastId = Fixture.Create<int?>();
            var fieldsValuePairsList = Fixture.Create<List<FieldsValuePair>>();

            // Act
            domainTrackerActivity.DomainTrackerActivityID = domainTrackerActivityId;
            domainTrackerActivity.DomainTrackerID = domainTrackerId;
            domainTrackerActivity.ProfileID = profileId;
            domainTrackerActivity.PageURL = pageURL;
            domainTrackerActivity.TimeStamp = timeStamp;
            domainTrackerActivity.IPAddress = iPAddress;
            domainTrackerActivity.UserAgent = userAgent;
            domainTrackerActivity.OS = oS;
            domainTrackerActivity.Browser = browser;
            domainTrackerActivity.ReferralURL = referralURL;
            domainTrackerActivity.SourceBlastID = sourceBlastId;
            domainTrackerActivity.FieldsValuePairsList = fieldsValuePairsList;

            // Assert
            domainTrackerActivity.DomainTrackerActivityID.ShouldBe(domainTrackerActivityId);
            domainTrackerActivity.DomainTrackerID.ShouldBe(domainTrackerId);
            domainTrackerActivity.ProfileID.ShouldBe(profileId);
            domainTrackerActivity.PageURL.ShouldBe(pageURL);
            domainTrackerActivity.TimeStamp.ShouldBe(timeStamp);
            domainTrackerActivity.IPAddress.ShouldBe(iPAddress);
            domainTrackerActivity.UserAgent.ShouldBe(userAgent);
            domainTrackerActivity.OS.ShouldBe(oS);
            domainTrackerActivity.Browser.ShouldBe(browser);
            domainTrackerActivity.ReferralURL.ShouldBe(referralURL);
            domainTrackerActivity.SourceBlastID.ShouldBe(sourceBlastId);
            domainTrackerActivity.FieldsValuePairsList.ShouldBe(fieldsValuePairsList);
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerActivity) => Property (Browser) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_Browser_Property_String_Type_Verify_Test()
        {
            // Arrange
            var domainTrackerActivity = Fixture.Create<DomainTrackerActivity>();
            domainTrackerActivity.Browser = Fixture.Create<string>();
            var stringType = domainTrackerActivity.Browser.GetType();

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

        #region General Getters/Setters : Class (DomainTrackerActivity) => Property (Browser) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_Class_Invalid_Property_BrowserNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBrowser = "BrowserNotPresent";
            var domainTrackerActivity  = Fixture.Create<DomainTrackerActivity>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerActivity.GetType().GetProperty(propertyNameBrowser));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_Browser_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBrowser = "Browser";
            var domainTrackerActivity  = Fixture.Create<DomainTrackerActivity>();
            var propertyInfo  = domainTrackerActivity.GetType().GetProperty(propertyNameBrowser);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerActivity) => Property (DomainTrackerActivityID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_DomainTrackerActivityID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var domainTrackerActivity = Fixture.Create<DomainTrackerActivity>();
            domainTrackerActivity.DomainTrackerActivityID = Fixture.Create<int>();
            var intType = domainTrackerActivity.DomainTrackerActivityID.GetType();

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

        #region General Getters/Setters : Class (DomainTrackerActivity) => Property (DomainTrackerActivityID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_Class_Invalid_Property_DomainTrackerActivityIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDomainTrackerActivityID = "DomainTrackerActivityIDNotPresent";
            var domainTrackerActivity  = Fixture.Create<DomainTrackerActivity>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerActivity.GetType().GetProperty(propertyNameDomainTrackerActivityID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_DomainTrackerActivityID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDomainTrackerActivityID = "DomainTrackerActivityID";
            var domainTrackerActivity  = Fixture.Create<DomainTrackerActivity>();
            var propertyInfo  = domainTrackerActivity.GetType().GetProperty(propertyNameDomainTrackerActivityID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerActivity) => Property (DomainTrackerID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_DomainTrackerID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var domainTrackerActivity = Fixture.Create<DomainTrackerActivity>();
            domainTrackerActivity.DomainTrackerID = Fixture.Create<int>();
            var intType = domainTrackerActivity.DomainTrackerID.GetType();

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

        #region General Getters/Setters : Class (DomainTrackerActivity) => Property (DomainTrackerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_Class_Invalid_Property_DomainTrackerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDomainTrackerID = "DomainTrackerIDNotPresent";
            var domainTrackerActivity  = Fixture.Create<DomainTrackerActivity>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerActivity.GetType().GetProperty(propertyNameDomainTrackerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_DomainTrackerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDomainTrackerID = "DomainTrackerID";
            var domainTrackerActivity  = Fixture.Create<DomainTrackerActivity>();
            var propertyInfo  = domainTrackerActivity.GetType().GetProperty(propertyNameDomainTrackerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerActivity) => Property (FieldsValuePairsList) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_Class_Invalid_Property_FieldsValuePairsListNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFieldsValuePairsList = "FieldsValuePairsListNotPresent";
            var domainTrackerActivity  = Fixture.Create<DomainTrackerActivity>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerActivity.GetType().GetProperty(propertyNameFieldsValuePairsList));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_FieldsValuePairsList_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFieldsValuePairsList = "FieldsValuePairsList";
            var domainTrackerActivity  = Fixture.Create<DomainTrackerActivity>();
            var propertyInfo  = domainTrackerActivity.GetType().GetProperty(propertyNameFieldsValuePairsList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerActivity) => Property (IPAddress) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_IPAddress_Property_String_Type_Verify_Test()
        {
            // Arrange
            var domainTrackerActivity = Fixture.Create<DomainTrackerActivity>();
            domainTrackerActivity.IPAddress = Fixture.Create<string>();
            var stringType = domainTrackerActivity.IPAddress.GetType();

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

        #region General Getters/Setters : Class (DomainTrackerActivity) => Property (IPAddress) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_Class_Invalid_Property_IPAddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIPAddress = "IPAddressNotPresent";
            var domainTrackerActivity  = Fixture.Create<DomainTrackerActivity>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerActivity.GetType().GetProperty(propertyNameIPAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_IPAddress_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIPAddress = "IPAddress";
            var domainTrackerActivity  = Fixture.Create<DomainTrackerActivity>();
            var propertyInfo  = domainTrackerActivity.GetType().GetProperty(propertyNameIPAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerActivity) => Property (OS) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_OS_Property_String_Type_Verify_Test()
        {
            // Arrange
            var domainTrackerActivity = Fixture.Create<DomainTrackerActivity>();
            domainTrackerActivity.OS = Fixture.Create<string>();
            var stringType = domainTrackerActivity.OS.GetType();

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

        #region General Getters/Setters : Class (DomainTrackerActivity) => Property (OS) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_Class_Invalid_Property_OSNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOS = "OSNotPresent";
            var domainTrackerActivity  = Fixture.Create<DomainTrackerActivity>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerActivity.GetType().GetProperty(propertyNameOS));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_OS_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOS = "OS";
            var domainTrackerActivity  = Fixture.Create<DomainTrackerActivity>();
            var propertyInfo  = domainTrackerActivity.GetType().GetProperty(propertyNameOS);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerActivity) => Property (PageURL) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_PageURL_Property_String_Type_Verify_Test()
        {
            // Arrange
            var domainTrackerActivity = Fixture.Create<DomainTrackerActivity>();
            domainTrackerActivity.PageURL = Fixture.Create<string>();
            var stringType = domainTrackerActivity.PageURL.GetType();

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

        #region General Getters/Setters : Class (DomainTrackerActivity) => Property (PageURL) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_Class_Invalid_Property_PageURLNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePageURL = "PageURLNotPresent";
            var domainTrackerActivity  = Fixture.Create<DomainTrackerActivity>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerActivity.GetType().GetProperty(propertyNamePageURL));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_PageURL_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePageURL = "PageURL";
            var domainTrackerActivity  = Fixture.Create<DomainTrackerActivity>();
            var propertyInfo  = domainTrackerActivity.GetType().GetProperty(propertyNamePageURL);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerActivity) => Property (ProfileID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_ProfileID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var domainTrackerActivity = Fixture.Create<DomainTrackerActivity>();
            var random = Fixture.Create<int>();

            // Act , Set
            domainTrackerActivity.ProfileID = random;

            // Assert
            domainTrackerActivity.ProfileID.ShouldBe(random);
            domainTrackerActivity.ProfileID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_ProfileID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var domainTrackerActivity = Fixture.Create<DomainTrackerActivity>();

            // Act , Set
            domainTrackerActivity.ProfileID = null;

            // Assert
            domainTrackerActivity.ProfileID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_ProfileID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameProfileID = "ProfileID";
            var domainTrackerActivity = Fixture.Create<DomainTrackerActivity>();
            var propertyInfo = domainTrackerActivity.GetType().GetProperty(propertyNameProfileID);

            // Act , Set
            propertyInfo.SetValue(domainTrackerActivity, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            domainTrackerActivity.ProfileID.ShouldBeNull();
            domainTrackerActivity.ProfileID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerActivity) => Property (ProfileID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_Class_Invalid_Property_ProfileIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameProfileID = "ProfileIDNotPresent";
            var domainTrackerActivity  = Fixture.Create<DomainTrackerActivity>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerActivity.GetType().GetProperty(propertyNameProfileID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_ProfileID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameProfileID = "ProfileID";
            var domainTrackerActivity  = Fixture.Create<DomainTrackerActivity>();
            var propertyInfo  = domainTrackerActivity.GetType().GetProperty(propertyNameProfileID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerActivity) => Property (ReferralURL) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_ReferralURL_Property_String_Type_Verify_Test()
        {
            // Arrange
            var domainTrackerActivity = Fixture.Create<DomainTrackerActivity>();
            domainTrackerActivity.ReferralURL = Fixture.Create<string>();
            var stringType = domainTrackerActivity.ReferralURL.GetType();

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

        #region General Getters/Setters : Class (DomainTrackerActivity) => Property (ReferralURL) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_Class_Invalid_Property_ReferralURLNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameReferralURL = "ReferralURLNotPresent";
            var domainTrackerActivity  = Fixture.Create<DomainTrackerActivity>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerActivity.GetType().GetProperty(propertyNameReferralURL));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_ReferralURL_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameReferralURL = "ReferralURL";
            var domainTrackerActivity  = Fixture.Create<DomainTrackerActivity>();
            var propertyInfo  = domainTrackerActivity.GetType().GetProperty(propertyNameReferralURL);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerActivity) => Property (SourceBlastID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_SourceBlastID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var domainTrackerActivity = Fixture.Create<DomainTrackerActivity>();
            var random = Fixture.Create<int>();

            // Act , Set
            domainTrackerActivity.SourceBlastID = random;

            // Assert
            domainTrackerActivity.SourceBlastID.ShouldBe(random);
            domainTrackerActivity.SourceBlastID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_SourceBlastID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var domainTrackerActivity = Fixture.Create<DomainTrackerActivity>();

            // Act , Set
            domainTrackerActivity.SourceBlastID = null;

            // Assert
            domainTrackerActivity.SourceBlastID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_SourceBlastID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameSourceBlastID = "SourceBlastID";
            var domainTrackerActivity = Fixture.Create<DomainTrackerActivity>();
            var propertyInfo = domainTrackerActivity.GetType().GetProperty(propertyNameSourceBlastID);

            // Act , Set
            propertyInfo.SetValue(domainTrackerActivity, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            domainTrackerActivity.SourceBlastID.ShouldBeNull();
            domainTrackerActivity.SourceBlastID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerActivity) => Property (SourceBlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_Class_Invalid_Property_SourceBlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSourceBlastID = "SourceBlastIDNotPresent";
            var domainTrackerActivity  = Fixture.Create<DomainTrackerActivity>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerActivity.GetType().GetProperty(propertyNameSourceBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_SourceBlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSourceBlastID = "SourceBlastID";
            var domainTrackerActivity  = Fixture.Create<DomainTrackerActivity>();
            var propertyInfo  = domainTrackerActivity.GetType().GetProperty(propertyNameSourceBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerActivity) => Property (TimeStamp) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_TimeStamp_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameTimeStamp = "TimeStamp";
            var domainTrackerActivity = Fixture.Create<DomainTrackerActivity>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = domainTrackerActivity.GetType().GetProperty(propertyNameTimeStamp);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(domainTrackerActivity, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerActivity) => Property (TimeStamp) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_Class_Invalid_Property_TimeStampNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTimeStamp = "TimeStampNotPresent";
            var domainTrackerActivity  = Fixture.Create<DomainTrackerActivity>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerActivity.GetType().GetProperty(propertyNameTimeStamp));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_TimeStamp_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTimeStamp = "TimeStamp";
            var domainTrackerActivity  = Fixture.Create<DomainTrackerActivity>();
            var propertyInfo  = domainTrackerActivity.GetType().GetProperty(propertyNameTimeStamp);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerActivity) => Property (UserAgent) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_UserAgent_Property_String_Type_Verify_Test()
        {
            // Arrange
            var domainTrackerActivity = Fixture.Create<DomainTrackerActivity>();
            domainTrackerActivity.UserAgent = Fixture.Create<string>();
            var stringType = domainTrackerActivity.UserAgent.GetType();

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

        #region General Getters/Setters : Class (DomainTrackerActivity) => Property (UserAgent) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_Class_Invalid_Property_UserAgentNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUserAgent = "UserAgentNotPresent";
            var domainTrackerActivity  = Fixture.Create<DomainTrackerActivity>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerActivity.GetType().GetProperty(propertyNameUserAgent));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerActivity_UserAgent_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUserAgent = "UserAgent";
            var domainTrackerActivity  = Fixture.Create<DomainTrackerActivity>();
            var propertyInfo  = domainTrackerActivity.GetType().GetProperty(propertyNameUserAgent);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (DomainTrackerActivity) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DomainTrackerActivity_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new DomainTrackerActivity());
        }

        #endregion

        #region General Constructor : Class (DomainTrackerActivity) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DomainTrackerActivity_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfDomainTrackerActivity = Fixture.CreateMany<DomainTrackerActivity>(2).ToList();
            var firstDomainTrackerActivity = instancesOfDomainTrackerActivity.FirstOrDefault();
            var lastDomainTrackerActivity = instancesOfDomainTrackerActivity.Last();

            // Act, Assert
            firstDomainTrackerActivity.ShouldNotBeNull();
            lastDomainTrackerActivity.ShouldNotBeNull();
            firstDomainTrackerActivity.ShouldNotBeSameAs(lastDomainTrackerActivity);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DomainTrackerActivity_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstDomainTrackerActivity = new DomainTrackerActivity();
            var secondDomainTrackerActivity = new DomainTrackerActivity();
            var thirdDomainTrackerActivity = new DomainTrackerActivity();
            var fourthDomainTrackerActivity = new DomainTrackerActivity();
            var fifthDomainTrackerActivity = new DomainTrackerActivity();
            var sixthDomainTrackerActivity = new DomainTrackerActivity();

            // Act, Assert
            firstDomainTrackerActivity.ShouldNotBeNull();
            secondDomainTrackerActivity.ShouldNotBeNull();
            thirdDomainTrackerActivity.ShouldNotBeNull();
            fourthDomainTrackerActivity.ShouldNotBeNull();
            fifthDomainTrackerActivity.ShouldNotBeNull();
            sixthDomainTrackerActivity.ShouldNotBeNull();
            firstDomainTrackerActivity.ShouldNotBeSameAs(secondDomainTrackerActivity);
            thirdDomainTrackerActivity.ShouldNotBeSameAs(firstDomainTrackerActivity);
            fourthDomainTrackerActivity.ShouldNotBeSameAs(firstDomainTrackerActivity);
            fifthDomainTrackerActivity.ShouldNotBeSameAs(firstDomainTrackerActivity);
            sixthDomainTrackerActivity.ShouldNotBeSameAs(firstDomainTrackerActivity);
            sixthDomainTrackerActivity.ShouldNotBeSameAs(fourthDomainTrackerActivity);
        }

        #endregion

        #region General Constructor : Class (DomainTrackerActivity) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DomainTrackerActivity_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var domainTrackerActivityId = -1;
            var domainTrackerId = -1;
            var pageURL = string.Empty;

            // Act
            var domainTrackerActivity = new DomainTrackerActivity();

            // Assert
            domainTrackerActivity.DomainTrackerActivityID.ShouldBe(domainTrackerActivityId);
            domainTrackerActivity.DomainTrackerID.ShouldBe(domainTrackerId);
            domainTrackerActivity.ProfileID.ShouldBeNull();
            domainTrackerActivity.PageURL.ShouldBe(pageURL);
            domainTrackerActivity.TimeStamp.ShouldBeNull();
            domainTrackerActivity.IPAddress.ShouldBeNull();
            domainTrackerActivity.UserAgent.ShouldBeNull();
            domainTrackerActivity.OS.ShouldBeNull();
            domainTrackerActivity.Browser.ShouldBeNull();
            domainTrackerActivity.ReferralURL.ShouldBeNull();
            domainTrackerActivity.SourceBlastID.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}