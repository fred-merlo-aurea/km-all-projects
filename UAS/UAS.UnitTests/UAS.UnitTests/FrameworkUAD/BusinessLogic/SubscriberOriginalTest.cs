using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAD.DataAccess.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAS.BusinessLogic.Fakes;
using KMPlatform.Object;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using BusinessLogicSubscriberOriginal = FrameworkUAD.BusinessLogic.SubscriberOriginal;
using EntitySubscriberOriginal = FrameworkUAD.Entity.SubscriberOriginal;
using ShimBusinessLogic = FrameworkUAD.BusinessLogic.Fakes.ShimSubscriberOriginal;

namespace UAS.UnitTests.FrameworkUAD.BusinessLogic
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SubscriberOriginalTest
    {
        private const string SampleString = "sample1";
        private const string FieldBulkSqlInsertSize = "BulkSqlInsertSize";

        private IDisposable _shimContext;
        private BusinessLogicSubscriberOriginal _testEntity;

        [SetUp]
        public void SetUp()
        {
            _shimContext = ShimsContext.Create();
            _testEntity = new BusinessLogicSubscriberOriginal();
            ShimFileLog.AllInstances.SaveFileLog = (a, b) => true;
            ShimBusinessLogic.GetEntityXmlSubscriberOriginal = _ => SampleString;
        }

        [TearDown]
        public void TearDown()
        {
            _shimContext.Dispose();
        }

        [Test]
        public void SaveBulkUpdate_WithEntityList_ReturnsTrue()
        {
            // Arrange
            var dataAccessCalled = false;
            var list = GetEntityList(1);
            ShimSubscriberOriginal.SaveBulkUpdateStringClientConnections = (a, b) =>
            {
                dataAccessCalled = true;
                return true;
            };

            // Act
            var result = _testEntity.SaveBulkUpdate(list, new ClientConnections());

            // Assert
            result.ShouldSatisfyAllConditions(
                () => dataAccessCalled.ShouldBeTrue(),
                () => result.ShouldBeTrue()
            );
        }

        [Test]
        public void SaveBulkUpdate_WorkerThrows_ReturnsFalse()
        {
            // Arrange
            var list = GetEntityList(1);
            ShimSubscriberOriginal.SaveBulkUpdateStringClientConnections = (a, b) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.SaveBulkUpdate(list, new ClientConnections());

            // Assert
            result.ShouldBeFalse();
        }

        [Test]
        public void SaveBulkInsert_WithEntityList_ReturnsTrue()
        {
            // Arrange
            var dataAccessCalled = false;
            var list = GetEntityList(1);
            ShimSubscriberOriginal.SaveBulkInsertStringClientConnections = (a, b) =>
            {
                dataAccessCalled = true;
                return true;
            };

            // Act
            var result = _testEntity.SaveBulkInsert(list, new ClientConnections());

            // Assert
            result.ShouldSatisfyAllConditions(
                () => dataAccessCalled.ShouldBeTrue(),
                () => result.ShouldBeTrue()
            );
        }

        [Test]
        public void SaveBulkInsert_WorkerThrows_ReturnsFalse()
        {
            // Arrange
            var list = GetEntityList(1);
            ShimSubscriberOriginal.SaveBulkInsertStringClientConnections = (a, b) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.SaveBulkInsert(list, new ClientConnections());

            // Assert
            result.ShouldBeFalse();
        }

        [Test]
        public void SaveBulkSqlInsert_EntityCountLessThanBatchSize_ReturnsTrue()
        {
            // Arrange
            var dataAccessCalledTimes = 0;
            var dataAccessDemoCalledTimes = 0;
            var list = GetEntityList(1);
            ShimSubscriberOriginal.SaveBulkSqlInsertListOfSubscriberOriginalClientConnections = (a, b) =>
            {
                dataAccessCalledTimes++;
                return true;
            };
            ShimSubscriberDemographicOriginal.SaveBulkSqlInsertListOfSubscriberDemographicOriginalClientConnections =
                (a, b) =>
                {
                    dataAccessDemoCalledTimes++;
                    return true;
                };

            // Act
            var result = _testEntity.SaveBulkSqlInsert(list, new ClientConnections());

            // Assert
            result.ShouldSatisfyAllConditions(
                () => dataAccessCalledTimes.ShouldBe(1),
                () => dataAccessDemoCalledTimes.ShouldBe(1),
                () => result.ShouldBeTrue()
            );
        }

        [Test]
        public void SaveBulkSqlInsert_EntityCountGreaterThanBatchSize_ReturnsTrue()
        {
            // Arrange
            const int total = 6;
            var processedEntities = 0;
            var processedDemoEntities = 0;
            var privateType = new PrivateType(_testEntity.GetType());
            privateType.SetStaticFieldOrProperty(FieldBulkSqlInsertSize, 2);
            var list = GetEntityList(total);
            ShimSubscriberOriginal.SaveBulkSqlInsertListOfSubscriberOriginalClientConnections = (entities, b) =>
            {
                processedEntities += entities.Count;
                return true;
            };
            ShimSubscriberDemographicOriginal.SaveBulkSqlInsertListOfSubscriberDemographicOriginalClientConnections =
                (entities, b) =>
                {
                    processedDemoEntities += entities.Count;
                    return true;
                };

            // Act
            var result = _testEntity.SaveBulkSqlInsert(list, new ClientConnections());

            // Assert
            result.ShouldSatisfyAllConditions(
                () => processedEntities.ShouldBe(total),
                () => processedDemoEntities.ShouldBe(total),
                () => result.ShouldBeTrue()
            );
        }

        [Test]
        public void SaveBulkSqlInsert_WorkerThrows_ReturnsFalse()
        {
            // Arrange
            var list = GetEntityList(1);
            ShimSubscriberOriginal.SaveBulkSqlInsertListOfSubscriberOriginalClientConnections = (a, b) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.SaveBulkSqlInsert(list, new ClientConnections());

            // Assert
            result.ShouldBeFalse();
        }

        private static List<EntitySubscriberOriginal> GetEntityList(int count)
        {
            var list = new List<EntitySubscriberOriginal>();

            for (var i = count; i > 0; i--)
            {
                var entity = new EntitySubscriberOriginal();
                entity.DemographicOriginalList.Add(new SubscriberDemographicOriginal());
                list.Add(entity);
            }

            return list;
        }
    }
}
