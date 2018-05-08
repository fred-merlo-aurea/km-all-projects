using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using ECN_Framework_DataLayer.DomainTracker;
using ECN_Framework_DataLayer.DomainTracker.Fakes;
using ECN_Framework_DataLayer.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Framework.DataLayer.Tests.DomainTracker
{
	/// <summary>
	/// UT for <see cref="DomainTrackerActivity"/>
	/// </summary>
	[TestFixture]
	[ExcludeFromCodeCoverage]
	public partial class DomainTrackerActivityTest
	{
		private const string DomainTrackerId = "@DomainTrackerID";
		private const string StartDate = "@startDate";
		private const string StartDate1 = "@StartDate";
		private const string EndDate = "@endDate";
		private const string EndDate1 = "@EndDate";
		private const string Filter = "@Filter";
		private const string TypeFilter = "@TypeFilter";
		private const string ProfileId = "@ProfileID";
		private const string PageUrl = "@PageUrl";
		private const string CmdTextProfileId = "e_DomainTrackerActivity_Select_ProfileID";
		private const string CmdTextTotalViews = "e_DomainTrackerActivity_GetTotalViews";
		private const string CmdTextOsStats = "e_DomainTrackerActivity_GetOSStats";
		private const string CmdTextPageViews = "e_DomainTrackerActivity_GetPageViews";
		private const string CmdTextBrowserStats = "e_DomainTrackerActivity_GetBrowserStats";
		private const string CmdTextBrowserStatsKnownUnknown = "e_DomainTrackerActivity_GetBrowserStats_Known_Unknown";
		private const string CmdTextHeatMapStats = "e_DomainTrackerActivity_HeatMapStats";
		private readonly string _dateTimeString = DateTime.Now.ToString(CultureInfo.InvariantCulture);
		private IDisposable _context;

		[SetUp]
		public void TestInitialize()
		{
			_context = ShimsContext.Create();
		}

		[TearDown]
		public void TestCleanup()
		{
			_context.Dispose();
		}

		[TestCase(true)]
		[TestCase(false)]
		public void GetByProfileID_BuildSqlCommand_SetsSqlCommand(bool isEmptyParameters)
		{
			// Arrange
			SqlCommand sqlCommand = null;
			ShimDomainTrackerActivity.GetListSqlCommand = sqlCmd =>
			{
				sqlCommand = sqlCmd;
				return new List<ECN_Framework_Entities.DomainTracker.DomainTrackerActivity>();
			};

			// Act
			DomainTrackerActivity.GetByProfileID(
				1,
				2,
				isEmptyParameters ? (DateTime?) null : DateTime.Now,
				isEmptyParameters ? (DateTime?) null : DateTime.Now,
				isEmptyParameters ? null : nameof(PageUrl));

			// Assert
			if(isEmptyParameters)
			{
				sqlCommand.ShouldSatisfyAllConditions(
					() => sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
					() => sqlCommand.CommandText.ShouldBe(CmdTextProfileId),
					() => sqlCommand.Parameters.Count.ShouldBe(2),
					() => sqlCommand.Parameters[ProfileId].Value.ShouldBe(1),
					() => sqlCommand.Parameters[DomainTrackerId].Value.ShouldBe(2));
			}
			else
			{
				sqlCommand.ShouldSatisfyAllConditions(
					() => sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
					() => sqlCommand.CommandText.ShouldBe(CmdTextProfileId),
					() => sqlCommand.Parameters[ProfileId].Value.ShouldBe(1),
					() => sqlCommand.Parameters[DomainTrackerId].Value.ShouldBe(2),
					() => sqlCommand.Parameters[StartDate1].ShouldNotBeNull(),
					() => sqlCommand.Parameters[EndDate1].ShouldNotBeNull(),
					() => sqlCommand.Parameters[PageUrl].Value.ShouldBe(nameof(PageUrl)));
			}
		}

		[TestCase(true)]
		[TestCase(false)]
		public void GetTotalViews_BuildSqlCommand_SetsSqlCommand(bool isEmptyFilter)
		{
			// Arrange
			SqlCommand sqlCommand = null;
			ShimDataFunctions.GetDataTableSqlCommandString = (sqlCmd, domainTracker) =>
			{
				sqlCommand = sqlCmd;
				return new DataTable();
			};

			// Act
			DomainTrackerActivity.GetTotalViews(
				1,
				_dateTimeString,
				_dateTimeString,
				nameof(TypeFilter),
				isEmptyFilter ? string.Empty : nameof(Filter));

			// Assert
			sqlCommand.ShouldSatisfyAllConditions(
				() => sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
				() => sqlCommand.CommandText.ShouldBe(CmdTextTotalViews),
				() => sqlCommand.Parameters[DomainTrackerId].Value.ShouldBe(1),
				() => sqlCommand.Parameters[StartDate].Value.ShouldBe(_dateTimeString),
				() => sqlCommand.Parameters[EndDate].Value.ShouldBe(_dateTimeString),
				() => sqlCommand.Parameters[Filter].Value.ShouldBe(isEmptyFilter ? string.Empty : nameof(Filter)),
				() => sqlCommand.Parameters[TypeFilter].Value.ShouldBe(nameof(TypeFilter)));
		}

		[TestCase(true)]
		[TestCase(false)]
		public void GetOSStats_BuildSqlCommandParams_SetsSqlCommand(bool isEmptyFilter)
		{
			// Arrange
			SqlCommand sqlCommand = null;
			ShimDataFunctions.GetDataTableSqlCommandString = (sqlCmd, domainTracker) =>
			{
				sqlCommand = sqlCmd;
				return new DataTable();
			};

			// Act
			DomainTrackerActivity.GetOSStats(
				1,
				_dateTimeString,
				_dateTimeString,
				isEmptyFilter ? string.Empty : nameof(Filter),
				nameof(TypeFilter));

			// Assert
			sqlCommand.ShouldSatisfyAllConditions(
				() => sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
				() => sqlCommand.CommandText.ShouldBe(CmdTextOsStats),
				() => sqlCommand.Parameters[DomainTrackerId].Value.ShouldBe(1),
				() => sqlCommand.Parameters[StartDate].Value.ShouldBe(_dateTimeString),
				() => sqlCommand.Parameters[EndDate].Value.ShouldBe(_dateTimeString),
				() => sqlCommand.Parameters[Filter].Value.ShouldBe(isEmptyFilter ? string.Empty : nameof(Filter)),
				() => sqlCommand.Parameters[TypeFilter].Value.ShouldBe(nameof(TypeFilter)));
		}

		[Test]
		public void GetOSStats_BuildSqlCommand_SetsSqlCommand()
		{
			// Arrange
			SqlCommand sqlCommand = null;
			ShimDataFunctions.GetDataTableSqlCommandString = (sqlCmd, domainTracker) =>
			{
				sqlCommand = sqlCmd;
				return new DataTable();
			};

			// Act
			DomainTrackerActivity.GetOSStats(1);

			// Assert
			sqlCommand.ShouldSatisfyAllConditions(
				() => sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
				() => sqlCommand.CommandText.ShouldBe(CmdTextOsStats),
				() => sqlCommand.Parameters[DomainTrackerId].Value.ShouldBe(1));
		}

		[TestCase(true)]
		[TestCase(false)]
		public void GetPageViewsPerDay_BuildSqlCommandParams_SetsSqlCommand(bool isEmptyFilter)
		{
			// Arrange
			SqlCommand sqlCommand = null;
			ShimDataFunctions.GetDataTableSqlCommandString = (sqlCmd, domainTracker) =>
			{
				sqlCommand = sqlCmd;
				return new DataTable();
			};

			// Act
			DomainTrackerActivity.GetPageViewsPerDay(
				1,
				_dateTimeString,
				_dateTimeString,
				isEmptyFilter ? string.Empty : nameof(Filter),
				nameof(TypeFilter));

			// Assert
			sqlCommand.ShouldSatisfyAllConditions(
				() => sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
				() => sqlCommand.CommandText.ShouldBe(CmdTextPageViews),
				() => sqlCommand.Parameters[DomainTrackerId].Value.ShouldBe(1),
				() => sqlCommand.Parameters[StartDate].Value.ShouldBe(_dateTimeString),
				() => sqlCommand.Parameters[EndDate].Value.ShouldBe(_dateTimeString),
				() => sqlCommand.Parameters[Filter].Value.ShouldBe(isEmptyFilter ? string.Empty : nameof(Filter)),
				() => sqlCommand.Parameters[TypeFilter].Value.ShouldBe(nameof(TypeFilter)));
		}

		[Test]
		public void GetPageViewsPerDay_BuildSqlCommand_SetsSqlCommand()
		{
			// Arrange
			SqlCommand sqlCommand = null;
			ShimDataFunctions.GetDataTableSqlCommandString = (sqlCmd, domainTracker) =>
			{
				sqlCommand = sqlCmd;
				return new DataTable();
			};

			// Act
			DomainTrackerActivity.GetPageViewsPerDay(1);

			// Assert
			sqlCommand.ShouldSatisfyAllConditions(
				() => sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
				() => sqlCommand.CommandText.ShouldBe(CmdTextPageViews),
				() => sqlCommand.Parameters[DomainTrackerId].Value.ShouldBe(1));
		}

		[TestCase(true)]
		[TestCase(false)]
		public void GetBrowserStats_BuildSqlCommandParams_SetsSqlCommand(bool isEmptyFilter)
		{
			// Arrange
			SqlCommand sqlCommand = null;
			ShimDataFunctions.GetDataTableSqlCommandString = (sqlCmd, domainTracker) =>
			{
				sqlCommand = sqlCmd;
				return new DataTable();
			};

			// Act
			DomainTrackerActivity.GetBrowserStats(
				1,
				_dateTimeString,
				_dateTimeString,
				isEmptyFilter ? string.Empty : nameof(Filter),
				TypeFilter);

			// Assert
			sqlCommand.ShouldSatisfyAllConditions(
				() => sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
				() => sqlCommand.CommandText.ShouldBe(CmdTextBrowserStats),
				() => sqlCommand.Parameters[DomainTrackerId].Value.ShouldBe(1),
				() => sqlCommand.Parameters[StartDate].Value.ShouldBe(_dateTimeString),
				() => sqlCommand.Parameters[EndDate].Value.ShouldBe(_dateTimeString),
				() => sqlCommand.Parameters[Filter].Value.ShouldBe(isEmptyFilter ? string.Empty : nameof(Filter)),
				() => sqlCommand.Parameters[TypeFilter].Value.ShouldBe(TypeFilter));
		}

		[TestCase(true)]
		[TestCase(false)]
		public void GetBrowserStatsKnownUnknown_BuildSqlCommandParams_SetsSqlCommand(bool isEmptyFilter)
		{
			// Arrange
			SqlCommand sqlCommand = null;
			ShimDataFunctions.GetDataTableSqlCommandString = (sqlCmd, domainTracker) =>
			{
				sqlCommand = sqlCmd;
				return new DataTable();
			};

			// Act
			DomainTrackerActivity.GetBrowserStats_Known_Unknown(
				1,
				_dateTimeString,
				_dateTimeString,
				isEmptyFilter ? string.Empty : nameof(Filter));

			// Assert
			sqlCommand.ShouldSatisfyAllConditions(
				() => sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
				() => sqlCommand.CommandText.ShouldBe(CmdTextBrowserStatsKnownUnknown),
				() => sqlCommand.Parameters[DomainTrackerId].Value.ShouldBe(1),
				() => sqlCommand.Parameters[StartDate].Value.ShouldBe(_dateTimeString),
				() => sqlCommand.Parameters[EndDate].Value.ShouldBe(_dateTimeString),
				() => sqlCommand.Parameters[Filter].Value.ShouldBe(isEmptyFilter ? string.Empty : nameof(Filter)));
		}

		[TestCase(true)]
		[TestCase(false)]
		public void GetHeatMapDataTableStats_BuildSqlCommandParams_SetsSqlCommand(bool isEmptyFilter)
		{
			// Arrange
			SqlCommand sqlCommand = null;
			ShimDataFunctions.GetDataTableSqlCommandString = (sqlCmd, domainTracker) =>
			{
				sqlCommand = sqlCmd;
				return new DataTable();
			};

			// Act
			DomainTrackerActivity.GetHeatMapDataTableStats(
				1,
				_dateTimeString,
				_dateTimeString,
				isEmptyFilter ? string.Empty : nameof(Filter),
				nameof(TypeFilter));

			// Assert
			sqlCommand.ShouldSatisfyAllConditions(
				() => sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
				() => sqlCommand.CommandText.ShouldBe(CmdTextHeatMapStats),
				() => sqlCommand.Parameters[DomainTrackerId].Value.ShouldBe(1),
				() => sqlCommand.Parameters[StartDate].Value.ShouldBe(_dateTimeString),
				() => sqlCommand.Parameters[EndDate].Value.ShouldBe(_dateTimeString),
				() => sqlCommand.Parameters[Filter].Value.ShouldBe(isEmptyFilter ? string.Empty : nameof(Filter)),
				() => sqlCommand.Parameters[TypeFilter].Value.ShouldBe(nameof(TypeFilter)));
		}
	}
}