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
    /// Unit Tests for <see cref="FileValidator_ImportError"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FileValidatorImportErrorTest
    {
        private const int Rows = 5;
        private const string ProcessCode = "process-code";
        private const int SourceFileId = 1;
        private const string ProcSelect = "e_FileValidator_ImportError_Select_ProcessCode_SourceFileID";
        private const string ParamProcessCode = "@ProcessCode";
        private const string ParamSourceFileId = "@SourceFileID";
        private const string DataBase = "data-base";
        private static readonly ClientConnections Client = new ClientConnections();

        private IDisposable _context;
        private DataTable _dataTable;
        private IList<Entity.FileValidator_ImportError> _list;
        private Entity.FileValidator_ImportError _objWithRandomValues;
        private Entity.FileValidator_ImportError _objWithDefaultValues;
        private SqlCommand _sqlCommand;
        private Dictionary<string, string> _bulkCopyColumns;
        private bool _bulkCopyClosed;
        private List<Entity.ImportError> _actualList;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _dataTable = new DataTable();
            _objWithRandomValues = typeof(Entity.FileValidator_ImportError).CreateInstance();
            _objWithDefaultValues = typeof(Entity.FileValidator_ImportError).CreateInstance(true);

            _list = new List<Entity.FileValidator_ImportError>
            {
                _objWithRandomValues,
                _objWithDefaultValues
            };

            _bulkCopyClosed = false;
            _bulkCopyColumns = new Dictionary<string, string>();

            SetupFakes();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void Select_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = FileValidator_ImportError.Select(ProcessCode, SourceFileId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProcessCode].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.Parameters[ParamSourceFileId].Value.ShouldBe(SourceFileId),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelect),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetList_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = FileValidator_ImportError.GetList(new SqlCommand());

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue());
        }

        [Test]
        public void SaveBulkSqlInsert_WhenCalledWithData_WriteDataTableToTheServer()
        {
            // Arrange
            var expectedList = new List<Entity.ImportError>
            {
                typeof(Entity.ImportError).CreateInstance()
            };

            // Act
            var result = FileValidator_ImportError.SaveBulkSqlInsert(expectedList, Client);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => _bulkCopyClosed.ShouldBeTrue(),
                () => _bulkCopyColumns.Keys.ToArray().ShouldBe(Columns),
                () => _bulkCopyColumns.Values.ToArray().ShouldBe(Columns),
                () => _actualList.ShouldNotBeNull(),
                () => _actualList.IsListContentMatched(expectedList).ShouldBeTrue());
        }

        private static string[] Columns => new[]
        {
            "SourceFileID",
            "RowNumber",
            "FormattedException",
            "ClientMessage",
            "MAFField",
            "BadDataRow",
            "ThreadID",
            "DateCreated",
            "ProcessCode",
            "IsDimensionError"
        };

        private void SetupFakes()
        {
            ShimSqlBulkCopy.AllInstances.WriteToServerAsyncDataTable = (bc, table) =>
            {
                foreach (SqlBulkCopyColumnMapping mapping in bc.ColumnMappings)
                {
                    _bulkCopyColumns.Add(mapping.SourceColumn, mapping.DestinationColumn);
                }

                _actualList = table.ConvertDataTable<Entity.ImportError>();

                return null;
            };

            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection
            {
                DatabaseGet = () =>  DataBase
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

            ShimSqlBulkCopy.AllInstances.Close = bc => { _bulkCopyClosed = true; };
        }
    }
}