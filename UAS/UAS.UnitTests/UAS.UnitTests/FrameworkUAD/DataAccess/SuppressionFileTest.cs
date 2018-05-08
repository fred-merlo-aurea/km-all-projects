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
    /// Unit tests for <see cref="SuppressionFile"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SuppressionFileTest
    {
        private const string CommandText = "e_SuppressionFile_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.SuppressionFile _suppressionFile;

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
            _suppressionFile = typeof(Entity.SuppressionFile).CreateInstance();

            // Act
            SuppressionFile.Save(_suppressionFile, new ClientConnections());

            // Assert
            _suppressionFile.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _suppressionFile = typeof(Entity.SuppressionFile).CreateInstance(true);

            // Act
            SuppressionFile.Save(_suppressionFile, new ClientConnections());

            // Assert
            _suppressionFile.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@SuppressionFileId"].Value.ShouldBe(_suppressionFile.SuppressionFileId),
                () => _saveCommand.Parameters["@FileName"].Value.ShouldBe((object) _suppressionFile.FileName ?? DBNull.Value),
                () => _saveCommand.Parameters["@FileDateModified"].Value.ShouldBe(_suppressionFile.FileDateModified));
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