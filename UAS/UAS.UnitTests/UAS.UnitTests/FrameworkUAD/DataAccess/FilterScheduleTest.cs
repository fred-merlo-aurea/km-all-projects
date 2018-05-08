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
    /// Unit tests for <see cref="FilterSchedule"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FilterScheduleTest
    {
        private const string CommandText = "e_FilterSchedule_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.FilterSchedule _schedule;

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
            _schedule = typeof(Entity.FilterSchedule).CreateInstance();

            // Act
            FilterSchedule.Save(new ClientConnections(), _schedule);

            // Assert
            _schedule.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _schedule = typeof(Entity.FilterSchedule).CreateInstance(true);

            // Act
            FilterSchedule.Save(new ClientConnections(), _schedule);

            // Assert
            _schedule.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            var fgIDSelected = string.Empty;
            var fgIDSuppressed = string.Empty;

            if (_schedule.FilterGroupID_Selected != null)
            {
                fgIDSelected = _schedule.FilterGroupID_Selected
                    .Aggregate(fgIDSelected, (x, index) => x + (x == string.Empty ? index.ToString() : "," + index.ToString()));
            }

            if (_schedule.FilterGroupID_Suppressed != null)
            {
                fgIDSuppressed = _schedule.FilterGroupID_Suppressed
                    .Aggregate(fgIDSuppressed, (current, i) => current + (current == string.Empty ? i.ToString() : "," + i.ToString()));
            }

            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@FilterScheduleID"].Value.ShouldBe(_schedule.FilterScheduleID),
                () => _saveCommand.Parameters["@FilterID"].Value.ShouldBe(_schedule.FilterID),
                () => _saveCommand.Parameters["@ExportTypeID"].Value.ShouldBe(_schedule.ExportTypeID),
                () => _saveCommand.Parameters["@IsRecurring"].Value.ShouldBe(_schedule.IsRecurring),
                () => _saveCommand.Parameters["@RecurrenceTypeID"].Value.ShouldBe((object)_schedule.RecurrenceTypeID ?? DBNull.Value),
                () => _saveCommand.Parameters["@StartDate"].Value.ShouldBe(_schedule.StartDate),
                () => _saveCommand.Parameters["@StartTime"].Value.ShouldBe(_schedule.StartTime),
                () => _saveCommand.Parameters["@EndDate"].Value.ShouldBe((object)_schedule.EndDate ?? DBNull.Value),
                () => _saveCommand.Parameters["@RunSunday"].Value.ShouldBe(_schedule.RunSunday),
                () => _saveCommand.Parameters["@RunMonday"].Value.ShouldBe(_schedule.RunMonday),
                () => _saveCommand.Parameters["@RunTuesday"].Value.ShouldBe(_schedule.RunTuesday),
                () => _saveCommand.Parameters["@RunWednesday"].Value.ShouldBe(_schedule.RunWednesday),
                () => _saveCommand.Parameters["@RunThursday"].Value.ShouldBe(_schedule.RunThursday),
                () => _saveCommand.Parameters["@RunFriday"].Value.ShouldBe(_schedule.RunFriday),
                () => _saveCommand.Parameters["@RunSaturday"].Value.ShouldBe(_schedule.RunSaturday),
                () => _saveCommand.Parameters["@MonthScheduleDay"].Value.ShouldBe((object)_schedule.MonthScheduleDay ?? DBNull.Value),
                () => _saveCommand.Parameters["@MonthLastDay"].Value.ShouldBe(_schedule.MonthLastDay),
                () => _saveCommand.Parameters["@UserID"].Value.ShouldBe(_schedule.FilterScheduleID > 0 ? _schedule.UpdatedBy : _schedule.CreatedBy),
                () => _saveCommand.Parameters["@EmailNotification"].Value.ShouldBe(_schedule.EmailNotification),
                () => _saveCommand.Parameters["@Server"].Value.ShouldBe((object)_schedule.Server ?? DBNull.Value),
                () => _saveCommand.Parameters["@UserName"].Value.ShouldBe((object)_schedule.UserName ?? DBNull.Value),
                () => _saveCommand.Parameters["@Password"].Value.ShouldBe((object)_schedule.Password ?? DBNull.Value),
                () => _saveCommand.Parameters["@Folder"].Value.ShouldBe((object)_schedule.Folder ?? DBNull.Value),
                () => _saveCommand.Parameters["@ExportFormat"].Value.ShouldBe((object)_schedule.ExportFormat ?? DBNull.Value),
                () => _saveCommand.Parameters["@FileName"].Value.ShouldBe((object)_schedule.FileName ?? DBNull.Value),
                () => _saveCommand.Parameters["@FilterGroupID_Selected"].Value.ShouldBe(fgIDSelected),
                () => _saveCommand.Parameters["@FilterGroupID_Suppressed"].Value.ShouldBe(fgIDSuppressed),
                () => _saveCommand.Parameters["@CustomerID"].Value.ShouldBe((object)_schedule.CustomerID ?? DBNull.Value),
                () => _saveCommand.Parameters["@FolderID"].Value.ShouldBe((object)_schedule.FolderID ?? DBNull.Value),
                () => _saveCommand.Parameters["@GroupID"].Value.ShouldBe((object)_schedule.GroupID ?? DBNull.Value),
                () => _saveCommand.Parameters["@ShowHeader"].Value.ShouldBe(_schedule.ShowHeader),
                () => _saveCommand.Parameters["@AppendDateTimeToFileName"].Value.ShouldBe(_schedule.AppendDateTimeToFileName),
                () => _saveCommand.Parameters["@ExportName"].Value.ShouldBe(_schedule.ExportName),
                () => _saveCommand.Parameters["@ExportNotes"].Value.ShouldBe((object)_schedule.ExportNotes ?? DBNull.Value),
                () => _saveCommand.Parameters["@FilterSegmentationID"].Value.ShouldBe((object)_schedule.FilterSegmentationID ?? DBNull.Value),
                () => _saveCommand.Parameters["@SelectedOperation"].Value.ShouldBe((object)_schedule.SelectedOperation ?? DBNull.Value),
                () => _saveCommand.Parameters["@SuppressedOperation"].Value.ShouldBe((object)_schedule.SuppressedOperation ?? DBNull.Value));
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
