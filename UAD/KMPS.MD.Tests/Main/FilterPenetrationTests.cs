using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KM.Platform.Fakes;
using KMPS.MD.Main;
using KMPS.MD.Main.Fakes;
using KMPS.MD.Objects;
using KMPS.MD.Objects.Fakes;
using NUnit.Framework;
using Shouldly;

namespace KMPS.MD.Tests.Main
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FilterPenetrationTests : BasePageTests
    {
        private const string TestZero = "0";
        private const string TestOne = "1";
        private const string AllProducts = "All Products";
        private const string MethodPageLoad = "Page_Load";
        private FilterPenetration _testEntity = new FilterPenetration();
        
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            InitializePage(_testEntity);

            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (_, __, ___, ____) => true;
            GetField<HiddenField>("hfBrandID").Value = TestZero;
            GetField<Panel>("pnlBrand").Visible = false;
            GetField<DropDownList>("drpBrand").Visible = false;
            GetField<Label>("lblBrandName").Visible = false;
        }

        [Test]
        public void Page_Load_ZeroBrands_HideAll()
        {
            // Arrange
            var isLoadFilterCalled = false;
            var isLoadSavedReportsCalled = false;
            ShimBrand.GetByUserIDClientConnectionsInt32 = (_, __) => new List<Brand>();
            ShimBrand.GetAllClientConnections = _ => new List<Brand>();
            ShimFilterPenetration.AllInstances.LoadFilters = _ => { isLoadFilterCalled = true; };
            ShimFilterPenetration.AllInstances.loadsavedreports = _ => { isLoadSavedReportsCalled = true; };
            
            // Act
            PrivatePage.Invoke(MethodPageLoad, this, EventArgs.Empty);

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => isLoadFilterCalled.ShouldBeTrue(),
                () => isLoadSavedReportsCalled.ShouldBeTrue(),
                () => GetField<Panel>("pnlBrand").Visible.ShouldBeFalse(),
                () => GetField<DropDownList>("drpBrand").Visible.ShouldBeFalse(),
                () => GetField<Label>("lblBrandName").Visible.ShouldBeFalse());
        }

        [Test]
        public void Page_Load_OneBrandAssigned_ShowBrandLabel()
        {
            // Arrange
            var isLoadFilterCalled = false;
            var isLoadSavedReportsCalled = false;
            var brandLabel = GetField<Label>("lblBrandName");
            ShimBrand.GetByUserIDClientConnectionsInt32 = (_, __) => new List<Brand>
            {
                new Brand {BrandID = 1, BrandName = "test"}
            };
            ShimBrand.GetAllClientConnections = _ => new List<Brand>();
            ShimFilterPenetration.AllInstances.LoadFilters = _ => { isLoadFilterCalled = true; };
            ShimFilterPenetration.AllInstances.loadsavedreports = _ => { isLoadSavedReportsCalled = true; };
            
            // Act
            PrivatePage.Invoke(MethodPageLoad, this, EventArgs.Empty);

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => isLoadFilterCalled.ShouldBeTrue(),
                () => isLoadSavedReportsCalled.ShouldBeTrue(),
                () => GetField<Panel>("pnlBrand").Visible.ShouldBeTrue(),
                () => GetField<DropDownList>("drpBrand").Visible.ShouldBeFalse(),
                () => brandLabel.Visible.ShouldBeTrue(),
                () => brandLabel.Text.ShouldBe("test"),
                () => GetField<HiddenField>("hfBrandID").Value.ShouldBe(TestOne));
        }

        [Test]
        public void Page_Load_OneBrandNotAssigned_ShowBrandDropDown()
        {
            // Arrange
            var isLoadFilterCalled = false;
            var isLoadSavedReportsCalled = false;
            var brandDropDown = GetField<DropDownList>("drpBrand");
            ShimBrand.GetByUserIDClientConnectionsInt32 = (_, __) => new List<Brand>();
            ShimBrand.GetAllClientConnections = _ => new List<Brand>
            {
                new Brand {BrandID = 1, BrandName = "test"}
            };
            ShimFilterPenetration.AllInstances.LoadFilters = _ => { isLoadFilterCalled = true; };
            ShimFilterPenetration.AllInstances.loadsavedreports = _ => { isLoadSavedReportsCalled = true; };
            
            // Act
            PrivatePage.Invoke(MethodPageLoad, this, EventArgs.Empty);

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => isLoadFilterCalled.ShouldBeTrue(),
                () => isLoadSavedReportsCalled.ShouldBeTrue(),
                () => GetField<Panel>("pnlBrand").Visible.ShouldBeTrue(),
                () => brandDropDown.Visible.ShouldBeTrue(),
                () => brandDropDown.Items.Count.ShouldBe(2),
                () => brandDropDown.Items[0].Text.ShouldBe(AllProducts),
                () => GetField<Label>("lblBrandName").Visible.ShouldBeFalse());
        }

        [Test]
        public void Page_Load_TwoBrandsAssigned_ShowBrandDropDown()
        {
            // Arrange
            var isLoadFilterCalled = false;
            var isLoadSavedReportsCalled = false;
            var brandDropDown = GetField<DropDownList>("drpBrand");
            ShimBrand.GetByUserIDClientConnectionsInt32 = (_, __) => new List<Brand>
            {
                new Brand {BrandID = 1, BrandName = "test"},
                new Brand {BrandID = 2, BrandName = "test2"}
            };
            ShimBrand.GetAllClientConnections = _ => new List<Brand>();
            ShimFilterPenetration.AllInstances.LoadFilters = _ => { isLoadFilterCalled = true; };
            ShimFilterPenetration.AllInstances.loadsavedreports = _ => { isLoadSavedReportsCalled = true; };
            
            // Act
            PrivatePage.Invoke(MethodPageLoad, this, EventArgs.Empty);

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => isLoadFilterCalled.ShouldBeFalse(),
                () => isLoadSavedReportsCalled.ShouldBeFalse(),
                () => GetField<Panel>("pnlBrand").Visible.ShouldBeTrue(),
                () => brandDropDown.Visible.ShouldBeTrue(),
                () => brandDropDown.Items.Count.ShouldBe(3),
                () => brandDropDown.Items[0].Text.ShouldBe(string.Empty),
                () => GetField<Label>("lblBrandName").Visible.ShouldBeFalse());
        }

        [Test]
        public void Page_Load_TwoBrandsNotAssigned_ShowBrandDropDown()
        {
            // Arrange
            var isLoadFilterCalled = false;
            var isLoadSavedReportsCalled = false;
            var brandDropDown = GetField<DropDownList>("drpBrand");
            ShimBrand.GetByUserIDClientConnectionsInt32 = (_, __) => new List<Brand>();
            ShimBrand.GetAllClientConnections = _ => new List<Brand>
            {
                new Brand {BrandID = 1, BrandName = "test"},
                new Brand {BrandID = 2, BrandName = "test2"}
            };
            ShimFilterPenetration.AllInstances.LoadFilters = _ => { isLoadFilterCalled = true; };
            ShimFilterPenetration.AllInstances.loadsavedreports = _ => { isLoadSavedReportsCalled = true; };
            
            // Act
            PrivatePage.Invoke(MethodPageLoad, this, EventArgs.Empty);

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => isLoadFilterCalled.ShouldBeTrue(),
                () => isLoadSavedReportsCalled.ShouldBeTrue(),
                () => GetField<Panel>("pnlBrand").Visible.ShouldBeTrue(),
                () => brandDropDown.Visible.ShouldBeTrue(),
                () => brandDropDown.Items.Count.ShouldBe(3),
                () => brandDropDown.Items[0].Text.ShouldBe(AllProducts),
                () => GetField<Label>("lblBrandName").Visible.ShouldBeFalse());
        }
    }
}
