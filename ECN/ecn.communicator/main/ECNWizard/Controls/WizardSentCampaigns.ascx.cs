using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using ecn.communicator.CommonControls;

namespace ecn.communicator.main.ECNWizard.Controls
{
    public partial class WizardSentCampaigns : System.Web.UI.UserControl
    {
        public event EventHandler EmojiEvent;
        public event EventHandler ErrorEvent;
        public ECN_Framework_Common.Objects.ECNException ecnException;
        int SentRecordCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            gvSent.PageIndexChanging += new GridViewPageEventHandler(gvSent_PageIndexChanging);

            if (!IsPostBack)
            {
                if (KMPlatform.BusinessLogic.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReport, KMPlatform.Enums.Access.View))
                {
                    txtFrom.Text = DateTime.Now.AddDays(-90).ToShortDateString();
                    txtTo.Text = DateTime.Now.ToShortDateString();


                    List<ECN_Framework_Entities.Communicator.Campaign> campaignList =
                        ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCustomerID_NoAccessCheck(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID, false);
                    var resultCampaignList = (from src in campaignList
                                              orderby src.CampaignName
                                              select src).ToList();
                    drpCampaign.DataSource = resultCampaignList;
                    drpCampaign.DataBind();
                    drpCampaign.Items.Insert(0, new ListItem("-- All --", "0"));

                    List<KMPlatform.Entity.User> userList =
                    KMPlatform.BusinessLogic.User.GetByCustomerID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID);
                    var resultUserList = (from src in userList
                                          orderby src.UserName
                                          select src).ToList();
                    drpSentUser.DataSource = resultUserList;
                    drpSentUser.DataBind();
                    drpSentUser.Items.Insert(0, new ListItem("-- All --", "0"));

                    loadSentCampaignsItems();

