using System.Data;
using System.Web.UI.WebControls;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using NUnit.Framework;
using Shouldly;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;

namespace ECN.Communicator.Tests.Main.ECNWizard.Controls
{
    public partial class WizardCampaignTest
    {
        private const string DropdownBlastFieldName = "drpBlastField";
        private const string LabelBlastFieldName = "lblBlastField";
        private const string DefaultSelectedValue = "-Select-";
        private const string SampleBlastField = "SampleBlastField";
        private const string TxtBlastField = "txtBlastField";
        private const string CustomSelectedValue = "Custom Value";
        private const string ColumnValue = "Value";
        private const string RbExistingCampaign = "rbExistingCampaign";
        private const string PanelExistingCampaign = "plExistingCampaign";
        private const string PanelCreateCampaign = "plCreateCampaign";
        private const string LoadBlastFieldConfigMethodName = "LoadBlastFieldConfig";

        [Test]
        public void LoadBlastFieldConfig_WhenCampaignBlastFieldMatchesBlastFieldValueDataTable_LoadsBlastFieldDropdownList()
        {
            // Arrange
            SetWizardCampaignPageControls();
            SetFakesForWizardCampaignMethod();

            // Act
            _privateTestObject.Invoke(LoadBlastFieldConfigMethodName);

            // Assert
            for (int i = 1; i <= 5; i++)
            {
                var drpBlastField = Get<DropDownList>(_privateTestObject, $"{DropdownBlastFieldName}{i}");
                var lblBlastField = Get<Label>(_privateTestObject, $"{LabelBlastFieldName}{i}");
                drpBlastField.ShouldSatisfyAllConditions(
                    () => drpBlastField.ShouldNotBeNull(),
                    () => drpBlastField.Items.Count.ShouldBe(3),
                    () => drpBlastField.SelectedValue.ShouldBe(DefaultSelectedValue),
                    () => lblBlastField.ShouldNotBeNull(),
                    () => lblBlastField.Text.ShouldBe($"{SampleBlastField}{i}"));
            }
        }

        [Test]
        public void LoadBlastFieldConfig_WhenCampaignBlastFieldNotMatchesBlastFieldValueDataTable_LoadsBlastFieldDropdownList()
        {
            // Arrange
            SetWizardCampaignPageControls();
            SetFakesForWizardCampaignMethod();
            ShimBlastFieldsValue.GetByBlastFieldIDInt32User = (bfId, user) => 
            {
                bfId = bfId + 10;
                return GetDataTable(bfId.ToString());
            };

            // Act
            _privateTestObject.Invoke(LoadBlastFieldConfigMethodName);

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

        private void SetWizardCampaignPageControls()
        {
            _testEntity.CampaignItemID = 1;
        }
        private void SetFakesForWizardCampaignMethod()
        {
            ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = (cid, user, b) => new CommunicatorEntities.CampaignItem
            {
                CampaignID = 1,
                CampaignItemID = 1,
                BlastField1 = "1",
                BlastField2 = "2",
                BlastField3 = "3",
                BlastField4 = "4",
                BlastField5 = "5",
            };
            ShimBlastFieldsName.GetByBlastFieldIDInt32User = (bfId, user) => new CommunicatorEntities.BlastFieldsName
            {
                BlastFieldID = bfId,
                BlastFieldsNameID = bfId,
                Name = $"{SampleBlastField}{bfId}"
            };
            ShimBlastFieldsValue.GetByBlastFieldIDInt32User = (bfId, user) => GetDataTable(bfId.ToString());
        }

        private DataTable GetDataTable(string blastFieldId)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add(ColumnValue, typeof(string));

            var row = dataTable.NewRow();
            row[ColumnValue] = blastFieldId;
            dataTable.Rows.Add(row);
            return dataTable;
        }
    }
}
