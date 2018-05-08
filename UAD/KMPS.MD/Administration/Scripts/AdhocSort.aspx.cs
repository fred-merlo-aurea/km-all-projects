using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using KMPS.MD.Objects;

namespace KMPS.MD.Administration
{
    public partial class AdhocSort : KMPS.MD.Main.WebPageHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Adhoc";
            Master.SubMenu = "Adhoc SetUp";
            divError.Visible = false;
            lblErrorMessage.Text = "";
            lblMessage.Visible = false;
            lblMessage.Text = "";

            if (!Page.User.IsInRole("admin"))
            {
                Response.Redirect("../Default.aspx");
            }

            if (!IsPostBack)
            {
                List<AdhocCategory> ac = AdhocCategory.Get();
                gvCategory.DataSource = ac;
                gvCategory.DataBind();

                for (int i = 1; i <= ac.Count() + 1; i++)
                {
                    drpSortingOrder.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }


                lstSourceFields.DataSource = Adhoc.GetByCategoryID(0,0,0);
                lstSourceFields.DataBind();
            }
        }

        protected void gvCategory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                drpSortingOrder.Items.Clear();

                int CategoryID = Convert.ToInt32(gvCategory.DataKeys[Convert.ToInt32(e.CommandArgument)].Value);
                lblCategoryID.Text = CategoryID.ToString();

                try
                {
                    List<AdhocCategory> ladhoccategory = AdhocCategory.Get();

                    foreach (AdhocCategory lac in ladhoccategory)
                    {
                        drpSortingOrder.Items.Add(lac.SortOrder.ToString());
                    }

                    AdhocCategory ac = ladhoccategory.Find(x => x.CategoryID == CategoryID);
                    txtCategoryName.Text = ac.CategoryName;
                    drpSortingOrder.Items.FindByValue(ac.SortOrder.ToString()).Selected = true;

                    btnSave.Text = "UPDATE";

                    lstSourceFields.DataSource = Adhoc.GetByCategoryID(0,0,0);
                    lstSourceFields.DataBind();

                    lstDestFields.DataSource = Adhoc.GetByCategoryID(CategoryID,0,0);
                    lstDestFields.DataBind();

                    lblpnlHeader.Text = "Edit Adhoc";
                }
                catch (Exception ex)
                {
                    divError.Visible = true;
                    lblErrorMessage.Text = ex.ToString();
                }
            }
        }

        protected void btnAdd_Click(Object sender, EventArgs e)
        {
            for (int i = 0; i < lstSourceFields.Items.Count; i++)
            {
                if (lstSourceFields.Items[i].Selected)
                {
                    lstDestFields.Items.Add(lstSourceFields.Items[i]);
                    lstSourceFields.Items.RemoveAt(i);
                    i--;
                }
            }
        }


        protected void btnRemove_Click(Object sender, EventArgs e)
        {
            for (int i = 0; i < lstDestFields.Items.Count; i++)
            {
                if (lstDestFields.Items[i].Selected)
                {
                    lstSourceFields.Items.Add(lstDestFields.Items[i]);
                    lstDestFields.Items.RemoveAt(i);
                    i--;
                }
            }
        }

        protected void btnUp_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstDestFields.Items.Count; i++)
            {
                if (lstDestFields.Items[i].Selected)
                {
                    if (i > 0 && !lstDestFields.Items[i - 1].Selected)
                    {
                        ListItem bottom = lstDestFields.Items[i];
                        lstDestFields.Items.Remove(bottom);
                        lstDestFields.Items.Insert(i - 1, bottom);
                        lstDestFields.Items[i - 1].Selected = true;
                    }
                }
            }
        }

        protected void btndown_Click(object sender, EventArgs e)
        {
            int startindex = lstDestFields.Items.Count - 1;

            for (int i = startindex; i > -1; i--)
            {
                if (lstDestFields.Items[i].Selected)
                {
                    if (i < startindex && !lstDestFields.Items[i + 1].Selected)
                    {
                        ListItem bottom = lstDestFields.Items[i];
                        lstDestFields.Items.Remove(bottom);
                        lstDestFields.Items.Insert(i + 1, bottom);
                        lstDestFields.Items[i + 1].Selected = true;
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int count = 0;

                if (btnSave.Text.ToUpper() == "SAVE")
                {
                    if (AdhocCategory.Exists(txtCategoryName.Text))
                    {
                        divError.Visible = true;
                        lblErrorMessage.Text = "Category Name already exists";
                        return;
                    }
                }

                int CategoryID = AdhocCategory.Save(Convert.ToInt32(lblCategoryID.Text), txtCategoryName.Text, Convert.ToInt32(drpSortingOrder.SelectedItem.Value));

                Adhoc.Delete(CategoryID);

                foreach (ListItem li in lstDestFields.Items)
                {

                    Adhoc adhoc = new Adhoc();
                    adhoc.AdhocName = li.Text;
                    adhoc.CategoryID = CategoryID;
                    adhoc.SortOrder = count + 1;
                    Adhoc.Save(adhoc);

                    count++;
                }

            }
            catch (Exception ex)
            {
                divError.Visible = true;
                lblErrorMessage.Text = ex.ToString();
            }

            Response.Redirect("AdhocSort.aspx");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdhocSort.aspx");
        }

        protected void gvCategory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView grdAdhoc = (GridView)e.Row.FindControl("grdAdhoc");
                grdAdhoc.DataSource = Adhoc.GetByCategoryID(Convert.ToInt32(gvCategory.DataKeys[e.Row.RowIndex].Value.ToString()),0,0);
                grdAdhoc.DataBind();
            }
        }
    }
}