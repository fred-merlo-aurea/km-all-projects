using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.lists
{
    public partial class mergeProfiles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            phError.Visible = false;
            if (!IsPostBack)
            {
                try
                {
                    Master.Heading = "Merge Profiles";

                    int emailID_old = Convert.ToInt32(Request.QueryString["oldemailid"].ToString());
                    ECN_Framework_Entities.Communicator.Email email_old = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID(emailID_old, Master.UserSession.CurrentUser);
                    var oldList = new List<ECN_Framework_Entities.Communicator.Email> { email_old };
                    dvEmailAddressOld.DataSource = oldList;
                    dvEmailAddressOld.DataBind();

                    int emailID_new = Convert.ToInt32(Request.QueryString["newemailid"].ToString());
                    ECN_Framework_Entities.Communicator.Email email_new = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID(emailID_new, Master.UserSession.CurrentUser);
                    var newList = new List<ECN_Framework_Entities.Communicator.Email> { email_new };
                    dvEmailAddressNew.DataSource = newList;
                    dvEmailAddressNew.DataBind();
                }
                catch (ECN_Framework_Common.Objects.ECNException ex)
                {
                    setECNError(ex);
                }
                catch (Exception exc)
                {
                    KM.Common.Entity.ApplicationLog.LogNonCriticalError(exc, "ecn.communicator.main.lists.MergeProfiles.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
                    Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["Accounts_VirtualPath"] + "/error.aspx", true);
                }
            }
        }

        protected void btnprofileOld_Click(object sender, EventArgs e)
        {
            ECN_Framework_BusinessLayer.Communicator.Email.MergeProfiles(Convert.ToInt32(Request.QueryString["newemailid"].ToString()), Convert.ToInt32(Request.QueryString["oldemailid"].ToString()), Master.UserSession.CurrentUser);
            Response.Redirect("default.aspx");
        }

        protected void btnprofileNew_Click(object sender, EventArgs e)
        {
            ECN_Framework_BusinessLayer.Communicator.Email.MergeProfiles(Convert.ToInt32(Request.QueryString["oldemailid"].ToString()), Convert.ToInt32(Request.QueryString["newemailid"].ToString()), Master.UserSession.CurrentUser);
            Response.Redirect("default.aspx");
        }


        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

    }
}