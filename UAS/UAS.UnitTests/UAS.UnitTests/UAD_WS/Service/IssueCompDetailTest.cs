using System;
using System.Diagnostics.CodeAnalysis;
using KMPlatform.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAD_WS.Service.Common;
using ServiceIssueCompDetail = UAD_WS.Service.IssueCompDetail;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimIssueCompDetail;
using ResponseStatus = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes;
using EntityIssueCompDetail = FrameworkUAD.Entity.IssueCompDetail;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class IssueCompDetailTest : Fakes
    {
        private const int IssueId = 100;

        private ServiceIssueCompDetail _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceIssueCompDetail();
        }

        [Test]
        public void Clear_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.ClearInt32ClientConnections = (_, __, ___) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Clear(Guid.Empty, IssueId, new ClientConnections());

            // Assert
            result.Status.ShouldBe(ResponseStatus.Error);
        }

        [Test]
        public void Clear_ByIssueId_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterId = 0;
            ShimWorker.AllInstances.ClearInt32ClientConnections = (_, id, ___) =>
            {
                parameterId = id;
                return false;
            };

            // Act
            var result = _testEntity.Clear(Guid.Empty, IssueId, new ClientConnections());

            // Assert
            result.Status.ShouldBe(ResponseStatus.Success);
            result.Result.ShouldBeFalse();
            parameterId.ShouldBe(IssueId);
        }
    }
}
