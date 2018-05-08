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
    /// Unit tests for <see cref="IssueComp"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class IssueCompTest
    {
        private const string CommandText = "e_IssueComp_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.IssueComp _issueComp;

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
            _issueComp = typeof(Entity.IssueComp).CreateInstance();

            // Act
            IssueComp.Save(_issueComp, new ClientConnections());

            // Assert
            _issueComp.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _issueComp = typeof(Entity.IssueComp).CreateInstance(true);

            // Act
            IssueComp.Save(_issueComp, new ClientConnections());

            // Assert
            _issueComp.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@IssueCompId"].Value.ShouldBe(_issueComp.IssueCompId),
                () => _saveCommand.Parameters["@IssueId"].Value.ShouldBe(_issueComp.IssueId),
                () => _saveCommand.Parameters["@ImportedDate"].Value.ShouldBe(_issueComp.ImportedDate),
                () => _saveCommand.Parameters["@IssueCompCount"].Value.ShouldBe(_issueComp.IssueCompCount),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_issueComp.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_issueComp.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_issueComp.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_issueComp.UpdatedByUserID ?? DBNull.Value),
                () => _saveCommand.Parameters["@IsActive"].Value.ShouldBe(_issueComp.IsActive));
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