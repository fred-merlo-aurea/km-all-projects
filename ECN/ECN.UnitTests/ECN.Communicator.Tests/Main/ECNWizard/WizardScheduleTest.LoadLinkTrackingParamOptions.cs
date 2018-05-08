using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using KMPlatform.BusinessLogic.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.ECNWizard
{
    public partial class WizardScheduleTest
    {
        [Test]
        public void LoadLinkTrackingParamOptions_WithCustomAndBaseChannelOverrideTrueAndIsDefaultTrue_PopulatesPageDropdowns()
        {
            // Arrange
            SetUpFakes();
            ConfigureLinkTrackingParamsFakes();
            ConfigureLinkTrackingParamOptionsFakes();
            ConfigureLinkTrackingSettingFakes();

            // Act
            _privateTestObject.Invoke(LoadLinkTrackingParamOptionsMethodName);

            // Assert
            AssertCampaingnDowndownLists(drpCampaignName: DrpCampaignContent);
            AssertCampaingnDowndownLists(drpCampaignName: DrpCampaignMedium);
            AssertCampaingnDowndownLists(drpCampaignName: DrpCampaignName);
            AssertCampaingnDowndownLists(drpCampaignName: DrpCampaignTerm);
            // Assert Omniture Dropdonws
            for (var i = 1; i <= 10; i++)
            {
                var ddlOmniture = Get<DropDownList>(_privateTestObject, $"ddlOmniture{i}");
                ddlOmniture.ShouldNotBeNull();
                ddlOmniture.ShouldSatisfyAllConditions(
                   () => ddlOmniture.Items.Count.ShouldBe(3),
                   () => ddlOmniture.Items.FindByText(SampleCompany).ShouldNotBeNull(),
                   () => ddlOmniture.Items.FindByText(CustomValue).ShouldNotBeNull());

                Get<Label>(_privateTestObject, $"lblOmniture{i}").Text.ShouldBe(SomeSetting);
            }
            Get<CheckBox>(_privateTestObject, ChkboxOmnitureTracking).Checked.ShouldBeTrue();
            Get<Panel>(_privateTestObject, PnlOmniture).Visible.ShouldBeTrue();
        }

        [Test]
        public void LoadLinkTrackingParamOptions_WithCustomAndBaseChannelOverrideTrueIsDefaultFalseForLinkTracking_PopulatesPageDropDowns()
        {
            // Arrange
            SetUpFakes();
            ConfigureLinkTrackingParamsFakes();
            ConfigureLinkTrackingParamOptionsFakes(isDefaultLinkTracking: false);
            ConfigureLinkTrackingSettingFakes();

            // Act
            _privateTestObject.Invoke(LoadLinkTrackingParamOptionsMethodName);

            // Assert
            AssertCampaingnDowndownLists(drpCampaignName: DrpCampaignContent);
            AssertCampaingnDowndownLists(drpCampaignName: DrpCampaignMedium);
            AssertCampaingnDowndownLists(drpCampaignName: DrpCampaignName);
            AssertCampaingnDowndownLists(drpCampaignName: DrpCampaignTerm);
            // Assert Omniture Dropdonws
            for (var i = 1; i <= 10; i++)
            {
                var ddlOmniture = Get<DropDownList>(_privateTestObject, $"ddlOmniture{i}");
                ddlOmniture.ShouldNotBeNull();
                ddlOmniture.ShouldSatisfyAllConditions(
                   () => ddlOmniture.Items.Count.ShouldBe(3),
                   () => ddlOmniture.Items.FindByText(SampleCompany).ShouldNotBeNull(),
                   () => ddlOmniture.Items.FindByText(CustomValue).ShouldNotBeNull());

                Get<Label>(_privateTestObject, $"lblOmniture{i}").Text.ShouldBe(SomeSetting);
            }
            Get<CheckBox>(_privateTestObject, ChkboxOmnitureTracking).Checked.ShouldBeTrue();
            Get<Panel>(_privateTestObject, PnlOmniture).Visible.ShouldBeTrue();
        }

        [Test]
        public void LoadLinkTrackingParamOptions_CustomerOverrideFalseForLinkTrackingAndIsDefaultTrue_PopulatesPageDropDownList()
        {
            // Arrange
            SetUpFakes();
            ConfigureLinkTrackingParamsFakes();
            ConfigureLinkTrackingParamOptionsFakes(isDefaultLinkTracking: false);
            ConfigureLinkTrackingSettingFakes(isOverride: false);

            // Act
            _privateTestObject.Invoke(LoadLinkTrackingParamOptionsMethodName);

            // Assert
            AssertCampaingnDowndownLists(drpCampaignName: DrpCampaignContent);
            AssertCampaingnDowndownLists(drpCampaignName: DrpCampaignMedium);
            AssertCampaingnDowndownLists(drpCampaignName: DrpCampaignName);
            AssertCampaingnDowndownLists(drpCampaignName: DrpCampaignTerm);
            // Assert Omniture Dropdonws
            for (var i = 1; i <= 10; i++)
            {
                var ddlOmniture = Get<DropDownList>(_privateTestObject, $"ddlOmniture{i}");
                ddlOmniture.ShouldNotBeNull();
                ddlOmniture.ShouldSatisfyAllConditions(
                   () => ddlOmniture.Items.Count.ShouldBe(3),
                   () => ddlOmniture.Items.FindByText($"Omniture{i}").ShouldNotBeNull(),
                   () => ddlOmniture.Items.FindByText(CustomValue).ShouldNotBeNull());

                Get<Label>(_privateTestObject, $"lblOmniture{i}").Text.ShouldBe(SomeSetting);
            }
            Get<CheckBox>(_privateTestObject, ChkboxOmnitureTracking).Checked.ShouldBeTrue();
            Get<Panel>(_privateTestObject, PnlOmniture).Visible.ShouldBeTrue();
        }

        [Test]
        public void LoadLinkTrackingParamOptions_CustomerOverrideFalseForLinkTrackingAndIDNotExists_PopulatesPageDropDownLists()
        {
            // Arrange
            SetUpFakes();
            ConfigureLinkTrackingParamsFakes();
            ConfigureLinkTrackingParamOptionsFakes();
            ConfigureLinkTrackingSettingFakes(isOverride: false);
            ShimCampaignItemLinkTracking.GetByCampaignItemIDInt32User = (id, u) => new List<CampaignItemLinkTracking>
            {
                new CampaignItemLinkTracking { }
            };
            
            // Act
            _privateTestObject.Invoke(LoadLinkTrackingParamOptionsMethodName);

            // Assert
            AssertCampaingnDowndownLists(drpCampaignName: DrpCampaignContent);
            AssertCampaingnDowndownLists(drpCampaignName: DrpCampaignMedium);
            AssertCampaingnDowndownLists(drpCampaignName: DrpCampaignName);
            AssertCampaingnDowndownLists(drpCampaignName: DrpCampaignTerm);
            // Assert Omniture Dropdonws
            for (var i = 1; i <= 10; i++)
            {
                var ddlOmniture = Get<DropDownList>(_privateTestObject, $"ddlOmniture{i}");
                ddlOmniture.ShouldNotBeNull();
                ddlOmniture.ShouldSatisfyAllConditions(
                   () => ddlOmniture.Items.Count.ShouldBe(3),
                   () => ddlOmniture.Items.FindByText($"Omniture{i}").ShouldNotBeNull(),
                   () => ddlOmniture.Items.FindByText(CustomValue).ShouldNotBeNull());

                Get<Label>(_privateTestObject, $"lblOmniture{i}").Text.ShouldBe(SomeSetting);
            }
            Get<CheckBox>(_privateTestObject, ChkboxOmnitureTracking).Checked.ShouldBeTrue();
            Get<Panel>(_privateTestObject, PnlOmniture).Visible.ShouldBeTrue();
        }

        [Test]
        public void LoadLinkTrackingParamOptions_CampaignItemTemplateHasValue_SetsSelectedValueForOmitureDropdownLists()
        {
            // Arrange
            SetUpFakes(campaignItemTemplateID: 1);
            ConfigureLinkTrackingParamsFakes();
            ConfigureLinkTrackingParamOptionsFakes();
            ConfigureLinkTrackingSettingFakes(isOverride: false);
            ShimCampaignItemLinkTracking.GetByCampaignItemIDInt32User = (id, u) => new List<CampaignItemLinkTracking>
            {
                new CampaignItemLinkTracking { }
            };

            // Act
            _privateTestObject.Invoke(LoadLinkTrackingParamOptionsMethodName);

            // Assert
            AssertCampaingnDowndownLists(drpCampaignName: DrpCampaignContent);
            AssertCampaingnDowndownLists(drpCampaignName: DrpCampaignMedium);
            AssertCampaingnDowndownLists(drpCampaignName: DrpCampaignName);
            AssertCampaingnDowndownLists(drpCampaignName: DrpCampaignTerm);
            // Assert Omniture Dropdonws
            for (var i = 1; i <= 10; i++)
            {
                var ddlOmniture = Get<DropDownList>(_privateTestObject, $"ddlOmniture{i}");
                ddlOmniture.ShouldNotBeNull();
                ddlOmniture.ShouldSatisfyAllConditions(
                   () => ddlOmniture.Items.Count.ShouldBe(3),
                   () => ddlOmniture.Items.FindByText($"Omniture{i}").ShouldNotBeNull(),
                   () => ddlOmniture.Items.FindByText(CustomValue).ShouldNotBeNull(),
                   () => ddlOmniture.SelectedValue.ShouldBe("1"));

                Get<Label>(_privateTestObject, $"lblOmniture{i}").Text.ShouldBe(SomeSetting);
            }
            Get<CheckBox>(_privateTestObject, ChkboxOmnitureTracking).Checked.ShouldBeTrue();
            Get<Panel>(_privateTestObject, PnlOmniture).Visible.ShouldBeTrue();
        }

        private void SetUpFakes(int? campaignItemTemplateID = null)
        {
            ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = (id, u, c) => new CampaignItem { CampaignItemTemplateID = campaignItemTemplateID };
            ShimClient.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (id, s, sf) => true;
            ShimCampaignItemTemplate.GetByCampaignItemTemplateIDInt32UserBoolean = (i, u, c) => new CampaignItemTemplate
            {
                Omniture1 = TemplateOmniture1,
                Omniture2 = TemplateOmniture2,
                Omniture3 = TemplateOmniture3,
                Omniture4 = TemplateOmniture4,
                Omniture5 = TemplateOmniture5,
                Omniture6 = TemplateOmniture6,
                Omniture7 = TemplateOmniture7,
                Omniture8 = TemplateOmniture8,
                Omniture9 = TemplateOmniture9,
                Omniture10 = TemplateOmniture10,
            };
        }

        private void ConfigureLinkTrackingParamsFakes()
        {
            ShimLinkTrackingParam.GetByLinkTrackingIDInt32 = (id) =>
            {
                var linkTrackingParamList = new List<LinkTrackingParam>();
                for (var i = 1; i <= 10; i++)
                {
                    linkTrackingParamList.Add(new LinkTrackingParam { DisplayName = $"Omniture{i}", LTPID = i });
                }
                return linkTrackingParamList;
            };
            ShimCampaignItemLinkTracking.GetByCampaignItemIDInt32User = (id, u) =>
            {
                var campaignItemLinkTrackingList = new List<CampaignItemLinkTracking>();
                for (var i = 1; i <= 10; i++)
                {
                    campaignItemLinkTrackingList.Add(new CampaignItemLinkTracking { LTPID = i, LTPOID = i });
                }
                return campaignItemLinkTrackingList;
            };

            ShimLinkTrackingParamSettings.Get_LTPID_CustomerIDInt32Int32 = (l, u) => new LinkTrackingParamSettings
            {
                DisplayName = SomeSetting,
                LTPSID = 1,
                AllowCustom = true
            };

            ShimLinkTrackingParamSettings.Get_LTPID_BaseChannelIDInt32Int32 = (l, u) => new LinkTrackingParamSettings
            {
                DisplayName = SomeSetting,
                LTPSID = 1,
                AllowCustom = true
            };
        }

        private void ConfigureLinkTrackingParamOptionsFakes(bool isDefaultLinkTracking = true)
        {
            ShimLinkTrackingParamOption.GetByLTPIDInt32 = (id) => new List<LinkTrackingParamOption>
            {
                new LinkTrackingParamOption { DisplayName = "SomeOtherCompany", LTPOID = 1 },
                new LinkTrackingParamOption { DisplayName = "SomeAnotherCompany", LTPOID = 2 }
            };
            ShimLinkTrackingParamOption.Get_LTPID_CustomerIDInt32Int32 = (l, c) => new List<LinkTrackingParamOption>
            {
                new LinkTrackingParamOption { DisplayName = SampleCompany,LTPOID = 1, IsDefault = isDefaultLinkTracking }
            };
            ShimLinkTrackingParamOption.Get_LTPID_BaseChannelIDInt32Int32 = (ltpid, c) => new List<LinkTrackingParamOption>
            {
                new LinkTrackingParamOption { DisplayName = $"Omniture{ltpid}",LTPOID = 1, IsDefault = isDefaultLinkTracking }
            };
        }

        private void ConfigureLinkTrackingSettingFakes(bool isOverride = true)
        {
            ShimLinkTrackingSettings.GetByBaseChannelID_LTIDInt32Int32 = (c, u) => new LinkTrackingSettings
            {
                LTSID = 1,
                XMLConfig = CreateBaseChannelXMLString().ToString(SaveOptions.None)
            };
            ShimLinkTrackingSettings.GetByCustomerID_LTIDInt32Int32 = (c, u) => new LinkTrackingSettings
            {
                LTSID = 1,
                XMLConfig = CreateCustomerXMLString(isOverride).ToString(SaveOptions.None)
            };
        }

        private XDocument CreateBaseChannelXMLString()
        {
            var xDoc = new XDocument(
                new XElement(XMLElementSettings,
                new XElement(XMLElementAllowCustomerOverride, bool.TrueString),
                new XElement(XMLElementQueryString, string.Empty),
                new XElement(XMLElementDelimiter, ",")
                ));
            return xDoc;
        }

        private XDocument CreateCustomerXMLString(bool isOverride = true)
        {
            var xDoc = new XDocument(
                new XElement(XMLElementSettings,
                new XElement(XMLElementOverride, isOverride.ToString()),
                new XElement(XMLElementQueryString, string.Empty),
                new XElement(XMLElementDelimiter, ",")
                ));
            return xDoc;
        }

        private void AssertCampaingnDowndownLists(string drpCampaignName)
        {
            var dropDownControl = Get<DropDownList>(_privateTestObject, drpCampaignName);
            dropDownControl.ShouldNotBeNull();
            dropDownControl.ShouldSatisfyAllConditions(
                () => dropDownControl.Items.Count.ShouldBe(3),
                () => dropDownControl.Items.FindByText(SomeOtherCompany).ShouldNotBeNull(),
                () => dropDownControl.Items.FindByText(SomeAnotherCompany).ShouldNotBeNull());
        }
    }
}
