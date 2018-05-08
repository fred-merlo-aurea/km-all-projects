using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Fakes;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using KM.Platform.Fakes;
using KMPS.MD.Fakes;
using KMPS.MD.Main;
using KMPS.MD.Main.Fakes;
using KMPS.MD.Objects;
using KMPS.MD.Objects.Fakes;
using NUnit.Framework;
using Shouldly;
using TestCommonHelpers;
using FilterCategory = KMPS.MD.Objects.FilterCategory;
using ShimFilterCategory = KMPS.MD.Objects.Fakes.ShimFilterCategory;

namespace KMPS.MD.Tests.Main
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FilterSchedulesTest : BasePageTests
    {
        private const string FilterSegmentation = "FilterSegmentation";
        private const string DummyString = "dummyString";
        private const string SortWithBrandName = "BRANDNAME";
        private const string SortWithExportName = "EXPORTNAME";
        private const string SortWithExpottNotes = "EXPORTNOTES";
        private const string SortWithFilterName = "FILTERNAME";
        private const string SortWithFilterSegmentationName = "FILTERSEGMENTATIONNAME";
        private const string SortWithRecurrenceType = "RECURRENCETYPE";
        private const string SortWithStartDate = "STARTDATE";
        private const string SortWithStartTime = "STARTTIME";
        private const string SortWithEndDate = "ENDDATE";
        private const string SortAscending = "ASC";
        private const string SortDescending = "DESC";
        private const string TestZero = "0";
        private const string TestOne = "1";
        private const string TestMinusOne = "-1";
        private const string MethodLoadFilterSegmentationGrid = "LoadFilterSegmentationGrid";
        private const string MethodLoadBrands = "LoadBrands";
        private const string MethodPageLoad = "Page_Load";
        private const string MethodLoadGrid = "LoadGrid";
        private const string FilterEqual = "EQUAL";
        private const string FilterStartWith = "START WITH";
        private const string FilterEndWith = "END WITH";
        private const string FilterContains = "CONTAINS";
        private const string MethodgvFilterSchedules_RowCommand = "gvFilterSchedules_RowCommand";
        private const string MethodgvFilterSegmentationSchedules_RowCommand = "gvFilterSegmentationSchedules_RowCommand";
        private const string ExpectedErrorMessage = "Command failed";
        private const string DeleteCommandName = "Delete";
        private const string ExportCommandName = "Export";
        private string _currentFilter = "EQUAL";
        private string _currentSort = "BRANDNAME";
        private string _currentSortDirection = "ASC";
        private int? _filterSegmentationID = 1;
        private NameValueCollection _querySting;
        private readonly FilterSchedules _testEntity = new FilterSchedules();
        private string _outputFileName = string.Empty;       
        private string _actualErrorMessage = string.Empty; 
        private bool _isDeletCalled;
        private bool _isLoadFilterSegmentationGridCalled;
        private bool _isLoadGridCalled;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            InitializePage(_testEntity);
            _querySting = new NameValueCollection();
        }

        [TestCase(TestZero)]
        [TestCase(TestOne)]
        [TestCase(TestMinusOne)]
        public void LoadFilterSegmentationGrid_Success_SegmentationSchedulesAreShown(string brandId)
        {
            // Arrange
            _currentFilter = FilterEqual;
            GetField<TextBox>("txtSearch").Text = FilterEqual;
            SetSearchDropDown(FilterEqual);
            CreateShimsForLoadGrid();
            ReflectionHelper.SetField(_testEntity, "hfBrandID", new HiddenField { Value = brandId });

            // Act
            PrivatePage.Invoke(MethodLoadFilterSegmentationGrid);

            // Assert
            var schedulesFilter = ReflectionHelper.GetField(_testEntity, "gvFilterSegmentationSchedules") as GridView;
            var schedulesFilterDataSource = schedulesFilter.DataSource as List<FilterSchedule>;
            var filterName = schedulesFilterDataSource.FirstOrDefault().FilterName;
            _testEntity.ShouldSatisfyAllConditions(
                () => schedulesFilter.DataSource.ShouldNotBeNull(),
                () => schedulesFilterDataSource.ShouldNotBeNull(),
                () => GetField<GridView>("gvFilterSchedules").Columns.ShouldNotBeNull(),
                () => filterName.ShouldBe(FilterEqual));
        }

        [Test]
        public void LoadFilterSegmentationGrid_UserHasNoEditAccess_SegmentationSchedulesColumn20IsHidden()
        {
            // Arrange
            _currentFilter = FilterEqual;
            SetSearchDropDown(FilterEqual);
            CreateShimsForLoadGrid();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = 
                (a, b, c, userAccess) => userAccess != KMPlatform.Enums.Access.Edit;

            // Act
            PrivatePage.Invoke(MethodLoadFilterSegmentationGrid);

            // Assert
            var schedulesFilter = ReflectionHelper.GetField(_testEntity, "gvFilterSegmentationSchedules") as GridView;
            schedulesFilter.Columns[20].Visible.ShouldBeFalse(); 
        }

        [Test]
        public void LoadFilterSegmentationGrid_UserHasNoDeleteAccess_SegmentationSchedulesColumn21IsHidden()
        {
            // Arrange
            _currentFilter = FilterEqual;
            SetSearchDropDown(FilterEqual);
            CreateShimsForLoadGrid();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess =
                (a, b, c, userAccess) => userAccess != KMPlatform.Enums.Access.Delete;

            // Act
            PrivatePage.Invoke(MethodLoadFilterSegmentationGrid);

            // Assert
            var schedulesFilter = ReflectionHelper.GetField(_testEntity, "gvFilterSegmentationSchedules") as GridView;
            schedulesFilter.Columns[21].Visible.ShouldBeFalse();
        }

        [TestCase(FilterEqual)]
        [TestCase(FilterStartWith)]
        [TestCase(FilterEndWith)]
        [TestCase(FilterContains)]
        public void LoadFilterSegmentationGrid_WithSelectedSearchFilter_FilterNameIsChanged(string filter)
        {
            // Arrange
            _currentFilter = filter;
            GetField<TextBox>("txtSearch").Text = filter;
            GetField<HiddenField>("hfBrandID").Value = TestOne;
            SetSearchDropDown(filter);
            CreateShimsForLoadGrid();

            // Act
            PrivatePage.Invoke(MethodLoadFilterSegmentationGrid);

            // Assert
            var schedulesFilter = ReflectionHelper.GetField(_testEntity, "gvFilterSegmentationSchedules") as GridView;
            var schedulesFilterDataSource = schedulesFilter.DataSource as List<FilterSchedule>;
            var filterName = schedulesFilterDataSource.FirstOrDefault().FilterName;
            _testEntity.ShouldSatisfyAllConditions(
                () => schedulesFilter.DataSource.ShouldNotBeNull(),
                () => schedulesFilterDataSource.ShouldNotBeNull(),
                () => filterName.ShouldBe(filter));
        }

        [TestCase(SortWithBrandName)]
        [TestCase(SortWithExportName)]
        [TestCase(SortWithExpottNotes)]
        [TestCase(SortWithFilterName)]
        [TestCase(SortWithFilterSegmentationName)]
        [TestCase(SortWithRecurrenceType)]
        [TestCase(SortWithStartDate)]
        [TestCase(SortWithStartTime)]
        [TestCase(SortWithEndDate)]
        public void LoadFilterSegmentationGrid_WithSelectedSortField_ListIsSorted(string sortField)
        {
            // Arrange
            _currentSort = sortField;
            _currentSortDirection = SortAscending;
            _currentFilter = FilterEqual;
            GetField<TextBox>("txtSearch").Text = FilterEqual;
            GetField<HiddenField>("hfBrandID").Value = TestOne;
            SetSearchDropDown(FilterEqual);
            CreateShimsForLoadGrid();

            // Act
            PrivatePage.Invoke(MethodLoadFilterSegmentationGrid);

            // Assert
            var schedulesFilter = ReflectionHelper.GetField(_testEntity, "gvFilterSegmentationSchedules") as GridView;
            var schedulesFilterDataSource = schedulesFilter.DataSource as List<FilterSchedule>;
            var filterName = schedulesFilterDataSource.FirstOrDefault().FilterName;
            _testEntity.ShouldSatisfyAllConditions(
                () => schedulesFilter.DataSource.ShouldNotBeNull(),
                () => schedulesFilterDataSource.ShouldNotBeNull(),
                () => filterName.ShouldBe(FilterEqual));
        }

        [TestCase(SortWithBrandName)]
        [TestCase(SortWithExportName)]
        [TestCase(SortWithExpottNotes)]
        [TestCase(SortWithFilterName)]
        [TestCase(SortWithFilterSegmentationName)]
        [TestCase(SortWithRecurrenceType)]
        [TestCase(SortWithStartDate)]
        [TestCase(SortWithStartTime)]
        [TestCase(SortWithEndDate)]
        public void LoadFilterSegmentationGrid_WithSelectedSortFieldDescending_ListIsSortedInDescendingOrder(string sortField)
        {
            // Arrange
            _currentSort = sortField;
            _currentSortDirection = SortDescending;
            _currentFilter = FilterEqual;
            GetField<TextBox>("txtSearch").Text = FilterEqual;
            GetField<HiddenField>("hfBrandID").Value = TestOne;
            SetSearchDropDown(FilterEqual);
            CreateShimsForLoadGrid();

            // Act
            PrivatePage.Invoke(MethodLoadFilterSegmentationGrid);

            // Assert
            var schedulesFilter = ReflectionHelper.GetField(_testEntity, "gvFilterSegmentationSchedules") as GridView;
            var schedulesFilterDataSource = schedulesFilter.DataSource as List<FilterSchedule>;
            var filterName = schedulesFilterDataSource.FirstOrDefault().FilterName;
            _testEntity.ShouldSatisfyAllConditions(
                () => schedulesFilter.DataSource.ShouldNotBeNull(),
                () => schedulesFilterDataSource.ShouldNotBeNull(),
                () => filterName.ShouldBe(FilterEqual));
        }

        [TestCase(TestZero)]
        [TestCase(TestOne)]
        [TestCase(TestMinusOne)]
        public void LoadGrid_Success_NonSegmentationSchedulesAreShown(string brandId)
        {
            // Arrange
            _filterSegmentationID = null;
            _currentFilter = FilterEqual;
            GetField<TextBox>("txtSearch").Text = FilterEqual;
            SetSearchDropDown(FilterEqual);
            CreateShimsForLoadGrid();
            ReflectionHelper.SetField(_testEntity, "hfBrandID", new HiddenField { Value = brandId });

            // Act
            PrivatePage.Invoke(MethodLoadGrid);

            // Assert
            var schedulesFilter = ReflectionHelper.GetField(_testEntity, "gvFilterSchedules") as GridView;
            var schedulesFilterDataSource = schedulesFilter.DataSource as List<FilterSchedule>;
            var filterName = schedulesFilterDataSource.FirstOrDefault().FilterName;
            _testEntity.ShouldSatisfyAllConditions(
                () => schedulesFilter.DataSource.ShouldNotBeNull(),
                () => schedulesFilterDataSource.ShouldNotBeNull(),
                () => GetField<GridView>("gvFilterSchedules").Columns.ShouldNotBeNull(),
                () => filterName.ShouldBe(FilterEqual));
        }

        [TestCase(FilterEqual)]
        [TestCase(FilterStartWith)]
        [TestCase(FilterEndWith)]
        [TestCase(FilterContains)]
        public void LoadGrid_WithSelectedSearchFilter_FilterNameIsChanged(string filter)
        {
            // Arrange
            _filterSegmentationID = null;
            _currentFilter = filter;
            GetField<TextBox>("txtSearch").Text = filter;
            GetField<HiddenField>("hfBrandID").Value = TestOne;
            SetSearchDropDown(filter);
            CreateShimsForLoadGrid();

            // Act
            PrivatePage.Invoke(MethodLoadGrid);

            // Assert
            var schedulesFilter = ReflectionHelper.GetField(_testEntity, "gvFilterSchedules") as GridView;
            var schedulesFilterDataSource = schedulesFilter.DataSource as List<FilterSchedule>;
            var filterName = schedulesFilterDataSource.FirstOrDefault().FilterName;
            _testEntity.ShouldSatisfyAllConditions(
                () => schedulesFilter.DataSource.ShouldNotBeNull(),
                () => schedulesFilterDataSource.ShouldNotBeNull(),
                () => filterName.ShouldBe(filter));
        }

        [TestCase(SortWithBrandName)]
        [TestCase(SortWithExportName)]
        [TestCase(SortWithExpottNotes)]
        [TestCase(SortWithFilterName)]
        [TestCase(SortWithRecurrenceType)]
        [TestCase(SortWithStartDate)]
        [TestCase(SortWithStartTime)]
        [TestCase(SortWithEndDate)]
        public void LoadGrid_WithSelectedSortField_ListIsSorted(string sortField)
        {
            // Arrange
            _filterSegmentationID = null;
            _currentSort = sortField;
            _currentSortDirection = SortAscending;
            _currentFilter = FilterEqual;
            GetField<TextBox>("txtSearch").Text = FilterEqual;
            GetField<HiddenField>("hfBrandID").Value = TestOne;
            SetSearchDropDown(FilterEqual);
            CreateShimsForLoadGrid();

            // Act
            PrivatePage.Invoke(MethodLoadGrid);

            // Assert
            var schedulesFilter = ReflectionHelper.GetField(_testEntity, "gvFilterSchedules") as GridView;
            var schedulesFilterDataSource = schedulesFilter.DataSource as List<FilterSchedule>;
            var filterName = schedulesFilterDataSource.FirstOrDefault().FilterName;
            _testEntity.ShouldSatisfyAllConditions(
                () => schedulesFilter.DataSource.ShouldNotBeNull(),
                () => schedulesFilterDataSource.ShouldNotBeNull(),
                () => filterName.ShouldBe(FilterEqual));
        }

        [TestCase(SortWithBrandName)]
        [TestCase(SortWithExportName)]
        [TestCase(SortWithExpottNotes)]
        [TestCase(SortWithFilterName)]
        [TestCase(SortWithRecurrenceType)]
        [TestCase(SortWithStartDate)]
        [TestCase(SortWithStartTime)]
        [TestCase(SortWithEndDate)]
        public void LoadGrid_WithSelectedSortFieldDescending_ListIsSortedInDescendingOrder(string sortField)
        {
            // Arrange
            _filterSegmentationID = null;
            _currentSort = sortField;
            _currentSortDirection = SortDescending;
            _currentFilter = FilterEqual;
            GetField<TextBox>("txtSearch").Text = FilterEqual;
            GetField<HiddenField>("hfBrandID").Value = TestOne;
            SetSearchDropDown(FilterEqual);
            CreateShimsForLoadGrid();

            // Act
            PrivatePage.Invoke(MethodLoadGrid);

            // Assert
            var schedulesFilter = ReflectionHelper.GetField(_testEntity, "gvFilterSchedules") as GridView;
            var schedulesFilterDataSource = schedulesFilter.DataSource as List<FilterSchedule>;
            var filterName = schedulesFilterDataSource.FirstOrDefault().FilterName;
            _testEntity.ShouldSatisfyAllConditions(
                () => schedulesFilter.DataSource.ShouldNotBeNull(),
                () => schedulesFilterDataSource.ShouldNotBeNull(),
                () => filterName.ShouldBe(FilterEqual));
        }

        [Test]
        public void Page_Load_WhenUserIsNotAdministrator_BrandsAreLoadedByUserId()
        {
            // Arrange
            var brandsLoadedByUserId = false;
            ShimBrand.GetByUserIDClientConnectionsInt32 = (x, y) =>
            {
                brandsLoadedByUserId = true;
                return new List<Brand>
                {
                    ReflectionHelper.CreateInstance(typeof(Brand))
                };
            };

            // Act
            ReflectionHelper.CallMethod(_testEntity, MethodPageLoad, null, EventArgs.Empty);

            // Assert
            brandsLoadedByUserId.ShouldBeTrue();
        }

        [Test]
        public void Page_Load_WhenModeIsFilterSegmentation_LoadFilterSegmentationGridIsCalled()
        {
            // Arrange
            var filterSegmentationGridCalled = false;
            ShimBrand.GetByUserIDClientConnectionsInt32 = (x, y) => new List<Brand> { ReflectionHelper.CreateInstance(typeof(Brand)) };
            ShimFilterSchedules.AllInstances.ModeGet = (x) => FilterSegmentation; ;
            ShimFilterSchedules.AllInstances.LoadFilterSegmentationGrid = (x) =>
            {
                filterSegmentationGridCalled = true;
            };

            // Act
            ReflectionHelper.CallMethod(_testEntity, MethodPageLoad, null, EventArgs.Empty);

            // Assert
            filterSegmentationGridCalled.ShouldBeTrue();
        }

        [Test]
        public void Page_Load_WhenModeIsNotFilterSegmentation_LoadGridIsCalled()
        {
            // Arrange
            var loadGridCalled = false;
            ShimBrand.GetByUserIDClientConnectionsInt32 = (x, y) => new List<Brand> { ReflectionHelper.CreateInstance(typeof(Brand)) };
            ShimFilterSchedules.AllInstances.ModeGet = (x) => DummyString;
            ShimFilterSchedules.AllInstances.LoadGrid = (x) =>
            {
                loadGridCalled = true;
            };

            // Act
            ReflectionHelper.CallMethod(_testEntity, MethodPageLoad, null, EventArgs.Empty);

            // Assert
            loadGridCalled.ShouldBeTrue();
        }

        [Test]
        public void Page_Load_WhenBrandListIsEmpty_AllBrandsAreLoaded()
        {
            // Arrange
            var allBrandsLoaded = false;
            ShimBrand.GetByUserIDClientConnectionsInt32 = (x, y) => new List<Brand>();
            ShimBrand.GetAllClientConnections = (x) =>
            {
                allBrandsLoaded = true;
                return new List<Brand> { ReflectionHelper.CreateInstance(typeof(Brand)) };
            };

            // Act
            ReflectionHelper.CallMethod(_testEntity, MethodPageLoad, null, EventArgs.Empty);

            // Assert
            allBrandsLoaded.ShouldBeTrue();
        }

        [TestCase(true, true)]
        public void Page_Load_WhenMoreThanOneBrands_LoadPageFiltersIsCalled(bool isAdministrator, bool result)
        {
            // Arrange
            _querySting.Add("BrandID", TestOne);
            CreateShimsForMethodLoadBrands(new { NumberOfBrands = 1 });
            var loadPageFiltersCalled = false;
            ShimUser.IsAdministratorUser = (x) => isAdministrator;
            ShimFilterSchedules.AllInstances.LoadPageFilters = (x) =>
            {
                loadPageFiltersCalled = true;
            };

            // Act
            ReflectionHelper.CallMethod(_testEntity, MethodPageLoad, null, EventArgs.Empty);

            // Assert
            var brandId = ReflectionHelper.GetBaseClassProperty(_testEntity, "BrandIdHiddenField") as HiddenField;
            _testEntity.ShouldSatisfyAllConditions(
                () => brandId.ShouldNotBeNull(),
                () => loadPageFiltersCalled.ShouldBe(result),
                () => brandId.Value.ShouldNotBeNullOrWhiteSpace());
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public void Page_Load_WhenSingleBrands_ShowBrandUIIsCalled(bool isAdministrator, bool result)
        {
            // Arrange
            _querySting.Add("BrandID", TestOne);
            CreateShimsForMethodLoadBrands(new { NumberOfBrands = 1 });
            var showBrandUiCalled = false;
            ShimUser.IsAdministratorUser = (x) => isAdministrator;
            ShimBrandsPageBase.AllInstances.ShowBrandUIBrand = (x, y) =>
            {
                showBrandUiCalled = true;
            };

            // Act
            ReflectionHelper.CallMethod(_testEntity, MethodPageLoad, null, EventArgs.Empty);

            // Assert
            var brandId = ReflectionHelper.GetBaseClassProperty(_testEntity, "BrandIdHiddenField") as HiddenField;
            _testEntity.ShouldSatisfyAllConditions(
                () => brandId.ShouldNotBeNull(),
                () => showBrandUiCalled.ShouldBe(result),
                () => brandId.Value.ShouldNotBeNullOrWhiteSpace());
        }

        [Test]
        public void gvFilterSchedules_RowCommand_DeleteCommand_DeleteMethodCalled()
        {
            // Arrange          
            var commandEventArgs = ArrangeShimsForgvFilterScheduleMethods(DeleteCommandName, true);

            // Act
            ReflectionHelper.CallMethod(_testEntity, MethodgvFilterSchedules_RowCommand, null, commandEventArgs);

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => _isDeletCalled.ShouldBeTrue(),
                () => _isLoadGridCalled.ShouldBeTrue());
        }

        [Test]
        public void gvFilterSchedules_RowCommand_DeleteCommandAndUserDoesNotHaveAccess_DeleteMethodNotCalled()
        {
            // Arrange          
            var commandEventArgs = ArrangeShimsForgvFilterScheduleMethods(DeleteCommandName, false);

            // Act
            ReflectionHelper.CallMethod(_testEntity, MethodgvFilterSchedules_RowCommand, null, commandEventArgs);

            // Assert
            _isDeletCalled.ShouldBeFalse();
        }

        [Test]
        public void gvFilterSchedules_RowCommand_DeleteCommandFailsWithException_ShowsError()
        {
            // Arrange        
            var commandEventArgs = ArrangeShimsForgvFilterScheduleMethods(DeleteCommandName, true);
            ShimFilterSchedule.DeleteClientConnectionsInt32Int32 = (_, __, ___) =>
            {                
                throw new InvalidOperationException(ExpectedErrorMessage);
            };
            
            // Act
            ReflectionHelper.CallMethod(_testEntity, MethodgvFilterSchedules_RowCommand, null, commandEventArgs);

            // Assert
            _actualErrorMessage.ShouldContain(ExpectedErrorMessage);
        }

        [Test]
        public void gvFilterSchedules_RowCommand_ExportCommand_ExportMethodCalled()
        {
            // Arrange                     
            var commandEventArgs = ArrangeShimsForgvFilterScheduleMethods(ExportCommandName, true);

            // Act
            ReflectionHelper.CallMethod(_testEntity, MethodgvFilterSchedules_RowCommand, null, commandEventArgs);

            // Assert
            _outputFileName.ShouldEndWith(".tsv");
        }

        [Test]
        public void gvFilterSchedules_RowCommand_ExportCommandFails_ShowError()
        {
            // Arrange                     
            var commandEventArgs = ArrangeShimsForgvFilterScheduleMethods(ExportCommandName, false);
            ShimUtilities.DownloadDataTableStringStringInt32Int32 = (_, fileName, ___, ____, _____) =>
            {
                throw new InvalidOperationException(ExpectedErrorMessage);
            };

            // Act
            ReflectionHelper.CallMethod(_testEntity, MethodgvFilterSchedules_RowCommand, null, commandEventArgs);

            // Assert
            _actualErrorMessage.ShouldContain(ExpectedErrorMessage);
        }

        [Test]
        public void gvFilterSegmentationSchedules_RowCommand_DeleteCommand_DeleteMethodCalled()
        {
            // Arrange          
            var commandEventArgs = ArrangeShimsForgvFilterScheduleMethods(DeleteCommandName, true);

            // Act
            ReflectionHelper.CallMethod(_testEntity, MethodgvFilterSegmentationSchedules_RowCommand, null, commandEventArgs);

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => _isDeletCalled.ShouldBeTrue(),
                () => _isLoadFilterSegmentationGridCalled.ShouldBeTrue());
        }

        [Test]
        public void gvFilterSegmentationSchedules_RowCommand_DeleteCommand_LoadFilterSegmentationGridMethodCalled()
        {
            // Arrange          
            var commandEventArgs = ArrangeShimsForgvFilterScheduleMethods(DeleteCommandName, true);            
            
            // Act
            ReflectionHelper.CallMethod(_testEntity, MethodgvFilterSegmentationSchedules_RowCommand, null, commandEventArgs);

            // Assert
            _isDeletCalled.ShouldBeTrue();
        }

        [Test]
        public void gvFilterSegmentationSchedules_RowCommand_DeleteCommandAndUserDoesNotHaveAccess_DeleteMethodNotCalled()
        {
            // Arrange          
            var commandEventArgs = ArrangeShimsForgvFilterScheduleMethods(DeleteCommandName, false);

            // Act
            ReflectionHelper.CallMethod(_testEntity, MethodgvFilterSegmentationSchedules_RowCommand, null, commandEventArgs);

            // Assert
            _isDeletCalled.ShouldBeFalse();
        }

        [Test]
        public void gvFilterSegmentationSchedules_RowCommand_DeleteCommandFailsWithException_ShowsError()
        {
            // Arrange        
            var commandEventArgs = ArrangeShimsForgvFilterScheduleMethods(DeleteCommandName, true);
            ShimFilterSchedule.DeleteClientConnectionsInt32Int32 = (_, __, ___) =>
            {
                throw new InvalidOperationException(ExpectedErrorMessage);
            };

            // Act
            ReflectionHelper.CallMethod(_testEntity, MethodgvFilterSegmentationSchedules_RowCommand, null, commandEventArgs);

            // Assert
            _actualErrorMessage.ShouldContain(ExpectedErrorMessage);
        }

        [Test]
        public void gvFilterSegmentationSchedules_RowCommand_ExportCommand_ExportMethodCalled()
        {
            // Arrange                     
            var commandEventArgs = ArrangeShimsForgvFilterScheduleMethods(ExportCommandName, true);

            // Act
            ReflectionHelper.CallMethod(_testEntity, MethodgvFilterSegmentationSchedules_RowCommand, null, commandEventArgs);

            // Assert
            _outputFileName.ShouldEndWith(".tsv");
        }

        [Test]
        public void gvFilterSegmentationSchedules_RowCommand_ExportCommandFails_ShowError()
        {
            // Arrange                     
            var commandEventArgs = ArrangeShimsForgvFilterScheduleMethods(ExportCommandName, false);
            ShimUtilities.DownloadDataTableStringStringInt32Int32 = (_, fileName, ___, ____, _____) =>
            {
                throw new InvalidOperationException(ExpectedErrorMessage);
            };

            // Act
            ReflectionHelper.CallMethod(_testEntity, MethodgvFilterSegmentationSchedules_RowCommand, null, commandEventArgs);

            // Assert
            _actualErrorMessage.ShouldContain(ExpectedErrorMessage);
        }

        private GridViewCommandEventArgs ArrangeShimsForgvFilterScheduleMethods(string command, bool hasAccessRights)
        {
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (_, __, ___, ____) => hasAccessRights;
            
            ShimFilterSchedule.ExportClientConnectionsInt32 = (_, ___) =>
                new Tuple<DataTable, string, DataTable, bool>(new DataTable(), String.Empty, new DataTable(), false);
            ShimHttpServerUtility.AllInstances.MapPathString = (_, __) => string.Empty;

            _outputFileName = string.Empty;
            ShimUtilities.DownloadDataTableStringStringInt32Int32 = (_, fileName, ___, ____, _____) =>
            {
                _outputFileName = fileName;
            };

            _isDeletCalled = false;
            ShimFilterSchedule.DeleteClientConnectionsInt32Int32 = (_, __, ___) =>
            {
                _isDeletCalled = true;
            };

            ShimFilterSchedules.AllInstances.DisplayErrorString = (_, error) => { _actualErrorMessage = error; };

            ShimFilterSchedules.AllInstances.LoadFilterSegmentationGrid = 
                (_) => { _isLoadFilterSegmentationGridCalled = true; };

            ShimFilterSchedules.AllInstances.LoadGrid =
                (_) => { _isLoadGridCalled = true; };

            return new GridViewCommandEventArgs(
                    new Object(),
                    new CommandEventArgs(command, 1));
        }

        private void CreatedCommonShims()
        {
            ShimFilterList.AllInstances.ResetControls = (x) => { };
            ShimFilterList.AllInstances.LoadGrid = (x) => { };
            ShimFilterList.AllInstances.LoadFilterSegmentationGrid = (x) => { };
            ShimFilterCategory.GetAllClientConnections = (x) => new List<FilterCategory>();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (a, b, c, d) => true;
            SetDefaults();
        }

        private void CreateShimsForMethodLoadBrands(dynamic args = null)
        {
            ShimBaseDataBoundControl.AllInstances.DataBind = (x) => { };
            ShimHttpRequest.AllInstances.QueryStringGet = (x) => _querySting;
            ShimBrand.GetByUserIDClientConnectionsInt32 = (x, y) => new List<Brand>
            {
                ReflectionHelper.CreateInstance(typeof(Brand)),
                ReflectionHelper.CreateInstance(typeof(Brand))
            };
            ShimBrand.GetAllClientConnections = (x) => new List<Brand>
            {
                ReflectionHelper.CreateInstance(typeof(Brand)),
                ReflectionHelper.CreateInstance(typeof(Brand))
            };
            if (args != null && args.NumberOfBrands == 1)
            {
                ShimBrand.GetByUserIDClientConnectionsInt32 = (x, y) => new List<Brand>
                {
                    ReflectionHelper.CreateInstance(typeof(Brand))
                };
                ShimBrand.GetAllClientConnections = (x) => new List<Brand>
                {
                    ReflectionHelper.CreateInstance(typeof(Brand))
                };
            }
            SetDefaults();
        }

        private void CreateShimsForLoadGrid()
        {
            var filterScheduleList = new List<FilterSchedule>();
            var dummyFilterSchedule = ReflectionHelper.CreateInstance(typeof(FilterSchedule));
            dummyFilterSchedule.BrandName = _currentFilter;
            dummyFilterSchedule.FilterName = _currentFilter;
            dummyFilterSchedule.FilterSegmentationID = _filterSegmentationID;
            filterScheduleList.Add(dummyFilterSchedule);
            var dataControlFieldCollection = new DataControlFieldCollection();
            for (var i = 0; i < 25; i++)
            {
                var dataContentField = new CheckBoxField { Visible = true };
                dataControlFieldCollection.Add(dataContentField);
            }
            ShimGridView.AllInstances.ColumnsGet = (x) => dataControlFieldCollection;
            ShimFilterBase.AllInstances.SortFieldFSGet = (x) => _currentSort;
            ShimFilterBase.AllInstances.SortDirectionFSGet = (x) => _currentSortDirection;
            ShimFilterSchedules.AllInstances.SortFieldGet = (x) => _currentSort;
            ShimFilterSchedules.AllInstances.SortDirectionGet = (x) => _currentSortDirection;
            ShimUser.IsAdministratorUser = (x) => true;
            ShimFilterSchedule.GetByBrandIDUserIDClientConnectionsInt32Int32Boolean = (a, b, c, d) => filterScheduleList;
            ShimFilterSchedule.GetByBrandIDClientConnectionsInt32Boolean = (a, b, c) => filterScheduleList;
            SetDefaults();
        }

        private void SetSearchDropDown(string filterValue)
        {
            ReflectionHelper.SetField(_testEntity, "drpSearch", new DropDownList
            {
                Items =
                {
                    new ListItem
                    {
                        Selected = true,
                        Value = filterValue
                    }
                }
            });
        }

        private void SetDefaults()
        {
            GetField<HiddenField>("hfBrandID").Value = TestZero;
            GetField<Panel>("pnlBrand").Visible = false;
            GetField<DropDownList>("drpBrand").Visible = false;
            GetField<Label>("lblBrandName").Visible = false;
        }
    }
}