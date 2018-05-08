using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Objects.Accounts;
using KMPlatform.Entity;
using Telerik.Web.UI;
using AccountsBLL = ECN_Framework_BusinessLayer.Accounts;
using AccountsEnt = ECN_Framework_Entities.Accounts;

namespace ecn.accounts.channelsmanager
{
    public partial class basechanneleditor : ECN_Framework.WebPageHelper
    {        
        KMPlatform.Entity.ClientGroup CurrentClientGroup;
        ECN_Framework_Entities.Accounts.BaseChannel CurrentBaseChannel;

        private static List<KMPlatform.Entity.ServiceFeature.ClientGroupTreeListRow> GetTreeData(int clientGroupID, bool isAdditionalCost)
        {
            return new KMPlatform.BusinessLogic.ServiceFeature().GetClientGroupTreeList(clientGroupID,isAdditionalCost);
        }
        //new KMPlatform.BusinessLogic.ServiceFeature().GetClientGroupTreeList(clientGroupID)
        //Dictionary<KMPlatform.Entity.ServiceFeature.ClientGroupTreeListRow, TreeListDataItem> dataTreeXref = 
        //    new Dictionary<KMPlatform.Entity.ServiceFeature.ClientGroupTreeListRow, TreeListDataItem>();

        private int getBaseChannelID()
        {
            int theBaseChannelID = 0;
            try
            {
                theBaseChannelID = Convert.ToInt32(Request.QueryString["baseChannelID"].ToString());
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theBaseChannelID;
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.CHANNELS;
            lblErrorMessage.Text = "";
            phError.Visible = false;

            if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
            {
                if (!Page.IsPostBack)
                {
                    ddlChannelType.DataSource = AccountsBLL.Code.GetByCodeType(ECN_Framework_Common.Objects.Accounts.Enums.CodeType.ChannelType, Master.UserSession.CurrentUser);
                    ddlChannelType.DataTextField = "CodeName";
                    ddlChannelType.DataValueField = "CodeValue";
                    ddlChannelType.DataBind();                    

                    int requestBaseChannelID = getBaseChannelID();
                    if (requestBaseChannelID > 0)
                    {
                        //change form for edit/update method
                        CurrentBaseChannel = AccountsBLL.BaseChannel.GetByBaseChannelID(requestBaseChannelID);

                        ddlMSCustomer.DataSource = AccountsBLL.Customer.GetByBaseChannelID(requestBaseChannelID);
                        ddlMSCustomer.DataBind();
                        ddlMSCustomer.Items.Insert(0, new ListItem("--- Use Current Customer ---", "0"));
                        ddlMSCustomer.Items.FindByValue((CurrentBaseChannel != null && CurrentBaseChannel.MSCustomerID != null) ? CurrentBaseChannel.MSCustomerID.Value.ToString() : "0").Selected = true;

                        LoadFormData(CurrentBaseChannel);
                        //loadProductLinesGrid(requestBaseChannelID);
                        SetUpdateInfo(requestBaseChannelID);
                    }
                    else
                    {
                        ddlMSCustomer.Items.Insert(0, new ListItem("--- Use Current Customer ---", "0"));
                        ddlMSCustomer.Items.FindByValue("0").Selected = true;
                    }
                   
                }
            }
            else
            {
                Response.Redirect("~/main/securityAccessError.aspx");
            }
        }

        #region Form Prep

        private void SetUpdateInfo(int setChannelID)
        {
            ChannelID.Text = setChannelID.ToString();
            btnSave.Text = "Update";
        }

        #endregion

        #region Data Load
        private void LoadFormData(ECN_Framework_Entities.Accounts.BaseChannel basechannel)
        {
            tbChannelName.Text = basechannel.BaseChannelName;
            tbChannelURL.Text = basechannel.ChannelURL;
            tbBounceDomain.Text = basechannel.BounceDomain;
            tbWebAddress.Text = basechannel.WebAddress;


            ECN_Framework_Entities.Accounts.Contact cont = new ECN_Framework_Entities.Accounts.Contact();

            cont.Salutation = basechannel.Salutation;
            string[] contactName = basechannel.ContactName.Split(' ');
            if (contactName.Count() > 0 && contactName[0] != null)
                cont.FirstName = contactName[0];
            if (contactName.Count() > 1 && contactName[1] != null)
                cont.LastName = contactName[1];
            cont.ContactTitle = basechannel.ContactTitle;
            cont.Phone = basechannel.Phone;
            cont.State = basechannel.State;
            cont.Country = basechannel.Country;
            cont.Zip = basechannel.Zip;
            cont.Fax = basechannel.Fax;
            cont.Email = basechannel.Email;
            cont.StreetAddress = basechannel.Address;
            cont.City = basechannel.City;

            ContactEditor.Contact = cont;
            ddlChannelPartnerType.SelectedValue = Convert.ToInt32(basechannel.ChannelPartnerType).ToString();
            ddlChannelType.Items.FindByValue(basechannel.ChannelType.ToString()).Selected = true;;

            ddlMSCustomer.Items.FindByValue(basechannel.MSCustomerID != null ? basechannel.MSCustomerID.Value.ToString() : "0").Selected = true;

            hfBaseChannelGuid.Value = basechannel.BaseChannelGuid.ToString();
            hfBaseChannelPlatformClientGroupID.Value = basechannel.PlatformClientGroupID.ToString();

            KMPlatform.Entity.ClientGroup clientGroup = (new KMPlatform.BusinessLogic.ClientGroup()).Select(basechannel.PlatformClientGroupID);
            rblActive.SelectedValue = clientGroup.IsActive ? "yes" : "no";

            LoadTreeData(null == CurrentBaseChannel ? 0 : CurrentBaseChannel.PlatformClientGroupID);
        }
        #endregion

        #region Data Handlers

        public void btnSave_Click(object sender, EventArgs e)
        {
            var currentUser = Master.UserSession.CurrentUser;
            var baseChannelId = getBaseChannelID();

            int clientGroupId;
            int.TryParse(hfBaseChannelPlatformClientGroupID.Value, out clientGroupId);

            var isUpdate = baseChannelId > 0;

            CurrentClientGroup = ConstructClientGroupEntity(clientGroupId, isUpdate, currentUser);
            CurrentBaseChannel = ConstructBaseChannelEntity(baseChannelId, clientGroupId, isUpdate, currentUser);

            // explicitly trigger BaseChannel validation before saving ClientGroup
            // manually validate, so ECN exceptions should be filtered out here.
            try
            {
                AccountsBLL.BaseChannel.Validate(CurrentBaseChannel, Master.UserSession.CurrentUser);
            }
            catch (ECN_Framework_Common.Objects.ECNException ecnex)
            {
                ShowErrorMessage(ecnex);
                return;
            }

            // attempt database updates
            var rollback = false;
            var savedException = default(Exception);
            var clientGroupBeforeUpdate = default(ClientGroup);
            var clientGroupBusinessLogic = new KMPlatform.BusinessLogic.ClientGroup();

            try
            {
                if (clientGroupId > 0) // capture the ClientGroup before an update
                {
                    clientGroupBeforeUpdate = clientGroupBusinessLogic.Select(clientGroupId);
                }
                
                // insert/update the ClientGroup and save ClientGroupID in the BaseChannel entity
                clientGroupId = clientGroupBusinessLogic.Save(CurrentClientGroup);
                CurrentBaseChannel.PlatformClientGroupID = clientGroupId;

                // update the hidden field in case something goes wrong inserting the base-channel record
                // this will prevent us from needlessly inserting orphan client-group records (e.g. without
                // a corresponding BaseChannel record in ECN.   Alternately, we could delete during 
                // rollback if inserting a base-channel to ECN fails...
                hfBaseChannelPlatformClientGroupID.Value = clientGroupId.ToString();

                // insert/update BaseChannel
                AccountsBLL.BaseChannel.Save(CurrentBaseChannel, currentUser);

                // update ClientGroupServiceMap and ClientGroupServiceFeatureMap from the TreeList
                UpdateClientGroupServiceMapsAndFeatureMapsFromTreeList(clientGroupId, currentUser);

                // create the base channel admin security group after successful insert
                if (!isUpdate)
                {
                    CreateBaseChannelAdminSecurityGroup(clientGroupId, currentUser);
                }
            }
            catch (Exception ee)
            {
                savedException = ee;
                // rollback if we're doing an update
                rollback = clientGroupBeforeUpdate != null;
            }

            // rollback if DB work was partially successful
            if (rollback)
            {
                try
                {
                    clientGroupBusinessLogic.Save(clientGroupBeforeUpdate);
                }
                catch (Exception ee)
                {
                    savedException = savedException == null
                        ? new ApplicationException("rollback of ClientGroup failed", ee) as Exception
                        : new AggregateException("rollback of ClientGroup failed", savedException, ee) as Exception;

                    var emailDirect = CreateEmail(CurrentBaseChannel);
                    ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(emailDirect);
                }
            }

            if (savedException != null)
            {
                throw savedException; // re-throw any exception we trapped so that errors by Global.asax
            }
            Master.UserSession.RemoveSession(); 
            Response.Redirect("default.aspx");
        }

        private void CreateBaseChannelAdminSecurityGroup(int clientGroupId, User user)
        {
            var securityGroupBusinessLogic = new KMPlatform.BusinessLogic.SecurityGroup();
            securityGroupBusinessLogic.CreateFromTemplateForClientGroup("Channel Administrator", clientGroupId, "ChannelAdministrator", user);
        }

        private void UpdateClientGroupServiceMapsAndFeatureMapsFromTreeList(int clientGroupId, User user)
        {
            var clientGroupServiceMapBusinessLogic = new KMPlatform.BusinessLogic.ClientGroupServiceMap();
            var clientGroupServiceFeatureMapBusinessLogic = new KMPlatform.BusinessLogic.ClientGroupServiceFeatureMap();

            //Do additional cost stuff
            var alreadydone = new List<ClientGroupServiceFeatureMap>();
            foreach (var tlDataItem in tlClientGroupServiceFeatures.Items)
            {
                int mapId, serviceId, featureId;
                int.TryParse(tlDataItem["MAPID"].Text, out mapId);
                int.TryParse(tlDataItem["ServiceID"].Text, out serviceId);
                int.TryParse(tlDataItem["ServiceFeatureID"].Text, out featureId);

                bool? isFeature = null;
                if (serviceId > 0 && featureId > 0)
                {
                    isFeature = true;
                }
                else if (featureId == 0 && serviceId > 0)
                {
                    isFeature = false;
                }

                bool isAdditionalCost;
                bool.TryParse(tlDataItem["IsAdditionalCost"].Text, out isAdditionalCost);

                var selected = tlDataItem.Selected;
                var enabled = isFeature == false ? selected : (IsServiceEnabled(serviceId) && (selected || !isAdditionalCost));

                var diagMessage = string.Format("{0}: {1}.{2} [isFeature: {3,-5}, selected: {4,-5}, enabled: {5,-5}, isAdditionalCost: {6,-5}", mapId, serviceId, featureId, isFeature, selected, enabled, isAdditionalCost);
                System.Diagnostics.Trace.TraceInformation(diagMessage);

                switch (isFeature)
                {
                    case false: // service
                        var clientGroupServiceMap = new ClientGroupServiceMap(clientGroupId, mapId, serviceId, enabled, user.UserID);
                        clientGroupServiceMap.DateUpdated = DateTime.Now;
                        clientGroupServiceMap.UpdatedByUserID = user.UserID;

                        clientGroupServiceMapBusinessLogic.Save(clientGroupServiceMap);
                        break;
                    case true: // service feature
                        var clientGroupServiceFeatureMap = new ClientGroupServiceFeatureMap(clientGroupId, mapId, serviceId, featureId, enabled, user.UserID);
                        clientGroupServiceFeatureMap.DateUpdated = DateTime.Now;
                        clientGroupServiceFeatureMap.UpdatedByUserID = user.UserID;
                        clientGroupServiceFeatureMapBusinessLogic.Save(clientGroupServiceFeatureMap);
                        alreadydone.Add(clientGroupServiceFeatureMap);
                        break;
                }
            }

            //Do non additional cost stuff
            //Do the Non-AdditionalCost items, only need to do on new base channel save

            var serviceFeature = new KMPlatform.BusinessLogic.ServiceFeature();
            var nonAdditionalCost = serviceFeature.GetClientGroupTreeList(clientGroupId, false);

            foreach (var cgTreeListRow in nonAdditionalCost)
            {
                if (cgTreeListRow.ServiceFeatureID > 0 && !alreadydone.Any(x => x.ServiceFeatureID == cgTreeListRow.ServiceFeatureID))
                {
                    var enabled = IsServiceEnabled(cgTreeListRow.ServiceID);
                    var clientGroupServiceFeatureMap =
                        new ClientGroupServiceFeatureMap(clientGroupId, cgTreeListRow.ServiceID, cgTreeListRow.ServiceFeatureID, enabled, user.UserID);

                    clientGroupServiceFeatureMapBusinessLogic.Save(clientGroupServiceFeatureMap);
                }
            }
        }

        #endregion

        private void ddlChannelPartnerType_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }

