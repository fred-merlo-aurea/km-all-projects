using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Entity;
using KMObject = KMPlatform.Object;
using KMFakes = KM.Common.Fakes;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit tests for <see cref="Product"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class ProductTest
    {
        private const string CommandText = "e_Product_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.Product _product;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _dataTable = new DataTable();

            _objWithRandomValues = typeof(Entity.Product).CreateInstance();
            _objWithDefaultValues = typeof(Entity.Product).CreateInstance(true);

            _list = new List<Entity.Product>
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
        public void Save_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            _product = typeof(Entity.Product).CreateInstance();

            // Act
            Product.Save(_product, new KMObject.ClientConnections());

            // Assert
            _product.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _product = typeof(Entity.Product).CreateInstance(true);

            // Act
            Product.Save(_product, new KMObject.ClientConnections());

            // Assert
            _product.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@PubID"].Value.ShouldBe(_product.PubID),
                () => _saveCommand.Parameters["@PubName"].Value.ShouldBe(_product.PubName),
                () => _saveCommand.Parameters["@istradeshow"].Value.ShouldBe((object)_product.istradeshow ?? DBNull.Value),
                () => _saveCommand.Parameters["@PubCode"].Value.ShouldBe((object)_product.PubCode ?? DBNull.Value),
                () => _saveCommand.Parameters["@PubTypeID"].Value.ShouldBe(_product.PubTypeID),
                () => _saveCommand.Parameters["@GroupID"].Value.ShouldBe(_product.GroupID),
                () => _saveCommand.Parameters["@EnableSearching"].Value.ShouldBe((object)_product.EnableSearching ?? DBNull.Value),
                () => _saveCommand.Parameters["@score"].Value.ShouldBe(_product.score),
                () => _saveCommand.Parameters["@SortOrder"].Value.ShouldBe(_product.SortOrder),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_product.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_product.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_product.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_product.UpdatedByUserID ?? DBNull.Value),
                () => _saveCommand.Parameters["@ClientID"].Value.ShouldBe(_product.ClientID),
                () => _saveCommand.Parameters["@YearStartDate"].Value.ShouldBe((object)_product.YearStartDate ?? DBNull.Value),
                () => _saveCommand.Parameters["@YearEndDate"].Value.ShouldBe((object)_product.YearEndDate ?? DBNull.Value),
                () => _saveCommand.Parameters["@IssueDate"].Value.ShouldBe((object)_product.IssueDate ?? DBNull.Value),
                () => _saveCommand.Parameters["@IsImported"].Value.ShouldBe((object)_product.IsImported ?? DBNull.Value),
                () => _saveCommand.Parameters["@IsActive"].Value.ShouldBe((object)_product.IsActive ?? DBNull.Value),
                () => _saveCommand.Parameters["@AllowDataEntry"].Value.ShouldBe((object)_product.AllowDataEntry ?? DBNull.Value),
                () => _saveCommand.Parameters["@FrequencyID"].Value.ShouldBe((object)_product.FrequencyID ?? DBNull.Value),
                () => _saveCommand.Parameters["@KMImportAllowed"].Value.ShouldBe((object)_product.KMImportAllowed ?? DBNull.Value),
                () => _saveCommand.Parameters["@ClientImportAllowed"].Value.ShouldBe((object)_product.ClientImportAllowed ?? DBNull.Value),
                () => _saveCommand.Parameters["@AddRemoveAllowed"].Value.ShouldBe((object)_product.AddRemoveAllowed ?? DBNull.Value),
                () => _saveCommand.Parameters["@AcsMailerInfoId"].Value.ShouldBe(_product.AcsMailerInfoId),
                () => _saveCommand.Parameters["@IsUAD"].Value.ShouldBe((object)_product.IsUAD ?? DBNull.Value),
                () => _saveCommand.Parameters["@IsCirc"].Value.ShouldBe((object)_product.IsCirc ?? DBNull.Value),
                () => _saveCommand.Parameters["@IsOpenCloseLocked"].Value.ShouldBe((object)_product.IsOpenCloseLocked ?? DBNull.Value),
                () => _saveCommand.Parameters["@HasPaidRecords"].Value.ShouldBe((object)_product.HasPaidRecords ?? DBNull.Value),
                () => _saveCommand.Parameters["@UseSubGen"].Value.ShouldBe((object)_product.UseSubGen ?? DBNull.Value));
        }

        private void SetupFakes()
        {
            var connection = new ShimSqlConnection().Instance;
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => connection;
            ShimDataFunctions.ExecuteScalarSqlCommand = cmd =>
            {
                _saveCommand = cmd;
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

            ShimSqlCommand.AllInstances.ConnectionGet = cmd => connection;
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return _list.GetSqlDataReader();
            };

            ShimSqlCommand.AllInstances.ExecuteReader = cmd =>
            {
                _sqlCommand = cmd;
                return _list.GetSqlDataReader();
            };
        }
    }
}