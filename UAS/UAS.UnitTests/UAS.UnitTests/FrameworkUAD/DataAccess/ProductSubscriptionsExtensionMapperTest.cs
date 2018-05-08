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
    /// Unit tests for <see cref="ProductSubscriptionsExtensionMapper"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ProductSubscriptionsExtensionMapperTest
    {
        private const string CommandText = "e_ProductSubscriptionsExtensionMapper_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.ProductSubscriptionsExtensionMapper _productSubscriptionsExtensionMapper;

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
            _productSubscriptionsExtensionMapper = typeof(Entity.ProductSubscriptionsExtensionMapper).CreateInstance();

            // Act
            ProductSubscriptionsExtensionMapper.Save(_productSubscriptionsExtensionMapper, new ClientConnections());

            // Assert
            _productSubscriptionsExtensionMapper.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _productSubscriptionsExtensionMapper = typeof(Entity.ProductSubscriptionsExtensionMapper).CreateInstance(true);

            // Act
            ProductSubscriptionsExtensionMapper.Save(_productSubscriptionsExtensionMapper, new ClientConnections());

            // Assert
            _productSubscriptionsExtensionMapper.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@PubSubscriptionsExtensionMapperID"].Value.ShouldBe(_productSubscriptionsExtensionMapper.PubSubscriptionsExtensionMapperID),
                () => _saveCommand.Parameters["@PubID"].Value.ShouldBe(_productSubscriptionsExtensionMapper.PubID),
                () => _saveCommand.Parameters["@StandardField"].Value.ShouldBe(_productSubscriptionsExtensionMapper.StandardField),
                () => _saveCommand.Parameters["@CustomField"].Value.ShouldBe(_productSubscriptionsExtensionMapper.CustomField),
                () => _saveCommand.Parameters["@CustomFieldDataType"].Value.ShouldBe(_productSubscriptionsExtensionMapper.CustomFieldDataType),
                () => _saveCommand.Parameters["@Active"].Value.ShouldBe(_productSubscriptionsExtensionMapper.Active),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_productSubscriptionsExtensionMapper.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_productSubscriptionsExtensionMapper.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_productSubscriptionsExtensionMapper.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_productSubscriptionsExtensionMapper.UpdatedByUserID ?? DBNull.Value));
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