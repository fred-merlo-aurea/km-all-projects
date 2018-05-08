using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KMPS.MD.Objects;
using Telerik.Web.UI;
using KMPS.MD.Extensions;
using FeatureEnums = KMPlatform.Enums.ServiceFeatures;

namespace KMPS.MD.Main
{
    public partial class FilterCategory : WebPageBase
    {
        private string SortField
        {
            get
            {
                return ViewState["SortField"].ToString();
            }
            set
            {
                ViewState["SortField"] = value;
            }
        }

        private string SortDirection
        {
            get
            {
                return ViewState["SortDirection"].ToString();
            }
            set
            {
                ViewState["SortDirection"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Filters";
            Master.SubMenu = "Filter Category";
            lblErrorMessage.Text = string.Empty;
            divError.Visible = false;
            lblPopupMessage.Text = string.Empty;
            divPopupMessage.Visible = false;

            if (!IsPostBack)
            {
                RedirectIfNoViewAccess(Master.UserSession.CurrentUser, FeatureEnums.FilterCategory);

                SortField = "CategoryName";
                SortDirection = "ASC";

                rtlFilterCategory.BindCategories(Master.UserSession.CurrentUser, FeatureEnums.FilterCategory, getData());
            }
        }

        protected void DisplayError(string errorMessage)
        {
            lblErrorMessage.Text = errorMessage;
            divError.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FilterCategory, KMPlatform.Enums.Access.Edit))
            {
                try
                {
                    if (KMPS.MD.Objects.FilterCategory.ExistsByCategoryName(Master.clientconnections, Convert.ToInt32(hfFilterCategoryID.Value), txtCategoryName.Text))
                    {
                        lblPopupMessage.Text = "Category Name already exists. Please enter different name.";
                        divPopupMessage.Visible = true;
                        mdlPopSaveFilterCategory.Show();
                        return;
                    }

                    KMPS.MD.Objects.FilterCategory fc = new KMPS.MD.Objects.FilterCategory();
                    fc.FilterCategoryID = Convert.ToInt32(hfFilterCategoryID.Value);
                    fc.CategoryName = txtCategoryName.Text;
                    fc.ParentID = Convert.ToInt32(hfParentFilterCategoryID.Value);

                    if (fc.FilterCategoryID > 0)
                        fc.UpdatedUserID = Master.LoggedInUser;
                    else
                        fc.CreatedUserID = Master.LoggedInUser;

                    KMPS.MD.Objects.FilterCategory.Save(Master.clientconnections, fc);

                    ResetControls();

                    List<KMPS.MD.Objects.FilterCategory> lst = getData();

                    rtlFilterCategory.DataSource = lst;
                    rtlFilterCategory.DataBind();
                    rtlFilterCategory.ExpandAllItems();

                    if ((lst != null) && (!lst.Any()))
                    {
                        if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FilterCategory, KMPlatform.Enums.Access.Edit))
                        {
                            rtlFilterCategory.Columns[1].Visible = false;
                            rtlFilterCategory.Columns[2].Visible = false;
                        }

                        if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FilterCategory, KMPlatform.Enums.Access.Delete))
                        {
                            rtlFilterCategory.Columns[3].Visible = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    DisplayError(ex.Message);
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ResetControls();
        }

        protected void ResetControls()
        {
            txtCategoryName.Text = string.Empty;
            hfFilterCategoryID.Value = "0";
            hfParentFilterCategoryID.Value = "0";
        }

        protected List<KMPS.MD.Objects.FilterCategory> getData()
        {
            List<KMPS.MD.Objects.FilterCategory> lst = new List<KMPS.MD.Objects.FilterCategory>();

            try
            {
                List<KMPS.MD.Objects.FilterCategory> fc = KMPS.MD.Objects.FilterCategory.GetAll(Master.clientconnections);

                if (fc != null && fc.Count > 0)
                {
                    switch (SortField.ToUpper())
                    {
                        case "CATEGORYNAME":
                            if (SortDirection.ToUpper() == "ASC")
                                lst = fc.OrderBy(o => o.CategoryName).ToList();
                            else
                                lst = fc.OrderByDescending(o => o.CategoryName).ToList();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }

            return lst;
        }

        protected void lnkAdd_Command(object sender, CommandEventArgs e)
        {
            hfFilterCategoryID.Value = "0";
            hfParentFilterCategoryID.Value = e.CommandArgument.ToString();
            mdlPopSaveFilterCategory.Show();
        }

        protected void lnkEdit_Command(object sender, CommandEventArgs e)
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FilterCategory, KMPlatform.Enums.Access.View))
            {
                try
                {
                    ResetControls();

                    KMPS.MD.Objects.FilterCategory fc = KMPS.MD.Objects.FilterCategory.GetByID(Master.clientconnections, Convert.ToInt32(e.CommandArgument));
                    hfFilterCategoryID.Value = fc.FilterCategoryID.ToString();
                    hfParentFilterCategoryID.Value = fc.ParentID.ToString();
                    txtCategoryName.Text = fc.CategoryName;

                    mdlPopSaveFilterCategory.Show();
                }
                catch (Exception ex)
                {
                    DisplayError(ex.Message);
                }
            }
        }

