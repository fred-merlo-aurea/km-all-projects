using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Core_AMS.Utilities;
using KMPlatform.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntityBatch = FrameworkUAD.Entity.Suppressed;
using EntitySubscriberFinal = FrameworkUAD.Entity.SubscriberFinal;
using ServiceSuppressed = UAD_WS.Service.Suppressed;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimSuppressed;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SuppressedTest : Fakes
    {
        private const int SampleId = 100;
        private const int AffectedCountPositive = 2;
        private const int AffectedCountNegative = -1;
        private const string SampleString = "this is a string.";

        private ServiceSuppressed _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceSuppressed();
        }

        [Test]
        public void PerformSuppression_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberFinal>();
            var fileInfo = new FileInfo(SampleString);
            ShimWorker.AllInstances.PerformSuppressionListOfSubscriberFinalClientConnectionsInt32StringString
                = (a, b, c, d, e, f) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.PerformSuppression(Guid.Empty, list, new ClientConnections(), SampleId, SampleString, fileInfo);

            // Assert
            VerifyErrorResponse(result, 0, StringFunctions.FriendlyServiceError());
        }

        [Test]
        public void PerformSuppression_WorkerReturnsPositive_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberFinal>();
            var fileInfo = new FileInfo(SampleString);
            ShimWorker.AllInstances.PerformSuppressionListOfSubscriberFinalClientConnectionsInt32StringString
                = (a, b, c, d, e, f) => AffectedCountPositive;

            // Act
            var result = _testEntity.PerformSuppression(Guid.Empty, list, new ClientConnections(), SampleId, SampleString, fileInfo);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void PerformSuppression_WorkerReturnsNegative_ReturnsErrorResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberFinal>();
            var fileInfo = new FileInfo(SampleString);
            ShimWorker.AllInstances.PerformSuppressionListOfSubscriberFinalClientConnectionsInt32StringString
                = (a, b, c, d, e, f) => AffectedCountNegative;

            // Act
            var result = _testEntity.PerformSuppression(Guid.Empty, list, new ClientConnections(), SampleId, SampleString, fileInfo);

            // Assert
            VerifyErrorResponse(result, AffectedCountNegative);
        }
    }
}
