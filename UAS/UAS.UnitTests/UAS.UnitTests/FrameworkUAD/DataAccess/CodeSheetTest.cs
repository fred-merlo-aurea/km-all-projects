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
    /// Unit tests for <see cref="CodeSheet"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CodeSheetTest
    {
        private const string CommandText = "e_CodeSheet_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.CodeSheet _codeSheet;

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
            _codeSheet = typeof(Entity.CodeSheet).CreateInstance();

            // Act
            CodeSheet.Save(_codeSheet, new ClientConnections());

            // Assert
            _codeSheet.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _codeSheet = typeof(Entity.CodeSheet).CreateInstance(true);

            // Act
            CodeSheet.Save(_codeSheet, new ClientConnections());

            // Assert
            _codeSheet.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@CodeSheetID"].Value.ShouldBe(_codeSheet.CodeSheetID),
                () => _saveCommand.Parameters["@PubID"].Value.ShouldBe(_codeSheet.PubID),
                () => _saveCommand.Parameters["@ResponseGroupID"].Value.ShouldBe(_codeSheet.ResponseGroupID),
                () => _saveCommand.Parameters["@ResponseGroup"].Value.ShouldBe((object)_codeSheet.ResponseGroup ?? DBNull.Value),
                () => _saveCommand.Parameters["@ResponseValue"].Value.ShouldBe((object)_codeSheet.ResponseValue ?? DBNull.Value),
                () => _saveCommand.Parameters["@ResponseDesc"].Value.ShouldBe((object)_codeSheet.ResponseDesc ?? DBNull.Value),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_codeSheet.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_codeSheet.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_codeSheet.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_codeSheet.UpdatedByUserID ?? DBNull.Value),
                () => _saveCommand.Parameters["@DisplayOrder"].Value.ShouldBe((object)_codeSheet.DisplayOrder ?? DBNull.Value),
                () => _saveCommand.Parameters["@ReportGroupID"].Value.ShouldBe((object)_codeSheet.ReportGroupID ?? DBNull.Value),
                () => _saveCommand.Parameters["@IsActive"].Value.ShouldBe((object)_codeSheet.IsActive ?? DBNull.Value),
                () => _saveCommand.Parameters["@WQT_ResponseID"].Value.ShouldBe((object)_codeSheet.WQT_ResponseID ?? DBNull.Value),
                () => _saveCommand.Parameters["@IsOther"].Value.ShouldBe((object)_codeSheet.IsOther ?? DBNull.Value));
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