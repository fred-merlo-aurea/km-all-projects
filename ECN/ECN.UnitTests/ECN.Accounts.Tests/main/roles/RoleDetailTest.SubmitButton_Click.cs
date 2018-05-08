using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using KMPlatform.Entity;
using KMPlatform.BusinessLogic.Fakes;
using NUnit.Framework;
using Shouldly;
using Telerik.Web.UI.Fakes;
using Telerik.Web.UI;

namespace ECN.Accounts.Tests.main.roles
{
    public partial class RoleDetailTest
    {
        private const string SecurityGroupIDControl = "hfSecurityGroupID";
        private const string ClientDropdown = "drpClient";
        private const string ClientgroupDropdown = "drpclientgroup";
        private const string TxtSecurityGroupName = "tbSecurityGroupName";
        private const string ErrorPlaceHolder = "phError";
        private const string ProductFeatureUpdatePanel = "ProductFeatureUpdatePannel";
        private const string PnlProductFeatureUpdate = "PnlProductFeatureUpdate";
        private const string IsChannelWideRadioButton = "rbIsChannelWide";
        private const string IsActiveRadioButton = "rbIsActive";
        private const string ErrorMessageLabel = "lblErrorMessagePhError";
        private const string YesNotation = "Y";
        private const string NoNotation = "N";
        private const string AdministratorName = "administrator";
        private const string LocalUser = "LocalUser";
        private const string UTException = "UT Exception";
        private const string DefaultPage = "Default.aspx";
        private const string SubmitButtonClickMethodName = "SubmitButton_Click";

        private bool _isSecurityGroupSaved;
        private SecurityGroup _savedSecurityGroup;
        private RadTreeList _tlClientGroupServiceFeatures;
        private SecurityGroupPermission _savedSecurityGroupPermission;
        private Random _random = new Random();

        [Test]
        public void SubmitButton_Click_WhenSecurityGroupNameIsAdmin_ThrowsECNException()
        {
            // Arrange
            SetFakesForSubmitButtonClickMethod();
            SetPageControls();
            Get<TextBox>(_privateTestObject, TxtSecurityGroupName).Text = AdministratorName;

            // Act
            _privateTestObject.Invoke(SubmitButtonClickMethodName, this, EventArgs.Empty);

            _privateTestObject.ShouldSatisfyAllConditions(
                () => Get<PlaceHolder>(_privateTestObject, ErrorPlaceHolder).Visible.ShouldBeTrue(),
                () => Get<Label>(_privateTestObject, ErrorMessageLabel).Text.ShouldContain("Invalid Security Group Name"));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void SubmitButton_Click_ChannelWideIsNotSelected_ThrowsECNException(int selectedIndex)
        {
            // Arrange
            SetFakesForSubmitButtonClickMethod();
            SetPageControls();
            ShimSecurityGroup.AllInstances.ExistsForClient_ClientGroupStringInt32Int32Int32 = (_, __, ___, _____, _______) => true;
            var rbIsChannelWide = Get<RadioButtonList>(_privateTestObject, IsChannelWideRadioButton);
            rbIsChannelWide.Items.Add(new ListItem { Value = "0" });
            rbIsChannelWide.Items.Add(new ListItem { Value = "1" });
            rbIsChannelWide.SelectedIndex = selectedIndex;

            // Act
            _privateTestObject.Invoke(SubmitButtonClickMethodName, this, EventArgs.Empty);

            _privateTestObject.ShouldSatisfyAllConditions(
                () => Get<PlaceHolder>(_privateTestObject, ErrorPlaceHolder).Visible.ShouldBeTrue(),
                () => Get<Label>(_privateTestObject, ErrorMessageLabel).Text.ShouldContain("Security Group Name already exists"));
        }

        [Test]
        [TestCase(1, 1, 1)]
        [TestCase(0, 1, 1)]
        [TestCase(0, 0, 1)]
        public void SubmitButton_Click_ChannelWideIsSelected_SavesRelatedEntities(int serviceFeatureMapId, int featureId, int serviceId)
        {
            // Arrange
            SetFakesForSubmitButtonClickMethod();
            SetPageControls();
            SetUpRadTreeList(serviceFeatureMapId, featureId, serviceId);
            
            // Act
            _privateTestObject.Invoke(SubmitButtonClickMethodName, this, EventArgs.Empty);

            // Assert
            _privateTestObject.ShouldSatisfyAllConditions(
                () => _isSecurityGroupSaved.ShouldBeTrue(),
                () => _savedSecurityGroup.ShouldNotBeNull(),
                () => 
                    {
                        if( serviceFeatureMapId == 1)
                        {
                            _savedSecurityGroupPermission.ShouldNotBeNull();
                        }
                        else
                        {
                            _savedSecurityGroupPermission.ShouldBeNull();
                        }
                        
                    },
                () => RedirectUrl.ShouldNotBeNullOrWhiteSpace(),
                () => RedirectUrl.ShouldContain(DefaultPage));
        }

        [Test]
        public void SubmitButton_Click_WhenThrowsECNException_LogsException()
        {
            // Arrange
            SetFakesForSubmitButtonClickMethod();
            SetPageControls();
            ShimSecurityGroup.AllInstances.SelectInt32BooleanBoolean = (_, __, ___, ____) => throw new ECNException(
                new List<ECNError> {
                    new ECNError { ErrorMessage = UTException}
                });

            // Act
            _privateTestObject.Invoke(SubmitButtonClickMethodName, this, EventArgs.Empty);

            // Assert
            _privateTestObject.ShouldSatisfyAllConditions(
                () => Get<PlaceHolder>(_privateTestObject, ErrorPlaceHolder).Visible.ShouldBeTrue(),
                () => Get<Label>(_privateTestObject, ErrorMessageLabel).Text.ShouldContain(UTException));
        }

        [Test]
        public void SubmitButton_Click_WhenCustomerIsNotActive_RedirectsToDefaultPage()
        {
            // Arrange
            SetFakesForSubmitButtonClickMethod();
            SetPageControls();
            ShimCustomer.GetByClientIDInt32Boolean = (_, __) => new Customer
            {
                ActiveFlag = NoNotation,
                CustomerID = 1,
            };

            // Act
            _privateTestObject.Invoke(SubmitButtonClickMethodName, this, EventArgs.Empty);

            // Assert
            _privateTestObject.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldNotBeNullOrWhiteSpace(),
                () => RedirectUrl.ShouldContain(DefaultPage));
        }

