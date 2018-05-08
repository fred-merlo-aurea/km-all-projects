using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KM.Platform.Fakes;
using KMPlatform;
using KMPS.MD.Main;
using KMPS.MD.Main.Fakes;
using KMPS.MD.MasterPages;
using KMPS.MD.Objects;
using KMPS.MD.Objects.Fakes;
using NUnit.Framework;
using Shouldly;
using Telerik.Web.UI;
using FilterCategory = KMPS.MD.Objects.FilterCategory;
using ShimFilterCategory = KMPS.MD.Objects.Fakes.ShimFilterCategory;

namespace KMPS.MD.Tests.Main
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FilterListTests : BasePageTests
    {
        private const string TestZero = "0";
        private const string TestOne = "1";
        private const string AllProducts = "All Products";
        private const string MethodPageLoad = "Page_Load";
        private const string All = "All";
        private const string ViewStatePropertyName = "ViewState";
        private const string SortedFieldFSPropertyName = "SortFieldFS";
        private const string SortDirectionFSPropertyName = "SortDirectionFS";

        private FilterList _testEntity = new FilterList();
        private RadioButtonList _listType;
        private bool _resetControlsIsCalled = false;
        private bool _loadGridIsCalled = false;
        private bool _loadFilterSegmentationGridIsCalled = false;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            InitializePage(_testEntity);

            _listType = (RadioButtonList)PrivatePage.GetField("rblListType");
            _listType.Items.Add("Filters");
            _listType.Items.Add("FilterSegmentation");

            _resetControlsIsCalled = false;
            _loadGridIsCalled = false;
            _loadFilterSegmentationGridIsCalled = false;
            ShimFilterList.AllInstances.ResetControls = (_) => { _resetControlsIsCalled = true; };
            ShimFilterList.AllInstances.LoadGrid = (_) => { _loadGridIsCalled = true; };
            ShimFilterList.AllInstances.LoadFilterSegmentationGrid = (_) => { _loadFilterSegmentationGridIsCalled = true; };
            ShimFilterCategory.GetAllClientConnections = _ => new List<FilterCategory>();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (_, __, ___, ____) => true;
            GetField<HiddenField>("hfBrandID").Value = TestZero;
            GetField<Panel>("pnlBrand").Visible = false;
            GetField<DropDownList>("drpBrand").Visible = false;
            GetField<Label>("lblBrandName").Visible = false;
        }

        [Test]
        public void rblListType_SelectedIndexChanged_ListTypeIsFiltersSegmentation_CallsLoadFilterSegmentationGrid()
        {
            // Arrange
            _listType.SelectedValue = "FilterSegmentation";
            
            // Act
            PrivatePage.Invoke("rblListType_SelectedIndexChanged", this, EventArgs.Empty);

            // Assert
            _resetControlsIsCalled.ShouldBeTrue();
            _loadFilterSegmentationGridIsCalled.ShouldBeTrue();
            _loadGridIsCalled.ShouldBeFalse();
            ((Label)PrivatePage.GetField("lblSearch")).Text.ShouldBe("Filter Name or Filter Segmentation");
            ((PlaceHolder)PrivatePage.GetField("phFilters")).Visible.ShouldBeFalse();
            ((PlaceHolder)PrivatePage.GetField("phFilterSegmentations")).Visible.ShouldBeTrue();
        }

        [Test]
        public void rblListType_SelectedIndexChanged_ListTypeIsFilters_CallsLoadGrid()
        {
            // Arrange
            _listType.SelectedValue = "Filters";
            
            // Act
            PrivatePage.Invoke("rblListType_SelectedIndexChanged", this, EventArgs.Empty);

            // Assert
            _resetControlsIsCalled.ShouldBeTrue();
            _loadFilterSegmentationGridIsCalled.ShouldBeFalse();
            _loadGridIsCalled.ShouldBeTrue();
            ((Label)PrivatePage.GetField("lblSearch")).Text.ShouldBe("Filter Name or Question Name");
            ((PlaceHolder)PrivatePage.GetField("phFilters")).Visible.ShouldBeTrue();
            ((PlaceHolder)PrivatePage.GetField("phFilterSegmentations")).Visible.ShouldBeFalse();
        }

        [Test]
        public void Page_Load_ZeroBrands_HideAll()
        {
            // Arrange
            ShimBrand.GetByUserIDClientConnectionsInt32 = (_, __) => new List<Brand>();
            ShimBrand.GetAllClientConnections = _ => new List<Brand>();
            
            // Act
            PrivatePage.Invoke(MethodPageLoad, this, EventArgs.Empty);

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<Panel>("pnlBrand").Visible.ShouldBeFalse(),
                () => GetField<DropDownList>("drpBrand").Visible.ShouldBeFalse(),
                () => GetField<Label>("lblBrandName").Visible.ShouldBeFalse());
        }

        [Test]
        public void Page_Load_OneBrandAssigned_ShowBrandLabel()
        {
            // Arrange
            var brandLabel = GetField<Label>("lblBrandName");
            ShimBrand.GetByUserIDClientConnectionsInt32 = (_, __) => new List<Brand>
            {
                new Brand {BrandID = 1, BrandName = "test"}
            };
            ShimBrand.GetAllClientConnections = _ => new List<Brand>();
            
            // Act
            PrivatePage.Invoke(MethodPageLoad, this, EventArgs.Empty);

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
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
            var brandDropDown = GetField<DropDownList>("drpBrand");
            ShimBrand.GetByUserIDClientConnectionsInt32 = (_, __) => new List<Brand>();
            ShimBrand.GetAllClientConnections = _ => new List<Brand>
            {
                new Brand {BrandID = 1, BrandName = "test"}
            };
            
            // Act
            PrivatePage.Invoke(MethodPageLoad, this, EventArgs.Empty);

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
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
            var brandDropDown = GetField<DropDownList>("drpBrand");
            ShimBrand.GetByUserIDClientConnectionsInt32 = (_, __) => new List<Brand>
            {
                new Brand {BrandID = 1, BrandName = "test"},
                new Brand {BrandID = 2, BrandName = "test2"}
            };
            ShimBrand.GetAllClientConnections = _ => new List<Brand>();
            
            // Act
            PrivatePage.Invoke(MethodPageLoad, this, EventArgs.Empty);

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<Panel>("pnlBrand").Visible.ShouldBeTrue(),
                () => brandDropDown.Visible.ShouldBeTrue(),
                () => brandDropDown.Items.Count.ShouldBe(3),
                () => brandDropDown.Items[0].Text.ShouldBe(All),
                () => GetField<Label>("lblBrandName").Visible.ShouldBeFalse());
        }

        [Test]
        public void Page_Load_TwoBrandsNotAssigned_ShowBrandDropDown()
        {
            // Arrange
            var brandDropDown = GetField<DropDownList>("drpBrand");
            ShimBrand.GetByUserIDClientConnectionsInt32 = (_, __) => new List<Brand>();
            ShimBrand.GetAllClientConnections = _ => new List<Brand>
            {
                new Brand {BrandID = 1, BrandName = "test"},
                new Brand {BrandID = 2, BrandName = "test2"}
            };
            
            // Act
            PrivatePage.Invoke(MethodPageLoad, this, EventArgs.Empty);

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<Panel>("pnlBrand").Visible.ShouldBeTrue(),
                () => brandDropDown.Visible.ShouldBeTrue(),
                () => brandDropDown.Items.Count.ShouldBe(3),
                () => brandDropDown.Items[0].Text.ShouldBe(AllProducts),
                () => GetField<Label>("lblBrandName").Visible.ShouldBeFalse());
        }
     
        [Test]
        public void SortFieldFS_gets_Viewstate_SortFieldFS()
        {            
            // Arrange 
            var stateBag = (StateBag)PrivatePage.GetProperty(ViewStatePropertyName);
            const string expectedValue = "Field1";

            // Act 
            stateBag[SortedFieldFSPropertyName] = expectedValue;

            // Assert
            PrivatePage.GetProperty(SortedFieldFSPropertyName).ShouldBe(expectedValue);                      
        }

        [Test]
        public void SortFieldFS_sets_Viewstate_SortFieldFS()
        {
            // Arrange 
            const string expectedValue = "Field2";
           PrivatePage.SetProperty(SortedFieldFSPropertyName, expectedValue);

            // Act 
            var stateBag = (StateBag)PrivatePage.GetProperty(ViewStatePropertyName);

            // Assert
            stateBag[SortedFieldFSPropertyName].ShouldBe(expectedValue);
        }

        [Test]
        public void SortDirectionFS_gets_Viewstate_SortFieldFS()
        {
            // Arrange 
            var stateBag = (StateBag)PrivatePage.GetProperty(ViewStatePropertyName);
            const string expectedValue = "DESC";

            // Act 
            stateBag[SortDirectionFSPropertyName] = expectedValue;

            // Assert
            PrivatePage.GetProperty(SortDirectionFSPropertyName).ShouldBe(expectedValue);
        }

        [Test]
        public void SortDirectionFS_sets_Viewstate_SortFieldFS()
        {
            // Arrange 
            const string expectedValue = "ASC";
            PrivatePage.SetProperty(SortDirectionFSPropertyName, expectedValue);

            // Act 
            var stateBag = (StateBag)PrivatePage.GetProperty(ViewStatePropertyName);

            // Assert
            stateBag[SortDirectionFSPropertyName].ShouldBe(expectedValue);
        }

    }
}
