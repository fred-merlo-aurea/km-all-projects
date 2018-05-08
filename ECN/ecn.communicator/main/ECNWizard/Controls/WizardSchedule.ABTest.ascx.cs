using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_Common.Objects.Communicator;
using BusinessGroup = ECN_Framework_BusinessLayer.Communicator.Group;
using CommunicatorCampaignItem = ECN_Framework_Entities.Communicator.CampaignItem;
using CommunicatorCampaignItemLinkTracking = ECN_Framework_Entities.Communicator.CampaignItemLinkTracking;

namespace ecn.communicator.main.ECNWizard.Controls
{
    public partial class WizardSchedule
    {
        private const string GroupNameField = "GroupName";

        private void PrePopFromAB(int sampleId)
        {
            var campaignItem = CampaignItem.GetBySampleID(
                sampleId,
                Enums.CampaignItemType.AB,
                ECNSession.CurrentSession().CurrentUser,
                true);

            var ciBlastList = CampaignItemBlast.GetByCampaignItemID(
                campaignItem.CampaignItemID,
                ECNSession.CurrentSession().CurrentUser,
                false);
            chkOptOutMasterSuppression.Checked = ciBlastList.Any(cbi => cbi.AddOptOuts_to_MS == true);
            PrepareOptOutGroupList(campaignItem);
            pnlOptOutSpecificGroups.Visible = chkOptOutSpecificGroup.Checked;
            chkOptOutMasterSuppression.Enabled = !chkOptOutSpecificGroup.Checked;
            chkOptOutSpecificGroup.Enabled = chkOptOutSpecificGroup.Checked;

            PrepareCompainItemLinkTracking(campaignItem);
        }

        private void PrepareCompainItemLinkTracking(CommunicatorCampaignItem campaignItem)
        {
            var listCilt = CampaignItemLinkTracking.GetByCampaignItemID(
                campaignItem.CampaignItemID,
                ECNSession.CurrentSession().CurrentUser);
            chkCacheBuster.Checked = campaignItem.EnableCacheBuster == true;
            chkboxConvTracking.Checked = listCilt.Any(x => x.LTPID == 6 || x.LTPID == 7);
            if (listCilt.Count > 0)
            {
                PrePopGoogleAnalyticsByLinkTracking(listCilt);
                PrePopOmnitureByLinkTracking(listCilt);
            }
            else
            {
                pnlOmniture.Visible = false;
                chkboxOmnitureTracking.Checked = false;
                pnlGoogleAnalytics.Visible = false;
                chkboxGoogleAnalytics.Checked = false;
            }
        }

        private void PrePopOmnitureByLinkTracking(IEnumerable<CommunicatorCampaignItemLinkTracking> listCilt)
        {
            var result = (listCilt
                .Join(LinkTrackingParam.GetByLinkTrackingID(OmnitureLinkTrackingId),
                    src => src.LTPID,
                    ltp => ltp.LTPID,
                    (src, ltp) => new {src, ltp})
                .Where(t => t.ltp.LTID == OmnitureLinkTrackingId)
                .Select(t => t.src)).ToList();

            if (result.Count > 0)
            {
                chkboxOmnitureTracking.Checked = true;
                pnlOmniture.Visible = true;
                SetPrePopOmnitureControl(result, 8, txtOmniture1, ddlOmniture1);
                SetPrePopOmnitureControl(result, 9, txtOmniture2, ddlOmniture2);
                SetPrePopOmnitureControl(result, 10, txtOmniture3, ddlOmniture3);
                SetPrePopOmnitureControl(result, 11, txtOmniture4, ddlOmniture4);
                SetPrePopOmnitureControl(result, 12, txtOmniture5, ddlOmniture5);
                SetPrePopOmnitureControl(result, 13, txtOmniture6, ddlOmniture6);
                SetPrePopOmnitureControl(result, 14, txtOmniture7, ddlOmniture7);
                SetPrePopOmnitureControl(result, 15, txtOmniture8, ddlOmniture8);
                SetPrePopOmnitureControl(result, 16, txtOmniture9, ddlOmniture9);
                SetPrePopOmnitureControl(result, 17, txtOmniture10, ddlOmniture10);
            }
            else
            {
                chkboxOmnitureTracking.Checked = false;
                pnlOmniture.Visible = false;
            }
        }

