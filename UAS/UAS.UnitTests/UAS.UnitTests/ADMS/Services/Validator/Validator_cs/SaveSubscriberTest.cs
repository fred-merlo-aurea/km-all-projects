using System.Diagnostics.CodeAnalysis;
using FrameworkUAD.ServiceResponse;
using NUnit.Framework;
using UAS.UnitTests.ADMS.Services.Validator.Common;
using UAS.UnitTests.Helpers;
using SourceFile = FrameworkUAS.Entity.SourceFile;
using ADMS_Validator = ADMS.Services.Validator.Validator;

namespace UAS.UnitTests.ADMS.Services.Validator.Validator_cs
{
    /// <summary>
    ///     Unit Tests for <see cref="ADMS_Validator"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class SaveSubscriberTest
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
        public void SaveSubscriber_IfSubscriptionIsNull_ReturnNewSavedSubscriber()
        {
            // Arrange
            var expected = new SavedSubscriber();

            // Act
            SavedSubscriber savedSubscriber = testEntity.Validator.SaveSubscriber(testEntity.Client, null);

            // Assert
            Assert.IsTrue(expected.IsContentMatched(savedSubscriber));
        }

        [Test]
        public void SaveSubscriber_IfSourceFileNotNull_VerifySourceFileAndSavedSubscriberIsCorrect()
        {
            // Arrange
            var expected = new SourceFile {SourceFileID = Constants.SourceFileId, ClientID = Constants.ClientId};

            // Act
            SavedSubscriber savedSubscriber =
                testEntity.Validator.SaveSubscriber(testEntity.Client, testEntity.Subscriber);

            // Assert
            Assert.IsTrue(expected.IsContentMatched(testEntity.SourceFile, nameof(SourceFile.RuleSets)));
            Assert.IsTrue(expectedSavedSubscriber.IsContentMatched(savedSubscriber));
            VerifyIfContainsSourceFile();
        }

        [Test]
        public void SaveSubscriber_IfSourceFileNull_VerifySubscriberIsCorrect()
        {
            // Arrange
            testEntity.SourceFile = null;
            testEntity.IsNullSourceFile = true;

            // Act
            SavedSubscriber savedSubscriber =
                testEntity.Validator.SaveSubscriber(testEntity.Client, testEntity.Subscriber);


            // Assert
            Assert.IsTrue(expectedSavedSubscriber.IsContentMatched(savedSubscriber));
            VerifyIfSourceFileIsNull();
        }

        [Test]
        public void SaveSubscriber_IfSubscriberPubCodeValid_VerifySubscriberIsCorrect()
        {
            // Arrange
            testEntity.IsPubCodeValid = true;
            expectedSavedSubscriber.IsPubCodeValid = true;

            // Act
            SavedSubscriber savedSubscriber =
                testEntity.Validator.SaveSubscriber(testEntity.Client, testEntity.Subscriber);


            // Assert
            Assert.IsTrue(expectedSavedSubscriber.IsContentMatched(savedSubscriber));
            VerifyIfPubCodeValid();
        }

        [Test]
        public void SaveSubscriber_IfProductSubscriberCreated_VerifySubscriberIsCorrect()
        {
            // Arrange
            testEntity.IsPubCodeValid = true;
            testEntity.IsProductSubscriberCreated = true;
            expectedSavedSubscriber.IsPubCodeValid = true;
            expectedSavedSubscriber.IsProductSubscriberCreated = true;

            // Act
            SavedSubscriber savedSubscriber =
                testEntity.Validator.SaveSubscriber(testEntity.Client, testEntity.Subscriber);

            // Assert
            Assert.IsTrue(expectedSavedSubscriber.IsContentMatched(savedSubscriber));
            VerifyIfProductSubscriberCreated();
        }
    }
}