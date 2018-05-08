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
    /// Unit tests for <see cref="Issue"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class IssueTest
    {
        private const string CommandText = "e_Issue_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.Issue _issue;

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
            _issue = typeof(Entity.Issue).CreateInstance();

            // Act
            Issue.Save(_issue, new ClientConnections());

            // Assert
            _issue.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _issue = typeof(Entity.Issue).CreateInstance(true);

            // Act
            Issue.Save(_issue, new ClientConnections());

            // Assert
            _issue.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@IssueId"].Value.ShouldBe(_issue.IssueId),
                () => _saveCommand.Parameters["@PublicationId"].Value.ShouldBe(_issue.PublicationId),
                () => _saveCommand.Parameters["@IssueName"].Value.ShouldBe(_issue.IssueName),
                () => _saveCommand.Parameters["@IssueCode"].Value.ShouldBe(_issue.IssueCode),
                () => _saveCommand.Parameters["@DateOpened"].Value.ShouldBe((object)_issue.DateOpened ?? DBNull.Value),
                () => _saveCommand.Parameters["@OpenedByUserID"].Value.ShouldBe(_issue.OpenedByUserID),
                () => _saveCommand.Parameters["@IsClosed"].Value.ShouldBe(_issue.IsClosed),
                () => _saveCommand.Parameters["@DateClosed"].Value.ShouldBe((object)_issue.DateClosed ?? DBNull.Value),
                () => _saveCommand.Parameters["@ClosedByUserID"].Value.ShouldBe(_issue.ClosedByUserID),
                () => _saveCommand.Parameters["@IsComplete"].Value.ShouldBe(_issue.IsComplete),
                () => _saveCommand.Parameters["@DateComplete"].Value.ShouldBe((object)_issue.DateComplete ?? DBNull.Value),
                () => _saveCommand.Parameters["@CompleteByUserID"].Value.ShouldBe(_issue.CompleteByUserID),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_issue.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_issue.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_issue.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe(_issue.UpdatedByUserID));
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