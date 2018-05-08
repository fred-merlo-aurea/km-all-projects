using System;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using NUnit.Framework;
using UAS.UnitTests.UAS_WS.Service.Common;
using EntityClientCustomProcedure = FrameworkUAS.Entity.ClientCustomProcedure;
using ServiceClientCustomProcedure = UAS_WS.Service.ClientCustomProcedure;
using ShimWorker = FrameworkUAS.BusinessLogic.Fakes.ShimClientCustomProcedure;

namespace UAS.UnitTests.UAS_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ClientCustomProcedureTest : Fakes
    {
        private const int AffectedCountPositive = 1;
        private const int AffectedCountNegative = -1;

        private ServiceClientCustomProcedure _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceClientCustomProcedure();
        }

        [Test]
        public void SaveReturnID_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityClientCustomProcedure();
            ShimWorker.AllInstances.SaveReturnIDClientCustomProcedure = (_, __) => throw new InvalidOperationException();
            ShimForJsonFunction<EntityClientCustomProcedure>();

            // Act
            var result = _testEntity.SaveReturnID(Guid.NewGuid(), entity);

            // Assert
            VerifyErrorResponse(result, 0, StringFunctions.FriendlyServiceError);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void SaveReturnID_ByEntity_ReturnsExpectedResponse(bool workerSuccess)
        {
            // Arrange
            var entity = new EntityClientCustomProcedure();
            ShimWorker.AllInstances.SaveReturnIDClientCustomProcedure = (_, __) => workerSuccess
                ? AffectedCountPositive
                : AffectedCountNegative;
            ShimForJsonFunction<EntityClientCustomProcedure>();

            // Act
            var result = _testEntity.SaveReturnID(Guid.NewGuid(), entity);

            // Assert
            if (workerSuccess)
            {
                VerifySuccessResponse(result, AffectedCountPositive);
            }
            else
            {
                VerifyErrorResponse(result, AffectedCountNegative);
            }
        }
    }
}
