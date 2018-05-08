using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Objects;
using System.Data;
using System.Data.SqlTypes;
using Telerik.Web.UI;

namespace ecn.accounts.main.reports
{
    public partial class BlastStatus : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.REPORTS;

            if (!IsPostBack)
            {
                if (!KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
                {
                    Response.Redirect("/ecn.accounts/main/default.aspx");
                }
            }

            if (TabContainer1.ActiveTabIndex == 1)
            {
                LoadBlastEngineStatus();
            }
        }
        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = string.Empty;
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }
        protected void btnSubmit_Click(object sender, System.EventArgs e)
        {
            phAdditionalData.Visible = false;
            plSendInformation.Visible = false;
            plReset.Visible = false;
            ResetControls();
            hfBlastID.Value = txtBlastID.Text;
            LoadBlastStatus();
        }
        private void ResetControls()
        {
            RadPanelBar1.Visible = false;
            plBlastReport.Visible = false;
            RadPanelItem rpiCurrentCounts = (RadPanelItem)RadPanelBar1.FindItemByValue("rpiCurrentCounts");
            rpiCurrentCounts.Visible = false;
            RadPanelItem rpiPreviousBlastInfo = (RadPanelItem)RadPanelBar1.FindItemByValue("rpiPreviousBlastInfo");
            rpiPreviousBlastInfo.Visible = false;
            RadPanelItem rpiSubscribers = (RadPanelItem)RadPanelBar1.FindItemByValue("rpiSubscribers");
            rpiSubscribers.Visible = false;
            RadPanelItem rpiErrors = (RadPanelItem)RadPanelBar1.FindItemByValue("rpiErrors");
            rpiErrors.Visible = false;
            cbGetBlastReport.Checked = false;
            cbGetCurrentCounts.Checked = false;
            cbGetPreviousBlastInfo.Checked = false;
            cbGetSubscribersToSend.Checked = false;
            cbShowErrors.Checked = false;
            RadGrid rgCurrentCounts = (RadGrid)RadPanelBar1.FindItemByValue("rpitemCurrentCounts").FindControl("rgCurrentCounts");
            rgCurrentCounts.DataSource = null;
            rgCurrentCounts.DataBind();
            RadGrid rgPreviousBlastInfo = (RadGrid)RadPanelBar1.FindItemByValue("rpitemPreviousBlastInfo").FindControl("rgPreviousBlastInfo");
            rgPreviousBlastInfo.DataSource = null;
            rgPreviousBlastInfo.DataBind();
            rgPreviousBlastInfo.Visible = false;
            RadGrid rgSubscribersToSend = (RadGrid)RadPanelBar1.FindItemByValue("rpitemSubscribers").FindControl("rgSubscribersToSend");
            rgSubscribersToSend.DataSource = null;
            rgSubscribersToSend.DataBind();
            RadGrid rgErrors = (RadGrid)RadPanelBar1.FindItemByValue("rpitemErrors").FindControl("rgErrors");
            rgErrors.DataSource = null;
            rgErrors.DataBind();
        }
        protected void rgBlastEngineStatus_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;

                item["StatusIndicator"].Controls.Clear();
                Image img = new Image();

                if (string.Equals(item["Status"].Text, "green", StringComparison.OrdinalIgnoreCase))
                {
                    img.ImageUrl = "/ecn.images/images/StatusGreen.png";
                }
                else
                {
                    img.ImageUrl = "/ecn.images/images/StatusRed.png";
                }

                item["StatusIndicator"].Controls.Add(img);

                RadRadialGauge radialGauge = (RadRadialGauge)item.FindControl("RadRadialGauge");
                if (Convert.ToInt32(item["SendTotal"].Text) > 1)
                {
                    radialGauge.Scale.Max = item["SendTotal"].Text == "&nbsp;" ? 0 : Convert.ToDecimal(item["SendTotal"].Text);
                    radialGauge.Visible = true;
                }
            }
        }

        protected void rgBlastEngineStatus_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            LoadBlastEngineStatus();
        }

        private void LoadBlastStatus()
        {
            if (txtBlastID.Text == string.Empty)
            {
                phError.Visible = true;
                lblErrorMessage.Text = "Please Enter BlastID"; 
            }
            else
            {
                DataTable dt = ECN_Framework_BusinessLayer.Communicator.Blast.GetBlastStatusByBlastID(Convert.ToInt32(hfBlastID.Value));

                dt.Columns["EmailSubject"].ReadOnly = false;
                foreach (DataRow dr in dt.Rows)
                {
                    dr["EmailSubject"] = ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(dr["EmailSubject"].ToString());
                }

                rgBlastSearch.DataSource = dt;
                rgBlastSearch.DataBind();
                rgBlastSearch.Visible = true;
                    

                
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (string.Equals(dt.Rows[0]["StatusCode"].ToString(), "Error", StringComparison.OrdinalIgnoreCase))
                    {
                        phAdditionalData.Visible = true;
                        plReset.Visible = true;
                    }
                    else
                    {
                        phAdditionalData.Visible = true;

                        if (string.Equals(dt.Rows[0]["StatusCode"].ToString(), "Sent", StringComparison.OrdinalIgnoreCase))
                        {
                            plSendInformation.Visible = true;
                        }
                    }
                    hfBlastEngineID.Value = dt.Rows[0]["BlastEngineID"].ToString();
                    hfBlastFinishTime.Value = dt.Rows[0]["FinishTime"].ToString();
                    hfGroupID.Value = dt.Rows[0]["GroupID"].ToString();
                    hfCustomerID.Value = dt.Rows[0]["CustomerID"].ToString();
                }
            }
        }
        protected void btnAdditionalData_Click(object sender, System.EventArgs e)
        {
            try
            {
                plBlastReport.Visible = false;
                RadPanelItem rpiCurrentCounts = (RadPanelItem)RadPanelBar1.FindItemByValue("rpiCurrentCounts");
                rpiCurrentCounts.Visible = false;
                RadPanelItem rpiPreviousBlastInfo = (RadPanelItem)RadPanelBar1.FindItemByValue("rpiPreviousBlastInfo");
                rpiPreviousBlastInfo.Visible = false;
                RadPanelItem rpiSubscribers = (RadPanelItem)RadPanelBar1.FindItemByValue("rpiSubscribers");
                rpiSubscribers.Visible = false;
                RadPanelItem rpiErrors = (RadPanelItem)RadPanelBar1.FindItemByValue("rpiErrors");
                rpiErrors.Visible = false;
                RadGrid rgCurrentCounts = (RadGrid)RadPanelBar1.FindItemByValue("rpitemCurrentCounts").FindControl("rgCurrentCounts");
                rgCurrentCounts.DataSource = null;
                rgCurrentCounts.DataBind();
                RadGrid rgPreviousBlastInfo = (RadGrid)RadPanelBar1.FindItemByValue("rpitemPreviousBlastInfo").FindControl("rgPreviousBlastInfo");
                rgPreviousBlastInfo.DataSource = null;
                rgPreviousBlastInfo.DataBind();
                rgPreviousBlastInfo.Visible = false;
                RadGrid rgSubscribersToSend = (RadGrid)RadPanelBar1.FindItemByValue("rpitemSubscribers").FindControl("rgSubscribersToSend");
                rgSubscribersToSend.DataSource = null;
                rgSubscribersToSend.DataBind();
                RadGrid rgErrors = (RadGrid)RadPanelBar1.FindItemByValue("rpitemErrors").FindControl("rgErrors");
                rgErrors.DataSource = null;
                rgErrors.DataBind();

                if (cbGetBlastReport.Checked)
                {
                    plBlastReport.Visible = true;
                    hlBlastReport.NavigateUrl = "/ecn.communicator/main/blasts/reports.aspx?blastID=" + hfBlastID.Value;
                }
                
                if (cbGetCurrentCounts.Checked || cbGetPreviousBlastInfo.Checked || cbGetSubscribersToSend.Checked || cbShowErrors.Checked)
                {
                    RadPanelBar1.Visible = true;
                }
                else
                {
                    RadPanelBar1.Visible = false;
                }

                if (cbGetCurrentCounts.Checked)
                {
                    rpiCurrentCounts.Visible = true;
                    rgCurrentCounts.DataSource = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetBlastReportData(Convert.ToInt32(hfBlastID.Value), "", "");
                    rgCurrentCounts.DataBind();
                }

                if (cbGetPreviousBlastInfo.Checked)
                {
                    rpiPreviousBlastInfo.Visible = true;
                    if (hfBlastFinishTime.Value != string.Empty)
                    {
                        rgPreviousBlastInfo.Visible = true;
                        rgPreviousBlastInfo.DataSource = ECN_Framework_BusinessLayer.Communicator.Blast.GetBlastStatusByBlastEngineIDFinishTime(Convert.ToInt32(hfBlastEngineID.Value), Convert.ToDateTime(hfBlastFinishTime.Value)); 
                        rgPreviousBlastInfo.DataBind();
                    }
                }

                if (cbGetSubscribersToSend.Checked)
                {
                    rpiSubscribers.Visible = true;
                    ECN_Framework_Entities.Communicator.CampaignItemBlast cib = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByBlastID(Convert.ToInt32(hfBlastID.Value.ToString()),Master.UserSession.CurrentUser,false);
                    List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> listFilters = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.GetByCampaignItemBlastID(cib.CampaignItemBlastID);
                    rgSubscribersToSend.DataSource = ECN_Framework_BusinessLayer.Communicator.Blast.GetBlastEmailListForDynamicContent(Convert.ToInt32(hfCustomerID.Value), Convert.ToInt32(hfBlastID.Value), Convert.ToInt32(hfGroupID.Value), listFilters, "", "", "", false, false);
                    rgSubscribersToSend.DataBind();
                }

                if (cbShowErrors.Checked)
                {
                    rpiErrors.Visible = true;
                    // ECN_Blast_Engine - ApplicationID = 2
                    rgErrors.DataSource = KM.Common.Entity.ApplicationLog.Select(2).OrderByDescending(x=>x.LogID).Take(5);
                    rgErrors.DataBind();
                }
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
        }
        protected void btnReset_Click(object sender, System.EventArgs e)
        {
            try
            {
                ECN_Framework_BusinessLayer.Communicator.Blast.UpdateStatusBlastEngineID(Convert.ToInt32(hfBlastID.Value), ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.Pending);
                LoadBlastStatus();
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
        }
        private void LoadBlastEngineStatus()
        {
            DataTable dtStatus = ECN_Framework_BusinessLayer.Communicator.BlastEngines.GetBlastEngineStatus();
            dtStatus.Columns["EmailSubject"].ReadOnly = false;
            foreach(DataRow dr in dtStatus.Rows)
            {
                dr["EmailSubject"] = ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(dr["EmailSubject"].ToString());
            }
            rgBlastEngineStatus.DataSource = dtStatus;
        }
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            lbTimeRefreshed.Text = "Grid will refresh after every 1 minute. Grid Refreshed at: " + DateTime.Now.ToLongTimeString();
        }

        protected void rgSubscribersToSend_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid rgSubscribersToSend = (RadGrid)RadPanelBar1.FindItemByValue("rpitemSubscribers").FindControl("rgSubscribersToSend");
            ECN_Framework_Entities.Communicator.CampaignItemBlast cib = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByBlastID(Convert.ToInt32(hfBlastID.Value.ToString()),Master.UserSession.CurrentUser, false);
            List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> listFilters = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.GetByCampaignItemBlastID(cib.CampaignItemBlastID);
            rgSubscribersToSend.DataSource = ECN_Framework_BusinessLayer.Communicator.Blast.GetBlastEmailListForDynamicContent(Convert.ToInt32(hfCustomerID.Value), Convert.ToInt32(hfBlastID.Value), Convert.ToInt32(hfGroupID.Value), listFilters, "", "", "",  false, false);
        }
    }
}