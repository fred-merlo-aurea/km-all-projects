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
using ShimKMDataFunctions = KM.Common.Fakes.ShimDataFunctions;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit tests for <see cref="DownloadTemplate"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class DownloadTemplateTest
    {
        private const string CommandText = "e_DownloadTemplate_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.DownloadTemplate _downloadTemplate;

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
            _downloadTemplate = typeof(Entity.DownloadTemplate).CreateInstance();

            // Act
            DownloadTemplate.Save(new ClientConnections(), _downloadTemplate);

            // Assert
            _downloadTemplate.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _downloadTemplate = typeof(Entity.DownloadTemplate).CreateInstance(true);

            // Act
            DownloadTemplate.Save(new ClientConnections(), _downloadTemplate);

            // Assert
            _downloadTemplate.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@DownloadTemplateID"].Value.ShouldBe(_downloadTemplate.DownloadTemplateID),
                () => _saveCommand.Parameters["@DownloadTemplateName"].Value.ShouldBe(_downloadTemplate.DownloadTemplateName),
                () => _saveCommand.Parameters["@BrandID"].Value.ShouldBe(_downloadTemplate.BrandID),
                () => _saveCommand.Parameters["@PubID"].Value.ShouldBe(_downloadTemplate.PubID),
                () => _saveCommand.Parameters["@IsDeleted"].Value.ShouldBe(_downloadTemplate.IsDeleted),
                () => _saveCommand.Parameters["@CreatedUserID"].Value.ShouldBe(_downloadTemplate.CreatedUserID),
                () => _saveCommand.Parameters["@CreatedDate"].Value.ShouldBe(_downloadTemplate.CreatedDate),
                () => _saveCommand.Parameters["@UpdatedUserID"].Value.ShouldBe(_downloadTemplate.UpdatedUserID),
                () => _saveCommand.Parameters["@UpdatedDate"].Value.ShouldBe(_downloadTemplate.UpdatedDate));
        }

        private void SetupFakes()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection().Instance;
            ShimKMDataFunctions.ExecuteScalarSqlCommandSqlConnection = (cmd, connection) =>
            {
                _saveCommand = cmd;
                return -1;
            };
        }
    }
}