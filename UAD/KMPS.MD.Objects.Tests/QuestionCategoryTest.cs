using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using KM.Common.Fakes;
using KMPlatform.Object;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using KMShimDataFunctions = KM.Common.Fakes.ShimDataFunctions;
using ShimDataFunctions = KMPS.MD.Objects.Fakes.ShimDataFunctions;

namespace KMPS.MD.Objects.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class QuestionCategoryTest
    {
        private const int PositiveExecuteResult = 12345;
        private const int NegativExecuteResult = -12345;
        private const string DummyDatabaseName = "MyTestDatabase";
        private const string TestDbString = "TestDb";
        private const string LiveDbString = "LiveDb";
        private const int QuestionCategoryId = 42;
        private const int ParentId = 24;
        private const string QuestionCategoryName = "MyQuestionCategoryName";
        private const string CacheKeyQuestionCategory = "QUESTIONCATEGORY";
        private const string ExistsByCategoryNameCommandText = "e_QuestionCategory_Exists_ByCategoryName";
        private const string ExistsByParentIdNameCommandText = "e_QuestionCategory_Exists_ByParentID";
        private const string CategoryNameParameterName = "@CategoryName";
        private const string CategoryIdParameterName = "@QuestionCategoryID"; 
        private const string ParentIdParameterName = "@ParentID"; 

        private static ClientConnections clientConnections;

        private IDisposable _context;

        private static ClientConnections ClientConnections => 
            clientConnections ?? (clientConnections = new ClientConnections(TestDbString, LiveDbString));

        [SetUp]
        public void Setup()
        {
            _context = ShimsContext.Create();
            AddCommonDbShims();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void DeleteCache_CacheEnabled_RemovedFromCache()
        {
            // Arrange
            var actualDatabaseNames = new HashSet<string>();
            var actualRemoveFromCache = new List<string>();

            ShimCacheUtil.IsCacheEnabled = () => true;
            ShimCacheUtil.GetFromCacheStringStringBoolean = (cacheKey, regionKey, _) => new object();

            ShimCacheUtil.RemoveFromCacheStringString = (cacheKey, dataBaseName) =>
            {
                actualRemoveFromCache.Add(cacheKey);
                actualDatabaseNames.Add(dataBaseName);
            };

            // Act
            QuestionCategory.DeleteCache(ClientConnections);
            
            // Assert
            actualRemoveFromCache.ShouldSatisfyAllConditions(
                () => actualRemoveFromCache.Count.ShouldBe(1),
                () => actualRemoveFromCache.ShouldContain(CacheKeyQuestionCategory),
                () => actualDatabaseNames.Count.ShouldBe(1),
                () => actualDatabaseNames.ShouldContain(DummyDatabaseName));
        } 

        [Test]
        public void DeleteCache_CacheNotEnabled_NothingDeleted()
        {
            // Arrange
            var actualRemoveFromCache = new List<string>();

            ShimCacheUtil.IsCacheEnabled = () => false;
            ShimCacheUtil.GetFromCacheStringStringBoolean = (cacheKey, regionKey, _) => new object();

            ShimCacheUtil.RemoveFromCacheStringString = (cacheKey, dataBaseName) =>
            {
                actualRemoveFromCache.Add(cacheKey);
            };

            // Act
            QuestionCategory.DeleteCache(ClientConnections);
            
            // Assert
            actualRemoveFromCache.ShouldSatisfyAllConditions(
                () => actualRemoveFromCache.Count.ShouldBe(0));
        }

        [Test]
        [TestCase(PositiveExecuteResult)]
        [TestCase(NegativExecuteResult)]
        public void ExistsByCategoryName_ParametersCorrect_CommandExecuted(int scalarResult)
        {
            // Arrange
            SqlCommand actualSqlCommand = null;
            var numberOfTimesExecuteCalled = 0;

            KMShimDataFunctions.ExecuteScalarSqlCommandSqlConnection = (command, connection) =>
            {
                actualSqlCommand = command;
                numberOfTimesExecuteCalled++;
                return scalarResult;
            };

            // Act
            var result = QuestionCategory.ExistsByCategoryName(ClientConnections, QuestionCategoryId, QuestionCategoryName);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(scalarResult > 0),
                () => numberOfTimesExecuteCalled.ShouldBe(1),
                () => actualSqlCommand.CommandText.ShouldBe(ExistsByCategoryNameCommandText),
                () => actualSqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
                () => actualSqlCommand.Parameters.Count.ShouldBe(2),
                () => actualSqlCommand.Parameters[CategoryIdParameterName].Value.ShouldBe(QuestionCategoryId),
                () => actualSqlCommand.Parameters[CategoryNameParameterName].Value.ShouldBe(QuestionCategoryName));
        }
        
        [Test]
        [TestCase(PositiveExecuteResult)]
        [TestCase(NegativExecuteResult)]
        public void ExistsByParentId_ParametersCorrect_CommandExecuted(int scalarResult)
        {
            // Arrange
            SqlCommand actualSqlCommand = null;
            var numberOfTimesExecuteCalled = 0;

            KMShimDataFunctions.ExecuteScalarSqlCommandSqlConnection = (command, connection) =>
            {
                actualSqlCommand = command;
                numberOfTimesExecuteCalled++;
                return scalarResult;
            };

            // Act
            var result = QuestionCategory.ExistsByParentID(ClientConnections, ParentId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(scalarResult > 0),
                () => numberOfTimesExecuteCalled.ShouldBe(1),
                () => actualSqlCommand.CommandText.ShouldBe(ExistsByParentIdNameCommandText),
                () => actualSqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
                () => actualSqlCommand.Parameters.Count.ShouldBe(1),
                () => actualSqlCommand.Parameters[ParentIdParameterName].Value.ShouldBe(ParentId));
        }

        private static void AddCommonDbShims()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new SqlConnection(); 
            ShimDataFunctions.GetDBNameClientConnections = connections => DummyDatabaseName;
            ShimSqlConnection.AllInstances.Open = _ => { }; 
        }
    }
}
