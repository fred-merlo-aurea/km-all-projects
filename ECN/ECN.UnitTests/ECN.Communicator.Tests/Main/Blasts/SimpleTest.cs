using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using ecn.communicator.blastsmanager;
using ECN.Tests.Helpers;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using Image = System.Web.UI.WebControls.Image;
using SocialActivity = ecn.communicator.blastsmanager.Simple.SocialActivity;

namespace ECN.Communicator.Tests.Main.Blasts
{
    /// <summary>
    /// Unit tests for <see cref="Simple"/>
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public class SimpleTest : BasePageTests
    {
        private const string SocialChart = "SocialChart";
        private const string ImageNoResults = "imgNoResults";
        private const int Height = 400;
        private const int Width = 600;
        private const string ChartAreaTitle = "ca1";
        private const int AxisXInterval = 1;
        private const string ChartUniqueSeries = "Unique";
        private const string ChartTotalSeries = "Total";
        private const string Impressions = "Impressions";
        private const string Clicks = "Clicks";
        private const string Likes = "Likes";
        private const string Shares = "Shares";
        private const string Comments = "Comments";
        private const string GvChartsRowDataBound = "gvCharts_RowDataBound";

        private static readonly Color Gray = Color.Gray;
        private static readonly Color KmBlue = ColorTranslator.FromHtml("#045DA4");
        private static readonly Color KmOrange = ColorTranslator.FromHtml("#F47E1F");

        private IDisposable _context;
        private Simple _page;
        private int _socialMediaId;
        private SocialActivity _activity;
        private Chart _chart;
        private Image _image;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _page = new Simple();
            _chart = new Chart();

            InitializePage(_page);
            SetupFakes();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void gvChartsRowDataBound_WhenCalledWithSocialIdGreaterThanZero_VerifyChart()
        {
            // Arrange
            _socialMediaId = 10;
            _activity = typeof(SocialActivity).CreateInstance();
            _activity.SocialMediaID = _socialMediaId;
            _image = new ShimImage().Instance;

            // Act
            _privateObject.Invoke(GvChartsRowDataBound, null, new GridViewRowEventArgs(new ShimGridViewRow().Instance));

            // Assert
            VerifyChart();
        }

        [Test]
        public void gvChartsRowDataBound_WhenCalledWithSocialIdLessThanZero_HideChart()
        {
            // Arrange
            _socialMediaId = 0;
            _activity = typeof(SocialActivity).CreateInstance();
            _activity.SocialMediaID = _socialMediaId;
            _image = new ShimImage().Instance;

            // Act
            _privateObject.Invoke(GvChartsRowDataBound, null, new GridViewRowEventArgs(new ShimGridViewRow().Instance));

            // Assert
            _chart.Visible.ShouldBeFalse();
        }

        private void VerifyChart()
        {
            _chart.ShouldSatisfyAllConditions(
            () => _chart.Visible.ShouldBeTrue(),
            () => _image.Visible.ShouldBeFalse(),
            () => _chart.Height.ShouldBe(Height),
            () => _chart.Width.ShouldBe(Width),
            () => _chart.Titles[0].Text.ShouldBe(_activity.Title),
            () => _chart.Titles[0].Alignment.ShouldBe(ContentAlignment.TopLeft));

            VerifyChartArea();
            VerifyChartSeries();
            VerifyUniqueDataPoints();
            VerifyTotalDataPoints();
            VerifyLegend();
        }

        private void VerifyChartArea()
        {
            _chart.ShouldSatisfyAllConditions(
                () => _chart.ChartAreas[ChartAreaTitle].AxisX.Interval.ShouldBe(AxisXInterval),
                () => _chart.ChartAreas[ChartAreaTitle].AxisX.IsStartedFromZero.ShouldBeTrue(),
                () => _chart.ChartAreas[ChartAreaTitle].AxisX.LineColor.ShouldBe(Gray),
                () => _chart.ChartAreas[ChartAreaTitle].AxisY.LineColor.ShouldBe(Gray));
        }

        private void VerifyChartSeries()
        {
            _chart.ShouldSatisfyAllConditions(
                () => _chart.Series[ChartUniqueSeries].LegendText.ShouldBe(ChartUniqueSeries),
                () => _chart.Series[ChartUniqueSeries].ChartType.ShouldBe(SeriesChartType.Bar),
                () => _chart.Series[ChartUniqueSeries].Color.ShouldBe(KmOrange),
                () => _chart.Series[ChartTotalSeries].LegendText.ShouldBe(ChartTotalSeries),
                () => _chart.Series[ChartTotalSeries].ChartType.ShouldBe(SeriesChartType.Bar),
                () => _chart.Series[ChartTotalSeries].Color.ShouldBe(KmBlue));
        }

        private void VerifyUniqueDataPoints()
        {
            _chart.ShouldSatisfyAllConditions(

                () => _chart.Series[ChartUniqueSeries].Points[0].AxisLabel.ShouldBe(Impressions),
                () => _chart.Series[ChartUniqueSeries].Points[0].YValues[0].ShouldBe(_activity.UniqueFBImpressions),
                () => _chart.Series[ChartUniqueSeries].Points[0].IsValueShownAsLabel.ShouldBeTrue(),
                () => _chart.Series[ChartUniqueSeries].Points[0].ToolTip.ShouldBe(_activity.UniqueFBImpressions.ToString()),
                () => _chart.Series[ChartUniqueSeries].Points[1].AxisLabel.ShouldBe(Clicks),
                () => _chart.Series[ChartUniqueSeries].Points[1].YValues[0].ShouldBe(_activity.UniqueClicks),
                () => _chart.Series[ChartUniqueSeries].Points[1].IsValueShownAsLabel.ShouldBeTrue(),
                () => _chart.Series[ChartUniqueSeries].Points[1].ToolTip.ShouldBe(_activity.UniqueClicks.ToString()),
                () => _chart.Series[ChartUniqueSeries].Points[2].AxisLabel.ShouldBe(Likes),
                () => _chart.Series[ChartUniqueSeries].Points[2].YValues[0].ShouldBe(_activity.TotalFBLikes),
                () => _chart.Series[ChartUniqueSeries].Points[2].IsValueShownAsLabel.ShouldBeTrue(),
                () => _chart.Series[ChartUniqueSeries].Points[2].ToolTip.ShouldBe(_activity.TotalFBLikes.ToString()),
                () => _chart.Series[ChartUniqueSeries].Points[3].AxisLabel.ShouldBe(Shares),
                () => _chart.Series[ChartUniqueSeries].Points[4].AxisLabel.ShouldBe(Comments),
                () => _chart.Series[ChartUniqueSeries].Points[4].YValues[0].ShouldBe(_activity.UniqueComments),
                () => _chart.Series[ChartUniqueSeries].Points[4].IsValueShownAsLabel.ShouldBeTrue(),
                () => _chart.Series[ChartUniqueSeries].Points[4].ToolTip.ShouldBe(_activity.UniqueComments.ToString()));
        }

        private void VerifyTotalDataPoints()
        {
            _chart.ShouldSatisfyAllConditions(
                () => _chart.Series[ChartTotalSeries].Points[0].AxisLabel.ShouldBe(Impressions),
                () => _chart.Series[ChartTotalSeries].Points[0].YValues[0].ShouldBe(_activity.TotalFBImpressions),
                () => _chart.Series[ChartTotalSeries].Points[0].IsValueShownAsLabel.ShouldBeTrue(),
                () => _chart.Series[ChartTotalSeries].Points[0].ToolTip.ShouldBe(_activity.TotalFBImpressions.ToString()),
                () => _chart.Series[ChartTotalSeries].Points[1].AxisLabel.ShouldBe(Clicks),
                () => _chart.Series[ChartTotalSeries].Points[1].YValues[0].ShouldBe(_activity.TotalClicks),
                () => _chart.Series[ChartTotalSeries].Points[1].IsValueShownAsLabel.ShouldBeTrue(),
                () => _chart.Series[ChartTotalSeries].Points[1].ToolTip.ShouldBe(_activity.TotalClicks.ToString()),
                () => _chart.Series[ChartTotalSeries].Points[2].AxisLabel.ShouldBe(Likes),
                () => _chart.Series[ChartTotalSeries].Points[2].YValues[0].ShouldBe(_activity.TotalFBLikes),
                () => _chart.Series[ChartTotalSeries].Points[2].IsValueShownAsLabel.ShouldBeTrue(),
                () => _chart.Series[ChartTotalSeries].Points[2].ToolTip.ShouldBe(_activity.TotalFBLikes.ToString()),
                () => _chart.Series[ChartTotalSeries].Points[3].AxisLabel.ShouldBe(Shares),
                () => _chart.Series[ChartTotalSeries].Points[3].YValues[0].ShouldBe(_activity.Shares),
                () => _chart.Series[ChartTotalSeries].Points[3].IsValueShownAsLabel.ShouldBeTrue(),
                () => _chart.Series[ChartTotalSeries].Points[3].ToolTip.ShouldBe(_activity.Shares.ToString()),
                () => _chart.Series[ChartTotalSeries].Points[4].AxisLabel.ShouldBe(Comments),
                () => _chart.Series[ChartTotalSeries].Points[4].YValues[0].ShouldBe(_activity.TotalComments),
                () => _chart.Series[ChartTotalSeries].Points[4].IsValueShownAsLabel.ShouldBeTrue(),
                () => _chart.Series[ChartTotalSeries].Points[4].ToolTip.ShouldBe(_activity.TotalComments.ToString()));
        }

        private void VerifyLegend()
        {
            _chart.ShouldSatisfyAllConditions(
                () => _chart.Legends[0].Enabled.ShouldBeTrue(),
                () => _chart.Legends[0].Docking.ShouldBe(Docking.Right),
                () => _chart.Legends[1].Enabled.ShouldBeTrue(),
                () => _chart.Legends[1].Docking.ShouldBe(Docking.Right));
        }

        private void SetupFakes()
        {
            ShimGridViewRow.AllInstances.DataItemGet = _ => _activity;
            ShimGridViewRow.AllInstances.RowTypeGet = _ => DataControlRowType.DataRow;
            ShimBaseDataBoundControl.AllInstances.DataBind = _ => { };
            ShimControl.AllInstances.FindControlString = (_, name) =>
            {
                switch (name)
                {
                    case SocialChart:
                        return _chart;
                    case ImageNoResults:
                        return _image;
                }

                return null;
            };
        }
    }
}
