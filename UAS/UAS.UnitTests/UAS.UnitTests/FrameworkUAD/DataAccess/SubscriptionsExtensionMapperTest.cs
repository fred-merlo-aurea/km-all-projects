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
    /// Unit tests for <see cref="SubscriptionsExtensionMapper"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SubscriptionsExtensionMapperTest
    {
        private const string CommandText = "e_SubscriptionsExtensionMapper_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.SubscriptionsExtensionMapper _subscriptionsExtensionMapper;

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
            _subscriptionsExtensionMapper = typeof(Entity.SubscriptionsExtensionMapper).CreateInstance();

            // Act
            SubscriptionsExtensionMapper.Save(_subscriptionsExtensionMapper, new ClientConnections());

            // Assert
            _subscriptionsExtensionMapper.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _subscriptionsExtensionMapper = typeof(Entity.SubscriptionsExtensionMapper).CreateInstance(true);

            // Act
            SubscriptionsExtensionMapper.Save(_subscriptionsExtensionMapper, new ClientConnections());

            // Assert
            _subscriptionsExtensionMapper.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@SubscriptionsExtensionMapperId"].Value.ShouldBe(_subscriptionsExtensionMapper.SubscriptionsExtensionMapperID),
                () => _saveCommand.Parameters["@StandardField"].Value.ShouldBe(_subscriptionsExtensionMapper.StandardField),
                () => _saveCommand.Parameters["@CustomField"].Value.ShouldBe(_subscriptionsExtensionMapper.CustomField),
                () => _saveCommand.Parameters["@CustomFieldDataType"].Value.ShouldBe(_subscriptionsExtensionMapper.CustomFieldDataType),
                () => _saveCommand.Parameters["@Active"].Value.ShouldBe(_subscriptionsExtensionMapper.Active),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_subscriptionsExtensionMapper.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_subscriptionsExtensionMapper.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_subscriptionsExtensionMapper.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_subscriptionsExtensionMapper.UpdatedByUserID ?? DBNull.Value));
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