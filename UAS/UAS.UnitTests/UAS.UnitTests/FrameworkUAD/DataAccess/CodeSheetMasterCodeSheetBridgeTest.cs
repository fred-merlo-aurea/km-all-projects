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
    /// Unit tests for <see cref="CodeSheetMasterCodeSheetBridge"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CodeSheetMasterCodeSheetBridgeTest
    {
        private const string CommandText = "e_CodeSheetMasterCodeSheetBridge_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.CodeSheetMasterCodeSheetBridge _codeSheetMasterCodeSheetBridge;

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
            _codeSheetMasterCodeSheetBridge = typeof(Entity.CodeSheetMasterCodeSheetBridge).CreateInstance();

            // Act
            CodeSheetMasterCodeSheetBridge.Save(_codeSheetMasterCodeSheetBridge, new ClientConnections());

            // Assert
            _codeSheetMasterCodeSheetBridge.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _codeSheetMasterCodeSheetBridge = typeof(Entity.CodeSheetMasterCodeSheetBridge).CreateInstance(true);

            // Act
            CodeSheetMasterCodeSheetBridge.Save(_codeSheetMasterCodeSheetBridge, new ClientConnections());

            // Assert
            _codeSheetMasterCodeSheetBridge.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@CodeSheetID"].Value.ShouldBe(_codeSheetMasterCodeSheetBridge.CodeSheetID),
                () => _saveCommand.Parameters["@MasterID"].Value.ShouldBe(_codeSheetMasterCodeSheetBridge.MasterID));
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