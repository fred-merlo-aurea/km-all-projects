using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAD.Entity;
using FrameworkUAD.ServiceResponse;
using NUnit.Framework;
using UAS.UnitTests.ADMS.Services.Validator.Common;
using UAS.UnitTests.Helpers;
using ADMS_Validator = ADMS.Services.Validator.Validator;

namespace UAS.UnitTests.ADMS.Services.Validator.Validator_cs
{
    /// <summary>
    ///     Unit Tests for <see cref="ADMS_Validator"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class SaveSubscriberTransformedTest
    {
        private TestEntity testEntity;

        [SetUp]
        public void Setup()
        {
            testEntity = new TestEntity();
            SetupFakes(testEntity.Mocks);
            Initialize();
        }

        [Test]
        public void SaveSubscriber_ContainsNotAnyProductSubscriptions_VerifySavedSubscriberIsCorrect()
        {
            // Arrange
            var expected = new SavedSubscriber { IsPubCodeValid = true };

            // Act
            SavedSubscriber savedSubscriber = testEntity.Validator.SaveSubscriber(testEntity.Client, testEntity.Subscriber);
            expected.ProcessCode = ProcessCode;


            // Assert
            Assert.True(expected.IsContentMatched(savedSubscriber));
            VerifyMethodInteractions();
        }

        [Test]
        public void SaveSubscriber_ContainsProductSubscriptions_VerifySavedSubscriberIsCorrect()
        {
            // Arrange
            var expected = new SavedSubscriber
            {
                IsPubCodeValid = true,
                IsProductSubscriberCreated = true,
            };

            testEntity.Subscriber.Products = subscriptions;

            // Act
            var savedSubscriber = testEntity.Validator.SaveSubscriber(testEntity.Client, testEntity.Subscriber);
            expected.ProcessCode = ProcessCode;
            foreach (List<SubscriberOriginal> subscriberOriginals in originals)
            {
                foreach (SubscriberOriginal so in subscriberOriginals)
                {
                    expected.SubscriberProductMessage +=
                        $"Saved Subscriber: {so.SORecordIdentifier} for Product: {so.PubCode}{System.Environment.NewLine}";
                    expected.IsProductSubscriberCreated = true;
                    expected.SubscriberProductIdentifiers.Add(so.SORecordIdentifier, so.PubCode);
                }
            }

            // Assert
            Assert.True(expected.IsContentMatched(savedSubscriber));
            VerifyMethodInteractions();
        }
    }
}