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
    /// Unit Tests for <see cref="DataCompareProfile"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class DataCompareProfileTest
    {
        private const int Rows = 5;
        private const string ProcessCode = "process-code";
        private const int DcTargetCodeId = 3;
        private const int Id = 4;
        private const string MatchType = "match-type";
        private const string ProcSelect = "e_DataCompareProfile_Select_ProcessCode";
        private const string ProcGetDataCompareCount = "dc_GetDataCompareCount";
        private const string ProcGetDataCompareData = "dc_GetDataCompareData";
        private const string ProcGetDataCompareSummary = "dc_GetDataCompareSummary";
        private const string ParamProcessCode = "@ProcessCode";
        private const string ParamDcTargetCodeId = "@dcTargetCodeId";
        private const string ParamId = "@id";
        private const string ParamProcessCodeLower = "@processCode";
        private const string ParamMatchType = "@MatchType";

        private static readonly ClientConnections Client = new ClientConnections();

        private IDisposable _context;
        private DataTable _dataTable;
        private IList<Entity.DataCompareProfile> _list;
        private Entity.DataCompareProfile _objWithRandomValues;
        private Entity.DataCompareProfile _objWithDefaultValues;
        private SqlCommand _sqlCommand;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _dataTable = new DataTable();

            _objWithRandomValues = typeof(Entity.DataCompareProfile).CreateInstance();
            _objWithDefaultValues = typeof(Entity.DataCompareProfile).CreateInstance(true);

            _list = new List<Entity.DataCompareProfile>
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
            var result = DataCompareProfile.Get(new SqlCommand());

            // Assert
            result.ShouldBe(_objWithDefaultValues);
        }

        [Test]
        public void GetList_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = DataCompareProfile.GetList(new SqlCommand());

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue());
        }

        [Test]
        public void Select_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = DataCompareProfile.Select(Client, ProcessCode);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProcessCode].Value.ShouldBe(ProcessCode),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelect),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetDataCompareCount_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = DataCompareProfile.GetDataCompareCount(Client, ProcessCode, DcTargetCodeId, Id);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProcessCode].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.Parameters[ParamDcTargetCodeId].Value.ShouldBe(DcTargetCodeId),
                () => _sqlCommand.Parameters[ParamId].Value.ShouldBe(Id),
                () => result.ShouldBe(Rows),
                () => _sqlCommand.CommandText.ShouldBe(ProcGetDataCompareCount),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetDataCompareData_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = DataCompareProfile.GetDataCompareData(Client, ProcessCode, DcTargetCodeId, MatchType, Id);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProcessCodeLower].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.Parameters[ParamDcTargetCodeId].Value.ShouldBe(DcTargetCodeId),
                () => _sqlCommand.Parameters[ParamId].Value.ShouldBe(Id),
                () => _sqlCommand.Parameters[ParamMatchType].Value.ShouldBe(MatchType),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcGetDataCompareData),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetDataCompareSummary_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = DataCompareProfile.GetDataCompareSummary(Client, ProcessCode, DcTargetCodeId, MatchType, Id);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProcessCodeLower].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.Parameters[ParamDcTargetCodeId].Value.ShouldBe(DcTargetCodeId),
                () => _sqlCommand.Parameters[ParamId].Value.ShouldBe(Id),
                () => _sqlCommand.Parameters[ParamMatchType].Value.ShouldBe(MatchType),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcGetDataCompareSummary),
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
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return _list.GetSqlDataReader();
            };
        }
    }
}