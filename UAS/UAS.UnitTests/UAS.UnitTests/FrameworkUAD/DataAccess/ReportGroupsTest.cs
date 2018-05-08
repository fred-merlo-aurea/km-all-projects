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
    /// Unit tests for <see cref="ReportGroups"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ReportGroupsTest
    {
        private const string CommandText = "e_ReportGroups_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.ReportGroups _reportGroups;

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
            _reportGroups = typeof(Entity.ReportGroups).CreateInstance();

            // Act
            ReportGroups.Save(new ClientConnections(), _reportGroups);

            // Assert
            _reportGroups.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _reportGroups = typeof(Entity.ReportGroups).CreateInstance(true);

            // Act
            ReportGroups.Save(new ClientConnections(), _reportGroups);

            // Assert
            _reportGroups.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@ReportGroupID"].Value.ShouldBe(_reportGroups.ReportGroupID),
                () => _saveCommand.Parameters["@ResponseGroupID"].Value.ShouldBe(_reportGroups.ResponseGroupID),
                () => _saveCommand.Parameters["@DisplayName"].Value.ShouldBe(_reportGroups.DisplayName),
                () => _saveCommand.Parameters["@DisplayOrder"].Value.ShouldBe(_reportGroups.DisplayOrder));
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