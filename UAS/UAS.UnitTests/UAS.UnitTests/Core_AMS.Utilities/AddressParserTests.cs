using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Core_AMS.Utilities;
using NUnit.Framework;
using Shouldly;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UAS.UnitTests.Core_AMS.Utilities
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AddressParserTests
    {
        private const string _initializeRegexMethodName = "InitializeRegex";
        private const string _addressRegexFieldName = "addressRegex";
        private MSTest::PrivateType _privateType;


        [SetUp]
        public void SetUp()
        {
            _privateType = new MSTest.PrivateType(typeof(AddressParser));
        }

        [Test]
        public void InitializeRegex_AddressRegexIsNotNull()
        {
            // Arrange & Act
            _privateType.InvokeStatic(_initializeRegexMethodName);
            var addressRegex =_privateType.GetStaticField(_addressRegexFieldName) as Regex;

            // Assert
            _privateType.ShouldSatisfyAllConditions(
                () => addressRegex.ShouldNotBeNull());
        }
        
        [Test]
        [TestCase("1600 Amphitheatre Pkwy, Mountain View CA 94043", true)]
        [TestCase("1600 Metro Avenue, Los Angeles CA 96031", true)]
        [TestCase("1600 Metro Avenue, Los Angeles CA 96031, USA", false)]
        public void InitializeRegex_AddressRegexIsInitializedWithCorrectPattern(string address, bool expectedValue)
        {
            // Arrange & Act
            _privateType.InvokeStatic(_initializeRegexMethodName);
            var addressRegex = _privateType.GetStaticField(_addressRegexFieldName) as Regex;
            var actualValue = addressRegex?.IsMatch(address);

            // Assert
            _privateType.ShouldSatisfyAllConditions(
                () => addressRegex.ShouldNotBeNull(),
                () => actualValue.ShouldBe(expectedValue));
        }
    }
}
