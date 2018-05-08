using System;
using NUnit.Framework;
using ECN_Framework_Entities.Publisher;
using Shouldly;

namespace ECN_Framework_Entities.Publisher.Tests
{
    [TestFixture]
    public class RuleTest
    {
        [Test]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {
            // Arrange    
            int ruleID = -1;
            string ruleName = string.Empty;
            int publicationID = -1;
            int editionID = -1;
            string whereClause = string.Empty;        

            // Act
            Rule rule = new Rule();    

            // Assert
            rule.RuleID.ShouldBe(ruleID);
            rule.RuleName.ShouldBe(ruleName);
            rule.PublicationID.ShouldBe(publicationID);
            rule.EditionID.ShouldBe(editionID);
            rule.WhereClause.ShouldBe(whereClause);
            rule.CreatedUserID.ShouldBeNull();
            rule.CreatedDate.ShouldBeNull();
            rule.UpdatedUserID.ShouldBeNull();
            rule.UpdatedDate.ShouldBeNull();
            rule.IsDeleted.ShouldBeNull();
            rule.CustomerID.ShouldBeNull();
        }
    }
}