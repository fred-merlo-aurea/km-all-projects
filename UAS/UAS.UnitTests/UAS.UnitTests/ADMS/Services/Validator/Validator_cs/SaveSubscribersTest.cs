using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAD.Object;
using FrameworkUAD.ServiceResponse;
using FrameworkUAS.Entity;
using Moq;
using NUnit.Framework;
using UAS.UnitTests.ADMS.Services.Validator.Common;
using UAS.UnitTests.Helpers;
using SourceFile = FrameworkUAS.Entity.SourceFile;
using ADMS_Validator = ADMS.Services.Validator.Validator;
using Assert = NUnit.Framework.Assert;

namespace UAS.UnitTests.ADMS.Services.Validator.Validator_cs
{
    /// <summary>
    ///     Unit Tests for <see cref="ADMS_Validator"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class SaveSubscribersTest
    {
        private List<SaveSubscriber> saveSubscribers;
        private TestEntity testEntity;

        [SetUp]
        public void SetUp()
        {
            testEntity = new TestEntity();
            SetupFakes(testEntity.Mocks);
            saveSubscribers = new List<SaveSubscriber> {new SaveSubscriber()};
            Initialize();
        }

        [Test]
        public void SaveSubscribers_IfSubscriptionIsNull_ThrowNullReferenceException()
        {
            // Arrange, Act, Assert
            Assert.That(() =>
                    testEntity.Validator.SaveSubscribers(testEntity.Client, null),
                Throws.TypeOf<NullReferenceException>());
        }

        [Test]
        public void SaveSubscribers_IfSourceFileNotNullWithCirculation_VerifySourceFileAndSavedSubscriberIsCorrect()
        {
            // Arrange
            var expected = new SourceFile {SourceFileID = Constants.SourceFileId, ClientID = Constants.ClientId};
            testEntity.IsCirculation = true;

            // Act
            SavedSubscriber savedSubscriber =
                testEntity.Validator.SaveSubscribers(testEntity.Client, saveSubscribers, testEntity.IsCirculation);

            // Assert
            Assert.IsTrue(expected.IsContentMatched(testEntity.SourceFile, nameof(SourceFile.RuleSets)));
            Assert.IsTrue(expectedSavedSubscriber.IsContentMatched(savedSubscriber));
            VerifyIfSourceFileNotNullWithCirculation();
        }

        [Test]
        public void SaveSubscribers_IfSourceFileNotNullWithoutCirculation_VerifySourceFileAndSavedSubscriberIsCorrect()
        {
            // Arrange
            var expected = new SourceFile {SourceFileID = Constants.SourceFileId, ClientID = Constants.ClientId};

            // Act
            SavedSubscriber savedSubscriber =
                testEntity.Validator.SaveSubscribers(testEntity.Client, saveSubscribers, testEntity.IsCirculation);

            // Assert
            Assert.IsTrue(expected.IsContentMatched(testEntity.SourceFile, nameof(SourceFile.RuleSets)));
            Assert.IsTrue(expectedSavedSubscriber.IsContentMatched(savedSubscriber));
            VerifyIfSourceFileNotNullWithoutCirculation();
        }

        [Test]
        public void SaveSubscribers_IfSourceFileNullWithCirculation_ThrowInvalidOperationException()
        {
            // Arrange
            testEntity.SourceFile = null;
            testEntity.IsNullSourceFile = true;
            testEntity.IsCirculation = true;

            // Act & Assert
            Assert.That(
                () => testEntity.Validator.SaveSubscribers(testEntity.Client, saveSubscribers,
                    testEntity.IsCirculation), Throws.InvalidOperationException);
        }

        [Test]
        public void SaveSubscribers_IfSourceFileNullWithoutCirculation_ThrowsInvalidOperationException()
        {
            // Arrange
            testEntity.SourceFile = null;
            testEntity.IsNullSourceFile = true;

            // Act & Assert
            Assert.That(
                () => testEntity.Validator.SaveSubscribers(testEntity.Client, saveSubscribers,
                    testEntity.IsCirculation), Throws.InvalidOperationException);
        }

        [Test]
        public void SaveSubscribers_IfSubscriberPubCodeValid_VerifySubscriberIsCorrect()
        {
            // Arrange
            testEntity.IsPubCodeValid = true;
            expectedSavedSubscriber.IsPubCodeValid = true;

            // Act
            SavedSubscriber savedSubscriber = testEntity.Validator.SaveSubscribers(testEntity.Client, saveSubscribers);

            // Assert
            Assert.IsTrue(expectedSavedSubscriber.IsContentMatched(savedSubscriber));
            VerifyIfPubCodeValid();
        }

        [Test]
        public void SaveSubscribers_SetupDMQToThrowException_VerifyLogError()
        {
            // Arrange
            testEntity.IsPubCodeValid = true;
            testEntity.IsProductSubscriberCreated = true;
            expectedSavedSubscriber.IsPubCodeValid = true;
            expectedSavedSubscriber.IsProductSubscriberCreated = true;
            testEntity.Mocks.DQmQue
                .Setup(x => x.Save(It.IsAny<DQMQue>()))
                .Throws<NullReferenceException>();

            // Act
            testEntity.Validator.SaveSubscribers(testEntity.Client, saveSubscribers);

            // Assert
            testEntity.Mocks.ServiceBase
                .Verify(x => x.LogError(It.IsAny<NullReferenceException>(), testEntity.Client,
                    $"{testEntity.Validator.GetType().Name}.SaveSubscriber", true, true));
        }
    }
}