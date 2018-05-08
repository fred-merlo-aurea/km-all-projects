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
    /// Unit tests for <see cref="DownloadTemplateDetails"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class DownloadTemplateDetailsTest
    {
        private const string CommandText = "e_DownloadTemplateDetails_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.DownloadTemplateDetails _downloadTemplateDetails;

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
            _downloadTemplateDetails = typeof(Entity.DownloadTemplateDetails).CreateInstance();

            // Act
            DownloadTemplateDetails.Save(new ClientConnections(), _downloadTemplateDetails);

            // Assert
            _downloadTemplateDetails.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _downloadTemplateDetails = typeof(Entity.DownloadTemplateDetails).CreateInstance(true);

            // Act
            DownloadTemplateDetails.Save(new ClientConnections(), _downloadTemplateDetails);

            // Assert
            _downloadTemplateDetails.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@DownloadTemplateID"].Value.ShouldBe(_downloadTemplateDetails.DownloadTemplateID),
                () => _saveCommand.Parameters["@ExportColumn"].Value.ShouldBe(_downloadTemplateDetails.ExportColumn),
                () => _saveCommand.Parameters["@IsDescription"].Value.ShouldBe(_downloadTemplateDetails.IsDescription),
                () => _saveCommand.Parameters["@FieldCase"].Value.ShouldBe((object)_downloadTemplateDetails.FieldCase ?? DBNull.Value));
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