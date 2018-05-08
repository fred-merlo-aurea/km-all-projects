using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using ECN_Framework_Common.Objects;
using KMPlatform.Entity;
using AccountsEntity = ECN_Framework_Entities.Accounts;
using BusinessBaseChannel = ECN_Framework_BusinessLayer.Accounts.BaseChannel;
using BusinessCustomer = ECN_Framework_BusinessLayer.Accounts.Customer;
using ClientServiceFeatureMap = KMPlatform.BusinessLogic.ClientServiceFeatureMap;
using ClientServiceMap = KMPlatform.BusinessLogic.ClientServiceMap;
using SecurityGroup = KMPlatform.BusinessLogic.SecurityGroup;
using BusinessCustomerConfig = ECN_Framework_BusinessLayer.Accounts.CustomerConfig;

namespace ecn.accounts.customersmanager
{
    public class CustomerEntryResult
    {
        public int UserId { get; set; }
        public int ClientId { get; set; }
        public int BaseChannelId { get; set; }
    }

    public class ClientUpdateResult
    {
        public bool IsUpdate { get; set; }
        public bool Rollback { get; set; }
        public Exception SavedException { get; set; }
        public Client ClientBeforeUpdate { get; set; }
        public KMPlatform.BusinessLogic.Client ClientBusinessLogic { get; set; }
    }

    public partial class customerdetail
    {
        private const string SecurityGroupTemplateName = "Administrator";

        private Dictionary<int, bool> _dServices;

        public void btnSave_Click(object sender, EventArgs e)
        {
            var customerEntryIds = PrepareCustomerEntry();
            if (!ValidateCustomer())
            {
                return;
            }

            var currentClientObj = CreateClientEntry(customerEntryIds.ClientId);
            var updateResult = ProcessClientUpdate(currentClientObj, customerEntryIds);

            if (updateResult.Rollback)
            {
                RollBackSaveOperation(updateResult);
            }

            if (updateResult.SavedException != null)
            {
                throw updateResult.SavedException;
            }

            CreateAssertFolders();

            if (!updateResult.IsUpdate)
            {
                NotifyOnNewCustomer();
            }

            ECN_Framework_BusinessLayer.Accounts.Customer.ClearCustomersCache_ByChannelID(customerEntryIds.BaseChannelId);
            Master.UserSession.ClearSession();
            Response.Redirect("default.aspx");
        }

        private void NotifyOnNewCustomer()
        {
            bool proceedMta;
            var emails = "";
            try
            {
                emails = ConfigurationManager.AppSettings["MTA_ToEmail"];
                proceedMta = true;
            }
            catch
            {
                proceedMta = false;
            }

            if (!proceedMta || string.IsNullOrEmpty(emails))
            {
                return;
            }

            var edMta = new ECN_Framework_Entities.Communicator.EmailDirect
            {
                Source = "ecn.Accounts.CustomerDetail",
                Process = "New Customer MTA Email",
                CreatedDate = DateTime.Now,
                SendTime = DateTime.Now,
                CustomerID = 1,
                CreatedUserID = Master.UserSession.CurrentUser.UserID,
                EmailAddress = emails
            };

            var bc = BusinessBaseChannel.GetByBaseChannelID(customer.BaseChannelID.Value);

            edMta.Content = GetMTAEmail_Content(customer.CustomerName,
                customer.CustomerID,
                bc.BaseChannelName,
                customer.BaseChannelID.Value);
            edMta.EmailSubject = "New Customer Added to KM Platform";

            edMta.FromEmailAddress = "emaildirect@ecn5.com";
            edMta.FromName = "Knowledge Marketing";
            edMta.ReplyEmailAddress = "info@knowledgemarketing.com";
            edMta.Status = "Pending";
            ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(edMta);
        }

        private void CreateAssertFolders()
        {
            var customerHomeDir = $"{ConfigurationManager.AppSettings["Images_VirtualPath"]}/customers/{ID}";

            if (!Directory.Exists(Server.MapPath(customerHomeDir)))
            {
                Directory.CreateDirectory(Server.MapPath(customerHomeDir));
            }

            var dataPath = $"{customerHomeDir}/data";

            if (!Directory.Exists(Server.MapPath(dataPath)))
            {
                Directory.CreateDirectory(Server.MapPath(dataPath));
            }

            var imagePath = $"{customerHomeDir}/images";

            if (!Directory.Exists(Server.MapPath(imagePath)))
            {
                Directory.CreateDirectory(Server.MapPath(imagePath));
            }
        }

