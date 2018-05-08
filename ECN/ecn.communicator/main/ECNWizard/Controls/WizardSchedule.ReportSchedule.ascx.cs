using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web.UI.WebControls;
using ecn.communicator.main.Helpers;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;
using Blast = ECN_Framework_BusinessLayer.Communicator.Blast;
using BusinessCampaignItem = ECN_Framework_BusinessLayer.Communicator.CampaignItem;
using BusinessCampaignItemBlast = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast;
using BusinessLinkTrackingParam = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParam;
using BusinessCampaignItemLinkTracking = ECN_Framework_BusinessLayer.Communicator.CampaignItemLinkTracking;
using Email = ECN_Framework_BusinessLayer.Communicator.Email;
using LinkTracking = ECN_Framework_BusinessLayer.Communicator.LinkTracking;
using LinkTrackingSettings = ECN_Framework_BusinessLayer.Communicator.LinkTrackingSettings;
using BusinessCampaignItemTestBlast = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast;
using BusinessContent = ECN_Framework_BusinessLayer.Communicator.Content;
using BusinessCampaignItemOptOutGroup = ECN_Framework_BusinessLayer.Communicator.CampaignItemOptOutGroup;

namespace ecn.communicator.main.ECNWizard.Controls
{
    public partial class WizardSchedule
    {
        private const string MinusSeparator = "-";
        private const string GroupIdField = "GroupID";
        private const string CampaignItemOptOutIdField = "CampaignItemOptOutID";
        private const string IsDeletedField = "IsDeleted";
        private const string OmnitureChannel1 = "Omniture1";
        private const string OmnitureChannel2 = "Omniture2";
        private const string OmnitureChannel3 = "Omniture3";
        private const string OmnitureChannel4 = "Omniture4";
        private const string OmnitureChannel5 = "Omniture5";
        private const string OmnitureChannel6 = "Omniture6";
        private const string OmnitureChannel7 = "Omniture7";
        private const string OmnitureChannel8 = "Omniture8";
        private const string OmnitureChannel9 = "Omniture9";
        private const string OmnitureChannel10 = "Omniture10";

        protected void btnSchedule_Click(object sender, EventArgs e)
        {
            var setupInfo = BlastScheduler1.SetupSchedule(getCampaignItemType());
            var currentUser = ECNSession.CurrentSession().CurrentUser;
            mpeMACheck.Hide();
            if (setupInfo == null)
            {
                throwECNException("Error when setting up the schedule.");
                return;
            }

            if (setupInfo.IsTestBlast.GetValueOrDefault())
            {
                ScheduleTestBlast(currentUser, setupInfo);
            }
            else
            {
                if (getCampaignItemType().Equals("ab", StringComparison.InvariantCultureIgnoreCase) &&
                    setupInfo.SendNowAmount != null &&
                    setupInfo.SendNowAmount % 2 != 0)
                {
                    throwECNException("ERROR - Please enter an even amount to send to.");
                    return;
                }

                ValidateAndScheduleBlast(setupInfo, currentUser);
            }
        }

        private void ValidateAndScheduleBlast(BlastSetupInfo setupInfo, User currentUser)
        {
            var campaignItem = BusinessCampaignItem.GetByCampaignItemID_NoAccessCheck(CampaignItemID, true);
            campaignItem.SendTime = setupInfo.SendTime.GetValueOrDefault();
            campaignItem.OverrideAmount = setupInfo.SendNowAmount;
            campaignItem.OverrideIsAmount = setupInfo.SendNowIsAmount;
            campaignItem.BlastScheduleID = setupInfo.BlastScheduleID;
            campaignItem.UpdatedUserID = currentUser.UserID;

            if (!CheckContentStatus(campaignItem) ||
                !CheckReportTimeIsInFuture(campaignItem) ||
                (chkboxGoogleAnalytics.Checked && !CheckGoogleAnalytics()) ||
                (chkboxOmnitureTracking.Checked && !CheckOmniture()))
            {
                return;
            }

            ScheduleBlast(currentUser, campaignItem);
        }

