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
    /// Unit tests for <see cref="SecurityGroupProductMap"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SecurityGroupProductMapTest
    {
        private const string CommandText = "e_SecurityGroupProductMap_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.SecurityGroupProductMap _securityGroupProductMap;

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
            _securityGroupProductMap = typeof(Entity.SecurityGroupProductMap).CreateInstance();

            // Act
            SecurityGroupProductMap.Save(new ClientConnections(), _securityGroupProductMap);

            // Assert
            _securityGroupProductMap.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _securityGroupProductMap = typeof(Entity.SecurityGroupProductMap).CreateInstance(true);

            // Act
            SecurityGroupProductMap.Save(new ClientConnections(), _securityGroupProductMap);

            // Assert
            _securityGroupProductMap.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@SecurityGroupProductMapID"].Value.ShouldBe(_securityGroupProductMap.SecurityGroupProductMapID),
                () => _saveCommand.Parameters["@ProductID"].Value.ShouldBe(_securityGroupProductMap.ProductID),
                () => _saveCommand.Parameters["@SecurityGroupID"].Value.ShouldBe(_securityGroupProductMap.SecurityGroupID),
                () => _saveCommand.Parameters["@HasAccess"].Value.ShouldBe(_securityGroupProductMap.HasAccess),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_securityGroupProductMap.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_securityGroupProductMap.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_securityGroupProductMap.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_securityGroupProductMap.UpdatedByUserID ?? DBNull.Value));
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