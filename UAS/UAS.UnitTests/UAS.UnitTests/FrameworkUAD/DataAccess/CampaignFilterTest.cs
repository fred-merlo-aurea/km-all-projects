using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Entity;
using KMFakes = KM.Common.Fakes;
using ClientConnections = KMPlatform.Object.ClientConnections;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit Tests for <see cref="CampaignFilter"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CampaignFilterTest
    {
        private const int Rows = 5;
        private const int CampaignFilterId = 1;
        private const int CampaignId = 2;
        private const string FilterName = "filter-name";
        private const int UserId = 4;
        private const string PromoCode = "Promo-code";
        private const string CampaignFilterExistsCommandText = "SELECT CampaignFilterID FROM CampaignFilter WHERE filterName=@FilterName and CampaignID=@CampaignID";
        private const string ProcInsert = "e_CampaignFilter_Save";

        private static readonly ClientConnections Client = new ClientConnections();

        private IDisposable _context;
        private DataTable _dataTable;
        private IList<Entity.CampaignFilter> _list;
        private Entity.CampaignFilter _objWithRandomValues;
        private Entity.CampaignFilter _objWithDefaultValues;
        private SqlCommand _sqlCommand;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _dataTable = new DataTable();

            _objWithRandomValues = typeof(Entity.CampaignFilter).CreateInstance();
            _objWithDefaultValues = typeof(Entity.CampaignFilter).CreateInstance(true);

            _list = new List<Entity.CampaignFilter>
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
        public void Get_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = CampaignFilter.Get(Client);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue());
        }

        [Test]
        public void GetByID_WhenCalled_VerifyReturnItem()
        {
            // Arrange
            _list.First().CampaignFilterID = CampaignFilterId;

            // Act
            var result = CampaignFilter.GetByID(Client, CampaignFilterId);

            // Assert
            result.ShouldBe(_objWithRandomValues);
        }

        [Test]
        public void GetByCampaignID_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = CampaignFilter.GetByCampaignID(Client, CampaignId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue());
        }

        [Test]
        public void CampaignFilterExists_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = CampaignFilter.CampaignFilterExists(Client, FilterName, CampaignId);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@FilterName"].Value.ShouldBe(FilterName),
                () => _sqlCommand.Parameters["@CampaignID"].Value.ShouldBe(CampaignId),
                () => result.ShouldBe(Rows),
                () => _sqlCommand.CommandText.ShouldBe(CampaignFilterExistsCommandText),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.Text));
        }

        [Test]
        public void Insert_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = CampaignFilter.Insert(Client, FilterName, UserId, CampaignId, PromoCode);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@FilterName"].Value.ShouldBe(FilterName),
                () => _sqlCommand.Parameters["@CampaignID"].Value.ShouldBe(CampaignId),
                () => _sqlCommand.Parameters["@UserID"].Value.ShouldBe(UserId),
                () => _sqlCommand.Parameters["@PromoCode"].Value.ShouldBe(PromoCode),
                () => result.ShouldBe(Rows),
                () => _sqlCommand.CommandText.ShouldBe(ProcInsert),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Delete_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            CampaignFilter.Delete(Client, CampaignFilterId);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@CampaignFilterID"].Value.ShouldBe(CampaignFilterId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.Text));
        }

        private void SetupFakes()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection().Instance;
            ShimDataFunctions.ExecuteScalarSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return Rows;
            };

            KMFakes.ShimDataFunctions.GetDataTableViaAdapterSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return _dataTable;
            };

            KMFakes.ShimDataFunctions.ExecuteNonQuerySqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return true;
            };

            ShimSqlCommand.AllInstances.ConnectionGet = cmd => new ShimSqlConnection().Instance;
            ShimSqlCommand.AllInstances.ExecuteReader = cmd =>
            {
                _sqlCommand = cmd;
                return _list.GetSqlDataReader();
            };

            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return _list.GetSqlDataReader();
            };
        }
    }
}
