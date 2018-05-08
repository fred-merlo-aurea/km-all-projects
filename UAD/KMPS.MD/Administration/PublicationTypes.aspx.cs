using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Linq;
using KMPS.MD.Objects;

namespace KMPS.MDAdmin
{
    public partial class PublicationTypes : KMPS.MD.Main.WebPageHelper
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
            Master.Menu = "Products";
            Master.SubMenu = "Product Types";
            divError.Visible = false;
            lblErrorMessage.Text = "";
            SqlDataSourcePublicationTypeConnect.ConnectionString = DataFunctions.GetClientSqlConnection(Master.clientconnections).ConnectionString;
            //Trace.Write(DataFunctions.GetClientSqlConnection(Master.clientconnections));

            if (!IsPostBack)
            {
                gvPublicationTypes.PageSize = 10;
                SortField = "PubTypeDisplayName";
                SortDirection = "ASC";

                LoadGrid();
            }
        }

        public void DisplayError(string errorMessage)
        {
            lblErrorMessage.Text = errorMessage;
            divError.Visible = true;
        }

        protected void LoadGrid()
        {
            ddlSortingOrder.Items.Clear();

            List<KMPS.MD.Objects.PubTypes> pt = PubTypes.GetAll(Master.clientconnections);

            List<KMPS.MD.Objects.PubTypes> lst = null;

            if (pt != null && pt.Count > 0)
            {
                switch (SortField.ToUpper())
                {
                    case "PUBTYPEDISPLAYNAME":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = pt.OrderBy(o => o.PubTypeDisplayName).ToList();
                        else
                            lst = pt.OrderByDescending(o => o.PubTypeDisplayName).ToList();
                        break;

                    case "SORTORDER":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = pt.OrderBy(o => o.SortOrder).ToList();
                        else
                            lst = pt.OrderByDescending(o => o.SortOrder).ToList();
                        break;

                    case "ISACTIVE":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = pt.OrderBy(o => o.IsActive).ToList();
                        else
                            lst = pt.OrderByDescending(o => o.IsActive).ToList();
                        break;
                }
            }

            gvPublicationTypes.DataSource = lst;
            gvPublicationTypes.DataBind();

            for (int i = 1; i <= pt.Count + 1; i++)
            {
                ddlSortingOrder.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }

        protected void gvPublicationTypes_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression.ToString() == SortField)
            {
                SortDirection = (SortDirection.ToUpper() == "ASC" ? "DESC" : "ASC");
            }
            else
            {
                SortField = e.SortExpression;
                SortDirection = "ASC";
            }

            LoadGrid();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (PubTypes.ExistsByPubTypeDisplayName(Master.clientconnections, txtPubTypeDisplayName.Text, Convert.ToInt32(txtPubTypeID.Text)))
                {
                    DisplayError("Publication Type Already Exists");
                    return;
                }

                if (Convert.ToInt32(txtPubTypeID.Text) != 0)
                {
                    if (!Convert.ToBoolean(ddlActive.SelectedItem.Value))
                    {
                        if (Pubs.ExistsByPubTypeID(Master.clientconnections, Convert.ToInt32(txtPubTypeID.Text)))
                        {
                            DisplayError("Publication Type is associated with the product. Is Active cannot be updated.");
                            return;
                        }
                    }
                }

                PubTypes pubtypes = new PubTypes();
                pubtypes.PubTypeID = Convert.ToInt32(txtPubTypeID.Text);
                pubtypes.PubTypeDisplayName = txtPubTypeDisplayName.Text;
                pubtypes.ColumnReference = txtPubTypeDisplayName.Text;
                pubtypes.IsActive = Convert.ToBoolean(ddlActive.SelectedItem.Value);
                pubtypes.SortOrder = Convert.ToInt32(ddlSortingOrder.SelectedItem.Value);
                PubTypes.Save(Master.clientconnections, pubtypes);

                Response.Redirect("PublicationTypes.aspx");
            }
            catch (Exception ex)
            {
                DisplayError(ex.ToString());
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtPubTypeDisplayName.Text = string.Empty;
            ddlActive.SelectedIndex = 0;
            btnSave.Text = "SAVE";
            lblpnlHeader.Text = "Add Publication Type";
            txtPubTypeDisplayName.Text = string.Empty;
            ddlSortingOrder.SelectedIndex = 0;
        }

        protected void gvPublicationTypes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        protected void gvPublicationTypes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                try
                {
                    ddlSortingOrder.Items.Clear();

                    int pubTypeID = Convert.ToInt32(gvPublicationTypes.DataKeys[Convert.ToInt32(e.CommandArgument)].Value);

                    List<PubTypes> pubtypes = PubTypes.GetAll(Master.clientconnections);

                    foreach (PubTypes pt in pubtypes)
                    {
                        ddlSortingOrder.Items.Add(pt.SortOrder.ToString());
                    }

                    PubTypes pts = PubTypes.GetAll(Master.clientconnections).Find(x => x.PubTypeID == pubTypeID);

                    txtPubTypeID.Text = pts.PubTypeID.ToString();
                    txtPubTypeDisplayName.Text = pts.PubTypeDisplayName;
                    ddlActive.SelectedValue = pts.IsActive.ToString();
                    ddlSortingOrder.SelectedValue = pts.SortOrder.ToString();

                    btnSave.Text = "UPDATE";
                    lblpnlHeader.Text = "Edit Publication Type"; 
                }
                catch (Exception ex)
                {
                    DisplayError(ex.ToString());
                }
            }
            if (e.CommandName == "Delete")
            {
                try
                {
                    if (Pubs.ExistsByPubTypeID(Master.clientconnections, Convert.ToInt32(e.CommandArgument.ToString())))
                    {
                        DisplayError("Publication Type cannot be deleted. There is a pub associated with the Publication Type.");
                    }
                    else
                    {
                        PubTypes.Delete(Master.clientconnections, Convert.ToInt32(e.CommandArgument.ToString()));
                    }

                    LoadGrid();
                }
                catch (Exception ex)
                {
                    DisplayError(ex.Message);
                }
            }
        }

        protected void gvPublicationTypes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPublicationTypes.PageIndex = e.NewPageIndex;
            LoadGrid();
        }
    }
}