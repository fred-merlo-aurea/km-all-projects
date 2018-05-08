using System;
using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using ECN_Framework_Entities.Communicator.ContentReplacement;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator.ContentReplacement
{
    [TestFixture]
    public class RSSFeedByCampaignItemTokenReplacementContextTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (RSSFeedByCampaignItemTokenReplacementContext) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeedByCampaignItemTokenReplacementContext_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var rSSFeedByCampaignItemTokenReplacementContext = Fixture.Create<RSSFeedByCampaignItemTokenReplacementContext>();
            var campiagnItemId = Fixture.Create<int>();
            var feedName = Fixture.Create<string>();
            var rSSFeed = Fixture.Create<ECN_Framework_Entities.Communicator.RSSFeed>();

            // Act
            rSSFeedByCampaignItemTokenReplacementContext.CampiagnItemID = campiagnItemId;
            rSSFeedByCampaignItemTokenReplacementContext.FeedName = feedName;
            rSSFeedByCampaignItemTokenReplacementContext.RSSFeed = rSSFeed;

            // Assert
            rSSFeedByCampaignItemTokenReplacementContext.CampiagnItemID.ShouldBe(campiagnItemId);
            rSSFeedByCampaignItemTokenReplacementContext.FeedName.ShouldBe(feedName);
            rSSFeedByCampaignItemTokenReplacementContext.RSSFeed.ShouldBe(rSSFeed);
        }

        #endregion

        #region General Getters/Setters : Class (RSSFeedByCampaignItemTokenReplacementContext) => Property (CampiagnItemID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeedByCampaignItemTokenReplacementContext_CampiagnItemID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var rSSFeedByCampaignItemTokenReplacementContext = Fixture.Create<RSSFeedByCampaignItemTokenReplacementContext>();
            rSSFeedByCampaignItemTokenReplacementContext.CampiagnItemID = Fixture.Create<int>();
            var intType = rSSFeedByCampaignItemTokenReplacementContext.CampiagnItemID.GetType();

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

        #region General Getters/Setters : Class (RSSFeedByCampaignItemTokenReplacementContext) => Property (CampiagnItemID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeedByCampaignItemTokenReplacementContext_Class_Invalid_Property_CampiagnItemIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampiagnItemID = "CampiagnItemIDNotPresent";
            var rSSFeedByCampaignItemTokenReplacementContext  = Fixture.Create<RSSFeedByCampaignItemTokenReplacementContext>();

            // Act , Assert
            Should.NotThrow(() => rSSFeedByCampaignItemTokenReplacementContext.GetType().GetProperty(propertyNameCampiagnItemID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeedByCampaignItemTokenReplacementContext_CampiagnItemID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampiagnItemID = "CampiagnItemID";
            var rSSFeedByCampaignItemTokenReplacementContext  = Fixture.Create<RSSFeedByCampaignItemTokenReplacementContext>();
            var propertyInfo  = rSSFeedByCampaignItemTokenReplacementContext.GetType().GetProperty(propertyNameCampiagnItemID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (RSSFeedByCampaignItemTokenReplacementContext) => Property (FeedName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeedByCampaignItemTokenReplacementContext_FeedName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var rSSFeedByCampaignItemTokenReplacementContext = Fixture.Create<RSSFeedByCampaignItemTokenReplacementContext>();
            rSSFeedByCampaignItemTokenReplacementContext.FeedName = Fixture.Create<string>();
            var stringType = rSSFeedByCampaignItemTokenReplacementContext.FeedName.GetType();

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

        #region General Getters/Setters : Class (RSSFeedByCampaignItemTokenReplacementContext) => Property (FeedName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeedByCampaignItemTokenReplacementContext_Class_Invalid_Property_FeedNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFeedName = "FeedNameNotPresent";
            var rSSFeedByCampaignItemTokenReplacementContext  = Fixture.Create<RSSFeedByCampaignItemTokenReplacementContext>();

            // Act , Assert
            Should.NotThrow(() => rSSFeedByCampaignItemTokenReplacementContext.GetType().GetProperty(propertyNameFeedName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeedByCampaignItemTokenReplacementContext_FeedName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFeedName = "FeedName";
            var rSSFeedByCampaignItemTokenReplacementContext  = Fixture.Create<RSSFeedByCampaignItemTokenReplacementContext>();
            var propertyInfo  = rSSFeedByCampaignItemTokenReplacementContext.GetType().GetProperty(propertyNameFeedName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (RSSFeedByCampaignItemTokenReplacementContext) => Property (RSSFeed) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeedByCampaignItemTokenReplacementContext_RSSFeed_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameRSSFeed = "RSSFeed";
            var rSSFeedByCampaignItemTokenReplacementContext = Fixture.Create<RSSFeedByCampaignItemTokenReplacementContext>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rSSFeedByCampaignItemTokenReplacementContext.GetType().GetProperty(propertyNameRSSFeed);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(rSSFeedByCampaignItemTokenReplacementContext, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (RSSFeedByCampaignItemTokenReplacementContext) => Property (RSSFeed) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeedByCampaignItemTokenReplacementContext_Class_Invalid_Property_RSSFeedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRSSFeed = "RSSFeedNotPresent";
            var rSSFeedByCampaignItemTokenReplacementContext  = Fixture.Create<RSSFeedByCampaignItemTokenReplacementContext>();

            // Act , Assert
            Should.NotThrow(() => rSSFeedByCampaignItemTokenReplacementContext.GetType().GetProperty(propertyNameRSSFeed));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeedByCampaignItemTokenReplacementContext_RSSFeed_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRSSFeed = "RSSFeed";
            var rSSFeedByCampaignItemTokenReplacementContext  = Fixture.Create<RSSFeedByCampaignItemTokenReplacementContext>();
            var propertyInfo  = rSSFeedByCampaignItemTokenReplacementContext.GetType().GetProperty(propertyNameRSSFeed);

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