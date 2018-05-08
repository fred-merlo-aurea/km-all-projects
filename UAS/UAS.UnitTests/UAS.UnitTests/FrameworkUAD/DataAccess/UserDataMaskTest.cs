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
    /// Unit tests for <see cref="UserDataMask"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class UserDataMaskTest
    {
        private const string CommandText = "e_UserDataMask_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.UserDataMask _userDataMask;

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
            _userDataMask = typeof(Entity.UserDataMask).CreateInstance();

            // Act
            UserDataMask.Save(new ClientConnections(), _userDataMask);

            // Assert
            _userDataMask.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _userDataMask = typeof(Entity.UserDataMask).CreateInstance(true);

            // Act
            UserDataMask.Save(new ClientConnections(), _userDataMask);

            // Assert
            _userDataMask.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@MaskUserID"].Value.ShouldBe(_userDataMask.UserID),
                () => _saveCommand.Parameters["@MaskField"].Value.ShouldBe(_userDataMask.MaskField),
                () => _saveCommand.Parameters["@UserID"].Value.ShouldBe(_userDataMask.CreatedUserID));
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