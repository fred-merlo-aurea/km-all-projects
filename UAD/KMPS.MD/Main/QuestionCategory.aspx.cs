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
    public partial class QuestionCategory : WebPageBase
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
            Master.SubMenu = "Question Category";

            lblErrorMessage.Text = string.Empty;
            divError.Visible = false;
            lblPopupMessage.Text = string.Empty;
            divPopupMessage.Visible = false;

            if (!IsPostBack)
            {
                RedirectIfNoViewAccess(Master.UserSession.CurrentUser, FeatureEnums.QuestionCategory);

                SortField = "CategoryName";
                SortDirection = "ASC";

                rtlQuestionCategory.BindCategories(Master.UserSession.CurrentUser, FeatureEnums.QuestionCategory, getData());
            }
        }

        public void DisplayError(string errorMessage)
        {
            lblErrorMessage.Text = errorMessage;
            divError.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (KMPS.MD.Objects.QuestionCategory.ExistsByCategoryName(Master.clientconnections, Convert.ToInt32(hfQuestionCategoryID.Value), txtCategoryName.Text))
                {
                    lblPopupMessage.Text = "Category Name already exists. Please enter different name.";
                    divPopupMessage.Visible = true;
                    mdlPopSaveQuestionCategory.Show();
                    return;
                }

                KMPS.MD.Objects.QuestionCategory qc = new KMPS.MD.Objects.QuestionCategory();
                qc.QuestionCategoryID = Convert.ToInt32(hfQuestionCategoryID.Value);
                qc.CategoryName = txtCategoryName.Text;
                qc.ParentID = Convert.ToInt32(Convert.ToInt32(hfParentQuestionCategoryID.Value));

                if (qc.QuestionCategoryID > 0)
                    qc.UpdatedUserID = Master.LoggedInUser;
                else
                    qc.CreatedUserID = Master.LoggedInUser;

                KMPS.MD.Objects.QuestionCategory.Save(Master.clientconnections, qc);

                ResetControls();

                List<KMPS.MD.Objects.QuestionCategory> lst = getData();

                rtlQuestionCategory.DataSource = lst;
                rtlQuestionCategory.DataBind();
                rtlQuestionCategory.ExpandAllItems();

                if ((lst != null) && (!lst.Any()))
                {
                    if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.QuestionCategory, KMPlatform.Enums.Access.Edit))
                    {
                        rtlQuestionCategory.Columns[1].Visible = false;
                        rtlQuestionCategory.Columns[2].Visible = false;
                    }

                    if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.QuestionCategory, KMPlatform.Enums.Access.Delete))
                    {
                        rtlQuestionCategory.Columns[3].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ResetControls();
        }
        protected void ResetControls()
        {
            txtCategoryName.Text = string.Empty;
            hfQuestionCategoryID.Value = "0";
            hfParentQuestionCategoryID.Value = "0";
        }

        protected List<KMPS.MD.Objects.QuestionCategory> getData()
        {
            List<KMPS.MD.Objects.QuestionCategory> lst = new List<KMPS.MD.Objects.QuestionCategory>();

            try
            {
                List<KMPS.MD.Objects.QuestionCategory> fc = KMPS.MD.Objects.QuestionCategory.GetAll(Master.clientconnections);

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
            hfQuestionCategoryID.Value = "0";
            hfParentQuestionCategoryID.Value = e.CommandArgument.ToString();
            mdlPopSaveQuestionCategory.Show();
        }

        protected void lnkEdit_Command(object sender, CommandEventArgs e)
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.QuestionCategory, KMPlatform.Enums.Access.Edit))
            {
                ResetControls();

                try
                {
                    KMPS.MD.Objects.QuestionCategory qc = KMPS.MD.Objects.QuestionCategory.GetByID(Master.clientconnections, Convert.ToInt32(e.CommandArgument));

                    hfQuestionCategoryID.Value = qc.QuestionCategoryID.ToString();
                    hfParentQuestionCategoryID.Value = qc.ParentID.ToString();
                    txtCategoryName.Text = qc.CategoryName;
                    mdlPopSaveQuestionCategory.Show();
                }
                catch (Exception ex)
                {
                    DisplayError(ex.Message);
                }
            }
        }

        protected void lnkDelete_Command(object sender, CommandEventArgs e)
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.QuestionCategory, KMPlatform.Enums.Access.Delete))
            {
                try
                {
                    if (MDFilter.ExistsByQuestionCategoryID(Master.clientconnections, Convert.ToInt32(e.CommandArgument)))
                    {
                        DisplayError("This cannot be deleted because this Question Category is associated with a filter.");
                        return;
                    }

                    if (KMPS.MD.Objects.QuestionCategory.ExistsByParentID(Master.clientconnections, Convert.ToInt32(e.CommandArgument)))
                    {
                        DisplayError("This cannot be deleted because this Question Category is associated with other Question Category.");
                        return;
                    }

                    KMPS.MD.Objects.QuestionCategory.Delete(Master.clientconnections, Convert.ToInt32(e.CommandArgument.ToString()), Master.LoggedInUser);
                }
                catch (Exception ex)
                {
                    DisplayError(ex.Message);
                }

                List<KMPS.MD.Objects.QuestionCategory> lst = getData();

                rtlQuestionCategory.DataSource = lst;
                rtlQuestionCategory.DataBind();
                rtlQuestionCategory.ExpandAllItems();

                if ((lst != null) && (!lst.Any()))
                {
                    if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.QuestionCategory, KMPlatform.Enums.Access.Edit))
                    {
                        rtlQuestionCategory.Columns[1].Visible = false;
                        rtlQuestionCategory.Columns[2].Visible = false;
                    }

                    if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.QuestionCategory, KMPlatform.Enums.Access.Delete))
                    {
                        rtlQuestionCategory.Columns[3].Visible = false;
                    }
                }
            }
        }

        protected void rtlQuestionCategory_NeedDataSource(object source, TreeListNeedDataSourceEventArgs e)
        {
            rtlQuestionCategory.DataSource = getData();

            if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.QuestionCategory, KMPlatform.Enums.Access.Edit))
            {
                rtlQuestionCategory.Columns[1].Visible = false;
                rtlQuestionCategory.Columns[2].Visible = false;
            }

            if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.QuestionCategory, KMPlatform.Enums.Access.Delete))
            {
                rtlQuestionCategory.Columns[3].Visible = false;
            }
        }

        protected void rtlQuestionCategory_ItemDataBound(object sender, Telerik.Web.UI.TreeListItemDataBoundEventArgs e)
        {
            if (e.Item is TreeListDataItem)
            {
            }
            else if(e.Item is TreeListHeaderItem)
            {
                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.QuestionCategory, KMPlatform.Enums.Access.Edit))
                {
                    var lnkAddRoot = (LinkButton)e.Item.FindControl("lnkAddRoot");
                    lnkAddRoot.Visible = false;
                }
            }
        }

        protected void lnkExpand_click(object sender, EventArgs e)
        {
            rtlQuestionCategory.ExpandAllItems();
        }

        protected void lnkCollapse_click(object sender, EventArgs e)
        {
            rtlQuestionCategory.CollapseAllItems();
        }
    }
}