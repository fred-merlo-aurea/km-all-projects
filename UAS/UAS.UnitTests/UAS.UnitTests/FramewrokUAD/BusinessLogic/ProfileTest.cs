using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAD.BusinessLogic;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.FramewrokUAD.BusinessLogic.Common;
using FrameworkUADEntity = FrameworkUAD.Entity;

namespace UAS.UnitTests.FramewrokUAD.BusinessLogic
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ProfileTest: Fake
    {
        [SetUp]
        public void Setup()
        {
            SetupFakes();
        }

        [Test]
        public void Search_SearchListEmpty_ReturnsEmpty()
        {
            // Arrange
            var searchList = new List<FrameworkUADEntity.Profile>();
            var searchValue = "1";

            var profile = new Profile();

            // Act
            var matches = profile.Search(searchValue, searchList);

            // Assert
            matches.ShouldNotBeNull();
            matches.Count.ShouldBe(0);
        }

        [Test]
        public void Search_SearchListContainsSearchValue_ReturnsFound()
        {
            // Arrange
            var profileFirstName = new FrameworkUADEntity.Profile { FirstName = "John1"};
            var profileUserId = new FrameworkUADEntity.Profile { FirstName = "912345"};
            var searchList = new List<FrameworkUADEntity.Profile>()
            {
                profileFirstName,
                profileUserId
            };
            var searchValue = "1";

            var profile = new Profile();

            // Act
            var matches = profile.Search(searchValue, searchList);

            // Assert
            matches.ShouldNotBeNull();
            matches.Count.ShouldBe(2);
            matches.ShouldContain(profileFirstName);
            matches.ShouldContain(profileUserId);
        }
    }
}
