using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using KMPS.MD.Objects;

namespace KMPS.MD.Tools
{
    public partial class ReportSetup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Tools";
            Master.SubMenu = "CrossTab Report Setup";
            divError.Visible = false;
            lblErrorMessage.Text = "";

            if (!IsPostBack)
            {
                if (!KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                {
                    Response.Redirect("../SecurityAccessError.aspx");
                }

                List<Brand> b = Brand.GetAll(Master.clientconnections);

                if (b.Count > 0)
                {
                    pnlBrand.Visible = true;

                    ddlBrand.Visible = true;
                    ddlBrand.DataSource = b;
                    ddlBrand.DataBind();
                    ddlBrand.Items.Insert(0, new ListItem("All Products", "0"));
                    hfBrandID.Value = ddlBrand.SelectedItem.Value;
                    LoadCrossTabReportGrid();
                }
                else
                {
                    LoadCrossTabReportGrid();
                }

                LoadDimensions();
            }
        }

        public void DisplayError(string errorMessage)
        {
            lblErrorMessage.Text = errorMessage;
            divError.Visible = true;
        }

        protected void LoadCrossTabReportGrid()
        {
            rgCrossTabReport.DataSource = MD.Objects.CrossTabReport.GetByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
            rgCrossTabReport.DataBind();
        }


        protected void LoadProducts()
        {
            List<Pubs> lpubs = new List<Pubs>();

            if (Convert.ToInt32(hfBrandID.Value) > 0)
                lpubs = Pubs.GetActiveByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
            else
                lpubs = Pubs.GetActive(Master.clientconnections);

            ddlProduct.DataSource = lpubs;
            ddlProduct.DataBind();
            ddlProduct.Items.Insert(0, new ListItem("", "0"));
            hfPubID.Value = ddlProduct.SelectedValue;
        }

        protected void LoadDimensions()
        {
            ddlRowDimension.Items.Clear();
            ddlColumnDimension.Items.Clear();

            if (ddlViewType.SelectedValue == Enums.ViewType.ProductView.ToString())
            {
                if (Convert.ToInt32(hfPubID.Value) > 0)
                {
                    List<ResponseGroup> rg = ResponseGroup.GetActiveByPubID(Master.clientconnections, Convert.ToInt32(hfPubID.Value));

                    ddlRowDimension.DataTextField = "DisplayName";
                    ddlRowDimension.DataValueField = "ResponseGroupID";
                    ddlRowDimension.DataSource = rg;
                    ddlRowDimension.DataBind();

                    ddlColumnDimension.DataTextField = "DisplayName";
                    ddlColumnDimension.DataValueField = "ResponseGroupID";
                    ddlColumnDimension.DataSource = rg;
                    ddlColumnDimension.DataBind();

                    ddlRowDimension.Items.Insert(0, new ListItem("Title", "Title"));
                    ddlRowDimension.Items.Insert(1, new ListItem("Company", "Company"));
                    ddlRowDimension.Items.Insert(2, new ListItem("City", "City"));
                    ddlRowDimension.Items.Insert(3, new ListItem("State", "State"));
                    ddlRowDimension.Items.Insert(4, new ListItem("Country", "Country"));
                    ddlRowDimension.Items.Insert(5, new ListItem("Zip", "Zip"));
                    ddlColumnDimension.Items.Insert(0, new ListItem("Title", "Title"));
                    ddlColumnDimension.Items.Insert(1, new ListItem("Company", "Company"));
                    ddlColumnDimension.Items.Insert(2, new ListItem("City", "City"));
                    ddlColumnDimension.Items.Insert(3, new ListItem("State", "State"));
                    ddlColumnDimension.Items.Insert(4, new ListItem("Country", "Country"));
                    ddlColumnDimension.Items.Insert(5, new ListItem("Zip", "Zip"));

                }
            }
            else
            {
                List<MasterGroup> mg = new List<MasterGroup>();

                if (Convert.ToInt32(hfBrandID.Value) > 0)
                    mg = MasterGroup.GetSubReportEnabledByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
                else
                    mg = MasterGroup.GetSubReportEnabled(Master.clientconnections);

                ddlRowDimension.DataTextField = "DisplayName";
                ddlRowDimension.DataValueField = "MasterGroupID";
                ddlRowDimension.DataSource = mg;
                ddlRowDimension.DataBind();

                ddlColumnDimension.DataTextField = "DisplayName";
                ddlColumnDimension.DataValueField = "MasterGroupID";
                ddlColumnDimension.DataSource = mg;
                ddlColumnDimension.DataBind();
                ddlRowDimension.Items.Insert(0, new ListItem("Title", "Title"));
                ddlRowDimension.Items.Insert(1, new ListItem("Company", "Company"));
                ddlRowDimension.Items.Insert(2, new ListItem("City", "City"));
                ddlRowDimension.Items.Insert(3, new ListItem("State", "State"));
                ddlRowDimension.Items.Insert(4, new ListItem("Country", "Country"));
                ddlRowDimension.Items.Insert(5, new ListItem("Zip", "Zip"));
                ddlColumnDimension.Items.Insert(0, new ListItem("Title", "Title"));
                ddlColumnDimension.Items.Insert(1, new ListItem("Company", "Company"));
                ddlColumnDimension.Items.Insert(2, new ListItem("City", "City"));
                ddlColumnDimension.Items.Insert(3, new ListItem("State", "State"));
                ddlColumnDimension.Items.Insert(4, new ListItem("Country", "Country"));
                ddlColumnDimension.Items.Insert(5, new ListItem("Zip", "Zip"));
            }
        }

