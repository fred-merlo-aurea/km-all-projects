using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using ecn.communicator.blasts.reports;
using ecn.communicator.blasts.reports.Fakes;
using ECN.Tests.Helpers;
using ECN_Framework_DataLayer.Activity.Report.Fakes;
using ECN_Framework_Entities.Activity.Report;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using ReflectionHelper = ECN.TestHelpers.ReflectionHelper;

namespace ECN.Communicator.Tests.Main.Blasts.Report
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class BlastComparisonReportTest : BasePageTests
    {
        private BlastComparisonReport _page;
        private static CultureInfo _previousCulture;
        private Chart _chtBlastComparision;
        private IDisposable _shims;
        private const string BlastListBox = "BlastListBox";
        private const string Value = "value";
        private const string Opens = "Opens";
        private const string Clicks = "Clicks";
        private const string Bounces = "Bounces";
        private const string OptOuts = "Opt-outs";
        private const string Complaints = "Complaints";
        private const string BlastId = "Asc";
        private const string Solid = "Solid";
        private const string BorderColor = "ff1a3b69";
        private const int Height = 500;
        private const int Width = 900;
        private const int BorderlineWidth = 1;

        [SetUp]
        public void Setup()
        {
            _shims = ShimsContext.Create();
            _page = ReflectionHelper.CreateInstance(typeof(BlastComparisonReport));
            InitializePage(_page);
        }

        [TearDown]
        public void TearDown()
        {
            _shims?.Dispose();
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

        [Test]
        public void CreateChart_ForQuery_AddDataPointCollection()
        {
            // Arrange
            InitializeCreateChart();

            // Act
            _page.CreateChart();

            // Assert
            var chartResult = (Chart)ReflectionHelper.GetFieldValue(_page, GetField(nameof(_chtBlastComparision)));
            chartResult.ShouldSatisfyAllConditions(
                () => chartResult.ShouldNotBeNull(),
                () => chartResult.Legends.ShouldNotBeNull(),
                () => chartResult.BorderlineWidth.ShouldBe(BorderlineWidth),
                () => chartResult.Height.ShouldBe(Height),
                () => chartResult.Width.ShouldBe(Width));
        }

        private void InitializeCreateChart()
        {
            var listItemCollection = new ListItemCollection
            {
                CreateListItem(Opens),
                CreateListItem(Clicks),
                CreateListItem(Bounces),
                CreateListItem(OptOuts),
                CreateListItem(Complaints)
            };
            ShimListControl.AllInstances.ItemsGet = (x) => listItemCollection;
            ShimBlastComparision.GetString = (x) => 
            new List<BlastComparision>
            {
                new BlastComparision()
                {
                    ActionTypeCode = Value,
                    BlastID = BlastId,
                    Perc = 10f
                }
            };
            ShimBlastComparisonReport.AllInstances.createGridDataTable = (x) => new DataTable();
        }

        private ListItem CreateListItem(string param)
        {
            return new ListItem() { Selected = true, Value = Value, Text = param };
        }

        private string GetField(string name)
        {
            return name.TrimStart('_');
        }
    }
}
