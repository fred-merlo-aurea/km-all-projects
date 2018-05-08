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
    /// Unit tests for <see cref="SecurityGroupBrandMap"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SecurityGroupBrandMapTest
    {
        private const string CommandText = "e_SecurityGroupBrandMap_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.SecurityGroupBrandMap _securityGroupBrandMap;

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
            _securityGroupBrandMap = typeof(Entity.SecurityGroupBrandMap).CreateInstance();

            // Act
            SecurityGroupBrandMap.Save(new ClientConnections(), _securityGroupBrandMap);

            // Assert
            _securityGroupBrandMap.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _securityGroupBrandMap = typeof(Entity.SecurityGroupBrandMap).CreateInstance(true);

            // Act
            SecurityGroupBrandMap.Save(new ClientConnections(), _securityGroupBrandMap);

            // Assert
            _securityGroupBrandMap.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@SecurityGroupBrandMapID"].Value.ShouldBe(_securityGroupBrandMap.SecurityGroupBrandMapID),
                () => _saveCommand.Parameters["@BrandID"].Value.ShouldBe(_securityGroupBrandMap.BrandID),
                () => _saveCommand.Parameters["@SecurityGroupID"].Value.ShouldBe(_securityGroupBrandMap.SecurityGroupID),
                () => _saveCommand.Parameters["@HasAccess"].Value.ShouldBe(_securityGroupBrandMap.HasAccess),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_securityGroupBrandMap.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_securityGroupBrandMap.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_securityGroupBrandMap.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_securityGroupBrandMap.UpdatedByUserID ?? DBNull.Value));
        }

        private void SetupFakes()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection().Instance;
            KMFakes.ShimDataFunctions.ExecuteScalarSqlCommandString = (cmd, _) =>
            {
                _saveCommand = cmd;
                return true;
            };
        }
    }
}