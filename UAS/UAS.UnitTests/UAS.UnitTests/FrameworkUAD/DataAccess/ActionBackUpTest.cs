using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using KMFakes = KM.Common.Fakes;
using ClientConnections = KMPlatform.Object.ClientConnections;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit Tests for <see cref="ActionBackUp"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ActionBackUpTest
    {
        private const int ProductId = 5;
        private const string ProcRestore = "e_ActionBackUp_Restore";
        private const string ProcBulkInsert = "e_ActionBackUp_Bulk_Insert";
        private const string ParamProductId = "@ProductID";
        private static readonly ClientConnections Client = new ClientConnections();

        private IDisposable _context;
        private SqlCommand _sqlCommand;

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
        public void Restore_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ActionBackUp.Restore(ProductId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProductId].Value.ShouldBe(ProductId),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcRestore),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Bulk_Insert_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ActionBackUp.Bulk_Insert(ProductId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProductId].Value.ShouldBe(ProductId),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcBulkInsert),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        private void SetupFakes()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection().Instance;
            KMFakes.ShimDataFunctions.ExecuteNonQuerySqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return true;
            };

            ShimSqlCommand.AllInstances.ConnectionGet = cmd => new ShimSqlConnection().Instance;
        }
    }
}