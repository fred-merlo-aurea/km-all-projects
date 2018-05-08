using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using KMPlatform.Object;
using NUnit.Framework;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntityFilter = FrameworkUAD.Entity.Filter;
using ServiceFilter = UAD_WS.Service.Filter;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimFilter;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FilterTest : Fakes
    {
        private const int AffectedCountPositive = 1;
        private const int AffectedCountNegative = -1;
        private const int SampleId = 100;

        private ServiceFilter _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceFilter();
        }

        [Test]
        public void Save_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SaveFilterClientConnections = (_, __, ___) => throw new InvalidOperationException();
            ShimForJsonFunction<EntityFilter>();

            // Act
            var result = _testEntity.Save(Guid.Empty, new ClientConnections(), null);

            // Assert
            VerifyErrorResponse(result, 0, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void Save_WorkerReturnsPositive_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityFilter();
            ShimWorker.AllInstances.SaveFilterClientConnections = (_, entityToSave, __) => AffectedCountPositive;
            ShimForJsonFunction<EntityFilter>();

            // Act
            var result = _testEntity.Save(Guid.Empty, new ClientConnections(), entity);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void Save_WorkerReturnsNegative_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityFilter();
            ShimWorker.AllInstances.SaveFilterClientConnections = (_, entityToSave, __) => AffectedCountNegative;
            ShimForJsonFunction<EntityFilter>();

            // Act
            var result = _testEntity.Save(Guid.Empty, new ClientConnections(), entity);

            // Assert
            VerifyErrorResponse(result, AffectedCountNegative);
        }

        [Test]
        public void Select_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityFilter>();
            ShimWorker.AllInstances.SelectClientConnections = (_, __) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Delete_ByFilterId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteInt32ClientConnections = (_, __, ___) => true;

            // Act
            var result = _testEntity.Delete(Guid.Empty, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, true);
        }
    }
}
