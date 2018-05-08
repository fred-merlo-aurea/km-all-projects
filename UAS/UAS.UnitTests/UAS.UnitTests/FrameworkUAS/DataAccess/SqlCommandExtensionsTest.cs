using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAS.DataAccess;
using KM.Common;
using KM.Common.Fakes;
using KMPlatform.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.FrameworkUAS.DataAccess.Common;
using UAS.UnitTests.Helpers;
using ShimBuilder = KM.Common.Fakes.ShimDynamicBuilder<UAS.UnitTests.FrameworkUAS.DataAccess.Common.SampleClass>;
using ShimDataFunctions = FrameworkUAS.DataAccess.Fakes.ShimDataFunctions;
using UasSqlCommandExtensions = FrameworkUAS.DataAccess.SqlCommandExtensions;
using ShimKMCommonDataFunctions = KM.Common.Fakes.ShimDataFunctions;
using ShimKMPlatformDataFunctions = KMPlatform.DataAccess.Fakes.ShimDataFunctions;

namespace UAS.UnitTests.FrameworkUAS.DataAccess
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SqlCommandExtensionsTest : Fakes
    {
        private const string SampleString = "string";
        private const int SampleCount = 2;

        private DynamicBuilder<SampleClass> _dynamicBuilder;
        private int _recordCount = 1;
        private SqlCommand _sqlCommand;
        private SqlConnection _connection;

        [SetUp]
        public void Setup()
        {
            SetupFakes();

            _dynamicBuilder = ReflectionHelper.CreateInstance<DynamicBuilder<SampleClass>>();
            _sqlCommand = new SqlCommand();
            _connection = new SqlConnection();
            _sqlCommand.Connection = _connection;
            ShimKMPlatformDataFunctions.GetClientSqlConnectionClientConnections = _ => _connection;
            ShimBuilder.AllInstances.BuildIDataRecord = (_, __) => new SampleClass();
            ShimBuilder.CreateBuilderIDataRecord = _ => _dynamicBuilder;
            SetUpForSqlDataReader();
        }

        [TearDown]
        public void TearDown()
        {
            DisposeContext();
            _sqlCommand?.Dispose();
            _connection?.Dispose();
        }

        [Test]
        public void GetList_WithSqlCommandAndConnectionString_ReturnsList()
        {
            // Arrange
            _recordCount = SampleCount;

            // Act
            var result = UasSqlCommandExtensions.GetList<SampleClass>(_sqlCommand, SampleString);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Count.ShouldBe(SampleCount)
            );
        }

        [Test]
        public void GetList_WithSqlCommandAndClientConnection_ReturnsList()
        {
            // Arrange
            _recordCount = SampleCount;

            // Act
            var result = UasSqlCommandExtensions.GetList<SampleClass>(_sqlCommand, new ClientConnections());

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Count.ShouldBe(SampleCount)
            );
        }

        private void SetUpForSqlDataReader()
        {
            ShimSqlDataReader.ConstructorSqlCommandCommandBehavior = (a, b, c) => { };
            ShimSqlDataReader.AllInstances.Close = _ => { };
            ShimSqlDataReader.AllInstances.Read = _ =>
            {
                var result = _recordCount > 0;
                _recordCount--;
                return result;
            };

            var reader = ReflectionHelper.CreateInstance<SqlDataReader>(_sqlCommand, CommandBehavior.CloseConnection);
            ShimKMCommonDataFunctions.ExecuteReaderSqlCommand = command => reader;
            ShimKMCommonDataFunctions.ExecuteReaderNullIfEmptySqlCommand = command => reader;
            ShimKMCommonDataFunctions.ExecuteReaderSqlCommandString = (_, __) => reader;
        }
    }
}
