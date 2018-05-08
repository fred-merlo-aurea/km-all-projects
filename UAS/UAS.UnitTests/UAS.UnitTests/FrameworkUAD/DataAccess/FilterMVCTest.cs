using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Object;
using Shouldly;
using ClientConnections = KMPlatform.Object.ClientConnections;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit Tests for <see cref="FilterMVC"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FilterMvcTest
    {
        private const string FilterQuery = "filter-query";
        private const int FilterId = 3;
        private const int Rows = 5;
        private static readonly ClientConnections Client = new ClientConnections();

        private IDisposable _context;
        private IList<Entity.FilterMVC> _list;
        private Entity.FilterMVC _objWithRandomValues;
        private Entity.FilterMVC _objWithDefaultValues;
        private SqlCommand _sqlCommand;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();

            _objWithRandomValues = typeof(Entity.FilterMVC).CreateInstance();
            _objWithDefaultValues = typeof(Entity.FilterMVC).CreateInstance(true);

            _list = new List<Entity.FilterMVC>
            {
                _objWithRandomValues,
                _objWithDefaultValues
            };
            SetupFakes();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void ExecuteFilter_WhenCalled_VerifyReturnItem()
        {
            // Arrange
            var filterMvc = new Entity.FilterMVC();
            Entity.Subscriber subscriber = typeof(Entity.Subscriber)
                .CreateInstance();
            filterMvc.SubscriberIDs = new List<int>
            {
                subscriber.SubscriberID
            };

            var subscribers = SetSubscribers(subscriber);

            var expectedSubscribers = new List<Entity.Subscriber>
            {
                new Entity.Subscriber()
            };

            // Act
            var result = FilterMVC.ExecuteFilter(Client, FilterQuery, filterMvc);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Subscribers.IsListContentMatched(expectedSubscribers).ShouldBeTrue(),
                () => result.Count.ShouldBe(subscribers.Count),
                () => result.Executed.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(FilterQuery),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.Text));
        }

        [Test]
        public void GetFilterByID_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = FilterMVC.GetFilterByID(Client, FilterId);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.CommandText.ShouldBe($"select * from Filters where FilterId={FilterId}"),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.Text),
                () => result.ShouldBe(_objWithDefaultValues));
        }

        [Test]
        public void GetCounts_WhenCalled_VerifyReturnItem()
        {
            // Arrange
            var filterMvc = new Entity.FilterMVC();

            // Act
            var result = FilterMVC.GetCounts(Client, FilterQuery, filterMvc);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.Count.ShouldBe(Rows),
                () => filterMvc.Executed.ShouldBeTrue(),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.Text),
                () => _sqlCommand.CommandText.ShouldBe(FilterQuery));
        }

        [Test]
        public void Execute_WhenCalled_VerifyReturnItem()
        {
            // Arrange
            var filterMvc = new Entity.FilterMVC();

            // Act
            var result = FilterMVC.Execute(Client, filterMvc, FilterQuery);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.Count.ShouldBe(Rows),
                () => filterMvc.Executed.ShouldBeTrue(),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.Text),
                () => _sqlCommand.CommandText.ShouldBe(FilterQuery));
        }

        [Test]
        public void GetSubscriber_WhenCalled_VerifyReturnItem()
        {
            // Arrange
            var filterMvc = new Entity.FilterMVC();
            Entity.Subscriber subscriber = typeof(Entity.Subscriber)
                .CreateInstance();
            SetSubscribers(subscriber);

            // Act
            var result = FilterMVC.getSubscriber(Client, filterMvc, FilterQuery);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.ShouldBe(new List<int> { 0 }),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.Text),
                () => _sqlCommand.CommandText.ShouldBe(FilterQuery));
        }

        private List<Entity.Subscriber> SetSubscribers(Entity.Subscriber subscriber)
        {
            var subscribers = new List<Entity.Subscriber>
            {
                subscriber
            };
            ShimSqlCommand.AllInstances.ExecuteReader = cmd =>
            {
                _sqlCommand = cmd;
                return subscribers.GetSqlDataReader();
            };
            return subscribers;
        }

        private void SetupFakes()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection().Instance;
            ShimSqlCommand.AllInstances.ConnectionGet = cmd => new ShimSqlConnection().Instance;
            ShimDataFunctions.ExecuteScalarSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return Rows;
            };

            ShimSqlCommand.AllInstances.ExecuteScalar = cmd =>
            {
                _sqlCommand = cmd;
                return Rows;
            };

            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return _list.GetSqlDataReader();
            };
        }
    }
}