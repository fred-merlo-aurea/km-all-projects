using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.UI.HtmlControls;
using KM.Platform.Fakes;
using KMPlatform;
using KMPS.MD.Main;
using KMPS.MD.Main.Fakes;
using KMPS.MD.MasterPages;
using NUnit.Framework;
using Shouldly;
using Telerik.Web.UI;

namespace KMPS.MD.Tests.Main
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FilterCategoryTests : BaseCategoryPageTests
    {
        private const string FieldTreeList = "rtlFilterCategory";
        private const string FieldFilterCategoryId = "FilterCategoryID";
        private const string FieldSortField = "SortField";
        private const string FieldSortDirection = "SortDirection";
        private const string MethodPageLoad = "Page_Load";
        private const string DivError = "divError";
        private const string DivPopupMessage = "divPopupMessage";

        private FilterCategory _testEntity;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _testEntity = new FilterCategory();
            InitializePage(_testEntity);
        }

        [Test]
        public void Page_Load_UserHasAccessAndListIsNotEmpty_BindsDataSource()
        {
            // Arrange
            var list = new List<Objects.FilterCategory>
            {
                new Objects.FilterCategory()
            };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (a, b, c, d) => true;
            ShimFilterCategory.AllInstances.getData = _ => list;
            var treeList = (RadTreeList)PrivatePage.GetField(FieldTreeList);
            InitializeTreeList(treeList, FieldFilterCategoryId);

            // Act
            PrivatePage.Invoke(MethodPageLoad, this, EventArgs.Empty);

            // Assert
            VerifyPageLoad(treeList, list, true);
        }

        [Test]
        public void Page_Load_UserHasNoEditAndDeleteAccessAndListIsEmpty_ColumnsInvisible()
        {
            // Arrange
            var list = new List<Objects.FilterCategory>();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (a, b, c, access) => access == Enums.Access.View;
            ShimFilterCategory.AllInstances.getData = _ => list;
            var treeList = (RadTreeList)PrivatePage.GetField(FieldTreeList);
            InitializeTreeList(treeList, FieldFilterCategoryId);

            // Act
            PrivatePage.Invoke(MethodPageLoad, this, EventArgs.Empty);

            // Assert
            treeList.Columns.First().Visible.ShouldBeTrue();
            treeList.Columns.RemoveAt(0);
            VerifyPageLoad(treeList, list, false);
        }

        private void VerifyPageLoad(RadTreeList treeList, IEnumerable<Objects.FilterCategory> dataSource, bool columnVisible)
        {
            if (treeList == null)
            {
                throw new ArgumentNullException(nameof(treeList));
            }

            MasterPage.Menu.ShouldBe("Filters");
            MasterPage.SubMenu.ShouldBe("Filter Category");

            treeList.DataSource.ShouldBeSameAs(dataSource);
            treeList.Columns.ShouldAllBe(c => c.Visible == columnVisible);

            ((HtmlGenericControl)PrivatePage.GetField(DivError)).Visible.ShouldBeFalse();
            ((HtmlGenericControl)PrivatePage.GetField(DivPopupMessage)).Visible.ShouldBeFalse();

            ((string)PrivatePage.GetFieldOrProperty(FieldSortField)).ShouldBe("CategoryName");
            ((string)PrivatePage.GetFieldOrProperty(FieldSortDirection)).ShouldBe("ASC");
        }
    }
}
