using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using NUnit.Framework;
using ECN_Framework_Entities.Salesforce.Convertors;

namespace ECN.Communicator.Tests.Main.Salesforce.Entity.Converters
{
    [TestFixture]
    public class EntityConverterBaseTest
    {
        [Test]
        public void Convert_PassEmptyJson_ReturnsEmptyCollection()
        {
            // Arrange
            var converter = new JustForTestConverter();
            var entity = Enumerable.Empty<string>();

            // Act
            var result = converter.Convert<JustForTestEntity>(entity);

            //Assert
            result.ShouldBeEmpty();
        }

        [Test]
        public void Convert_PassJsonWithoutLastProperty_ReturnsEmptyCollection()
        {
            // Arrange
            var converter = new JustForTestConverter();
            var entity = GetEntityAsJson().Take(GetEntityAsJson().Count() - 1).ToArray();

            // Act
            var result = converter.Convert<JustForTestEntity>(entity);

            //Assert
            result.ShouldBeEmpty();
        }

        [Test]
        public void Convert_PassJsoWithOneEntity_ReturnsCollectionWithOneEntity()
        {
            // Arrange
            var expectedEntity = GetExpectedEntity();
            var converter = new JustForTestConverter();
            var entity = GetEntityAsJson().ToArray();

            // Act
            var result = converter.Convert<JustForTestEntity>(entity);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldHaveSingleItem(),
                () => result.ShouldContain(
                     x => x.First == expectedEntity.First &&
                          x.Second == expectedEntity.Second &&
                          x.Last == expectedEntity.Last));
        }

        [Test]
        public void Convert_PassJsoWithSeveralEntities_ReturnsCollectionSeveralEntities()
        {
            // Arrange
            const int twoItems = 2;
            var converter = new JustForTestConverter();
            var expectedEntity = GetExpectedEntity();
            var entity = GetEntityAsJson().Concat(GetEntityAsJson()).ToArray();

            // Act
            var result = converter.Convert<JustForTestEntity>(entity);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.Count().ShouldBe(twoItems),
                () => result.ShouldContain(
                    x => x.First == expectedEntity.First &&
                         x.Second == expectedEntity.Second &&
                         x.Last == expectedEntity.Last));
        }

        [Test]
        public void Convert_AddCustomConverterForProperty_ReturnsCollectionWithCustomConverterPropertyAtEntity()
        {
            // Arrange
            const string propertyToMultiply = "First";
            const int TenMultiplier = 10;
            var expectedEntity = GetExpectedEntity();
            var entity = GetEntityAsJson().ToArray();
            var converter = new JustForTestConverter();
            converter.AddPropertyConverter(propertyToMultiply, x => Convert.ToInt32(x) * TenMultiplier);

            // Act
            var result = converter.Convert<JustForTestEntity>(entity);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldHaveSingleItem(),
                () => result.ShouldContain(
                    x => x.First == expectedEntity.First * TenMultiplier &&
                         x.Second == expectedEntity.Second &&
                         x.Last == expectedEntity.Last));
        }

        [Test]
        public void Convert_AddPropertyMapping_ReturnsCollectionWithEntityWhereOriginalPropertyHasMappedValue()
        {
            // Arrange
            const int mappedValue = 222;
            const string originalProperty = "First";
            const string propertyFromJson = "Mapped";
            var mappedPropertyValue = $" \"{propertyFromJson}\": {mappedValue}";
            var expectedEntity = GetExpectedEntity();
            var entity = new[] { mappedPropertyValue }.Union(GetEntityAsJson().Where(x => !x.Contains(originalProperty))).ToArray();
            var converter = new JustForTestConverter();
            converter.AddPropertyMapping(originalProperty, propertyFromJson);

            // Act
            var result = converter.Convert<JustForTestEntity>(entity);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldHaveSingleItem(),
                () => result.ShouldContain(
                    x => x.First == mappedValue &&
                         x.Second == expectedEntity.Second &&
                         x.Last == expectedEntity.Last));
        }

        [Test]
        public void Convert_PassJsonWithInvalidProperty_ReturnsValidEntity()
        {
            // Arrange
            var expectedEntity = GetExpectedEntity();
            const string invalidPropertyValue = " \"Invalid\": 1";
            var entity = new[] { invalidPropertyValue }.Union(GetEntityAsJson()).ToArray();
            var converter = new JustForTestConverter();

            // Act
            var result = converter.Convert<JustForTestEntity>(entity);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldHaveSingleItem(),
                () => result.ShouldContain(
                    x => x.First == expectedEntity.First &&
                         x.Second == expectedEntity.Second &&
                         x.Last == expectedEntity.Last));
        }

        private JustForTestEntity GetExpectedEntity()
        {
            return new JustForTestEntity
            {
                First = 1,
                Second = 2,
                Last = 100
            };
        }

        private IEnumerable<string> GetEntityAsJson()
        {
            yield return " \"First\": 1";
            yield return " \"Second\": 2";
            yield return " \"Last\": 100";
        }

        private class JustForTestEntity
        {
            public int First { get; set; }
            public int Second { get; set; }
            public int Last { get; set; }
        }

        private class JustForTestConverter : EntityConverterBase
        {
            protected override string LastPropertyName => "Last";
        }
    }
}
