using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Fakes;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using ecn.communicator.main.blasts.Report;
using ECN.TestHelpers;
using ecn.communicator.main.blasts.Report.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Entities.Accounts;
using Microsoft.Reporting.WebForms;
using Microsoft.Reporting.WebForms.Fakes;
using Moq;
using NUnit.Framework;
using Shouldly;
using BusinessLogic = ECN_Framework_BusinessLayer.Activity.Report.Fakes;
using MasterPages = ecn.communicator.MasterPages;

namespace ECN.Communicator.Tests.Main.Blasts.Report
{
    /// <summary>
    /// Unit Tests for <see cref="PerformanceByDayAndTimeReport.performanceByDayAndTime_ExportReport"/>
    /// </summary>
    public partial class PerformanceByDayAndTimeReportTest
    {
        private const string ExportReport = "performanceByDayAndTime_ExportReport";
        private const string ReportViewer = "ReportViewer1";
        private const string StartDate = "3/14/2018";
        private const string EndDate = "3/15/2018";
        private const string ReportPath = "main\\blasts\\Report\\rpt_PerformanceByDayAndTime.rdlc";
        private const string Extension = "extension";
        private const string Type = "type";
        private const string PDFType = "PDF";
        private const string FilterView1 = "1";
        private const string FilterView2 = "2";
        private const string HourGroup = "HourGroup";

        private bool _isDayChartGenerated;
        private bool _isWeekChartGenerated;
        private ReportViewer _reportViewer1;
        private DropDownList _ddlDay;
        private DropDownList _ddlOpensOrClicks;
        private DropDownList _ddlFilterValue1;
        private DropDownList _ddlFilterValue2;
        private TextBox _txtstartDate;
        private TextBox _txtendDate;
        private byte[] _reportData;
        private Stream _outputStream;
        private bool _dataBind;
        private int _ddlOpensOrClicksSelectedIndex;

        [Test, TestCaseSource(nameof(ShortDayOfWeek))]
        public void ExportReport_OpensMetricShow_VerifyIfReportIsExported(string day)
        {
            // Arrange
            _ddlOpensOrClicksSelectedIndex = 0;
            SetupForExportReport();

            // Act
            _page.GetType().CallMethod(ExportReport, new object[] { HttpResponse, PDFType }, _page);

            // Assert
            VerifyExportChart(day, OpensMetric);
        }

        [Test, TestCaseSource(nameof(ShortDayOfWeek))]
        public void ExportReport_ClicksMetricShow_VerifyIfReportIsExported(string day)
        {
            // Arrange
            _ddlOpensOrClicksSelectedIndex = 1;
            SetupForExportReport();

            // Act
            _page.GetType().CallMethod(ExportReport, new object[] { HttpResponse, string.Empty }, _page);

            // Assert
            VerifyExportChart(day, ClicksMetric);
        }

        private void VerifyExportChart(string day, string metric)
        {
            var dataSources = _reportViewer1.LocalReport.DataSources;
            dataSources.ShouldNotBeNull();
            dataSources.Count.ShouldBe(2);

            var dayDataSource = ((DataView)dataSources.First().Value).ToTable();
            var weekDataSource = ((DataView)dataSources.Last().Value).ToTable();

            var currentDayReport = _currentReport.Where(x => x.DayGroup == day).ToList();
            for (var rowIndex = 0; rowIndex < dayDataSource.Rows.Count; rowIndex++)
            {
                var row = dayDataSource.Rows[rowIndex];

                row.ShouldSatisfyAllConditions(
                    () => row[HourGroup].ShouldBe(currentDayReport[rowIndex].HourGroup),
                    () => row[OpensMetric].ShouldBe(currentDayReport[rowIndex].Opens),
                    () => row[ClicksMetric].ShouldBe(currentDayReport[rowIndex].Clicks));
            }

            for (var keyIndex = 0; keyIndex < DayOfWeekColors.Keys.Count; keyIndex++)
            {
                var key = DayOfWeekColors.Keys.ElementAt(keyIndex);
                var weekDayReport = _currentReport.Where(x => x.DayGroup == ShortDayOfWeek[keyIndex]).ToList();

                for (var index = 0; index < weekDataSource.Rows.Count; index++)
                {
                    var row = weekDataSource.Rows[index];
                    row.ShouldSatisfyAllConditions(
                        () => row[$"{key}"]
                            .ShouldBe(
                                metric == OpensMetric ?
                                    double.Parse(weekDayReport[index].Opens) :
                                    double.Parse(weekDayReport[index].Clicks)),

                        () => row[XValueMember].ShouldBe(_currentReport[index].HourGroup));
                }
            }

            _reportViewer1.ShouldSatisfyAllConditions(
                () => _isDayChartGenerated.ShouldBeTrue(),
                () => _isWeekChartGenerated.ShouldBeTrue(),
                () => _dataBind.ShouldBeTrue(),
                () => _reportViewer1.LocalReport.ReportPath.ShouldBe(ReportPath));
        }

        private void SetupForExportReport()
        {
            _outputStream = new Mock<Stream>().Object;
            _isDayChartGenerated = false;
            _isWeekChartGenerated = false;
            _dataBind = false;

            SetupFakesForExport();
            SetupDropDownLists();
            SetupDateTimeTextBoxes();

            _reportViewer1 = (ReportViewer)_page.GetFieldValue(ReportViewer);
        }

