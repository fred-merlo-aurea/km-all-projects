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
    /// Unit tests for <see cref="IssueSplit"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class IssueSplitTest
    {
        private const string CommandText = "e_IssueSplit_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.IssueSplit _split;

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
            _split = typeof(Entity.IssueSplit).CreateInstance();

            // Act
            IssueSplit.Save(_split, new ClientConnections());

            // Assert
            _split.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _split = typeof(Entity.IssueSplit).CreateInstance(true);

            // Act
            IssueSplit.Save(_split, new ClientConnections());

            // Assert
            _split.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@IssueSplitId"].Value.ShouldBe(_split.IssueSplitId),
                () => _saveCommand.Parameters["@IssueId"].Value.ShouldBe(_split.IssueId),
                () => _saveCommand.Parameters["@IssueSplitCode"].Value.ShouldBe(_split.IssueSplitCode),
                () => _saveCommand.Parameters["@IssueSplitName"].Value.ShouldBe(_split.IssueSplitName),
                () => _saveCommand.Parameters["@IssueSplitCount"].Value.ShouldBe(_split.IssueSplitCount),
                () => _saveCommand.Parameters["@FilterId"].Value.ShouldBe(_split.FilterId),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_split.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_split.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_split.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_split.UpdatedByUserID ?? DBNull.Value),
                () => _saveCommand.Parameters["@IsActive"].Value.ShouldBe(_split.IsActive),
                () => _saveCommand.Parameters["@KeyCode"].Value.ShouldBe((object)_split.KeyCode ?? DBNull.Value),
                () => _saveCommand.Parameters["@IssueSplitRecords"].Value.ShouldBe(_split.IssueSplitRecords),
                () => _saveCommand.Parameters["@IssueSplitDescription"].Value.ShouldBe((object)_split.IssueSplitDescription ?? DBNull.Value));
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
