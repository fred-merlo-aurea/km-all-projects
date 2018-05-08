using System;
using System.Diagnostics.CodeAnalysis;
using Core_AMS.Utilities;
using NUnit.Framework;
using UAS.UnitTests.UAS_WS.Service.Common;
using EntityUserLog = KMPlatform.Entity.UserLog;
using ServiceUserLog = UAS_WS.Service.UserLog;
using ShimWorker = KMPlatform.BusinessLogic.Fakes.ShimUserLog;

namespace UAS.UnitTests.UAS_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class UserLogTest : Fakes
    {
        private const int AffectedCountPositive = 1;
        private const int AffectedCountNegative = -1;

        private ServiceUserLog _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceUserLog();
        }

        [Test]
        public void Save_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityUserLog();
            ShimWorker.AllInstances.SaveUserLog = (_, __) => throw new InvalidOperationException();
            ShimForJsonFunction<EntityUserLog>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity);

            // Assert
            VerifyErrorResponse(result, 0, StringFunctions.FriendlyServiceError());
        }

        [Test]
        public void Save_WorkerReturnsPositive_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityUserLog();
            ShimWorker.AllInstances.SaveUserLog = (_, __) => AffectedCountPositive;
            ShimForJsonFunction<EntityUserLog>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void Save_WorkerReturnsNegative_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityUserLog();
            ShimWorker.AllInstances.SaveUserLog = (_, __) => AffectedCountNegative;
            ShimForJsonFunction<EntityUserLog>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity);

            // Assert
            VerifyErrorResponse(result, AffectedCountNegative);
        }
    }
}
