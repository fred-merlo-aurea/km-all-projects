using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using ecn.communicator.main.blasts.Report;
using ECN.TestHelpers;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Blasts.Report
{
    /// <summary>
    /// Unit Tests for <see cref="PerformanceByDayAndTimeReport.drawDayChart"/>
    /// </summary>
    public partial class PerformanceByDayAndTimeReportTest
    {
        private const string DrawDayChart = "drawDayChart";
        private const string DayLegend = "Legends2";
        private const string DayChartArea = "ChartArea2";
        private const string DayChartTitle = "Title1";

        [Test, TestCaseSource(nameof(ShortDayOfWeek))]
        public void DrawDayChart_GivenMetricShow_VerifyCharts(string day)
        {
            // Arrange
            _page.SetField(GetField(nameof(_ddlLineOrColumn)), _ddlLineOrColumn);

            // Act
            _page.GetType().CallMethod(DrawDayChart, new object[] { day.ToLower() }, _page);

            // Assert
            VerifyDrawDayChart(day);
        }

        private void VerifyDrawDayChart(string day)
        {
            VerifyDayChartAreaAndLegend(day);

            var dataSource = _chtReportByDay.DataSource as DataTable;
            dataSource.ShouldNotBeNull();

            var currentDayReport = _currentReport.Where(x => x.DayGroup == day).ToList();
            for (var rowIndex = 0; rowIndex < dataSource.Rows.Count; rowIndex++)
            {
                var row = dataSource.Rows[rowIndex];

                row.ShouldSatisfyAllConditions(
                    () => row[XValueMember].ShouldBe(currentDayReport[rowIndex].HourGroup),
                    () => row[OpensMetric].ShouldBe(currentDayReport[rowIndex].Opens),
                    () => row[ClicksMetric].ShouldBe(currentDayReport[rowIndex].Clicks));
            }
        }

        private void VerifyDayChartAreaAndLegend(string day)
        {
            var currentDay = DayOfWeekColors.Keys.ElementAt(Array.IndexOf(ShortDayOfWeek, day));

            _chtReportByDay.ShouldSatisfyAllConditions(
                    () => _chtReportByDay.ChartAreas[DayChartArea].AxisX.Title.ShouldBe(AxisXTitle),
                    () => _chtReportByDay.ChartAreas[DayChartArea].AxisY.Title.ShouldBe("Rates (%) "),
                    () => _chtReportByDay.ChartAreas[DayChartArea].AxisX.MajorGrid.Enabled.ShouldBe(true),
                    () => _chtReportByDay.ChartAreas[DayChartArea].AxisY.MajorGrid.Enabled.ShouldBe(true),
                    () => _chtReportByDay.ChartAreas[DayChartArea].AxisX.Interval.ShouldBe(1),
                    () => _chtReportByDay.ChartAreas[DayChartArea].AxisX.MajorGrid.LineColor.ShouldBe(LightGray),
                    () => _chtReportByDay.ChartAreas[DayChartArea].AxisY.MajorGrid.LineColor.ShouldBe(LightGray),
                    () => _chtReportByDay.ChartAreas[DayChartArea].BackColor.ShouldBe(Transparent),
                    () => _chtReportByDay.ChartAreas[DayChartArea].ShadowColor.ShouldBe(Transparent),
                    () => _chtReportByDay.AntiAliasing.ShouldBe(AntiAliasingStyles.Graphics),
                    () => _chtReportByDay.Height.ShouldBe(250),
                    () => _chtReportByDay.Width.ShouldBe(545),
                    () => _chtReportByDay.Legends[DayLegend].Enabled.ShouldBe(true),
                    () => _chtReportByDay.Legends[DayLegend].Docking.ShouldBe(Docking.Bottom),
                    () => _chtReportByDay.Legends[DayLegend].Alignment.ShouldBe(StringAlignment.Center),
                    () => _chtReportByDay.Legends[DayLegend].IsEquallySpacedItems.ShouldBe(true),
                    () => _chtReportByDay.Legends[DayLegend].TextWrapThreshold.ShouldBe(0),
                    () => _chtReportByDay.Legends[DayLegend].IsTextAutoFit.ShouldBe(true),
                    () => _chtReportByDay.Legends[DayLegend].BackColor.ShouldBe(Transparent),
                    () => _chtReportByDay.Legends[DayLegend].ShadowColor.ShouldBe(Transparent),
                    () => _chtReportByDay.Titles[DayChartTitle].Text.ShouldBe($"Open and Click Rates for {currentDay}"),
                    () => _chtReportByDay.Series[OpensMetric].XValueMember.ShouldBe(XValueMember),
                    () => _chtReportByDay.Series[OpensMetric].YValueMembers.ShouldBe(OpensMetric),
                    () => _chtReportByDay.Series[OpensMetric].ChartType.ShouldBe(SeriesChartType.Column),
                    () => _chtReportByDay.Series[OpensMetric].IsVisibleInLegend.ShouldBe(true),
                    () => _chtReportByDay.Series[OpensMetric].BorderWidth.ShouldBe(3),
                    () => _chtReportByDay.Series[OpensMetric].Color.ShouldBe(Color.Blue),
                    () => _chtReportByDay.Series[ClicksMetric].XValueMember.ShouldBe(XValueMember),
                    () => _chtReportByDay.Series[ClicksMetric].YValueMembers.ShouldBe(ClicksMetric),
                    () => _chtReportByDay.Series[ClicksMetric].ChartType.ShouldBe(SeriesChartType.Column),
                    () => _chtReportByDay.Series[ClicksMetric].IsVisibleInLegend.ShouldBe(true),
                    () => _chtReportByDay.Series[ClicksMetric].BorderWidth.ShouldBe(3),
                    () => _chtReportByDay.Series[ClicksMetric].Color.ShouldBe(Color.OrangeRed));
        }
    }
}