        private void RollBackSaveOperation(ClientUpdateResult updateResult)
        {
            try
            {
                updateResult.ClientBusinessLogic.Save(updateResult.ClientBeforeUpdate);
            }
            catch (Exception ee)
            {
                if (null == updateResult.SavedException)
                {
                    updateResult.SavedException = new ApplicationException("rollback of Client failed", ee);
                }
                else
                {
                    updateResult.SavedException =
                        new AggregateException("rollback of Client failed", updateResult.SavedException, ee);
                }

                ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(
                    new ECN_Framework_Entities.Communicator.EmailDirect
                    {
                        CustomerID = 1, // KM
                        Status = "Pending",
                        SendTime = DateTime.Now,
                        EmailAddress = "dev-group@knowledgemarketing.com",
                        EmailSubject = "ECN.Accounts - Customer Update Rollback Failure",
                        Content = $@"<html>
                                <head></head>
                                <body>
                                    <h1>Warning: <pre>Rollback of ClientID <em>{
                                customer.PlatformClientID
                            }</em> after failure updating CustomerID {customer.CustomerID}</pre></h1>
                                    <h2>Source: main/customers/customerdetail.aspx</h2>
                                    <div>Please review the Client and Customer records for any descrepencies.</div>
                                </body>
                                <html>",
                        FromName = "ECN.Accounts",
                        ReplyEmailAddress = "ecn.accounts-no-reply@ecn5.com",
                        Source = "main/customer/customereditor.aspx?CustomerID=" + customer.CustomerID,
                        Process = "ECN.Accounts"
                    });
            }
        }

        private ClientUpdateResult ProcessClientUpdate(
            Client currentClientObj,
            CustomerEntryResult customerEntryIds
            )
        {
            var result = new ClientUpdateResult();
            var completedClientUpdate = false;
            result.IsUpdate = customer.CustomerID > 0;
            result.ClientBusinessLogic = new KMPlatform.BusinessLogic.Client();

            var clientServiceMapBusinessLogic = new ClientServiceMap();
            var clientServiceFeatureMapBusinessLogic = new ClientServiceFeatureMap();
            var securityGroupBusinessLogic = new SecurityGroup();
            var clientGroupClientMapBusinessLogic = new KMPlatform.BusinessLogic.ClientGroupClientMap();

            try
            {
                if (customer.PlatformClientID > 0)
                {
                    result.ClientBeforeUpdate = result.ClientBusinessLogic.Select(customer.PlatformClientID);
                }

                customer.PlatformClientID = result.ClientBusinessLogic.Save(currentClientObj, true, getClientGroupID());

                // update the hidden field in case something goes wrong inserting the base-channel record
                // this will prevent us from needlessly inserting orphan client-group records (e.g. without
                // a corresponding BaseChannel record in ECN.   Alternately, we could delete during
                // rollback if inserting a customer to ECN fails...
                hfCustomerPlatformClientID.Value = customer.PlatformClientID.ToString();
                if (result.ClientBeforeUpdate != null)
                {
                    completedClientUpdate = true;
                }

                // insert/update ECN5_Accounts.dbo.Customer
                customer.CustomerID = BusinessCustomer.Save(customer, Master.UserSession.CurrentUser);
                ClientGroupClientMapBusinessLogic(result, clientGroupClientMapBusinessLogic);
                SaveClientServiceMapWithFeatures(
                    customerEntryIds,
                    clientServiceMapBusinessLogic,
                    clientServiceFeatureMapBusinessLogic);
                SaveCustomerConfigs();
                CreateAdministratorSecurityGroup(result, securityGroupBusinessLogic);
            }
            catch (Exception ee)
            {
                result.SavedException = ee;
                if (completedClientUpdate)
                {
                    result.Rollback = true;
                }
            }

            return result;
        }

        private void CreateAdministratorSecurityGroup(ClientUpdateResult result, SecurityGroup securityGroupBusinessLogic)
        {
            if (!result.IsUpdate)
            {
                securityGroupBusinessLogic.CreateFromTemplateForClient(
                    SecurityGroupTemplateName,
                    0,
                    customer.PlatformClientID,
                    SecurityGroupTemplateName,
                    Master.UserSession.CurrentUser);

                var defaultTemplates = KMPlatform.BusinessLogic.SecurityGroupTemplate.GetNonAdminTemplates();
                foreach (var sgt in defaultTemplates)
                {
                    securityGroupBusinessLogic.CreateFromTemplateForClient(
                        sgt.SecurityGroupName,
                        0,
                        customer.PlatformClientID,
                        string.Empty,
                        Master.UserSession.CurrentUser);
                }
            }
            else
            {
                securityGroupBusinessLogic.UpdateAdministrators(
                    customer.PlatformClientID,
                    getClientGroupID(),
                    Master.UserSession.CurrentUser.UserID);
            }
        }

