using System.Collections.Generic;
using ECN_Framework_BusinessLayer.Communicator;
using KMPlatform.Entity;
using NUnit.Framework;
using ECN_Framework_Common.Objects;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using KM.Platform.Fakes;
using Shouldly;
using ECNEntities = ECN_Framework_Entities.Communicator;
using KMPlatformFakes = KMPlatform.BusinessLogic.Fakes;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    public partial class ContentTest
    {
        [Test]
        public void Validate_WithCustomerIdNotNull_ThrowsEcnExceptionWithErrorResult()
        {
            // Arrange
            SetUpFakes();
            var content = GetDefultContent();
            var user = new User();

            // Act
            var ecnExp = Should.Throw<ECNException>(() => Content.Validate(content, user));

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business);
            ecnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ContentTitle is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("FolderID is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("CustomerID is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Sharing is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("LockedFlag is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains($"UT exception \"{SampleFeed2}\"")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains($"UT exception \"{SampleFeed1}\"")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains($"UT exception \"{DynamicFeed3}\"")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Invalid Dynamic Tag - 1")));
        }

        [Test]
        public void Validate_WithFolderIdNotNull_ThrowsEcnExceptionWithErrorResult()
        {
            // Arrange
            SetUpFakes();
            var content = GetDefultContent();
            content.ContentTitle = string.Empty;
            content.FolderID = 1;
            content.CreatedUserID = 1;
            var user = new User();

            // Act
            var ecnExp = Should.Throw<ECNException>(() => { Content.Validate(content, user); });

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business);
            ecnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ContentTitle is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ContentTitle cannot be empty")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("CustomerID is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("CreatedUserID is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Sharing is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("LockedFlag is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains($"UT exception \"{SampleFeed2}\"")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains($"UT exception \"{SampleFeed1}\"")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains($"UT exception \"{DynamicFeed3}\"")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Invalid Dynamic Tag - 1")));
        }

        [Test]
        public void Validate_WithContextTextEmpty_ThrowsEcnExceptionWithErrorResult()
        {
            // Arrange
            SetUpFakes();
            var content = GetDefultContent();
            content.ContentText = string.Empty;
            content.FolderID = 1;
            content.CreatedUserID = 1;
            var user = new User();

            // Act
            var ecnExp = Should.Throw<ECNException>(() => { Content.Validate(content, user); });

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business);
            ecnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ContentTitle is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ContentTitle cannot be empty")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("CustomerID is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("CreatedUserID is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Sharing is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("LockedFlag is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ContentText is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains($"UT exception \"{SampleFeed2}\"")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains($"UT exception \"{DynamicFeed3}\"")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Invalid Dynamic Tag - 1")));
        }

        [Test]
        public void Validate_WithContentSourceEmpty_ThrowsEcnExceptionWithErrorResult()
        {
            // Arrange
            SetUpFakes();
            var content = GetDefultContent();
            content.ContentTypeCode = ContentTypeCode.HTML.ToString();
            content.ContentSource = string.Empty;
            content.ContentMobile = string.Empty;
            content.FolderID = 1;
            content.CreatedUserID = 1;
            var user = new User();

            // Act
            var ecnExp = Should.Throw<ECNException>(() => { Content.Validate(content, user); });

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business);
            ecnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ContentTitle is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ContentTitle cannot be empty")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("CustomerID is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("CreatedUserID is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Sharing is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("LockedFlag is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ContentSource is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ContentMobile is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains($"UT exception \"{SampleFeed1}\"")));
        }

        [Test]
        public void Validate_WithContentSMSEmpty_ThrowsEcnExceptionWithErrorResult()
        {
            // Arrange
            SetUpFakes();
            var content = GetDefultContent();
            content.ContentTypeCode = ContentTypeCode.SMS.ToString();
            content.ContentSource = string.Empty;
            content.ContentMobile = string.Empty;
            content.ContentSMS = string.Empty;
            content.FolderID = 1;
            content.CreatedUserID = 1;
            var user = new User();

            // Act
            var ecnExp = Should.Throw<ECNException>(() => { Content.Validate(content, user); });

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business);
            ecnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ContentTitle is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ContentTitle cannot be empty")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("CustomerID is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("CreatedUserID is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Sharing is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("LockedFlag is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ContentSMS is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains($"UT exception \"{SampleFeed1}\"")));
        }

        [Test]
        public void Validate_WithInValidTitleNameAndContentSharingHasValue_ThrowsEcnExceptionWithErrorResult()
        {
            // Arrange
            SetUpFakes();
            var content = GetDefultContent();
            content.ContentTitle = "SampleTitle#";
            content.ContentID = 1;
            content.FolderID = 1;
            content.Sharing = "Y";
            content.UpdatedUserID = null;
            var user = new User();

            // Act
            var ecnExp = Should.Throw<ECNException>(() => { Content.Validate(content, user); });

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business);
            ecnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ContentTitle already exists in this folder")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ContentTitle has invalid characters")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("CustomerID is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("UpdatedUserID is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Sharing is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("LockedFlag is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains($"UT exception \"{SampleFeed2}\"")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains($"UT exception \"{SampleFeed1}\"")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains($"UT exception \"{DynamicFeed3}\"")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Invalid Dynamic Tag - 1")));
        }

        [Test]
        public void Validate_CustomerIdNull_ThrowsEcnExceptionWithErrorResult()
        {
            // Arrange
            SetUpFakes();
            var content = GetDefultContent();
            content.CustomerID = null;
            content.ContentSource = string.Empty;
            content.ContentMobile = string.Empty;
            content.ContentText = string.Empty;
            var user = new User();

            // Act
            var ecnExp = Should.Throw<ECNException>(() => { Content.Validate(content, user); });

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business);
            ecnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ContentTitle is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("CustomerID is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("LockedFlag is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ContentText is invalid")));
        }

        [Test]
        public void Validate_WithValidContent_DoesNotThrowException()
        {
            // Arrange
            SetUpFakes();
            var content = new ECNEntities.Content
            {
                ContentTitle = "SampleTitle",
                CustomerID = 1,
                FolderID = 1,
                Sharing = "N",
                LockedFlag = "Y",

            };
            var user = new User();
            ShimContent.ExistsInt32StringInt32Int32 = (u, m, n, f) => false;
            ShimCustomer.ExistsInt32 = (c) => true;

            // Act
            Should.NotThrow(() => { Content.Validate(content, user); });
        }

        private void SetUpFakes()
        {
            ShimRSSFeed.GetByFeedNameStringInt32 = (f, u) => new ECNEntities.RSSFeed { };
            ShimRSSFeed.VerifyString = (u) => throw new ECNException(new List<ECNError>
            {
                new ECNError
                {
                    ErrorMessage = "UT exception"
                }
            });
            ShimCustomer.ExistsInt32 = (c) => false;
            ShimFolder.ExistsInt32Int32EnumsFolderTypes = (c, u, f) => true;
            ShimUser.IsSystemAdministratorUser = (u) => false;
            ShimUser.IsChannelAdministratorUser = (u) => false;
            ShimUser.IsAdministratorUser = (u) => false;
            KMPlatformFakes.ShimUser.GetByUserIDInt32Boolean = (c, b) => new User();
            KMPlatformFakes.ShimUser.ExistsInt32Int32 = (c, b) => false;
            ShimContent.ExistsInt32StringInt32Int32 = (u, m, n, f) => true;
            ShimDynamicTag.GetByTagStringUserBoolean = (t, u, b) => null;
        }

        private ECNEntities.Content GetDefultContent()
        {
            var content = new ECNEntities.Content
            {
                LockedFlag = string.Empty,
                ContentTypeCode = ContentTypeCode.TEXT.ToString(),
                CustomerID = 1,
                CreatedUserID = null,
                Sharing = string.Empty,
                ContentText = $@"ECN.RSSFEED.{SampleFeed1}.ECN.RSSFEED",
                ContentSource = $@"ECN.RSSFEED.{SampleFeed2}.ECN.RSSFEED",
                ContentMobile = $@"ECN.RSSFEED.{DynamicFeed3}.ECN.RSSFEED",

            };
            return content;
        }
    }
}
