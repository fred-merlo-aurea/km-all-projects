using System;
using NUnit.Framework;
using ECN_Framework_Entities.Communicator;
using Shouldly;

namespace ECN_Framework_Entities.Communicator.Tests
{
    [TestFixture]
    public class SocialMediaTest
    {
        [Test]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {
            // Arrange    
            int socialMediaID = -1;
            string displayName = string.Empty;
            string matchString = string.Empty;
            string imagePath = string.Empty;
            string shareLink = string.Empty;
            string reportImagePath = string.Empty;        

            // Act
            SocialMedia socialMedia = new SocialMedia();    

            // Assert
            socialMedia.SocialMediaID.ShouldBe(socialMediaID);
            socialMedia.DisplayName.ShouldBe(displayName);
            socialMedia.IsActive.ShouldBeNull();
            socialMedia.MatchString.ShouldBe(matchString);
            socialMedia.ImagePath.ShouldBe(imagePath);
            socialMedia.ShareLink.ShouldBe(shareLink);
            socialMedia.CanShare.ShouldBeNull();
            socialMedia.CanPublish.ShouldBeNull();
            socialMedia.DateAdded.ShouldBeNull();
            socialMedia.ReportImagePath.ShouldBe(reportImagePath);
        }
    }
}