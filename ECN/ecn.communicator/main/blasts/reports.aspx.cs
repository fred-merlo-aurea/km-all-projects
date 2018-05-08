using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Text;
using System.Collections.Generic;
using System.IO;
using aspNetMX;
using System.Web.UI.DataVisualization.Charting;
using System.Linq;
using Microsoft.Reporting.WebForms;
using ECN_Framework_Common.Objects;
using System.Drawing;
using ECN_Framework_Entities.Communicator;
using CampaignItem = ECN_Framework_BusinessLayer.Communicator.CampaignItem;
using CampaignItemSocialMedia = ECN_Framework_BusinessLayer.Communicator.CampaignItemSocialMedia;
using SocialMediaAuth = ECN_Framework_BusinessLayer.Communicator.SocialMediaAuth;

namespace ecn.communicator.blastsmanager
{
    public partial class reports : ECN_Framework.WebPageHelper
    {

        string qryStringParam = "";
      

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.REPORTS;
            Master.SubMenu = "";
            Master.Heading = "Blast Reporting";
            Master.HelpContent = "<img align='right' src='/ecn.images/images/icoblasts.gif'><b>Summary Reports</b><br>Gives a summary of Clicks, Bounces and Subscription reports.<br /><br /><b>Latest Clicks</b><br />Lists 10 recent URL clicks in the recent blasts sent.<br /><br /><b>Latest Bounces</b><br />Lists 10 latest email address bounced in the rencet Blasts. <i>Blast</i> column lists the email blast for which the bounced email address was assigned.<br /><i>Bounce Type</i> lists the type of bounce, which would be a <i>softBounce</i> (for instance: email Inbox full) or a <i>hardBounce</i> (for instance: email address doesnot exist).<br /><br /><b>Latest Subscription Changes</b><br />Gives a report of 15 recent email addresses subscribed or unsubscribed.";
            Master.HelpTitle = "Blast Manager";
            KMPlatform.Entity.User currentUser = Master.UserSession.CurrentUser;

            if (KM.Platform.User.HasAccess(currentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReport, KMPlatform.Enums.Access.View))
                 {

                     string currentChannelID = Master.UserSession.CurrentBaseChannel.BaseChannelID.ToString();
                     int requestBlastID = getBlastID();
                     int requestCampaignItemID = getCampaignItemID();

                     qryStringParam = "BlastID=" + requestBlastID;

                     if (getUDFName() != string.Empty && getUDFData() != string.Empty)
                         qryStringParam = qryStringParam + "&UDFName=" + getUDFName() + "&UDFdata=" + getUDFData();

                     if (requestCampaignItemID > 0)
                     {
                         qryStringParam = "CampaignItemID=" + requestCampaignItemID;
                     }

                     if (ECN_Framework_BusinessLayer.Communicator.Blast.DynamicCotentExists(requestBlastID))
                     {
                         lnkISP.Enabled = true;
                         lnkDCTracking.NavigateUrl = "DCReports.aspx?blastID=" + requestBlastID.ToString();
                         pnlDCReport.Visible = true;
                     }
                     else
                     {
                         lnkISP.Enabled = false;
                         pnlDCReport.Visible = false;
                     }

                     if (!ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(Master.UserSession.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ISPReporting))
                     {
                         lnkISP.Enabled = false;
                         lnkISP.Attributes.Add("onclick", "buyISPFeature()");
                     }
                     else
                     {
                         if (getBlastID() > 0)
                         {
                             lnkISP.Enabled = true;
                             lnkISP.NavigateUrl = "ISPReports.aspx?" + qryStringParam;
                         }
                         else
                         {
                             lnkISP.Enabled = false;
                         }
                     }

                     ConversionTrkingSetupLNK.Text = "View / Setup ROI Values for this Message";
                     if (!ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(Master.UserSession.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ROIReporting))
                     {
                         lnkROITracking.Enabled = false;
                         lnkROITracking.Attributes.Add("onclick", "buyROIFeature()");
                         ROITrkingSetupLNK.Attributes.Add("onclick", "buyROIFeature()");
                     }
                     else
                     {
                         lnkROITracking.Enabled = true;
                         lnkROITracking.NavigateUrl = "reportsgraphical.aspx?" + qryStringParam;
                     }

                     ConversionTrkingSetupLNK.Text = "View / Setup Conversion Tracking links for this Message";
                     if (!ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(Master.UserSession.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ConversionTracking))
                     {
                         lnkConversionTracking.Enabled = false;
                         lnkConversionTracking.Attributes.Add("onclick", "buyConvTrackingFeature()");
                         ConversionTrkingSetupLNK.Attributes.Add("onclick", "buyConvTrackingFeature()");
                     }
                     else
                     {
                         if (requestBlastID > 0)
                         {
                             lnkConversionTracking.Enabled = true;
                             lnkConversionTracking.NavigateUrl = "conversionTracker.aspx?" + qryStringParam;
                         }
                     }

                     //if (KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID, "blastpriv") || KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID, "viewreport") || KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
                    
                         if (Page.IsPostBack == false)
                         {
                             if (requestBlastID > 0 || requestCampaignItemID > 0)
                             {
                                 if (requestBlastID > 0)
                                 {
                                     ECN_Framework_Entities.Communicator.Blast blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(requestBlastID, true);

                                     if (blast.BlastType.ToLower().Equals("sms"))
                                     {
                                         pnlEmail.Visible = false;
                                         pnlSMS.Visible = true;
                                         loadSMSReport(requestBlastID);
                                     }
                                     else
                                     {
                                         pnlEmail.Visible = true;
                                         pnlSMS.Visible = false;
                                     }
                                     LoadFormData(blast);
                                 }
                                 else
                                 {
                                     LoadCampaignItemFormData(requestCampaignItemID);
                                 }
                             }
                         }
                 }
                 else
                 {
                     throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
                 }
              }

        protected void downloadDeliveredEmails(object sender, System.EventArgs e)
        {
            string downloadType = ".xls";
            ArrayList columnHeadings = new ArrayList();
            IEnumerator aListEnum = null;
            string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + Master.UserSession.CurrentUser.CustomerID + "/downloads/");

            string newline = "";
            DateTime date = DateTime.Now;
            string filename = getBlastID() == 0 ? "CampaignItemID-" + getCampaignItemID().ToString() : "BlastID-" + getBlastID().ToString();
            String tfile = Master.UserSession.CurrentUser.CustomerID + "-" + filename + "-delivered" + downloadType;
            string outfileName = txtoutFilePath + tfile;

            if (!Directory.Exists(txtoutFilePath))
            {
                Directory.CreateDirectory(txtoutFilePath);
            }

            if (File.Exists(outfileName))
            {
                File.Delete(outfileName);
            }

