using System.Diagnostics.CodeAnalysis;
using System.Linq;
using EmailMarketing.API.Search;
using NUnit.Framework;
using Shouldly;

namespace EmailMarketing.API.Tests.Search
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SearchConfigurationTests
    {
        private static readonly string[] _configurationGroupNames =
        {
            "content",
            "message",
            "image",
            "imagefolder",
            "group",
            "filter",
            "customfield",
            "customer",
            "folder",
            "simpleblast",
            "simpleblastv2",
        };

        [Test]
        public void Library_GetConfigurations_ContainsCorrectNumberOfConfigurationGroups()
        {
            // Arrange
            var expectedNumberOfConfigurationGroups = _configurationGroupNames.Length;

            // Act
            var configurationGroups = SearchConfiguration.Library;

            // Assert
            configurationGroups.ShouldSatisfyAllConditions(
                () => configurationGroups.ShouldNotBeNull(),
                () => configurationGroups.Count.ShouldBe(expectedNumberOfConfigurationGroups));
        }

        [Test]
        public void Library_GetConfigurations_ContainsCorrectConfigurationGroups()
        {
            // Arrange && Act
            var configurationGroups = SearchConfiguration.Library;

            // Assert
            configurationGroups.ShouldSatisfyAllConditions(
                () => configurationGroups.ShouldNotBeNull(),
                () => _configurationGroupNames.ShouldBeSubsetOf(configurationGroups.Select(x => x.Key)));
        }
    }
}