        private void SaveCustomerConfigs()
        {
            if (string.IsNullOrWhiteSpace(txtPickupPath.Text))
            {
                BusinessCustomerConfig.Delete(customer.CustomerID, Master.UserSession.CurrentUser);
            }
            else
            {
                var customerPickupConfig = new AccountsEntity.CustomerConfig();
                var customerConfigs =
                    BusinessCustomerConfig.GetByCustomerID(customer.CustomerID, Master.UserSession.CurrentUser);
                UpdatePickupConfig(customerConfigs, customerPickupConfig);
                UpdateMailingConfig(customerConfigs);
            }
        }

        private void UpdateMailingConfig(List<AccountsEntity.CustomerConfig> customerConfigs)
        {
            var customerMailingConfig = new AccountsEntity.CustomerConfig();

            var cmConfig = customerConfigs.Find(
                x => x.ProductID == 100 &&
                     x.ConfigName ==
                     ECN_Framework_Common.Objects.Accounts.Enums.ConfigName.MailingIP.ToString());
            if (cmConfig != null)
            {
                customerMailingConfig.CustomerConfigID = cmConfig.CustomerConfigID;
                customerMailingConfig.UpdatedUserID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);
            }
            else
            {
                customerMailingConfig.CreatedUserID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);
            }

            customerMailingConfig.CustomerID = customer.CustomerID;
            customerMailingConfig.ProductID = 100;
            customerMailingConfig.ConfigName =
                ECN_Framework_Common.Objects.Accounts.Enums.ConfigName.MailingIP.ToString();
            customerMailingConfig.ConfigValue = txtMailingIP.Text;