            TextWriter txtfile = File.AppendText(outfileName);
            DataTable sends = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.DownloadBlastReportDetails(getBlastID() > 0 ? getBlastID() : getCampaignItemID(), getBlastID() > 0 ? false : true, "send", "", string.Empty, Master.UserSession.CurrentUser);
            DataTable bounces = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.DownloadBlastReportDetails(getBlastID() > 0 ? getBlastID() : getCampaignItemID(), getBlastID() > 0 ? false : true, "bounce", "", string.Empty, Master.UserSession.CurrentUser);


            var result = (from send in sends.AsEnumerable()
                          join bounce in bounces.AsEnumerable()
                          on send.Field<string>("EmailAddress") equals bounce.Field<string>("EmailAddress") into grp1
                          where !grp1.Any()
                          from bounce in grp1.DefaultIfEmpty()
                          select send).ToList();

            for (int i = 0; i < sends.Columns.Count; i++)
            {
                columnHeadings.Add(sends.Columns[i].ColumnName.ToString());
            }

            aListEnum = columnHeadings.GetEnumerator();
            while (aListEnum.MoveNext())
            {
                newline += aListEnum.Current.ToString() + (downloadType == ".xls" ? "\t" : ", ");
            }
            txtfile.WriteLine(newline);

            foreach (var dr in result)
            {
                newline = "";
                aListEnum.Reset();
                while (aListEnum.MoveNext())
                {
                    newline += dr[aListEnum.Current.ToString()].ToString() + (downloadType == ".xls" ? "\t" : ", ");
                }
                txtfile.WriteLine(newline);
            }

