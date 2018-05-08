using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KMPS.MD.Objects;
using Microsoft.Reporting.WebForms;
using System.Text;

namespace KMPS.MD.Main
{
    public partial class Campaign : KMPS.MD.Main.WebPageHelper
    {
        delegate void RebuildSubscriberList();

        delegate void HidePanel();

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Campaigns";
            Master.SubMenu = "View Campaigns";

            RebuildSubscriberList delNoParam = new RebuildSubscriberList(Reload);
            this.DownloadPanel1.DelMethod = delNoParam;

            HidePanel delNoParam1 = new HidePanel(Hide);
            this.DownloadPanel1.hideDownloadPopup = delNoParam1;

            if (!IsPostBack)
            {
                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.Campaign, KMPlatform.Enums.Access.View))
                {
                    Response.Redirect("../SecurityAccessError.aspx");
                }

                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.Campaign, KMPlatform.Enums.Access.Download))
                {
                    btnDownload.Visible = false;
                    drpExport.Visible = false;
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
                        drpBrand.Visible = true;
                        drpBrand.DataSource = b;
                        drpBrand.DataBind();

                        if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser) || !IsBrandAssignedUser)
                        {
                            drpBrand.Items.Insert(0, new ListItem("All Products", "0"));
                            loadgrid();
                        }
                        else
                        {
                            drpBrand.Items.Insert(0, new ListItem("", "-1"));
                            pnlCampaign.Visible = false;
                            pnlExport.Visible = false;
                        }

                        hfBrandID.Value = drpBrand.SelectedItem.Value;
                    }
                    else if (b.Count == 1)
                    {
                        if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser) || !IsBrandAssignedUser)
                        {
                            drpBrand.Visible = true;
                            drpBrand.DataSource = b;
                            drpBrand.DataBind();
                            drpBrand.Items.Insert(0, new ListItem("All Products", "0"));
                            hfBrandID.Value = drpBrand.SelectedItem.Value;
                        }
                        else
                        {
                            lblColon.Visible = true;
                            lblBrandName.Visible = true;
                            lblBrandName.Text = b.FirstOrDefault().BrandName;
                            hfBrandID.Value = b.FirstOrDefault().BrandID.ToString();
                        }

                        loadgrid();
                    }
                }
                else
                {
                    loadgrid();
                }

                DownloadPanel1.error = false;
                DownloadPanel1.BrandID = Convert.ToInt32(hfBrandID.Value);
            }


            lblErrorMsg.Text = string.Empty; 
            divErrorMsg.Visible = false;
        }

        protected void loadgrid()
        {
            if (Convert.ToInt32(hfBrandID.Value) >= 0)
            {
                List<KMPS.MD.Objects.Campaigns> lst = null;

                if (Convert.ToInt32(hfBrandID.Value) > 0)
                    lst = Campaigns.GetByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
                else
                    lst = Campaigns.GetNotInBrand(Master.clientconnections);

                gvCampaign.DataSource = lst;
                gvCampaign.DataBind();

                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.Campaign, KMPlatform.Enums.Access.Delete))
                {
                    gvCampaign.Columns[4].Visible = false;
                }
            }
        }

        protected void gvCampaign_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCampaign.PageIndex = e.NewPageIndex;
            loadgrid();
        }

        protected void gvCampaign_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView gv = new GridView();
                gv = (GridView)e.Row.FindControl("gvCampaignFilter");

                gv.DataSource = CampaignFilter.GetByCampaignID(Master.clientconnections, Convert.ToInt32(gvCampaign.DataKeys[e.Row.RowIndex].Value));
                gv.DataBind();

                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.Campaign, KMPlatform.Enums.Access.Delete))
                {
                    gv.Columns[3].Visible = false;
                }
            }
        }

        protected void gvCampaign_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                Campaigns.Delete(Master.clientconnections, Convert.ToInt32(e.CommandArgument.ToString()));
                loadgrid();
            }
        }

        protected void gvCampaign_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        protected void gvCampaignFilter_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                CampaignFilter.Delete(Master.clientconnections, Convert.ToInt32(e.CommandArgument.ToString()));
                loadgrid();
            }
        }

        protected void gvCampaignFilter_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        protected void gvCampaignFilter_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView gvChild = ((GridView)sender);
            GridViewRow gvRowParent = ((GridView)sender).Parent.Parent as GridViewRow;
            gvChild.PageIndex = e.NewPageIndex;
            loadgrid();
        }

        protected void lnkdownloadAll_Command(object sender, CommandEventArgs e)
        {
            string temp = e.CommandArgument.ToString();
            string[] args = temp.Split('/');
            lblCampaignFilterID.Text = "0";
            lblCampaignID.Text = Convert.ToInt32(args[0]).ToString();
            DetailsDownload(Convert.ToInt32(args[1]));
   
        }

        protected void lnkdownload_Command(object sender, CommandEventArgs e)
        {
            string temp = e.CommandArgument.ToString();
            string[] args = temp.Split('/');
            lblCampaignFilterID.Text = Convert.ToInt32(args[1]).ToString();
            lblCampaignID.Text = Convert.ToInt32(args[0]).ToString(); 
            DetailsDownload(Convert.ToInt32(args[2]));
        }
         
        #region Export/Download Events

        private void DetailsDownload(int DownloadCount)
        {
            if (DownloadCount > 0)
            {
                DownloadPanel1.SubscriptionID = null;
                DownloadPanel1.SubscribersQueries = Campaigns.GetCampaignQuery(Int32.Parse(lblCampaignID.Text), Int32.Parse(lblCampaignFilterID.Text));
                DownloadPanel1.Visible = true;
                DownloadPanel1.ViewType = Enums.ViewType.ConsensusView;
                DownloadPanel1.EnableCbIsRecentData = true;
                DownloadPanel1.VisibleCbIsRecentData = true;
                DownloadPanel1.downloadCount = DownloadCount;
                DownloadPanel1.LoadControls();
                DownloadPanel1.CampaignID = Convert.ToInt32(lblCampaignID.Text);
                DownloadPanel1.CampaignFilterID = Convert.ToInt32(lblCampaignFilterID.Text);
                DownloadPanel1.Showexporttoemailmarketing = true;
                DownloadPanel1.Showsavetocampaign = false;
                DownloadPanel1.ValidateExportPermission();
                DownloadPanel1.LoadControls();
                DownloadPanel1.LoadDownloadTemplate();
                DownloadPanel1.loadExportFields();
            }
        }

        #endregion

        private void Reload()
        {
            DownloadPanel1.SubscribersQueries = Campaigns.GetCampaignQuery(Int32.Parse(lblCampaignID.Text), Int32.Parse(lblCampaignFilterID.Text));
        }

        private void Hide()
        {
            DownloadPanel1.Visible = false;
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            bool ischecked = false;
            string strIDs = string.Empty; 

            foreach (GridViewRow r in gvCampaign.Rows)
            {
                CheckBox chk = r.FindControl("chkSelectDownload") as CheckBox;
                if (chk != null && chk.Checked)
                    ischecked = true;
            }

            if (!ischecked)
            {
                divErrorMsg.Visible = true;
                lblErrorMsg.Text = "Please select a checkbox to download";
            }
            else 
            {
                foreach (GridViewRow r in gvCampaign.Rows)
                {
                    CheckBox chk = r.FindControl("chkSelectDownload") as CheckBox;
                    if (chk != null && chk.Checked && chk.Enabled)
                        strIDs += strIDs == string.Empty ? gvCampaign.DataKeys[r.RowIndex].Values[0].ToString() : "," + gvCampaign.DataKeys[r.RowIndex].Values[0].ToString();
                }

                Microsoft.Reporting.WebForms.ReportDataSource rds = new Microsoft.Reporting.WebForms.ReportDataSource("DS_Campaign", Objects.Campaigns.GetCampaignsByID(Master.clientconnections, strIDs));
                ReportViewer1.Visible = false;
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rds);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/main/reports/" + "rpt_Campaign.rdlc");
                ReportParameter[] parameters = new ReportParameter[1];
                parameters[0] = new ReportParameter("CampaignIDs", strIDs);
                ReportViewer1.LocalReport.SetParameters(parameters);
                ReportViewer1.LocalReport.Refresh();
                ReportViewer1.LocalReport.SubreportProcessing += new Microsoft.Reporting.WebForms.SubreportProcessingEventHandler(this.ReportViewer1_SubReport);

                Warning[] warnings = null;
                string[] streamids = null;
                String mimeType = null;
                String encoding = null;
                String extension = null;
                Byte[] bytes = null;

                string format = string.Empty;

                switch (drpExport.SelectedValue.ToString())
                {
                    case "xls":
                        format = "excel";
                        break;

                    case "doc":
                        format = "word";
                        break;

                    case "pdf":
                        format = "pdf";
                        break;
                }

                bytes = ReportViewer1.LocalReport.Render(format, "", out mimeType, out encoding, out extension, out streamids, out warnings);
                Response.ContentType = mimeType;

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Campaign." + extension);
                Response.BinaryWrite(bytes);
                Response.End();
            }
        }

        private void ReportViewer1_SubReport(object sender, SubreportProcessingEventArgs e)
        {
            if (e.ReportPath == "rpt_CampaignFilters")
                e.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("DS_CampaignFilter", Objects.CampaignFilter.GetByCampaignID(Master.clientconnections, Convert.ToInt32(e.Parameters["CampaignID"].Values[0]))));
        }

        protected void drpBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            hfBrandID.Value = drpBrand.SelectedValue;
            pnlCampaign.Visible = false;
            pnlExport.Visible = false;

            if (Convert.ToInt32(drpBrand.SelectedValue) >= 0)
            {
                loadgrid();
                pnlCampaign.Visible = true;
                pnlExport.Visible = true;
            }

            DownloadPanel1.BrandID = Convert.ToInt32(hfBrandID.Value);
        }

    }
}