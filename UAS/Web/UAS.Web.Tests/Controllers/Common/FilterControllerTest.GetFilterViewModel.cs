using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using KM.Platform.Fakes;
using KMPlatform;
using NUnit.Framework;
using Shouldly;
using UAS.Web.Controllers.Common;
using UAS.Web.Controllers.Common.Fakes;
using UAS.Web.Models.UAD.Filter;

namespace UAS.Web.Tests.Controllers.Common
{
    public partial class FilterControllerTest
    {
        private const string FilterPartialView = "Partials/_Filter";
        private const string RecencyViewType = "RecencyView";
        private const string CrossProductViewType = "CrossProductView";

        private static readonly string[] ViewTypes = {
            FilterController.ConsensusViewType,
            FilterController.CrossProductViewType,
            FilterController.ProductViewType,
            FilterController.RecencyViewType
        };

        [Test]
        public void GetFilterViewModel_WithoutServicePermission_RedirectsToError()
        {
            // Arrange, Act
            var result = _testEntity.GetFilterViewModel(SampleId) as RedirectToRouteResult;

            // Assert
            VerifyRedirectToError(result);
        }

        [Test]
        [TestCaseSource(nameof(ViewTypes))]
        public void GetFilterViewModel_ViewTypeWithoutAccessPermission_RedirectsToError(string viewType)
        {
            // Arrange
            UserServices.Add(Enums.Services.FULFILLMENT);

            // Act
            var result = _testEntity.GetFilterViewModel(SampleId, false, SampleId, viewType) as RedirectToRouteResult;

            // Assert
            VerifyRedirectToError(result);
        }

        [Test]
        [TestCaseSource(nameof(ViewTypes))]
        public void GetFilterViewModel_ViewTypeWithAccessPermission_ReturnsPartialView(string viewType)
        {
            // Arrange
            var isProductView = viewType.Contains(FilterController.ProductViewType);
            Enum.TryParse(viewType, true, out Enums.ServiceFeatures feature);
            UserServices.Add(Enums.Services.UAD);
            UserFeatures.Add(feature);
            UserAccesses.Add(Enums.Access.View);
            ShimForCommonFilters();

            // Act
            var result = _testEntity.GetFilterViewModel(SampleId, false, SampleId, viewType) as PartialViewResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ViewName.ShouldBe(FilterPartialView));

            var model = result.Model as FilterViewModel;
            model.ShouldSatisfyAllConditions(
                () => model.ShouldNotBeNull(),
                () => model.IsCirc.ShouldBeFalse(),
                () => model.ViewType.ShouldBe(viewType),
                () => model.showDimensionPanel.ShouldBeTrue(),
                () => model.showPubTypePanel.ShouldBe(!isProductView),
                () => model.showDCPanel.ShouldBeTrue(),
                () => model.showBrandPanel.ShouldBeTrue(),
                () => model.showProductPanel.ShouldBe(isProductView),
                () => model.showSavedLink.ShouldBeTrue(),
                () => model.showMarketPanel.ShouldBeTrue(),
                () => model.showDemoPanel.ShouldBeFalse(),
                () => model.IsSavedlinkEnabled.ShouldBeFalse(),
                () => model.HasEditAccessToSaved.ShouldBeFalse(),
                () => model.DynamicData.ShouldNotBeNull(),
                () => model.DynamicData.DemosFilterList.ShouldNotBeNull(),
                () => model.CircFilter.ShouldNotBeNull(),
                () => model.geoFilter.ShouldNotBeNull(),
                () => model.standardFilter.ShouldNotBeNull(),
                () => model.filterCategoryTree.ShouldNotBeNull(),
                () => model.filters.ShouldNotBeNull());
        }

        [Test]
        public void GetFilterViewModel_IsCirc_ReturnsPartialView()
        {
            // Arrange
            UserServices.Add(Enums.Services.FULFILLMENT);
            ShimForCommonFilters();

            // Act
            var result = _testEntity.GetFilterViewModel(SampleId, IsCirc: true) as PartialViewResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ViewName.ShouldBe(FilterPartialView));

