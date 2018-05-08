using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using KMPS.MD.MasterPages;
using KMPS.MD.MasterPages.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace KMPS.MD.Tests.MasterPage
{
    /// <summary>
    /// Unit test for <see cref="Site"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SiteTests : BasePageTests
    {
        private const string PageLoadPublic = "Page_Load";
        private const string NavigationMenu = "NavigationMenu";
        private const string AudienceViews = "AUDIENCE VIEWS";
        private const string Markets = "MARKETS";
        private const string Filters = "FILTERS";
        private const string Campaigns = "CAMPAIGNS";
        private const string Tools = "TOOLS";
        private const string ScheduleFilterExport = "Schedule Filter Export";
        private const string SalesView = "Sales View";
        private const string Consensus = "Consensus";
        private const string Recency = "Recency";
        private const string Product = "Product";
        private const string CrossProduct = "Cross Product";
        private const string RecordView = "Record View";
        private const string MarketCreation = "Market Creation";
        private const string MarketComparison = "Market Comparison";
        private const string ViewFilters = "View Filters/Filter Segmentations";
        private const string ScheduledExport = "Scheduled Export";
        private const string FilterComparison = "Filter Comparison";
        private const string FilterCategory = "Filter Category";
        private const string QuestionCategory = "Question Category";
        private const string ViewCampaign = "View Campaign";
        private const string CampaignComparison = "Campaign Comparison";
        private const string GeoCoding = "GeoCoding";
        private const string SummaryReport = "Summary Report";
        private const string RecordViewSetup = "Record View Setup";
        private const string BrandSetup = "Brand Setup";
        private const string UserBrandSetup = "User Brand Setup";
        private const string DownloadTemplateSetup = "Download Template Setup";
        private const string UserDataMaskSetup = "UserData Mask Setup";
        protected Site _site;
        protected PrivateObject _privateObject;
        protected PrivateType _privateType;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            ShimScriptManager.Constructor = (sender) => { };
            ShimMenuItemCollection.AllInstances.RemoveAtInt32 = (sender, id) => { };
            _site = new Site();
            _privateObject = new PrivateObject(_site);
            _privateType = new PrivateType(typeof(Site));
            base.InitializeAllControls(_site);
        }

        [TestCase(AudienceViews, Consensus)]
        [TestCase(AudienceViews, Recency)]
        [TestCase(AudienceViews, Product)]
        [TestCase(AudienceViews, CrossProduct)]
        [TestCase(AudienceViews, RecordView)]
        [TestCase(Markets, MarketCreation)]
        [TestCase(Markets, MarketComparison)]
        [TestCase(Filters, ViewFilters)]
        [TestCase(Filters, ScheduledExport)]
        [TestCase(Filters, FilterComparison)]
        [TestCase(Filters, FilterCategory)]
        [TestCase(Filters, QuestionCategory)]
        [TestCase(Campaigns, ViewCampaign)]
        [TestCase(Campaigns, CampaignComparison)]
        [TestCase(Tools, GeoCoding)]
        [TestCase(Tools, SummaryReport)]
        [TestCase(Tools, RecordViewSetup)]
        [TestCase(Tools, BrandSetup)]
        [TestCase(Tools, UserBrandSetup)]
        [TestCase(Tools, DownloadTemplateSetup)]
        [TestCase(Tools, UserDataMaskSetup)]
        [TestCase(Tools, GeoCoding)]
        public void PageLoad_NavigationMenuTTexthaveValue_UpdateControlValues(string navigationMenuText, string childItemText)
        {
            //Arrange
            var navigationMenu = GetField<Menu>(NavigationMenu);
            var audianceView = new MenuItem { Text = navigationMenuText };
            audianceView.ChildItems.Add(new MenuItem { Text = childItemText });
            navigationMenu.Items.Add(audianceView);
            _site.Menu = "";
            _site.SubMenu = ScheduleFilterExport;
            var menuExist = false;
            ShimMenu.AllInstances.FindItemString = (sender, item) =>
            {
                menuExist = true;
                var menu = new MenuItem { Text = SalesView };
                menu.ChildItems.Add(new MenuItem { Text = ScheduleFilterExport });
                return menu;
            };
            ShimSite.AllInstances.loadBrandLogo = (sender) => { };
            ShimSite.AllInstances.updateNavigationLinks = (sender) => { };

            var parameters = new object[] { this, EventArgs.Empty };

            //Act
            _privateObject.Invoke(PageLoadPublic, parameters);

            //Assert
            menuExist.ShouldBeTrue();
        }

        protected override T GetField<T>(string fieldName)
        {
            var field = _privateObject.GetField(fieldName) as T;

            field.ShouldNotBeNull($"The field {field} of type {typeof(T).Name} cannot be null");

            return field;
        }
    }
}
