using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core_AMS.Utilities;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAS_WS.Service.Common;
using EntityFpsCustomRule = FrameworkUAS.Entity.FpsCustomRule;
using ServiceFpsStandardRule = UAS_WS.Service.FpsCustomRule;
using ShimWorker = FrameworkUAS.BusinessLogic.Fakes.ShimFpsCustomRule;

namespace UAS.UnitTests.UAS_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FpsCustomRuleTest : Fakes
    {
        private const int FileId = 100;
        private const int AffectedCountPositive = 1;
        private const int AffectedCountNegative = -1;

        private ServiceFpsStandardRule _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceFpsStandardRule();
        }

        [Test]
        public void Delete_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteInt32 = (_, __) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Delete(Guid.Empty, FileId);

            // Assert
            VerifyErrorResponse(result, false, StringFunctions.FriendlyServiceError());
        }

        [Test]
        public void Delete_ByFileId_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterId = 0;
            ShimWorker.AllInstances.DeleteInt32 = (_, id) =>
            {
                parameterId = id;
                return false;
            };

            // Act
            var result = _testEntity.Delete(Guid.Empty, FileId);

            // Assert
            VerifySuccessResponse(result, false);
            parameterId.ShouldBe(FileId);
        }

        [Test]
        public void SelectSourceFileId_BySourceFileId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityFpsCustomRule>();
            ShimWorker.AllInstances.SelectSourceFileIdInt32 = (_, __) => list;

            // Act
            var result = _testEntity.SelectSourceFileId(Guid.Empty, FileId);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Save_WorkerReturnsPositive_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityFpsCustomRule();
            ShimWorker.AllInstances.SaveFpsCustomRule = (_, __) => AffectedCountPositive;
            ShimForJsonFunction<EntityFpsCustomRule>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void Save_WorkerReturnsNegative_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityFpsCustomRule();
            ShimWorker.AllInstances.SaveFpsCustomRule = (_, __) => AffectedCountNegative;
            ShimForJsonFunction<EntityFpsCustomRule>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity);

            // Assert
            VerifyErrorResponse(result, AffectedCountNegative);
        }

        [Test]
        public void SelectClientId_ByClientId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityFpsCustomRule>();
            ShimWorker.AllInstances.SelectClientIdInt32 = (a, b) => list;

            // Act
            var result = _testEntity.SelectClientId(Guid.Empty, FileId);

            // Assert
            VerifySuccessResponse(result, list);
        }
    }
}
