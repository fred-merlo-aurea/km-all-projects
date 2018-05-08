using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAS.DataAccess;
using KM.Common;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using EntityFilter = FrameworkUAS.Entity.Filter;
using ShimBuilder = KM.Common.Fakes.ShimDynamicBuilder<FrameworkUAS.Entity.Filter>;
using ShimKMCommonDataFunctions = KM.Common.Fakes.ShimDataFunctions;

namespace UAS.UnitTests.FrameworkUAS.DataAccess
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FilterTest
    {
        private IDisposable _context;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void GetList_WithSqlCommand_ExecutesCommandAndReturnsList()
        {
            // Arrange
            const int recordCount = 5;
            var command = new SqlCommand();
            var dynamicBuild = ReflectionHelper.CreateInstance<DynamicBuilder<EntityFilter>>();
            ShimBuilder.AllInstances.BuildIDataRecord = (_, __) => new EntityFilter();
            ShimBuilder.CreateBuilderIDataRecord = _ => dynamicBuild;
            SetUpForSqlDataReader(command, recordCount);

            // Act
            var result = Filter.GetList(command);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Count.ShouldBe(recordCount));
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
            ShimKMCommonDataFunctions.ExecuteReaderSqlCommand = command => reader;
            ShimKMCommonDataFunctions.ExecuteReaderNullIfEmptySqlCommand = command => reader;
            ShimKMCommonDataFunctions.ExecuteReaderSqlCommandString = (_, __) => reader;
            ShimKMCommonDataFunctions.GetSqlConnectionString = _ => sqlCommand.Connection;
        }
    }
}
