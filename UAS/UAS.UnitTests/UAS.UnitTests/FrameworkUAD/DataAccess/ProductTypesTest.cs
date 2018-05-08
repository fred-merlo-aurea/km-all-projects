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
    /// Unit tests for <see cref="ProductTypes"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ProductTypesTest
    {
        private const string CommandText = "e_ProductTypes_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.ProductTypes _productTypes;

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
            _productTypes = typeof(Entity.ProductTypes).CreateInstance();

            // Act
            ProductTypes.Save(_productTypes, new ClientConnections());

            // Assert
            _productTypes.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _productTypes = typeof(Entity.ProductTypes).CreateInstance(true);

            // Act
            ProductTypes.Save(_productTypes, new ClientConnections());

            // Assert
            _productTypes.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@PubTypeID"].Value.ShouldBe(_productTypes.PubTypeID),
                () => _saveCommand.Parameters["@PubTypeDisplayName"].Value.ShouldBe(_productTypes.PubTypeDisplayName),
                () => _saveCommand.Parameters["@ColumnReference"].Value.ShouldBe(_productTypes.ColumnReference),
                () => _saveCommand.Parameters["@IsActive"].Value.ShouldBe(_productTypes.IsActive),
                () => _saveCommand.Parameters["@SortOrder"].Value.ShouldBe(_productTypes.SortOrder),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_productTypes.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_productTypes.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_productTypes.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_productTypes.UpdatedByUserID ?? DBNull.Value));
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