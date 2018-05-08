using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
    /// Unit tests for <see cref="FilterSegmentationGroup"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FilterSegmentationGroupTest
    {
        private const string CommandText = "e_FilterSegmentationGroup_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.FilterSegmentationGroup _filterSegmentationGroup;

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
            _filterSegmentationGroup = typeof(Entity.FilterSegmentationGroup).CreateInstance();

            // Act
            FilterSegmentationGroup.Save(_filterSegmentationGroup, new ClientConnections());

            // Assert
            _filterSegmentationGroup.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _filterSegmentationGroup = typeof(Entity.FilterSegmentationGroup).CreateInstance(true);

            // Act
            FilterSegmentationGroup.Save(_filterSegmentationGroup, new ClientConnections());

            // Assert
            _filterSegmentationGroup.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            var fgIdSelected = string.Empty;
            var fgIdSuppressed = string.Empty;

            if (_filterSegmentationGroup.FilterGroupID_Selected != null)
            {
                fgIdSelected = _filterSegmentationGroup.FilterGroupID_Selected
                    .Aggregate(fgIdSelected, (current, i) => current + (current == string.Empty ? i.ToString() : $",{i}"));
            }

            if (_filterSegmentationGroup.FilterGroupID_Suppressed != null)
            {
                fgIdSuppressed = _filterSegmentationGroup.FilterGroupID_Suppressed
                    .Aggregate(fgIdSuppressed, (current, i) => current + (current == string.Empty ? i.ToString() : $",{i}"));
            }
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@FilterSegmentationGroupID"].Value.ShouldBe(_filterSegmentationGroup.FilterSegmentationGroupID),
                () => _saveCommand.Parameters["@FilterSegmentationID"].Value.ShouldBe(_filterSegmentationGroup.FilterSegmentationID),
                () => _saveCommand.Parameters["@FilterGroupID_Selected"].Value.ShouldBe(fgIdSelected),
                () => _saveCommand.Parameters["@FilterGroupID_Suppressed"].Value.ShouldBe(fgIdSuppressed),
                () => _saveCommand.Parameters["@SelectedOperation"].Value.ShouldBe(_filterSegmentationGroup.SelectedOperation),
                () => _saveCommand.Parameters["@SuppressedOperation"].Value.ShouldBe((object)_filterSegmentationGroup.SuppressedOperation ?? DBNull.Value));
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