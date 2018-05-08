using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Linq;
using System.Transactions;
using System.Collections.Generic;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.listsmanager 
{	
	public partial class customdefinedfieldeditor : ECN_Framework.WebPageHelper 
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

        protected void Page_Load(object sender, System.EventArgs e)
        {
            phError.Visible = false;
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.GROUPS; 
            Master.SubMenu = "";
            Master.Heading = "Groups > Manage Groups > User Defined Fields > Edit";
            Master.HelpContent = "<img align='right' src=/ecn.images/images/icogroups.gif><b>Edit Email Data Fields</b> <br />Here you can edit your Defined Email Data Fields";
            Master.HelpTitle = "Email Data Fields Manager";
            isPublicChkbox.Enabled = ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(Master.UserSession.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailProfilePreferences);

            if(!IsPostBack)
            {
                LoadBoxes(Convert.ToInt32(Request.QueryString["GroupDatafieldsID"].ToString()), Convert.ToInt32(Request.QueryString["GroupID"].ToString()));
            }

            if (Request.QueryString["delete"] != null)
            {
                try
                {
                    DeleteFieldData(Convert.ToInt32(Request.QueryString["GroupDatafieldsID"].ToString()),  Convert.ToInt32(Request.QueryString["GroupID"].ToString()));
                    Response.Redirect("customerdefinedfields.aspx?GroupID=" + Request.QueryString["GroupID"].ToString(), true);
                }
                catch (ECNException ex)
                {
                    setECNError(ex);
                }
            }
		}

        private void LoadBoxes(int groupDataFieldId, int groupID) 
        {
            ECN_Framework_Entities.Communicator.GroupDataFields groupDataFeilds=
            ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByID(groupDataFieldId, groupID, Master.UserSession.CurrentUser);

            short_name.Text = groupDataFeilds.ShortName;
            long_name.Text = groupDataFeilds.LongName;
            if (groupDataFeilds.IsPublic.Equals("Y"))
            {
				isPublicChkbox.Checked = true;
			}
            else
            {
				isPublicChkbox.Checked = false;			
			}

            if (groupDataFeilds.DatafieldSetID.HasValue && groupDataFeilds.DatafieldSetID.Value > 0)
            {
                pnlWholeDefaultValue.Visible = true;
                ECN_Framework_Entities.Communicator.GroupDataFieldsDefault gdfd = ECN_Framework_BusinessLayer.Communicator.GroupDataFieldsDefault.GetByGDFID(groupDataFieldId);
                if (gdfd != null && gdfd.GDFID > 0)
                {
                    chkUseDefaultValue.Checked = true;
                    pnlDefaultValue.Visible = true;
                    if (!string.IsNullOrEmpty(gdfd.DataValue))
                    {
                        ddlDefaultType.SelectedValue = "default";
                        txtDefaultValue.Visible = true;
                        txtDefaultValue.Text = gdfd.DataValue;
                        ddlSystemValues.Visible = false;
                    }
                    else
                    {
                        ddlDefaultType.SelectedValue = "system";
                        ddlSystemValues.Visible = true;
                        ddlSystemValues.SelectedValue = gdfd.SystemValue;
                        txtDefaultValue.Visible = false;
                    }
                }
            }
            else
            {
                pnlWholeDefaultValue.Visible = false;
                chkUseDefaultValue.Checked = false;
                pnlDefaultValue.Visible = false;
            }
        }

        private void DeleteFieldData(int groupDataFieldId, int groupID) 
        {
            using (TransactionScope scope = new TransactionScope())
            {                
                ECN_Framework_BusinessLayer.Communicator.EmailDataValues.Delete(groupDataFieldId, Master.UserSession.CurrentUser);
                ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Delete(groupDataFieldId, groupID, Master.UserSession.CurrentUser);
                ECN_Framework_BusinessLayer.Communicator.GroupDataFieldsDefault.Delete(groupDataFieldId);
                scope.Complete();
            }
        }

        private void UpdateFieldData(int GroupDatafieldsID, int groupID) 
        {
			string shrtNm	= StringFunctions.CleanString(short_name.Text);
			shrtNm			= shrtNm.Replace("'", "");
			shrtNm			= shrtNm.Replace(" ", "_");
			string longNm	= StringFunctions.CleanString(long_name.Text);
			string isPublic	= isPublicChkbox.Checked?"Y":"N";
            ECN_Framework_Entities.Communicator.GroupDataFields groupDataFields=
            ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByID(GroupDatafieldsID, groupID, Master.UserSession.CurrentUser);
            groupDataFields.ShortName = shrtNm;
            groupDataFields.LongName = longNm;
            groupDataFields.IsPublic = isPublic;
            groupDataFields.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
            try
            {
                
                if (chkUseDefaultValue.Checked)
                {
                    ECN_Framework_Entities.Communicator.GroupDataFieldsDefault gdfd = ECN_Framework_BusinessLayer.Communicator.GroupDataFieldsDefault.GetByGDFID(GroupDatafieldsID);
                    if (gdfd == null)
                        gdfd = new ECN_Framework_Entities.Communicator.GroupDataFieldsDefault();

                    gdfd.GDFID = GroupDatafieldsID;
                    if (ddlDefaultType.SelectedValue.ToString().ToLower().Equals("default"))
                    {
                        if (txtDefaultValue.Text.Trim().Length > 0)
                        {
                            gdfd.DataValue = txtDefaultValue.Text.Trim();
                            gdfd.SystemValue = "";
                        }
                        else
                        {
                            
                            setECNError(new ECN_Framework_Common.Objects.ECNException(new List<ECNError>() { new ECNError(Enums.Entity.GroupDataFieldsDefault, Enums.Method.Validate, "Please enter a value for Default Value") }));
                            return;
                        }
                    }
                    else
                    {
                        gdfd.DataValue = "";
                        gdfd.SystemValue = ddlSystemValues.SelectedValue.ToString();
                    }
                    ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Save(groupDataFields, Master.UserSession.CurrentUser);
                    ECN_Framework_BusinessLayer.Communicator.GroupDataFieldsDefault.Save(gdfd);
                }
                else
                {
                    ECN_Framework_BusinessLayer.Communicator.GroupDataFieldsDefault.Delete(GroupDatafieldsID);
                    ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Save(groupDataFields, Master.UserSession.CurrentUser);
                }
                Response.Redirect("customerdefinedfields.aspx?GroupID=" + groupID, true);
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }        

            
        }

        protected void chkUseDefaultValue_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseDefaultValue.Checked)
            {
                pnlDefaultValue.Visible = true;
                if (ddlDefaultType.SelectedValue.ToString().ToLower().Equals("default"))
                {
                    txtDefaultValue.Visible = true;
                    ddlSystemValues.Visible = false;
                }
                else
                {
                    txtDefaultValue.Visible = false;
                    ddlSystemValues.Visible = true;
                }
            }
            else
            {
                pnlDefaultValue.Visible = false;
            }
        }

        protected void ddlDefaultType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDefaultType.SelectedValue.ToString().ToLower().Equals("default"))
            {
                txtDefaultValue.Visible = true;
                ddlSystemValues.Visible = false;
            }
            else
            {
                txtDefaultValue.Visible = false;
                ddlSystemValues.Visible = true;
            }
        }

        protected void update_button_Click(object sender, EventArgs e)
        {
            UpdateFieldData(Convert.ToInt32(Request.QueryString["GroupDatafieldsID"]), Convert.ToInt32(Request.QueryString["GroupID"].ToString()));
        }
	}
}
