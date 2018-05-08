using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using KMPS.MD.Main;
using KMPS.MD.Main.Fakes;
using KMPS.MD.Objects;
using KMPS.MD.Objects.Fakes;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using Telerik.Web.UI.Fakes;

namespace KMPS.MD.Tests.Main
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FilterListAspxTest: BasePageTests
    {
        private FilterList filterList = new FilterList();
        private const string TestZero = "0";
        private const string TestOne = "1";
        private const string TestNegative = "-1";
        private const string SortAsc = "ASC";
        private const string SortDsc = "DSC";
        private const string Filter = "filter";
        private const string BrandId = "hfBrandID";
        private const string Equal = "EQUAL";
        private const string StartWith = "START WITH";
        private const string EndWith = "END WITH";
        private const string Contains = "CONTAINS";
        private const string BrandName = "BRANDNAME";
        private const string Name = "NAME";
        private const string FilterType = "FILTERTYPE";
        private const string PubName = "PUBNAME";
        private const string FilterCategoryName = "FILTERCATEGORYNAME";
        private const string QuestionCategoryName = "QUESTIONCATEGORYNAME";
        private const string CreateDate = "CREATEDDATE";
        private const string CreateName = "CREATEDNAME";
        private const string NotesName = "NOTES";
        private const string StringId = "10";
        private const string TxtSearch = "txtSearch";
        private const string GvFilters = "gvFilters";
        private const string PnlBrand = "pnlBrand";
        private const string DrpSearch = "drpBrand";
        private const string LblBrandName = "lblBrandName";
        private const int Id = 10;
        private const int TestValueOne = 1;
        private const int TestNegativeValueOne = -1;
        private const int TestNegativeTwo = -2;
        private const int TestNegativeThree = -3;
        private const int TestNegativeFour = -4;
        private const int TestNegativeFive = -5;
        private const int TestNegativeSix = -6;
        private const int TestValueZero = 0;
        private const bool True = true;
        private const bool False = false;

        private List<MDFilter> mdList;
        private List<User> userList;

        [Test]
        [TestCase(TestValueOne, True, SortAsc)]
        [TestCase(TestValueOne, True, SortDsc)]
        [TestCase(TestValueOne, False, SortAsc)]
        [TestCase(TestValueOne, False, SortDsc)]
        [TestCase(TestNegativeValueOne, True, SortAsc)]
        [TestCase(TestNegativeValueOne, True, SortDsc)]
        [TestCase(TestValueZero, True, SortAsc)]
        [TestCase(TestValueZero, True, SortDsc)]
        [TestCase(TestValueZero, False, SortAsc)]
        [TestCase(TestValueZero, False, SortDsc)]
        [TestCase(TestNegativeTwo, False, SortAsc)]
        [TestCase(TestNegativeTwo, False, SortDsc)]
        [TestCase(TestNegativeThree, False, SortAsc)]
        [TestCase(TestNegativeThree, False, SortDsc)]
        [TestCase(TestNegativeFour, False, SortAsc)]
        [TestCase(TestNegativeFour, False, SortDsc)]
        [TestCase(TestNegativeFive, False, SortAsc)]
        [TestCase(TestNegativeFive, False, SortDsc)]
        [TestCase(TestNegativeSix, False, SortAsc)]
        [TestCase(TestNegativeSix, False, SortDsc)]
        public void LoadGrid_ForNegativeValueAndAdministrator_PopulateValues(int value, bool admin, string sort)
        {
            // Arrange
            SetUpLoadGrid(value, admin, sort);

            // Act
            filterList.LoadGrid();

            // Assert
            filterList.ShouldSatisfyAllConditions(
                () => GetField<Panel>(PnlBrand).Visible.ShouldBeFalse(),
                () => GetField<DropDownList>(DrpSearch).Visible.ShouldBeFalse(),
                () => GetField<Label>(LblBrandName).Visible.ShouldBeFalse());
        }

        public void SetUpLoadGrid(int value, bool admin, string sort)
        {
            base.SetUp();
            InitializePage(filterList);
            KM.Platform.Fakes.ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (_, __, ___, ____) => True;
            if(value > TestValueZero && admin == True && sort.Equals(SortAsc))
            {
                KM.Platform.Fakes.ShimUser.IsAdministratorUser = (_) => True;
                GetField<HiddenField>(BrandId).Value = TestOne;
                mdList = new List<MDFilter>();
                var mdFilter = new MDFilter()
                {
                    FilterCategoryID = Id,
                    BrandName = Filter,
                    CreatedUserID = Id,
                    Name = Filter
                };
                mdList.Add(mdFilter);
                ShimMDFilter.GetByBrandIDClientConnectionsInt32 = (x, y) => mdList;
                ShimListControl.AllInstances.SelectedValueGet = (x) => Equal;
                ShimFilterList.AllInstances.SortFieldGet = (x) => BrandName;
                ShimFilterList.AllInstances.SortDirectionGet = (x) => SortAsc;
            }
            else if (value > TestValueZero && admin == True && sort.Equals(SortDsc))
            {
                KM.Platform.Fakes.ShimUser.IsAdministratorUser = (_) => True;
                GetField<HiddenField>(BrandId).Value = TestOne;
                mdList = new List<MDFilter>();
                var mdFilter = new MDFilter()
                {
                    FilterCategoryID = Id,
                    BrandName = Filter,
                    CreatedUserID = Id,
                    Name = Filter
                };
                mdList.Add(mdFilter);
                ShimMDFilter.GetByBrandIDClientConnectionsInt32 = (x, y) => mdList;
                ShimListControl.AllInstances.SelectedValueGet = (x) => Equal;
                ShimFilterList.AllInstances.SortFieldGet = (x) => BrandName;
                ShimFilterList.AllInstances.SortDirectionGet = (x) => SortDsc;
            }
            else if (value > TestValueZero && admin == False && sort.Equals(SortAsc))
            {
                KM.Platform.Fakes.ShimUser.IsAdministratorUser = (_) => False;
                GetField<HiddenField>(BrandId).Value = TestOne;
                mdList = new List<MDFilter>();
                var mdFilter = new MDFilter()
                {
                    FilterCategoryID = Id,
                    BrandName = Filter,
                    CreatedUserID = Id,
                    Name = Filter
                };
                mdList.Add(mdFilter);
                ShimMDFilter.GetByBrandIDUserIDClientConnectionsInt32Int32 = (x, y, z) => mdList;
                ShimListControl.AllInstances.SelectedValueGet = (x) => StartWith;
                ShimFilterList.AllInstances.SortFieldGet = (x) => Name;
                ShimFilterList.AllInstances.SortDirectionGet = (x) => SortAsc;
            }
            else if (value > TestValueZero && admin == False && sort.Equals(SortDsc))
            {
                KM.Platform.Fakes.ShimUser.IsAdministratorUser = (_) => False;
                GetField<HiddenField>(BrandId).Value = TestOne;
                mdList = new List<MDFilter>();
                var mdFilter = new MDFilter()
                {
                    FilterCategoryID = Id,
                    BrandName = Filter,
                    CreatedUserID = Id,
                    Name = Filter
                };
                mdList.Add(mdFilter);
                ShimMDFilter.GetByBrandIDUserIDClientConnectionsInt32Int32 = (x, y, z) => mdList;
                ShimListControl.AllInstances.SelectedValueGet = (x) => StartWith;
                ShimFilterList.AllInstances.SortFieldGet = (x) => Name;
                ShimFilterList.AllInstances.SortDirectionGet = (x) => SortDsc;
            }
            else if (value == TestNegativeValueOne && admin == True && sort.Equals(SortAsc))
            {
                KM.Platform.Fakes.ShimUser.IsAdministratorUser = (_) => True;
                GetField<HiddenField>(BrandId).Value = TestNegative;
                mdList = new List<MDFilter>();
                var mdFilter = new MDFilter()
                {
                    FilterCategoryID = Id,
                    BrandName = Filter,
                    CreatedUserID = Id,
                    Name = Filter,
                    FilterType = Filter
                };
                mdList.Add(mdFilter);
                ShimMDFilter.GetInBrandByUserIDClientConnectionsInt32 = (x, y) => mdList;
                ShimListControl.AllInstances.SelectedValueGet = (x) => EndWith;
                ShimFilterList.AllInstances.SortFieldGet = (x) => FilterType;
                ShimFilterList.AllInstances.SortDirectionGet = (x) => SortAsc;
            }
            else if (value == TestNegativeValueOne && admin == True && sort.Equals(SortDsc))
            {
                KM.Platform.Fakes.ShimUser.IsAdministratorUser = (_) => True;
                GetField<HiddenField>(BrandId).Value = TestNegative;
                mdList = new List<MDFilter>();
                var mdFilter = new MDFilter()
                {
                    FilterCategoryID = Id,
                    CreatedUserID = Id,
                    Name = Filter,
                    FilterType = Filter
                };
                mdList.Add(mdFilter);
                ShimMDFilter.GetInBrandByUserIDClientConnectionsInt32 = (x, y) => mdList;
                ShimListControl.AllInstances.SelectedValueGet = (x) => EndWith;
                ShimFilterList.AllInstances.SortFieldGet = (x) => FilterType;
                ShimFilterList.AllInstances.SortDirectionGet = (x) => SortDsc;
            }
            else if (value == TestValueZero && admin == True && sort.Equals(SortAsc))
            {
                KM.Platform.Fakes.ShimUser.IsAdministratorUser = (_) => True;
                GetField<HiddenField>(BrandId).Value = TestZero;
                mdList = new List<MDFilter>();
                var mdFilter = new MDFilter()
                {
                    FilterCategoryID = Id,
                    CreatedUserID = Id,
                    Name = Filter,
                    PubName = Filter
                };
                mdList.Add(mdFilter);
                ShimMDFilter.GetNotInBrandClientConnections = (x) => mdList;
                ShimListControl.AllInstances.SelectedValueGet = (x) => Contains;
                ShimFilterList.AllInstances.SortFieldGet = (x) => PubName;
                ShimFilterList.AllInstances.SortDirectionGet = (x) => SortAsc;
            }
            else if (value == TestValueZero && admin == True && sort.Equals(SortDsc))
            {
                KM.Platform.Fakes.ShimUser.IsAdministratorUser = (_) => True;
                GetField<HiddenField>(BrandId).Value = TestZero;
                mdList = new List<MDFilter>();
                var mdFilter = new MDFilter()
                {
                    FilterCategoryID = Id,
                    CreatedUserID = Id,
                    Name = Filter,
                    PubName = Filter
                };
                mdList.Add(mdFilter);
                ShimMDFilter.GetNotInBrandClientConnections = (x) => mdList;
                ShimListControl.AllInstances.SelectedValueGet = (x) => Contains;
                ShimFilterList.AllInstances.SortFieldGet = (x) => PubName;
                ShimFilterList.AllInstances.SortDirectionGet = (x) => SortDsc;
            }
            else if (value == TestValueZero && admin == False && sort.Equals(SortAsc))
            {
                KM.Platform.Fakes.ShimUser.IsAdministratorUser = (_) => False;
                GetField<HiddenField>(BrandId).Value = TestZero;
                mdList = new List<MDFilter>();
                var mdFilter = new MDFilter()
                {
                    FilterCategoryID = Id,
                    CreatedUserID = Id,
                    Name = Filter,
                    PubName = Filter
                };
                mdList.Add(mdFilter);
                ShimMDFilter.GetNotInBrandByUserIDClientConnectionsInt32 = (x, y) => mdList;
                ShimListControl.AllInstances.SelectedValueGet = (x) => Contains;
                ShimFilterList.AllInstances.SortFieldGet = (x) => PubName;
                ShimFilterList.AllInstances.SortDirectionGet = (x) => SortAsc;
            }
            else if (value == TestValueZero && admin == False && sort.Equals(SortDsc))
            {
                KM.Platform.Fakes.ShimUser.IsAdministratorUser = (_) => False;
                GetField<HiddenField>(BrandId).Value = TestZero;
                mdList = new List<MDFilter>();
                var mdFilter = new MDFilter()
                {
                    FilterCategoryID = Id,
                    CreatedUserID = Id,
                    Name = Filter,
                    PubName = Filter
                };
                mdList.Add(mdFilter);
                ShimMDFilter.GetNotInBrandByUserIDClientConnectionsInt32 = (x, y) => mdList;
                ShimListControl.AllInstances.SelectedValueGet = (x) => Contains;
                ShimFilterList.AllInstances.SortFieldGet = (x) => PubName;
                ShimFilterList.AllInstances.SortDirectionGet = (x) => SortDsc;
            }
            else if (value == TestNegativeTwo && admin == False && sort.Equals(SortAsc))
            {
                KM.Platform.Fakes.ShimUser.IsAdministratorUser = (_) => True;
                GetField<HiddenField>(BrandId).Value = TestNegative;
                mdList = new List<MDFilter>();
                var mdFilter = new MDFilter()
                {
                    FilterCategoryID = Id,
                    BrandName = Filter,
                    CreatedUserID = Id,
                    Name = Filter,
                    FilterCategoryName = Filter
                };
                mdList.Add(mdFilter);
                ShimMDFilter.GetInBrandByUserIDClientConnectionsInt32 = (x, y) => mdList;
                ShimListControl.AllInstances.SelectedValueGet = (x) => EndWith;
                ShimFilterList.AllInstances.SortFieldGet = (x) => FilterCategoryName;
                ShimFilterList.AllInstances.SortDirectionGet = (x) => SortAsc;
            }
            else if (value == TestNegativeTwo && admin == False && sort.Equals(SortDsc))
            {
                KM.Platform.Fakes.ShimUser.IsAdministratorUser = (_) => True;
                GetField<HiddenField>(BrandId).Value = TestNegative;
                mdList = new List<MDFilter>();
                var mdFilter = new MDFilter()
                {
                    FilterCategoryID = Id,
                    CreatedUserID = Id,
                    Name = Filter,
                    FilterCategoryName = Filter
                };
                mdList.Add(mdFilter);
                ShimMDFilter.GetInBrandByUserIDClientConnectionsInt32 = (x, y) => mdList;
                ShimListControl.AllInstances.SelectedValueGet = (x) => EndWith;
                ShimFilterList.AllInstances.SortFieldGet = (x) => FilterCategoryName;
                ShimFilterList.AllInstances.SortDirectionGet = (x) => SortDsc;
            }
            else if (value == TestNegativeThree && admin == False && sort.Equals(SortAsc))
            {
                KM.Platform.Fakes.ShimUser.IsAdministratorUser = (_) => True;
                GetField<HiddenField>(BrandId).Value = TestNegative;
                mdList = new List<MDFilter>();
                var mdFilter = new MDFilter()
                {
                    FilterCategoryID = Id,
                    BrandName = Filter,
                    CreatedUserID = Id,
                    Name = Filter,
                    QuestionCategoryName = Filter
                };
                mdList.Add(mdFilter);
                ShimMDFilter.GetInBrandByUserIDClientConnectionsInt32 = (x, y) => mdList;
                ShimListControl.AllInstances.SelectedValueGet = (x) => EndWith;
                ShimFilterList.AllInstances.SortFieldGet = (x) => QuestionCategoryName;
                ShimFilterList.AllInstances.SortDirectionGet = (x) => SortAsc;
            }
            else if (value == TestNegativeThree && admin == False && sort.Equals(SortDsc))
            {
                KM.Platform.Fakes.ShimUser.IsAdministratorUser = (_) => True;
                GetField<HiddenField>(BrandId).Value = TestNegative;
                mdList = new List<MDFilter>();
                var mdFilter = new MDFilter()
                {
                    FilterCategoryID = Id,
                    CreatedUserID = Id,
                    Name = Filter,
                    QuestionCategoryName = Filter
                };
                mdList.Add(mdFilter);
                ShimMDFilter.GetInBrandByUserIDClientConnectionsInt32 = (x, y) => mdList;
                ShimListControl.AllInstances.SelectedValueGet = (x) => EndWith;
                ShimFilterList.AllInstances.SortFieldGet = (x) => QuestionCategoryName;
                ShimFilterList.AllInstances.SortDirectionGet = (x) => SortDsc;
            }
            else if (value == TestNegativeFour && admin == False && sort.Equals(SortAsc))
            {
                KM.Platform.Fakes.ShimUser.IsAdministratorUser = (_) => True;
                GetField<HiddenField>(BrandId).Value = TestNegative;
                mdList = new List<MDFilter>();
                var mdFilter = new MDFilter()
                {
                    FilterCategoryID = Id,
                    BrandName = Filter,
                    CreatedUserID = Id,
                    Name = Filter,
                    CreatedDate = new DateTime(2018, 03, 28)
                };
                mdList.Add(mdFilter);
                ShimMDFilter.GetInBrandByUserIDClientConnectionsInt32 = (x, y) => mdList;
                ShimListControl.AllInstances.SelectedValueGet = (x) => EndWith;
                ShimFilterList.AllInstances.SortFieldGet = (x) => CreateDate;
                ShimFilterList.AllInstances.SortDirectionGet = (x) => SortAsc;
            }
            else if (value == TestNegativeFour && admin == False && sort.Equals(SortDsc))
            {
                KM.Platform.Fakes.ShimUser.IsAdministratorUser = (_) => True;
                GetField<HiddenField>(BrandId).Value = TestNegative;
                mdList = new List<MDFilter>();
                var mdFilter = new MDFilter()
                {
                    FilterCategoryID = Id,
                    CreatedUserID = Id,
                    Name = Filter,
                    CreatedDate = new DateTime(2018, 03, 28)
                };
                mdList.Add(mdFilter);
                ShimMDFilter.GetInBrandByUserIDClientConnectionsInt32 = (x, y) => mdList;
                ShimListControl.AllInstances.SelectedValueGet = (x) => EndWith;
                ShimFilterList.AllInstances.SortFieldGet = (x) => CreateDate;
                ShimFilterList.AllInstances.SortDirectionGet = (x) => SortDsc;
            }
            else if (value == TestNegativeFive && admin == False && sort.Equals(SortAsc))
            {
                KM.Platform.Fakes.ShimUser.IsAdministratorUser = (_) => True;
                GetField<HiddenField>(BrandId).Value = TestNegative;
                mdList = new List<MDFilter>();
                var mdFilter = new MDFilter()
                {
                    FilterCategoryID = Id,
                    BrandName = Filter,
                    CreatedUserID = Id,
                    Name = Filter,
                    Notes = Filter
                };
                mdList.Add(mdFilter);
                ShimMDFilter.GetInBrandByUserIDClientConnectionsInt32 = (x, y) => mdList;
                ShimListControl.AllInstances.SelectedValueGet = (x) => EndWith;
                ShimFilterList.AllInstances.SortFieldGet = (x) => CreateName;
                ShimFilterList.AllInstances.SortDirectionGet = (x) => SortAsc;
            }
            else if (value == TestNegativeFive && admin == False && sort.Equals(SortDsc))
            {
                KM.Platform.Fakes.ShimUser.IsAdministratorUser = (_) => True;
                GetField<HiddenField>(BrandId).Value = TestNegative;
                mdList = new List<MDFilter>();
                var mdFilter = new MDFilter()
                {
                    FilterCategoryID = Id,
                    CreatedUserID = Id,
                    Name = Filter,
                    Notes = Filter
                };
                mdList.Add(mdFilter);
                ShimMDFilter.GetInBrandByUserIDClientConnectionsInt32 = (x, y) => mdList;
                ShimListControl.AllInstances.SelectedValueGet = (x) => EndWith;
                ShimFilterList.AllInstances.SortFieldGet = (x) => CreateName;
                ShimFilterList.AllInstances.SortDirectionGet = (x) => SortDsc;
            }
            else if (value == TestNegativeSix && admin == False && sort.Equals(SortAsc))
            {
                KM.Platform.Fakes.ShimUser.IsAdministratorUser = (_) => True;
                GetField<HiddenField>(BrandId).Value = TestNegative;
                mdList = new List<MDFilter>();
                var mdFilter = new MDFilter()
                {
                    FilterCategoryID = Id,
                    BrandName = Filter,
                    CreatedUserID = Id,
                    Name = Filter,
                    Notes = Filter
                };
                mdList.Add(mdFilter);
                ShimMDFilter.GetInBrandByUserIDClientConnectionsInt32 = (x, y) => mdList;
                ShimListControl.AllInstances.SelectedValueGet = (x) => EndWith;
                ShimFilterList.AllInstances.SortFieldGet = (x) => NotesName;
                ShimFilterList.AllInstances.SortDirectionGet = (x) => SortAsc;
            }
            else if (value == TestNegativeSix && admin == False && sort.Equals(SortDsc))
            {
                KM.Platform.Fakes.ShimUser.IsAdministratorUser = (_) => True;
                GetField<HiddenField>(BrandId).Value = TestNegative;
                mdList = new List<MDFilter>();
                var mdFilter = new MDFilter()
                {
                    FilterCategoryID = Id,
                    CreatedUserID = Id,
                    Name = Filter,
                    Notes = Filter
                };
                mdList.Add(mdFilter);
                ShimMDFilter.GetInBrandByUserIDClientConnectionsInt32 = (x, y) => mdList;
                ShimListControl.AllInstances.SelectedValueGet = (x) => EndWith;
                ShimFilterList.AllInstances.SortFieldGet = (x) => NotesName;
                ShimFilterList.AllInstances.SortDirectionGet = (x) => SortDsc;
            }
            GetField<TextBox>(TxtSearch).Text = Filter;
            var gridView = GetField<GridView>(GvFilters);
            ShimBaseDataBoundControl.AllInstances.DataSourceGet = (x) => gridView;
            ShimRadTreeView.AllInstances.SelectedValueGet = (x) => StringId;
            var user = new User() { UserID = Id };
            userList = new List<User>();
            userList.Add(user);
            ShimUser.AllInstances.SelectBoolean = (x, y) => userList;
            GetField<Panel>(PnlBrand).Visible = false;
            GetField<DropDownList>(DrpSearch).Visible = false;
            GetField<Label>(LblBrandName).Visible = false;
        }
    }
}
