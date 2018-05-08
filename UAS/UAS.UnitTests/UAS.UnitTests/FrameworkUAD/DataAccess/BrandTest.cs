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
    /// Unit tests for <see cref="Brand"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class BrandTest
    {
        private const string CommandText = "e_Brand_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.Brand _brand;

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
            _brand = typeof(Entity.Brand).CreateInstance();

            // Act
            Brand.Save(_brand, new ClientConnections());

            // Assert
            _brand.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _brand = typeof(Entity.Brand).CreateInstance(true);

            // Act
            Brand.Save(_brand, new ClientConnections());

            // Assert
            _brand.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@BrandID"].Value.ShouldBe(_brand.BrandID),
                () => _saveCommand.Parameters["@BrandName"].Value.ShouldBe(_brand.BrandName),
                () => _saveCommand.Parameters["@Logo"].Value.ShouldBe(_brand.Logo),
                () => _saveCommand.Parameters["@IsBrandGroup"].Value.ShouldBe(_brand.IsBrandGroup),
                () => _saveCommand.Parameters["@IsDeleted"].Value.ShouldBe(_brand.IsDeleted),
                () => _saveCommand.Parameters["@CreatedUserID"].Value.ShouldBe(_brand.CreatedUserID),
                () => _saveCommand.Parameters["@CreatedDate"].Value.ShouldBe(_brand.CreatedDate),
                () => _saveCommand.Parameters["@UpdatedDate"].Value.ShouldBe((object)_brand.UpdatedDate ?? DBNull.Value),
                () => _saveCommand.Parameters["@UpdatedUserID"].Value.ShouldBe((object)_brand.UpdatedUserID ?? DBNull.Value));
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