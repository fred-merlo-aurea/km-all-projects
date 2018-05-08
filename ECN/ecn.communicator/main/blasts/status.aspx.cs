using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Linq;

namespace ecn.communicator.blastsmanager
{
    public partial class status : ECN_Framework.WebPageHelper
    {
        protected System.Web.UI.WebControls.Label FinishTime;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.BLASTS;
            Master.SubMenu = "";
            Master.Heading = "Blast Status";
            Master.HelpContent = "<img align='right' src=/ecn.images/images/icoblasts.gif><b>Reports</b><br />Gives a report of the Blast in progress.<br />";
            Master.HelpTitle = "Blast Manager";

            //if (KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID, "blastpriv") || KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Blast, KMPlatform.Enums.Access.View)
                )	
            {
                if (Page.IsPostBack == false)
                {
                    int requestBlastID = getBlastID();
                    if (requestBlastID > 0)
                    {
                        LoadFormData(requestBlastID);
                    }
                }
            }
            else
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }
        }

        public int getBlastID()
        {
            if (Request.QueryString["BlastID"] != null)
            {
                return Convert.ToInt32(Request.QueryString["BlastID"].ToString());
            }
            else
                return 0;
        }

        #region Data Load
        private void LoadFormData(int setBlastID)
        {
            ECN_Framework_Entities.Communicator.Blast blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(setBlastID, Master.UserSession.CurrentUser, true);

            double SendTotal = 0;
            double Success = 0;
            int PercentFinished = 0;
            DateTime started = DateTime.Now;
            double sendrate = 0;
            double timeleft = 0;
            TimeSpan elapsed;


            object groupname = blast.Group.GroupName;
            if (groupname != null)
            {
                GroupTo.Text = groupname.ToString();
                GroupTo.NavigateUrl = "../lists/groupeditor.aspx?GroupID=" + blast.GroupID;
            }
            else
            {
                GroupTo.Text = "<font color=darkred>&lt;deleted&gt;</font>";
            }

            object campaignname = blast.Layout.LayoutName;
            if (campaignname != null)
            {
                Campaign.Text = campaignname.ToString();
                Campaign.NavigateUrl = "../content/layouteditor.aspx?LayoutID=" + blast.LayoutID ;
            }
            else
            {
                Campaign.Text = "<font color=darkred>&lt;deleted&gt;</font>";
            }

            EmailSubject.Text = blast.EmailSubject;
            EmailFrom.Text = blast.EmailFromName + " &lt; " + blast.EmailFrom + " &gt;";
            started = blast.SendTime.Value;
            Success = blast.SuccessTotal.Value;
            SendTotal = blast.SendTotal.Value;
            SendTime.Text = blast.SendTime.Value.ToString();
            
               
            if (Success == 0 || SendTotal == 0)
            {
                PercentFinished = 0;
            }
            else
            {
                PercentFinished = Convert.ToInt32((Success / SendTotal) * 100);
            }
            if (PercentFinished == 100)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Redit", "alert('This blast has been sent'); window.location='../ecnwizard/default.aspx';", true);
            }

            Successful.Text = "(" + PercentFinished + "%)  " + Success.ToString() + "/" + SendTotal.ToString();
            SuccessBar.Percent = PercentFinished;
            SuccessBar.ToolTip = "" + PercentFinished + "%";

            SendTime.Text = started.ToString();

            if (Success != 0)
            {
                elapsed = DateTime.Now - started;
                sendrate = System.Math.Round(Success / elapsed.TotalSeconds);
                timeleft = (SendTotal - Success) / sendrate;
                string ratetext = "";
                if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
                {
                    ratetext = "~(" + sendrate + " emails/sec)";
                }
                if (timeleft < 60)
                {
                    Remaining.Text = System.Math.Round(timeleft) + " seconds " + ratetext;
                }
                else if (timeleft > 3600)
                {
                    Remaining.Text = System.Math.Round(timeleft / 3600) + " hours " + ratetext;
                }
                else
                {
                    Remaining.Text = System.Math.Round(timeleft / 60) + " minutes " + ratetext;
                }
            }
            else
            {
                Remaining.Text = "N/A";
            }
            ECN_Framework_Entities.Communicator.CampaignItemBlast cib = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByBlastID(blast.BlastID, Master.UserSession.CurrentUser, true);
            if (cib.Filters != null && cib.Filters.Count > 0)
            {
                lblNoFilter.Visible = false;
                gvFilters.Visible = true;
                gvFilters.DataSource = cib.Filters.Where(x => x.FilterID != null).ToList();
                gvFilters.DataBind();
            }
            else
            {
                gvFilters.Visible = false;
                lblNoFilter.Visible = true;
            }
        }
        #endregion

        protected void gvFilters_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf = (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter)e.Row.DataItem;
                HyperLink hlFilter = (HyperLink)e.Row.FindControl("hlFilterName");
                ECN_Framework_Entities.Communicator.Filter f = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID(cibf.FilterID.Value, Master.UserSession.CurrentUser);
                hlFilter.NavigateUrl = "../lists/filters.aspx?FilterID=" + f.FilterID;
                hlFilter.Text = f.FilterName;
            }
        }
    }
}