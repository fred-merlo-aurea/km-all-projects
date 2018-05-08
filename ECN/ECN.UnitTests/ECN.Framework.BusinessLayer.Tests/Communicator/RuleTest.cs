using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using ECN_Framework_Entities.Communicator;
using Shouldly;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KMPlatform.Entity;
using BusinessLayer = ECN_Framework_BusinessLayer.Communicator;

namespace ECN_Framework_BusinessLayer.Tests.Communicator
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class RuleTest
    {
        private IDisposable _shimObject;
        private PrivateObject _privateObjcet;
        private const string _MethodGenerateWhereClause = "GenerateWhereClause";

        [SetUp]
        public void SetUp()
        {
            _shimObject = ShimsContext.Create();
            _privateObjcet = new PrivateObject(typeof(BusinessLayer::Rule));
        }

        [TearDown]
        public void TearDown()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void GenerateWhereClause_OnEmptyRule_ReturnEmptyString()
        {
            // Arrange
            var rule = new Rule
            {
                RuleID = 1
            };
            var user = new User
            {
                UserID = 1
            };

            ShimRule.GetByRuleIDInt32UserBoolean = (id, usr, child) =>
            {
                return new Rule
                {
                    RuleConditionsList = new List<RuleCondition>()
                };
            };

            // Act	
            var actualResult = _privateObjcet.Invoke(_MethodGenerateWhereClause, BindingFlags.Static | BindingFlags.NonPublic , new object[] { rule, user }) as string;

            // Assert
            actualResult.ShouldBeNullOrWhiteSpace();
        }

        [Test]
        [TestCase(nameof(String), "Equals")]
        [TestCase(nameof(String), "Not Equals")]
        [TestCase(nameof(String), "Contains")]
        [TestCase(nameof(String), "Starts With")]
        [TestCase(nameof(String), "Ends With")]
        [TestCase(nameof(String), "Is Empty")]
        [TestCase(nameof(String), "Is Not Empty")]
        [TestCase("Number", "Equals")]
        [TestCase("Number", "Not Equals")]
        [TestCase("Number", "Greater than")]
        [TestCase("Number", "Less than")]
        [TestCase("Number", "Is Empty")]
        [TestCase("Number", "Is Not Empty")]
        [TestCase("Date", "Equals")]
        [TestCase("Date", "Not Equals")]
        [TestCase("Date", "Greater than")]
        [TestCase("Date", "Less than")]
        [TestCase("Date", "Is Empty")]
        [TestCase("Date", "Is Not Empty")]
        public void GenerateWhereClause_DataTypeAndComparatorHasDifferentValues_ReturnString(string dataType, string comparator)
        {
            // Arrange
            var rule = new Rule
            {
                RuleID = 1
            };
            var user = new User
            {
                UserID = 1
            };
            ShimRule.GetByRuleIDInt32UserBoolean = (id, usr, child) =>
            {
                return new Rule
                {
                    RuleConditionsList = new List<RuleCondition>
                    {
                        new RuleCondition
                        {
                            DataType = dataType,
                            Comparator = comparator,
                            IsDeleted = false,
                            Field = "TestField",
                            Value = "TestValue"
                        }
                    }
                };
            };

            // Act	
            var actualResult = _privateObjcet.Invoke(_MethodGenerateWhereClause, BindingFlags.Static | BindingFlags.NonPublic, new object[] { rule, user }) as string;

            // Assert
            actualResult.ShouldNotBeNullOrWhiteSpace();
        }
    }
}
