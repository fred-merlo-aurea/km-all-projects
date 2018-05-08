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
    /// Unit Tests for <see cref="SubscriptionDetail"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SubscriptionDetailTest
    {
        private const int MasterId = 1;
        private const string ProcDeleteMasterId = "e_SubscriptionDetails_Delete_MasterID";

        private static readonly ClientConnections Client = new ClientConnections();

        private IDisposable _context;
        private IList<Entity.SubscriptionDetail> _list;
        private Entity.SubscriptionDetail _objWithRandomValues;
        private Entity.SubscriptionDetail _objWithDefaultValues;
        private SqlCommand _sqlCommand;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();

            _objWithRandomValues = typeof(Entity.SubscriptionDetail).CreateInstance();
            _objWithDefaultValues = typeof(Entity.SubscriptionDetail).CreateInstance(true);

            _list = new List<Entity.SubscriptionDetail>
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
        public void DeleteMasterID_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriptionDetail.DeleteMasterID(Client, MasterId);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@MasterID"].Value.ShouldBe(MasterId),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcDeleteMasterId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Get_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = SubscriptionDetail.Get(new SqlCommand());

            // Assert
            result.ShouldBe(_objWithDefaultValues);
        }

        [Test]
        public void GetList_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = SubscriptionDetail.GetList(new SqlCommand());

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue());
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
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return _list.GetSqlDataReader();
            };
        }
    }
}