                    gvSent.Columns[7].Visible = KMPlatform.BusinessLogic.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Blast, KMPlatform.Enums.Access.Edit);
                }
                else
                {
                    throw new ECN_Framework_Common.Objects.SecurityException() { SecurityType = ECN_Framework_Common.Objects.Enums.SecurityExceptionType.RoleAccess };
                }
            }
        }

        private void setECNError(ECN_Framework_Common.Objects.ECNException validationExc)
        {

            ecnException = validationExc;
            ErrorEvent(null, null);


           
        }

        #region SentGrid Events
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            loadSentCampaignsItems();
            if(EmojiEvent != null)
                EmojiEvent(sender, e);
        }

        protected void gvSent_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropDown = (DropDownList)sender;
            this.gvSent.PageSize = int.Parse(dropDown.SelectedValue);
            loadSentCampaignsItems();
        }

        protected void gvSent_TextChanged(object sender, EventArgs e)
        {
            TextBox txtGoToPageContent = (TextBox)sender;

            int pageNumber;
            if (int.TryParse(txtGoToPageContent.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= this.gvSent.PageCount)
            {
                this.gvSent.PageIndex = pageNumber - 1;
            }
            else
            {
                this.gvSent.PageIndex = 0;
            }
            loadSentCampaignsItems();
        }

        protected void gvSent_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                Label lblTotalRecordsContent = (Label)e.Row.FindControl("gvSent_lblTotalRecords");
                lblTotalRecordsContent.Text = SentRecordCount.ToString();

                Label lblTotalNumberOfPagesContent = (Label)e.Row.FindControl("gvSent_lblTotalNumberOfPages");
                lblTotalNumberOfPagesContent.Text = gvSent.PageCount.ToString();

                TextBox txtGoToPageContent = (TextBox)e.Row.FindControl("gvSent_txtGoToPage");
                txtGoToPageContent.Text = (gvSent.PageIndex + 1).ToString();

                DropDownList ddlPageSizeContent = (DropDownList)e.Row.FindControl("gvSent_ddlPageSize");
                ddlPageSizeContent.SelectedValue = gvSent.PageSize.ToString();
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton blastDetails = (LinkButton)e.Row.FindControl("lnkbtnBlastDetails");
                blastDetails.CommandArgument = e.Row.RowIndex.ToString();
                GridView gvDetails = (GridView)e.Row.FindControl("gvBlastDetails");
                gvDetails.Columns[8].Visible = KMPlatform.BusinessLogic.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Blast, KMPlatform.Enums.Access.Edit);

                DataRowView dr = (DataRowView)e.Row.DataItem;
                try
                {
                    if (dr["CampaignItemType"].ToString().ToLower() == ECN_Framework_Common.Objects.Communicator.Enums.BlastType.Personalization.ToString().ToLower())
                    {
                        LinkButton lb = (LinkButton)e.Row.FindControl("CopyCampaignItemBtn");
                        lb.Visible = false;
                    }

                }
                catch { }

            }
        }

        protected void gvSent_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                gvSent.PageIndex = e.NewPageIndex;
            }
            gvSent.DataBind();
            loadSentCampaignsItems();
        }

        protected void gvSent_Command(Object sender, GridViewCommandEventArgs e)
        {
            
            if (e.CommandName == "BlastDetails")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                loadBlastDetails_Sent(index);
            }
            else if (e.CommandName == "CopyCampaignItem")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                CopyCampaignItem(index);                
            }

            if (EmojiEvent != null)
            {
                EmojiEvent(sender, e);
            }

        }

        private void CopyCampaignItem(int campaignItemID)
        {
            try
            {
                int copyCampaignItemID = ECN_Framework_BusinessLayer.Communicator.CampaignItem.CopyCampaignItem(campaignItemID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(copyCampaignItemID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, false);
                string redirectURL = "/ecn.communicator/main/ecnwizard/wizardsetup.aspx?CampaignItemID=" + ci.CampaignItemID + "&campaignItemType=" + ci.CampaignItemType;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redir", "document.location = '" + redirectURL + "'", true);
            }
            catch(ECN_Framework_Common.Objects.ECNException ecn)
            {
                setECNError(ecn);
            }
        }

        private void loadBlastDetails_Sent(int index)
        {

            Panel pnlBlastReport = gvSent.Rows[index].FindControl("pnlBlastReport") as Panel;
            GridView gvBlastDetails = gvSent.Rows[index].FindControl("gvBlastDetails") as GridView;
            int campaignItemID = Convert.ToInt32(((Label)gvSent.Rows[index].FindControl("lblCampaignItemID")).Text);
            LinkButton exp = gvSent.Rows[index].FindControl("lnkbtnBlastDetails") as LinkButton;
            if (gvBlastDetails.Rows.Count > 0)
            {
                if (pnlBlastReport.Visible == true)
                {
                    pnlBlastReport.Visible = false;
                    exp.Text = "+Details";
                    gvSent.Rows[index].Font.Bold = false;

                }
                else if (pnlBlastReport.Visible == false)
                {
                    pnlBlastReport.Visible = true;
                    exp.Text = "-Details";
                    gvSent.Rows[index].Font.Bold = true;
                }
            }
            else
            {
                if (exp.Text.Equals("-Details"))
                {
                    exp.Text = "+Details";
                    gvSent.Rows[index].Font.Bold = false;
                }
                else if (exp.Text.Equals("+Details"))
                {
                    List<ECN_Framework_Entities.Communicator.BlastAbstract> blastList =
                    ECN_Framework_BusinessLayer.Communicator.Blast.GetByCampaignItemID_NoAccessCheck(campaignItemID,  true);

                    if (drpBlastType.SelectedValue.Equals("false"))
                    {
                        //Live blast section
                        var result = (from src in blastList
                                      where src.TestBlast.ToLower() == "n"
                                      select new {BlastID = src.BlastID,EmailSubject = src.EmailSubject,GroupName = src.Group.GroupName, SendTotal=src.SendTotal,TestBlast=src.TestBlast,hasEmailPreview=src.HasEmailPreview }).ToList();

                        gvBlastDetails.DataSource = result;
                        gvBlastDetails.DataBind();
                        gvBlastDetails.Columns[8].Visible = false;
                    }
                    else
                    {
                        //Test blast section
                        var result = (from src in blastList
                                      where src.TestBlast.ToLower() == "y"
                                      select new { BlastID = src.BlastID, EmailSubject = src.EmailSubject, GroupName = src.Group.GroupName, SendTotal = src.SendTotal, TestBlast = src.TestBlast, hasEmailPreview = src.HasEmailPreview }).ToList();

                        gvBlastDetails.DataSource = result;
                        gvBlastDetails.DataBind();
                        gvBlastDetails.Columns[8].Visible = KMPlatform.BusinessLogic.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Blast, KMPlatform.Enums.Access.Edit);
                    }

                    pnlBlastReport.Visible = true;
                    exp.Text = "-Details";
                    gvSent.Rows[index].Font.Bold = true;
                    if (blastList.Count == 0)
                    {
                        pnlBlastReport.Visible = false;
                    }
                }
            }
        }
        #endregion

        private void loadSentCampaignsItems()
        {
            DataTable dt = new DataTable();
            int? userID = null;
            if (Convert.ToInt32(drpSentUser.SelectedValue) > 0)
                userID = Convert.ToInt32(drpSentUser.SelectedValue);
            string campaignName = string.Empty;
            if (Convert.ToInt32(drpCampaign.SelectedValue) > 0)
                campaignName = drpCampaign.SelectedItem.ToString().Trim();
            string emailSubject = string.Empty;
            string groupName = string.Empty;
            string layoutName = string.Empty;
            string campaignItemName = string.Empty;
            int blastID = 0;
            if (drpSearchCriteria.SelectedValue.Equals("CampaignItem"))
            {
                campaignItemName = txtSearch.Text;
            }
            else if (drpSearchCriteria.SelectedValue.Equals("Subject"))
            {
                emailSubject = txtSearch.Text;
            }
            else if (drpSearchCriteria.SelectedValue.Equals("Message"))
            {
                layoutName = txtSearch.Text;
            }
            else if (drpSearchCriteria.SelectedValue.Equals("Group"))
            {
                groupName = txtSearch.Text;
            }
            else if (drpSearchCriteria.SelectedValue.Equals("BlastID"))
            {
                int.TryParse(txtSearch.Text.Trim(), out blastID);
            }

            var dateValidator = new DateValidator();
            bool isValid = dateValidator.ValidateDates(txtFrom, txtTo, 2, phError, lblErrorMessage, true);

            if (isValid)
            {
                DateTime fromDate = DateTime.Today.AddDays(-14);
                try
                {
                    fromDate = Convert.ToDateTime(txtFrom.Text);
                }
                catch (Exception)
                {
                    txtFrom.Text = String.Format("{0:M/d/yyyy}", fromDate);
                }

                DateTime toDate = DateTime.Today;
                try
                {
                    toDate = Convert.ToDateTime(txtTo.Text);
                }
                catch (Exception)
                {
                    txtTo.Text = String.Format("{0:M/d/yyyy}", toDate);
                }

                dt = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetSentCampaignItems(campaignName, campaignItemName, emailSubject, layoutName, groupName, blastID, fromDate, toDate, userID, Convert.ToBoolean(drpBlastType.SelectedValue), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);

                SentRecordCount = dt.Rows.Count;
                gvSent.DataSource = dt;
                try
                {
                    gvSent.DataBind();
                }
                catch
                {
                    gvSent.PageIndex = 0;
                    gvSent.DataBind();
                }   
            }

             
        }

        protected bool CheckCampaginItemReportVisible()
        {
            if (drpBlastType.SelectedValue.ToLower().Equals("true"))
            {
                return false;
            }
            else
                return true;
        }

        protected bool CheckEmailPreviewVisible(string IsTestBlast, string hasEmailPreview)
        {
            if (IsTestBlast.ToLower().Equals("y") && hasEmailPreview.ToLower().Equals("true"))
            {
                return true;
            }
            else
                return false;
        }

        protected string EvalWithMaxLength(string fieldName, int maxLength)
        {
            object value = this.Eval(fieldName);
            if (value == null)
                return null;
            string str = value.ToString();
            if (str.Length > maxLength)
                return str.Substring(0, maxLength - 3) + "...";
            else
                return str;
        }


        #region BlastDetails Events

        public void gvBlastDetails_Command(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("ResendTestBlast"))
            {
                KMPlatform.Entity.User cUser = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;
                ECN_Framework_Entities.Communicator.Blast testBlast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(Convert.ToInt32(e.CommandArgument.ToString()),cUser, false );
                ECN_Framework_Entities.Communicator.CampaignItemTestBlast citb = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.GetByBlastID(Convert.ToInt32(e.CommandArgument.ToString()), cUser, true);
                if (Convert.ToInt32(BlastCheck(cUser.CustomerID, testBlast.GroupID.Value, citb.Filters, "", cUser)) <= getTestBlastCount())
                    ECN_Framework_BusinessLayer.Communicator.Blast.ResendTestBlast(Convert.ToInt32(e.CommandArgument.ToString()), cUser);
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "testblastcount", "alert('ERROR - License limit exceeded');", true);
                }
            }
        }

        public void gvBlastDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Adding try-catch for re-usability of the function for gvPending and gvSent
                try
                {
                    if (e.Row.RowIndex >= 0)
                    {
                        Label lblBlastID = (Label)e.Row.FindControl("lblBlastID");
                        ImageButton btnEmailPreview = (ImageButton)e.Row.FindControl("btnEmailPreview");
                        btnEmailPreview.OnClientClick = "javascript:window.open('../blasts/BlastEmailPreview.aspx?blastID=" + lblBlastID.Text + "','BlastEmailPreview','resizable=yes,scrollbars=yes,menubar=yes,titlebar=no,toolbar=no'); return false;";
                    }
                }
                catch { }
            }
        }

        private int getTestBlastCount()
        {
            int count = 0;
            try
            {
                count = Convert.ToInt32(ConfigurationManager.AppSettings["CU_" + ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID + "_TestBlastEmails"].ToString());
            }
            catch
            {
                try
                {
                    count = Convert.ToInt32(ConfigurationManager.AppSettings["CH_" + ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID + "_TestBlastEmails"].ToString());
                }
                catch
                {
                    count = Convert.ToInt32(ConfigurationManager.AppSettings["BASE_TestBlastEmails"].ToString());
                }
            }

            return count;
        }

        private string BlastCheck(int CustomerID, int GroupID, List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> filters, string Suppressionlist, KMPlatform.Entity.User user)
        {

            return ECN_Framework_BusinessLayer.Communicator.Blast.GetBlastEmailsListCount(CustomerID, 0, GroupID, filters,"", Suppressionlist,true, user).ToString();

        }

        #endregion
    }
}