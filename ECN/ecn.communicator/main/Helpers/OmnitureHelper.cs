using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml;
using ECN_Framework_Entities.Communicator;
using BusinessLinkTrackingParamOption = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamOption;
using BusinessLinkTrackingParamSettings = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings;
    

namespace ecn.communicator.main.Helpers
{
    public class OmnitureHelper
    {
        private const string SettingsXPath = "/Settings";

        public static void InitializeOmnitureDropDown(
            IList<LinkTrackingParam> ltpList,
            List<CampaignItemLinkTracking> lstCampaignItemLinkTracking,
            string displayName,
            DropDownList ddlOmniture,
            Label lblOmniture,
            Panel pnlOmniture,
            CheckBox chkboxOmnitureTracking,
            Func<string, bool> hasCampainItemLinkTracking,
            Func<string, List<LinkTrackingParamOption>> ltpoListFunc,
            Func<string, LinkTrackingParamSettings> ltpsFunc)
        {
            var ltpoList = ltpoListFunc(displayName);

            ddlOmniture.DataSource = ltpoList;
            ddlOmniture.DataTextField = "DisplayName";
            ddlOmniture.DataValueField = "LTPOID";
            ddlOmniture.DataBind();
            ddlOmniture.Items.Insert(0, new ListItem {Text = "-Select-", Value = "0"});

            var ltps1 = ltpsFunc(displayName);
            if (ltps1 != null)
            {
                if (ltps1.AllowCustom)
                {
                    ddlOmniture.Items.Add(new ListItem {Text = "CustomValue", Value = "-1"});
                }

                lblOmniture.Text = ltps1.DisplayName;
            }

            if (ltpoList.Exists(x => x.IsDefault) &&
                ddlOmniture.SelectedValue == "0" &&
                hasCampainItemLinkTracking(displayName))
            {
                ddlOmniture.SelectedValue = ltpoList.First(x => x.IsDefault).LTPOID.ToString();
                chkboxOmnitureTracking.Checked = true;
                pnlOmniture.Visible = true;
            }
            else if (lstCampaignItemLinkTracking.Exists(x =>
                x.LTPID == ltpList.First(y => y.DisplayName == displayName).LTPID))
            {
                ddlOmniture.SelectedValue = lstCampaignItemLinkTracking
                    .First(x => x.LTPID == ltpList.First(y => y.DisplayName == displayName).LTPID)
                    .LTPOID.ToString();
                chkboxOmnitureTracking.Checked = true;
                pnlOmniture.Visible = true;
            }
        }

        public static OmnitureSettings GetOmnitureSettings(
            LinkTrackingSettings ltsBase,
            LinkTrackingSettings ltsCustomer)
        {
            var settings = new OmnitureSettings()
            {
                AllowCustOverride = true,
                OverrideBaseChannel = true,
                HasBaseSetup = false,
                HasCustSetup = false
            };
            bool parseValue;

            if (ltsBase?.LTSID > 0)
            {
                var doc = new XmlDocument();
                doc.LoadXml(ltsBase.XMLConfig);
                var rootNode = doc.SelectSingleNode(SettingsXPath);
                if (rootNode?.HasChildNodes == true)
                {
                    bool.TryParse(rootNode["AllowCustomerOverride"]?.InnerText, out parseValue);
                    settings.AllowCustOverride = parseValue;
                    settings.HasBaseSetup = true;
                }
            }

            if (ltsCustomer?.LTSID > 0)
            {
                var doc = new XmlDocument();
                doc.LoadXml(ltsCustomer.XMLConfig);
                var rootNode = doc.SelectSingleNode(SettingsXPath);
                if (rootNode?.HasChildNodes == true)
                {
                    bool.TryParse(rootNode["Override"]?.InnerText, out parseValue);
                    settings.OverrideBaseChannel = parseValue;
                    settings.HasCustSetup = true;
                }
            }

            return settings;
        }

        public static IEnumerable<CampaignItemLinkTracking> LoadOmnitureCompainItemSavedData(
            List<LinkTrackingParam> linkTrackingParamList,
            List<CampaignItemLinkTracking> campaignItemLinkTrackingList,
            string displayName,
            DropDownList omnitureDropDown,
            TextBox omnitureTextBox)
        {
            var omniParam = (from src in linkTrackingParamList
                where src.DisplayName == displayName
                select src).ToList();
            var omniValue = (from src in campaignItemLinkTrackingList
                where src.LTPID == omniParam.FirstOrDefault()?.LTPID
                select src).ToList();
            if (omniValue.Any())
            {
                var ltpoidValue = omniValue.First().LTPOID.Value;
                if (omnitureDropDown.Items.FindByValue(ltpoidValue.ToString()) != null)
                {
                    omnitureDropDown.SelectedValue = ltpoidValue.ToString();
                }

                if (ltpoidValue.ToString() == "-1")
                {
                    omnitureTextBox.Visible = true;
                    omnitureTextBox.Text = omniValue.First().CustomValue;
                }
                else
                {
                    omnitureTextBox.Visible = false;
                }
            }

            return omniValue;
        }
    }
}