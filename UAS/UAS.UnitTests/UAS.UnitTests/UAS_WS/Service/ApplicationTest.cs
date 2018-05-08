using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAS_WS.Service.Common;
using EntityApplication = KMPlatform.Entity.Application;
using ResponseStatus = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes;
using ServiceApplication = UAS_WS.Service.Application;
using ShimWorker = KMPlatform.BusinessLogic.Fakes.ShimApplication;

namespace UAS.UnitTests.UAS_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ApplicationTest : Fakes
    {
        private const int UserId = 100;
        private const int SecurityGroupId = 200;
        private const int ServiceId = 300;
        private const int AffectedCountPositive = 2;
        private const int AffectedCountNegative = -1;
        private const string SearchValue = "sample search value";

        private ServiceApplication _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceApplication();
        }

        [Test]
        public void Select_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.Select = _ => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Select(Guid.NewGuid());

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Error);
        }

        [Test]
        public void Select_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityApplication>();
            ShimWorker.AllInstances.Select = _ =>
            {
                list.Add(new EntityApplication());
                return list;
            };

            // Act
            var result = _testEntity.Select(Guid.NewGuid());

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Success);
            list.ShouldNotBeEmpty();
        }

        [Test]
        public void SelectForUser_ByUserId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityApplication>();
            var parameterId = 0;
            ShimWorker.AllInstances.SelectForUserInt32 = (_, id) =>
            {
                parameterId = id;
                list.Add(new EntityApplication());
                return list;
            };

            // Act
            var result = _testEntity.SelectForUser(Guid.NewGuid(), UserId);

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Success);
            list.ShouldNotBeEmpty();
            parameterId.ShouldBe(UserId);
        }

        [Test]
        public void SelectForSecurityGroup_BySecurityGroupId_ReturnsErrorResponse()
        {
            // Arrange, Act
            var result = _testEntity.SelectForSecurityGroup(Guid.NewGuid(), SecurityGroupId);

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Error);
        }

        [Test]
        public void SelectForService_ByServiceId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityApplication>();
            var parameterId = 0;
            ShimWorker.AllInstances.SelectForServiceInt32 = (_, id) =>
            {
                parameterId = id;
                list.Add(new EntityApplication());
                return list;
            };

            // Act
            var result = _testEntity.SelectForService(Guid.NewGuid(), ServiceId);

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Success);
            list.ShouldNotBeEmpty();
            parameterId.ShouldBe(ServiceId);
        }

        [Test]
        public void SelectOnlyEnabledForService_ByServiceId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityApplication>();
            var parameterId = 0;
            ShimWorker.AllInstances.SelectOnlyEnabledForServiceInt32 = (_, id) =>
            {
                parameterId = id;
                list.Add(new EntityApplication());
                return list;
            };

            // Act
            var result = _testEntity.SelectOnlyEnabledForService(Guid.NewGuid(), ServiceId);

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Success);
            list.ShouldNotBeEmpty();
            parameterId.ShouldBe(ServiceId);
        }

        [Test]
        public void SelectOnlyEnabledForServiceUserID_ByServiceIdAndUserId_ReturnsErrorResponse()
        {
            // Arrange, Act
            var result = _testEntity.SelectOnlyEnabledForServiceUserID(Guid.NewGuid(), ServiceId, UserId);

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Error);
        }

        [Test]
        public void Search_BySearchValue_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityApplication>();
            var parameterSearch = string.Empty;
            ShimWorker.AllInstances.SearchStringListOfApplicationNullableOfBoolean = (a, search, c, d) =>
            {
                parameterSearch = search;
                list.Add(new EntityApplication());
                return list;
            };

            // Act
            var result = _testEntity.Search(Guid.NewGuid(), SearchValue, list);

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Success);
            list.ShouldNotBeEmpty();
            parameterSearch.ShouldBe(SearchValue);
        }

        [Test]
        public void Save_WorkerReturnsPositive_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityApplication();
            ShimWorker.AllInstances.SaveApplication = (_, __) => AffectedCountPositive;
            ShimForJsonFunction<EntityApplication>();

            // Act
            var result = _testEntity.Save(Guid.NewGuid(), entity);

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Success);
            result.Result.ShouldBe(AffectedCountPositive);
        }

        [Test]
        public void Save_WorkerReturnsNegative_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityApplication();
            ShimWorker.AllInstances.SaveApplication = (_, __) => AffectedCountNegative;
            ShimForJsonFunction<EntityApplication>();

            // Act
            var result = _testEntity.Save(Guid.NewGuid(), entity);

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResponseStatus.Error);
            result.Result.ShouldBe(AffectedCountNegative);
        }
    }
}
