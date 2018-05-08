using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using ecn.accounts.includes.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using KMPlatform.DataAccess.Fakes;
using KMEntities = KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;
using Telerik.Web.UI;
using Telerik.Web.UI.Fakes;
using ServiceFeatures = KMPlatform.Entity.ServiceFeature;
using KMBusinessFakes = KMPlatform.BusinessLogic.Fakes;
using ECNAccountDataFakes = ECN_Framework_DataLayer.Accounts.Fakes;
using ECNDataAccessFakes = ECN_Framework_DataLayer.Fakes;
using static ECN_Framework_Common.Objects.Accounts.Enums;
using static NUnit.Framework.TestContext;
using System.Diagnostics;
using System.IO;
using ShimKMCommonDataFunctions = KM.Common.Fakes.ShimDataFunctions;

namespace ECN.Accounts.Tests.main.Customers
{
    public partial class CustomerDetailTest
    {
        [Test]
        public void btnSave_Click_NotValidCustomer_RetrunsWithErrorMessage()
        {
            // Arrange
            SetPageControls();
            SetPageDropDownLists();
            ShimCustomer.ValidateCustomerUser = (c, u) => throw new ECNException(
                TestExceptionMessage, 
                new List<ECNError>
                {
                    new ECNError
                    {
                        ErrorMessage = SampleCustomerValidationMessage
                    }
                });

            // Act
            _testEntity.btnSave_Click(this, EventArgs.Empty);

            // Assert
            Get<Label>(_privateTestObj, "lblErrorMessage").Text.ShouldContain(SampleCustomerValidationMessage);
            Get<PlaceHolder>(_privateTestObj, "phError").Visible.ShouldBeTrue();
            _sqlCommandExecutedList.ShouldBeEmpty();
        }

        [Test]
        public void btnSave_Click_ValidCustomer_SavesClientCustomerAndRelatedEntities()
        {
            // Arrange
            SetPageControls();
            SetPageDropDownLists();
            SetUpFakes();

            // Act
            _testEntity.btnSave_Click(this, EventArgs.Empty);

            // Assert
            _isCustomerValidated.ShouldBeTrue();
            _isClientSaved.ShouldBeTrue();
            _savedClient.ShouldSatisfyAllConditions(
                () => _savedClient.ShouldNotBeNull(),
                () => _savedClient.UpdatedByUserID.ShouldBe(_testEntity.Master.UserSession.CurrentUser.UserID),
                () => _savedClient.ClientID.ShouldBe(1),
                () => _savedClient.ClientName.ShouldBe(SampleCustomer),
                () => _savedClient.DisplayName.ShouldBe(SampleCustomer),
                () => _savedClient.IsActive.ShouldBeTrue(),
                () => _savedClient.IsKMClient.ShouldBeFalse());
            _isGroupSaved.ShouldBeTrue();
            _savedGroup.ShouldSatisfyAllConditions(
                () => _savedGroup.CustomerID.ShouldBe(1),
                () => _savedGroup.GroupName.ShouldBe("Master Suppression"),
                () => _savedGroup.OwnerTypeCode.ShouldBe("customer"),
                () => _savedGroup.MasterSupression.ShouldBe(1),
                () => _savedGroup.AllowUDFHistory.ShouldBe("N"),
                () => _savedGroup.IsSeedList.Value.ShouldBeFalse());
            _sqlCommandExecutedList.ShouldSatisfyAllConditions(
                () => _sqlCommandExecutedList.ShouldContain("e_Customer_Save"),
                () => _sqlCommandExecutedList.ShouldContain("e_BillingContact_Save"),
                () => _sqlCommandExecutedList.ShouldContain("e_ClientGroupClientMap_Save"),
                () => _sqlCommandExecutedList.ShouldContain("e_ClientServiceFeatureMap_Save"),
                () => _sqlCommandExecutedList.ShouldContain("e_CustomerConfig_Save"),
                () => _sqlCommandExecutedList.ShouldContain("e_CustomerConfig_Save"),
                () => _sqlCommandExecutedList.ShouldContain("e_EmailDirect_Save"));
            Directory.Exists($"{CurrentContext.TestDirectory}{CustomerRootDirectoryName}").ShouldBeTrue();
            RedirectUrl.ShouldContain("default.aspx");
        }

