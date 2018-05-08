using System;
using System.Diagnostics.CodeAnalysis;
using KMPlatform.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAD_WS.Service.Common;
using ServiceOperations = UAD_WS.Service.Operations;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimOperations;
using ResponseStatus = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class OperationsTest : Fakes
    {
        private const string SampleCode = "code1";
        private const int SampleId = 111;

        private ServiceOperations _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceOperations();
        }

        [Test]
        public void RemovePubCode_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.RemovePubCodeClientConnectionsString = (_, __, ___) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.RemovePubCode(Guid.Empty, SampleCode, new ClientConnections());

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Error);
        }

        [Test]
        public void RemovePubCode_WithPublicationCode_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterCode = string.Empty;
            ShimWorker.AllInstances.RemovePubCodeClientConnectionsString = (_, __, code) =>
            {
                parameterCode = code;
                return false;
            };

            // Act
            var result = _testEntity.RemovePubCode(Guid.Empty, SampleCode, new ClientConnections());

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Success);
            parameterCode.ShouldBe(SampleCode);
        }

        [Test]
        public void RemoveProcessCode_WithProcessCode_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterCode = string.Empty;
            ShimWorker.AllInstances.RemoveProcessCodeClientConnectionsString = (_, __, code) =>
            {
                parameterCode = code;
                return false;
            };

            // Act
            var result = _testEntity.RemoveProcessCode(Guid.Empty, SampleCode, new ClientConnections());

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Success);
            parameterCode.ShouldBe(SampleCode);
        }

        [Test]
        public void QSourceValidation_WithFileIdAndProcessCode_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterCode = string.Empty;
            var parameterId = 0;
            ShimWorker.AllInstances.QSourceValidationClientConnectionsInt32String = (_, __, id, code) =>
            {
                parameterId = id;
                parameterCode = code;
                return false;
            };

            // Act
            var result = _testEntity.QSourceValidation(Guid.Empty, SampleId, SampleCode, new ClientConnections());

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Success);
            parameterId.ShouldBe(SampleId);
            parameterCode.ShouldBe(SampleCode);
        }

        [Test]
        public void FileValidator_QSourceValidation_WithFileIdAndProcessCode_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterCode = string.Empty;
            var parameterId = 0;
            ShimWorker.AllInstances.FileValidator_QSourceValidationClientConnectionsInt32String = (_, __, id, code) =>
            {
                parameterId = id;
                parameterCode = code;
                return false;
            };

            // Act
            var result = _testEntity.FileValidator_QSourceValidation(Guid.Empty, SampleId, SampleCode, new ClientConnections());

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Success);
            parameterId.ShouldBe(SampleId);
            parameterCode.ShouldBe(SampleCode);
        }
    }
}
