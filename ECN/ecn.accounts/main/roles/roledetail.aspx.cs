using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using ECNCommon = ECN_Framework_Common.Objects;

namespace ecn.accounts.main.roles
{
    public partial class roledetail : System.Web.UI.Page
    {            
        enum TreeDataItemMapReferenceType { Service, Feature, Access, ERROR }

        private const string DefaultRedirectDestination = "Default.aspx";
        private static KMPlatform.Entity.SecurityGroup CurrentSecurityGroup;

        public new ecn.accounts.MasterPages.Accounts Master
        {
            get
            {
                return ((ecn.accounts.MasterPages.Accounts)(base.Master));
            }
        }      

        public bool IsChannelWide { get; set; }

        public static bool IsUpdate { get; set; }

        private int currentClientGroupID()
        {
            int id = 0;
            try { id = Convert.ToInt32(drpclientgroup.SelectedValue); }
            catch { id = Master.UserSession.ClientGroupID; }
            return id;
        }

        private int currentClientID()
        {
            int id = 0;
            try { id = Convert.ToInt32(drpClient.SelectedValue); }
            catch { id = Master.UserSession.ClientID; }
            return id;
        }

        private int currentSecurityGroupID()
        {
            int id = -1;
            try { id = Convert.ToInt32(String.IsNullOrWhiteSpace(hfSecurityGroupID.Value) ? Request.QueryString["SecurityGroupID"].ToString() : hfSecurityGroupID.Value); }
            catch { }
            return id;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            Master.Heading = "Users > Manage Role";
            int clientGroupID = currentClientGroupID();
            int clientID = currentClientID();
            int securityGroupID = currentSecurityGroupID();

            bool isChannelAdmin = KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser);

            if (securityGroupID > 0)
            {
                IsUpdate = true;
                SubmitButton.Text = "Update";

                var sgLogic = new KMPlatform.BusinessLogic.SecurityGroup();
                CurrentSecurityGroup = sgLogic.Select(securityGroupID, false);
                if (SecurityCheck(CurrentSecurityGroup))
                {
                    drpClient.Enabled = false; // cannot edit the clientID associated with a security group
                    drpclientgroup.Enabled = false; // cannot edit the clientGroupID associated with a security group
                    rbIsChannelWide.Enabled = false; // cannot edit scope at all

                    hfSecurityGroupID.Value = CurrentSecurityGroup.SecurityGroupID.ToString();
                    tbSecurityGroupName.Text = CurrentSecurityGroup.SecurityGroupName;

                    tbSecurityGroupDescription.Text = CurrentSecurityGroup.Description;
                    //tbSecurityGroupDescription.Enabled = false;

                    rbIsActive.SelectedIndex = CurrentSecurityGroup.IsActive ? 0 : 1;
                    //tbLastModified.Text = (CurrentSecurityGroup.DateUpdated ?? CurrentSecurityGroup.DateCreated).ToString();


                    if (CurrentSecurityGroup.ClientGroupID > 0)
                    {
                        rbIsChannelWide.SelectedIndex = 0; // yes
                        clientGroupID = CurrentSecurityGroup.ClientGroupID;
                        clientID = 0;
                    }
                    else if (CurrentSecurityGroup.ClientID > 0)
                    {
                        rbIsChannelWide.SelectedIndex = 1; // no
                        clientID = CurrentSecurityGroup.ClientID;
                        List<KMPlatform.Entity.ClientGroupClientMap> cgcm = (new KMPlatform.BusinessLogic.ClientGroupClientMap()).SelectForClientID(clientID);
                        clientGroupID = cgcm.First().ClientGroupID;
                    }
                    else
                    {
                        throw new ApplicationException("Security Group requires associated ClientGroup or Client");
                    }
                }
                else
                {
                    throw new ECN_Framework_Common.Objects.SecurityException() { SecurityType = ECNCommon.Enums.SecurityExceptionType.RoleAccess };
                }
            }
            else
            {
                IsUpdate = false;
                CurrentSecurityGroup = new KMPlatform.Entity.SecurityGroup();
                rbIsChannelWide.SelectedIndex = 1; // no
            }

            drpclientgroup.DataSource = Master.UserSession.CurrentUser.ClientGroups.Where(x => x.IsActive == true).OrderBy(x => x.ClientGroupName);
            drpclientgroup.DataBind();
            if (clientGroupID > 0)
            {
                drpclientgroup.Items.FindByValue(clientGroupID.ToString()).Selected = true;
            }

