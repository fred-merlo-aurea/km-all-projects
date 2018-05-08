using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.UI.HtmlControls;
using KM.Platform.Fakes;
using KMPlatform;
using KMPS.MD.Main;
using KMPS.MD.Main.Fakes;
using NUnit.Framework;
using Shouldly;
using Telerik.Web.UI;

namespace KMPS.MD.Tests.Main
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class QuestionCategoryTests : BaseCategoryPageTests
    {
        private const string FieldTreeList = "rtlQuestionCategory";
        private const string FieldQuestionCategoryId = "QuestionCategoryID";
        private const string FieldSortField = "SortField";
        private const string FieldSortDirection = "SortDirection";
        private const string MethodPageLoad = "Page_Load";
        private const string DivError = "divError";
        private const string DivPopupMessage = "divPopupMessage";

        private QuestionCategory _testEntity;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _testEntity = new QuestionCategory();
            InitializePage(_testEntity);
        }

        [Test]
        public void Page_Load_UserHasAccessAndListIsNotEmpty_BindsDataSource()
        {
            // Arrange
            var list = new List<Objects.QuestionCategory>
            {
                new Objects.QuestionCategory()
            };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (a, b, c, d) => true;
            ShimQuestionCategory.AllInstances.getData = _ => list;
            var treeList = (RadTreeList)PrivatePage.GetField(FieldTreeList);
            InitializeTreeList(treeList, FieldQuestionCategoryId);

            // Act
            PrivatePage.Invoke(MethodPageLoad, this, EventArgs.Empty);

            // Assert
            VerifyPageLoad(treeList, list, true);
        }

        [Test]
        public void Page_Load_UserHasNoEditAndDeleteAccessAndListIsEmpty_ColumnsInvisible()
        {
            // Arrange
            var list = new List<Objects.QuestionCategory>();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (a, b, c, access) => access == Enums.Access.View;
            ShimQuestionCategory.AllInstances.getData = _ => list;
            var treeList = (RadTreeList)PrivatePage.GetField(FieldTreeList);
            InitializeTreeList(treeList, FieldQuestionCategoryId);

            // Act
            PrivatePage.Invoke(MethodPageLoad, this, EventArgs.Empty);

            // Assert
            treeList.Columns.First().Visible.ShouldBeTrue();
            treeList.Columns.RemoveAt(0);
            VerifyPageLoad(treeList, list, false);
        }

        private void VerifyPageLoad(RadTreeList treeList, IEnumerable<Objects.QuestionCategory> dataSource, bool columnVisible)
        {
            if (treeList == null)
            {
                throw new ArgumentNullException(nameof(treeList));
            }

            MasterPage.Menu.ShouldBe("Filters");
            MasterPage.SubMenu.ShouldBe("Question Category");

            treeList.DataSource.ShouldBeSameAs(dataSource);
            treeList.Columns.ShouldAllBe(c => c.Visible == columnVisible);

            ((HtmlGenericControl)PrivatePage.GetField(DivError)).Visible.ShouldBeFalse();
            ((HtmlGenericControl)PrivatePage.GetField(DivPopupMessage)).Visible.ShouldBeFalse();

            ((string)PrivatePage.GetFieldOrProperty(FieldSortField)).ShouldBe("CategoryName");
            ((string)PrivatePage.GetFieldOrProperty(FieldSortDirection)).ShouldBe("ASC");
        }
    }
}
