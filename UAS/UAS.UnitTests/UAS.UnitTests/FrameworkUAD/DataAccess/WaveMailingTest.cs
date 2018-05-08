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
    /// Unit tests for <see cref="WaveMailing"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class WaveMailingTest
    {
        private const string CommandText = "e_WaveMailing_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.WaveMailing _wave;

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
            _wave = typeof(Entity.WaveMailing).CreateInstance();

            // Act
            WaveMailing.Save(_wave, new ClientConnections());

            // Assert
            _wave.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _wave = typeof(Entity.WaveMailing).CreateInstance(true);

            // Act
            WaveMailing.Save(_wave, new ClientConnections());

            // Assert
            _wave.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@WaveMailingID"].Value.ShouldBe(_wave.WaveMailingID),
                () => _saveCommand.Parameters["@IssueID"].Value.ShouldBe(_wave.IssueID),
                () => _saveCommand.Parameters["@WaveMailingName"].Value.ShouldBe(_wave.WaveMailingName),
                () => _saveCommand.Parameters["@WaveNumber"].Value.ShouldBe(_wave.WaveNumber),
                () => _saveCommand.Parameters["@PublicationID"].Value.ShouldBe(_wave.PublicationID),
                () => _saveCommand.Parameters["@DateSubmittedToPrinter"].Value.ShouldBe((object)_wave.DateSubmittedToPrinter ?? DBNull.Value),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_wave.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_wave.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@SubmittedToPrinterByUserID"].Value.ShouldBe(_wave.SubmittedToPrinterByUserID),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_wave.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe(_wave.UpdatedByUserID));
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