            if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
            {
                drpClient.DataSource = (new KMPlatform.BusinessLogic.Client()).SelectActiveForClientGroupLite(clientGroupID).OrderBy(x => x.ClientName);
            }
            else if (KMPlatform.BusinessLogic.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
            {
                drpClient.DataSource = new KMPlatform.BusinessLogic.Client().SelectActiveForClientGroupLite(clientGroupID).OrderBy(x => x.ClientName);
            }
            else
            {
                drpClient.DataSource = new KMPlatform.BusinessLogic.Client().SelectbyUserIDclientgroupID(Master.UserSession.CurrentUser.UserID,clientGroupID );
                drpClient.Enabled = false;
            }
            drpClient.DataBind();

            if (clientID > 0)
            {
                if (drpClient.Items.FindByValue(clientID.ToString()) != null)
                {
                    drpClient.Items.FindByValue(clientID.ToString()).Selected = true;
                }
                else
                {
                    drpClient.SelectedIndex = 0;
                }
            }
            ShowHidePannels();
        }

        private bool SecurityCheck(KMPlatform.Entity.SecurityGroup sg)
        {
            if(KMPlatform.BusinessLogic.User.IsAdministrator(Master.UserSession.CurrentUser) )
            {
                if (KMPlatform.BusinessLogic.User.IsSystemAdministrator(Master.UserSession.CurrentUser))//Sys admin can see everything
                    return true;
                int clientID = sg.ClientID;
                int clientGroupID = sg.ClientGroupID;
                if (clientID > 0)//SG is a client role, So just verify user is admin for the client
                { 
                    if(clientID == Master.UserSession.ClientID)//if it's same as logged in client just return cause we already did that check
                    {
                        return true;
                    }
                    int usersSGID = Master.UserSession.CurrentUser.UserClientSecurityGroupMaps.First(x => x.ClientID == clientID && x.IsActive == true).SecurityGroupID;
                    KMPlatform.Entity.SecurityGroup sgToCheck = new KMPlatform.BusinessLogic.SecurityGroup().Select(usersSGID, false, false);
                    if (sgToCheck.AdministrativeLevel != KMPlatform.Enums.SecurityGroupAdministrativeLevel.None)//Check security groups admin level where clientid == clientid of security we are editing
                    {
                        return true;
                    }
                    else
                        return false;

                }
                else if (clientGroupID > 0)//SG is a Client GroupRole, need to validate that the current security group is the same clientgroup as the logged in user
                {
                    if (clientGroupID == Master.UserSession.CurrentUser.CurrentClientGroup.ClientGroupID && KMPlatform.BusinessLogic.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
                    {
                        return true;
                    }
                    else
                    {
                        //cant let them edit a role from a diff base channel
                        return false;
                    }
                }
                else
                    return false;

            }
            else
            {
                return false;
            }
        }

        protected void drpclientgroup_DataBound(object sender, EventArgs e)
        {
            //ShowHidePannels();
        }

        protected void rbIsChannelWide_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowHidePannels();
        }

        protected void tlSecurityGroupAccess_ItemCreated(object sender, Telerik.Web.UI.TreeListItemCreatedEventArgs e)
        {
            if (e.Item is TreeListDataItem)
            {
                var dataItem = e.Item as TreeListDataItem;

                // prevent collapse
                Control expandButton = e.Item.FindControl("ExpandCollapseButton");
                if (expandButton != null)
                {
                    expandButton.Visible = false;
                }

                // set selected based on the IsEnabled flag
                if (dataItem.DataItem is KMPlatform.Entity.ServiceFeature.SecurityGroupTreeListRow)
                {
                    KMPlatform.Entity.ServiceFeature.SecurityGroupTreeListRow row = dataItem.DataItem
                        as KMPlatform.Entity.ServiceFeature.SecurityGroupTreeListRow;
                    dataItem.Selected = row.IsEnabled;
                }
            }
            else if(e.Item is TreeListHeaderItem)
            {
                (e.Item as TreeListHeaderItem)["SelectColumn"].Controls.Add(new Label() { ID = "tlhSelect", Text = "Select" });
            }
        }

        protected void tlSecurityGroupAccess_NeedDataSource(object sender, Telerik.Web.UI.TreeListNeedDataSourceEventArgs e)
        {
            LoadSecurityGroupAccessTreeView();
        }