            txtfile.Close();
            if (downloadType == ".xls")
            {
                Response.ContentType = "application/vnd.ms-excel";
            }
            else
            {
                Response.ContentType = "text/csv";
            }
            Response.AddHeader("content-disposition", "attachment; filename=" + tfile);
            Response.WriteFile(outfileName);
            Response.Flush();
            Response.End();
        }

        private void loadSMSReport(int BlastID)
        {
            //Uncomment and add to new framework when SMS wizard is launched

            //SqlCommand cmd = new SqlCommand("select AttemptTotal from Blasts where BlastID=" + BlastID.ToString() + "");
            //DataTable dt = DataFunctions.GetDataTable("communicator", cmd);
            //lblsmsAttempt.Text = dt.Rows[0][0].ToString();

            //cmd = new SqlCommand("select Count(SAID) from SMSActivityLog where BlastID=" + BlastID.ToString() + "  and IsWelcomeMsg='False'");
            //dt = DataFunctions.GetDataTable("activity", cmd);
            //lblsmsSent.Text = dt.Rows[0][0].ToString();

            //cmd = new SqlCommand("select Count(SAID) from SMSActivityLog where BlastID=" + BlastID.ToString() + " and SendStatus='sent'  and IsWelcomeMsg='False'");
            //dt = DataFunctions.GetDataTable("activity", cmd);
            //lblsmsDelivered.Text = dt.Rows[0][0].ToString();

            //cmd = new SqlCommand("select Count(SAID) from SMSActivityLog where BlastID=" + BlastID.ToString() + " and SendStatus='failed'  and IsWelcomeMsg='False'");
            // dt = DataFunctions.GetDataTable("activity", cmd);
            //lblsmsOptOut.Text = dt.Rows[0][0].ToString();

            //cmd = new SqlCommand("select Count(SAID) from SMSActivityLog where BlastID=" + BlastID.ToString() + " and IsWelcomeMsg='True'");
            //dt = DataFunctions.GetDataTable("activity", cmd);
            //lblsmsWelcome.Text = dt.Rows[0][0].ToString();
        }

        public int getBlastID()
        {
            int theBlastID = 0;
            try
            {
                theBlastID = Convert.ToInt32(Request.QueryString["BlastID"].ToString());
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theBlastID;
        }

        public int getCampaignItemID()
        {
            int CampaignItemID = 0;
            try
            {
                CampaignItemID = Convert.ToInt32(Request.QueryString["CampaignItemID"].ToString());
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return CampaignItemID;
        }

        public string getUDFName()
        {
            try
            {
                return Request.QueryString["UDFName"].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public string getUDFData()
        {
            try
            {
                return Request.QueryString["UDFdata"].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }


        private Decimal loadTotalRevenueConversion(int BlastID)
        {
            Decimal revenueTotal = 0.0M;
            DataTable dt = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetRevenueConversionData(getBlastID(), "simple");
            revenueTotal = Convert.ToDecimal(dt.Rows[0][0].ToString());
            return revenueTotal;
        }

        #region Data Load
        private void LoadFormData(ECN_Framework_Entities.Communicator.Blast blast)
        {
            SocialGrid.DataSource = ECN_Framework_BusinessLayer.Activity.Report.SocialSummary.GetSocialSummaryByBlastID(blast.BlastID, Master.UserSession.CurrentCustomer.CustomerID);
            SocialGrid.DataBind();

            if (blast.TestBlast.ToUpper().Equals("N"))
            {
                var campaignItem = CampaignItem.GetByBlastID_NoAccessCheck(blast.BlastID, false);
                LoadSocialSimpleGrid(campaignItem.CampaignItemID);
            }

            string[] report_domains = new string[] { "msn.com", "aol.com", "excite.com", "yahoo.com" };
            string customerID = Master.UserSession.CurrentUser.CustomerID.ToString();
            lblCampaignFilters.Visible = false;
            if (blast.Group != null)
            {
                GroupTo.Text = blast.Group.GroupName;
                GroupTo.NavigateUrl = "/ecn.communicator.mvc/Subscriber/Index/" + blast.Group.GroupID;
                GroupTo1.Text = blast.Group.GroupName;
                GroupTo1.NavigateUrl = "/ecn.communicator.mvc/Subscriber/Index/" + blast.Group.GroupID;
            }
            else
            {
                GroupTo.Text = "";
                GroupTo.NavigateUrl = "";
                GroupTo1.Text = "";
                GroupTo1.NavigateUrl = "";
            }
            if (blast.TestBlast.ToLower().Equals("y"))
            {
                ECN_Framework_Entities.Communicator.CampaignItemTestBlast citb = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.GetByBlastID_NoAccessCheck(blast.BlastID, true);
                if (citb.Filters != null && blast.Group != null)
                {
                    gvFilters.DataSource = citb.Filters.Where(x => x.CampaignItemTestBlastID != null);
                    gvFilters.DataBind();
                }
            }
            else
            {
                ECN_Framework_Entities.Communicator.CampaignItemBlast cib = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByBlastID_NoAccessCheck(blast.BlastID,  true);
                if (cib.Filters != null && blast.Group != null)
                {
                    gvFilters.DataSource = cib.Filters.Where(x => x.CampaignItemBlastID != null);
                    gvFilters.DataBind();
                }
            }


            if (blast.Layout != null)
            {
                Campaign.Text = blast.Layout.LayoutName;
                Campaign.NavigateUrl = "../content/layouteditor.aspx?LayoutID=" + blast.Layout.LayoutID;
                Campaign1.Text = blast.Layout.LayoutName;
                Campaign1.NavigateUrl = "../content/layouteditor.aspx?LayoutID=" + blast.Layout.LayoutID;
            }
            string blastSuppressionList = blast.BlastSuppression;
            if (blast.IgnoreSuppression.Value)
            {
                SuppressionList.Text = "Transactional Emails - Suppression Not Applied";
            }
            else if (blast.BlastSuppression.Trim().Length > 0 && blast.Group != null)
            {
                ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByBlastID_NoAccessCheck(blast.BlastID,  false);
                List<ECN_Framework_Entities.Communicator.CampaignItemSuppression> ciSuppresssionList = ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.GetByCampaignItemID_NoAccessCheck(ci.CampaignItemID, true);
                foreach (ECN_Framework_Entities.Communicator.CampaignItemSuppression ciSuppresssion in ciSuppresssionList)
                {
                    ECN_Framework_Entities.Communicator.Group grp = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(ciSuppresssion.GroupID.Value);
                    if (ciSuppresssion.Filters != null && ciSuppresssion.Filters.Count() > 0)
                    {
                        foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in ciSuppresssion.Filters.OrderBy(x => x.SmartSegmentID))
                        {
                            if (cibf.SmartSegmentID != null)
                            {
                                ECN_Framework_Entities.Communicator.SmartSegment ss = ECN_Framework_BusinessLayer.Communicator.SmartSegment.GetSmartSegmentByID(cibf.SmartSegmentID.Value);
                                SuppressionList.Text = SuppressionList.Text + grp.GroupName + " Smart " + ss.SmartSegmentName + " " + cibf.RefBlastIDs + "<BR/>";
                            }
                            else if(cibf.FilterID != null)
                            {
                                ECN_Framework_Entities.Communicator.Filter f = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID_NoAccessCheck(cibf.FilterID.Value);
                                SuppressionList.Text = SuppressionList.Text + grp.GroupName + " Filter: " + f.FilterName + "<BR/>";
                            }
                        }

                        
                    }
                    else
                    {
                        SuppressionList.Text = SuppressionList.Text + grp.GroupName + "<BR/>";
                    }
                }
            }
            else
            {
                SuppressionList.Text = "";
            }

            if (lnkConversionTracking.Enabled)
            {
                ConversionTrkingSetupLNK.NavigateUrl = "../content/ConversionTrackingLinks.aspx?layoutID=" + blast.Layout.LayoutID;
            }
            if (lnkROITracking.Enabled)
            {
                if (blast.Layout != null)
                {
                    ROITrkingSetupLNK.Attributes.Add("onclick", "openROISetup('" + blast.Layout.LayoutID + "')");
                    ROITrkingSetupLNK.NavigateUrl = "javascript:void(0);";
                }
            }

            EmailSubject.Text = blast.EmailSubject;
            EmailFrom.Text = blast.EmailFromName + " &lt; " + blast.EmailFrom + " &gt;";
            SendTime.Text = blast.SendTime.ToString();
            FinishTime.Text = blast.FinishTime.ToString();
            SendTime1.Text = blast.SendTime.ToString();

            DataTable reportingDT = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetBlastReportData(blast.BlastID, getUDFName(), getUDFData());
            loadReportsData(reportingDT);
        }

        private void LoadCampaignItemFormData(int campaignItemID)
        {
            SocialGrid.DataSource = ECN_Framework_BusinessLayer.Activity.Report.SocialSummary.GetSocialSummaryByCampaignItemID(campaignItemID, Master.UserSession.CurrentCustomer.CustomerID);
            SocialGrid.DataBind();


            LoadSocialSimpleGrid(campaignItemID);

            string[] report_domains = new string[] { "msn.com", "aol.com", "excite.com", "yahoo.com" };
            int customerID = Master.UserSession.CurrentUser.CustomerID;

            DataTable grpNmDT = ECN_Framework_BusinessLayer.Communicator.Blast.GetGroupNamesByBlasts(getCampaignItemID(), customerID);
            lblCampaignFilters.Visible = true;
            int i = 0;
            foreach (DataRow dr in grpNmDT.Rows)
            {
                GroupTo.Text = i == 0 ? dr["Groups"].ToString() : GroupTo.Text + "<br/>" + dr["Groups"].ToString();
                Campaign.Text = dr["Layout"].ToString();


                if (lnkConversionTracking.Enabled)
                {
                    ConversionTrkingSetupLNK.NavigateUrl = "../content/ConversionTrackingLinks.aspx?layoutID=" + dr["LayoutID"].ToString();
                    ConversionTrkingSetupLNK.Attributes.Add("alt", "Setup Link Tracking / Link Alias");
                }

                if (lnkROITracking.Enabled)
                {
                    ROITrkingSetupLNK.Attributes.Add("onclick", "openROISetup('" + dr["LayoutID"].ToString() + "')");
                    ROITrkingSetupLNK.NavigateUrl = "javascript:void(0);";
                }

                EmailSubject.Text = dr["EmailSubject"].ToString();
                EmailFrom.Text = dr["EmailFromName"].ToString() + "<br>&lt; " + dr["EmailFrom"].ToString() + " &gt;";
                SendTime.Text = dr["SendTime"].ToString();
                FinishTime.Text = dr["FinishTime"].ToString();
                i++;
            }
            ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(campaignItemID, false);

            if (ci.IgnoreSuppression.Value)
            {
                SuppressionList.Text = "Transactional Emails - Suppression Not Applied";
            }
            else
            {

                List<ECN_Framework_Entities.Communicator.CampaignItemSuppression> ciSuppresssionList = ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.GetByCampaignItemID_NoAccessCheck(campaignItemID, true);
                if (ciSuppresssionList.Count > 0)
                {
                    foreach (ECN_Framework_Entities.Communicator.CampaignItemSuppression ciSuppresssion in ciSuppresssionList)
                    {
                        ECN_Framework_Entities.Communicator.Group grp = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(ciSuppresssion.GroupID.Value);
                        if (ciSuppresssion.Filters != null && ciSuppresssion.Filters.Count(x => x.SmartSegmentID != null) > 0)
                        {
                            foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in ciSuppresssion.Filters.OrderBy(x => x.SmartSegmentID))
                            {
                                if (cibf.SmartSegmentID != null)
                                {
                                    ECN_Framework_Entities.Communicator.SmartSegment ss = ECN_Framework_BusinessLayer.Communicator.SmartSegment.GetSmartSegmentByID(cibf.SmartSegmentID.Value);
                                    SuppressionList.Text = SuppressionList.Text + grp.GroupName + " Smart " + ss.SmartSegmentName + " " + cibf.RefBlastIDs + "<BR/>";
                                }
                                else if (cibf.FilterID != null)
                                {
                                    ECN_Framework_Entities.Communicator.Filter f = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID_NoAccessCheck(cibf.FilterID.Value);
                                    SuppressionList.Text = SuppressionList.Text + grp.GroupName + " Filter: " + f.FilterName + "<BR/>";
                                }
                            }

                        }
                        else
                        {
                            SuppressionList.Text = SuppressionList.Text + grp.GroupName + "<BR/>";
                        }
                    }
                }
                else
                {
                    SuppressionList.Text = "";
                }
            }


            //if(SuppressionList.Text.Length>0)
            //    SuppressionList.Text = SuppressionList.Text.Substring(0,SuppressionList.Text.Length-5);


            DataTable reportingDT = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetBlastReportDataByCampaignItemID(getCampaignItemID());
            loadReportsData(reportingDT);
        }

        

        private void loadReportsData(DataTable reportingDT)
        {
            Decimal SendTotal = 0;
            Decimal Success = 0;
            Decimal clicksuniquecount = 0.0M, clickstotalcount = 0.0M;
            Decimal noclickstotalcount = 0.0M;
            Decimal opensuniquecount = 0.0M, openstotalcount = 0.0M;
            Decimal noopenstotalcount = 0.0M;
            Decimal bouncesuniquecount = 0.0M, bouncestotalcount = 0.0M;
            Decimal subscribesuniquecount = 0.0M, subscribestotalcount = 0.0M;
            Decimal resendsuniquecount = 0.0M, resendstotalcount = 0.0M;
            Decimal forwardsuniquecount = 0.0M, forwardstotalcount = 0.0M;
            Decimal aolUnsubscribesuniquecount = 0.0M, aolUnsubscribestotalcount = 0.0M;
            Decimal abuseuniquecount = 0.0M, abusetotalcount = 0.0M;
            Decimal mastersuppressuniquecount = 0.0M, mastersuppresstotalcount = 0.0M;
            Decimal suppresseduniquecount = 0.0M, suppressedtotalcount = 0.0M;
            Decimal clickthroughcount = 0.0M;


            //check View Details permission 
                 
            bool hasViewDetails = KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReport, KMPlatform.Enums.Access.ViewDetails);
            bool hasSendsDetails = KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportSends, KMPlatform.Enums.Access.ViewDetails);
            bool hasResendsDetails = KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportResends, KMPlatform.Enums.Access.ViewDetails);
            bool hasUnopenedDetails = KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportUnopened, KMPlatform.Enums.Access.ViewDetails);
            bool hasNoClicksDetails = KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportNoClicks, KMPlatform.Enums.Access.ViewDetails);
            bool hasUnsubscribesDetails = KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportUnsubscribes, KMPlatform.Enums.Access.ViewDetails);
            bool hasSuppressedDetails = KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportSuppressed, KMPlatform.Enums.Access.ViewDetails);
            bool hasForwardDetails = KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportForwards, KMPlatform.Enums.Access.ViewDetails);
            bool hasClickThroughDetails = KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportClickThroughRatio, KMPlatform.Enums.Access.ViewDetails);

            //SENDS
            DataRow[] sendDR = reportingDT.Select("ActionTypeCode = 'send' OR ActionTypeCode = 'testsend'");
            try
            {
                foreach (DataRow sdr in sendDR)
                    SendTotal += Convert.ToDecimal(sdr[0]);
            }
            catch
            {
                SendTotal = 0;
            }

            //CLICKS
            DataRow[] clicksDR = reportingDT.Select("ActionTypeCode = 'click'");
            try { clicksuniquecount = Convert.ToDecimal(clicksDR[0][0]); }
            catch { clicksuniquecount = 0; }
            try {ClicksUnique.Text = (hasViewDetails ? "<a href='Clicks.aspx?" + qryStringParam + "'>" + clicksDR[0][0].ToString() + "</a>" : clicksDR[0][0].ToString()) ; }
            catch { ClicksUnique.Text = "0"; }
            
            try { clickstotalcount = Convert.ToDecimal(clicksDR[0][1]); }
            catch { clickstotalcount = 0; }
            try { ClicksTotal.Text = (hasViewDetails ? "<a href='Clicks.aspx?" + qryStringParam + "'>" + clicksDR[0][1].ToString() + "</a>": clicksDR[0][1].ToString());}
            catch { ClicksTotal.Text = "0"; }

            //ClickThrough Ratio
            DataRow[] clickThroughDR = reportingDT.Select("ActionTypeCode = 'clickthrough'");

            try { clickthroughcount = Convert.ToDecimal(clickThroughDR[0][0]); }
            catch { clicksuniquecount = 0; }
            try { ClickThrough.Text = (hasClickThroughDetails ? "<a href='ClickThrough.aspx?" + qryStringParam + "'>" + clickThroughDR[0][0].ToString() + "</a>" : clickThroughDR[0][0].ToString()); }
            catch { ClickThrough.Text = "0"; }

            //OPENS
            DataRow[] opensDR = reportingDT.Select("ActionTypeCode = 'open'");
            try { opensuniquecount = Convert.ToDecimal(opensDR[0][0]); }
            catch { opensuniquecount = 0; }
            try { OpensUnique.Text = (hasViewDetails ? "<a href='Opens.aspx?" + qryStringParam + "'>" + opensDR[0][0].ToString() + "</a>": opensDR[0][0].ToString()); }
            catch { OpensUnique.Text = "0"; }
            try { openstotalcount = Convert.ToDecimal(opensDR[0][1]); }
            catch { openstotalcount = 0; }
            try { OpensTotal.Text = (hasViewDetails ? "<a href='Opens.aspx?" + qryStringParam + "'>" + opensDR[0][1].ToString() + "</a>" : opensDR[0][1].ToString()); }
            catch { OpensTotal.Text = "0"; }

            //BOUNCES
            DataRow[] bouncesDR = reportingDT.Select("ActionTypeCode = 'bounce'");
            try { bouncesuniquecount = Convert.ToDecimal(bouncesDR[0][0]); }
            catch { bouncesuniquecount = 0; }
            try { BouncesUnique.Text =(hasViewDetails ? "<a href='bounces.aspx?" + qryStringParam + "'>" + bouncesDR[0][0].ToString() + "</a>": bouncesDR[0][0].ToString()) ; }
            catch { BouncesUnique.Text = "0"; }
            try { bouncestotalcount = Convert.ToDecimal(bouncesDR[0][1]); }
            catch { bouncestotalcount = 0; }
            try { BouncesTotal.Text = (hasViewDetails ? "<a href='bounces.aspx?" + qryStringParam + "'>" + bouncesDR[0][1].ToString() + "</a>": bouncesDR[0][1].ToString()); }
            catch { BouncesTotal.Text = "0"; }

            int bounce = Convert.ToInt32(bouncesuniquecount);
            Success = SendTotal - bounce;

            //RESENDS
            DataRow[] resendsDR = reportingDT.Select("ActionTypeCode = 'resend'");
            try { resendsuniquecount = Convert.ToDecimal(resendsDR[0][0]); }
            catch { resendsuniquecount = 0; }
            try { ResendsUnique.Text = (hasViewDetails && hasResendsDetails ? "<a href='resends.aspx?" + qryStringParam + "'>" + resendsDR[0][0].ToString() + "</a>" : resendsDR[0][0].ToString()); }
            catch { ResendsUnique.Text = "0"; }
            try { resendstotalcount = Convert.ToDecimal(resendsDR[0][1]); }
            catch { resendstotalcount = 0; }
            try { ResendsTotal.Text = (hasViewDetails && hasResendsDetails ? "<a href='resends.aspx?" + qryStringParam + "'>" + resendsDR[0][1].ToString() + "</a>" : resendsDR[0][1].ToString()); }
            catch { ResendsTotal.Text = "0"; }

            //FORWARDS
            DataRow[] forwardsDR = reportingDT.Select("ActionTypeCode = 'refer'");
            try { forwardsuniquecount = Convert.ToDecimal(forwardsDR[0][0]); }
            catch { forwardsuniquecount = 0; }
            try { ForwardsUnique.Text = (hasViewDetails && hasForwardDetails ? "<a href='forwards.aspx?" + qryStringParam + "'>" + forwardsDR[0][0].ToString() + "</a>" : forwardsDR[0][0].ToString()); }
            catch { ForwardsUnique.Text = "0"; }
            try { forwardstotalcount = Convert.ToDecimal(forwardsDR[0][1]); }
            catch { forwardstotalcount = 0; }
            try { ForwardsTotal.Text = (hasViewDetails && hasForwardDetails ? "<a href='forwards.aspx?" + qryStringParam + "'>" + forwardsDR[0][1].ToString() + "</a>" : forwardsDR[0][1].ToString()); }
            catch { ForwardsTotal.Text = "0"; }

            //UNSUBSCRIBES
            DataRow[] unsubsDR = reportingDT.Select("ActionTypeCode = 'subscribe'");
            try { subscribesuniquecount = Convert.ToDecimal(unsubsDR[0][0]); }
            catch { subscribesuniquecount = 0; }
            try { SubscribesUnique.Text = (hasViewDetails && hasUnsubscribesDetails ? "<a href='subscribes.aspx?OnlyUnique=1&" + qryStringParam + "'>" + unsubsDR[0][0].ToString() + "</a>" : unsubsDR[0][0].ToString()); }
            catch { SubscribesUnique.Text = "0"; }
            try { subscribestotalcount = Convert.ToDecimal(unsubsDR[0][1]); }
            catch { subscribestotalcount = 0; }
            try { SubscribesTotal.Text = (hasViewDetails && hasUnsubscribesDetails ? "<a href='subscribes.aspx?OnlyUnique=0&" + qryStringParam + "'>" + unsubsDR[0][1].ToString() + "</a>" : unsubsDR[0][1].ToString()); }
            catch { SubscribesTotal.Text = "0"; }

            //AOL FEEDBACK UNSUBSCRIBES
            DataRow[] aolUnsubsDR = reportingDT.Select("ActionTypeCode = 'FEEDBACK_UNSUB'");
            try { aolUnsubscribesuniquecount = Convert.ToDecimal(aolUnsubsDR[0][0]); }
            catch { aolUnsubscribesuniquecount = 0; }
            try { AOLFeedbackUnsubscribeUnique.Text = (hasViewDetails && hasUnsubscribesDetails ? "<a href='subscribes.aspx?OnlyUnique=1&" + qryStringParam + "&code=FEEDBACK_UNSUB'>" + aolUnsubsDR[0][0].ToString() + "</a>" : aolUnsubsDR[0][0].ToString()); }
            catch { AOLFeedbackUnsubscribeUnique.Text = "0"; }
            try { aolUnsubscribestotalcount = Convert.ToDecimal(aolUnsubsDR[0][1]); }
            catch { aolUnsubscribestotalcount = 0; }
            try { AOLFeedbackUnsubscribeTotal.Text = (hasViewDetails && hasUnsubscribesDetails ? "<a href='subscribes.aspx?OnlyUnique=0&" + qryStringParam + "&code=FEEDBACK_UNSUB'>" + aolUnsubsDR[0][1].ToString() + "</a>" : aolUnsubsDR[0][1].ToString()); }
            catch { AOLFeedbackUnsubscribeTotal.Text = "0"; }

            //ABUSE REPORTS UNSUBSCRIBES
            DataRow[] abuseUnsubsDR = reportingDT.Select("ActionTypeCode = 'ABUSERPT_UNSUB'");
            try { abuseuniquecount = Convert.ToDecimal(abuseUnsubsDR[0][0]); }
            catch { abuseuniquecount = 0; }
            try { AbuseUnique.Text = (hasViewDetails && hasUnsubscribesDetails ? "<a href='subscribes.aspx?OnlyUnique=1&" + qryStringParam + "&code=ABUSERPT_UNSUB'>" + abuseUnsubsDR[0][0].ToString() + "</a>" : abuseUnsubsDR[0][0].ToString()); }
            catch { AbuseUnique.Text = "0"; }
            try { abusetotalcount = Convert.ToDecimal(abuseUnsubsDR[0][1]); }
            catch { abusetotalcount = 0; }
            try { AbuseTotal.Text = (hasViewDetails && hasUnsubscribesDetails ? "<a href='subscribes.aspx?OnlyUnique=0&" + qryStringParam + "&code=ABUSERPT_UNSUB'>" + abuseUnsubsDR[0][1].ToString() + "</a>" : abuseUnsubsDR[0][1].ToString()); }
            catch { AbuseTotal.Text = "0"; }

            //MASTER SUPPRESSED UNSUBSCRIBES
            DataRow[] masterSuppressUnsubsDR = reportingDT.Select("ActionTypeCode = 'MASTSUP_UNSUB'");
            try { mastersuppressuniquecount = Convert.ToDecimal(masterSuppressUnsubsDR[0][0]); }
            catch { mastersuppressuniquecount = 0; }
            try { MasterSuppressUnique.Text = (hasViewDetails && hasUnsubscribesDetails ? "<a href='subscribes.aspx?OnlyUnique=1&" + qryStringParam + "&code=MASTSUP_UNSUB'>" + masterSuppressUnsubsDR[0][0].ToString() + "</a>" : masterSuppressUnsubsDR[0][0].ToString()); }
            catch { MasterSuppressUnique.Text = "0"; }
            try { mastersuppresstotalcount = Convert.ToDecimal(masterSuppressUnsubsDR[0][1]); }
            catch { mastersuppresstotalcount = 0; }
            try { MasterSuppressTotal.Text = (hasViewDetails && hasUnsubscribesDetails ? "<a href='subscribes.aspx?OnlyUnique=0&" + qryStringParam + "&code=MASTSUP_UNSUB'>" + masterSuppressUnsubsDR[0][1].ToString() + "</a>" : masterSuppressUnsubsDR[0][1].ToString()); }
            catch { MasterSuppressTotal.Text = "0"; }

            //SUPPRESSED
            string suppressedCodes = string.Empty;
            List<ECN_Framework_Entities.Activity.SuppressedCodes> codesList = ECN_Framework_BusinessLayer.Activity.SuppressedCodes.GetAll();
            foreach (ECN_Framework_Entities.Activity.SuppressedCodes codes in codesList)
            {
                suppressedCodes += "'" + codes.SupressedCode + "',";
            }
            if (suppressedCodes.Length > 0)
            {
                suppressedCodes = suppressedCodes.Remove(suppressedCodes.Length - 1, 1);
            }
            DataRow[] suppressedUnsubsDR = reportingDT.Select("ActionTypeCode in(" + suppressedCodes + ")");

            suppresseduniquecount = 0;
            for (int i = 0; i < suppressedUnsubsDR.Length; i++)
            {
                try { suppresseduniquecount += Convert.ToDecimal(suppressedUnsubsDR[i][0]); }
                catch { }
            }

            try { SuppressedUnique.Text = (hasViewDetails && hasSuppressedDetails? "<a href='suppressed.aspx?" + qryStringParam + "&value=ALL'>" + suppresseduniquecount.ToString() + "</a>" : suppresseduniquecount.ToString()); }
            catch { SuppressedUnique.Text = "0"; }

            suppressedtotalcount = 0;
            for (int i = 0; i < suppressedUnsubsDR.Length; i++)
            {
                try { suppressedtotalcount += Convert.ToDecimal(suppressedUnsubsDR[i][1]); }
                catch { }
            }
            //suppressedtotalcount = tempSuppressedCount1 + tempSuppressedCount2;
            try { SuppressedTotal.Text = (hasViewDetails && hasSuppressedDetails? "<a href='suppressed.aspx?" + qryStringParam + "&value=ALL'>" + suppressedtotalcount.ToString() + "</a>" : subscribestotalcount.ToString()); }
            catch { SuppressedTotal.Text = "0"; }

            //No Opens
            try { noopenstotalcount = Convert.ToDecimal(SendTotal - opensuniquecount); }
            catch { noopenstotalcount = 0; };
            try { NoOpenTotal.Text = (hasViewDetails && hasUnopenedDetails ? "<a href='noopens.aspx?" + qryStringParam + "'>" + Convert.ToString(Convert.ToDecimal(SendTotal - opensuniquecount)) + "</a>" : Convert.ToString(Convert.ToDecimal(SendTotal - opensuniquecount))) ; }
            catch { NoOpenTotal.Text = "0"; }

            //No Clicks
            DataRow[] unclicksDR = reportingDT.Select("ActionTypeCode = 'UNIQCLIQ'");
            decimal noClicksUniqueCount = 0.0M;
            try { noClicksUniqueCount = Convert.ToDecimal(unclicksDR[0][0]); }
            catch { noClicksUniqueCount = 0; }

            try { noclickstotalcount = Convert.ToDecimal(SendTotal - noClicksUniqueCount); }
            catch { noclickstotalcount = 0; };
            try { NoClickTotal.Text = (hasViewDetails && hasNoClicksDetails ? "<a href='noclicks.aspx?" + qryStringParam + "'>" + Convert.ToString(noclickstotalcount) + "</a>" : Convert.ToString(noclickstotalcount)); }
            catch { NoClickTotal.Text = "0"; }


            //Success = Success - bouncesuniquecount;
            SuccessfulDownload.Text = Success.ToString();
            Successful.Text = "/" + SendTotal.ToString();
            SendsUnique.Text = (hasViewDetails && hasSendsDetails ? "<a href='Sends.aspx?" + qryStringParam + "'>" + SendTotal.ToString() + "</a>" : SendTotal.ToString());
            SendsTotal.Text = (hasViewDetails && hasSendsDetails ? "<a href='Sends.aspx?" + qryStringParam + "'>" + SendTotal.ToString() + "</a>" : SendTotal.ToString());

            SuccessfulPercentage.Text = "(" + Decimal.Round(((SendTotal == 0 ? 0 : Success / SendTotal) * 100), 0).ToString() + "%)";

            try
            {
                if (clicksuniquecount > Success)
                    ClicksPercentage.Text = "100 %";
                else
                    ClicksPercentage.Text = Decimal.Round(((Success == 0 ? 0 : clicksuniquecount / Success) * 100), 0).ToString() + " %";
            }
            catch { }

            try
            {
                if(clickthroughcount >= Success)
                {
                    ClickThroughPercentage.Text = "100 %";
                }
                else
                {
                    ClickThroughPercentage.Text = Decimal.Round(((Success == 0 ? 0 : clickthroughcount / Success) * 100), 0).ToString() + " %";
                }
            }
            catch { }

            BouncesPercentage.Text = Decimal.Round(((SendTotal == 0 ? 0 : bouncesuniquecount / SendTotal) * 100), 0).ToString() + "%";
            OpensPercentage.Text = Decimal.Round(((Success == 0 ? 0 : opensuniquecount / Success) * 100), 0).ToString() + "%";
            SubscribesPercentage.Text = Decimal.Round(((Success == 0 ? 0 : subscribesuniquecount / Success) * 100), 0).ToString() + "%";
            ResendsPercentage.Text = Decimal.Round(((Success == 0 ? 0 : resendsuniquecount / Success) * 100), 0).ToString() + "%";
            ForwardsPercentage.Text = Decimal.Round(((Success == 0 ? 0 : forwardsuniquecount / Success) * 100), 0).ToString() + "%";
            AOLFeedbackUnsubscribePercentage.Text = Decimal.Round(((Success == 0 ? 0 : aolUnsubscribesuniquecount / Success) * 100), 0).ToString() + "%";
            AbusePercentage.Text = Decimal.Round(((Success == 0 ? 0 : abuseuniquecount / Success) * 100), 0).ToString() + "%";
            MasterSuppressPercentage.Text = Decimal.Round(((Success == 0 ? 0 : mastersuppressuniquecount / Success) * 100), 0).ToString() + "%";
            SuppressedPercentage.Text = "0";

            decimal _openPrcntTotal = Decimal.Round(((Success == 0 ? 0 : openstotalcount / Success) * 100), 0);
            decimal _clickPrcntTotal = Decimal.Round(((Success == 0 ? 0 : clickstotalcount / Success) * 100), 0);
            decimal _bncsPrcntTotal = Decimal.Round(((SendTotal == 0 ? 0 : bouncestotalcount / SendTotal) * 100), 0);
            decimal _subsPrcntTotal = Decimal.Round(((Success == 0 ? 0 : subscribestotalcount / Success) * 100), 0);
            decimal _resndsPrcntTotal = Decimal.Round(((Success == 0 ? 0 : resendstotalcount / Success) * 100), 0);
            decimal _fwdsPrcntTotal = Decimal.Round(((Success == 0 ? 0 : forwardstotalcount / Success) * 100), 0);
            decimal _aolunsubsPrcntTotal = Decimal.Round(((Success == 0 ? 0 : aolUnsubscribestotalcount / Success) * 100), 0);
            decimal _abusePrcntTotal = Decimal.Round(((Success == 0 ? 0 : abusetotalcount / Success) * 100), 0);
            decimal _msprssPrcntTotal = Decimal.Round(((Success == 0 ? 0 : mastersuppresstotalcount / Success) * 100), 0);
            decimal _noopenPrcntTotal = Decimal.Round(((SendTotal == 0 ? 0 : noopenstotalcount / SendTotal) * 100), 0);
            decimal _noclickPrcntTotal = Decimal.Round(((SendTotal == 0 ? 0 : noclickstotalcount / SendTotal) * 100), 0);

            OpensTotalPercentage.Text = ((_openPrcntTotal > 100) ? "100" : _openPrcntTotal.ToString()) + "%";
            ClicksTotalPercentage.Text = ((_clickPrcntTotal > 100) ? "100" : _clickPrcntTotal.ToString()) + "%";
            BouncesTotalPercentage.Text = ((_bncsPrcntTotal > 100) ? "100" : _bncsPrcntTotal.ToString()) + "%";
            SubscribesTotalPercentage.Text = ((_subsPrcntTotal > 100) ? "100" : _subsPrcntTotal.ToString()) + "%";
            ResendsTotalPercentage.Text = ((_resndsPrcntTotal > 100) ? "100" : _resndsPrcntTotal.ToString()) + "%";
            ForwardsTotalPercentage.Text = ((_fwdsPrcntTotal > 100) ? "100" : _fwdsPrcntTotal.ToString()) + "%";
            AOLFeedbackUnsubscribeTotalPercentage.Text = ((_aolunsubsPrcntTotal > 100) ? "100" : _aolunsubsPrcntTotal.ToString()) + "%";
            AbuseTotalPercentage.Text = ((_abusePrcntTotal > 100) ? "100" : _abusePrcntTotal.ToString()) + "%";
            MasterSuppressTotalPercentage.Text = ((_msprssPrcntTotal > 100) ? "100" : _msprssPrcntTotal.ToString()) + "%";
            NoOpenPercentage.Text = ((_noopenPrcntTotal > 100) ? "100" : _noopenPrcntTotal.ToString()) + "%";
            NoClickPercentage.Text = ((_noclickPrcntTotal > 100) ? "100" : _noclickPrcntTotal.ToString()) + "%";
            SuppressedTotalPercentage.Text = "0";
        }

        private void LoadSocialSimpleGrid(int campaignItemId)
        {
            ECN_Framework_Entities.Communicator.CampaignItem ci = CampaignItem.GetByCampaignItemID_NoAccessCheck(campaignItemId, true);
            List<ECN_Framework_Entities.Communicator.CampaignItemSocialMedia> cismList = CampaignItemSocialMedia.GetByCampaignItemID(ci.CampaignItemID);

            int ssmId = 0;
            List<SsmModel> ssmModelList = new List<SsmModel>();

            foreach (var cism in cismList)
            {
                if (!string.IsNullOrWhiteSpace(cism.SimpleShareDetailID.ToString()))
                {
                    ECN_Framework_Entities.Communicator.SocialMedia sm = ECN_Framework_BusinessLayer.Communicator.SocialMedia.GetSocialMediaByID(cism.SocialMediaID);

                    var ssmModel = new SsmModel
                    {
                        //ID = ssmId,
                        SocialMediaId = Convert.ToInt32(cism.CampaignItemSocialMediaID),
                        SocialMediaTypeId = sm.SocialMediaID,
                        Icon = ConfigurationManager.AppSettings["Image_DomainPath"] + sm.ReportImagePath,
                        Page = cism.PageID,
                    };

                    if (sm.SocialMediaID < 4)
                    {
                        if (cism != null)
                            ssmModel.Status = cism.Status;
                        else
                            ssmModel.Status = "N/A";
                    }

                    ssmModelList.Add(ssmModel);
                    ssmId++;

                    if (cism.SocialMediaAuthID != null)
                    {
                        var smAuthId = (int)cism.SocialMediaAuthID;
                        var socialMediaAuth = SocialMediaAuth.GetBySocialMediaAuthID(smAuthId);

                        switch (sm.SocialMediaID)
                        {
                            case 1:  //FaceBook
                            case 4:  //FaceBookPageLikes
                                List<SocialMediaHelper.FBAccount> fbPages = SocialMediaHelper.GetUserAccounts(socialMediaAuth.Access_Token);
                                SocialMediaHelper.FBAccount fbAccount = fbPages.FirstOrDefault(x => x.id == ssmModel.Page);
                                if (fbAccount != null)
                                {
                                    ssmModel.Page = fbAccount.name;
                                }
                                else
                                {
                                    ssmModel.Page = "Unable to get page name";
                                }
                                break;
                            case 2:
                                //twitter doesn't have pages/companies
                                break;
                            case 3:  //LinkedIN
                                List<SocialMediaHelper.LIAccount> liCompanies = SocialMediaHelper.GetLICompanies(socialMediaAuth.Access_Token);
                                SocialMediaHelper.LIAccount liAccount = liCompanies.FirstOrDefault(x => x.id == ssmModel.Page);
                                if (liAccount != null)
                                {
                                    ssmModel.Page = liAccount.name;
                                }
                                else
                                {
                                    ssmModel.Page = "Unable to get page name";
                                }
                                break;
                        }
                    }
                }

            }

            //Reorder display order
            foreach (var item in ssmModelList)
            {
                switch (item.SocialMediaTypeId)
                {
                    case 2:
                        item.SocialMediaTypeId = 3;
                        break;
                    case 3:
                        item.SocialMediaTypeId = 2;
                        break;
                }
            }

            SocialSimpleGrid.DataSource = ssmModelList.OrderBy(x => x.SocialMediaTypeId);
            SocialSimpleGrid.DataBind();
        }
        #endregion


        protected void SocialGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow dr = e.Row;
            if (dr.RowType == DataControlRowType.DataRow)
            {
                HyperLink hl = (HyperLink)e.Row.FindControl("hlMediaReporting");
                string txt = hl.Text;
                hl.Text = "<img src='" + ConfigurationManager.AppSettings["Image_DomainPath"] + txt + "' alt='Social Reporting for the Blast' border='0'>";
            }
        }

        private bool IsSMSBlast(int BlastID)
        {
            ECN_Framework_Entities.Communicator.BlastAbstract blast =
            ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(BlastID, false);

            if (blast.BlastType.ToLower().Equals("sms"))
                return true;
            else
                return false;
        }

        protected void SocialSimpleGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            bool allowRepost = true;
            GridViewRow dr = e.Row;
            if (dr.RowType == DataControlRowType.DataRow)
            {
                LinkButton linkButton = (LinkButton)e.Row.FindControl("lbStatus");
                string txt = linkButton.Text;

                ECN_Framework_Entities.Communicator.CampaignItemSocialMedia cism = ECN_Framework_BusinessLayer.Communicator.CampaignItemSocialMedia.GetByCampaignItemSocialMediaID(Convert.ToInt32(linkButton.CommandArgument));
                if (txt == ECN_Framework_Common.Objects.Communicator.Enums.SocialMediaStatusType.Sent.ToString())
                {
                    linkButton.Font.Bold = false;
                    linkButton.Font.Underline = false;
                    if (cism != null && cism.SocialMediaID != 1)
                    {
                        linkButton.Attributes["OnClick"] = "return false;";
                    }
                    linkButton.ForeColor = Color.Green;

                    linkButton.Text = "Successful";
                    linkButton.CommandName = ECN_Framework_Common.Objects.Communicator.Enums.SocialMediaStatusType.Sent.ToString();
                }
                if (txt == ECN_Framework_Common.Objects.Communicator.Enums.SocialMediaStatusType.Failed.ToString())
                {
                    linkButton.ForeColor = Color.Red;
                    linkButton.Font.Bold = false;
                    linkButton.Font.Underline = false;
                    linkButton.CssClass = "linkbuttonNoUnderlineFail";
                    linkButton.Text = "Failed (click to repost)";

                    if (cism.LastErrorCode.HasValue)
                    {
                        if (cism.LastErrorCode.Value.Equals(401))
                        {
                            linkButton.Text = "Failed";
                            linkButton.Attributes["OnClick"] = "return false;";
                        }
                        else
                        {
                            SocialMediaErrorCodes errorMsg = LookupErrorCode(cism.LastErrorCode.Value, 1);
                            if ((cism.SocialMediaID == 1 || cism.SocialMediaID == 4) && (errorMsg == null)) //Facebook
                            {
                                linkButton.Text = "Failed";
                                linkButton.Attributes["OnClick"] = "return false;";
                            }
                            if (cism.SocialMediaID == 3) //LinkedIn
                            {
                                errorMsg = LookupErrorCode(cism.LastErrorCode.Value, 3, false);
                                if (errorMsg != null)
                                {
                                    linkButton.Text = "Failed";
                                    linkButton.Attributes["OnClick"] = "return false;";
                                }
                            }
                        }
                    }
                    
                }
                if (txt == ECN_Framework_Common.Objects.Communicator.Enums.SocialMediaStatusType.Pending.ToString())
                {
                    linkButton.Font.Bold = false;
                    linkButton.Font.Underline = false;
                    linkButton.Attributes["OnClick"] = "return false;";
                }
            }
        }

        protected SocialMediaErrorCodes LookupErrorCode(int errorCode, int mediaType, bool repostCodes = true)
        {
            SocialMediaErrorCodes smErrorCode = ECN_Framework_BusinessLayer.Communicator.SocialMediaErrorCodes.GetByErrorCode(errorCode, mediaType,repostCodes);
            return smErrorCode;
        }

        public void SocialSimpleGrid_Command(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals(ECN_Framework_Common.Objects.Communicator.Enums.SocialMediaStatusType.Sent.ToString()))
            {
                //Response.Redirect("Simple.aspx?" + qryStringParam);                
                KM.Common.Entity.Encryption ec = KM.Common.Entity.Encryption.GetCurrentByApplicationID(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"]));
                StringBuilder reportPath = new StringBuilder();
                reportPath.Append("simple.aspx?");
                string queryString = qryStringParam + "&cism=" + Convert.ToInt32(e.CommandArgument).ToString();
                string encryptedQuery = System.Web.HttpUtility.UrlEncode(KM.Common.Encryption.Encrypt(queryString, ec));
                reportPath.Append(encryptedQuery);
                Response.Redirect(reportPath.ToString());
            }

            if (e.CommandName.Equals("ResendSocialSimpleBlast"))
            {
                ECN_Framework_Entities.Communicator.CampaignItemSocialMedia cism =
                    ECN_Framework_BusinessLayer.Communicator.CampaignItemSocialMedia.GetByCampaignItemSocialMediaID(
                        Convert.ToInt32(e.CommandArgument));

                cism.Status = ECN_Framework_Common.Objects.Communicator.Enums.SocialMediaStatusType.Pending.ToString();
                ECN_Framework_BusinessLayer.Communicator.CampaignItemSocialMedia.Save(cism);

                string Url = Request.RawUrl;
                Response.Redirect(Url);
            }
        }

        public class SsmModel // SimpleSocialMediaModel
        {
            public int SocialMediaId { get; set; }
            public int SocialMediaTypeId { get; set; }
            public string Icon { get; set; }
            public string Page { get; set; }
            public string Status { get; set; }
            public string StatusLink { get; set; }
        }

        protected void gvFilters_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf = (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter)e.Row.DataItem;
                HyperLink hlFilter = (HyperLink)e.Row.FindControl("hlFilterName");
                Label lblSS = (Label)e.Row.FindControl("lblSS");
                if (cibf.FilterID != null)
                {
                    hlFilter.Visible = true;
                    lblSS.Visible = false;
                    ECN_Framework_Entities.Communicator.Filter f = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID_NoAccessCheck(cibf.FilterID.Value);

                    hlFilter.Text = f.FilterName;
                    hlFilter.NavigateUrl = "/ecn.communicator.mvc/Filter/Edit/" + f.FilterID;
                }
                else if (cibf.SmartSegmentID != null)
                {
                    hlFilter.Visible = false;
                    lblSS.Visible = true;
                    string[] refBlasts = cibf.RefBlastIDs.Split(',');
                    ECN_Framework_Entities.Communicator.SmartSegment ss = ECN_Framework_BusinessLayer.Communicator.SmartSegment.GetSmartSegmentByID(cibf.SmartSegmentID.Value);
                    foreach (string s in refBlasts)
                    {
                        ECN_Framework_Entities.Communicator.Blast b = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(Convert.ToInt32(s),  false);
                        lblSS.Text += "Smart " + ss.SmartSegmentName + " [" + s + "] " + b.EmailSubject + "<br />";
                    }
                }
            }
        }
    }
}