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
using Entity = FrameworkUAD.Object;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit tests for <see cref="FilterDetails"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FilterDetailsTest
    {
        private const string CommandText = "e_FilterDetails_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.FilterDetails _filterDetails;

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
            _filterDetails = typeof(Entity.FilterDetails).CreateInstance();

            // Act
            FilterDetails.Save(new ClientConnections(), _filterDetails);

            // Assert
            _filterDetails.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _filterDetails = typeof(Entity.FilterDetails).CreateInstance(true);

            // Act
            FilterDetails.Save(new ClientConnections(), _filterDetails);

            // Assert
            _filterDetails.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@FilterDetailsID"].Value.ShouldBe(_filterDetails.FilterDetailsID),
                () => _saveCommand.Parameters["@FilterType"].Value.ShouldBe(_filterDetails.FilterType),
                () => _saveCommand.Parameters["@Group"].Value.ShouldBe(_filterDetails.Group),
                () => _saveCommand.Parameters["@Name"].Value.ShouldBe(_filterDetails.Name),
                () => _saveCommand.Parameters["@Values"].Value.ShouldBe(_filterDetails.Values),
                () => _saveCommand.Parameters["@SearchCondition"].Value.ShouldBe(_filterDetails.SearchCondition),
                () => _saveCommand.Parameters["@FilterGroupID"].Value.ShouldBe(_filterDetails.FilterGroupID));
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