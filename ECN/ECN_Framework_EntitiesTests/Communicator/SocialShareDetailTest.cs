using System;
using NUnit.Framework;
using ECN_Framework_Entities.Communicator;
using Shouldly;

namespace ECN_Framework_Entities.Communicator.Tests
{
    [TestFixture]
    public class SocialShareDetailTest
    {
        [Test]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {
            // Arrange    
            int socialShareDetailID = -1;
            int contentID = -1;        

            // Act
            SocialShareDetail socialShareDetail = new SocialShareDetail();    

            // Assert
            socialShareDetail.SocialShareDetailID.ShouldBe(socialShareDetailID);
            socialShareDetail.ContentID.ShouldBe(contentID);
            socialShareDetail.Title.ShouldBeNull();
            socialShareDetail.Description.ShouldBeNull();
            socialShareDetail.Image.ShouldBeNull();
            socialShareDetail.CreatedUserID.ShouldBeNull();
            socialShareDetail.CreatedDate.ShouldBeNull();
            socialShareDetail.UpdatedUserID.ShouldBeNull();
            socialShareDetail.UpdatedDate.ShouldBeNull();
            socialShareDetail.IsDeleted.ShouldBeNull();
        }
    }
}