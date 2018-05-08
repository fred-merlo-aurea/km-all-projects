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
    /// Unit Tests for <see cref="FilterGroup"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FilterGroupTest
    {
        private const int Rows = 5;
        private const int FilterId = 1;
        private const int SortOrder = 2;
        private const string GetByFilterIdCommandText = "select * from filterGroup where FilterID = @FilterID order by sortorder";
        private const string ProcSave = "e_FilterGroup_Save";
        private const string ProcDeleteByFilterId = "e_FilterGroup_Delete_ByFilterID";

        private static readonly ClientConnections Client = new ClientConnections();

        private IDisposable _context;
        private IList<Entity.FilterGroup> _list;
        private Entity.FilterGroup _objWithRandomValues;
        private Entity.FilterGroup _objWithDefaultValues;
        private SqlCommand _sqlCommand;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();

            _objWithRandomValues = typeof(Entity.FilterGroup).CreateInstance();
            _objWithDefaultValues = typeof(Entity.FilterGroup).CreateInstance(true);

            _list = new List<Entity.FilterGroup>
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
        public void GetByFilterID_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = FilterGroup.getByFilterID(Client, FilterId);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@FilterID"].Value.ShouldBe(FilterId),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(GetByFilterIdCommandText),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.Text));
        }

        [Test]
        public void Save_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = FilterGroup.Save(Client, FilterId, SortOrder);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@FilterID"].Value.ShouldBe(FilterId),
                () => _sqlCommand.Parameters["@SortOrder"].Value.ShouldBe(SortOrder),
                () => result.ShouldBe(Rows),
                () => _sqlCommand.CommandText.ShouldBe(ProcSave),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void DeleteByFilterID_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            FilterGroup.Delete_ByFilterID(Client, FilterId);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@FilterID"].Value.ShouldBe(FilterId),
                () => _sqlCommand.CommandText.ShouldBe(ProcDeleteByFilterId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        private void SetupFakes()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection().Instance;
            ShimDataFunctions.ExecuteScalarSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return Rows;
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
        }
    }
}