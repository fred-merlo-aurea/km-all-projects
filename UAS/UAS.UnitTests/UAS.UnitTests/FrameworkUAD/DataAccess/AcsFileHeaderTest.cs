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
    /// Unit tests for <see cref="AcsFileHeader"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AcsFileHeaderTest
    {
        private const string CommandText = "e_AcsFileHeader_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.AcsFileHeader _fileHeader;

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
            _fileHeader = typeof(Entity.AcsFileHeader).CreateInstance();

            // Act
            AcsFileHeader.Save(_fileHeader, new ClientConnections());

            // Assert
            _fileHeader.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _fileHeader = typeof(Entity.AcsFileHeader).CreateInstance(true);

            // Act
            AcsFileHeader.Save(_fileHeader, new ClientConnections());

            // Assert
            _fileHeader.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@AcsFileHeaderId"].Value.ShouldBe(_fileHeader.AcsFileHeaderId),
                () => _saveCommand.Parameters["@RecordType"].Value.ShouldBe(_fileHeader.RecordType),
                () => _saveCommand.Parameters["@FileVersion"].Value.ShouldBe(_fileHeader.FileVersion),
                () => _saveCommand.Parameters["@CustomerID"].Value.ShouldBe(_fileHeader.CustomerID),
                () => _saveCommand.Parameters["@CreateDate"].Value.ShouldBe(_fileHeader.CreateDate),
                () => _saveCommand.Parameters["@ShipmentNumber"].Value.ShouldBe(_fileHeader.ShipmentNumber),
                () => _saveCommand.Parameters["@TotalAcsRecordCount"].Value.ShouldBe(_fileHeader.TotalAcsRecordCount),
                () => _saveCommand.Parameters["@TotalCoaCount"].Value.ShouldBe(_fileHeader.TotalCoaCount),
                () => _saveCommand.Parameters["@TotalNixieCount"].Value.ShouldBe(_fileHeader.TotalNixieCount),
                () => _saveCommand.Parameters["@TrdRecordCount"].Value.ShouldBe(_fileHeader.TrdRecordCount),
                () => _saveCommand.Parameters["@TrdAcsFeeAmount"].Value.ShouldBe(_fileHeader.TrdAcsFeeAmount),
                () => _saveCommand.Parameters["@TrdCoaCount"].Value.ShouldBe(_fileHeader.TrdCoaCount),
                () => _saveCommand.Parameters["@TrdCoaAcsFeeAmount"].Value.ShouldBe(_fileHeader.TrdCoaAcsFeeAmount),
                () => _saveCommand.Parameters["@TrdNixieCount"].Value.ShouldBe(_fileHeader.TrdNixieCount),
                () => _saveCommand.Parameters["@TrdNixieAcsFeeAmount"].Value.ShouldBe(_fileHeader.TrdNixieAcsFeeAmount),
                () => _saveCommand.Parameters["@OcdRecordCount"].Value.ShouldBe(_fileHeader.OcdRecordCount),
                () => _saveCommand.Parameters["@OcdAcsFeeAmount"].Value.ShouldBe(_fileHeader.OcdAcsFeeAmount),
                () => _saveCommand.Parameters["@OcdCoaCount"].Value.ShouldBe(_fileHeader.OcdCoaCount),
                () => _saveCommand.Parameters["@OcdCoaAcsFeeAmount"].Value.ShouldBe(_fileHeader.OcdCoaAcsFeeAmount),
                () => _saveCommand.Parameters["@OcdNixieCount"].Value.ShouldBe(_fileHeader.OcdNixieCount),
                () => _saveCommand.Parameters["@OcdNixieAcsFeeAmount"].Value.ShouldBe(_fileHeader.OcdNixieAcsFeeAmount),
                () => _saveCommand.Parameters["@FsRecordCount"].Value.ShouldBe(_fileHeader.FsRecordCount),
                () => _saveCommand.Parameters["@FsAcsFeeAmount"].Value.ShouldBe(_fileHeader.FsAcsFeeAmount),
                () => _saveCommand.Parameters["@FsCoaCount"].Value.ShouldBe(_fileHeader.FsCoaCount),
                () => _saveCommand.Parameters["@FsCoaAcsFeeAmount"].Value.ShouldBe(_fileHeader.FsCoaAcsFeeAmount),
                () => _saveCommand.Parameters["@FsNixieCount"].Value.ShouldBe(_fileHeader.FsNixieCount),
                () => _saveCommand.Parameters["@FsNixieAcsFeeAmount"].Value.ShouldBe(_fileHeader.FsNixieAcsFeeAmount),
                () => _saveCommand.Parameters["@ImpbRecordCount"].Value.ShouldBe(_fileHeader.ImpbRecordCount),
                () => _saveCommand.Parameters["@ImpbAcsFeeAmount"].Value.ShouldBe(_fileHeader.ImpbAcsFeeAmount),
                () => _saveCommand.Parameters["@ImpbCoaCount"].Value.ShouldBe(_fileHeader.ImpbCoaCount),
                () => _saveCommand.Parameters["@ImpbCoaAcsFeeAmount"].Value.ShouldBe(_fileHeader.ImpbCoaAcsFeeAmount),
                () => _saveCommand.Parameters["@ImpbNixieCount"].Value.ShouldBe(_fileHeader.ImpbNixieCount),
                () => _saveCommand.Parameters["@ImpbNixieAcsFeeAmount"].Value.ShouldBe(_fileHeader.ImpbNixieAcsFeeAmount),
                () => _saveCommand.Parameters["@Filler"].Value.ShouldBe(_fileHeader.Filler),
                () => _saveCommand.Parameters["@EndMarker"].Value.ShouldBe(_fileHeader.EndMarker),
                () => _saveCommand.Parameters["@ProcessCode"].Value.ShouldBe(_fileHeader.ProcessCode));
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
