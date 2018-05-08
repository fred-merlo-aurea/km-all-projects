using System;
using System.Collections.Generic;
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
using ClientConnections = KMPlatform.Object.ClientConnections;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit Tests for <see cref="FileValidator_DemographicTransformed"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FileValidatorDemographicTransformedTest
    {
        private const string DataBase = "data-base";
        private static readonly ClientConnections Client = new ClientConnections();

        private IDisposable _context;
        private Dictionary<string, string> _bulkCopyColumns;
        private bool _bulkCopyClosed;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
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
        public void SaveBulkSqlInsert_WhenCalled_VerifyReturnItem()
        {
            // Arrange
            var validatorTransformeds = new List<Entity.FileValidator_Transformed>
            {
                typeof(Entity.FileValidator_Transformed).CreateInstance()
            };

            // Act
            var result = FileValidator_DemographicTransformed.SaveBulkSqlInsert(validatorTransformeds, Client);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => _bulkCopyClosed.ShouldBeTrue(),
                () => _bulkCopyColumns.Keys.ToArray().ShouldBe(Columns),
                () => _bulkCopyColumns.Values.ToArray().ShouldBe(Columns));
        }

        private static string[] Columns => new[]
        {
            "FV_DemographicTransformedID",
            "PubID",
            "STRecordIdentifier",
            "MAFField",
            "Value",
            "NotExists",
            "NotExistReason",
            "DateCreated",
            "DateUpdated",
            "CreatedByUserID",
            "UpdatedByUserID"
        };

        private void SetupFakes()
        {
            var sqlConnection = new ShimSqlConnection
            {
                DatabaseGet = () => DataBase
            }.Instance;

            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ =>sqlConnection;
            ShimSqlCommand.AllInstances.ConnectionGet = cmd => sqlConnection;

            ShimSqlBulkCopy.AllInstances.WriteToServerDataTable = (bulkCopy, table) =>
            {
                foreach (SqlBulkCopyColumnMapping mapping in bulkCopy.ColumnMappings)
                {
                    _bulkCopyColumns.Add(mapping.SourceColumn, mapping.DestinationColumn);
                }
            };

            ShimSqlBulkCopy.AllInstances.Close = _ => { _bulkCopyClosed = true; };
        }
    }
}