using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAS_WS.Service.Common;
using ServiceTransformation = UAS_WS.Service.Transformation;
using ShimWorker = FrameworkUAS.BusinessLogic.Fakes.ShimTransformation;
using ResponseStatus = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes;
using EntityTransformation = FrameworkUAS.Entity.Transformation;

namespace UAS.UnitTests.UAS_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class TransformationTest : Fakes
    {
        private const int ClientId = 100;
        private const int SourceFileId = 200;
        private const int TransformationId = 300;
        private const int FieldMappingId = 400;
        private const int AffectedCountPositive = 2;
        private const int AffectedCountNegative = -1;
        private const string SampleString = "dummyString";

        private ServiceTransformation _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceTransformation();
        }

        [Test]
        public void Select_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectInt32 = (_, __) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Select(Guid.Empty, TransformationId);

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Error);
        }

        [Test]
        public void Select_ByTransformationId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityTransformation>();
            var parameterId = 0;
            ShimWorker.AllInstances.SelectInt32 = (_, id) =>
            {
                parameterId = id;
                return list;
            };

            // Act
            var result = _testEntity.Select(Guid.Empty, TransformationId);

            // Assert
            VerifySuccessResponse(result, list);
            parameterId.ShouldBe(TransformationId);
        }

        [Test]
        public void Select_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityTransformation>();
            ShimWorker.AllInstances.Select = _ => list;

            // Act
            var result = _testEntity.Select(Guid.Empty);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Select_ByClientId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityTransformation>();
            var parameterId = 0;
            ShimWorker.AllInstances.SelectClientInt32Boolean = (_, id, __) =>
            {
                parameterId = id;
                return list;
            };

            // Act
            var result = _testEntity.Select(Guid.Empty, ClientId, true);

            // Assert
            VerifySuccessResponse(result, list);
            parameterId.ShouldBe(ClientId);
        }

        [Test]
        public void Select_ByClientIdAndSourceFileId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityTransformation>();
            var parameterClientId = 0;
            var parameterSourceFileId = 0;
            ShimWorker.AllInstances.SelectInt32Int32Boolean = (_, clientId, fileId, __) =>
            {
                parameterClientId = clientId;
                parameterSourceFileId = fileId;
                return list;
            };

            // Act
            var result = _testEntity.Select(Guid.Empty, ClientId, SourceFileId);

            // Assert
            VerifySuccessResponse(result, list);
            parameterClientId.ShouldBe(ClientId);
            parameterSourceFileId.ShouldBe(SourceFileId);
        }

        [Test]
        public void SelectAssigned_ByFieldMappingId_ReturnsErrorResponse()
        {
            // Arrange
            var list = new List<EntityTransformation>();
            var parameterId = 0;
            ShimWorker.AllInstances.SelectAssignedInt32 = (_, id) =>
            {
                parameterId = id;
                return list;
            };

            // Act
            var result = _testEntity.SelectAssigned(Guid.Empty, FieldMappingId);

            // Assert
            VerifySuccessResponse(result, list);
            parameterId.ShouldBe(FieldMappingId);
        }

        [Test]
        public void Delete_WorkerReturnsPositive_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterId = 0;
            ShimWorker.AllInstances.DeleteInt32 = (_, id) =>
            {
                parameterId = id;
                return AffectedCountPositive;
            };

            // Act
            var result = _testEntity.Delete(Guid.Empty, TransformationId);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
            parameterId.ShouldBe(TransformationId);
        }

        [Test]
        public void Delete_WorkerReturnsNegative_ReturnsErrorResponse()
        {
            // Arrange
            var parameterId = 0;
            ShimWorker.AllInstances.DeleteInt32 = (_, id) =>
            {
                parameterId = id;
                return AffectedCountNegative;
            };

            // Act
            var result = _testEntity.Delete(Guid.Empty, TransformationId);

            // Assert
            VerifyErrorResponse(result, AffectedCountNegative);
            parameterId.ShouldBe(TransformationId);
        }

        [Test]
        public void Save_WorkerReturnsPositive_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityTransformation();
            ShimWorker.AllInstances.SaveTransformation = (_, __) => AffectedCountPositive;
            ShimForJsonFunction<EntityTransformation>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void Save_WorkerReturnsNegative_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityTransformation();
            ShimWorker.AllInstances.SaveTransformation = (_, __) => AffectedCountNegative;
            ShimForJsonFunction<EntityTransformation>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity);

            // Assert
            VerifyErrorResponse(result, AffectedCountNegative);
        }

        [Test]
        public void SelectAssigned_ByFileName_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityTransformation>();
            ShimWorker.AllInstances.SelectAssignedString = (_, __) => list;

            // Act
            var result = _testEntity.SelectAssigned(Guid.Empty, SampleString);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Select_ByTransformationNameAndClientName_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityTransformation>();
            ShimWorker.AllInstances.SelectTransformationIDStringString = (_, __, ___) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, SampleString, SampleString);

            // Assert
            VerifySuccessResponse(result, list);
        }
    }
}
