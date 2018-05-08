using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web.UI.WebControls;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.contentmanager
{
    public partial class layouteditor : ECN_Framework.WebPageHelper
    {
        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }
        private void setECNWarning(ECN_Framework_Common.Objects.ECNWarning ecnException, bool bVisible = true)
        {
            phWarning.Visible = bVisible;
            lblWarningMessage.Text = string.Empty;
            foreach (ECN_Framework_Common.Objects.ECNWarning ecnError in ecnException.ErrorList)
            {
                lblWarningMessage.Text = lblWarningMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.WarningMessage;
            }
        }
        private int getLayoutID()
        {
            int layoutID = 0;
            try
            {
                if (Request.QueryString["LayoutID"] != null)
                    layoutID = Convert.ToInt32(Request.QueryString["LayoutID"].ToString());
            }
            catch (Exception)
            {
            }
            return layoutID;
        }
        
        
        [WebMethod(EnableSession = true)]
        public static void RemovePageSession()
        {
            HttpContext.Current.Session.Remove("templaterepeater_LastSelectedValue");
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            phError.Visible = false;
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.CONTENT; ;
            Master.SubMenu = "new message";
            if(getLayoutID() > 0)
                Master.Heading = "Content/Messages > Edit Message";
            else
                Master.Heading = "Content/Messages > Create Message";
            Master.HelpContent = "<b>Building a Message</b><br/><div id='par1'><ul><li>Enter a Name to describe your message in the Name field.</li><li>Select a <em>wireframe template</em> (the pieces of Content you select will be placed in the numbered areas).</li>&#13;&#10;<li>If you desire a border, click on the <em>Yes</em> radial. Note: borders within a message may trigger the message as spam.</li><li>The address information will auto-fill from your profile information.<br /><em class='note'>Note: Your address is REQUIRED for anti-spam compliance.</em></li>&#13;&#10;<li>Use the drop down lists to choose Content Pieces that coordinate with which Slot you would like to put them in.</li><li>Click <em>Create</em>.</li></ul></div>";
            Master.HelpTitle = "Content Manager";

            //if (!ECN_Framework_BusinessLayer.Communicator.Layout.HasPermission(KMPlatform.Enums.Access.Edit, Master.UserSession.CurrentUser))
            if (!Page.IsPostBack && !KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Content, KMPlatform.Enums.Access.Edit))
            {
                throw new SecurityException();
            }
        }

        public void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                Session.Remove("templaterepeater_LastSelectedValue");
                layoutEditor1.SaveLayout();
                Response.Redirect("defaultMsg.aspx");
            }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                setECNError(ex);
            }
            catch (ECN_Framework_Common.Objects.ECNWarning ex)
            {
                setECNWarning(ex);
                Response.AddHeader("REFRESH", "1;URL=defaultMsg.aspx");
            }
        }       
    }
}