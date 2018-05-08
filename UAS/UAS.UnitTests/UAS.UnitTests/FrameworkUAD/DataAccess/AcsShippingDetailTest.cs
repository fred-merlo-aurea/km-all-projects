using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using KMPlatform.Object;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Entity;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit tests for <see cref="AcsShippingDetail"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AcsShippingDetailTest
    {
        private const string CommandText = "e_AcsShippingDetail_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.AcsShippingDetail _acsShippingDetail;

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
        public void Save_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            _acsShippingDetail = typeof(Entity.AcsShippingDetail).CreateInstance();

            // Act
            AcsShippingDetail.Save(_acsShippingDetail, new ClientConnections());

            // Assert
            _acsShippingDetail.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _acsShippingDetail = typeof(Entity.AcsShippingDetail).CreateInstance(true);

            // Act
            AcsShippingDetail.Save(_acsShippingDetail, new ClientConnections());

            // Assert
            _acsShippingDetail.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@AcsShippingDetailId"].Value.ShouldBe(_acsShippingDetail.AcsShippingDetailId),
                () => _saveCommand.Parameters["@CustomerNumber"].Value.ShouldBe(_acsShippingDetail.CustomerNumber),
                () => _saveCommand.Parameters["@AcsDate"].Value.ShouldBe(_acsShippingDetail.AcsDate),
                () => _saveCommand.Parameters["@ShipmentNumber"].Value.ShouldBe(_acsShippingDetail.ShipmentNumber),
                () => _saveCommand.Parameters["@AcsTypeId"].Value.ShouldBe(_acsShippingDetail.AcsTypeId),
                () => _saveCommand.Parameters["@AcsId"].Value.ShouldBe(_acsShippingDetail.AcsId),
                () => _saveCommand.Parameters["@AcsName"].Value.ShouldBe(_acsShippingDetail.AcsName),
                () => _saveCommand.Parameters["@ProductCode"].Value.ShouldBe(_acsShippingDetail.ProductCode),
                () => _saveCommand.Parameters["@Description"].Value.ShouldBe(_acsShippingDetail.Description),
                () => _saveCommand.Parameters["@Quantity"].Value.ShouldBe(_acsShippingDetail.Quantity),
                () => _saveCommand.Parameters["@UnitCost"].Value.ShouldBe(_acsShippingDetail.UnitCost),
                () => _saveCommand.Parameters["@TotalCost"].Value.ShouldBe(_acsShippingDetail.TotalCost),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_acsShippingDetail.DateCreated),
                () => _saveCommand.Parameters["@IsBilled"].Value.ShouldBe(_acsShippingDetail.IsBilled),
                () => _saveCommand.Parameters["@BilledDate"].Value.ShouldBe((object)_acsShippingDetail.BilledDate ?? DBNull.Value),
                () => _saveCommand.Parameters["@BilledByUserID"].Value.ShouldBe((object)_acsShippingDetail.BilledByUserID ?? DBNull.Value),
                () => _saveCommand.Parameters["@ProcessCode"].Value.ShouldBe(_acsShippingDetail.ProcessCode));
        }

        private void SetupFakes()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection().Instance;
            ShimDataFunctions.ExecuteScalarSqlCommand = cmd =>
            {
                _saveCommand = cmd;
                return -1;
            };
        }
    }
}