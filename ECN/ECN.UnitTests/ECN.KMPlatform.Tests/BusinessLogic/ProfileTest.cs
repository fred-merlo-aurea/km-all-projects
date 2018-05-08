using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;
using Shouldly;

using ProfileObject = KMPlatform.BusinessLogic.Profile;
using ProfileEntity = KMPlatform.Entity.Profile;

namespace ECN.KMPlatform.Tests.BusinessLogic
{
    [TestFixture]
    public class ProfileTest
    {
        [Test]
        public void Search_SearchListEmpty_ReturnsEmpty()
        {
            // Arrange
            var searchList = new List<ProfileEntity>();
            var searchValue = "1";

            var profileObject = new ProfileObject();

            // Act
            var matches = profileObject.Search(searchValue, searchList);

            // Assert
            matches.ShouldNotBeNull();
            matches.Count().ShouldBe(0);
        }

        [Test]
        public void Search_SearchListContainsSearchValue_ReturnsFound()
        {
            // Arrange
            var profileFirstName = new ProfileEntity { FirstName = "John1" };
            var profileUserId = new ProfileEntity { FirstName = "912345" };
            var searchList = new List<ProfileEntity>()
            {
                profileFirstName,
                profileUserId
            };
            var searchValue = "1";

            var profileObject = new ProfileObject();

            // Act
            var matches = profileObject.Search(searchValue, searchList);

            // Assert
            matches.ShouldNotBeNull();
            matches.Count().ShouldBe(2);
            matches.ShouldContain(profileFirstName);
            matches.ShouldContain(profileUserId);
        }
    }
}
