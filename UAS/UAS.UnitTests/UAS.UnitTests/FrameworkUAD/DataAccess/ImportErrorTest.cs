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
    /// Unit tests for <see cref="ImportError"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ImportErrorTest
    {
        private const string CommandText = "e_ImportError_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.ImportError _importError;

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
            _importError = typeof(Entity.ImportError).CreateInstance();

            // Act
            ImportError.Save(_importError, new ClientConnections());

            // Assert
            _importError.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _importError = typeof(Entity.ImportError).CreateInstance(true);

            // Act
            ImportError.Save(_importError, new ClientConnections());

            // Assert
            _importError.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@SourceFileID"].Value.ShouldBe(_importError.SourceFileID),
                () => _saveCommand.Parameters["@RowNumber"].Value.ShouldBe(_importError.RowNumber),
                () => _saveCommand.Parameters["@FormattedException"].Value.ShouldBe((object)_importError.FormattedException ?? DBNull.Value),
                () => _saveCommand.Parameters["@ClientMessage"].Value.ShouldBe((object)_importError.ClientMessage ?? DBNull.Value),
                () => _saveCommand.Parameters["@MAFField"].Value.ShouldBe((object)_importError.MAFField ?? DBNull.Value),
                () => _saveCommand.Parameters["@BadDataRow"].Value.ShouldBe((object)_importError.BadDataRow ?? DBNull.Value),
                () => _saveCommand.Parameters["@ThreadID"].Value.ShouldBe(_importError.ThreadID),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_importError.DateCreated),
                () => _saveCommand.Parameters["@ProcessCode"].Value.ShouldBe((object)_importError.ProcessCode ?? DBNull.Value),
                () => _saveCommand.Parameters["@IsDimensionError"].Value.ShouldBe(_importError.IsDimensionError));
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