using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.UI.Fakes;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KM.Platform.Fakes;
using KMPlatform;
using KMPS.MD.Main;
using KMPS.MD.Main.Fakes;
using KMPS.MD.MasterPages;
using KMPS.MD.Objects;
using KMPlatform.Object;
using KMPS.MD.Objects.Fakes;
using NUnit.Framework;
using Shouldly;
using Telerik.Web.UI;
using Telerik.Web.UI.Fakes;
using FilterCategory = KMPS.MD.Objects.FilterCategory;
using QuestionCategory = KMPS.MD.Objects.QuestionCategory;
using static KMPS.MD.Objects.Enums;
using ShimFilterCategory = KMPS.MD.Objects.Fakes.ShimFilterCategory;
using ShimQuestionCategory = KMPS.MD.Objects.Fakes.ShimQuestionCategory;

namespace KMPS.MD.Tests.Main
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class QuestionsTests : BasePageTests
    {
        private const string TestZero = "0";
        private const string TestOne = "1";
        private const string AllProducts = "All Products";
        private const string MethodPageLoad = "Page_Load";
        private const string All = "All";
        private const string SampleField = "SampleField";
        private const string SampleFilterName = "SampleFilterName";
        private const string SampleGroup = "SampleGroup";
        private const string SampleFilterValues = "1, 2, 3";
        private const string SampleSearchCondition = "SampleSearchCondition";
        private const string GridFilterValues = "grdFilterValues";
        private const string AdhocFilterName = "Adhoc";
        private const string GridFilters = "grdFilters";
        private const string LabelViewType = "lblViewType";
        private const string LabelPubID = "lblPubID";
        private const string LabelBrandID = "lblBrandID";
        private const string Linkdownload = "lnkdownload";
        private const string LabelFilterText = "lblFilterText";
        private const string LabelFiltername = "lblFiltername";
        private const string LabelFilterValues = "lblFilterValues";
        private const string LabelSearchCondition = "lblSearchCondition";
        private const string LabellblGroup = "lblGroup";
        private const string LabelFilterType = "lblFilterType";
        private const string HiddenReloadValue = "hfReloadValue";
        private const string SampleCategory = "SampleCategory";

        private Questions _testEntity = new Questions();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            InitializePage(_testEntity);
            
            ShimFilterCategory.GetAllClientConnections = _ => new List<FilterCategory>();
            ShimQuestionCategory.GetAllClientConnections = _ => new List<QuestionCategory>();
            ShimRadGrid.AllInstances.DataBind = _ => { };
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
        public void Page_Load_WhenUserDoesNotHaveAccess_RedirectsToSecurityAccess()
        {
            // Arrange
            ShimBrand.GetByUserIDClientConnectionsInt32 = (_, __) => new List<Brand>();
            ShimBrand.GetAllClientConnections = _ => new List<Brand>();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (_, __, ___, ____) => false;
            ShimQuestionCategory.GetAllClientConnections = _ => new List<QuestionCategory>
            {
                new QuestionCategory { QuestionCategoryID = 1, ParentID = 1 , CategoryName = SampleCategory },
                new QuestionCategory { QuestionCategoryID = 2, ParentID = 1 , CategoryName = SampleCategory }
            }; 

            // Act
            PrivatePage.Invoke(MethodPageLoad, this, EventArgs.Empty);

            // Assert
            PrivatePage.ShouldSatisfyAllConditions(
                () => GetField<Button>("btnDownloadQuestions").Visible.ShouldBeFalse(),
                () => GetField<Button>("btnSamplePdf").Visible.ShouldBeFalse(),
                () => IsPageRedirected.ShouldBeTrue(),
                () => RedirectedUrl.ShouldNotBeNullOrWhiteSpace(),
                () => RedirectedUrl.ShouldContain("SecurityAccessError.aspx"));
        }

        [Test]
        public void Page_Load_WhenPageIsPostBack_ExecutesFilter()
        {
            // Arrange
            var isFilterExecuted = false;
            Filters executedFilterInstance = null;
            ShimBrand.GetByUserIDClientConnectionsInt32 = (_, __) => new List<Brand>();
            ShimBrand.GetAllClientConnections = _ => new List<Brand>();
            ShimPage.AllInstances.IsPostBackGet = (p) => true;
            SetUpPageControls();
            ShimFilters.AllInstances.Execute = (filter) =>
            {
                executedFilterInstance = filter;
                isFilterExecuted = true;
            };
            
            // Act
            PrivatePage.Invoke(MethodPageLoad, this, EventArgs.Empty);

            // Assert
            PrivatePage.ShouldSatisfyAllConditions(
                () => isFilterExecuted.ShouldBeTrue(),
                () => executedFilterInstance.ShouldNotBeNull(),
                () => executedFilterInstance.Count.ShouldBe(1));
        }

        [Test]
        public void Page_Load_WhenPageIsPostBackAndReloadValueFalse_ExecutesFilter()
        {
            // Arrange
            ShimBrand.GetByUserIDClientConnectionsInt32 = (_, __) => new List<Brand>();
            ShimBrand.GetAllClientConnections = _ => new List<Brand>();
            ShimPage.AllInstances.IsPostBackGet = (p) => true;
            GetField<HiddenField>(HiddenReloadValue).Value = bool.FalseString;
            
            // Act
            PrivatePage.Invoke(MethodPageLoad, this, EventArgs.Empty);

            // Assert
            PrivatePage.ShouldSatisfyAllConditions(
                () => GetField<Button>("btnDownloadQuestions").Visible.ShouldBeTrue(),
                () => GetField<Button>("btnSamplePdf").Visible.ShouldBeTrue(),
                () => GetField<HiddenField>(HiddenReloadValue).Value.ShouldBe(bool.TrueString.ToLower()));
        }

        private void SetUpPageControls()
        {
            SetFiltersGrid();
            GetField<HiddenField>(HiddenReloadValue).Value = bool.TrueString;
        }

        private void SetFiltersGrid()
        {
            var gvSelectedGroups = GetField<GridView>(GridFilters);
            var filters = new Filters(new ClientConnections(), 1);
            filters.Add(new Filter
            {
                FilterName = SampleFilterName,
                BrandID = 1,
                PubID = 1,
                FilterNo = 1
            });
            gvSelectedGroups.DataKeyNames = new[] { nameof(Filter.FilterNo) };
            gvSelectedGroups.DataSource = filters;
            gvSelectedGroups.DataBind();
            gvSelectedGroups.Rows[0].RowType = DataControlRowType.DataRow;

            gvSelectedGroups.Rows[0].Cells[0].Controls.Add(new LinkButton { ID = Linkdownload, Text = "5" });
            gvSelectedGroups.Rows[0].Cells[1].Controls.Add(new Label { ID = LabelViewType, Text = ViewType.RecordDetails.ToString() });
            gvSelectedGroups.Rows[0].Cells[2].Controls.Add(new Label { ID = LabelPubID, Text = "1" });
            gvSelectedGroups.Rows[0].Cells[3].Controls.Add(new Label { ID = LabelBrandID, Text = "1" });
            gvSelectedGroups.Rows[0].Cells[4].Controls.Add(GetFiltersValuesGrid());
        }

        private GridView GetFiltersValuesGrid()
        {
            var gvSelectedGroups = new GridView();
            gvSelectedGroups.ID = GridFilterValues;
            var fields = new List<Field>
            {
                new Field{ Text = $"{SampleField}1" },
                new Field{ Text = $"{SampleField}2" },
            };
            gvSelectedGroups.DataSource = fields;
            gvSelectedGroups.DataBind();
            for (var i = 0; i < gvSelectedGroups.Rows.Count; i++)
            {
                gvSelectedGroups.Rows[i].Cells[0].Controls.Add(new Label { ID = LabelFilterText, Text = $"{SampleField}{i}" });
                gvSelectedGroups.Rows[i].Cells[1].Controls.Add(new Label { ID = LabelFiltername, Text = i == 1 ? AdhocFilterName : SampleFilterName });
                gvSelectedGroups.Rows[i].Cells[2].Controls.Add(new Label { ID = LabelFilterValues, Text = SampleFilterValues });
                gvSelectedGroups.Rows[i].Cells[3].Controls.Add(new Label { ID = LabelSearchCondition, Text = $"{SampleSearchCondition}{i}" });
                gvSelectedGroups.Rows[i].Cells[3].Controls.Add(new Label { ID = LabellblGroup, Text = $"{SampleGroup}{i}" });
                gvSelectedGroups.Rows[i].Cells[3].Controls.Add(new Label { ID = LabelFilterType, Text = FiltersType.Brand.ToString() });
            }
            return gvSelectedGroups;
        }
    }
}
