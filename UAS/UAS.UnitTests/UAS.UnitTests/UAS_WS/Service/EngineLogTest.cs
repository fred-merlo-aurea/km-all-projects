using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAS_WS.Service.Common;
using EngineEnums = FrameworkUAS.BusinessLogic.Enums.Engine;
using EntityEngineLog = FrameworkUAS.Entity.EngineLog;
using ServiceEngineLog = UAS_WS.Service.EngineLog;
using ShimWorker = FrameworkUAS.BusinessLogic.Fakes.ShimEngineLog;

namespace UAS.UnitTests.UAS_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class EngineLogTest : Fakes
    {
        private const int ClientId = 100;
        private const int EngineLogId = 200;

        private ServiceEngineLog _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceEngineLog();
        }

        [Test]
        public void UpdateIsRunningClientIdEngineEnum_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.UpdateIsRunningInt32EnumsEngineBooleanString = (a, b, c, d, e) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.UpdateIsRunningClientIdEngineEnum(Guid.Empty, ClientId, EngineEnums.ADMS, false);

            // Assert
            VerifyErrorResponse(result, false, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void UpdateIsRunningClientIdEngineEnum_ByClientId_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterClientId = 0;
            ShimWorker.AllInstances.UpdateIsRunningInt32EnumsEngineBooleanString = (a, clientId, c, d, e) =>
            {
                parameterClientId = clientId;
                return true;
            };

            // Act
            var result = _testEntity.UpdateIsRunningClientIdEngineEnum(Guid.Empty, ClientId, EngineEnums.ADMS, false);

            // Assert
            VerifySuccessResponse(result, true);
            parameterClientId.ShouldBe(ClientId);
        }

        [Test]
        public void UpdateIsRunning_ByEngineLogId_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterId = 0;
            ShimWorker.AllInstances.UpdateIsRunningInt32BooleanString = (_, id, __, ___) =>
            {
                parameterId = id;
                return true;
            };

            // Act
            var result = _testEntity.UpdateIsRunning(Guid.Empty, EngineLogId, false);

            // Assert
            VerifySuccessResponse(result, true);
            parameterId.ShouldBe(EngineLogId);
        }

        [Test]
        public void UpdateRefreshClientIdEngine_ByClientId_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterId = 0;
            ShimWorker.AllInstances.UpdateRefreshInt32String = (_, id, __) =>
            {
                parameterId = id;
                return true;
            };

            // Act
            var result = _testEntity.UpdateRefreshClientIdEngine(Guid.Empty, ClientId, string.Empty);

            // Assert
            VerifySuccessResponse(result, true);
            parameterId.ShouldBe(ClientId);
        }

        [Test]
        public void SelectAll_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityEngineLog>();
            ShimWorker.AllInstances.Select = _ => list;

            // Act
            var result = _testEntity.SelectAll(Guid.Empty);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectClientId_ByClientId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityEngineLog>();
            ShimWorker.AllInstances.SelectInt32 = (_, __) => list;

            // Act
            var result = _testEntity.SelectClientId(Guid.Empty, ClientId);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectIsRunning_ByRunningTrue_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityEngineLog>();
            ShimWorker.AllInstances.SelectBoolean = (_, __) => list;

            // Act
            var result = _testEntity.SelectIsRunning(Guid.Empty, true);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Select_ByRunningTrue_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityEngineLog();
            ShimWorker.AllInstances.SelectInt32String = (_, __, ___) => entity;

            // Act
            var result = _testEntity.Select(Guid.Empty, ClientId, string.Empty);

            // Assert
            VerifySuccessResponse(result, entity);
        }

        [Test]
        public void Save_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityEngineLog();
            ShimWorker.AllInstances.SaveEngineLog = (_, __) => true;
            ShimForJsonFunction<EntityEngineLog>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity);

            // Assert
            VerifySuccessResponse(result, true);
        }

        [Test]
        public void UpdateRefresh_ByEngineLogId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.UpdateRefreshInt32String = (_, __, ___) => true;

            // Act
            var result = _testEntity.UpdateRefresh(Guid.Empty, EngineLogId);

            // Assert
            VerifySuccessResponse(result, true);
        }

        [Test]
        public void UpdateRefreshClientIdEngineEnum_ByClientId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.UpdateRefreshInt32EnumsEngineString = (a, b, c, d) => true;

            // Act
            var result = _testEntity.UpdateRefreshClientIdEngineEnum(Guid.Empty, ClientId, EngineEnums.ADMS);

            // Assert
            VerifySuccessResponse(result, true);
        }

        [Test]
        public void UpdateIsRunningClientIdEngine_ByClientId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.UpdateIsRunningInt32StringBooleanString = (a, b, c, d, e) => true;

            // Act
            var result = _testEntity.UpdateIsRunningClientIdEngine(Guid.Empty, ClientId, string.Empty, true);

            // Assert
            VerifySuccessResponse(result, true);
        }
    }
}
