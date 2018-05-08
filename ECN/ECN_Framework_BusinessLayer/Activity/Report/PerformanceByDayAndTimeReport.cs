using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ActivityReport = ECN_Framework_Entities.Activity.Report.PerformanceByDayAndTimeReport;
using DataLayerPerformanceByDayAndTimeReport = ECN_Framework_DataLayer.Activity.Report.PerformanceByDayAndTimeReport;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    public class PerformanceByDayAndTimeReport
    {
        private const string DayGroupMon = "mon";
        private const string DayGroupTue = "tue";
        private const string DayGroupWed = "wed";
        private const string DayGroupThur = "thur";
        private const string DayGroupFri = "fri";
        private const string DayGroupSat = "sat";
        private const string DayGroupSun = "sun";
        private const string ColumnNameSort = "Sort";
        private const string ColumnNameTimes = "Times";
        private const string ColumnNameMonday = "Monday";
        private const string ColumnNameTuesday = "Tuesday";
        private const string ColumnNameWednesday = "Wednesday";
        private const string ColumnNameThursday = "Thursday";
        private const string ColumnNameFriday = "Friday";
        private const string ColumnNameSaturday = "Saturday";
        private const string ColumnNameSunday = "Sunday";
        private const string MetricToShowOpens = "opens";

        public static List<ActivityReport> Get(int customerId, DateTime startDate, DateTime endDate, string filterOne, int filterOneValue, string filterTwo, int? filterTwoValue)
        {
            return DataLayerPerformanceByDayAndTimeReport.Get(customerId, startDate, endDate, filterOne, filterOneValue, filterTwo, filterTwoValue);
        }

        public static DataTable GetWeekDataTable(string metricToShow, DateTime startDate, DateTime endDate, string filter1, int filter1Value, string filter2, int filter2Value, int customerId)
        {
            var performanceReports = Get(customerId, startDate, endDate, filter1, filter1Value, filter2, filter2Value);

            var monday = SelectFromPerformanceReports(performanceReports, DayGroupMon).ToList();
            var tuesday = SelectFromPerformanceReports(performanceReports, DayGroupTue).ToList();
            var wednesday = SelectFromPerformanceReports(performanceReports, DayGroupWed).ToList();
            var thursday = SelectFromPerformanceReports(performanceReports, DayGroupThur).ToList();
            var friday = SelectFromPerformanceReports(performanceReports, DayGroupFri).ToList();
            var saturday = SelectFromPerformanceReports(performanceReports, DayGroupSat).ToList();
            var sunday = SelectFromPerformanceReports(performanceReports, DayGroupSun).ToList();

            var weekDataTable = CreateDataTable();

            weekDataTable = metricToShow == MetricToShowOpens
                                ? AddRows(weekDataTable, monday, tuesday, wednesday, thursday, friday, saturday, sunday, report => report.Opens)
                                : AddRows(weekDataTable, monday, tuesday, wednesday, thursday, friday, saturday, sunday, report => report.Clicks);



            return weekDataTable;
        }

        private static DataTable CreateDataTable()
        {
            var weekDataTable = new DataTable();
            weekDataTable.Columns.Add(new DataColumn(ColumnNameSort, typeof(int)));
            weekDataTable.Columns.Add(new DataColumn(ColumnNameTimes));
            weekDataTable.Columns.Add(new DataColumn(ColumnNameMonday, typeof(double)));
            weekDataTable.Columns.Add(new DataColumn(ColumnNameTuesday, typeof(double)));
            weekDataTable.Columns.Add(new DataColumn(ColumnNameWednesday, typeof(double)));
            weekDataTable.Columns.Add(new DataColumn(ColumnNameThursday, typeof(double)));
            weekDataTable.Columns.Add(new DataColumn(ColumnNameFriday, typeof(double)));
            weekDataTable.Columns.Add(new DataColumn(ColumnNameSaturday, typeof(double)));
            weekDataTable.Columns.Add(new DataColumn(ColumnNameSunday, typeof(double)));

            return weekDataTable;
        }

        private static DataTable AddRows(
            DataTable weekDataTable,
            IReadOnlyList<ActivityReport> monday,
            IReadOnlyList<ActivityReport> tuesday,
            IReadOnlyList<ActivityReport> wednesday,
            IReadOnlyList<ActivityReport> thursday,
            IReadOnlyList<ActivityReport> friday,
            IReadOnlyList<ActivityReport> saturday,
            IReadOnlyList<ActivityReport> sunday,
            Func<ActivityReport, string> selectFunc)
        {
            foreach (var dayIndex in Enumerable.Range(0, 9))
            {
                weekDataTable.Rows.Add(
                    dayIndex,
                    monday[dayIndex].HourGroup,
                    selectFunc(monday[dayIndex]),
                    selectFunc(tuesday[dayIndex]),
                    selectFunc(wednesday[dayIndex]),
                    selectFunc(thursday[dayIndex]),
                    selectFunc(friday[dayIndex]),
                    selectFunc(saturday[dayIndex]),
                    selectFunc(sunday[dayIndex]));
            }

            return weekDataTable;
        }

        private static IEnumerable<ActivityReport> SelectFromPerformanceReports(IEnumerable<ActivityReport> performanceReports, string dayGroup)
        {
            return performanceReports.Where(report => report.DayGroup.Equals(dayGroup, StringComparison.OrdinalIgnoreCase));
        }
    }
}
