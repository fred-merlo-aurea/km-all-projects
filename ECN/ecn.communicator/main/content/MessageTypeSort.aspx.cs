using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ecn.communicator.contentmanager
{
    public partial class MessageTypeSort : ECN_Framework.WebPageHelper
    {
      
        int channelID = 0;
        int customerID = 0;
        int userID = 0;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.CONTENT;
            Master.SubMenu = "message type priority";
            Master.Heading = "Content/Messages > Message Type Priority";
            Master.HelpContent = "";
            Master.HelpTitle = "Message Type Priority";

            if (KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
            {
                channelID = Convert.ToInt32(Master.UserSession.CurrentBaseChannel.BaseChannelID.ToString());
                customerID = Convert.ToInt32(Master.UserSession.CurrentUser.CustomerID.ToString());
                userID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);

                if (!IsPostBack)
                {
                    LoadListBox();
                }
            }
            else
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }
        }

        protected void btnUp_Click(Object sender, EventArgs e)
        {
            for (int i = 0; i < lstSourceFields.Items.Count; i++)
            {
                if (lstSourceFields.Items[i].Selected)
                {
                    if (i > 0 && !lstSourceFields.Items[i - 1].Selected)
                    {
                        ListItem bottom = lstSourceFields.Items[i];
                        lstSourceFields.Items.Remove(bottom);
                        lstSourceFields.Items.Insert(i - 1, bottom);
                        lstSourceFields.Items[i - 1].Selected = true;
                    }
                }
            }
        }

        protected void btndown_Click(Object sender, EventArgs e)
        {
            int startindex = lstSourceFields.Items.Count - 1;

            for (int i = startindex; i > -1; i--)
            {
                if (lstSourceFields.Items[i].Selected)
                {
                    if (i < startindex && !lstSourceFields.Items[i + 1].Selected)
                    {
                        ListItem bottom = lstSourceFields.Items[i];
                        lstSourceFields.Items.Remove(bottom);
                        lstSourceFields.Items.Insert(i + 1, bottom);
                        lstSourceFields.Items[i + 1].Selected = true;
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int count = 0;
            foreach (ListItem li in lstSourceFields.Items)
            {
                count++;
                ECN_Framework_Entities.Communicator.MessageType messageType = ECN_Framework_BusinessLayer.Communicator.MessageType.GetByMessageTypeID(Convert.ToInt32(li.Value), Master.UserSession.CurrentUser);
                messageType.SortOrder = count;
                ECN_Framework_BusinessLayer.Communicator.MessageType.UpdateSortOrder(Convert.ToInt32(li.Value), count, Master.UserSession.CurrentUser);
            }
            LoadListBox();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        private void LoadListBox()
        {
            List<ECN_Framework_Entities.Communicator.MessageType> messageTypeList = ECN_Framework_BusinessLayer.Communicator.MessageType.GetActivePriority(true, true, channelID, Master.UserSession.CurrentUser);
            var result = (from src in messageTypeList
                          orderby src.SortOrder
                         select new
                         {
                             MessageTypeID= src.MessageTypeID,
                             Name= src.Name
                         }).ToList();
            lstSourceFields.DataSource = result;
            lstSourceFields.DataBind();
        }
    }
}
