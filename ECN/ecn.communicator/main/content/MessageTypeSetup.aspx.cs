using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using Business = ECN_Framework_BusinessLayer.Communicator;

namespace ecn.communicator.contentmanager
{
    public partial class MessageTypeSetup : ECN_Framework.WebPageHelper
    {
        private const string TxtNameControlName = "txtName";
        private const string TxtDescriptionControlName = "txtDescription";
        private const string DdlThresholdControlName = "ddlThreshold";
        private const string DdlPriorityControlName = "ddlPriority";
        private const string DdlIsActiveControlName = "ddlIsActive";

        int channelID = 0;
        int customerID = 0;
        int userID = 0;

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

            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.CONTENT; ;
            Master.SubMenu = "message type setup";
            Master.Heading = "Content/Messages > Message Types";
            Master.HelpContent = "";
            Master.HelpTitle = "Message Type Setup";

            if (KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
            {
                channelID = Master.UserSession.CurrentBaseChannel.BaseChannelID;
                customerID = Master.UserSession.CurrentUser.CustomerID;
                userID =Master.UserSession.CurrentUser.UserID;

                if (!IsPostBack)
                {
                    ViewState["mtSortField"] = "Name";
                    ViewState["mtSortDirection"] = "ASC";

                    LoadGridView();
                    dvMessageTypes.ChangeMode(DetailsViewMode.Insert);
                    dvMessageTypes.HeaderText = "Add Message Type";
                }
            }
            else
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }
        }

        private void LoadGridView()
        {
            List<ECN_Framework_Entities.Communicator.MessageType> messageTypeList = ECN_Framework_BusinessLayer.Communicator.MessageType.GetByBaseChannelID(channelID, Master.UserSession.CurrentUser);
            var result = (from src in messageTypeList
                         orderby src.Name
                         select src).ToList();
            if (ViewState["mtSortField"].ToString().Equals("Description"))
            {
                result = (from src in messageTypeList
                         orderby src.Description
                         select src).ToList();
            }
            else if (ViewState["mtSortField"].ToString().Equals("Threshold"))
            {
                result = (from src in messageTypeList
                         orderby src.Threshold
                         select src).ToList();
            }
            else if (ViewState["mtSortField"].ToString().Equals("Priority"))
            {
                result = (from src in messageTypeList
                         orderby src.Priority
                         select src).ToList();
            }
            else if (ViewState["mtSortField"].ToString().Equals("IsActive"))
            {
                result = (from src in messageTypeList
                         orderby src.IsActive
                         select src).ToList();
            }

            if (ViewState["mtSortDirection"].ToString().Equals("DESC"))
            {
                result.Reverse();
            }

            try
            {
                gvMessageTypes.DataSource = result;
                gvMessageTypes.DataBind();
            }
            catch
            {
                gvMessageTypes.PageIndex = 0;
                gvMessageTypes.DataBind();				
            }
        }

        private void LoadDetailsView(int MessageTypeID)
        {
            ECN_Framework_Entities.Communicator.MessageType messageType = ECN_Framework_BusinessLayer.Communicator.MessageType.GetByMessageTypeID(MessageTypeID, Master.UserSession.CurrentUser);
            var messageTypeList = new List<ECN_Framework_Entities.Communicator.MessageType> { messageType };
            dvMessageTypes.DataSource = messageTypeList;
            dvMessageTypes.DataBind();

            bool MessageTypeUsed= ECN_Framework_BusinessLayer.Communicator.Layout.MessageTypeUsedInLayout(MessageTypeID);
            if (MessageTypeUsed)
            {
                DetailsViewRow dvRow = dvMessageTypes.Rows[0];
                DropDownList isActive = (DropDownList)dvRow.FindControl("ddlIsActive");
                isActive.Enabled = false;
            }
        }

        public void DeleteMessageType(int messageTypeID)
        {
            try
            {
                ECN_Framework_BusinessLayer.Communicator.MessageType.Delete(messageTypeID, Master.UserSession.CurrentUser);
                LoadGridView();
                LoadDetailsView(0);
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
        }

        protected void gvMessageTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            dvMessageTypes.ChangeMode(DetailsViewMode.Edit);
            dvMessageTypes.HeaderText = "Edit Message Type";
            LoadDetailsView(Convert.ToInt32(gvMessageTypes.DataKeys[gvMessageTypes.SelectedIndex].Value));
        }

        protected void dvMessageTypes_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {

        }