            BusinessCustomerConfig.Save(customerMailingConfig,
                Master.UserSession.CurrentUser);
        }

        private void UpdatePickupConfig(List<AccountsEntity.CustomerConfig> customerConfigs, AccountsEntity.CustomerConfig customerPickupConfig)
        {
            var cpConfig = customerConfigs.Find(
                x => x.ProductID == 100 &&
                     x.ConfigName ==
                     ECN_Framework_Common.Objects.Accounts.Enums.ConfigName.PickupPath.ToString());
            if (cpConfig != null)
            {
                customerPickupConfig.CustomerConfigID = cpConfig.CustomerConfigID;
                customerPickupConfig.UpdatedUserID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);
            }
            else
            {
                customerPickupConfig.CreatedUserID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);
            }

            customerPickupConfig.CustomerID = customer.CustomerID;
            customerPickupConfig.ProductID = 100;
            customerPickupConfig.ConfigName =
                ECN_Framework_Common.Objects.Accounts.Enums.ConfigName.PickupPath.ToString();
            customerPickupConfig.ConfigValue = txtPickupPath.Text;

            BusinessCustomerConfig.Save(customerPickupConfig, Master.UserSession.CurrentUser);
        }

        private bool IsServiceEnabled(int serviceId)
        {
            if (!_dServices.ContainsKey(serviceId))
            {
                var serviceIdString = $"S{serviceId}";
                var serviceDataItem = tlClientServiceFeatures.Items.FirstOrDefault(di => di["ID"].Text == serviceIdString);
                _dServices[serviceId] = serviceDataItem != null && serviceDataItem.Selected;
            }

            return _dServices[serviceId];
        }

        private void SaveClientServiceMapWithFeatures(
            CustomerEntryResult customerEntryIds,
            ClientServiceMap clientServiceMapBusinessLogic,
            ClientServiceFeatureMap clientServiceFeatureMapBusinessLogic)
        {
            _dServices = new Dictionary<int, bool>();

            var now = DateTime.Now;
            var alreadyDone = new List<KMPlatform.Entity.ClientServiceFeatureMap>();
            SaveAdditionalCosts(
                customerEntryIds,
                clientServiceMapBusinessLogic,
                clientServiceFeatureMapBusinessLogic,
                now,
                alreadyDone);

            SaveNonAdditionalCostsForNewCustomer(
                customerEntryIds,
                clientServiceFeatureMapBusinessLogic,
                alreadyDone,
                now);
        }

        private void SaveNonAdditionalCostsForNewCustomer(
            CustomerEntryResult customerEntryIds,
            ClientServiceFeatureMap clientServiceFeatureMapBusinessLogic,
            List<KMPlatform.Entity.ClientServiceFeatureMap> alreadyDone,
            DateTime now)
        {
            var nonAdditionalCost =
                new KMPlatform.BusinessLogic.ServiceFeature().GetClientTreeList(getClientGroupID(), -1, false);

            foreach (var cgtlr in nonAdditionalCost)
            {
                if (cgtlr.ServiceFeatureID <= 0 ||
                    alreadyDone.Any(x => x.ServiceFeatureID == cgtlr.ServiceFeatureID))
                {
                    continue;
                }

                var csfm = new KMPlatform.Entity.ClientServiceFeatureMap
                {
                    ClientID = customer.PlatformClientID,
                    ServiceFeatureID = cgtlr.ServiceFeatureID,
                    IsEnabled = IsServiceEnabled(cgtlr.ServiceID),
                    DateCreated = now,
                    DateUpdated = now,
                    CreatedByUserID = customerEntryIds.UserId
                };

                clientServiceFeatureMapBusinessLogic.Save(csfm);
            }
        }

        private void SaveAdditionalCosts(
            CustomerEntryResult customerEntryIds,
            ClientServiceMap clientServiceMapBusinessLogic,
            ClientServiceFeatureMap clientServiceFeatureMapBusinessLogic,
            DateTime now,
            ICollection<KMPlatform.Entity.ClientServiceFeatureMap> alreadyDone)
        {
            foreach (var item in tlClientServiceFeatures.Items)
            {
                int mid;
                int sid;
                int fid;
                int.TryParse(item["MAPID"].Text, out mid);
                int.TryParse(item["ServiceID"].Text, out sid);
                int.TryParse(item["ServiceFeatureID"].Text, out fid);

                bool? isFeature = null;
                if (sid > 0 && fid > 0)
                {
                    isFeature = true;
                }
                else if (fid == 0 && sid > 0)
                {
                    isFeature = false;
                }

                bool isAdditionalCost;
                bool.TryParse(item["IsAdditionalCost"].Text, out isAdditionalCost);
                var selected = item.Selected;
                var enabled = false == isFeature
                    ? selected
                    : IsServiceEnabled(sid) && (selected || !isAdditionalCost);
                var diagMessage =
                    $"{mid}: {sid}.{fid} [isFeature: {isFeature,-5}, selected: {selected,-5}, enabled: {enabled,-5}, isAdditionalCost: {isAdditionalCost,-5}";
                System.Diagnostics.Trace.TraceInformation(diagMessage);

                if (isFeature == false)
                {
                    _dServices[sid] = selected;
                    clientServiceMapBusinessLogic.Save(new KMPlatform.Entity.ClientServiceMap
                    {
                        ClientServiceMapID = mid,
                        ClientID = customer.PlatformClientID,
                        ServiceID = sid,
                        IsEnabled = enabled,
                        DateCreated = now,
                        DateUpdated = now,
                        CreatedByUserID = customerEntryIds.UserId,
                        UpdatedByUserID = customerEntryIds.UserId
                    });
                }
                else if (isFeature == true)
                {
                    var csfm = new KMPlatform.Entity.ClientServiceFeatureMap
                    {
                        ClientServiceFeatureMapID = mid,
                        ClientID = customer.PlatformClientID,
                        ServiceFeatureID = fid,
                        IsEnabled = enabled,
                        DateCreated = now,
                        DateUpdated = now,
                        CreatedByUserID = customerEntryIds.UserId,
                        UpdatedByUserID = customerEntryIds.UserId
                    };
                    clientServiceFeatureMapBusinessLogic.Save(csfm);
                    alreadyDone.Add(csfm);
                }
            }
        }

        private void ClientGroupClientMapBusinessLogic(
            ClientUpdateResult result,
            KMPlatform.BusinessLogic.ClientGroupClientMap clientGroupClientMapBusinessLogic)
        {
            if (!result.IsUpdate)
            {
                var cgcm = new ClientGroupClientMap
                {
                    ClientGroupID = getClientGroupID(),
                    ClientID = customer.PlatformClientID,
                    IsActive = true,
                    DateCreated = DateTime.Now,
                    CreatedByUserID = Master.UserSession.CurrentUser.UserID
                };
                clientGroupClientMapBusinessLogic.Save(cgcm);
            }
            else
            {
                try
                {
                    var cgcm = new KMPlatform.BusinessLogic.ClientGroupClientMap()
                        .SelectForClientID(customer.PlatformClientID)
                        .FirstOrDefault(x => x.ClientGroupID == getClientGroupID());
                    if (cgcm != null)
                    {
                        cgcm.IsActive = customer.ActiveFlag.Equals("y", StringComparison.InvariantCultureIgnoreCase);
                        cgcm.DateUpdated = DateTime.Now;
                        cgcm.UpdatedByUserID = Master.UserSession.CurrentUser.UserID;
                        clientGroupClientMapBusinessLogic.Save(cgcm);
                    }
                }
                catch
                {
                    //POSSIBLE BUG: No exception logging
                }
            }
        }

        private Client CreateClientEntry(int clientId)
        {
            var currentClientObj = new Client
            {
                ClientID = clientId,
                ClientName = customer.CustomerName,
                DisplayName = customer.CustomerName,
                IsActive = customer.ActiveFlag.Equals("y", StringComparison.InvariantCultureIgnoreCase),
                ClientCode = customer.CustomerID.ToString(),
                ParentClientId = 0,
                IsKMClient = false
            };
            if (clientId > 0)
            {
                currentClientObj.DateUpdated = DateTime.Now;
                currentClientObj.UpdatedByUserID = Master.UserSession.CurrentUser.UserID;
            }
            else
            {
                currentClientObj.DateCreated = DateTime.Now;
                currentClientObj.CreatedByUserID = Master.UserSession.CurrentUser.UserID;
            }

            return currentClientObj;
        }

        private bool ValidateCustomer()
        {
            try
            {
                BusinessCustomer.Validate(customer, Master.UserSession.CurrentUser);
            }
            catch (ECNException ecnex)
            {
                var sb = new StringBuilder();

                foreach (var err in ecnex.ErrorList)
                {
                    sb.Append($"{err.ErrorMessage}<BR>");
                }

                lblErrorMessage.Text = sb.ToString();
                phError.Visible = true;
                return false;
            }

            return true;
        }

        private CustomerEntryResult PrepareCustomerEntry()
        {
            var result = GetCustomerEntryResult();

            customer = new AccountsEntity.Customer
            {
                PlatformClientID = result.ClientId,
                CustomerID = getCustomerID(),
                BaseChannelID = result.BaseChannelId,
                CustomerName = txtCustomerName.Text,
                GeneralContant = GeneralContact.Contact,
                BillingContact = BillingContact.Contact,
                ActiveFlag = cbActiveStatus.Checked ? "Y" : "N",
                DemoFlag = cbDemoCustomer.Checked ? "Y" : "N",
                WebAddress = txtWebAddress.Text,
                TechContact = txttechContact.Text,
                TechEmail = txttechEmail.Text,
                TechPhone = txttechPhone.Text,
                SubscriptionsEmail = txtSubscriptionEmail.Text,
                CustomerType = ddlCustomerType.SelectedValue,
                AccountExecutiveID = ParseOrNull(ddlAccountExecutive.SelectedItem.Value),
                AccountManagerID = ParseOrNull(ddlAccountManager.SelectedItem.Value)
            };
            InitializeUnsafeProperties(customer);

            return result;
        }

        private void InitializeUnsafeProperties(AccountsEntity.Customer customer)
        {
            try
            {
                customer.IsStrategic = rblStrategic.SelectedItem.Value == "Y";
            }
            catch
            {
                customer.IsStrategic = false;
            }

            try
            {
                customer.ABWinnerType = ddlAbWinnerType.SelectedValue;
            }
            catch
            {
                customer.ABWinnerType = "clicks";
            }

            try
            {
                customer.DefaultBlastAsTest = chkDefaultBlastAsTest.Checked;
            }
            catch
            {
                customer.DefaultBlastAsTest = false;
            }

            customer.AccountsLevel = "0";
            var msCustomerId = int.Parse(ddlMSCustomer.SelectedItem.Value);
            if (msCustomerId > 0)
            {
                customer.MSCustomerID = msCustomerId;
            }
            else
            {
                customer.MSCustomerID = null;
            }

            if (customer.CustomerID > 0)
            {
                customer.UpdatedUserID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);
            }
            else
            {
                customer.CreatedUserID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);
            }
        }

        private static int? ParseOrNull(string value)
        {
            int outVal;
            return int.TryParse(value, out outVal) ? (int?) outVal : null;
        }

        private CustomerEntryResult GetCustomerEntryResult()
        {
            var result = new CustomerEntryResult
            {
                UserId = Master.UserSession.CurrentUser.UserID
            };

            int clientId;
            int.TryParse(hfCustomerPlatformClientID.Value, out clientId);
            result.ClientId = clientId;

            int baseChannelId;
            int.TryParse(ddlBaseChannels.SelectedItem.Value, out baseChannelId);
            result.BaseChannelId = baseChannelId;
            return result;
        }
    }
}