        protected void ddlBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            hfBrandID.Value = ddlBrand.SelectedValue;
            LoadCrossTabReportGrid();
            ResetControls();
        }

        protected void ddlViewType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlViewType.SelectedValue == Enums.ViewType.ProductView.ToString())
            {
                phProduct.Visible = true;
                LoadProducts();
                LoadDimensions();
            }
            else
            {
                phProduct.Visible = false;
                LoadDimensions();
            }
        }

        protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            hfPubID.Value = ddlProduct.SelectedValue;
            LoadDimensions();
        }


        protected void rgCrossTabReport_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            rgCrossTabReport.DataSource = MD.Objects.CrossTabReport.GetByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
        }

        protected void lnk_Command(object sender, CommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("EditCrossTab", StringComparison.OrdinalIgnoreCase))
                {
                    if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                    {
                        CrossTabReport ctr = CrossTabReport.GetByID(Master.clientconnections, Convert.ToInt32(e.CommandArgument));

                        hfCrossTabReportID.Value = ctr.CrossTabReportID.ToString();
                        hfCreatedDate.Value = ctr.CreatedDate.ToString();
                        hfCreatedUserID.Value = ctr.CreatedUserID.ToString();
                        txtCrossTabReportName.Text = ctr.CrossTabReportName;
                        ddlViewType.SelectedValue = ctr.View_Type.ToString();

                        if (ctr.View_Type.ToString() == Enums.ViewType.ProductView.ToString())
                        {
                            phProduct.Visible = true;
                            LoadProducts();
                            ddlProduct.SelectedValue = ctr.PubID.ToString();
                            hfPubID.Value = ctr.PubID.ToString();
                            LoadDimensions();
                        }
                        else
                        {
                            phProduct.Visible = false;
                            hfPubID.Value = "0";
                            ddlProduct.Items.Clear();
                            LoadDimensions();
                        }

                        ddlRowDimension.SelectedValue = ctr.Row.ToString();
                        ddlColumnDimension.SelectedValue = ctr.Column.ToString();

                        btnSave.Text = "UPDATE";
                        lblpnlHeader.Text = "Edit CrossTab Report";
                    }
                }
                else
                {
                    if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                    {
                        CrossTabReport.Delete(Master.clientconnections, Convert.ToInt32(e.CommandArgument.ToString()), Master.UserSession.UserID);
                        LoadCrossTabReportGrid();
                    }
                }

            }
            catch (Exception ex)
            {
                divError.Visible = true;
                lblErrorMessage.Text = ex.Message;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (CrossTabReport.ExistsByCrossTabReportName(Master.clientconnections, Convert.ToInt32(hfCrossTabReportID.Value), txtCrossTabReportName.Text))
                {
                    DisplayError("Report Name already exists.");
                    return;
                }

                if(ddlRowDimension.SelectedValue == ddlColumnDimension.SelectedValue)
                {
                    DisplayError("Row and Column cannot be same.");
                    return;
                }

                CrossTabReport ctr = new CrossTabReport();

                ctr.CrossTabReportID  = Convert.ToInt32(hfCrossTabReportID.Value);
                ctr.CrossTabReportName = txtCrossTabReportName.Text;
                    
                if(Convert.ToInt32(hfCrossTabReportID.Value) > 0)
                {
                    ctr.CreatedUserID = Convert.ToInt32(hfCreatedUserID.Value);
                    ctr.CreatedDate = Convert.ToDateTime(hfCreatedDate.Value);
                    ctr.UpdatedUserID = Master.LoggedInUser;
                    ctr.UpdatedDate = DateTime.Now;
                }
                else
                {
                    ctr.CreatedUserID = Master.LoggedInUser;
                    ctr.CreatedDate = DateTime.Now;
                    ctr.UpdatedUserID = Master.LoggedInUser;
                    ctr.UpdatedDate = DateTime.Now;
                }
                ctr.View_Type = (Enums.ViewType)Enum.Parse(typeof(Enums.ViewType), ddlViewType.SelectedValue, true);
                ctr.PubID = Convert.ToInt32(hfPubID.Value);
                ctr.BrandID = Convert.ToInt32(hfBrandID.Value) == 0 ? (int?)null : Convert.ToInt32(hfBrandID.Value);
                ctr.IsDeleted = false;
                ctr.Row = ddlRowDimension.SelectedValue;
                ctr.Column = ddlColumnDimension.SelectedValue;

                CrossTabReport.Save(Master.clientconnections, ctr);
            }
            catch (Exception ex)
            {
                DisplayError(ex.ToString());
                return;
            }

            LoadCrossTabReportGrid();
            ResetControls();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ResetControls();
        }

        protected void ResetControls()
        {
            lblpnlHeader.Text = "Add CrossTab Report";
            hfCrossTabReportID.Value = "0";
            hfPubID.Value = "0";
            hfCreatedDate.Value = "";
            hfCreatedUserID.Value = "";
            txtCrossTabReportName.Text = "";
            ddlViewType.ClearSelection();
            phProduct.Visible = false;
            LoadDimensions();
        }
    }
}