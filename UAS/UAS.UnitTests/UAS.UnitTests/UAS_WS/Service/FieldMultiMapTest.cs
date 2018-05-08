using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAS_WS.Service.Common;
using ServiceFieldMultiMap = UAS_WS.Service.FieldMultiMap;
using ShimWorker = FrameworkUAS.BusinessLogic.Fakes.ShimFieldMultiMap;
using ResponseStatus = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes;
using EntityFieldMultiMap = FrameworkUAS.Entity.FieldMultiMap;

namespace UAS.UnitTests.UAS_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FieldMultiMapTest : Fakes
    {
        private const int MapId = 100;
        private const int SourceFileId = 200;
        private const int MappingId = 300;
        private const int AffectedCountPositive = 2;
        private const int AffectedCountNegative = -1;

        private ServiceFieldMultiMap _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceFieldMultiMap();
        }

        [Test]
        public void DeleteByFieldMultiMapID_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteByFieldMultiMapIDInt32 = (_, __) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.DeleteByFieldMultiMapID(Guid.Empty, MapId);

            // Assert
            result.Status.ShouldBe(ResponseStatus.Error);
        }

        [Test]
        public void DeleteByFieldMultiMapID_IfWorkerReturnPositive_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterMapId = 0;
            ShimWorker.AllInstances.DeleteByFieldMultiMapIDInt32 = (_, id) =>
            {
                parameterMapId = id;
                return AffectedCountPositive;
            };

            // Act
            var result = _testEntity.DeleteByFieldMultiMapID(Guid.Empty, MapId);

            // Assert
            result.Status.ShouldBe(ResponseStatus.Success);
            parameterMapId.ShouldBe(MapId);
        }

        [Test]
        public void DeleteByFieldMultiMapID_IfWorkerReturnNegative_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteByFieldMultiMapIDInt32 = (_, id) => AffectedCountNegative;

            // Act
            var result = _testEntity.DeleteByFieldMultiMapID(Guid.Empty, MapId);

            // Assert
            result.Status.ShouldBe(ResponseStatus.Error);
            result.Result.ShouldBe(AffectedCountNegative);
        }

        [Test]
        public void DeleteBySourceFileID_BySourceFileId_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterId = 0;
            ShimWorker.AllInstances.DeleteBySourceFileIDInt32 = (_, id) =>
            {
                parameterId = id;
                return AffectedCountPositive;
            };

            // Act
            var result = _testEntity.DeleteBySourceFileID(Guid.Empty, SourceFileId);

            // Assert
            result.Status.ShouldBe(ResponseStatus.Success);
            parameterId.ShouldBe(SourceFileId);
        }

        [Test]
        public void Select_ByAccessId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityFieldMultiMap>();
            ShimWorker.AllInstances.Select = _ =>
            {
                list.Add(new EntityFieldMultiMap());
                return list;
            };

            // Act
            var result = _testEntity.Select(Guid.Empty);

            // Assert
            result.Status.ShouldBe(ResponseStatus.Success);
            result.Result.ShouldNotBeEmpty();
        }

        [Test]
        public void SelectFieldMappingID_ByMappingId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityFieldMultiMap>();
            var parameterMappingId = 0;
            ShimWorker.AllInstances.SelectFieldMappingIDInt32 = (_, id) =>
            {
                parameterMappingId = id;
                list.Add(new EntityFieldMultiMap());
                return list;
            };

            // Act
            var result = _testEntity.SelectFieldMappingID(Guid.Empty, MappingId);

            // Assert
            result.Status.ShouldBe(ResponseStatus.Success);
            result.Result.ShouldNotBeEmpty();
            parameterMappingId.ShouldBe(MappingId);
        }

        [Test]
        public void SelectFieldMultiMapID_ByMappingId_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterMappingId = 0;
            ShimWorker.AllInstances.SelectFieldMultiMapIDInt32 = (_, id) =>
            {
                parameterMappingId = id;
                return new EntityFieldMultiMap();
            };

            // Act
            var result = _testEntity.SelectFieldMultiMapID(Guid.Empty, MappingId);

            // Assert
            result.Status.ShouldBe(ResponseStatus.Success);
            result.Result.ShouldNotBeNull();
            parameterMappingId.ShouldBe(MappingId);
        }

        [Test]
        public void Save_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityFieldMultiMap();
            EntityFieldMultiMap parameterEntity = null;
            ShimWorker.AllInstances.SaveFieldMultiMap = (_, entityToSave) =>
            {
                parameterEntity = entityToSave;
                return AffectedCountPositive;
            };

            // Act
            var result = _testEntity.Save(Guid.Empty, entity);

            // Assert
            result.Status.ShouldBe(ResponseStatus.Success);
            result.Result.ShouldBe(AffectedCountPositive);
            parameterEntity.ShouldBeSameAs(entity);
        }

        [Test]
        public void DeleteByFieldMappingID_WithMappingId_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterMappingId = 0;
            ShimWorker.AllInstances.DeleteByFieldMappingIDInt32 = (_, id) =>
            {
                parameterMappingId = id;
                return AffectedCountPositive;
            };

            // Act
            var result = _testEntity.DeleteByFieldMappingID(Guid.Empty, MappingId);

            // Assert
            result.Status.ShouldBe(ResponseStatus.Success);
            result.Result.ShouldBe(AffectedCountPositive);
            parameterMappingId.ShouldBe(MappingId);
        }
    }
}
