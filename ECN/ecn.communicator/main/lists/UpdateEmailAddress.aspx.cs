using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.lists
{
    public partial class UpdateEmailAddress : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            phError.Visible = false;
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.GROUPS;
            Master.SubMenu = "update emails";
            Master.Heading = "Update Email Address for the Base Channel";
            //Master.Heading = "Manage Content & Messages";
            Master.HelpContent = "<b>Updating Email Address:</b><br/><div id='par1'>Update an Email Address to a new Email Address across the Base Channel.</div>";
            Master.HelpTitle = "Update Emails";
            //if(KM.Platform.User.IsChannelAdministrator(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser))
            if(KM.Platform.User.IsChannelAdministrator(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser))
            {
               
            }
            else
            {
                throw new SecurityException();
            }

           
        }

        protected void btnUpdateEmail_Click(object sender, EventArgs e)
        {
            if(ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(txtOldEmail.Text.Trim()))
            {
                if(ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(txtNewEmail.Text.Trim()))
                {
                    if (!isEmailSuppressed(txtNewEmail.Text.Trim()))
                    {                        
                        try
                        {
                            ECN_Framework_BusinessLayer.Communicator.Email.UpdateEmail_BaseChannel(txtOldEmail.Text.Trim(), txtNewEmail.Text.Trim(), Master.UserSession.CurrentBaseChannel.BaseChannelID, Master.UserSession.CurrentUser.UserID, "ecn.communicator.UpdateEmailAddress");
                            txtNewEmail.Text = "";
                            txtOldEmail.Text = "";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Redit", "alert('Email Address updated');", true);
                        }
                        catch (ECNException ecn)
                        {
                            setECNError(ecn);
                        }                        
                    }
                }
                else
                {
                    throwECNException("New Email Address is not a valid Email");
                }
            }
            else
            {
                throwECNException("Old Email Address is not a valid Email");
            }
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

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.Email, Enums.Method.Validate, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }

        private bool isEmailSuppressed(string email)
        {
            ECN_Framework_Entities.Communicator.Group MSGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetMasterSuppressionGroup(Master.UserSession.CurrentUser.CustomerID, Master.UserSession.CurrentUser);
            ECN_Framework_Entities.Communicator.EmailGroup msEmailGroup = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailAddressGroupID(email, MSGroup.GroupID, Master.UserSession.CurrentUser);
            if (msEmailGroup != null && msEmailGroup.CustomerID.HasValue)
            {
                throwECNException("New email address is Master Suppressed. Updating is not allowed.");
                return true;
            }

            List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList> channelMasterSuppressionList_List =
                ECN_Framework_BusinessLayer.Communicator.ChannelMasterSuppressionList.GetByEmailAddress(Master.UserSession.CurrentBaseChannel.BaseChannelID, email.Replace("'", "''"), Master.UserSession.CurrentUser);
            if (channelMasterSuppressionList_List.Count > 0)
            {
                throwECNException("New email address is Channel Master Suppressed. Updating is not allowed.");
                return true;
            }

            List<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList> globalMasterSuppressionList_List =
                ECN_Framework_BusinessLayer.Communicator.GlobalMasterSuppressionList.GetByEmailAddress(email.Replace("'", "''"), Master.UserSession.CurrentUser);
            if (globalMasterSuppressionList_List.Count > 0)
            {
                throwECNException("New email address is Global Master Suppressed. Updating is not allowed.");
                return true;
            }

            return false;
        }
    }
}