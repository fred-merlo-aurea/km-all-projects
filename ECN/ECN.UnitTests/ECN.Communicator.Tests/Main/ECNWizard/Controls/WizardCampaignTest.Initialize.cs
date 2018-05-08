using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using KMPlatform.BusinessLogic.Fakes;
using NUnit.Framework;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;
using static KMPlatform.Enums;
using ECN_Framework_BusinessLayer.Activity.View.Fakes;
using System.Data;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using System.Collections.Specialized;
using ECN_Framework_BusinessLayer.Application;
using System.Web.UI.WebControls;
using ecn.communicator.main.ECNWizard.Controls.Fakes;
using System.Configuration.Fakes;
using Shouldly;

namespace ECN.Communicator.Tests.Main.ECNWizard.Controls
{
    public partial class WizardCampaignTest
    {
        private const string SampleIDKey = "SampleID";
        private const string SampleCampaignName = "SampleCampaignName";
        private const string SampleCampaignItem = "SampleCampaignItem";
        private const string TxtCampaignItemName = "txtCampaignItemName";
        private const string RbNewCampaign = "rbNewCampaign";
        private const string BlastIDColumn = "BlastID";
        private const string CbIgnoreSuppression = "cbIgnoreSuppression";
        private const string InitializeMethodName = "Initialize";

        [Test]
        public void Initialize_WhenSampleIDHasValuesAndBlastFieldDoesNotMatchDropDownValues_LoadsPageControls()
        {
            // Arrange
            SetWizardCampaignPageControls();
            SetFakesForWizardCampaignMethod();
            SetFakesForInitializeMethod();

            // Act
            _privateTestObject.Invoke(InitializeMethodName);

            // Assert
            for (int i = 1; i <= 5; i++)
            {
                var drpBlastField = Get<DropDownList>(_privateTestObject, $"{DropdownBlastFieldName}{i}");
                var lblBlastField = Get<Label>(_privateTestObject, $"{LabelBlastFieldName}{i}");
                var txtBlastField = Get<TextBox>(_privateTestObject, $"{TxtBlastField}{i}");
                drpBlastField.ShouldSatisfyAllConditions(
                    () => drpBlastField.ShouldNotBeNull(),
                    () => drpBlastField.Items.Count.ShouldBe(3),
                    () => drpBlastField.SelectedValue.ShouldBe(CustomSelectedValue),
                    () => lblBlastField.ShouldNotBeNull(),
                    () => lblBlastField.Text.ShouldBe($"{SampleBlastField}{i}"),
                    () => txtBlastField.Visible.ShouldBeTrue(),
                    () => txtBlastField.Text.ShouldBe(i.ToString()));
            }
        }

        [Test]
        public void Initialize_WhenSampleIDHasValuesAndBlastFieldDoesMatchDropDownValues_LoadsPageControls()
        {
            // Arrange
            SetWizardCampaignPageControls();
            SetFakesForInitializeMethod();
            LoadBlastFieldDropDownLists();

            // Act
            _privateTestObject.Invoke(InitializeMethodName);

            // Assert
            for (int i = 1; i <= 5; i++)
            {
                var drpBlastField = Get<DropDownList>(_privateTestObject, $"{DropdownBlastFieldName}{i}");
                var txtBlastField = Get<TextBox>(_privateTestObject, $"{TxtBlastField}{1}");
                var lblBlastField = Get<Label>(_privateTestObject, $"{LabelBlastFieldName}{i}");
                drpBlastField.ShouldSatisfyAllConditions(
                    () => drpBlastField.ShouldNotBeNull(),
                    () => drpBlastField.Items.Count.ShouldBe(1),
                    () => drpBlastField.SelectedValue.ShouldBe(i.ToString()),
                    () => lblBlastField.ShouldNotBeNull(),
                    () => lblBlastField.Text.ShouldBeNullOrWhiteSpace(),
                    () => txtBlastField.Visible.ShouldBeFalse(),
                    () => Get<RadioButton>(_privateTestObject, RbExistingCampaign).Checked.ShouldBeTrue(),
                    () => Get<PlaceHolder>(_privateTestObject, PanelExistingCampaign).Visible.ShouldBeTrue(),
                    () => Get<PlaceHolder>(_privateTestObject, PanelCreateCampaign).Visible.ShouldBeFalse());
            }
        }

