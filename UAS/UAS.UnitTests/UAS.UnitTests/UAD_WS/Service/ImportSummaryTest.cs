using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using KMPlatform.Object;
using NUnit.Framework;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntityImportErrorSummary = FrameworkUAD.Object.ImportErrorSummary;
using ServiceImportSummary = UAD_WS.Service.ImportSummary;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimImportErrorSummary;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ImportSummaryTest : Fakes
    {
        private const int SampleId = 100;
        private const string SampleString = "dummy";

        private ServiceImportSummary _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceImportSummary();
        }

        [Test]
        public void Select_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectInt32StringClientConnections = (a, b, c, d) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Select(Guid.Empty, SampleId, SampleString, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, null, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void Select_BySourceFileId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityImportErrorSummary>();
            ShimWorker.AllInstances.SelectInt32StringClientConnections = (a, b, c, d) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, SampleId, SampleString, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }
    }
}
