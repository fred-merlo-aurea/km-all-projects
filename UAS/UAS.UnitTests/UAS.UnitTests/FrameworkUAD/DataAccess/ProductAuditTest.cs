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
    /// Unit tests for <see cref="ProductAudit"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ProductAuditTest
    {
        private const string CommandText = "e_ProductAudit_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.ProductAudit _productAudit;

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
            _productAudit = typeof(Entity.ProductAudit).CreateInstance();

            // Act
            ProductAudit.Save(_productAudit, new ClientConnections());

            // Assert
            _productAudit.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _productAudit = typeof(Entity.ProductAudit).CreateInstance(true);

            // Act
            ProductAudit.Save(_productAudit, new ClientConnections());

            // Assert
            _productAudit.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@ProductAuditId"].Value.ShouldBe(_productAudit.ProductAuditId),
                () => _saveCommand.Parameters["@ProductId"].Value.ShouldBe(_productAudit.ProductId),
                () => _saveCommand.Parameters["@AuditField"].Value.ShouldBe(_productAudit.AuditField),
                () => _saveCommand.Parameters["@FieldMappingTypeId"].Value.ShouldBe(_productAudit.FieldMappingTypeId),
                () => _saveCommand.Parameters["@ResponseGroupID"].Value.ShouldBe(_productAudit.ResponseGroupID),
                () => _saveCommand.Parameters["@SubscriptionsExtensionMapperID"].Value.ShouldBe(_productAudit.SubscriptionsExtensionMapperID),
                () => _saveCommand.Parameters["@IsActive"].Value.ShouldBe(_productAudit.IsActive),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_productAudit.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_productAudit.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_productAudit.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_productAudit.UpdatedByUserID ?? DBNull.Value));
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