using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAD.DataAccess.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAS.BusinessLogic.Fakes;
using KMPlatform.Object;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using SubscriberTransformedUnderTest = FrameworkUAD.BusinessLogic.SubscriberTransformed;

namespace UAS.UnitTests.FrameworkUAD.BusinessLogic
{
    [TestFixture]
    public class SubscriberTransformedTest
    {
        private IDisposable _shimsContext;
        private List<SubscriberTransformed> _transformed;
        private List<SubscriberDemographicTransformed> _demographicTransformed;

        [SetUp]
        public void SetUp()
        {
            _transformed = new List<SubscriberTransformed>();
            _demographicTransformed = new List<SubscriberDemographicTransformed>();

            _shimsContext = ShimsContext.Create();

            ShimSubscriberTransformed
                    .SaveBulkSqlInsertListOfSubscriberTransformedClientConnections =
                (dataList, connections) =>
                {
                    _transformed.AddRange(dataList);
                    return true;
                };

            ShimSubscriberDemographicTransformed
                    .SaveBulkSqlInsertListOfSubscriberDemographicTransformedClientConnectionsBooleanInt32String =
                (dataList, client, isDataCompare, sourceFileId, processCode) =>
                {
                    _demographicTransformed.AddRange(dataList);
                    return true;
                };

            ShimFileLog.AllInstances.SaveFileLog = (log, f) => true;
        }

        [TearDown]
        public void TearDown()
        {
            _shimsContext.Dispose();
        }

        [Test]
        public void SaveBulkSqlInsert_EmptyArguments_ReturnsTrue()
        {
            // Arrange
            var subscriberTransformed = new SubscriberTransformedUnderTest();
            var list = new List<SubscriberTransformed>();
            var client = new ClientConnections();

            // Act
            var result = subscriberTransformed.SaveBulkSqlInsert(list, client, false);

            // Assert
            result.ShouldBe(true);
        }

        [Test]
        public void SaveBulkSqlInsert_ArgumentListWithOneItem_ReturnsTrue()
        {
            // Arrange
            var subscriberTransformed = new SubscriberTransformedUnderTest();
            var list = new List<SubscriberTransformed>
            {
                new SubscriberTransformed
                {
                    DemographicTransformedList = new HashSet<SubscriberDemographicTransformed>
                    {
                        new SubscriberDemographicTransformed()
                    }
                }
            }; 
            var client = new ClientConnections();

            // Act
            var result = subscriberTransformed.SaveBulkSqlInsert(list, client, false);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(true),
                () => _transformed.ShouldBe(list),
                () => _demographicTransformed.ShouldBe(list.First().DemographicTransformedList));           
        }
    }
}