        [Test]
        public void btnSave_Click_ValidCustomerCustomerIDExistsAndRollBackTrue_SendsEmailThrowsExceptionand()
        {
            // Arrange
            SetPageControls();
            SetUpFakes();
            SetPageDropDownLists();
            QueryString.Add(QuerystringCustomerIDKey, CustomerID);
            ShimKMCommonDataFunctions.ExecuteNonQuerySqlCommandString = 
                (cmd, connStrName) => throw new InvalidOperationException(TestExceptionMessage);
            KMBusinessFakes.ShimClient.AllInstances.SaveClientBooleanNullableOfInt32 = (cl, c, isClientUpdated, i) =>
            {
                if (isClientUpdated)
                {
                    return 1;
                }
                throw new InvalidOperationException(TestExceptionMessage);
            };

            // Act
            var resultException = Should.Throw<AggregateException>(() => _testEntity.btnSave_Click(this, EventArgs.Empty));

            // Assert
            _isCustomerValidated.ShouldBeTrue();
            _isClientSaved.ShouldBeFalse();
            _savedClient.ShouldBeNull();
            _isGroupSaved.ShouldBeFalse();
            _savedGroup.ShouldBeNull();
            _sqlCommandExecutedList.ShouldSatisfyAllConditions(
                () => _sqlCommandExecutedList.ShouldContain("e_Customer_Exists_ByID"),
                () => _sqlCommandExecutedList.ShouldContain("e_Customer_Save"),
                () => _sqlCommandExecutedList.ShouldContain("e_BillingContact_Save"),
                () => _sqlCommandExecutedList.ShouldContain("e_ClientGroupClientMap_Save"),
                () => _sqlCommandExecutedList.ShouldContain("e_ClientServiceFeatureMap_Save"),
                () => _sqlCommandExecutedList.ShouldContain("e_CustomerConfig_Save"),
                () => _sqlCommandExecutedList.ShouldContain("e_CustomerConfig_Save"),
                () => _sqlCommandExecutedList.ShouldContain("e_EmailDirect_Save"));
            resultException.ShouldSatisfyAllConditions(
                () => resultException.InnerExceptions.Count.ShouldBe(2),
                () => resultException.InnerExceptions.ShouldContain(x => x.Message.Contains(TestExceptionMessage)));
        }

        [Test]
        public void btnSave_Click_ValidCustomerAndCustomerIDExists_SavesClientCustomerAndRelatedEntities()
        {
            // Arrange
            SetPageControls(clientId: "0");
            SetUpFakes();
            SetPageDropDownLists(ddlMSCustomerValue: "0");
            QueryString.Add(QuerystringCustomerIDKey, CustomerID);
            ConfigureGetEntitiesFakes(productID: 101);
            ConfigureRadTreeListControl();

            // Act
            _testEntity.btnSave_Click(this, EventArgs.Empty);

            // Assert
            _isCustomerValidated.ShouldBeTrue();
            _isClientSaved.ShouldBeTrue();
            _savedClient.ShouldSatisfyAllConditions(
                () => _savedClient.ShouldNotBeNull(),
                () => _savedClient.CreatedByUserID.ShouldBe(_testEntity.Master.UserSession.CurrentUser.UserID),
                () => _savedClient.UpdatedByUserID.ShouldBeNull(),
                () => _savedClient.ClientID.ShouldBe(1),
                () => _savedClient.ClientName.ShouldBe(SampleCustomer),
                () => _savedClient.DisplayName.ShouldBe(SampleCustomer),
                () => _savedClient.IsActive.ShouldBeTrue(),
                () => _savedClient.IsKMClient.ShouldBeFalse());
            _isGroupSaved.ShouldBeFalse();
            _savedGroup.ShouldBeNull();
            _sqlCommandExecutedList.ShouldSatisfyAllConditions(
                () => _sqlCommandExecutedList.ShouldContain("e_UserClientSecurityGroup_Insert_ClientGroupRoles"),
                () => _sqlCommandExecutedList.ShouldContain("e_Customer_Exists_ByID"),
                () => _sqlCommandExecutedList.ShouldContain("e_Customer_Save"),
                () => _sqlCommandExecutedList.ShouldContain("e_BillingContact_Save"),
                () => _sqlCommandExecutedList.ShouldContain("e_ClientGroupClientMap_Save"),
                () => _sqlCommandExecutedList.ShouldContain("e_ClientServiceFeatureMap_Save"),
                () => _sqlCommandExecutedList.ShouldContain("e_CustomerConfig_Save"),
                () => _sqlCommandExecutedList.ShouldContain("e_CustomerConfig_Save"),
                () => _sqlCommandExecutedList.ShouldContain("e_SecurityGroupPermission_UpdateAdministrators"));
            Directory.Exists($"{CurrentContext.TestDirectory}{CustomerRootDirectoryName}").ShouldBeTrue();
            RedirectUrl.ShouldContain("default.aspx");
        }