            var model = result.Model as FilterViewModel;
            model.ShouldSatisfyAllConditions(
                () => model.ShouldNotBeNull(),
                () => model.IsCirc.ShouldBeTrue(),
                () => model.showDimensionPanel.ShouldBeFalse(),
                () => model.showPubTypePanel.ShouldBeFalse(),
                () => model.showDCPanel.ShouldBeFalse(),
                () => model.showBrandPanel.ShouldBeFalse(),
                () => model.showProductPanel.ShouldBeFalse(),
                () => model.showSavedLink.ShouldBeFalse(),
                () => model.showMarketPanel.ShouldBeFalse(),
                () => model.showDemoPanel.ShouldBeTrue(),
                () => model.DynamicData.ShouldNotBeNull(),
                () => model.DynamicData.DemosFilterList.ShouldNotBeNull(),
                () => model.CircFilter.ShouldNotBeNull(),
                () => model.geoFilter.ShouldNotBeNull(),
                () => model.standardFilter.ShouldNotBeNull(),
                () => model.filterCategoryTree.ShouldNotBeNull(),
                () => model.filters.ShouldNotBeNull());
        }

        [Test]
        [TestCase(false, RecencyViewType)]
        [TestCase(false, CrossProductViewType)]
        [TestCase(true, RecencyViewType)]
        [TestCase(true, CrossProductViewType)]
        public void GetFilterViewModel_UserHasMultiBrands_ReturnsPartialViewAndShowBrandPanel(bool isAdmin, string viewType)
        {
            // Arrange
            var list = new List<SelectListItem>
            {
                new SelectListItem { Text = "brand1", Value = "1" }
            };

            if (isAdmin)
            {
                list.Add(new SelectListItem { Text = "brand2", Value = "2" });
            }

            var dynamicFilter = new DynamicFilter();
            var isProductView = viewType.Contains(FilterController.ProductViewType);
            Enum.TryParse(viewType, true, out Enums.ServiceFeatures feature);
            UserServices.Add(Enums.Services.UAD);
            UserFeatures.Add(feature);
            UserAccesses.Add(Enums.Access.View);
            ShimUser.IsAdministratorUser = user => isAdmin;
            ShimFilterController.AllInstances.GetAllActiveBrandsByUser = _ => new List<SelectListItem>();
            ShimFilterController.AllInstances.GetAllActiveBrands = _ => list;
            ShimForCommonFilters();
            ShimFilterController.AllInstances.GetConsAndRecDynamicDataInt32 = (_, __) => dynamicFilter;
            ShimFilterController.AllInstances.GetAllSearchEnabledProductsInt32 = (_, __) => list;

            // Act
            var result = _testEntity.GetFilterViewModel(SampleId, false, vwType: viewType) as PartialViewResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ViewName.ShouldBe(FilterPartialView));

            var model = result.Model as FilterViewModel;

            if (isProductView)
            {
                model.ProductFilterList.ShouldBeSameAs(list);
            }
            else
            {
                model.DynamicData.ShouldBeSameAs(dynamicFilter);
            }

            model.ShouldSatisfyAllConditions(
                () => model.ShouldNotBeNull(),
                () => model.IsCirc.ShouldBeFalse(),
                () => model.showBrandPanel.ShouldBeTrue(),
                () => model.BrandFilterList.Count.ShouldBe(list.Count + 1),
                () => model.showDimensionPanel.ShouldBeTrue(),
                () => model.showPubTypePanel.ShouldBe(!isProductView),
                () => model.showDCPanel.ShouldBeTrue(),
                () => model.showProductPanel.ShouldBe(isProductView),
                () => model.showSavedLink.ShouldBeTrue(),
                () => model.showMarketPanel.ShouldBeTrue(),
                () => model.showDemoPanel.ShouldBeFalse(),
                () => model.DynamicData.ShouldNotBeNull(),
                () => model.DynamicData.DemosFilterList.ShouldNotBeNull(),
                () => model.CircFilter.ShouldNotBeNull(),
                () => model.geoFilter.ShouldNotBeNull(),
                () => model.standardFilter.ShouldNotBeNull(),
                () => model.filterCategoryTree.ShouldNotBeNull(),
                () => model.filters.ShouldNotBeNull());
        }

        private static void ShimForCommonFilters()
        {
            ShimFilterController.AllInstances.GetCircFilterBoolean = (a, b) => new CirculationFilter();
            ShimFilterController.AllInstances.GetGeoFilter = _ => new GeoFilter();
            ShimFilterController.AllInstances.GetStandardFilter = _ => new StandardFilter();
            ShimFilterController.AllInstances.GetAllFilterCategories = _ => new List<TreeViewItemModel>();
            ShimFilterController.AllInstances.GetAllResponseGroupInt32 = (_, __) => new List<Demos>();
            ShimFilterController.AllInstances.GetAllSearchEnabledProductsInt32 = (_, __) => new List<SelectListItem>();
        }
    }
}
