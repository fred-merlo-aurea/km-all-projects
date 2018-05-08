using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Entities = ECN_Framework_Entities.Communicator;
using ECN_Framework_BusinessLayer.Communicator;
using ECN.TestHelpers;
using Shouldly;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FilterConditionTest
    {
        private const string _MethodCheckValues = "CheckValues";       

        [Test]
        public void CheckValues_OnException_ReturnFalse()
        {
            // Arrange
            var filterCondition = new Entities::FilterCondition
            {
                Comparator = "TestString",
                FieldType = nameof(String),
                CompareValue = string.Empty
            };

            // Act	
            var actualResult = typeof(FilterCondition).CallMethod(_MethodCheckValues, new object[] { filterCondition }) as bool?;

            // Assert
            actualResult.ShouldNotBeNull();
            actualResult.HasValue.ShouldBeTrue();
            actualResult.Value.ShouldBeFalse();
        }

        [Test]
        [TestCase("TestString", "String", "TestValue", "")]
        [TestCase("TestString", "Date", "EXP:", "")]
        [TestCase("equals", "Date", "1/1/2018", "full")]
        [TestCase("is in", "Date", "1/1/2018", "full")]
        [TestCase("TestString", "Date", "1/1/2018", "full")]
        [TestCase("equals", "Date", "1", "TestValue")]
        [TestCase("is in", "Date", "1/1/2018", "TestValue")]
        [TestCase("equals", "Number", "1", "")]
        [TestCase("is in", "Number", "1", "")]
        [TestCase("TestString", "Number", "1", "")]
        [TestCase("equals", "Money", "1", "")]
        [TestCase("is in", "Money", "1", "")]
        [TestCase("TestString", "Money", "1", "")]
        [TestCase("TestString", "TestString", "1", "")]
        public void CheckValues_MultipleTypes_ReturnFalse(string comparator, string type, string value, string datePart)
        {
            // Arrange
            var filterCondition = new Entities::FilterCondition
            {
                Comparator = comparator,
                FieldType = type,
                CompareValue = value,
                DatePart = datePart
            };

            // Act	
            var actualResult = typeof(FilterCondition).CallMethod(_MethodCheckValues, new object[] { filterCondition }) as bool?;

            // Assert
            actualResult.ShouldNotBeNull();
            actualResult.HasValue.ShouldBeTrue();
            actualResult.Value.ShouldBeTrue();
        }
    }
}