        private static void SetPrePopOmnitureControl(
            IEnumerable<CommunicatorCampaignItemLinkTracking> result,
            int ltpId,
            TextBox omnitureTextBox,
            DropDownList omnitureDropDown)
        {
            var item = result.FirstOrDefault(x => x.LTPID == ltpId);
            if (item == null)
            {
                return;
            }

            if (item.LTPOID == -1)
            {
                omnitureTextBox.Visible = true;
                omnitureTextBox.Text = item.CustomValue;
                omnitureDropDown.SelectedValue = "-1";
            }
            else
            {
                omnitureDropDown.SelectedValue = item.LTPOID.ToString();
            }
        }

        private void PrePopGoogleAnalyticsByLinkTracking(IEnumerable<CommunicatorCampaignItemLinkTracking> listCilt)
        {
            var result = (listCilt.Join(LinkTrackingParam.GetByLinkTrackingID(GoogleLinkTrackingId),
                    src => src.LTPID,
                    ltp => ltp.LTPID,
                    (src, ltp) => new {src, ltp})
                .Where(t => t.ltp.LTID == GoogleLinkTrackingId)
                .Select(t => t.src)).ToList();
            if (result.Count > 0)
            {
                chkboxGoogleAnalytics.Checked = true;
                pnlGoogleAnalytics.Visible = true;
                SetPrePopGoogleAnalyticsControl(result, 1, txtCampaignSource, drpCampaignSource);
                SetPrePopGoogleAnalyticsControl(result, 2, txtCampaignMedium, drpCampaignMedium);
                SetPrePopGoogleAnalyticsControl(result, 3, txtCampaignTerm, drpCampaignTerm);
                SetPrePopGoogleAnalyticsControl(result, 4, txtCampaignContent, drpCampaignContent);
                SetPrePopGoogleAnalyticsControl(result, 2, txtCampaignName, drpCampaignName);
            }
            else
            {
                chkboxGoogleAnalytics.Checked = false;
                pnlGoogleAnalytics.Visible = false;
            }
        }

        private void SetPrePopGoogleAnalyticsControl(
            IEnumerable<CommunicatorCampaignItemLinkTracking> result,
            int ltpId,
            TextBox controlTextBox,
            DropDownList controlDropDown)
        {
            var item = result.FirstOrDefault(x => x.LTPID == ltpId);
            if (item == null)
            {
                return;
            }

            if (item.LTPOID == 6)
            {
                controlTextBox.Visible = true;
                controlTextBox.Text = item.CustomValue;
            }

            controlDropDown.SelectedValue = item.LTPOID.ToString();
        }

        private void PrepareOptOutGroupList(CommunicatorCampaignItem campaignItem)
        {
            if (campaignItem.OptOutGroupList?.Any() != true)
            {
                return;
            }

            chkOptOutSpecificGroup.Checked = true;
            var tempDt = OptoutGroups_DT;
            foreach (var cioog in campaignItem.OptOutGroupList)
            {
                var g = BusinessGroup.GetByGroupID(cioog.GroupID.GetValueOrDefault(), ECNSession.CurrentSession().CurrentUser);
                if (g != null)
                {
                    var dr = tempDt.NewRow();
                    dr[CampaignItemOptOutIdField] = cioog.CampaignItemOptOutID.ToString();
                    dr[GroupIdField] = cioog.GroupID;
                    dr[GroupNameField] = g.GroupName;
                    dr[IsDeletedField] = false;
                    tempDt.Rows.Add(dr);
                }
            }

            OptoutGroups_DT = tempDt;
            loadOptoutGroupsGrid();
        }
    }
}