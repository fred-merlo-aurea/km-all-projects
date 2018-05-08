using System;
using NUnit.Framework;
using ECN_Framework_Entities.FormDesigner;
using Shouldly;

namespace ECN_Framework_Entities.FormDesigner.Tests
{
    [TestFixture]
    public class RuleTest
    {
        [Test]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {
            // Arrange    
            int rule_Seq_ID = -1;        

            // Act
            Rule rule = new Rule();    

            // Assert
            rule.Rule_Seq_ID.ShouldBe(rule_Seq_ID);
            rule.OverwritePostValue.ShouldBeEmpty();
            rule.RequestQueryValue.ShouldBeEmpty();
        }
    }
}