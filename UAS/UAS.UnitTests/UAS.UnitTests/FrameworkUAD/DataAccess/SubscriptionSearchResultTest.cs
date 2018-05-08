using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Object;
using ClientConnections = KMPlatform.Object.ClientConnections;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit Tests for <see cref="SubscriptionSearchResult"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SubscriptionSearchResultTest
    {
        private const int SubscriberId = 3;
        private const string ProcSelect = "o_SubscriptionSearchResult_Select_SubscriberID";
        private const string ProcSelectMultiple = "o_SubscriptionSearchResult_Select_SubscriberID_Multiple";
        private static readonly ClientConnections Client = new ClientConnections();

        private IDisposable _context;
        private IList<Entity.SubscriptionSearchResult> _list;
        private Entity.SubscriptionSearchResult _objWithRandomValues;
        private Entity.SubscriptionSearchResult _objWithDefaultValues;
        private SqlCommand _sqlCommand;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();

            _objWithRandomValues = typeof(Entity.SubscriptionSearchResult).CreateInstance();
            _objWithDefaultValues = typeof(Entity.SubscriptionSearchResult).CreateInstance(true);

            _list = new List<Entity.SubscriptionSearchResult>
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
        public void Select_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriptionSearchResult.Select(SubscriberId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@SubscriberID"].Value.ShouldBe(SubscriberId),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelect),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectMultiple_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var integers = new List<int> { 1, 2, 3, 4 };
            var xml = new StringBuilder();
            xml.AppendLine("<XML>");
            foreach (var x in integers)
            {
                xml.AppendLine("<SubscriptionSearchResult>");
                xml.AppendLine($"<SubscriptionID>{x}</SubscriptionID>");
                xml.AppendLine("</SubscriptionSearchResult>");
            }
            xml.AppendLine("</XML>");

            // Act
            var result = SubscriptionSearchResult.SelectMultiple(integers, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@xml"].Value.ShouldBe(xml.ToString()),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectMultiple),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        private void SetupFakes()
        {
            var connection = new ShimSqlConnection().Instance;
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => connection;
            ShimSqlCommand.AllInstances.ConnectionGet = cmd => connection;
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return _list.GetSqlDataReader();
            };
        }
    }
}