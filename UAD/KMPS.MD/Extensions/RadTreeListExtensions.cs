using System;
using System.Collections.Generic;
using System.Linq;
using KMPlatform;
using Telerik.Web.UI;
using EntityUser = KMPlatform.Entity.User;
using PlatformUser = KM.Platform.User;

namespace KMPS.MD.Extensions
{
    public static class RadTreeListExtensions
    {
        private const int MinCountForEdit = 3;
        private const int MinCountForDelete = 4;

        public static void BindCategories(
            this RadTreeList treeList,
            EntityUser currentUser,
            Enums.ServiceFeatures feature,
            IEnumerable<object> dataSource)
        {
            if (treeList == null)
            {
                throw new ArgumentNullException(nameof(treeList));
            }

            if (currentUser == null)
            {
                throw new ArgumentNullException(nameof(currentUser));
            }

            treeList.DataSource = dataSource;
            treeList.DataBind();
            treeList.ExpandAllItems();

            if (dataSource != null && !dataSource.Any())
            {
                if (!PlatformUser.HasAccess(currentUser, Enums.Services.UAD, feature, Enums.Access.Edit)
                    && treeList.Columns.Count >= MinCountForEdit)
                {
                    treeList.Columns[1].Visible = false;
                    treeList.Columns[2].Visible = false;
                }

                if (!PlatformUser.HasAccess(currentUser, Enums.Services.UAD, feature, Enums.Access.Delete)
                    && treeList.Columns.Count >= MinCountForDelete)
                {
                    treeList.Columns[3].Visible = false;
                }
            }
        }
    }
}