        protected void lnkDelete_Command(object sender, CommandEventArgs e)
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FilterCategory, KMPlatform.Enums.Access.Delete))
            {
                try
                {
                    if (MDFilter.ExistsByFilterCategoryID(Master.clientconnections, Convert.ToInt32(e.CommandArgument)))
                    {
                        DisplayError("This cannot be deleted because this Filter Category is associated with a filter.");
                        return;
                    }

                    if (KMPS.MD.Objects.FilterCategory.ExistsByParentID(Master.clientconnections, Convert.ToInt32(e.CommandArgument)))
                    {
                        DisplayError("This cannot be deleted because this Filter Category is associated with other Filter Category.");
                        return;
                    }

                    KMPS.MD.Objects.FilterCategory.Delete(Master.clientconnections, Convert.ToInt32(e.CommandArgument.ToString()), Master.LoggedInUser);
                }
                catch (Exception ex)
                {
                    DisplayError(ex.Message);
                }

                List<KMPS.MD.Objects.FilterCategory> lst = getData();

                rtlFilterCategory.DataSource = lst;
                rtlFilterCategory.DataBind();
                rtlFilterCategory.ExpandAllItems();

                if ((lst != null) && (!lst.Any()))
                {
                    if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FilterCategory, KMPlatform.Enums.Access.Edit))
                    {
                        rtlFilterCategory.Columns[1].Visible = false;
                        rtlFilterCategory.Columns[2].Visible = false;
                    }

                    if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FilterCategory, KMPlatform.Enums.Access.Delete))
                    {
                        rtlFilterCategory.Columns[3].Visible = false;
                    }
                }
            }
        }

        protected void rtlFilterCategory_ItemCommand(object sender, TreeListCommandEventArgs e)
        {
            if (e.CommandName == RadTreeList.ExpandCollapseCommandName)
            {
                getData();
            }
        }

        protected void rtlFilterCategory_PageIndexChanged(object source, Telerik.Web.UI.TreeListPageChangedEventArgs e)
        {
            rtlFilterCategory.CurrentPageIndex = e.NewPageIndex;
            getData();
        }

        protected void rtlFilterCategory_PageSizeChanged(object source, Telerik.Web.UI.TreeListPageSizeChangedEventArgs e)
        {
            getData();
        }

        protected void rtlFilterCategory_NeedDataSource(object source, TreeListNeedDataSourceEventArgs e)
        {
            rtlFilterCategory.DataSource = getData();

            if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FilterCategory, KMPlatform.Enums.Access.Edit))
            {
                rtlFilterCategory.Columns[1].Visible = false;
                rtlFilterCategory.Columns[2].Visible = false;
            }

            if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FilterCategory, KMPlatform.Enums.Access.Delete))
            {
                rtlFilterCategory.Columns[3].Visible = false;
            }
        }

        protected void rtlFilterCategory_ItemDataBound(object sender, Telerik.Web.UI.TreeListItemDataBoundEventArgs e)
        {
            if (e.Item is TreeListDataItem)
            {
            }
            else if (e.Item is TreeListHeaderItem)
            {
                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FilterCategory, KMPlatform.Enums.Access.Edit))
                {
                    var lnkAddRoot = (LinkButton)e.Item.FindControl("lnkAddRoot");
                    lnkAddRoot.Visible = false;
                }
            }
        }

        protected void lnkExpand_click(object sender, EventArgs e)
        {
            rtlFilterCategory.ExpandAllItems();
        }

        protected void lnkCollapse_click(object sender, EventArgs e)
        {
            rtlFilterCategory.CollapseAllItems();
        }
    }
}