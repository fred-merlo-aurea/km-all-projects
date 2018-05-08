using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KMPS.MD.Objects;

namespace KMPS.MD.Main
{
    public partial class CampaignSegmentation : System.Web.UI.Page
    {
        delegate void RebuildSubscriberList();

        delegate void HidePanel();

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Campaigns";
            Master.SubMenu = "Campaign Comparison";

            RebuildSubscriberList delNoParam = new RebuildSubscriberList(Reload);
            this.DownloadPanel1.DelMethod = delNoParam;

            HidePanel delNoParam1 = new HidePanel(Hide);
            this.DownloadPanel1.hideDownloadPopup = delNoParam1;

            if (!IsPostBack)
            {
                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.CampaignComparison, KMPlatform.Enums.Access.View))
                {
                    Response.Redirect("../SecurityAccessError.aspx");
                }

                List<Brand> b = new List<Brand>();
                bool IsBrandAssignedUser = false;

                if (!KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                {
                    b = Brand.GetByUserID(Master.clientconnections, Master.LoggedInUser);

                    if (b.Count > 0)
                        IsBrandAssignedUser = true;
                }

                if (b.Count == 0)
                {
                    b = Brand.GetAll(Master.clientconnections);
                }

                if (b.Count > 0)
                {
                    pnlBrand.Visible = true;
                    if (b.Count > 1)
                    {
                        ddlBrand.Visible = true;
                        ddlBrand.DataSource = b;
                        ddlBrand.DataBind();

                        if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser) || !IsBrandAssignedUser)
                        {
                            ddlBrand.Items.Insert(0, new ListItem("All Products", "0"));
                            loadcampaign();
                        }
                        else
                        {
                            ddlBrand.Items.Insert(0, new ListItem("", "-1"));
                        }

                        hfBrandID.Value = ddlBrand.SelectedItem.Value;
                    }
                    else if (b.Count == 1)
                    {
                        if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser) || !IsBrandAssignedUser)
                        {
                            ddlBrand.Visible = true;
                            ddlBrand.DataSource = b;
                            ddlBrand.DataBind();
                            ddlBrand.Items.Insert(0, new ListItem("All Products", "0"));
                            hfBrandID.Value = ddlBrand.SelectedItem.Value;
                        }
                        else
                        {
                            lblBrandName.Visible = true;
                            lblBrandName.Text = b.FirstOrDefault().BrandName;
                            hfBrandID.Value = b.FirstOrDefault().BrandID.ToString();
                        }

                        loadcampaign();
                    }
                }
                else
                {
                    loadcampaign();
                }

                DownloadPanel1.Showexporttoemailmarketing = true;
                DownloadPanel1.Showsavetocampaign = false;
                DownloadPanel1.error = false;
                DownloadPanel1.BrandID = Convert.ToInt32(hfBrandID.Value);
            }

            lblErrorMsg.Text = string.Empty;
            divErrorMsg.Visible = false;
        }

        public void DisplayError(string errorMessage)
        {
            lblErrorMsg.Text = errorMessage;
            divErrorMsg.Visible = true;
        }

        private void Reload()
        {
            DownloadPanel1.SubscribersQueries = Campaigns.GetCampaignSegmentationQuery(hfSelected.Value, hfSuppressed.Value, hfOperation.Value);
        }

        private void Hide()
        {
            DownloadPanel1.Visible = false;
        }

        protected void loadcampaign()
        {
            if (Convert.ToInt32(hfBrandID.Value) >= 0)
            {
                List<Campaigns> lcampaigns = new List<Campaigns>();

                if (Convert.ToInt32(hfBrandID.Value) > 0)
                    lcampaigns = Campaigns.GetByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
                else
                    lcampaigns = Campaigns.GetNotInBrand(Master.clientconnections);

                lstCampaignIn.DataSource = lcampaigns;
                lstCampaignIn.DataBind();
                lstCampaignNotIn.DataSource = lcampaigns;
                lstCampaignNotIn.DataBind();
            }
        }

        protected void ddlOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstCampaignIn.ClearSelection();
            lstCampaignNotIn.ClearSelection();
            phResults.Visible = false;

            if (ddlOperation.SelectedValue.Equals("Intersect", StringComparison.OrdinalIgnoreCase) || ddlOperation.SelectedValue.Equals("Union", StringComparison.OrdinalIgnoreCase))
            {
                lblCampaign.Text = "Campaigns";
                phOperationNotIn.Visible = false;
            }
            else if (ddlOperation.SelectedValue.Equals("NotIn", StringComparison.OrdinalIgnoreCase))
            {
                lblCampaign.Text = "Campaigns In";
                phOperationNotIn.Visible = true;
            }
            else
            {
                lblCampaign.Text = "Campaigns";
                phOperationNotIn.Visible = false;
            }
        }

        protected void ddlBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlOperation.ClearSelection();
            lstCampaignIn.Items.Clear();
            lstCampaignNotIn.Items.Clear();
            phResults.Visible = false;
            lblCampaign.Text = "Campaigns";

            hfBrandID.Value = ddlBrand.SelectedValue;
            
            if (Convert.ToInt32(ddlBrand.SelectedValue) >= 0)
            {
                loadcampaign();
                phOperationNotIn.Visible = false;
            }

            DownloadPanel1.BrandID = Convert.ToInt32(hfBrandID.Value);
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            lnkdownload.Text = "";
            lnkdownload.Enabled = false;
            phResults.Visible = false;
            hfSelected.Value = "";
            hfSuppressed.Value = "";
            hfOperation.Value = "";

            if (ddlOperation.SelectedValue.Equals("NotIn", StringComparison.OrdinalIgnoreCase))
            {
                if (lstCampaignIn.SelectedValue == "")
                {
                    DisplayError("Please select campaigns In.");
                    return;
                }
                else
                {
                    if (lstCampaignNotIn.SelectedValue == "")
                    {
                        DisplayError("Please select campaigns Not In.");
                        return;
                    }
                }

                if (lstCampaignIn.GetSelectedIndices().Length + lstCampaignNotIn.GetSelectedIndices().Length > 10)
                {
                    DisplayError("Please select no more than 10 campaigns.");
                    return;
                }

                List<string> lSelected = new List<string>();
                List<string> lSuppressed = new List<string>();
                string Campaign = string.Empty;

                lSelected = lstCampaignIn.GetSelectedIndices()
                  .Select(i => lstCampaignIn.Items[i].Text)
                  .ToList();

                lSuppressed = lstCampaignNotIn.GetSelectedIndices()
                  .Select(i => lstCampaignNotIn.Items[i].Text)
                  .ToList();

                var difference = lSelected.Intersect(lSuppressed);
                foreach (var item in difference)
                    Campaign += Campaign == string.Empty ? item : ", " + item;

                if (Campaign != string.Empty)
                {
                    DisplayError(Campaign + " can only be selected in In or Not In list.");
                    return;
                }
            }
            else
            {
                if (lstCampaignIn.SelectedValue == "")
                {
                    DisplayError("Please select campaigns.");
                    return;
                }

                if (lstCampaignIn.GetSelectedIndices().Length > 10)
                {
                    DisplayError("Please select no more than 10 campaigns.");
                    return;
                }
            }

            try
            {
                List<string> Selected_CampaignID = new List<string>();
                List<string> Suppressed_CampaignID = new List<string>();

                Selected_CampaignID = lstCampaignIn.GetSelectedIndices()
                    .Select(i => lstCampaignIn.Items[i].Value)
                    .ToList();

                Suppressed_CampaignID = lstCampaignNotIn.GetSelectedIndices()
                    .Select(i => lstCampaignNotIn.Items[i].Value)
                    .ToList();
                
                hfSelected.Value = string.Join(",", Selected_CampaignID.ToArray());
                hfSuppressed.Value = string.Join(",", Suppressed_CampaignID.ToArray());
                hfOperation.Value = ddlOperation.SelectedValue.Equals("NotIn", StringComparison.OrdinalIgnoreCase) ? "" : ddlOperation.SelectedValue;

                List<int> CampaignList = Campaigns.getSubscriptionIDforCampaignOperation(Master.clientconnections, ddlOperation.SelectedValue, Selected_CampaignID, Suppressed_CampaignID, Convert.ToInt32(hfBrandID.Value));
                lnkdownload.Text = CampaignList.Count().ToString();
                lnkdownload.Enabled = CampaignList.Count() > 0 ? true : false;
                phResults.Visible = true;
            }
            catch(Exception ex)
            {
                DisplayError(ex.ToString());
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ddlBrand.ClearSelection();
            ddlOperation.ClearSelection();
            lstCampaignIn.Items.Clear();
            lstCampaignNotIn.Items.Clear();
            phResults.Visible = false;
            lblCampaign.Text = "Campaigns";

            hfBrandID.Value = ddlBrand.SelectedValue != "" ? ddlBrand.SelectedValue : "0";

            if (hfBrandID.Value != "")
            {
                loadcampaign();
                phOperationNotIn.Visible = false;
            }

            DownloadPanel1.BrandID = Convert.ToInt32(hfBrandID.Value);
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lnkdownload.Text) > 0)
            {
                DownloadPanel1.SubscriptionID = null;
                DownloadPanel1.SubscribersQueries = Campaigns.GetCampaignSegmentationQuery(hfSelected.Value, hfSuppressed.Value, hfOperation.Value);
                DownloadPanel1.Visible = true;
                DownloadPanel1.ViewType = Enums.ViewType.ConsensusView;
                DownloadPanel1.EnableCbIsRecentData = true;
                DownloadPanel1.VisibleCbIsRecentData = true;
                DownloadPanel1.downloadCount = Convert.ToInt32(lnkdownload.Text);
                DownloadPanel1.LoadControls();
                DownloadPanel1.LoadDownloadTemplate();
                DownloadPanel1.loadExportFields();
                DownloadPanel1.ValidateExportPermission();
            }
        }
    }   
}