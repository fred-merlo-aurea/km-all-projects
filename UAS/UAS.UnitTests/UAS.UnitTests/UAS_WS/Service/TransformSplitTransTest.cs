using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAS_WS.Service.Common;
using ServiceTransformSplitTrans = UAS_WS.Service.TransformSplitTrans;
using ShimWorker = FrameworkUAS.BusinessLogic.Fakes.ShimTransformSplitTrans;
using ResponseStatus = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes;
using EntityTransformSplitTrans = FrameworkUAS.Entity.TransformSplitTrans;

namespace UAS.UnitTests.UAS_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class TransformSplitTransTest : Fakes
    {
        private const int AffectedCountPositive = 2;
        private const int AffectedCountNegative = -1;

        private ServiceTransformSplitTrans _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceTransformSplitTrans();
        }

        [Test]
        public void Save_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityTransformSplitTrans();
            ShimWorker.AllInstances.SaveTransformSplitTrans = (_, __) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity);

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Error);
        }

        [Test]
        public void Save_IfWorkerReturnPositive_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityTransformSplitTrans();
            ShimWorker.AllInstances.SaveTransformSplitTrans = (_, __) => AffectedCountPositive;

            // Act
            var result = _testEntity.Save(Guid.Empty, entity);

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Success);
            result.Result.ShouldBe(AffectedCountPositive);
        }

        [Test]
        public void Save_IfWorkerReturnNegative_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityTransformSplitTrans();
            ShimWorker.AllInstances.SaveTransformSplitTrans = (_, __) => AffectedCountNegative;

            // Act
            var result = _testEntity.Save(Guid.Empty, entity);

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Error);
            result.Result.ShouldBe(AffectedCountNegative);
        }
    }
}
