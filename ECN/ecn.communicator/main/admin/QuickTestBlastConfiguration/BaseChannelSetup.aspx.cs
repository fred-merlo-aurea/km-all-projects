using ECN_Framework_Common.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;
using ecn.common.classes;
using System.Text;

namespace ecn.communicator.main.admin.QuickTestBlastConfiguration
{
    using Role = KM.Platform.User;
    public partial class BaseChannelSetup : ECN_Framework.WebPageHelper
    {
        KMPlatform.Entity.User SessionCurrentUser { get { return Master.UserSession.CurrentUser; } }
        private static ECN_Framework_Entities.Communicator.QuickTestBlastConfig QTB = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            phError.Visible = false;
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.BASECHANNEL;
            if (!IsPostBack)
            {
                if (Role.IsChannelAdministrator(SessionCurrentUser))
                {
                    pnlNoAccess.Visible = false;
                    pnlSettings.Visible = true;
                    LoadData();

                }
                else
                {
                    pnlNoAccess.Visible = true;
                    pnlSettings.Visible = false;
                    Label1.Text = "You do not have access to this page because you are not a Basechannel Administrator.";
                }
            }
        }

        private void LoadData()
        {
            QTB = ECN_Framework_BusinessLayer.Communicator.QuickTestBlastConfig.GetByBaseChannelID(Master.UserSession.CurrentCustomer.BaseChannelID.Value);
            if (QTB != null)
            {
                if (QTB.BaseChannelDoesOverride.Value == true)
                {
                    rblOverrideDefaultSettings.SelectedValue = "Yes";
                }
                if (QTB.CustomerCanOverride.Value == true)
                {
                    rblAllowCustomerOverrideSettings.SelectedValue = "Yes";
                }
                cbAllowAdhocEmails.Checked = QTB.AllowAdhocEmails.Value;
                cbAutoCreateGroup.Checked = QTB.AutoCreateGroup.Value;
                cbAutoArchiveGroup.Checked = QTB.AutoArchiveGroup.Value;
            }
        }

        protected void btnSave_onclick(object sender, EventArgs e)
        {
            phError.Visible = false;
            //LandingPageAssign

            if (QTB == null)
            {
                QTB = new ECN_Framework_Entities.Communicator.QuickTestBlastConfig();
                QTB.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                QTB.BaseChannelID = Master.UserSession.CurrentCustomer.BaseChannelID;
            }
            //Save Override Default selection
            if (rblOverrideDefaultSettings.SelectedValue.Equals("Yes"))
            {
                QTB.BaseChannelDoesOverride = true;
            }
            else
                QTB.BaseChannelDoesOverride = false;
            //Save Allow Customer Override selection
            if (rblAllowCustomerOverrideSettings.SelectedValue.Equals("Yes"))
                QTB.CustomerCanOverride = true;
            else
                QTB.CustomerCanOverride = false;

            QTB.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
            QTB.AllowAdhocEmails = cbAllowAdhocEmails.Checked;
            QTB.AutoCreateGroup = cbAutoCreateGroup.Checked;
            QTB.AutoArchiveGroup = cbAutoArchiveGroup.Checked;
            ECN_Framework_BusinessLayer.Communicator.QuickTestBlastConfig.Save(QTB, Master.UserSession.CurrentUser);
        }

        protected void btnCancel_onclick(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void cbAllowAdhocEmails_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbAllowAdhocEmails.Checked)
            {
                cbAutoCreateGroup.Checked = false;
                cbAutoArchiveGroup.Checked = false;
            }
        }

        protected void cbAutoCreateGroup_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAutoCreateGroup.Checked)
            {
                cbAllowAdhocEmails.Checked = true;
            }
        }

        protected void cbAutoArchiveGroup_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAutoArchiveGroup.Checked)
            {
                cbAllowAdhocEmails.Checked = true;
            }
        }
    }
}