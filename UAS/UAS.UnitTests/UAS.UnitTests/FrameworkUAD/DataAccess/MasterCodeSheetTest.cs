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
    /// Unit tests for <see cref="MasterCodeSheet"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class MasterCodeSheetTest
    {
        private const string CommandText = "e_MasterCodeSheet_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.MasterCodeSheet _masterCodeSheet;

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
            _masterCodeSheet = typeof(Entity.MasterCodeSheet).CreateInstance();

            // Act
            MasterCodeSheet.Save(_masterCodeSheet, new ClientConnections());

            // Assert
            _masterCodeSheet.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _masterCodeSheet = typeof(Entity.MasterCodeSheet).CreateInstance(true);

            // Act
            MasterCodeSheet.Save(_masterCodeSheet, new ClientConnections());

            // Assert
            _masterCodeSheet.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@MasterID"].Value.ShouldBe(_masterCodeSheet.MasterID),
                () => _saveCommand.Parameters["@MasterGroupID"].Value.ShouldBe(_masterCodeSheet.MasterGroupID),
                () => _saveCommand.Parameters["@MasterValue"].Value.ShouldBe(_masterCodeSheet.MasterValue),
                () => _saveCommand.Parameters["@MasterDesc"].Value.ShouldBe(_masterCodeSheet.MasterDesc),
                () => _saveCommand.Parameters["@MasterDesc1"].Value.ShouldBe((object)_masterCodeSheet.MasterDesc1 ?? DBNull.Value),
                () => _saveCommand.Parameters["@EnableSearching"].Value.ShouldBe(_masterCodeSheet.EnableSearching),
                () => _saveCommand.Parameters["@SortOrder"].Value.ShouldBe(_masterCodeSheet.SortOrder),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_masterCodeSheet.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_masterCodeSheet.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_masterCodeSheet.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_masterCodeSheet.UpdatedByUserID ?? DBNull.Value));
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