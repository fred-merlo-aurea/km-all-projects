using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using ecn.communicator.main.blasts.Report;
using ecn.communicator.main.blasts.Report.Fakes;
using ECN.Tests.Helpers;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using ReflectionHelper = ECN.TestHelpers.ReflectionHelper;
using MasterPages = ecn.communicator.MasterPages;
using Entity = ECN_Framework_Entities.Activity.Report;

namespace ECN.Communicator.Tests.Main.Blasts.Report
{
    /// <summary>
    /// Unit tests for <see cref="PerformanceByDayAndTimeReport"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class PerformanceByDayAndTimeReportTest : BasePageTests
    {
        private const string OpensMetric = "Opens";
        private const string ClicksMetric = "Clicks";
        private const string ChartArea = "ChartArea1";
        private const string AxisXTitle = "Time";
        private const string Legend = "Legends1";
        private const string XValueMember = "Times";
        private const string ColumnChartType = "column";
        private const string LineChartType = "line";
        private static readonly Color Transparent = Color.Transparent;
        private static readonly Color LightGray = Color.LightGray;
        private static readonly string[] ShortDayOfWeek =
        {
            "Mon", "Tue", "Wed", "Thur", "Fri", "Sat", "Sun"
        };

        private static readonly string[] HourGroups =
        {
            "Midnight-6", "6-8 AM", "8-10 AM", "10-12 PM", "12-2 PM", "2-4 PM", "4-6 PM", "6-8 PM", "8-Midnight"
        };

        private static readonly IDictionary<string, Color> DayOfWeekColors =
            new Dictionary<string, Color>
            {
                ["Monday"] = Color.Blue,
                ["Tuesday"] = Color.Brown,
                ["Wednesday"] = Color.Red,
                ["Thursday"] = Color.Plum,
                ["Friday"] = Color.Yellow,
                ["Saturday"] = Color.Green,
                ["Sunday"] = Color.Black
            };

        private PerformanceByDayAndTimeReport _page;
        private Chart _chtReportByFullWeek;
        private Chart _chtReportByDay;
        private DropDownList _ddlLineOrColumn;
        private IDisposable _context;
        private List<Entity.PerformanceByDayAndTimeReport> _currentReport;
        private static CultureInfo _previousCulture;

        [SetUp]
        public void Setup()
        {
            _context = ShimsContext.Create();

            _page = ReflectionHelper.CreateInstance(typeof(PerformanceByDayAndTimeReport));

            InitializePage(_page);
            SetupPageControls();
            SetupCurrentReport();
            ReflectionHelper.SetField(_page, "_master", new MasterPages.Communicator());
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [OneTimeSetUp]
        public static void OneTimeSetup()
        {
            _previousCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(DefaultCulture);
        }

        [OneTimeTearDown]
        public static void OneTimeTearDown()
        {
            Thread.CurrentThread.CurrentCulture = _previousCulture;
        }

        private void SetupPageControls()
        {
            _chtReportByFullWeek =
                (Chart)ReflectionHelper.GetFieldValue(_page, GetField(nameof(_chtReportByFullWeek)));
            _chtReportByDay =
                (Chart)ReflectionHelper.GetFieldValue(_page, GetField(nameof(_chtReportByDay)));
            _ddlLineOrColumn = (DropDownList)ReflectionHelper.GetFieldValue(_page, GetField(nameof(_ddlLineOrColumn)));
            _ddlLineOrColumn.Items.Clear();
            _ddlLineOrColumn.Items.Add(new ListItem(ColumnChartType, ColumnChartType));
            _ddlLineOrColumn.Items.Add(new ListItem(LineChartType, LineChartType));
        }

        private void SetupCurrentReport()
        {
            _currentReport = new List<Entity.PerformanceByDayAndTimeReport>();
            foreach (string day in ShortDayOfWeek)
            {
                for (int rowIndex = 0; rowIndex < 9; rowIndex++)
                {
                    _currentReport.Add(new Entity.PerformanceByDayAndTimeReport
                    {
                        Clicks = $"{rowIndex}",
                        Opens = $"{rowIndex + 9}",
                        DayGroup = day,
                        HourGroup = HourGroups[rowIndex]
                    });
                }
            }

            ShimPerformanceByDayAndTimeReport.AllInstances.currentReportGet = report => _currentReport;
        }

        protected override void InitializePage(Page page)
        {
            base.InitializePage(page);

            _page.GetType().CallMethod("clearChartArea", new object[0], _page);
            _page.GetType().CallMethod("HideAndClear", new object[0], _page);
        }

        private string GetField(string name)
        {
            return name.TrimStart('_');
        }
    }
}