        private void ScheduleBlast(User currentUser, CampaignItem campaignItem)
        {
            try
            {
                BusinessCampaignItemLinkTracking.DeleteByCampaignItemID(campaignItem.CampaignItemID, currentUser);
                var linkTrackingList = LinkTracking.GetAll();

                if (chkboxGoogleAnalytics.Checked)
                {
                    AddGoogleConversionTrackingChannel(campaignItem);
                }

                if (chkboxConvTracking.Checked)
                {
                    AddConversionTrackingChannel(currentUser, campaignItem);
                }

                if (chkboxOmnitureTracking.Checked)
                {
                    AddOmnitureLinkTrackingChannel(currentUser, campaignItem);
                }

                RemoveUnselectedChannelsLinkTracking(currentUser, campaignItem, linkTrackingList);

                if (chkOptOutSpecificGroup.Checked)
                {
                    UpdateOptOutGroups(currentUser, campaignItem);
                }
                else
                {
                    BusinessCampaignItemOptOutGroup.Delete(campaignItem.CampaignItemID, CurrentCustomer.CustomerID, currentUser);
                }

                foreach (var ciBlast in campaignItem.BlastList)
                {
                    ciBlast.AddOptOuts_to_MS = chkOptOutMasterSuppression.Checked;
                    ciBlast.UpdatedUserID = ECNSession.CurrentSession().CurrentUser.UserID;
                    BusinessCampaignItemBlast.Save(ciBlast, currentUser);
                }

                if (chkCacheBuster.Checked)
                {
                    campaignItem.EnableCacheBuster = true;
                }

                var campaignItemId = BusinessCampaignItem.Save(campaignItem, currentUser);
                Blast.CreateBlastsFromCampaignItem(campaignItem.CampaignItemID, currentUser);
                var newBlastList = Blast.GetByCampaignItemID(campaignItemId, currentUser, false);

                SaveScheduleReport(newBlastList
                    .Where(x => x.TestBlast.Equals("N", StringComparison.InvariantCultureIgnoreCase))
                    .ToList());

                Response.Redirect(getCampaignItemType().ToLower().Equals("ab") && chkGoToChampion.Checked
                                    ? $"wizardsetup.aspx?campaignitemtype=Champion&SampleID={campaignItem.SampleID.GetValueOrDefault()}"
                                    : "default.aspx");
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
        }

        private void UpdateOptOutGroups(User currentUser, CampaignItem campaignItem)
        {
            if (OptoutGroups_DT == null)
            {
                return;
            }

            foreach (var dr in OptoutGroups_DT.AsEnumerable())
            {
                var isDeleted = dr[IsDeletedField].ToString();
                if (dr[CampaignItemOptOutIdField].ToString().Contains(MinusSeparator) &&
                    isDeleted.Equals(bool.FalseString))
                {
                    var ciOptOutGroup = new CampaignItemOptOutGroup
                    {
                        CampaignItemID = campaignItem.CampaignItemID,
                        CustomerID = CurrentCustomer.CustomerID,
                        CreatedUserID = ECNSession.CurrentSession().CurrentUser.UserID,
                        GroupID = Convert.ToInt32(dr[GroupIdField].ToString())
                    };
                    BusinessCampaignItemOptOutGroup.Save(ciOptOutGroup, currentUser);
                }

                if (isDeleted.Equals(bool.TrueString) &&
                    !dr[CampaignItemOptOutIdField].ToString().Contains(MinusSeparator))
                {
                    BusinessCampaignItemOptOutGroup.Delete( Convert.ToInt32(dr[CampaignItemOptOutIdField].ToString()), currentUser);
                }
            }
        }

        private void AddGoogleConversionTrackingChannel(CampaignItem campaignItem)
        {
            SaveCampaignItemLinkTrackingForGoogleAnalytics(campaignItem, 1, drpCampaignSource);
            SaveCampaignItemLinkTrackingForGoogleAnalytics(campaignItem, 2, drpCampaignMedium);
            SaveCampaignItemLinkTrackingForGoogleAnalytics(campaignItem, 3, drpCampaignTerm);
            SaveCampaignItemLinkTrackingForGoogleAnalytics(campaignItem, 4, drpCampaignContent);
            SaveCampaignItemLinkTrackingForGoogleAnalytics(campaignItem, 5, drpCampaignName);
        }

        private static void AddConversionTrackingChannel(User currentUser, CampaignItem campaignItem)
        {
            SaveCampaignItemLinkTrackingForConversionTracking(currentUser, campaignItem, 6);
            SaveCampaignItemLinkTrackingForConversionTracking(currentUser, campaignItem, 7);
        }

        private void AddOmnitureLinkTrackingChannel(User currentUser, CampaignItem campaignItem)
        {
            var linkTrackingParamList = BusinessLinkTrackingParam.GetByLinkTrackingID(OmnitureLinkTrackingId);

            SaveCampaignItemLinkTrackingForOmniture(
                currentUser,
                campaignItem,
                linkTrackingParamList,
                OmnitureChannel1,
                ddlOmniture1,
                txtOmniture1);

            SaveCampaignItemLinkTrackingForOmniture(
                currentUser,
                campaignItem,
                linkTrackingParamList,
                OmnitureChannel2,
                ddlOmniture2,
                txtOmniture2);

            SaveCampaignItemLinkTrackingForOmniture(
                currentUser,
                campaignItem,
                linkTrackingParamList,
                OmnitureChannel3,
                ddlOmniture3,
                txtOmniture3);

            SaveCampaignItemLinkTrackingForOmniture(
                currentUser,
                campaignItem,
                linkTrackingParamList,
                OmnitureChannel4,
                ddlOmniture4,
                txtOmniture4);

            SaveCampaignItemLinkTrackingForOmniture(
                currentUser,
                campaignItem,
                linkTrackingParamList,
                OmnitureChannel5,
                ddlOmniture5,
                txtOmniture5);

            SaveCampaignItemLinkTrackingForOmniture(
                currentUser,
                campaignItem,
                linkTrackingParamList,
                OmnitureChannel6,
                ddlOmniture6,
                txtOmniture6);

            SaveCampaignItemLinkTrackingForOmniture(
                currentUser,
                campaignItem,
                linkTrackingParamList,
                OmnitureChannel7,
                ddlOmniture7,
                txtOmniture7);

            SaveCampaignItemLinkTrackingForOmniture(
                currentUser,
                campaignItem,
                linkTrackingParamList,
                OmnitureChannel8,
                ddlOmniture8,
                txtOmniture8);

            SaveCampaignItemLinkTrackingForOmniture(
                currentUser,
                campaignItem,
                linkTrackingParamList,
                OmnitureChannel9,
                ddlOmniture9,
                txtOmniture9);

            SaveCampaignItemLinkTrackingForOmniture(
                currentUser,
                campaignItem,
                linkTrackingParamList,
                OmnitureChannel10,
                ddlOmniture10,
                txtOmniture10);
        }

        private void RemoveUnselectedChannelsLinkTracking(User currentUser, CampaignItem campaignItem, List<ECN_Framework_Entities.Communicator.LinkTracking> linkTrackingList)
        {
            var channelsToRemove = new List<string>();
            if (!chkboxGoogleAnalytics.Checked)
            {
                channelsToRemove.Add("Google");
            }

            if (!chkboxConvTracking.Checked)
            {
                channelsToRemove.Add("ECN Conversion Tracking");
            }

            if (!chkboxOmnitureTracking.Checked)
            {
                channelsToRemove.Add("Omniture");
            }

            if (!channelsToRemove.Any())
            {
                return;
            }

            foreach (var channel in linkTrackingList
                .Where(src => channelsToRemove.Contains(src.DisplayName))
                .Select(src => src.LTID)
                .Distinct())
            {
                BusinessCampaignItemLinkTracking.DeleteByLTID(campaignItem.CampaignItemID,
                    channel,
                    currentUser);
            }
        }

        private void SaveCampaignItemLinkTrackingForOmniture(
            User currentUser,
            CampaignItem campaignItem,
            List<LinkTrackingParam> linkTrackingParamList,
            string displayName,
            DropDownList omnitureDropDown,
            TextBox omnitureTextBox)
        {
            int ltpoId;
            if (!int.TryParse(omnitureDropDown.SelectedValue, out ltpoId))
            {
                return;
            }

            var omni = new CampaignItemLinkTracking
            {
                LTPID = linkTrackingParamList.First(x => x.DisplayName == displayName).LTPID,
                LTPOID = ltpoId
            };
            if (omni.LTPOID == -1)
            {
                omni.CustomValue = omnitureTextBox.Text.Trim();
            }

            omni.CampaignItemID = campaignItem.CampaignItemID;
            BusinessCampaignItemLinkTracking.Save(omni, currentUser);
        }

        private static void SaveCampaignItemLinkTrackingForConversionTracking(User currentUser, CampaignItem campaignItem, int ltpid)
        {
            var convTracking1 = new CampaignItemLinkTracking
            {
                LTPID = ltpid,
                CampaignItemID = campaignItem.CampaignItemID
            };
            BusinessCampaignItemLinkTracking.Save(convTracking1, currentUser);
        }

        private void SaveCampaignItemLinkTrackingForGoogleAnalytics(CampaignItem campaignItem, int ltpid, DropDownList selectionDropDown)
        {
            var googleAnalytics1 = new CampaignItemLinkTracking
            {
                LTPID = ltpid,
                LTPOID = Convert.ToInt32(selectionDropDown.SelectedValue)
            };
            if (Convert.ToInt32(selectionDropDown.SelectedValue) == 6)
            {
                googleAnalytics1.CustomValue = txtCampaignSource.Text;
            }

            googleAnalytics1.CampaignItemID = campaignItem.CampaignItemID;
            BusinessCampaignItemLinkTracking.Save(googleAnalytics1,
                ECNSession.CurrentSession().CurrentUser);
        }

        private bool CheckOmniture()
        {
            var customerId = CurrentCustomer.CustomerID;
            var baseChannelId = ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID;
            var linkTrackingParamList = BusinessLinkTrackingParam.GetByLinkTrackingID(OmnitureLinkTrackingId);
            var ltsBase = LinkTrackingSettings.GetByBaseChannelID_LTID(
                ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID,
                OmnitureLinkTrackingId);
            var ltsCustomer = LinkTrackingSettings.GetByCustomerID_LTID(CurrentCustomer.CustomerID, OmnitureLinkTrackingId);

            var omnitureSettings = OmnitureHelper.GetOmnitureSettings(ltsBase, ltsCustomer);
            if (
                !ValidateOmnitureChannel(omnitureSettings, linkTrackingParamList, baseChannelId, customerId, OmnitureChannel1, ddlOmniture1, txtOmniture1) ||
                !ValidateOmnitureChannel(omnitureSettings, linkTrackingParamList, baseChannelId, customerId, OmnitureChannel2, ddlOmniture2, txtOmniture2) ||
                !ValidateOmnitureChannel(omnitureSettings, linkTrackingParamList, baseChannelId, customerId, OmnitureChannel3, ddlOmniture3, txtOmniture3) ||
                !ValidateOmnitureChannel(omnitureSettings, linkTrackingParamList, baseChannelId, customerId, OmnitureChannel4, ddlOmniture4, txtOmniture4) ||
                !ValidateOmnitureChannel(omnitureSettings, linkTrackingParamList, baseChannelId, customerId, OmnitureChannel5, ddlOmniture5, txtOmniture5) ||
                !ValidateOmnitureChannel(omnitureSettings, linkTrackingParamList, baseChannelId, customerId, OmnitureChannel6, ddlOmniture6, txtOmniture6) ||
                !ValidateOmnitureChannel(omnitureSettings, linkTrackingParamList, baseChannelId, customerId, OmnitureChannel7, ddlOmniture7, txtOmniture7) ||
                !ValidateOmnitureChannel(omnitureSettings, linkTrackingParamList, baseChannelId, customerId, OmnitureChannel8, ddlOmniture8, txtOmniture8) ||
                !ValidateOmnitureChannel(omnitureSettings, linkTrackingParamList, baseChannelId, customerId, OmnitureChannel9, ddlOmniture9, txtOmniture9) ||
                !ValidateOmnitureChannel(omnitureSettings, linkTrackingParamList, baseChannelId, customerId, OmnitureChannel10, ddlOmniture10, txtOmniture10))
            {
                return false;
            }

            if (!CheckQueryParamLength())
            {
                throwECNException("ERROR - Omniture query string exceeds 255 characters");
                return false;
            }

            return true;
        }

        private bool ValidateOmnitureChannel(
            OmnitureSettings omnitureSettings,
            List<LinkTrackingParam> linkTrackingParamList,
            int baseChannelId,
            int customerId,
            string omnitureChannel,
            DropDownList dropDownList,
            TextBox textBox)
        {
            LinkTrackingParamSettings ltps;
            if (omnitureSettings.UseBaseChannel())
            {
                ltps = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings
                    .Get_LTPID_BaseChannelID(
                        linkTrackingParamList.First(x => x.DisplayName == omnitureChannel).LTPID,
                        baseChannelId);
            }
            else
            {
                ltps = ECN_Framework_BusinessLayer.Communicator.LinkTrackingParamSettings
                    .Get_LTPID_CustomerID(
                        linkTrackingParamList.First(x => x.DisplayName == omnitureChannel).LTPID,
                        customerId);
            }

            if (ltps.IsRequired && dropDownList.SelectedIndex == 0)
            {
                throwECNException($"ERROR - Please select a value for {ltps.DisplayName}");
                return false;
            }

            if (dropDownList.SelectedValue == "-1" && string.IsNullOrWhiteSpace(textBox.Text))
            {
                throwECNException($"ERROR - Please enter a custom value for {ltps.DisplayName}");
                return false;
            }

            return true;
        }

        private bool CheckGoogleAnalytics()
        {
            if (drpCampaignMedium.SelectedValue.Equals("0") ||
                drpCampaignName.SelectedValue.Equals("0") ||
                drpCampaignSource.SelectedValue.Equals("0"))
            {
                throwECNException("ERROR - Please select the required values for Google Analytics Tracking");
                return false;
            }

            if (drpCampaignSource.SelectedValue.Equals("6") && string.IsNullOrWhiteSpace(txtCampaignSource.Text))
            {
                throwECNException("ERROR - Please enter a value for Campaign Source");
                return false;
            }

            if (drpCampaignContent.SelectedValue.Equals("6") && string.IsNullOrWhiteSpace(txtCampaignContent.Text))
            {
                throwECNException("ERROR - Please enter a value for Campaign Content");
                return false;
            }

            if (drpCampaignMedium.SelectedValue.Equals("6") && string.IsNullOrWhiteSpace(txtCampaignMedium.Text))
            {
                throwECNException("ERROR - Please enter a value for Campaign Medium");
                return false;
            }

            if (drpCampaignName.SelectedValue.Equals("6") && string.IsNullOrWhiteSpace(txtCampaignName.Text))
            {
                throwECNException("ERROR - Please enter a value for Campaign Name");
                return false;
            }

            if (drpCampaignTerm.SelectedValue.Equals("6") && string.IsNullOrWhiteSpace(txtCampaignTerm.Text))
            {
                throwECNException("ERROR - Please enter a value for Campaign Term");
                return false;
            }

            return true;
        }

        private bool CheckReportTimeIsInFuture(CampaignItem campaignItem)
        {
            DateTime tempTime;
            DateTime.TryParse(reportTime, out tempTime);
            var tempDate = new DateTime(
                reportDate.Year,
                reportDate.Month,
                reportDate.Day,
                tempTime.Hour,
                tempTime.Minute,
                tempTime.Second);

            if (tempDate.CompareTo(campaignItem.SendTime) <= 0 && tempDate != DateTime.MinValue)
            {
                throwECNException(
                    $"ERROR - You have scheduled a report for {tempDate} but the blast is only scheduled for {campaignItem.SendTime}. Please select a later report time or an earlier campaign send time.");
                return false;
            }

            return true;
        }

        private bool CheckContentStatus(CampaignItem campaignItem)
        {
            try
            {
                BusinessContent.ValidateContentStatus(campaignItem.BlastList[0].LayoutID.GetValueOrDefault());
                return true;
            }
            catch (ECNException ex)
            {
                setECNError(ex);
                return false;
            }
        }

        private void ScheduleTestBlast(User currentUser, BlastSetupInfo setupInfo)
        {
            var cbEmailPreview = (CheckBox) BlastScheduler1.FindControl("cbEmailPreview");
            var testCacheBuster = chkGoogleTestBuster.Checked;

            if (rbTestExisting.Checked)
            {
                TestExistingGroups(currentUser, setupInfo, testCacheBuster, cbEmailPreview);
            }
            else if (rbTestNew.Checked)
            {
                TestNewGroups(setupInfo, cbEmailPreview, testCacheBuster);
            }
            else
            {
                throwECNException("ERROR - Select Group for Testing the Message");
            }
        }

        private void TestNewGroups(BlastSetupInfo setupInfo, CheckBox cbEmailPreview, bool testCacheBuster)
        {
            if (string.IsNullOrWhiteSpace(txtGroupName.Text))
            {
                throwECNException("ERROR - Group Name cannot be empty.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmailAddress.Text))
            {
                throwECNException("ERROR - No Email Address entered.");
                return;
            }

            try
            {
                var campaignItem = BusinessCampaignItem.GetByCampaignItemID(
                    CampaignItemID,
                    ECNSession.CurrentSession().CurrentUser,
                    true);

                if (cbEmailPreview.Checked)
                {
                    try
                    {
                        BusinessContent.ValidateLinks(Convert.ToInt32(campaignItem.BlastList[0].LayoutID));
                    }
                    catch (ECNException ex)
                    {
                        setECNError(ex);
                        return;
                    }
                }

                ScheduleTests(setupInfo, cbEmailPreview, testCacheBuster, campaignItem);

                Response.Redirect("default.aspx");
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
        }

        private void ScheduleTests(
            BlastSetupInfo setupInfo,
            CheckBox cbEmailPreview,
            bool testCacheBuster,
            CampaignItem campaignItem)
        {
            var groupId = AddEmails();
            campaignItem.EnableCacheBuster = testCacheBuster;
            var campaignItemTestBlast = new CampaignItemTestBlast
            {
                CampaignItemID = campaignItem.CampaignItemID,
                GroupID = groupId,
                HasEmailPreview = cbEmailPreview.Checked,
                CreatedUserID = ECNSession.CurrentSession().CurrentUser.UserID,
                CustomerID = ECNSession.CurrentSession().CurrentUser.CustomerID,
                EmailSubject = campaignItem.BlastList[0].EmailSubject,
                FromEmail = campaignItem.FromEmail,
                ReplyTo = campaignItem.ReplyTo,
                FromName = campaignItem.FromName
            };
            using (var scope = new TransactionScope())
            {
                BusinessCampaignItemTestBlast.Insert(campaignItemTestBlast, ECNSession.CurrentSession().CurrentUser);
                scope.Complete();
            }

            if (setupInfo.SendTextTestBlast == true)
            {
                var ciTestText = new CampaignItemTestBlast
                {
                    CampaignItemID = campaignItem.CampaignItemID,
                    GroupID = groupId,
                    HasEmailPreview = cbEmailPreview.Checked,
                    CreatedUserID = ECNSession.CurrentSession().CurrentUser.UserID,
                    CustomerID = ECNSession.CurrentSession().CurrentUser.CustomerID,
                    CampaignItemTestBlastType = "TEXT",
                    EmailSubject = campaignItem.BlastList[0].EmailSubject,
                    FromEmail = campaignItem.FromEmail,
                    ReplyTo = campaignItem.ReplyTo,
                    FromName = campaignItem.FromName
                };

                BusinessCampaignItemTestBlast.Insert(ciTestText, ECNSession.CurrentSession().CurrentUser);
            }
        }

        private void TestExistingGroups(
            User currentUser,
            BlastSetupInfo setupInfo,
            bool testCacheBuster,
            CheckBox cbEmailPreview)
        {
            var selectedGroupsDt = testgroupExplorer1.getSelectedGroups();
            if (!selectedGroupsDt.Any())
            {
                throwECNException("ERROR - Please select at least one group.");
                return;
            }

            var blastLicenseExceed = testgroupExplorer1.FindControl("hfLicenseExceed") as HiddenField;
            if (!string.IsNullOrWhiteSpace(blastLicenseExceed?.Value))
            {
                throwECNException($"ERROR - License limit exceeded for {blastLicenseExceed.Value}");
                return;
            }

            var ecnError = false;
            var campaignItem = BusinessCampaignItem.GetByCampaignItemID(CampaignItemID, currentUser, true);
            campaignItem.UpdatedUserID = currentUser.UserID;
            campaignItem.EnableCacheBuster = testCacheBuster;
            try
            {
                BusinessContent.ValidateContentStatus(Convert.ToInt32(campaignItem.BlastList[0].LayoutID));
            }
            catch (ECNException ex)
            {
                setECNError(ex);
                return;
            }

            if (cbEmailPreview.Checked)
            {
                try
                {
                    BusinessContent.ValidateLinks(Convert.ToInt32(campaignItem.BlastList[0].LayoutID));
                }
                catch (ECNException ex)
                {
                    setECNError(ex);
                    return;
                }
            }

            BusinessCampaignItem.Save(campaignItem, currentUser);

            foreach (var dr in selectedGroupsDt)
            {
                if (!TestExistingGroup(currentUser, setupInfo, cbEmailPreview, campaignItem, dr, ref ecnError))
                {
                    break;
                }
            }

            if (!ecnError)
            {
                Response.Redirect("default.aspx");
            }
        }

        private bool TestExistingGroup(
            User currentUser,
            BlastSetupInfo setupInfo,
            CheckBox cbEmailPreview,
            CampaignItem campaignItem,
            ECNWizard.Group.GroupObject dr,
            ref bool ecnError)
        {
            var campaignItemTestBlast = new CampaignItemTestBlast
            {
                CampaignItemID = campaignItem.CampaignItemID,
                GroupID = dr.GroupID,
                HasEmailPreview = cbEmailPreview.Checked,
                CreatedUserID = currentUser.UserID,
                CustomerID = currentUser.CustomerID,
                Filters = dr.filters,
                EmailSubject = campaignItem.BlastList[0].EmailSubject,
                FromEmail = campaignItem.FromEmail,
                ReplyTo = campaignItem.ReplyTo,
                FromName = campaignItem.FromName
            };
            try
            {
                BusinessCampaignItemTestBlast.Insert(campaignItemTestBlast, currentUser);
            }
            catch (ECNException ex)
            {
                setECNError(ex);
                ecnError = true;
                return false;
            }

            if (setupInfo.SendTextTestBlast == true)
            {
                var ciTestText = new CampaignItemTestBlast
                {
                    CampaignItemID = campaignItem.CampaignItemID,
                    GroupID = dr.GroupID,
                    HasEmailPreview = cbEmailPreview.Checked,
                    CreatedUserID = currentUser.UserID,
                    CustomerID = currentUser.CustomerID,
                    Filters = dr.filters,
                    CampaignItemTestBlastType = "TEXT",
                    EmailSubject = campaignItem.BlastList[0].EmailSubject,
                    FromEmail = campaignItem.FromEmail,
                    ReplyTo = campaignItem.ReplyTo,
                    FromName = campaignItem.FromName
                };
                try
                {
                    BusinessCampaignItemTestBlast.Insert(ciTestText, currentUser);
                }
                catch (ECNException ex)
                {
                    setECNError(ex);
                    ecnError = true;
                    return false;
                }
            }

            return true;
        }

        private void SetupBlastScheduler(bool isTestBlast, List<CampaignItemBlast> campaignItemBlasts, CampaignItem campaignItem)
        {
            BlastScheduler1.CanScheduleBlast = true;
            BlastScheduler1.RequestBlastID = 0;
            if (campaignItemBlasts.Count > 0)
            {
                BlastScheduler1.SourceBlastID = campaignItemBlasts[0].BlastID ?? 0;
            }
            else
            {
                BlastScheduler1.SourceBlastID = 0;
            }

            BlastScheduler1.CampaignItemType = campaignItem.CampaignItemType.ToLower();
            if (BlastScheduler1.SourceBlastID == 0)
            {
                BlastScheduler1.SetupWizard(isTestBlast);
                pnlTestOptions.Visible = isTestBlast;
            }
            else
            {
                BlastScheduler1.SetupWizard();
                pnlTestOptions.Visible = false;
            }
        }

        private void CheckFromEmailValid(List<string> errorMessages)
        {
            if (!Email.IsValidEmailAddress(txtFromEmail.Text))
            {
                errorMessages.Add($"Invalid from email: {txtFromEmail.Text}.");
            }
        }

        private void CheckFtpCredentialsValid(List<string> errorMessages)
        {
            if (!errorMessages.Any() && !CredentialsValid(txtFtpUsername.Text, txtFtpPassword.Text, txtFtpUrl.Text))
            {
                errorMessages.Add("Entered FTP credentials are invalid");
            }
        }

        private void CheckFtpPasswordEntered(List<string> errorMessages)
        {
            if (string.IsNullOrWhiteSpace(txtFtpPassword.Text))
            {
                errorMessages.Add("Please enter a Password for connecting to the FTP site.");
            }
        }

        private void CheckFtpUrlEntered(List<string> errorMessages)
        {
            if (string.IsNullOrWhiteSpace(txtFtpUrl.Text))
            {
                errorMessages.Add("Please enter a valid FTP URL.");
            }
        }

        private void CheckItemsToExportSelected(List<string> errorMessages)
        {
            var atLeastOneFtp = false;
            foreach (ListItem ftpToExport in lbFtpExports.Items)
            {
                if (ftpToExport.Selected)
                {
                    atLeastOneFtp = true;
                    break;
                }
            }

            if (!atLeastOneFtp)
            {
                errorMessages.Add("Please select the statistics you would like exported to FTP.");
            }
        }

        private void CheckReportTypeSelected(List<string> errorMessages)
        {
            if (!chkFtpExport.Checked && !chkEmailBlastReport.Checked)
            {
                errorMessages.Add("Please select at least one type of report.");
            }
        }
    }
}