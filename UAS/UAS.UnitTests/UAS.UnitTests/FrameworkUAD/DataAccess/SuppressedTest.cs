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
    /// Unit Tests for <see cref="Suppressed"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SuppressedTest
    {
        private const int Rows = 5;
        private const string Xml = "xml";
        private const int SourceFileId = 4;
        private const string ProcessCode = "process-code";
        private const string SuppFileName = "supp-file-name";
        private const string MethodGet = "Get";
        private const string MethodGetList = "GetList";
        private const string ProcJobSuppression = "job_Suppression";
        private const string DataBase = "data-base";
        private static readonly ClientConnections Client = new ClientConnections();

        private IDisposable _context;
        private DataTable _dataTable;
        private IList<Entity.Suppressed> _list;
        private Entity.Suppressed _objWithRandomValues;
        private Entity.Suppressed _objWithDefaultValues;
        private SqlCommand _sqlCommand;
        private Dictionary<string, string> _bulkCopyColumns;
        private bool _bulkCopyClosed;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _dataTable = new DataTable();
            _bulkCopyClosed = false;
            _bulkCopyColumns = new Dictionary<string, string>();

            _objWithRandomValues = typeof(Entity.Suppressed).CreateInstance();
            _objWithDefaultValues = typeof(Entity.Suppressed).CreateInstance(true);

            _list = new List<Entity.Suppressed>
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
            var result = typeof(Suppressed).CallMethod(MethodGet, new object[] { new SqlCommand() });

            // Assert
            result.ShouldBe(_objWithDefaultValues);
        }

        [Test]
        public void GetList_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = typeof(Suppressed).CallMethod(MethodGetList, new object[] { new SqlCommand() }) as List<Entity.Suppressed>;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue());
        }

        [Test]
        public void SaveBulkSqlInsert_WhenCalled_VerifyReturnItem()
        {
            // Act
            var result = Suppressed.SaveBulkSqlInsert(new List<Entity.Suppressed>(), Client);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => _bulkCopyClosed.ShouldBeTrue(),
                () => _bulkCopyColumns.Keys.ToArray().ShouldBe(Columns),
                () => _bulkCopyColumns.Values.ToArray().ShouldBe(Columns),
                () => result.ShouldBeTrue());
        }

        [Test]
        public void PerformSuppression_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Suppressed.PerformSuppression(Xml, Client, SourceFileId, ProcessCode, SuppFileName);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@Xml"].Value.ShouldBe(Xml),
                () => _sqlCommand.Parameters["@SourceFileID"].Value.ShouldBe(SourceFileId),
                () => _sqlCommand.Parameters["@ProcessCode"].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.Parameters["@SuppFileName"].Value.ShouldBe(SuppFileName),
                () => result.ShouldBe(Rows),
                () => _sqlCommand.CommandText.ShouldBe(ProcJobSuppression),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        private static string[] Columns => new[]
        {
            "STRecordIdentifier",
            "SFRecordIdentifier",
            "Source",
            "IsSuppressed",
            "IsEmailMatch",
            "IsPhoneMatch",
            "IsAddressMatch",
            "IsCompanyMatch",
            "ProcessCode",
            "DateCreated",
            "DateUpdated",
            "UpdatedByUserID"
        };

        private void SetupFakes()
        {
            ShimSqlBulkCopy.AllInstances.WriteToServerDataTable = (bc, table) =>
            {
                foreach (SqlBulkCopyColumnMapping mapping in bc.ColumnMappings)
                {
                    _bulkCopyColumns.Add(mapping.SourceColumn, mapping.DestinationColumn);
                }
            };

            ShimSqlBulkCopy.AllInstances.Close = bc => { _bulkCopyClosed = true; };

            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection
            {
                DatabaseGet = () => DataBase
            }.Instance;
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