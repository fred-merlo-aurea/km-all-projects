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
using Entity = FrameworkUAD.Object;
using KMFakes = KM.Common.Fakes;
using ClientConnections = KMPlatform.Object.ClientConnections;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit Tests for <see cref="PubCode"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class PubCodeTest
    {
        private const string DbName = "db-name";
        private const string SelectWithDbNameText = "SELECT pubcode From pubs With(NoLock)";
        private const string ProcSelect = "e_PubCode_Select";
        private const string MethodGet = "Get";
        private static readonly ClientConnections Client = new ClientConnections();

        private IDisposable _context;
        private IList<Entity.PubCode> _list;
        private Entity.PubCode _objWithRandomValues;
        private Entity.PubCode _objWithDefaultValues;
        private SqlCommand _sqlCommand;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();

            _objWithRandomValues = typeof(Entity.PubCode).CreateInstance();
            _objWithDefaultValues = typeof(Entity.PubCode).CreateInstance(true);

            _list = new List<Entity.PubCode>
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
        public void Select_WhenCalledWithDbName_VerifySqlParameters()
        {
            // Arrange, Act
            var result = PubCode.Select(DbName);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(SelectWithDbNameText),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.Text));
        }

        [Test]
        public void Select_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = PubCode.Select(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelect),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Get_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = typeof(PubCode).CallMethod(MethodGet, new object[] { new SqlCommand(), DbName });

            // Assert
            result.ShouldBe(_objWithDefaultValues);
        }

        private void SetupFakes()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection().Instance;
            ShimSqlCommand.AllInstances.ConnectionGet = cmd => new ShimSqlConnection().Instance;
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return _list.GetSqlDataReader();
            };

            KMFakes.ShimDataFunctions.ExecuteReaderSqlCommandString = (cmd, db) =>
            {
                db.ShouldBe(DbName);

                _sqlCommand = cmd;
                return _list.GetSqlDataReader();
            };
        }
    }
}