        protected void drpClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowHidePannels();
        }

        protected void drpclientgroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
            {
                drpClient.DataSource = (new KMPlatform.BusinessLogic.Client()).SelectActiveForClientGroupLite(currentClientGroupID()).OrderBy(x => x.ClientName);
            }
            else if (KMPlatform.BusinessLogic.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
            {
                drpClient.DataSource = new KMPlatform.BusinessLogic.Client().SelectActiveForClientGroupLite(currentClientGroupID()).OrderBy(x => x.ClientName);
            }
           
            drpClient.DataBind();
            if (currentClientID() > 0)
            {
                ListItem clientItem = drpClient.Items.FindByValue(currentClientID().ToString());
                if (clientItem != null)
                {
                    clientItem.Selected = true;
                }
            }
            ShowHidePannels();
        }

        void LoadSecurityGroupAccessTreeView()
        {
            int clientGroupID = 0;
            Int32.TryParse(drpclientgroup.SelectedValue, out clientGroupID);
            int clientID = -1;

            if (rbIsChannelWide.SelectedIndex != 0)
            {
                Int32.TryParse(drpClient.SelectedValue, out clientID);

            }
            if (1 > clientGroupID && 1 > clientID)
            {
                tlSecurityGroupAccess.DataSource = null;
                return;
            }

            

            int securityGroupID = currentSecurityGroupID();
            List<KMPlatform.Entity.ServiceFeature.SecurityGroupTreeListRow> allItems = IsUpdate
                ? new KMPlatform.BusinessLogic.ServiceFeature().GetSecurityGroupTreeList(securityGroupID, clientGroupID, clientID)
                : new KMPlatform.BusinessLogic.ServiceFeature().GetEmptySecurityGroupTreeList(clientGroupID, clientID);
            PreprocessSecurityGroupAccessTreeData(allItems);
            tlSecurityGroupAccess.DataSource = allItems;
            tlSecurityGroupAccess.DataBind();
            tlSecurityGroupAccess.ExpandAllItems();
        }

        void PreprocessSecurityGroupAccessTreeData(List<KMPlatform.Entity.ServiceFeature.SecurityGroupTreeListRow> allItems)
        {
            Dictionary<Tuple<int, int?>, bool> enabled = new Dictionary<Tuple<int, int?>, bool>();
            Dictionary<int, bool> sEnabled = new Dictionary<int, bool>();
            Dictionary<int, bool> fEnabled = new Dictionary<int, bool>();
            Func<int, int?, bool> isEnabled = null; isEnabled = (sid, fid) =>
             {
                 Tuple<int, int?> key = new Tuple<int, int?>(sid, fid);
                 if (false == enabled.ContainsKey(key))
                 {
                     if (fid.HasValue && fid > 0)
                     {
                         enabled[key] = allItems.Any(x => x.ServiceID == sid && x.ServiceFeatureID == fid && x.ServiceFeatureAccessMapID > 0 && x.IsEnabled);
                     }
                     else
                     {
                         enabled[key] = allItems.Any(x => x.ServiceID == sid && x.ServiceFeatureID > 0 && isEnabled(sid, x.ServiceFeatureID));
                     }
                 }
                 return enabled[key];
             };
            allItems.ForEach(x => x.IsEnabled = String.IsNullOrEmpty(x.AccessLevelName) ? isEnabled(x.ServiceID, x.ServiceFeatureID) : x.IsEnabled);
        }

