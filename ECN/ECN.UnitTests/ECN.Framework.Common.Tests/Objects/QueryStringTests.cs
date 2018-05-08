using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ECN_Framework_Common.Objects;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Common.Tests.Objects
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class QueryStringTests
    {
        [Test]
        [TestCase("B=value", "BlastID", "value")]
        [TestCase("b=value", "BlastID", "value")]
        [TestCase("b=value,ignore", "BlastID", "value")]
        [TestCase("bid=value", "BlastID", "value")]
        [TestCase("bid=value,ignore", "BlastID", "value")]
        [TestCase("blastid=value", "BlastID", "value")]
        [TestCase("bLaSTId=value", "BlastID", "value")]
        [TestCase("blastid=value,ignore", "BlastID", "value")]
        [TestCase("campaignitemid=value", "CampaignItemID", "value")]
        [TestCase("campaignitemid=value,ignore", "CampaignItemID", "value")]
        [TestCase("s=value", "Subscribe", "value")]
        [TestCase("s=value,ignore", "Subscribe", "value")]
        [TestCase("f=value", "Format", "value")]
        [TestCase("f=value,ignore", "Format", "value")]
        [TestCase("g=value", "GroupID", "value")]
        [TestCase("g=value,ignore", "GroupID", "value")]
        [TestCase("gid=value", "GroupID", "value")]
        [TestCase("gid=value,ignore", "GroupID", "value")]
        [TestCase("e=value", "EmailID", "value")]
        [TestCase("e=email@email.com", "EmailAddress", "email@email.com")]
        [TestCase("e=value,ignore", "EmailID", "value")]
        [TestCase("ei=value", "EmailID", "value")]
        [TestCase("ei=value,ignore", "EmailID", "value")]
        [TestCase("eid=value", "EmailID", "value")]
        [TestCase("eid=value,ignore", "EmailID", "value")]
        [TestCase("emailid=value", "EmailID", "value")]
        [TestCase("emailid=value,ignore", "EmailID", "value")]
        [TestCase("lid=value", "BlastLinkID", "value")]
        [TestCase("lid=value,ignore", "BlastLinkID", "value")]
        [TestCase("m=value", "SocialMediaID", "value")]
        [TestCase("m=value,ignore", "SocialMediaID", "value")]
        [TestCase("c=value", "CustomerID", "value")]
        [TestCase("c=value,ignore", "CustomerID", "value")]
        [TestCase("preview=value", "Preview", "value")]
        [TestCase("sfid=value", "SmartFormID", "value")]
        [TestCase("url=value", "URL", "value")]
        [TestCase("layoutid=value", "LayoutID", "value")]
        [TestCase("monitor=value", "Monitor", "value")]
        [TestCase("bcid=value", "BaseChannelID", "value")]
        [TestCase("newemail=value", "NewEmail", "value")]
        [TestCase("oldemail=value", "OldEmail", "value")]
        [TestCase("edid=value", "EmailDirectID", "value")]
        [TestCase("c=value1&b=value2&emailid=value3", "CustomerID", "value1", 3)]
        public void GetECNParameters_DifferentInputs_ReturnsValidParametersList(
            string input,
            string expectedParamName,
            string expectedParamValue,
            int expectedNumberOfParams = 1)
        {
            // Arrange & Act
            var actualResult = QueryString.GetECNParameters(input);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ParameterList.ShouldNotBeNull(),
                () => actualResult.ParameterList.Count.ShouldBe(expectedNumberOfParams),
                () => actualResult.ParameterList
                    .First()
                    .Parameter
                    .ToString()
                    .ShouldBe(expectedParamName, StringCompareShould.IgnoreCase),
                () => actualResult.ParameterList
                    .First()
                    .ParameterValue
                    .ShouldBe(expectedParamValue, StringCompareShould.IgnoreCase));
        }

        [Test]
        public void GetECNParameters_NullStringToParse_ReturnsNullPrameterList()
        {
            // Arrange
            var expectedQueryString = new QueryString();
            expectedQueryString.ParameterList = new List<QueryStringParameters>();

            // Act
            var result = QueryString.GetECNParameters(null);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ParameterList.ShouldBe(expectedQueryString.ParameterList),
                () => result.StringToParse.ShouldBe(expectedQueryString.StringToParse)
            );
        }
    }
}