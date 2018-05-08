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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ecn.accounts.channelsmanager
{
    public partial class channelseditor : ECN_Framework.WebPageHelper
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.CHANNELS;
            lblErrorMessage.Text = "";
            phError.Visible = false;

            if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser) )
            {
                if (Page.IsPostBack == false)
                {
                    int requestChannelID = getChannelID();
                    if (requestChannelID > 0)
                    {
                        //change form for edit/update method
                        LoadFormData(requestChannelID);
                        SetUpdateInfo(requestChannelID);
                    }
                }
            }
            else
            {
                Response.Redirect("~/main/securityAccessError.aspx");
            }
        }

        private int getChannelID()
        {
            int theChannelID = 0;
            try
            {
                theChannelID = Convert.ToInt32(Request.QueryString["ChannelID"].ToString());
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theChannelID;
        }

        #region Form Prep

        private void SetUpdateInfo(int setChannelID)
        {
            ChannelID.Text = setChannelID.ToString();
            btnSave.Text = "Update";
        }

        #endregion

        #region Data Load
        private void LoadFormData(int setChannelID)
        {
            ECN_Framework_Entities.Accounts.Channel channel = ECN_Framework_BusinessLayer.Accounts.Channel.GetByChannelID(setChannelID);

            tbChannelName.Text = channel.ChannelName;
            tbVirtualPath.Text = channel.VirtualPath;
            tbAssetsPath.Text = channel.AssetsPath;
            tbPickupPath.Text = channel.PickupPath;
            tbHeaderSource.Text = channel.HeaderSource;
            tbFooterSource.Text = channel.FooterSource;
            tbMailingIP.Text = channel.MailingIP;
            ECN_Framework_Entities.Accounts.BaseChannel basechannel = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(Convert.ToInt32(channel.BaseChannelID));
            tbChannelURL.Text = basechannel.ChannelURL;
            tbBounceDomain.Text = basechannel.BounceDomain;
        }
        #endregion

        #region Data Handlers

        public void btnSave_Click(object sender, System.EventArgs e)
        {
            ECN_Framework_Entities.Accounts.Channel channel = ECN_Framework_BusinessLayer.Accounts.Channel.GetByChannelID(getChannelID());

            string hsource = ECN_Framework_DataLayer.DataFunctions.CleanString(tbHeaderSource.Text);
            string fsource = ECN_Framework_DataLayer.DataFunctions.CleanString(tbFooterSource.Text);
            channel.VirtualPath = tbVirtualPath.Text;
            channel.AssetsPath = tbAssetsPath.Text;
            channel.PickupPath = tbPickupPath.Text;
            channel.MailingIP = tbMailingIP.Text;
            channel.HeaderSource = hsource;
            channel.FooterSource = fsource;

            if (channel.ChannelID > 0)
                channel.UpdatedUserID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);
            else
                channel.CreatedUserID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);

            try
            {
                ECN_Framework_BusinessLayer.Accounts.Channel.Save(channel, Master.UserSession.CurrentUser); 
            }
            catch (ECN_Framework_Common.Objects.ECNException ecnex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (ECN_Framework_Common.Objects.ECNError err in ecnex.ErrorList)
                {
                    sb.Append(err.ErrorMessage + "<BR>");
                }
                lblErrorMessage.Text = sb.ToString();
                phError.Visible = true;
                return;
            }

            if (channel.ChannelID > 0)
            {
                Response.Redirect("basechanneleditor.aspx?baseChannelID=" + channel.BaseChannelID + "&action=edit");
            }
            else
            {
                Response.Redirect("default.aspx");
            }
        }

        #endregion
    }
}