        [Test]
        public void Initailize_WhenSampleIDHasNoValue_LoadsPageControl()
        {
            // Arrange
            SetWizardCampaignPageControls();
            SetFakesForWizardCampaignMethod();
            SetFakesForInitializeMethod();
            QueryString.Clear();
            QueryString.Add(SampleIDKey, "0");

            // Act
            _privateTestObject.Invoke(InitializeMethodName);

            // Assert
            var rbExistingCampaign = Get<RadioButton>(_privateTestObject, RbExistingCampaign);
            var plExistingCampaign = Get<PlaceHolder>(_privateTestObject, PanelExistingCampaign);
            var plCreateCampaign = Get<PlaceHolder>(_privateTestObject, PanelCreateCampaign);
            rbExistingCampaign.ShouldSatisfyAllConditions(
                () => rbExistingCampaign.Checked.ShouldBeTrue(),
                () => plExistingCampaign.Visible.ShouldBeTrue(),
                () => plCreateCampaign.Visible.ShouldBeFalse(),
                () => Get<TextBox>(_privateTestObject, TxtCampaignItemName).Text.ShouldBe(SampleCampaignItem),
                () => Get<CheckBox>(_privateTestObject, CbIgnoreSuppression).Checked.ShouldBeTrue());

        }

        [Test]
        public void Initailize_WhenSampleIDAndCampaignItemIdHasNoValue_LoadsPageControl()
        {
            // Arrange
            SetFakesForWizardCampaignMethod();
            SetFakesForInitializeMethod();
            QueryString.Clear();
            QueryString.Add(SampleIDKey, "0");

            // Act
            _privateTestObject.Invoke(InitializeMethodName);

            // Assert
            var rbNewCampaign = Get<RadioButton>(_privateTestObject, RbNewCampaign);
            var rbExistingCampaign = Get<RadioButton>(_privateTestObject, RbExistingCampaign);
            var plExistingCampaign = Get<PlaceHolder>(_privateTestObject, PanelExistingCampaign);
            var plCreateCampaign = Get<PlaceHolder>(_privateTestObject, PanelCreateCampaign);
            rbExistingCampaign.ShouldSatisfyAllConditions(
                () => rbNewCampaign.Checked.ShouldBeFalse(),
                () => rbExistingCampaign.Checked.ShouldBeTrue(),
                () => plExistingCampaign.Visible.ShouldBeTrue(),
                () => plCreateCampaign.Visible.ShouldBeFalse());

        }

        private void SetFakesForInitializeMethod()
        {
            QueryString.Clear();
            QueryString.Add(SampleIDKey, "1");

            var settings = new NameValueCollection();
            settings.Add($"IgnoreSuppression_{ECNSession.CurrentSession().CurrentUser.UserID}", bool.TrueString);
            ShimConfigurationManager.AppSettingsGet = () => settings;

            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (user, r, d, g) => true;
            ShimCampaign.GetByCustomerID_NonArchivedInt32UserBoolean = (custId, user, b) => new List<CommunicatorEntities.Campaign>
            {
                new CommunicatorEntities.Campaign {CampaignName = SampleCampaignName, CampaignID = 1}
            };
            ShimSample.GetBySampleIDInt32User = (sampleId, user) => new CommunicatorEntities.Sample { };
            ShimCampaignItem.GetByBlastIDInt32UserBoolean = (bid, user, b) => new CommunicatorEntities.CampaignItem
            {
                CampaignID = 1,
                CampaignItemID = 1,
                CampaignItemName = SampleCampaignName,
                CampaignItemTemplateID = 1,
                BlastField1 = "1",
                BlastField2 = "2",
                BlastField3 = "3",
                BlastField4 = "4",
                BlastField5 = "5",
            };
            ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = (cid, user, b) => new CommunicatorEntities.CampaignItem
            {
                CampaignID = 1,
                CampaignItemName = SampleCampaignItem,
                CampaignItemTemplateID = 1,
                IgnoreSuppression  = true
            };
            ShimCampaignItemTemplate.GetByCampaignItemTemplateIDInt32UserBoolean = (cid, user, b) => new CommunicatorEntities.CampaignItemTemplate();
            ShimBlastActivity.ChampionByProcInt32BooleanUserString = (sid,b,user,t) => GetActivityDataTable();
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (cid, s, f) => true;
        }

        private DataTable GetActivityDataTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add(BlastIDColumn, typeof(int));

            var row = dataTable.NewRow();
            row[BlastIDColumn] = "1";
            dataTable.Rows.Add(row);
            return dataTable;
        }

        private void LoadBlastFieldDropDownLists()
        {
            ShimWizardCampaign.AllInstances.LoadBlastFieldConfig = (w) =>
            {
                for (int i = 1; i <= 5; i++)
                {
                    var drpBlastField = Get<DropDownList>(_privateTestObject, $"{DropdownBlastFieldName}{i}");
                    drpBlastField.Items.Add(new ListItem { Text = i.ToString(), Value = i.ToString() });
                }
            };
        }
    }
}