        private void SetPageControls()
        {
            Get<HiddenField>(_privateTestObject, SecurityGroupIDControl).Value = "1";

            var drpClient = Get<DropDownList>(_privateTestObject, ClientDropdown);
            drpClient.Items.Add(new ListItem { Value = "1" });
            var drpclientgroup = Get<DropDownList>(_privateTestObject, ClientgroupDropdown);
            drpclientgroup.Items.Add(new ListItem { Value = "1" });

            var productFeaturePanel = Get<UpdatePanel>(_privateTestObject, ProductFeatureUpdatePanel);
            productFeaturePanel.ID = PnlProductFeatureUpdate;
            productFeaturePanel.UpdateMode = UpdatePanelUpdateMode.Conditional;

            var rbIsChannelWide = Get<RadioButtonList>(_privateTestObject, IsChannelWideRadioButton);
            rbIsChannelWide.Items.Add(new ListItem { Value = "0" });
            rbIsChannelWide.Items.Add(new ListItem { Value = "1" });
            rbIsChannelWide.SelectedIndex = 1;

            var rbIsActive = Get<RadioButtonList>(_privateTestObject, IsActiveRadioButton);
            rbIsActive.Items.Add(new ListItem { Value = "0" });
            rbIsActive.Items.Add(new ListItem { Value = "1" });
            rbIsActive.SelectedIndex = 1;

            Get<TextBox>(_privateTestObject, TxtSecurityGroupName).Text = LocalUser;
        }

        private void SetFakesForSubmitButtonClickMethod()
        {
            _isSecurityGroupSaved = false;
            _savedSecurityGroup = null;
            _savedSecurityGroupPermission = null;

            ShimCustomer.GetByClientIDInt32Boolean = (_, __) => new Customer
            {
                ActiveFlag = YesNotation,
                CustomerID = 1,
            };
            ShimSecurityGroup.AllInstances.SelectInt32BooleanBoolean = (_, __, ___, ____) => new SecurityGroup();
            
            ShimSecurityGroup.AllInstances.ExistsForClient_ClientGroupStringInt32Int32Int32 = (_, __, ___, _____, _______) => false;
            ShimSecurityGroup.AllInstances.SaveSecurityGroup = (s, securityGroup) =>
            {
                _savedSecurityGroup = securityGroup;
                _isSecurityGroupSaved = true;
                return _savedSecurityGroup.SecurityGroupID;
            };
            ShimSecurityGroupPermission.AllInstances.SaveSecurityGroupPermission = (sg, groupPermission) =>
            {
                _savedSecurityGroupPermission = groupPermission;
                return _savedSecurityGroupPermission.SecurityGroupPermissionID;
            };
        }

        private void SetUpRadTreeList(int serviceFeatureMapId, int featureId, int serviceId)
        {
            _tlClientGroupServiceFeatures = Get<RadTreeList>(_privateTestObject, "tlSecurityGroupAccess");
            const string ServiceIdColumn = "ServiceID";
            const string FeatureIdCollumn = "ServiceFeatureID";
            const string MapIdColumn = "MAPID";
            const string ServiceFeatureAccessMapIDColumn = "ServiceFeatureAccessMapID";
            const string IdColumn = "ID";
            var item = new TreeListDataItem(_tlClientGroupServiceFeatures, 0, false);
            var mapId = GetAnyNumber();
            var row = new Dictionary<string, string>
            {
                {
                    ServiceIdColumn ,
                    serviceId.ToString()
                },
                {
                    FeatureIdCollumn,
                    featureId.ToString()
                },
                {
                    MapIdColumn,
                    mapId.ToString()
                },
                {
                    ServiceFeatureAccessMapIDColumn,
                    serviceFeatureMapId.ToString()
                },
                {
                    IdColumn,
                    $"S{serviceId}"
                }
            };
            item.ExtractValues(row);
            _tlClientGroupServiceFeatures.Items.Add(item);
            item.Selected = true;
            ShimTreeListDataItem.AllInstances.ItemGetString = (instance, key) =>
            {
                return new TableCell { Text = row[key] };
            };
        }

        private int GetAnyNumber()
        {
            const int randomRangeMin = 10;
            const int randomRangeMax = randomRangeMin * 100;
            return _random.Next(randomRangeMin, randomRangeMax);
        }
    }
}