        protected void tlClientGroupServiceFeatures_NeedDataSource(object sender, Telerik.Web.UI.TreeListNeedDataSourceEventArgs e)
        {
            int clientGroupID = null == CurrentBaseChannel ? 0 : CurrentBaseChannel.PlatformClientGroupID;
            //CurrentTreeData = new KMPlatform.BusinessLogic.ServiceFeature().GetClientGroupTreeList(clientGroupID);
            
            LoadTreeData(clientGroupID);
        }

        void LoadTreeData(int clientGroupID)
        {
            tlClientGroupServiceFeatures.DataSource = GetTreeData(clientGroupID,true);
            //tlClientGroupServiceFeatures.ExpandAllItems();
        }

        protected void tlClientGroupServiceFeatures_ItemCreated(object sender, TreeListItemCreatedEventArgs e)
        {
            if (e.Item is TreeListDataItem)
            {
                var dataItem = e.Item as TreeListDataItem;
                Control expandButton = e.Item.FindControl("ExpandCollapseButton");

                if (expandButton != null)
                {
                    expandButton.Visible = false;
                }

                if (dataItem.DataItem is KMPlatform.Entity.ServiceFeature.ClientGroupTreeListRow)
                {

                    // hide features that don't have an additional cost, these are always enabled
                    var data = dataItem.DataItem as KMPlatform.Entity.ServiceFeature.ClientGroupTreeListRow;
                    if (data.ServiceFeatureID > 0 && false == data.IsAdditionalCost)
                    {
                        dataItem.Visible = false;
                    }
                    // set selected based on the IsEnabled flag
                    if (data.IsEnabled)
                    {
                        dataItem.Selected = true;
                    }

                    // fully expand
                    if (dataItem.CanExpand)
                    {
                        dataItem.Expanded = true;
                    }

                    string diagMessage = String.Format("{0}: {1}.{2} [isFeature: {3,-5}, selected: {4,-5}, enabled: {5,-5}, isAdditionalCost: {6,-5}", data.MAPID, data.ServiceID, data.ServiceFeatureID, data.ServiceFeatureID != 0, dataItem.Selected, data.IsEnabled, data.IsAdditionalCost); System.Diagnostics.Trace.TraceInformation(diagMessage);
                }                
            }
            else if (e.Item is TreeListHeaderItem)
            {
                (e.Item as TreeListHeaderItem)["SelectColumn"].Controls.Add(new Label() { ID = "tlhSelect", Text = "Select" });
            }
            
        }

