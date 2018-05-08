using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
using ecn.communicator.main.blasts.Report;
using ECN.TestHelpers;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Blasts.Report
{
    /// <summary>
    /// Unit Tests for <see cref="PerformanceByDayAndTimeReport.drawWeekChart"/>
    /// </summary>
    public partial class PerformanceByDayAndTimeReportTest
    {
        private const string DrawWeekChart = "drawWeekChart";

        [Test]
        [TestCase(true, ClicksMetric)]
        [TestCase(false, OpensMetric)]
        public void DrawWeekChart_GivenMetricShow_VerifyCharts(bool isLineChartType, string metric)
        {
            // Arrange
            _ddlLineOrColumn.SelectedValue = isLineChartType ? LineChartType : ColumnChartType;
            _page.SetField(GetField(nameof(_ddlLineOrColumn)), _ddlLineOrColumn);

            // Act
            _page.GetType().CallMethod(DrawWeekChart, new object[] { metric.ToLower() }, _page);

            // Assert
            VerifyWeekChart(metric, isLineChartType ? SeriesChartType.Line : SeriesChartType.Column);
        }

        private void VerifyWeekChart(string metric, SeriesChartType chartType)
        {
            VerifyWeekChartAreaAndLegend();

            var dataSource = _chtReportByFullWeek.DataSource as DataTable;
            dataSource.ShouldNotBeNull();

            foreach (KeyValuePair<string, Color> dayOfWeekColor in DayOfWeekColors)
            {
                var day = dayOfWeekColor.Key;
                var color = dayOfWeekColor.Value;

                for (var index = 0; index < dataSource.Rows.Count; index++)
                {
                    var row = dataSource.Rows[index];
                    row.ShouldSatisfyAllConditions(
                        () => row[$"{day}{metric}"]
                            .ShouldBe(
                                metric == OpensMetric ?
                                         _currentReport[index].Opens :
                                         _currentReport[index].Clicks),

                        () => row[XValueMember].ShouldBe(_currentReport[index].HourGroup)
                    );
                }

                _chtReportByFullWeek.ShouldSatisfyAllConditions(
                    () => _chtReportByFullWeek.Series[day].XValueMember.ShouldBe(XValueMember),
                    () => _chtReportByFullWeek.Series[day].YValueMembers.ShouldBe($"{day}{metric}"),
                    () => _chtReportByFullWeek.Series[day].ChartType.ShouldBe(chartType),
                    () => _chtReportByFullWeek.Series[day].IsVisibleInLegend.ShouldBe(true),
                    () => _chtReportByFullWeek.Series[day].BorderWidth.ShouldBe(3),
                    () => _chtReportByFullWeek.Series[day].Color.ShouldBe(color)
                );
            }
        }

        private void VerifyWeekChartAreaAndLegend()
        {
            _chtReportByFullWeek.ShouldSatisfyAllConditions(
                () => _chtReportByFullWeek.ChartAreas.Count.ShouldBe(1),
                () => _chtReportByFullWeek.ChartAreas[ChartArea].AxisX.Title.ShouldBe(AxisXTitle),
                () => _chtReportByFullWeek.ChartAreas[ChartArea].AxisX.MajorGrid.Enabled.ShouldBeTrue(),
                () => _chtReportByFullWeek.ChartAreas[ChartArea].AxisY.MajorGrid.Enabled.ShouldBeTrue(),
                () => _chtReportByFullWeek.ChartAreas[ChartArea].AxisX.Interval.ShouldBe(1),
                () => _chtReportByFullWeek.ChartAreas[ChartArea].AxisX.MajorGrid.LineColor.ShouldBe(LightGray),
                () => _chtReportByFullWeek.ChartAreas[ChartArea].AxisY.MajorGrid.LineColor.ShouldBe(LightGray),
                () => _chtReportByFullWeek.ChartAreas[ChartArea].BackColor.ShouldBe(Transparent),
                () => _chtReportByFullWeek.ChartAreas[ChartArea].ShadowColor.ShouldBe(Transparent),
                () => _chtReportByFullWeek.Height.ShouldBe(450),
                () => _chtReportByFullWeek.Width.ShouldBe(800),
                () => _chtReportByFullWeek.Legends[Legend].Enabled.ShouldBe(true),
                () => _chtReportByFullWeek.Legends[Legend].Docking.ShouldBe(Docking.Bottom),
                () => _chtReportByFullWeek.Legends[Legend].Alignment.ShouldBe(StringAlignment.Center),
                () => _chtReportByFullWeek.Legends[Legend].IsEquallySpacedItems.ShouldBe(true),
                () => _chtReportByFullWeek.Legends[Legend].TextWrapThreshold.ShouldBe(0),
                () => _chtReportByFullWeek.Legends[Legend].IsTextAutoFit.ShouldBe(true),
                () => _chtReportByFullWeek.Legends[Legend].BackColor.ShouldBe(Transparent),
                () => _chtReportByFullWeek.Legends[Legend].ShadowColor.ShouldBe(Transparent)
            );
        }
    }
}
