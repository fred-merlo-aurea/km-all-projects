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
    /// Unit Tests for <see cref="SuppressionFileDetail"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SuppressionFileDetailTest
    {
        private const int Rows = 5;
        private const int SuppressionFileId = 4;
        private const string Xml = "xml";
        private const string ProcDeleteBySourceFileId = "e_SuppressionFileDetail_deleteBySourceFileId";
        private const string ProcSaveBulkInsert = "e_SuppressionFileDetail_SaveBulkInsert";
        private const string MethodGet = "Get";
        private const string MethodGetList = "GetList";
        private const string ParamSuppressionFileId = "@SuppressionFileId";
        private const string ParamXml = "@xml";
        private const string ParamSuppFileId = "@suppFileId";

        private static readonly ClientConnections Client = new ClientConnections();

        private IDisposable _context;
        private DataTable _dataTable;
        private IList<Entity.SuppressionFileDetail> _list;
        private Entity.SuppressionFileDetail _objWithRandomValues;
        private Entity.SuppressionFileDetail _objWithDefaultValues;
        private SqlCommand _sqlCommand;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _dataTable = new DataTable();

            _objWithRandomValues = typeof(Entity.SuppressionFileDetail).CreateInstance();
            _objWithDefaultValues = typeof(Entity.SuppressionFileDetail).CreateInstance(true);

            _list = new List<Entity.SuppressionFileDetail>
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
            var result = typeof(SuppressionFileDetail).CallMethod(MethodGet, new object[] { new SqlCommand() });

            // Assert
            result.ShouldBe(_objWithDefaultValues);
        }

        [Test]
        public void GetList_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = typeof(SuppressionFileDetail).CallMethod(MethodGetList, new object[] { new SqlCommand() }) as List<Entity.SuppressionFileDetail>;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue());
        }

        [Test]
        public void DeleteBySourceFileId_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var suppressionFile = new Entity.SuppressionFile
            {
                SuppressionFileId = SuppressionFileId
            };
         
            // Act
            var result = SuppressionFileDetail.deleteBySourceFileId(suppressionFile, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamSuppressionFileId].Value.ShouldBe(SuppressionFileId),
                () => result.ShouldBe(Rows),
                () => _sqlCommand.CommandText.ShouldBe(ProcDeleteBySourceFileId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SaveBulkInsert_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SuppressionFileDetail.SaveBulkInsert(Xml, SuppressionFileId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamXml].Value.ShouldBe(Xml),
                () => _sqlCommand.Parameters[ParamSuppFileId].Value.ShouldBe(SuppressionFileId),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSaveBulkInsert),
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
