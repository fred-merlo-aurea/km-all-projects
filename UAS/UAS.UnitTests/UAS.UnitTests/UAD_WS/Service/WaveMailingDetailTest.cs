using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using KMPlatform.Object;
using NUnit.Framework;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntityWaveMailingDetail = FrameworkUAD.Entity.WaveMailingDetail;
using ServiceWaveMailingDetail = UAD_WS.Service.WaveMailingDetail;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimWaveMailingDetail;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class WaveMailingDetailTest : Fakes
    {
        private const int AffectedCountPositive = 1;
        private const int AffectedCountNegative = -1;
        private const int SampleId = 100;

        private ServiceWaveMailingDetail _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceWaveMailingDetail();
        }

        [Test]
        public void Save_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SaveWaveMailingDetailClientConnections = (_, __, ___) => throw new InvalidOperationException();
            ShimForJsonFunction<EntityWaveMailingDetail>();

            // Act
            var result = _testEntity.Save(Guid.Empty, null, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, 0, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void Save_WorkerReturnsPositive_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityWaveMailingDetail();
            ShimWorker.AllInstances.SaveWaveMailingDetailClientConnections = (_, __, ___) => AffectedCountPositive;
            ShimForJsonFunction<EntityWaveMailingDetail>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void Save_WorkerReturnsNegative_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityWaveMailingDetail();
            ShimWorker.AllInstances.SaveWaveMailingDetailClientConnections = (_, __, ___) => AffectedCountNegative;
            ShimForJsonFunction<EntityWaveMailingDetail>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, AffectedCountNegative);
        }

        [Test]
        public void SelectIssue_ByIssueId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityWaveMailingDetail>();
            ShimWorker.AllInstances.SelectIssueInt32ClientConnections = (_, __, ___) => list;

            // Act
            var result = _testEntity.SelectIssue(Guid.Empty, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void UpdateOriginalSubInfo_ByProductIdAndUserId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.UpdateOriginalSubInfoInt32Int32ClientConnections = (a, b, c, d) => true;

            // Act
            var result = _testEntity.UpdateOriginalSubInfo(Guid.Empty, SampleId, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, true);
        }
    }
}
