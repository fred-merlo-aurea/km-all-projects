using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FrameworkUAD.Entity;
using FrameworkUAS.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using static UAS.UnitTests.ADMS.Services.Validator.Common.Constants;
using ADMS_Validator = ADMS.Services.Validator.Validator;
using RuleObject = FrameworkUAS.Object.Rule;

namespace UAS.UnitTests.ADMS.Services.Validator.Validator_cs
{
    /// <summary>
    ///     Unit Tests for <see cref="ADMS_Validator"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SetRuleIsQualifiedTest
    {
        private const string CompanyKey = "Company";
        private const bool IsQualified = false;

        private PrivateObject _validatorPrivateObject;
        private SubscriberTransformed _subscriber;
        private StringBuilder _sbRules;
        private RuleObject _rule;
        private string _ruleName;

        [SetUp]
        public void Initialize()
        {
            _validatorPrivateObject = new PrivateObject(typeof(ADMS_Validator));
            _subscriber = new SubscriberTransformed();
            _sbRules = new StringBuilder();
            _rule = new RuleObject();
            _ruleName = CompanyKey;
        }

        [Test]
        public void SetRuleIsQualified_WhenSubscriberTransformedIsNull_ThrowsException()
        {
            // Arrange
            _subscriber = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                _validatorPrivateObject.Invoke(SetRuleIsQualifiedMethod, _subscriber, _sbRules, _rule, _ruleName, IsQualified));
        }

        [Test]
        public void SetRuleIsQualified_WhenStringBuilderRulesIsNull_ThrowsException()
        {
            // Arrange
            _sbRules = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                _validatorPrivateObject.Invoke(SetRuleIsQualifiedMethod, _subscriber, _sbRules, _rule, _ruleName, IsQualified));
        }

        [Test]
        public void SetRuleIsQualified_WhenRuleObjectIsNull_ThrowsException()
        {
            // Arrange
            _rule = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                _validatorPrivateObject.Invoke(SetRuleIsQualifiedMethod, _subscriber, _sbRules, _rule, _ruleName, IsQualified));
        }

        [Test]
        public void SetRuleIsQualified_WhenRuleNameIsEmptyNull_ReturnsFalse()
        {
            // Arrange
            _subscriber = new SubscriberTransformed
            {
                Company = string.Empty
            };

            _rule = new RuleObject
            {
                RuleValues = new HashSet<RuleValue>
                {
                    new RuleValue { Value = CompanyKey }
                }
            };

            // Act, Assert
            var result = _validatorPrivateObject.Invoke(
                SetRuleIsQualifiedMethod, _subscriber, _sbRules, _rule, _ruleName, IsQualified) as bool?;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.Value.ShouldBeFalse());
        }

        [Test]
        public void SetRuleIsQualified_WhenRuleNameIsNotEmptyOrNull_ReturnsFalse()
        {
            // Arrange
            _subscriber = new SubscriberTransformed
            {
                Company = CompanyKey
            };

            _rule = new RuleObject
            {
                RuleValues = new HashSet<RuleValue>
                {
                    new RuleValue { Value = CompanyKey }
                }
            };

            // Act, Assert
            var result = _validatorPrivateObject.Invoke(
                SetRuleIsQualifiedMethod, _subscriber, _sbRules, _rule, _ruleName, IsQualified) as bool?;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.Value.ShouldBeTrue());
        }
    }
}