        [Test]
        public void btnSave_Click_ValidCustomerIsUpdateTrueRadControlWIthColumnValuesZero_SavesClientCustomerAndRelatedEntities()
        {
            // Arrange
            SetPageControls(clientId: "0");
            SetUpFakes();
            SetPageDropDownLists(ddlMSCustomerValue: "0");
            QueryString.Add(QuerystringCustomerIDKey, CustomerID);
            ConfigureGetEntitiesFakes(productID: 101);
            ConfigureRadTreeListControl(defaultColumnValues: "0");

            // Act
            _testEntity.btnSave_Click(this, EventArgs.Empty);

            // Assert
            _isCustomerValidated.ShouldBeTrue();
            _isClientSaved.ShouldBeTrue();
            _savedClient.ShouldSatisfyAllConditions(
                () => _savedClient.ShouldNotBeNull(),
                () => _savedClient.CreatedByUserID.ShouldBe(_testEntity.Master.UserSession.CurrentUser.UserID),
                () => _savedClient.UpdatedByUserID.ShouldBeNull(),
                () => _savedClient.ClientID.ShouldBe(1),
                () => _savedClient.ClientName.ShouldBe(SampleCustomer),
                () => _savedClient.DisplayName.ShouldBe(SampleCustomer),
                () => _savedClient.IsActive.ShouldBeTrue(),
                () => _savedClient.IsKMClient.ShouldBeFalse());
            _isGroupSaved.ShouldBeFalse();
            _savedGroup.ShouldBeNull();
            _sqlCommandExecutedList.ShouldSatisfyAllConditions(
                () => _sqlCommandExecutedList.ShouldContain("e_UserClientSecurityGroup_Insert_ClientGroupRoles"),
                () => _sqlCommandExecutedList.ShouldContain("e_Customer_Exists_ByID"),
                () => _sqlCommandExecutedList.ShouldContain("e_Customer_Save"),
                () => _sqlCommandExecutedList.ShouldContain("e_BillingContact_Save"),
                () => _sqlCommandExecutedList.ShouldContain("e_ClientGroupClientMap_Save"),
                () => _sqlCommandExecutedList.ShouldContain("e_ClientServiceMap_Save"),
                () => _sqlCommandExecutedList.ShouldContain("e_ClientServiceFeatureMap_Save"),
                () => _sqlCommandExecutedList.ShouldContain("e_CustomerConfig_Save"),
                () => _sqlCommandExecutedList.ShouldContain("e_CustomerConfig_Save"),
                () => _sqlCommandExecutedList.ShouldContain("e_SecurityGroupPermission_UpdateAdministrators"));
            Directory.Exists($"{CurrentContext.TestDirectory}{CustomerRootDirectoryName}").ShouldBeTrue();
            RedirectUrl.ShouldContain("default.aspx");
        }

        private void SetUpFakes()
        {
            _sqlCommandExecutedList.Clear();
            SetConfigFakes();
            SetEntitiesValidationFakes();
            SetSaveEntitiesFakes();
            ConfigureGetEntitiesFakes();

            KMBusinessFakes.ShimSecurityGroup.AllInstances.CreateFromTemplateForClientStringInt32Int32StringUser =
                (q, e, t, u, v, b) => 1;
            KMBusinessFakes.ShimSecurityGroupTemplate.GetNonAdminTemplates = () => new List<KMEntities.SecurityGroupTemplate>
            {
                new KMEntities.SecurityGroupTemplate{}
            };
            KM.Platform.Fakes.ShimUser.IsSystemAdministratorUser = (u) => true;
        }

