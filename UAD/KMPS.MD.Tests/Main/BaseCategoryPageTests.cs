using System;
using System.Diagnostics.CodeAnalysis;
using Telerik.Web.UI;

namespace KMPS.MD.Tests.Main
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseCategoryPageTests : BasePageTests
    {
        private const string ParentId = "ParentID";
        private const string CategoryName = "CategoryName";

        protected void InitializeTreeList(RadTreeList treeList, string idField)
        {
            if (treeList == null)
            {
                throw new ArgumentNullException(nameof(treeList));
            }

            treeList.DataKeyNames = new[] { idField };
            treeList.ParentDataKeyNames = new[] { ParentId };

            var boundColumn = new TreeListBoundColumn();
            treeList.Columns.Add(boundColumn);
            boundColumn.DataField = idField;

            boundColumn = new TreeListBoundColumn();
            treeList.Columns.Add(boundColumn);
            boundColumn.DataField = CategoryName;

            boundColumn = new TreeListBoundColumn();
            treeList.Columns.Add(boundColumn);
            boundColumn.DataField = CategoryName;

            boundColumn = new TreeListBoundColumn();
            treeList.Columns.Add(boundColumn);
            boundColumn.DataField = ParentId;
        }
    }
}
