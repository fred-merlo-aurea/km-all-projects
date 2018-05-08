using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Shouldly;
using BusinessLogicSubscription = FrameworkUAD.BusinessLogic.Subscription;
using EntitySubscription = FrameworkUAD.Entity.Subscription;

namespace UAS.UnitTests.FrameworkUAD.BusinessLogic
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SubscriptionTest
    {
        private BusinessLogicSubscription _testEntity;

        [SetUp]
        public void SetUp()
        {
            _testEntity = new BusinessLogicSubscription();
        }

        [Test]
        [TestCase(1, "00012", "12--", "00012", "12--")]
        [TestCase(1, "123", "12345", "", "")]
        [TestCase(1, "1234", "1234", "01234", "1234")]
        [TestCase(1, "123-45 ", "1", "12345", "")]
        [TestCase(1, "12345678", "11", "12345", "")]
        [TestCase(1, "1234567890", "", "1234567890", "")]
        [TestCase(1, "1234-56789", "", "1234-56789", "6789")]
        [TestCase(1, "123-------4", "11", "", "11")]
        [TestCase(2, "123456", "1", "123 456", "")]
        [TestCase(2, "123 456", "1", "123 456", "")]
        [TestCase(2, "123456", "1", "123 456", "")]
        [TestCase(2, "123", "456", "123 456", "")]
        [TestCase(2, "1234567 ", "1", "1234567", "")]
        [TestCase(2, "12345678", "1", "123 456", "")]
        [TestCase(3, "1234567", "1234", "1234567", "")]
        public void FormatZipCode_WithEntity_ZipFormatted(int countryId, string inputZip, string inputPlus4, string expectedZip, string expectedPlus4)
        {
            // Arrange
            var entity = GetEntityWithZipCode(countryId, inputZip, inputPlus4);

            // Act
            var result = _testEntity.FormatZipCode(entity);

            // Assert
            VerifyFormattedZip(result, expectedZip, expectedPlus4);
        }

        [Test]
        [TestCase(1, "00012", "12--", "", "12--")]
        [TestCase(1, "123", "12345", "", "")]
        [TestCase(1, "1234", "1234", "01234", "1234")]
        [TestCase(1, "123-45 ", "1", "12345", "")]
        [TestCase(1, "12345678", "11", "12345", "")]
        [TestCase(1, "1234567890", "", "1234567890", "")]
        [TestCase(1, "1234-56789", "", "1234-56789", "6789")]
        [TestCase(1, "123-------4", "11", "", "11")]
        [TestCase(2, "123456", "1", "123 456", "")]
        [TestCase(2, "123 456", "1", "123 456", "")]
        [TestCase(2, "123456", "1", "123 456", "")]
        [TestCase(2, "123", "456", "123 456", "")]
        [TestCase(2, "1234567 ", "1", "1234567", "")]
        [TestCase(2, "12345678", "1", "123 456", "")]
        [TestCase(3, "1234567", "1234", "1234567", "")]
        public void FormatZipCode_WithEntityList_ZipFormatted(int countryId, string inputZip, string inputPlus4, string expectedZip, string expectedPlus4)
        {
            // Arrange
            var entity = GetEntityWithZipCode(countryId, inputZip, inputPlus4);
            var list = new List<EntitySubscription> { entity };

            // Act
            var result = _testEntity.FormatZipCode(list);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ForEach(e => VerifyFormattedZip(e, expectedZip, expectedPlus4)));
        }

        private static EntitySubscription GetEntityWithZipCode(int countryId, string zip, string plus4 = "")
        {
            return new EntitySubscription
            {
                CountryID = countryId,
                Zip = zip,
                Plus4 = plus4
            };
        }

        private static void VerifyFormattedZip(EntitySubscription entity, string expectedZip, string expectedPlus4)
        {
            entity.ShouldSatisfyAllConditions(
                () => entity.ShouldNotBeNull(),
                () => entity.Zip.ShouldBe(expectedZip),
                () => entity.Plus4.ShouldBe(expectedPlus4));
        }
    }
}