        protected void dvMessageTypes_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            try
            {
                var messageType = new MessageType
                {
                    CreatedUserID = Master.UserSession.CurrentUser.UserID
                };

                InitMessageType(messageType);
                SaveMessageType(messageType);
                ReloadViews();
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
        }

        protected void dvMessageTypes_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {

        }

        protected void dvMessageTypes_ModeChanging(object sender, DetailsViewModeEventArgs e)
        {
            if (e.CancelingEdit)
            {
                dvMessageTypes.ChangeMode(DetailsViewMode.Insert);
                dvMessageTypes.HeaderText = "Add Message Type";
            }
        }

        protected void gvMessageTypes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                gvMessageTypes.PageIndex = e.NewPageIndex;
            }
            LoadGridView();
        }

        protected void gvMessageTypes_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression.ToString().Equals(ViewState["mtSortField"].ToString()))
            {
                switch (ViewState["mtSortDirection"].ToString())
                {
                    case "ASC":
                        ViewState["mtSortDirection"] = "DESC";
                        break;
                    case "DESC":
                        ViewState["mtSortDirection"] = "ASC";
                        break;
                }
            }
            else
            {
                ViewState["mtSortField"] = e.SortExpression;
                ViewState["mtSortDirection"] = "ASC";
            }
            LoadGridView();
        }

        protected void gvMessageTypes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int messageTypeID = Convert.ToInt32(gvMessageTypes.DataKeys[e.RowIndex].Values[0]);
            DeleteMessageType(messageTypeID);
        }

        public void gvMessageTypes_Command(Object sender, DataGridCommandEventArgs e)
        {
            int messageTypeID = Convert.ToInt32(e.CommandArgument.ToString());
            DeleteMessageType(messageTypeID);
        }

        protected void dvMessageTypes_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            try
            {
                var messageTypeId = 0;
                if (Int32.TryParse(dvMessageTypes.DataKey[0].ToString(), out messageTypeId))
                {
                    var messageType = 
                        ECN_Framework_BusinessLayer.Communicator.MessageType.GetByMessageTypeID(
                        messageTypeId, Master.UserSession.CurrentUser);
                    messageType.UpdatedUserID = Master.UserSession.CurrentUser.UserID;

                    InitMessageType(messageType);
                    SaveMessageType(messageType);
                    ReloadViews();
                }
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
        }

        private void InitMessageType(MessageType messageType)
        {
            if (dvMessageTypes.Rows.Count == 0 || messageType == null)
            {
                return;
            }

            var firstRow = dvMessageTypes.Rows[0];
            var name = firstRow.FindControl(TxtNameControlName) as TextBox;
            var desc = firstRow.FindControl(TxtDescriptionControlName) as TextBox;
            var threshold = firstRow.FindControl(DdlThresholdControlName) as DropDownList;
            var priority = firstRow.FindControl(DdlPriorityControlName) as DropDownList;
            var isActive = firstRow.FindControl(DdlIsActiveControlName) as DropDownList;

            var itemName = String.Empty;
            var itemDesc = String.Empty;
            var itemThreshold = false;
            var itemPriority = false;
            var itemIsActive = false;

            if (name != null)
            {
                itemName = name.Text;
            }
            if (desc != null)
            {
                itemDesc = desc.Text;
            }
            if (threshold != null)
            {
                Boolean.TryParse(threshold.SelectedValue, out itemThreshold);
            }
            if (priority != null)
            {
                Boolean.TryParse(priority.SelectedValue, out itemPriority);
            }
            if (isActive != null)
            {
                Boolean.TryParse(isActive.SelectedValue, out itemIsActive);
            }

            messageType.Name = itemName;
            messageType.Description = itemDesc;
            messageType.Threshold = itemThreshold;
            messageType.Priority = itemPriority;
            messageType.IsActive = itemIsActive;
            messageType.BaseChannelID = channelID;
            messageType.CustomerID = null;
            if (itemPriority)
            {
                messageType.SortOrder = Business.MessageType.GetMaxSortOrder(channelID);
            }
        }

        private void SaveMessageType(MessageType messageType)
        {
            Business.MessageType.Save(
                messageType, Master.UserSession.CurrentUser);
        }

        private void ReloadViews()
        {
            const string headerText = "Add Message Type";
            dvMessageTypes.ChangeMode(DetailsViewMode.Insert);
            dvMessageTypes.HeaderText = headerText;

            LoadGridView();
            LoadDetailsView(0);
        }
    }
}
