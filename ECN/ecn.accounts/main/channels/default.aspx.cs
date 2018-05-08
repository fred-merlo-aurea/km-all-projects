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

namespace ecn.accounts.channelsmanager {

    public partial class channels_main : ECN_Framework.WebPageHelper
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.CHANNELS;
            Master.HelpContent = "Channels Help";
            Master.HelpTitle = "CHANNEL MANAGER"; 
            
            if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser)) 
            {
                loadChannelsGrid();
            }
            else
            {
                Response.Redirect("~/main/securityAccessError.aspx");
            }
        }

		/*private void loadChannelsGrid() {
			string sqlquery = "";
			if(KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser)) {
				sqlquery =
					" SELECT ch.ChannelID, ch.ChannelName, Count(cu.CustomerID) As CustomerCount "+
					" FROM Channel ch, Customer cu "+
					" WHERE ch.ChannelID*=cu.CustomerID "+
					" GROUP BY ch.ChannelID, ch.ChannelName ";
			}else if(Master.UserSession.CurrentKM.Platform.User.IsChannelAdministrator(user)) {
				sqlquery =
					" SELECT ch.ChannelID, ch.ChannelName, Count(cu.CustomerID) As CustomerCount "+
					" FROM Channel ch, Customer cu "+
					" WHERE ch.ChannelID = cu.CustomerID and ch.ChannelID = "+Master.UserSession.CurrentBaseChannel.BaseChannelID+
					" GROUP BY ch.ChannelID, ch.ChannelName ";
			}
			ChannelsGrid.DataSource=DataFunctions.GetDataTable(sqlquery);
			ChannelsGrid.DataBind();
		}*/

		private void loadChannelsGrid() {

			if(KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser)) {

                grdChannels.DataSource = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetAll().Select(x => new { x.BaseChannelID, x.BaseChannelName }).OrderBy(x => x.BaseChannelName);  

			}
            //else if(Master.UserSession.CurrentKM.Platform.User.IsChannelAdministrator(user)) {
            else if(KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser)) {
                grdChannels.DataSource = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(Master.UserSession.CurrentBaseChannel.BaseChannelID);
			}
            grdChannels.DataBind();
		}

        protected void grdChannels_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void grdChannels_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToUpper() == "DELETE")
            {
                DeleteChannel(Convert.ToInt32(e.CommandArgument.ToString()));
            }
        }

        protected void grdChannels_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdChannels.PageIndex = e.NewPageIndex;
            loadChannelsGrid();
        }

        public void DeleteChannel(int basechanneID)
        {
            ECN_Framework_BusinessLayer.Accounts.BaseChannel.Delete(basechanneID, Master.UserSession.CurrentUser);
            loadChannelsGrid();
        }

	}
}


















