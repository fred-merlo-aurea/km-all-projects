using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Text;
using System.Data;
using System.IO;
using System.Configuration;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Functions;

namespace ecn.communicator.main.lists
{
    public partial class domainTrackerList : System.Web.UI.Page
    {
        private static string DomainTrackerID;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (KMPlatform.BusinessLogic.ClientGroup.HasServiceFeature(Master.UserSession.ClientGroupID, KMPlatform.Enums.Services.DOMAINTRACKING, KMPlatform.Enums.ServiceFeatures.DomainSetup))
                {
                    loadDomains();
                }
                else
                {
                    throw new SecurityException();
                }
            }
        }

        private void loadDomains()
        {
            List<ECN_Framework_Entities.DomainTracker.DomainTracker> domainTrackerList = ECN_Framework_BusinessLayer.DomainTracker.DomainTracker.GetByBaseChannelID(Master.UserSession.CurrentBaseChannel.BaseChannelID, Master.UserSession.CurrentUser);
            gvDomainTracker.DataSource = domainTrackerList;
            gvDomainTracker.DataBind();
        }

        protected void gvDomainTracker_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DomainTrackerID = e.CommandArgument.ToString();
            if (e.CommandName == "DomainDelete")
            {
                ECN_Framework_BusinessLayer.DomainTracker.DomainTracker.Delete(Convert.ToInt32(DomainTrackerID), Master.UserSession.CurrentUser);
                loadDomains();
            }
            else if (e.CommandName == "GenerateScript")
            {
                rblJQuery.SelectedIndex = 0;
                rblProtocol.SelectedIndex = 0;
                string script = GenerateScript(DomainTrackerID);
                txtDomainScript.Text = script;
                mpeGenerateScript.Show();

            }
        }

        protected void GVDomainTracker_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            KMPlatform.Entity.User currentUser = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;
            if (!KM.Platform.User.IsSystemAdministrator(currentUser) && !KM.Platform.User.IsChannelAdministrator(currentUser) && e.Row.RowType == DataControlRowType.DataRow && gvDomainTracker.Rows.Count > 0)
            {
                ((DataControlField)gvDomainTracker.Columns.Cast<DataControlField>().Where(fld => fld.HeaderText == "Delete").SingleOrDefault()).Visible = false;

                ((DataControlField)gvDomainTracker.Columns.Cast<DataControlField>().Where(fld => fld.HeaderText == "Edit").SingleOrDefault()).Visible = false;
            }
        }
        private string GenerateScript(string trackerID)
        {
            string webServicesURL = ConfigurationManager.AppSettings["webServicesURL"].ToString();
            string script = "";
            if (rblJQuery.SelectedValue.ToLower().Equals("with"))
            {
                script = "<script type=\"text/javascript\"> var TrackerKey = \"" + trackerID + "\"; var url = \"" + webServicesURL.ToString() + "\"; var script = document.createElement(\"script\"); script.setAttribute(\"src\", url); script.setAttribute(\"type\", \"text/javascript\"); document.body.appendChild(script);</script>";
            }
            else
            {
                string getJQueryURL = ConfigurationManager.AppSettings["jQueryURL"].ToString();
                script = "<script type='text/javascript' src='" + getJQueryURL.ToString() + "'></script><script type=\"text/javascript\"> var TrackerKey = \"" + trackerID + "\"; var url = \"" + webServicesURL.ToString() + "\";     var script = document.createElement(\"script\"); script.setAttribute(\"src\", url); script.setAttribute(\"type\", \"text/javascript\"); document.body.appendChild(script);</script>";
            }

            if (!rblProtocol.SelectedValue.ToLower().Equals("http"))
            {
                script = script.Replace("http", "https");
            }

            return script;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("domainTrackeredit.aspx");
        }

        protected void btnCloseScript_Click(object sender, EventArgs e)
        {
            mpeGenerateScript.Hide();
        }

        protected void rblProtocol_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDomainScript.Text = GenerateScript(DomainTrackerID);
            mpeGenerateScript.Show();
        }

        protected void rblJQuery_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDomainScript.Text = GenerateScript(DomainTrackerID);
            mpeGenerateScript.Show();
        }


    }
}