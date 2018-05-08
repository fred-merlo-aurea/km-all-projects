using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;
using Shouldly;
using ECN_Framework_Entities.Salesforce;

namespace ECN_Framework_EntitiesTests.SalesForce
{
    /// <summary>
    /// Unit Test for <see cref="SF_Utilities.GetStateAbbr"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SF_UtilitiesTest
    {
        private const string XmlRoot = "<sObjects xmlns=\"http://www.force.com/2009/06/asyncapi/dataload\">";
        private const string XmlEnd = "</sObjects>";

        [Test]
        public void GetStateAbbr_NotFoundState_ReturnString()
        {
            // Arrange
            var state = "Teststing";

            // Act
            var actualRes = SF_Utilities.GetStateAbbr(state);

            // Assert
            actualRes.ShouldBe(state);
        }

        [Test]
        [TestCase("alabama", "AL")]
        [TestCase("alaska", "AK")]
        [TestCase("new brunswick", "NB")]
        [TestCase("ontario", "ON")]
        [TestCase("alberta", "AB")]
        [TestCase("quebec", "QC")]
        [TestCase("manitoba", "MB")]
        [TestCase("Washington", "WA")]
        [TestCase("texas", "TX")]
        [TestCase("tennessee", "TN")]
        [TestCase("arizona", "AZ")]
        [TestCase("arkansas", "AR")]
        [TestCase("california", "CA")]
        [TestCase("colorado", "CO")]
        [TestCase("connecticut", "CT")]
        [TestCase("delaware", "DE")]
        [TestCase("florida", "FL")]
        [TestCase("georgia", "FA")]
        [TestCase("hawaii", "HI")]
        [TestCase("idaho", "ID")]
        [TestCase("illinois", "IL")]
        [TestCase("indiana", "IN")]
        [TestCase("iowa", "IA")]
        [TestCase("kansas", "KS")]
        [TestCase("kentucky", "KY")]
        [TestCase("louisiana", "LA")]
        [TestCase("maine", "ME")]
        [TestCase("maryland", "MD")]
        [TestCase("massachusetts", "MA")]
        [TestCase("michigan", "MI")]
        [TestCase("minnesota", "MN")]
        [TestCase("mississippi", "MS")]
        [TestCase("missouri", "MO")]
        [TestCase("montana", "MT")]
        [TestCase("nebraska", "NE")]
        [TestCase("nevada", "NV")]
        [TestCase("new hampshire", "NH")]
        [TestCase("new jersey", "NJ")]
        [TestCase("new mexico", "NM")]
        [TestCase("new york", "NY")]
        [TestCase("north carolina", "NC")]
        [TestCase("north dakota", "ND")]
        [TestCase("ohio", "OH")]
        [TestCase("oklahoma", "OK")]
        [TestCase("oregon", "OR")]
        [TestCase("pennsylvania", "PA")]
        [TestCase("rhode island", "RI")]
        [TestCase("south carolina", "SC")]
        [TestCase("south dakota", "SD")]
        [TestCase("utah", "UT")]
        [TestCase("vermont", "VT")]
        [TestCase("virginia", "VA")]
        [TestCase("west virginia", "WV")]
        [TestCase("wisconsin", "WI")]
        [TestCase("wyoming", "WY")]
        [TestCase("british columbia", "BC")]
        [TestCase("newfoundland and labrador", "NL")]
        [TestCase("nova scotia", "NS")]
        [TestCase("northwest territories", "NT")]
        [TestCase("prince edward island", "PE")]
        [TestCase("saskatchewan", "SK")]
        [TestCase("yukon", "YT")]
        [TestCase("nunavut", "NU")]
        public void GetStateAbbr_ValidCall_ReturnString(string state, string stateAbbr)
        {
            // Arrange - Act
            var actualRes = SF_Utilities.GetStateAbbr(state);

            // Assert
            actualRes.ShouldBe(stateAbbr);
        }

        [Test]
        public void ExcludedCharacters_ValidCall_ReturnExpectedList()
        {
            // Arrange
            var expected = new[] { ",", "'", "%", "*", "#", "--", "&", "<", ">", ";", "xp_", "_", "/*", "*/" };

            // Act
            var result = SF_Utilities.ExcludedCharacters();

            // Assert
            result.ShouldAllBe(x => expected.Any(y => y == x));
        }

        [Test]
        public void GetXMLForOptOutJob_PassEmptyDictionary_ReturnsEmptyXml()
        {
            // Arrange
            var dic = new Dictionary<string, string>();

            // Act
            var result = SF_Utilities.GetXMLForOptOutJob(dic);

            // Assert
            result.ShouldSatisfyAllConditions(
              () => result.ShouldStartWith(XmlRoot),
              () => result.ShouldEndWith(XmlEnd));
        }

        [Test]
        public void GetXMLForOptOutJob_PassDictionaryWithKeys_ReturnsValidXml()
        {
            // Arrange
            const string Key = "Key";
            const string Value = "Value";
            const string emailTag = "<HasOptedOutOfEmail>true</HasOptedOutOfEmail>";
            var idTagValue = $"<id>{Key}</id>";
            var dic = new Dictionary<string, string>
            {
                { Key, Value }
            };

            // Act
            var result = SF_Utilities.GetXMLForOptOutJob(dic);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldStartWith(XmlRoot),
                () => result.ShouldContain(idTagValue),
                () => result.ShouldNotContain(Value),
                () => result.ShouldContain(emailTag));
        }

        [Test]
        public void GetXMLForMasterSuppressJob_PassEmptyDictionary_ReturnsEmptyXml()
        {
            // Arrange
            var dic = new Dictionary<string, string>();

            // Act
            var result = SF_Utilities.GetXMLForMasterSuppressJob(dic);

            // Assert
            result.ShouldSatisfyAllConditions(
              () => result.ShouldStartWith(XmlRoot),
              () => result.ShouldEndWith(XmlEnd));
        }

        [Test]
        public void GetXMLForMasterSuppressJob_PassDictionaryWithKeys_ReturnsValidXml()
        {
            // Arrange
            const string Key = "Key";
            const string Value = "Value";
            const string MasterSuppressedTag = "<Master_Suppressed__c>true</Master_Suppressed__c>";
            var idTagValue = $"<id>{Key}</id>";
            var dic = new Dictionary<string, string>
            {
                { Key, Value }
            };

            // Act
            var result = SF_Utilities.GetXMLForMasterSuppressJob(dic);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldStartWith(XmlRoot),
                () => result.ShouldContain(idTagValue),
                () => result.ShouldNotContain(Value),
                () => result.ShouldContain(MasterSuppressedTag));
        }
    }
}
