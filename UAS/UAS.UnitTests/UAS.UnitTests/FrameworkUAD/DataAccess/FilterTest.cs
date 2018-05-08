using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using KM.Common;
using KMPlatform.Object;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Entity;
using ShimBuilder = KM.Common.Fakes.ShimDynamicBuilder<FrameworkUAD.Entity.Filter>;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit tests for <see cref="Filter"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FilterTest
    {
        private const string CommandText = "e_Filter_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.Filter _filter;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            SetupFakes();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void Save_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            _filter = typeof(Entity.Filter).CreateInstance();

            // Act
            Filter.Save(_filter, new ClientConnections());

            // Assert
            _filter.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _filter = typeof(Entity.Filter).CreateInstance(true);

            // Act
            Filter.Save(_filter, new ClientConnections());

            // Assert
            _filter.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void GetList_WithSqlCommand_ExecutesCommandAndReturnsList()
        {
            // Arrange
            const int recordCount = 5;
            var command = new SqlCommand();
            var dynamicBuild = ReflectionHelper.CreateInstance<DynamicBuilder<Entity.Filter>>();
            ShimBuilder.AllInstances.BuildIDataRecord = (_, __) => new Entity.Filter();
            ShimBuilder.CreateBuilderIDataRecord = _ => dynamicBuild;
            SetUpForSqlDataReader(command, recordCount);

            // Act
            var result = Filter.GetList(command);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Count.ShouldBe(recordCount));
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@FilterID"].Value.ShouldBe(_filter.FilterID),
                () => _saveCommand.Parameters["@FilterName"].Value.ShouldBe((object)_filter.FilterName ?? DBNull.Value),
                () => _saveCommand.Parameters["@ProductID"].Value.ShouldBe(_filter.ProductID),
                () => _saveCommand.Parameters["@FilterDetails"].Value.ShouldBe((object)_filter.FilterDetails ?? DBNull.Value),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_filter.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_filter.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_filter.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_filter.UpdatedByUserID ?? DBNull.Value));
        }

        private void SetupFakes()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection().Instance;
            ShimDataFunctions.ExecuteScalarSqlCommand = cmd =>
            {
                _saveCommand = cmd;
                return -1;
            };
        }

        private static void SetUpForSqlDataReader(SqlCommand sqlCommand, int count)
        {
            var recordCount = count > 0 ? count : 0;
            sqlCommand.Connection = new ShimSqlConnection().Instance;
            ShimSqlDataReader.ConstructorSqlCommandCommandBehavior = (a, b, c) => { };
            ShimSqlDataReader.AllInstances.Close = _ => { };
            ShimSqlDataReader.AllInstances.Read = _ =>
            {
                var result = recordCount > 0;
                recordCount--;
                return result;
            };

            var reader = ReflectionHelper.CreateInstance<SqlDataReader>(sqlCommand, CommandBehavior.CloseConnection);
            KM.Common.Fakes.ShimDataFunctions.ExecuteReaderSqlCommand = command => reader;
            KM.Common.Fakes.ShimDataFunctions.ExecuteReaderNullIfEmptySqlCommand = command => reader;
            KM.Common.Fakes.ShimDataFunctions.ExecuteReaderSqlCommandString = (_, __) => reader;
        }
    }
}