        void ShowHidePannels()
        {
            bool isChannelAdmin = false;
            if (KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
            {
                isChannelAdmin = true;
                IsActiveRadioButtonPanel.Visible = true;
            }
            else
            {
                IsChannelWideRadioButtonPanel.Visible = false;
                rbIsChannelWide.SelectedIndex = 1; // no
            }

            switch (rbIsChannelWide.SelectedIndex)
            {
                case 1: // no
                    ClientGroupDropDownPanel.Visible = isChannelAdmin;
                    ClientDropDownPanel.Visible = true;
                    if (!IsUpdate)
                        drpClient.Enabled = drpClient.Items.Count > 1;
                    break;
                default:
                    ClientDropDownPanel.Visible = false;
                    ClientGroupDropDownPanel.Visible = true;
                    if (!IsUpdate)
                        drpclientgroup.Enabled = drpclientgroup.Items.Count > 1;
                    break;
            }



            LoadSecurityGroupAccessTreeView();
        }

        private void ThrowECNException(string message, PlaceHolder phError, Label lblErrorMessage)
        {
            var ecnError = new ECNCommon.ECNError(ECNCommon.Enums.Entity.User, ECNCommon.Enums.Method.Get, message);
            var errorList = new List<ECNCommon.ECNError> { ecnError };
            SetECNError(new ECNCommon.ECNException(errorList, ECNCommon.Enums.ExceptionLayer.WebSite), phError, lblErrorMessage);
        }

        private void SetECNError(ECNCommon.ECNException ecnException, PlaceHolder phError, Label lblErrorMessage)
        {
            phError.Visible = true;
            lblErrorMessage.Text = string.Empty;
            foreach (var ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = $"{lblErrorMessage.Text}<br/>{ecnError.Entity}: {ecnError.ErrorMessage}";
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            var securityGroupId = currentSecurityGroupID();
            var currentClientID = Convert.ToInt32(drpClient.SelectedValue.ToString());
            var customer = GetCustomer(currentClientID);

            if (customer != null && customer.ActiveFlag.Equals("y", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var beforeSecurityGroup = GetSecurityGroup(securityGroupId);

                    if (tbSecurityGroupName.Text.Trim().Equals("administrator", StringComparison.OrdinalIgnoreCase)
                        || tbSecurityGroupName.Text.Trim().Equals("channel administrator", StringComparison.OrdinalIgnoreCase))
                    {
                        ThrowECNException("Invalid Security Group Name", phError, lblErrorMessagePhError);
                        ProductFeatureUpdatePannel.Update();
                        return;
                    }
                    else
                    {
                        string errorMesage;
                        var isValid = ValidateSecurityGropuId(securityGroupId, out errorMesage);

                        if (isValid)
                        {
                            ThrowECNException(errorMesage, phError, lblErrorMessagePhError);
                            ProductFeatureUpdatePannel.Update();
                            return;
                        }
                    }

                    UpdateCurrentSecurityGroup(securityGroupId, beforeSecurityGroup.CreatedByUserID);
                    UpdateSecurytyPermissionsFromTreeList();

                    Response.Redirect(DefaultRedirectDestination);
                }
                catch (ECNCommon.ECNException ecn)
                {
                    SetECNError(ecn, phError, lblErrorMessagePhError);
                    ProductFeatureUpdatePannel.Update();
                }
            }
            else
            {
                Response.Redirect(DefaultRedirectDestination);
            }
        }

        private void UpdateCurrentSecurityGroup(int securityGroupId, int securityGroupCreatedByUserId)
        {
            CurrentSecurityGroup = new KMPlatform.Entity.SecurityGroup
            {
                SecurityGroupName = tbSecurityGroupName.Text,
                SecurityGroupID = securityGroupId,
                Description = tbSecurityGroupDescription.Text,
                IsActive = rbIsActive.SelectedValue.Equals("1") ? true : false,
                ClientGroupID = rbIsChannelWide.SelectedIndex == 0 ? Int32.Parse(drpclientgroup.SelectedValue) : 0,
                ClientID = rbIsChannelWide.SelectedIndex != 0 ? Int32.Parse(drpClient.SelectedValue) : 0,
                CreatedByUserID = IsUpdate ? securityGroupCreatedByUserId : Master.UserSession.CurrentUser.UserID,
                UpdatedByUserID = IsUpdate == false ? (int?)null : Master.UserSession.CurrentUser.UserID
            };

            CurrentSecurityGroup.SecurityGroupID = new KMPlatform.BusinessLogic.SecurityGroup().Save(CurrentSecurityGroup);
        }

        private bool ValidateSecurityGropuId(int securityGroupId, out string errorMessage)
        {
            var exists = false;
            errorMessage = string.Empty;
            if (rbIsChannelWide.SelectedIndex == 0)
            {
                // Client Group Role
                if (new KMPlatform.BusinessLogic.SecurityGroup().ExistsForClient_ClientGroup(tbSecurityGroupName.Text.Trim(), Convert.ToInt32(drpclientgroup.SelectedValue.ToString()), -1, securityGroupId))
                {
                    exists = true;
                    errorMessage = "Security Group Name already exists for Base Channel";
                }
            }
            else
            {
                // Client Role
                if (new KMPlatform.BusinessLogic.SecurityGroup().ExistsForClient_ClientGroup(tbSecurityGroupName.Text.Trim(), -1, Convert.ToInt32(drpClient.SelectedValue.ToString()), securityGroupId))
                {
                    exists = true;
                    errorMessage = "Security Group Name already exists for Customer";
                }
            }

            return exists;
        }

        private bool IsServiceEnabledInSecurityGroupAccess(int serviceID, Dictionary<int, bool> cache)
        {
            if (!cache.ContainsKey(serviceID))
            {
                var serviceIdString = $"S{serviceID.ToString()}";
                var serviceDataItem = tlSecurityGroupAccess.Items.FirstOrDefault(di => di["ID"].Text == serviceIdString);
                cache[serviceID] = serviceDataItem != null && serviceDataItem.Selected;
            }
            return cache[serviceID];
        }

        private void UpdateSecurytyPermissionsFromTreeList()
        {
            var services = new Dictionary<int, bool>();
            var features = new Dictionary<int, bool>();
            var permissionBusinessLogic =
                new KMPlatform.BusinessLogic.SecurityGroupPermission();

            var now = DateTime.Now;
            var userID = Master.UserSession.CurrentUser.UserID;

            foreach (var item in tlSecurityGroupAccess.Items)
            {
                var mapId = GetItemIdOrDefault(item, "MAPID");
                var serviceId = GetItemIdOrDefault(item, "ServiceID");
                var featureId = GetItemIdOrDefault(item, "ServiceFeatureID");
                var accessId = GetItemIdOrDefault(item, "ServiceFeatureAccessMapID");

                var selected = item.Selected;

                var mapType = FindMapType(serviceId, featureId, accessId);

                var enabled = mapType == TreeDataItemMapReferenceType.Access ?
                                        IsServiceEnabledInSecurityGroupAccess(serviceId, services) && IsServiceEnabledInSecurityGroupAccess(featureId, features) && selected
                                        : (mapType == TreeDataItemMapReferenceType.Feature ? IsServiceEnabledInSecurityGroupAccess(serviceId, services) && selected : selected);

                switch (mapType)
                {
                    case TreeDataItemMapReferenceType.Service:
                        services[serviceId] = enabled;
                        break;

                    case TreeDataItemMapReferenceType.Feature:
                        features[featureId] = enabled;
                        break;

                    case TreeDataItemMapReferenceType.Access:
                        permissionBusinessLogic.Save(new KMPlatform.Entity.SecurityGroupPermission
                        {
                            SecurityGroupPermissionID = mapId,
                            ServiceFeatureAccessMapID = accessId,
                            SecurityGroupID = CurrentSecurityGroup.SecurityGroupID,
                            IsActive = enabled,
                            CreatedByUserID = userID,
                            UpdatedByUserID = userID,
                            CreatedDate = now,
                            UpdatedDate = now,
                        });
                        break;
                }

            }
        }

        private static TreeDataItemMapReferenceType FindMapType(int sid, int fid, int aid)
        {
            var treeDataItemMapReferenceTypeFromService = sid > 0 ? TreeDataItemMapReferenceType.Service : TreeDataItemMapReferenceType.ERROR;
            var treeDataItemMapReferenceTypeFromServiceOrFeature = (fid > 0 & sid > 0 ? TreeDataItemMapReferenceType.Feature : treeDataItemMapReferenceTypeFromService);
            var mapType = aid > 0 ?
                    TreeDataItemMapReferenceType.Access :
                    treeDataItemMapReferenceTypeFromServiceOrFeature;
            return mapType;
        }

        private static int GetItemIdOrDefault(TreeListDataItem item, string columnName)
        {
            int mid;
            Int32.TryParse(item[columnName].Text, out mid);
            return mid;
        }

        private static ECN_Framework_Entities.Accounts.Customer GetCustomer(int currentClientId)
        {
            ECN_Framework_Entities.Accounts.Customer customer = null;
            if (currentClientId > 0)
            {
                customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(currentClientId, false);
            }

            return customer;
        }

        private static KMPlatform.Entity.SecurityGroup GetSecurityGroup(int securityGroupId)
        {
            KMPlatform.Entity.SecurityGroup before = null;

            if (securityGroupId > 0)
            {
                before = new KMPlatform.BusinessLogic.SecurityGroup().Select(securityGroupId, false);
            }

            return before;
        }
    }
}