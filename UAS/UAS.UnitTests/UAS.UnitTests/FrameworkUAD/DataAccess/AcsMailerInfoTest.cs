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
    /// Unit tests for <see cref="AcsMailerInfo"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AcsMailerInfoTest
    {
        private const string CommandText = "e_AcsMailerInfo_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.AcsMailerInfo _acsMailerInfo;

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
            _acsMailerInfo = typeof(Entity.AcsMailerInfo).CreateInstance();

            // Act
            AcsMailerInfo.Save(_acsMailerInfo, new ClientConnections());

            // Assert
            _acsMailerInfo.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _acsMailerInfo = typeof(Entity.AcsMailerInfo).CreateInstance(true);

            // Act
            AcsMailerInfo.Save(_acsMailerInfo, new ClientConnections());

            // Assert
            _acsMailerInfo.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@AcsMailerInfoId"].Value.ShouldBe(_acsMailerInfo.AcsMailerInfoId),
                () => _saveCommand.Parameters["@AcsCode"].Value.ShouldBe(_acsMailerInfo.AcsCode),
                () => _saveCommand.Parameters["@MailerID"].Value.ShouldBe(_acsMailerInfo.MailerID),
                () => _saveCommand.Parameters["@ImbSeqCounter"].Value.ShouldBe(_acsMailerInfo.ImbSeqCounter),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_acsMailerInfo.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_acsMailerInfo.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_acsMailerInfo.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_acsMailerInfo.UpdatedByUserID ?? DBNull.Value));
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