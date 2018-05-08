using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace KM.Common.UnitTests.Functions
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class QueryStringTests
    {
        [Test]
        [TestCase("B=value", "BlastID", "value")]
        [TestCase("b=value", "BlastID", "value")]
        [TestCase("b=value,ignore", "BlastID", "value")]
        [TestCase("rEDIrectaPP=value", "BlastID", "value")]
        [TestCase("redirectapp=value", "BlastID", "value")]
        [TestCase("redirectapp=value,ignore", "BlastID", "value")]
        [TestCase("bid=value", "BlastID", "value")]
        [TestCase("bid=value,ignore", "BlastID", "value")]
        [TestCase("blastid=value", "BlastID", "value")]
        [TestCase("blastid=value,ignore", "BlastID", "value")]
        [TestCase("campaignitemid=value", "CampaignItemID", "value")]
        [TestCase("campaignitemid=value,ignore", "CampaignItemID", "value")]
        [TestCase("g=value", "GroupID", "value")]
        [TestCase("g=value,ignore", "GroupID", "value")]
        [TestCase("gid=value", "GroupID", "value")]
        [TestCase("gid=value,ignore", "GroupID", "value")]
        [TestCase("e=value", "EmailID", "value")]
        [TestCase("e=value,ignore", "EmailID", "value")]
        [TestCase("eid=value", "EmailID", "value")]
        [TestCase("eid=value,ignore", "EmailID", "value")]
        [TestCase("emailid=value", "EmailID", "value")]
        [TestCase("emailid=value,ignore", "EmailID", "value")]
        [TestCase("lid=value", "BlastLinkID", "value")]
        [TestCase("lid=value,ignore", "BlastLinkID", "value")]
        [TestCase("m=value", "SocialMediaID", "value")]
        [TestCase("m=value,ignore", "SocialMediaID", "value")]
        [TestCase("cism=value", "CampaignItemSocialMediaID", "value")]
        [TestCase("cism=value,ignore", "CampaignItemSocialMediaID", "value")]
        [TestCase("c=value", "CustomerID", "value")]
        [TestCase("c=value,ignore", "CustomerID", "value")]
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
    }
}
