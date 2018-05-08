using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Data.SqlTypes;
using KMPS.MD.Objects;
using System.IO;
using System.Xml.Linq;
using System.Threading;
using Microsoft.VisualBasic.FileIO;
using System.Text.RegularExpressions;
using KMPS.MD.Helpers;

namespace KMPS.MD.Tools
{
    public partial class BrandSetup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Tools";
            Master.SubMenu = "Brand Setup";
            divError.Visible = false;
            lblErrorMessage.Text = string.Empty;

            if (!IsPostBack)
            {
                if (!KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                {
                    Response.Redirect("../SecurityAccessError.aspx");
                }

                lstSourceFields.DataValueField = "PubID";
                lstSourceFields.DataTextField = "PubName";
                lstSourceFields.DataSource = Pubs.GetActive(Master.clientconnections) ;
                lstSourceFields.DataBind();

                loadgrid();
            }
        }

        public void DisplayError(string errorMessage)
        {
            lblErrorMessage.Text = errorMessage;
            divError.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
            {
                try
                {
                    if (Brand.ExistsByBrandName(Master.clientconnections, Convert.ToInt32(hfBrandID.Value), txtBrandName.Text))
                    {
                        DisplayError("Brand Name already exists. Please enter different name.");
                        return;
                    }

                    if (lstDestFields.Items.Count == 0)
                    {
                        DisplayError("Please select a product.");
                        return;
                    }

                    if (Convert.ToInt32(hfBrandID.Value) > 0)
                    {
                        BrandDetails.Delete(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
                    }

                    Brand b = new Brand();

                    b.BrandID = Convert.ToInt32(hfBrandID.Value);
                    b.BrandName = txtBrandName.Text;
                    b.Logo = hfImage.Value;
                    b.IsBrandGroup = false;
                    if (b.BrandID > 0)
                    {
                        b.UpdatedUserID = Master.LoggedInUser;
                        b.UpdatedDate = DateTime.Now;
                        b.CreatedDate = Convert.ToDateTime(hfCreatedDate.Value);
                        b.CreatedUserID = Convert.ToInt32(hfCreatedUserID.Value);
                    }
                    else
                    {
                        b.CreatedUserID = Master.LoggedInUser;
                        b.CreatedDate = DateTime.Now;
                        b.UpdatedUserID = Master.LoggedInUser;
                        b.UpdatedDate = DateTime.Now;
                    }

                    int brandID = Brand.Save(Master.clientconnections, b);

                    if (lstDestFields.Items.Count > 0)
                    {
                        foreach (ListItem item in lstDestFields.Items)
                        {
                            BrandDetails bd = new BrandDetails();
                            bd.BrandID = brandID;
                            bd.PubID = Convert.ToInt32(item.Value);
                            BrandDetails.Save(Master.clientconnections, bd);
                        }
                    }

                    BrandScore.UpdateBrandScrore(Master.clientconnections, brandID);

                    Response.Redirect("BrandSetup.aspx");
                }
                catch (Exception ex)
                {
                    DisplayError(ex.Message);
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("BrandSetup.aspx");
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

        //image upload events

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            lblErrorMessage.Text = string.Empty;
            divError.Visible = false;

            var result = FileHelper.SaveLogo(FileSelector.PostedFile, Master.UserSession.CustomerID, Server);

            if (result.Succeeded)
            {
                imglogo.Visible = true;
                imglogo.ImageUrl = result.Url;
                hfImage.Value = result.FileName;
            }
            else
            {
                DisplayError(result.ErrorMessage);
            }
        }

        protected void btnRemoveLogo_Click(object sender, EventArgs e)
        {
            imglogo.Visible = false;
            hfImage.Value = "";
        }

        protected void lnkEdit_Command(object sender, CommandEventArgs e)
        {
            if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
            {
                ResetControls();
                Brand b = Brand.GetByID(Master.clientconnections, Convert.ToInt32(e.CommandArgument));
                hfBrandID.Value = b.BrandID.ToString();
                txtBrandName.Text = b.BrandName;
                hfCreatedDate.Value = b.CreatedDate.ToString();
                hfCreatedUserID.Value = b.CreatedUserID.ToString();

                if (b.Logo != string.Empty)
                {
                    int customerID = Master.UserSession.CustomerID;
                    imglogo.ImageUrl = "../Images/logo/" + customerID + "/" + b.Logo;
                    imglogo.Visible = true;
                    hfImage.Value = b.Logo;
                }

                List<BrandDetails> bd = BrandDetails.GetByBrandID(Master.clientconnections, Convert.ToInt32(e.CommandArgument.ToString()));

                Dictionary<int, string> fields = new Dictionary<int, string>();

                List<Pubs> Pub = Pubs.GetActive(Master.clientconnections);

                foreach (Pubs p in Pub)
                {
                    if (!bd.Exists(x => x.PubID == p.PubID))
                        lstSourceFields.Items.Add(new ListItem(p.PubName, p.PubID.ToString()));
                    else
                        fields.Add(p.PubID, p.PubName);
                }

                foreach (BrandDetails d in bd)
                {
                    string pubName = fields.FirstOrDefault(x => x.Key == d.PubID).Value;
                    lstDestFields.Items.Add(new ListItem(pubName, d.PubID.ToString()));
                }
            }
        }

        protected void gvBrand_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                {
                    Brand.Delete(Master.clientconnections, Convert.ToInt32(e.CommandArgument.ToString()), Master.LoggedInUser);
                    loadgrid();
                }
            }
        }

        protected void gvBrand_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        public void ResetControls()
        {
            imglogo.Visible = false;
            hfImage.Value = string.Empty;
            txtBrandName.Text = string.Empty;
            lstDestFields.Items.Clear();
            lstSourceFields.Items.Clear();
        }

        protected void gvBrand_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBrand.PageIndex = e.NewPageIndex;
            loadgrid();
        }

        protected void loadgrid()
        {
            gvBrand.DataSource = Brand.GetAll(Master.clientconnections);
            gvBrand.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            List<Pubs> lst = Pubs.GetActive(Master.clientconnections);

            for (int i = 0; i < lstDestFields.Items.Count; i++)
            {

                lst.RemoveAll((x) => x.PubName == lstDestFields.Items[i].Text);

            }

            if (lst != null)
            {
                if (txtSearch.Text != string.Empty)
                {
                    lst = lst.FindAll(x => x.PubName.ToLower().Contains(txtSearch.Text.ToLower()));
                }
            }

            lstSourceFields.Items.Clear();
            lstSourceFields.DataSource = lst;
            lstSourceFields.DataBind();
        }
   }
}