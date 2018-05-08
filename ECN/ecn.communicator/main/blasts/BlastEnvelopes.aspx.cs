using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ECN_Framework.Communicator;
using ECN_Framework.Common;
using aspNetMX;
using aspNetEmail;

namespace ecn.communicator.main.blasts
{
    public partial class BlastEnvelopes : ECN_Framework.WebPageHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            phError.Visible = false;
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.BLASTS;
            Master.SubMenu = "blast envelopes";
            Master.Heading = "Blasts/Reporting > Blast Envelopes";
            Master.HelpContent = "";
            Master.HelpTitle = "";

            if (!IsPostBack)
            {
                //if (!(KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser)))
                if (!KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                {
                    throw new ECN_Framework_Common.Objects.SecurityException(); 
                }

                if(KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastEnvelopes, KMPlatform.Enums.Access.Edit))
                {
                    pnlAdd.Visible = true;
                }
                else
                { 
                    pnlAdd.Visible = false; 
                }

                loadBlastEnvelopes();
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

        private void loadBlastEnvelopes()
        {
            gvEnvelope.DataSource = ECN_Framework_BusinessLayer.Communicator.BlastEnvelope.GetByCustomerID(Master.UserSession.CurrentUser.CustomerID, Master.UserSession.CurrentUser);
            try
            {
                gvEnvelope.DataBind();
            }
            catch
            {
                gvEnvelope.PageIndex = 0;
                gvEnvelope.DataBind();
            }

            if(KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastEnvelopes, KMPlatform.Enums.Access.Edit))
            {
                gvEnvelope.Columns[3].Visible = true;
                gvEnvelope.Columns[4].Visible = true;
            }
            else
            {
                gvEnvelope.Columns[3].Visible = false;
                gvEnvelope.Columns[4].Visible = false;
            }
        }

        protected void gvEnvelope_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                gvEnvelope.PageIndex = e.NewPageIndex;
            }
            gvEnvelope.DataBind();
            loadBlastEnvelopes();
        }

        protected void gvEnvelope_Command(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("DeleteBlastEnvelope"))
            {
                try
                {
                    ECN_Framework_BusinessLayer.Communicator.BlastEnvelope.Delete(Convert.ToInt32(e.CommandArgument.ToString()), Master.UserSession.CurrentUser.CustomerID, Master.UserSession.CurrentUser);
                    loadBlastEnvelopes();
                }
                catch (ECN_Framework_Common.Objects.ECNException ex)
                {
                    setECNError(ex);
                }
            }
            else if (e.CommandName.Equals("EditBlastEnvelope"))
            {
                try
                {
                    ECN_Framework_Entities.Communicator.BlastEnvelope blastEnvelope = ECN_Framework_BusinessLayer.Communicator.BlastEnvelope.GetByBlastEnvelopeID(Convert.ToInt32(e.CommandArgument.ToString()), Master.UserSession.CurrentUser);
                    txtFromName.Text = blastEnvelope.FromName;
                    txtFromEmail.Text = blastEnvelope.FromEmail;
                    lblBlastEnvelopeID.Text =  blastEnvelope.BlastEnvelopeID.ToString();
                }
                catch (ECN_Framework_Common.Objects.ECNException ex)
                {
                    setECNError(ex);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                MXValidate mx = new MXValidate();
                mx.LogInMemory = true;
                string EmailAddress = txtFromEmail.Text;
                MXValidateLevel level = mx.Validate(EmailAddress, MXValidateLevel.MXRecords);

                if (level == MXValidateLevel.MXRecords)
                {
                    ECN_Framework_Entities.Communicator.BlastEnvelope blastEnvelope = new ECN_Framework_Entities.Communicator.BlastEnvelope();
                    blastEnvelope.FromName = txtFromName.Text;
                    blastEnvelope.FromEmail = txtFromEmail.Text;
                    blastEnvelope.BlastEnvelopeID = Convert.ToInt32(lblBlastEnvelopeID.Text);
                    blastEnvelope.CustomerID = Master.UserSession.CurrentUser.CustomerID;

                    if (blastEnvelope.BlastEnvelopeID > 0)
                        blastEnvelope.UpdatedUserID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);
                    else
                        blastEnvelope.CreatedUserID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);

                    ECN_Framework_BusinessLayer.Communicator.BlastEnvelope.Save(blastEnvelope, Master.UserSession.CurrentUser);
                    Response.Redirect("BlastEnvelopes.aspx");
                }
                else
                {
                    phError.Visible = true;
                    lblErrorMessage.Text = "ERROR - From Email Address Entered is not Valid. Please use a different Email Address.";
                }
             }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                setECNError(ex);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            lblBlastEnvelopeID.Text = "";
            txtFromEmail.Text = "";
            txtFromName.Text = "";
        }
    }
}