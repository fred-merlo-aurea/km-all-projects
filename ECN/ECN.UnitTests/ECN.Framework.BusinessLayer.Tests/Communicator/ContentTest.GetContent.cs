using System.Collections.Generic;
using ECN_Framework_BusinessLayer.Communicator;
using KMPlatform.Entity;
using NUnit.Framework;
using ECN_Framework_Common.Objects;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using KM.Platform.Fakes;
using Shouldly;
using ECNEntities = ECN_Framework_Entities.Communicator;
using KMPlatformFakes = KMPlatform.BusinessLogic.Fakes;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using ECN_Framework_Common.Functions.Fakes;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    public partial class ContentTest
    {
        private const string DummyString = "DummyString";

        [Test]
        public void GetContent_OnNullParamaters_ReturnEmptyString()
        {
            // Arrange, Act	
            var actualResult = Content.GetContent(null, ContentTypeCode.FILE, false, null, null, null, null);

            // Assert
            actualResult.ShouldBeNullOrWhiteSpace();
        }

        [Test]
        [TestCase(ContentTypeCode.HTML)]
        [TestCase(ContentTypeCode.TEXT)]
        public void GetContent_DefaultSwitchCase_ReturnEmptyString(ContentTypeCode type)
        {
            // Arrange
            ShimContent.GetByContentIDInt32UserBoolean = (id, usr, child) =>
            {
                return new ECNEntities::Content
                {
                    ContentTypeCode = DummyString,
                };
            };
            var user = new User();
            
            // Act	
            var actualResult = Content.GetContent(1, type, false, user, 1, 1, null);

            // Assert
            actualResult.ShouldBeNullOrWhiteSpace();
        }

        [Test]
        [TestCase(ContentTypeCode.HTML, "html", true, true)]
        [TestCase(ContentTypeCode.HTML, "html", true, false)]
        [TestCase(ContentTypeCode.HTML, "html", false, false)]
        [TestCase(ContentTypeCode.HTML, "text", true, false)]
        [TestCase(ContentTypeCode.HTML, "feed", true, false)]
        [TestCase(ContentTypeCode.TEXT, "html", false, false)]
        [TestCase(ContentTypeCode.TEXT, "text", true, false)]
        [TestCase(ContentTypeCode.TEXT, "feed", true, false)]
        public void GetContent_MultipleValues_RetursString(ContentTypeCode type, string contentTypeCode, bool isMobile, bool contentMobileIsEmpty)
        {
            // Arrange
            ShimContent.GetByContentIDInt32UserBoolean = (id, usr, child) =>
            {
                return new ECNEntities::Content
                {
                    ContentTypeCode = contentTypeCode,
                    ContentMobile = contentMobileIsEmpty ? string.Empty : DummyString,
                    ContentSource = DummyString,
                    ContentText = DummyString,
                    ContentURL = DummyString
                };
            };
            var user = new User();
            ShimHTTPFunctions.GetWebFeedString = (str) => DummyString;

            // Act	
            var actualResult = Content.GetContent(1, type, isMobile, user, 1, 1, null);

            // Assert
            actualResult.ShouldNotBeNullOrWhiteSpace();
            actualResult.ShouldContain(DummyString);
        }

        [Test]
        public void GetContent_NoAccessCheck_OnNullParamaters_ReturnEmptyString()
        {
            // Arrange, Act	
            var actualResult = Content.GetContent_NoAccessCheck(null, ContentTypeCode.FILE, false, null, null, null);

            // Assert
            actualResult.ShouldBeNullOrWhiteSpace();
        }

        [Test]
        [TestCase(ContentTypeCode.HTML)]
        [TestCase(ContentTypeCode.TEXT)]
        public void GetContent_NoAccessCheck_DefaultSwitchCase_ReturnEmptyString(ContentTypeCode type)
        {
            // Arrange
            ShimContent.GetByContentID_NoAccessCheckInt32Boolean = (id, child) => new ECNEntities::Content
            {
                ContentTypeCode = DummyString,
                CustomerID = 1
            };

            // Act	
            var actualResult = Content.GetContent_NoAccessCheck(1, type, false, 1, 1, null);

            // Assert
            actualResult.ShouldBeNullOrWhiteSpace();
        }

        [Test]
        [TestCase(ContentTypeCode.HTML, "html", true, true)]
        [TestCase(ContentTypeCode.HTML, "html", true, false)]
        [TestCase(ContentTypeCode.HTML, "html", false, false)]
        [TestCase(ContentTypeCode.HTML, "text", true, false)]
        [TestCase(ContentTypeCode.HTML, "feed", true, false)]
        [TestCase(ContentTypeCode.TEXT, "html", false, false)]
        [TestCase(ContentTypeCode.TEXT, "text", true, false)]
        [TestCase(ContentTypeCode.TEXT, "feed", true, false)]
        public void GetContent_NoAccessCheck_MultipleValues_RetursString(ContentTypeCode type, string contentTypeCode, bool isMobile, bool contentMobileIsEmpty)
        {
            // Arrange
            ShimContent.GetByContentID_NoAccessCheckInt32Boolean = (id, child) => new ECNEntities::Content
            {
                ContentTypeCode = contentTypeCode,
                CustomerID = 1,
                ContentMobile = contentMobileIsEmpty ? string.Empty : DummyString,
                ContentSource = DummyString,
                ContentText = DummyString,
                ContentURL = DummyString
            };
            ShimHTTPFunctions.GetWebFeedString = (str) => DummyString;

            // Act	
            var actualResult = Content.GetContent_NoAccessCheck(1, type, isMobile, 1, 1, null);

            // Assert
            actualResult.ShouldNotBeNullOrWhiteSpace();
            actualResult.ShouldContain(DummyString);
        }
    }
}