        private void SetConfigFakes()
        {
            var settings = new NameValueCollection();
            settings.Add("MTA_ToEmail", "sample@sample.com");
            ShimConfigurationManager.AppSettingsGet = () => settings;
        }

        private void ConfigureGetEntitiesFakes(int productID = 100)
        {
            ShimClient.GetSqlCommand = (cmd) => new KMEntities.Client();
            ECNAccountDataFakes.ShimCustomerConfig.GetListSqlCommand = (cmd) => new List<CustomerConfig>
            {
                new CustomerConfig() { ProductID = productID , ConfigName = ConfigName.PickupPath.ToString() },
                new CustomerConfig() { ProductID = productID , ConfigName = ConfigName.MailingIP.ToString() }
            };
            ShimClientGroupClientMap.GetListSqlCommand = (cmd) => new List<KMEntities.ClientGroupClientMap>
            {
                new KMEntities.ClientGroupClientMap {ClientGroupID = 1, ClientID = 1,ClientGroupClientMapID = 1},
                new KMEntities.ClientGroupClientMap {ClientGroupID = 0, ClientID = 1,ClientGroupClientMapID = 1},
                new KMEntities.ClientGroupClientMap {ClientGroupID = -1, ClientID = 1,ClientGroupClientMapID = 1}
            };
            ShimServiceFeature.GetListOf1SqlCommand((cmd) => new List<ServiceFeatures.ClientGroupTreeListRow>
            {
                new ServiceFeatures.ClientGroupTreeListRow{
                    ID = "1",
                    PID = "1",
                    ServiceFeatureID = 1,
                    MAPID = 1,
                    ServiceID = 1,
                    IsAdditionalCost = true,
                    Description = "sampleDescription",
                    ServiceName = "SampleServiceName",
                    IsEnabled = true,
                    ServiceDisplayOrder = 1,
                    ServiceFeatureDisplayOrder = 1,
                    ServiceFeatureName = "sample"
                }
            });
            ShimBaseChannel.GetAll = () => new List<BaseChannel>
            {
                new BaseChannel{ BaseChannelID = 0 },
                new BaseChannel{ BaseChannelID = 1 },
            };
            ShimBaseChannel.GetByBaseChannelIDInt32 = (c) => new BaseChannel { BaseChannelID = 1 };
        }

        private void SetSaveEntitiesFakes()
        {
            ShimGroup.SaveGroupUser = (grp, u) => 
            {
                _isGroupSaved = true;
                _savedGroup = grp;
                return 1;
            };
            ShimClient.SaveClient = (client) =>
            {
                _isClientSaved = true;
                _savedClient = client;
                return 1;
            };
            ECNDataAccessFakes.ShimDataFunctions.ExecuteScalarSqlCommandString = (cmd, connStrName) =>
            {
                _sqlCommandExecutedList.Add(cmd.CommandText);
                return "1";
            };
            ShimKMCommonDataFunctions.ExecuteScalarSqlCommandString = (cmd, connStrName) =>
            {
                _sqlCommandExecutedList.Add(cmd.CommandText);
                return "1";
            };
            ShimKMCommonDataFunctions.ExecuteNonQuerySqlCommandString = (cmd, connStrName) =>
            {
                _sqlCommandExecutedList.Add(cmd.CommandText);
                return true;
            };
        }

        private void SetEntitiesValidationFakes()
        {
            _isCustomerValidated = false;
            ShimCustomer.ValidateCustomerUser = (c, u) => { _isCustomerValidated = true; };
            ShimContact.ValidateContactUser = (co, u) => { };
            ShimGroup.ValidateGroup = (g) => { };
            ShimCustomerConfig.ValidateCustomerConfigUser = (c, u) => { };
            ShimEmailDirect.ValidateEmailDirect = (e) => { };
        }