        private ClientGroup ConstructClientGroupEntity(int clientGroupId, bool isUpdate, User user)
        {
            var clientGroup = new ClientGroup
            {
                ClientGroupID = clientGroupId,
                ClientGroupDescription = tbChannelName.Text,
                ClientGroupName = tbChannelName.Text,
                Color = string.Empty,
                CreatedByUserID = user.UserID,
                IsActive = rblActive.SelectedValue.Equals("yes"),
                DateCreated = DateTime.Now
            };

            if(isUpdate)
            {
                clientGroup.UpdatedByUserID = user.UserID;
                clientGroup.DateUpdated = DateTime.Now;
            }

            return clientGroup;
        }

        private AccountsEnt.BaseChannel ConstructBaseChannelEntity(int baseChannelId, int clientGroupId, bool isUpdate, User user)
        {
            var channelType = Enums.ChannelType.Other;
            Enum.TryParse<Enums.ChannelType>(ddlChannelType.SelectedItem.Text, out channelType);

            int channelPartnerTypeValue;
            int.TryParse(ddlChannelPartnerType.SelectedValue, out channelPartnerTypeValue);

            var baseChannel = new AccountsEnt.BaseChannel
            {
                BaseChannelID = baseChannelId,
                PlatformClientGroupID = clientGroupId,
                BaseChannelName = tbChannelName.Text,
                Salutation = ContactEditor.Contact.Salutation,
                ContactName = string.Format("{0} {1}", ContactEditor.Contact.FirstName, ContactEditor.Contact.LastName),
                ContactTitle = ContactEditor.Contact.ContactTitle,
                Phone = ContactEditor.Contact.Phone,
                State = ContactEditor.Contact.State,
                Country = ContactEditor.Contact.Country,
                Zip = ContactEditor.Contact.Zip,
                Fax = ContactEditor.Contact.Fax,
                Email = ContactEditor.Contact.Email,
                Address = ContactEditor.Contact.StreetAddress,
                City = ContactEditor.Contact.City,
                ChannelPartnerType = (Enums.ChannelPartnerType)channelPartnerTypeValue,
                ChannelType = ddlChannelType.SelectedValue,
                ChannelURL = tbChannelURL.Text,
                WebAddress = tbWebAddress.Text,
                ChannelTypeCode = channelType
            };

            if (isUpdate)
            {
                baseChannel.UpdatedUserID = user.UserID;
                
                Guid baseChannelGuid;
                baseChannel.BaseChannelGuid = Guid.TryParse(hfBaseChannelGuid.Value, out baseChannelGuid) ? (Guid?)baseChannelGuid : null;
            }
            else
            {
                int msCustomerID;
                int.TryParse(ddlMSCustomer.SelectedItem.Value, out msCustomerID);
                baseChannel.MSCustomerID = msCustomerID > 0 ? (int?)msCustomerID : null;

                baseChannel.CreatedUserID = user.UserID;
                baseChannel.BaseChannelGuid = Guid.NewGuid();
            }

            return baseChannel;
        }

