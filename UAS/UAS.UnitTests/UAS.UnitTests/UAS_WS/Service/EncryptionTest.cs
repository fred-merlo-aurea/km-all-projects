using System;
using System.Diagnostics.CodeAnalysis;
using Core_AMS.Utilities;
using NUnit.Framework;
using UAS.UnitTests.UAS_WS.Service.Common;
using EntityEncryption = FrameworkUAS.Object.Encryption;
using ServiceEncryption = UAS_WS.Service.Encryption;
using ShimWorker = FrameworkUAS.BusinessLogic.Fakes.ShimEncryption;

namespace UAS.UnitTests.UAS_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class EncryptionTest : Fakes
    {
        private ServiceEncryption _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceEncryption();
            ShimForJsonFunction<EntityEncryption>();
        }

        [Test]
        public void Encrypt_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityEncryption();
            ShimWorker.AllInstances.EncryptEncryption = (_, __) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Encrypt(Guid.Empty, entity);

            // Assert
            VerifyErrorResponse(result, null, StringFunctions.FriendlyServiceError());
        }

        [Test]
        public void Encrypt_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityEncryption();
            ShimWorker.AllInstances.EncryptEncryption = (_, __) => entity;

            // Act
            var result = _testEntity.Encrypt(Guid.Empty, entity);

            // Assert
            VerifySuccessResponse(result, entity);
        }

        [Test]
        public void Decrypt_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityEncryption();
            ShimWorker.AllInstances.DecryptEncryption = (_, __) => entity;

            // Act
            var result = _testEntity.Decrypt(Guid.Empty, entity);

            // Assert
            VerifySuccessResponse(result, entity);
        }

        [Test]
        public void Decrypt_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityEncryption();
            ShimWorker.AllInstances.DecryptEncryption = (_, __) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Decrypt(Guid.Empty, entity);

            // Assert
            VerifyErrorResponse(result, null, StringFunctions.FriendlyServiceError());
        }
    }
}