        private void SetPageControls(string clientId = ClientID)
        {
            ShimContactEditor.AllInstances.ContactGet = (c) => new Contact();
            Get<HiddenField>(_privateTestObj, "hfCustomerPlatformClientID").Value = clientId;
            Get<TextBox>(_privateTestObj, "txtCustomerName").Text = SampleCustomer;
            Get<TextBox>(_privateTestObj, "txtWebAddress").Text = "SampleWebAddress";
            Get<TextBox>(_privateTestObj, "txttechContact").Text = "SampleTechContact";
            Get<TextBox>(_privateTestObj, "txttechEmail").Text = "tech@sample.com";
            Get<TextBox>(_privateTestObj, "txtSubscriptionEmail").Text = "sample@sample.com";
            Get<TextBox>(_privateTestObj, "txtCustomerName").Text = SampleCustomer;
            Get<TextBox>(_privateTestObj, "txtPickupPath").Text = "SamplePath";
            Get<CheckBox>(_privateTestObj, "cbActiveStatus").Checked = true;
            Get<CheckBox>(_privateTestObj, "cbDemoCustomer").Checked = true;
            Get<CheckBox>(_privateTestObj, "chkDefaultBlastAsTest").Checked = true;
        }

        private void SetPageDropDownLists(string ddlMSCustomerValue = "1")
        {
            var ddlBaseChannels = Get<DropDownList>(_privateTestObj, "ddlBaseChannels");
            ddlBaseChannels.Items.Add(new ListItem(BaseChannelID, BaseChannelID) { Selected = true });

            var ddlCustomerType = Get<DropDownList>(_privateTestObj, "ddlCustomerType");
            ddlCustomerType.Items.Add(new ListItem("SampleCustomerType", "SampleCustomerType") { Selected = true });

            var ddlAccountExecutive = Get<DropDownList>(_privateTestObj, "ddlAccountExecutive");
            ddlAccountExecutive.Items.Add(new ListItem("1", "1") { Selected = true });

            var ddlAccountManager = Get<DropDownList>(_privateTestObj, "ddlAccountManager");
            ddlAccountManager.Items.Add(new ListItem("1", "1") { Selected = true });

            var ddlAbWinnerType = Get<DropDownList>(_privateTestObj, "ddlAbWinnerType");
            ddlAbWinnerType.Items.Add(new ListItem("clicks", "clicks") { Selected = true });

            var ddlMSCustomer = Get<DropDownList>(_privateTestObj, "ddlMSCustomer");
            ddlMSCustomer.Items.Add(new ListItem(ddlMSCustomerValue, ddlMSCustomerValue) { Selected = true });

            var rblStrategic = Get<RadioButtonList>(_privateTestObj, "rblStrategic");
            rblStrategic.Items.Add(new ListItem { Text = "Y", Value = "Y", Selected = true });
        }

        private void ConfigureRadTreeListControl(string defaultColumnValues = "1")
        {
            var radTreeList = Get<RadTreeList>(_privateTestObj, "tlClientServiceFeatures");
            radTreeList.ClearSelectedItems();
            radTreeList.DataKeyNames = new[] { "ID" };
            radTreeList.ParentDataKeyNames = new[] { "PID" };
            var dataItem = new TreeListDataItem(new RadTreeList(), 1, false)
            {
                DataItem = new ServiceFeatures.ClientGroupTreeListRow
                {
                    ID = "1",
                    PID = "1",
                    ServiceFeatureID = 1,
                    MAPID = 1,
                    ServiceID = 1,
                    IsAdditionalCost = true,
                    Description = "sampleDescription",
                    ServiceName = "SampleServiceName",
                    IsEnabled = true,
                    ServiceDisplayOrder = 1,
                    ServiceFeatureDisplayOrder = 1,
                    ServiceFeatureName = "sample"
                },
            };
            radTreeList.Items.Add(dataItem);
            ShimTreeListDataItem.AllInstances.ItemGetString = (x, colName) => 
            {
                if(colName == "ServiceFeatureID")
                {
                    return new TableCell { Text = defaultColumnValues };
                }
                return new TableCell() { Text = "1" };
            };
        }
    }
}