        private void ShowErrorMessage(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            var errorMessageBuilder = new StringBuilder();

            foreach (var ecnError in ecnException.ErrorList)
            {
                errorMessageBuilder.Append(ecnError.ErrorMessage);
                errorMessageBuilder.Append("<BR>");
            }

            lblErrorMessage.Text = errorMessageBuilder.ToString();
            phError.Visible = true;
        }

        private ECN_Framework_Entities.Communicator.EmailDirect CreateEmail(AccountsEnt.BaseChannel baseChannel)
        {
            var emailDirect = new ECN_Framework_Entities.Communicator.EmailDirect()
            {
                CustomerID = 1, // KM
                Status = "Pending",
                SendTime = DateTime.Now,
                EmailAddress = "dev-group@knowledgemarketing.com",
                EmailSubject = "ECN.Accounts - BaseChannel Update Rollback Failure",
                Content = string.Format(@"<html>
                                <head></head>
                                <body>
                                    <h1>Warning: <pre>Rollback of ClientGroupID <em>{0}</em> after failure updating BaseChannel {1}</pre></h1>
                                    <h2>Source: main/channels/basechanneleditor.aspx</h2>
                                    <div>Please review the ClientGroup and BaseChannel records for any descrepencies.</div>
                                </body>
                                <html>", baseChannel.PlatformClientGroupID, baseChannel.BaseChannelID),
                FromName = "ECN.Accounts",
                ReplyEmailAddress = "ecn.accounts-no-reply@ecn5.com",
                Source = "main/channels/basechanneleditor.aspx?BaseChannelID=" + baseChannel.BaseChannelID,
                Process = "ECN.Accounts"
            };

            return emailDirect;
        }

        private bool IsServiceEnabled(int serviceId)
        {
            var serviceIdString = string.Format("S{0}", serviceId);
            var serviceDataItem = tlClientGroupServiceFeatures.Items.FirstOrDefault(di => di["ID"].Text == serviceIdString);
            
            return serviceDataItem != null && serviceDataItem.Selected;
        }

        // remove the expand/colapse control as items are added: 
        //   http://www.telerik.com/forums/prevent-client-side-collapse-of-nodes
        //protected void tlClientGroupServiceFeatures_ItemCreated(object sender, Telerik.Web.UI.TreeListItemCreatedEventArgs e)
        //{
        //    //if (false == e.Item is TreeListDataItem)
        //    //{
        //    //    return;
        //    //}
        //    //var dataItem = (TreeListDataItem)e.Item;

        //    //// hide features that don't have an additional cost, these are always enabled
        //    //var data = dataItem.DataItem as KMPlatform.Entity.ServiceFeature.ClientGroupTreeListRow;
        //    //if (data.ServiceFeatureID > 0 && false == data.IsAdditionalCost)
        //    //{
        //    //    dataItem.Visible = false;
        //    //}

        //    //// expand all, disable collapse
        //    //if (dataItem.CanExpand)
        //    //{
        //    //    dataItem.Expanded = true;
        //    //    var expandCell = dataItem.Cells[dataItem.HierarchyIndex.NestedLevel];
        //    //    if (expandCell.Controls.Count > 0)
        //    //    {
        //    //        expandCell.Controls[0].Visible = false;
        //    //    }
        //    //}            

        //    //if (false == dataItem.DataItem is KMPlatform.Entity.ServiceFeature.ClientGroupTreeListRow)
        //    //{
        //    //    return;
        //    //}

        //}
    }
}

