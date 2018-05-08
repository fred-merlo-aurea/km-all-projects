﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Linq;
using KMPlatform.Object;
using KMPS.MD.Objects.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using ShimDataFunctions = KM.Common.Fakes.ShimDataFunctions;
using UADShimDataFunctions = FrameworkUAD.DataAccess.Fakes.ShimDataFunctions;

namespace KMPS.MD.Objects.Tests
{
    public class MasterGroupTest
    {
        private const int MasterGroupId = 42;
        private const int ExecuteResult = 12345;
        private const string MasterGroupDeleteCommandText = "sp_MasterGroup_Delete";
        private const string MasterGroupValidateDeleteOrInactiveCommandText = "e_MasterGroup_Validate_DeleteorInActive";
        private const string MasterGroupIdKey = "@MasterGroupID";
        private const string TestDbString = "TestDb";
        private const string LiveDbString = "LiveDb";
        private const string ReferenceKey = "Reference";
        private const string ReferenceNameKey = "ReferenceName";
        private const string DummyReference = "Dummy";
        private const string DummyReferenceName = "DummyName";
        private const string ParameterKeySuffix = " : ";
        private const string FilterExportSchedule = "FILTER EXPORT SCHEDULE";
        private const string ReferenceFilter = "<a href='../main/FilterExport.aspx?FilterScheduleId="
                                               + ReferenceId2Value + "&FilterID=" + ReferenceId1Value + "'>"
                                               + ReferenceNameValue + "</a>";
        private const string ReferenceId2Value = "ReferenceID2";
        private const string ReferenceId1Value = "ReferenceID1";
        private const string ReferenceNameValue = "ReferenceName";
        private const string ReferenceId2Key = "ReferenceID2";
        private const string ReferenceId1Key = "ReferenceID1";

        private static ClientConnections clientConnections;

        private IDisposable _context;

        private static ClientConnections ClientConnections => clientConnections ?? (clientConnections = new ClientConnections(TestDbString, LiveDbString));

        private static IEnumerable<IDictionary<string, string>> TestData
        {
            get
            {
                yield return new Dictionary<string, string>()
                {
                    [ReferenceKey] = FilterExportSchedule,
                    [ReferenceId2Key] = ReferenceId2Value,
                    [ReferenceId1Key] = ReferenceId1Value,
                    [ReferenceNameKey] = ReferenceNameValue,
                };

                yield return new Dictionary<string, string>()
                {
                    [ReferenceKey] = DummyReference,
                    [ReferenceNameKey] = DummyReferenceName,
                };
            }
        }

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
        public void Delete_ValidArgument_CommandCreatedAndExecuted()
        {
            // Arrange
            SqlCommand actualSqlCommand = null;
            ClientConnections actualClientConnections = null;
            var numberOfTimesExecuteCalled = 0;
            var numberOfTimeClearCachedCalled = 0;

            ShimMasterGroup.DeleteCacheClientConnections = clientConnectionsParameter =>
            {
                numberOfTimeClearCachedCalled++;
                actualClientConnections = clientConnectionsParameter;
            };

            ShimDataFunctions.ExecuteSqlCommandSqlConnection = (command, connection) =>
            {
                actualSqlCommand = command;
                numberOfTimesExecuteCalled++;
                return ExecuteResult;
            };

            // Act
            MasterGroup.Delete(ClientConnections, MasterGroupId);

            // Assert
            actualSqlCommand.ShouldSatisfyAllConditions(
                () => actualSqlCommand.CommandText.ShouldBe(MasterGroupDeleteCommandText),
                () => actualSqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
                () => actualSqlCommand.Parameters.Count.ShouldBe(1),
                () => actualSqlCommand.Parameters[MasterGroupIdKey].Value.ShouldBe(MasterGroupId),
                () => actualSqlCommand.CommandTimeout.ShouldBe(0),
                () => numberOfTimesExecuteCalled.ShouldBe(1),
                () => numberOfTimeClearCachedCalled.ShouldBe(1),
                () => actualClientConnections.ShouldBe(ClientConnections));
        }

        [Test]
        public void ValidationForDeleteorInActive_ValidArgument_CommandExecutedCollectionReturned()
        {
            // Arrange
            SqlCommand actualSqlCommand = null;
            var numberOfTimesExecuteCalled = 0;

            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                actualSqlCommand = command;
                numberOfTimesExecuteCalled++;
                return CreateShimDataReader(TestData.ToList());
            };

            // Act
            var result = MasterGroup.ValidationForDeleteorInActive(ClientConnections, MasterGroupId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => actualSqlCommand.CommandText.ShouldBe(MasterGroupValidateDeleteOrInactiveCommandText),
                () => actualSqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
                () => actualSqlCommand.Parameters.Count.ShouldBe(1),
                () => actualSqlCommand.Parameters[MasterGroupIdKey].Value.ShouldBe(MasterGroupId),
                () => actualSqlCommand.CommandTimeout.ShouldBe(0),
                () => numberOfTimesExecuteCalled.ShouldBe(1),
                () => result.Count.ShouldBe(2),
                () => result[GetParameterName(FilterExportSchedule)].ShouldBe(ReferenceFilter),
                () => result[GetParameterName(DummyReference)].ShouldBe(DummyReferenceName));
        }

        private static SqlDataReader CreateShimDataReader(IList<IDictionary<string, string>> data)
        {
            var shimDataReader = new ShimSqlDataReader();
            var counter = -1;
            shimDataReader.Read = () => ++counter < data.Count;
            shimDataReader.ItemGetString = key => data[counter][key];

            return shimDataReader.Instance;
        }

        private static void AddCommonDbShims()
        {
            ShimSqlConnection.AllInstances.Open = _ => { };
            UADShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new SqlConnection();
        }

        private static string GetParameterName(string parameter)
        {
            return $"{parameter}{ParameterKeySuffix}";
        }
    }
}