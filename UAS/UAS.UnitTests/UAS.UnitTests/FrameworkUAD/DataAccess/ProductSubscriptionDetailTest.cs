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
using KMFakes = KM.Common.Fakes;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit tests for <see cref="ProductSubscriptionDetail"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ProductSubscriptionDetailTest
    {
        private const string CommandText = "e_ProductSubscriptionDetail_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.ProductSubscriptionDetail _productSubscriptionDetail;

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
            _productSubscriptionDetail = typeof(Entity.ProductSubscriptionDetail).CreateInstance();

            // Act
            ProductSubscriptionDetail.Save(new ClientConnections(), _productSubscriptionDetail);

            // Assert
            _productSubscriptionDetail.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _productSubscriptionDetail = typeof(Entity.ProductSubscriptionDetail).CreateInstance(true);

            // Act
            ProductSubscriptionDetail.Save(new ClientConnections(), _productSubscriptionDetail);

            // Assert
            _productSubscriptionDetail.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@SubscriptionID"].Value.ShouldBe(_productSubscriptionDetail.SubscriptionID),
                () => _saveCommand.Parameters["@CodeSheetID"].Value.ShouldBe(_productSubscriptionDetail.CodeSheetID),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_productSubscriptionDetail.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_productSubscriptionDetail.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_productSubscriptionDetail.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_productSubscriptionDetail.UpdatedByUserID ?? DBNull.Value),
                () => _saveCommand.Parameters["@ResponseOther"].Value.ShouldBe(_productSubscriptionDetail.ResponseOther));
        }

        private void SetupFakes()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection().Instance;
            KMFakes.ShimDataFunctions.ExecuteNonQuerySqlCommand = cmd =>
            {
                _saveCommand = cmd;
                return true;
            };
        }
    }
}