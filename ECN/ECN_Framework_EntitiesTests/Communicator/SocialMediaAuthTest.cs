using System;
using NUnit.Framework;
using ECN_Framework_Entities.Communicator;
using Shouldly;

namespace ECN_Framework_Entities.Communicator.Tests
{
    [TestFixture]
    public class SocialMediaAuthTest
    {
        [Test]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {
            // Arrange    
            int socialMediaAuthID = -1;
            int socialMediaID = -1;
            int customerID = -1;
            string access_Token = string.Empty;
            string userID = string.Empty;
            bool? isDeleted = false;
            string access_Secret = string.Empty;
            string profileName = string.Empty;        

            // Act
            SocialMediaAuth socialMediaAuth = new SocialMediaAuth();    

            // Assert
            socialMediaAuth.SocialMediaAuthID.ShouldBe(socialMediaAuthID);
            socialMediaAuth.SocialMediaID.ShouldBe(socialMediaID);
            socialMediaAuth.CustomerID.ShouldBe(customerID);
            socialMediaAuth.Access_Token.ShouldBe(access_Token);
            socialMediaAuth.UserID.ShouldBe(userID);
            socialMediaAuth.CreatedDate.ShouldBeNull();
            socialMediaAuth.CreatedUserID.ShouldBeNull();
            socialMediaAuth.UpdatedDate.ShouldBeNull();
            socialMediaAuth.UpdatedUserID.ShouldBeNull();
            socialMediaAuth.IsDeleted.ShouldBe(isDeleted);
            socialMediaAuth.Access_Secret.ShouldBe(access_Secret);
            socialMediaAuth.ProfileName.ShouldBe(profileName);
        }
    }
}