        private void SetupDateTimeTextBoxes()
        {
            _txtstartDate = (TextBox)_page.GetFieldValue(GetField(nameof(_txtstartDate)));
            _txtendDate = (TextBox)_page.GetFieldValue(GetField(nameof(_txtendDate)));
            _txtstartDate.Text = StartDate;
            _txtendDate.Text = EndDate;
            _page.SetField(GetField(nameof(_txtstartDate)), _txtstartDate);
            _page.SetField(GetField(nameof(_txtendDate)), _txtendDate);
        }

        private void SetupDropDownLists()
        {
            _ddlDay = (DropDownList)_page.GetFieldValue(GetField(nameof(_ddlDay)));
            foreach (string day in ShortDayOfWeek)
            {
                _ddlDay.Items.Add(new ListItem(day, day));
            }
            _page.SetField(GetField(nameof(_ddlDay)), _ddlDay);

            _reportData = new byte[1];

            _ddlOpensOrClicks = (DropDownList)_page.GetFieldValue(GetField(nameof(_ddlOpensOrClicks)));
            _ddlOpensOrClicks.Items.Add(new ListItem(OpensMetric, OpensMetric));
            _ddlOpensOrClicks.Items.Add(new ListItem(ClicksMetric, ClicksMetric));
            _ddlOpensOrClicks.SelectedIndex = _ddlOpensOrClicksSelectedIndex;
            _page.SetField(GetField(nameof(_ddlOpensOrClicks)), _ddlOpensOrClicks);

            _ddlFilterValue1 = (DropDownList)_page.GetFieldValue(GetField(nameof(_ddlFilterValue1)));
            _ddlFilterValue2 = (DropDownList)_page.GetFieldValue(GetField(nameof(_ddlFilterValue2)));
            _ddlFilterValue1.Items.Add(new ListItem(FilterView1, FilterView1));
            _ddlFilterValue2.Items.Add(new ListItem(FilterView2, FilterView2));
            _ddlFilterValue1.SelectedIndex = 0;
            _ddlFilterValue2.SelectedIndex = 0;
            _page.SetField(GetField(nameof(_ddlFilterValue1)), _ddlFilterValue1);
            _page.SetField(GetField(nameof(_ddlFilterValue2)), _ddlFilterValue2);
        }

        private void SetupFakesForExport()
        {
            SetupSessionFakes();
            SetupChartMethodFakes();
            SetupWebControlFakes();

            BusinessLogic.ShimPerformanceByDayAndTimeReport
                    .GetInt32DateTimeDateTimeStringInt32StringNullableOfInt32 = (
                        customerId,
                        startDate,
                        endDate,
                        filter1,
                        filter1Value,
                        filter2,
                        filter2Value) =>
                {
                    startDate.ShouldBe(DateTime.Parse(StartDate));
                    endDate.ShouldBe(DateTime.Parse(EndDate));
                    customerId.ShouldBe(CustomerId);

                    return _currentReport;
                };
        }

        private static void SetupSessionFakes()
        {
            ShimECNSession.CurrentSession = () =>
            {
                ECNSession session = typeof(ECNSession).CreateInstance();

                Customer customer = typeof(Customer).CreateInstance();
                customer.CustomerID = CustomerId;

                session.SetField(nameof(ECNSession.CurrentCustomer), customer);

                return session;
            };
        }

        private void SetupChartMethodFakes()
        {
            ShimPerformanceByDayAndTimeReport.AllInstances.drawDayChartString = (report, selectedValue) =>
            {
                selectedValue.ShouldBe(_ddlDay.SelectedValue.ToLower());
                _isDayChartGenerated = true;
            };

            ShimPerformanceByDayAndTimeReport.AllInstances.drawWeekChartString = (report, selectedValue) =>
            {
                selectedValue.ShouldBe(_ddlOpensOrClicks.SelectedItem.ToString().ToLower());
                _isWeekChartGenerated = true;
            };
        }

        private void SetupWebControlFakes()
        {
            ShimPerformanceByDayAndTimeReport
                .AllInstances.MasterGet = report => new MasterPages.Communicator();

            ShimCompositeControl.AllInstances.DataBind = control =>
            {
                _dataBind = true;
            };

            ShimLocalReport.AllInstances.SetParametersIEnumerableOfReportParameter = (_, parameters) =>
            {
                parameters.Count().ShouldBe(5);
            };

            ShimLocalReport
                .AllInstances
                .RenderStringStringPageCountModeStringOutStringOutStringOutStringArrayOutWarningArrayOut = (
                LocalReport format,
                string renderers,
                string info,
                PageCountMode mode,
                out string type,
                out string encoding,
                out string extension,
                out string[] streams,
                out Warning[] warnings) =>
            {
                type = Type;
                encoding = string.Empty;
                extension = Extension;
                streams = new string[0];
                warnings = new Warning[0];

                return _reportData;
            };
        }

        private HttpResponse HttpResponse => new ShimHttpResponse
        {
            ContentTypeSetString = x =>
            {
                x.ShouldBe(Type);
            },
            AppendHeaderStringString = (x, x1) =>
            {
                x.ShouldBe("Content-Disposition");
                x1.ShouldBe($"attachment; filename=PerformanceByDayAndTimeReport.{Extension}");
            },
            OutputStreamGet = () => _outputStream
        }.Instance;
    }
}
