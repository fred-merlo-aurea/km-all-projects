using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Linq;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Functions;

using Role = KM.Platform.User;

namespace ecn.communicator.listsmanager.addressverifier
{
	
	public partial class addressloader : ECN_Framework.WebPageHelper
    {
        delegate void HidePopup();
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
            ECNError ecnError = new ECNError(Enums.Entity.Email, Enums.Method.None, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }

        KMPlatform.Entity.User SessionCurrentUser { get { return Master.UserSession.CurrentUser;} }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            phError.Visible = false;
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.GROUPS; 
            Master.SubMenu = "clean emails";
            Master.Heading = "Groups > Clean Emails";
            Master.HelpContent = "<b>Group/List Hygiene</b><div id='par1'><ul><li>In the Group Field, select the group name you want to work with.</li><li>In the Validation Type, you have two options: Check Syntax or Delete Bad Records; </li><li>Choose the Check Syntax option first, then click <em>Verify</em>.</li><li>The page will refresh and show the number of emails marked as bad.</li><li>At this point you can change the Validation Type to Delete Bad Records.</li><li>Or, (Suggested method), Go back into the Groups List.</li><li>Click on the <em>pencil</em> of the group that you just tested.</li><li>In the Filter by section, select Marked As Bad Records, Click <em>Get Results</em>.</li><li>When the page refreshes, you can review each bad email address. Some will be incomplete and will need to be deleted (by clicking on the red X). Some however, will just have a misspelling, may have the .com missing, etc. Those situations can be easily repaired by clicking on the <em>pencil</em> and editing the profile. </li></ul></div>";
            Master.HelpTitle = "Groups Manager";
            HidePopup delGroupsLookupPopup = new HidePopup(GroupsLookupPopupHide);
            this.ctrlgroupsLookup1.hideGroupsLookupPopup = delGroupsLookupPopup;

            //KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID,  "grouppriv") || KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser)
            //if (Role.IsAdministratorOrHasUserPermission(SessionCurrentUser, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Manage_Groups))
            if (Role.HasAccess(SessionCurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.CleanEmails))
            {				
				if (Page.IsPostBack==false) 
                {
                    loadDD(Master.UserSession.CurrentUser.CustomerID);
				}
			}
            else
            {
				Response.Redirect("../default.aspx");				
			}
		}

		private void loadDD(int CustomerID)
        {
            List<ECN_Framework_Entities.Communicator.Group> groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetByCustomerID(CustomerID, Master.UserSession.CurrentUser);
            var result = (from src in groupList
                         orderby src.GroupName
                          select src).ToList() ;

            GroupID.DataSource = result;
            GroupID.DataTextField = "GroupName";
            GroupID.DataValueField="GroupID";
			GroupID.DataBind();          
			ValidationLevel.Items.Add(new ListItem("Check Syntax", "syntax"));			
			ValidationLevel.Items.Add(new ListItem("Delete Bad Records", "delete"));			
		}

		public void VerifyEmails(object sender, System.EventArgs e) 
        {	
			string vl		= ValidationLevel.SelectedItem.Value;
			string gid	= hfSelectGroupID.Value;

            //cause exception if no group has been selected and group id = 0
            if (Convert.ToInt32(gid) < 1)
            {
                throwECNException("Please select a Group.");
            }
            else
            {
                int bademails = 0;
                Server.ScriptTimeout = 360;

                if (vl == "syntax")
                {
                    bademails = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ValidateEmails(Convert.ToInt32(gid), Master.UserSession.CurrentUser.UserID);
                    ShowResponse.Text = "<span class=errormsg>Process complete. " + bademails.ToString() + " emails marked as bad</span>";
                }

                if (vl == "delete")
                {
                    bademails = ECN_Framework_BusinessLayer.Communicator.EmailGroup.DeleteBadEmails(Convert.ToInt32(gid), Master.UserSession.CurrentUser.UserID, Master.UserSession.CurrentUser);
                    ShowResponse.Text = "<span class=errormsg>Process Complete. " + bademails.ToString() + " emails deleted</span>";
                }			
            }
		}

        protected void imgSelectGroup_Click(object sender, ImageClickEventArgs e)
        {
            hfGroupSelectionMode.Value = "SelectGroup";
            ctrlgroupsLookup1.LoadControl();
            ctrlgroupsLookup1.Visible = true;
        }

        protected override bool OnBubbleEvent(object sender, EventArgs e)
        {
            try
            {
                string source = sender.ToString();
                if (source.Equals("GroupSelected"))
                {
                    int groupID = ctrlgroupsLookup1.selectedGroupID;
                    ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(groupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    if (hfGroupSelectionMode.Value.Equals("SelectGroup"))
                    {
                        lblSelectGroupName.Text = group.GroupName;
                        hfSelectGroupID.Value = groupID.ToString();
                    }
                    else
                    {
                        //noop
                    }
                    ctrlgroupsLookup1.Visible = false;
                }
            }
            catch
            {
                //noop
            }
            return true;
        }

        private void GroupsLookupPopupHide()
        {
            ctrlgroupsLookup1.Visible = false;
        }

        protected void GroupChoice_CheckedChanged(object sender, EventArgs e)
        {
            imgSelectGroup.Enabled = rbGroupChoice1.Checked;
            lblSelectGroupName.Text = "-No Group Selected-";
            hfSelectGroupID.Value = "0";

            if(rbGroupChoice2.Checked)
            {
                ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetMasterSuppressionGroup(Convert.ToInt32(Master.UserSession.CurrentUser.CustomerID), Master.UserSession.CurrentUser);
                hfSelectGroupID.Value = Convert.ToString(group.GroupID);
            }
        }


	}
}
