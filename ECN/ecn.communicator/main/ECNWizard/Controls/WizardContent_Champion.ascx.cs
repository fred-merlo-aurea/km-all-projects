using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Transactions;
using ECN_Framework_BusinessLayer.Activity.View;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_Common.Objects;
using CampaignItemBlastFilter = ECN_Framework_Entities.Communicator.CampaignItemBlastFilter;
using CommCampaignItemBlast = ECN_Framework_Entities.Communicator.CampaignItemBlast;
using CommCampaignItem = ECN_Framework_Entities.Communicator.CampaignItem;

namespace ecn.communicator.main.ECNWizard.Controls
{
    public partial class WizardContent_Champion : System.Web.UI.UserControl, IECNWizard
    {
        private const string SelectedSuppressionGroupsKey = "SelectedSuppressionGroups";
        private const string SelectedGroupIdKey = "SelectedGroupID";
        private const string SelectedFilterIdKey = "SelectedFilterID";
        private const string GroupNameColumn = "GroupName";
        private const string GroupIdColumn = "GroupID";
        private const string BlastIdColumn = "BlastID";
        private const string BlastSuppressionColumn = "BlastSuppression";
        private const string AbWinnerTypeColumn = "ABWinnerType";
        private const string ReplyTo = "ReplyTo";
        private const string EmailFromName = "EmailFromName";
        private const string EmailFrom = "EmailFrom";
        private const string SelectWinnerMessage = "Please select who to send the winner to";
        private const string NoSampleSelected = "No Sample Selected";

        int _campaignItemID = 0;
        public int CampaignItemID
        {
            set
            {
                _campaignItemID = value;
            }
            get
            {
                return _campaignItemID;
            }
        }

        public int SampleID
        {
            get
            {
                if (Request.QueryString["SampleID"] != null)
                    return Convert.ToInt32(Request.QueryString["SampleID"].ToString());
                else
                    return -1;
            }
        }

        string _errormessage = string.Empty;
        public string ErrorMessage
        {
            set
            {
                _errormessage = value;
            }
            get
            {
                return _errormessage;
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void LoadSampleDD(int sampleid = -1)
        {
            LoadDropDown();
            bool setOptions = false;
            if (sampleid > 0)
            {
                AbSampleBlast.SelectedValue = sampleid.ToString();
                setOptions = true;
            }
            ECN_Framework_Entities.Communicator.CampaignItem ci =
            ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(CampaignItemID, true);
            if (AbSampleBlast.Items.Count > 0)
            {
                if (sampleid > 0)
                {
                    ECN_Framework_Entities.Communicator.Sample s = ECN_Framework_BusinessLayer.Communicator.Sample.GetBySampleID(sampleid, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    chkAorB.Checked = s.DidNotReceiveAB.HasValue ? s.DidNotReceiveAB.Value : true;
                    chkLosingCampaign.Checked = string.IsNullOrEmpty(s.DeliveredOrOpened) ? false : true;
                    if (chkLosingCampaign.Checked)
                    {
                        rblLosingAction.Visible = true;
                        rblLosingAction.SelectedValue = s.DeliveredOrOpened;
                    }
                    else
                    {
                        rblLosingAction.Visible = false;
                    }
                    setOptions = false;
                }
                else
                {
                    if (ci.SampleID != null)
                    {

                        AbSampleBlast.SelectedValue = ci.SampleID.ToString();
                        ECN_Framework_Entities.Communicator.Sample s = ECN_Framework_BusinessLayer.Communicator.Sample.GetBySampleID(ci.SampleID.Value, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                        chkAorB.Checked = s.DidNotReceiveAB.HasValue ? s.DidNotReceiveAB.Value : true;
                        chkLosingCampaign.Checked = string.IsNullOrEmpty(s.DeliveredOrOpened) ? false : true;
                        if (chkLosingCampaign.Checked)
                        {
                            rblLosingAction.Visible = true;
                            rblLosingAction.SelectedValue = s.DeliveredOrOpened;
                        }
                        else
                        {
                            rblLosingAction.Visible = false;
                        }
                        setOptions = false;
                    }

                    else
                    {
                        AbSampleBlast.Items[0].Selected = true;
                        chkAorB.Checked = true;
                        chkLosingCampaign.Checked = true;
                        rblLosingAction.Visible = true;

                    }
                }

                LoadSample(setOptions);
            }
        }

        public void Initialize()
        {

            if (KMPlatform.BusinessLogic.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Blast, KMPlatform.Enums.Access.Edit))
            {
                if (SampleID > 0)
                {
                    LoadDropDown();
                    AbSampleBlast.SelectedValue = SampleID.ToString();
                    LoadSample(true);
                }
                else
                {
                    LoadSampleDD(SampleID);
                }
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
            }
        }

        private void LoadDropDown()
        {
            ECN_Framework_Entities.Communicator.CampaignItem ci =
            ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(CampaignItemID, true);
            DataTable dt = ECN_Framework_BusinessLayer.Communicator.Sample.GetAvailableSamples(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, ci.CampaignItemID);
            AbSampleBlast.DataSource = dt.DefaultView;
            AbSampleBlast.DataBind();
        }

        public bool Save()
        {
            if (AbSampleBlast.Items.Count == 0)
            {
                throwECNException(NoSampleSelected);
            }

            if (!chkAorB.Checked && !chkLosingCampaign.Checked)
            {
                throwECNException(SelectWinnerMessage);
            }

            var dataTable = Blast.GetSampleInfo(ECNSession.CurrentSession().CurrentUser.CustomerID, Convert.ToInt32(AbSampleBlast.SelectedValue.ToString()), ECNSession.CurrentSession().CurrentUser);

            using (var scope = new TransactionScope())
            {
                var sample = Sample.GetBySampleID(Convert.ToInt32(AbSampleBlast.SelectedValue.ToString()), ECNSession.CurrentSession().CurrentUser);
                sample.DidNotReceiveAB = chkAorB.Checked;

                if (chkLosingCampaign.Checked)
                {
                    sample.DeliveredOrOpened = rblLosingAction.SelectedValue;
                }
                else
                {
                    sample.DeliveredOrOpened = string.Empty;
                }

                sample.UpdatedUserID = ECNSession.CurrentSession().CurrentUser.UserID;
                Sample.Save(sample, ECNSession.CurrentSession().CurrentUser);

                var campaignItem = CampaignItem.GetByCampaignItemID_NoAccessCheck(CampaignItemID, true);
                campaignItem.SampleID = Convert.ToInt32(AbSampleBlast.SelectedValue);
                campaignItem.UpdatedUserID = ECNSession.CurrentSession().CurrentUser.UserID;

                var drWinner = dataTable.Rows[0];
                campaignItem.FromEmail = drWinner[EmailFrom].ToString();
                campaignItem.FromName = drWinner[EmailFromName].ToString();

                if (campaignItem.CompletedStep < 2)
                {
                    campaignItem.CompletedStep = 2;
                }

                campaignItem.ReplyTo = drWinner[ReplyTo].ToString();
                CampaignItem.Save(campaignItem, ECNSession.CurrentSession().CurrentUser);
                var ciBlast = GetCampaignItemBlast(campaignItem);

                CampaignItemSuppression.Delete_NoAccessCheck(CampaignItemID, ECNSession.CurrentSession().CurrentUser);
                campaignItem.SuppressionList = new List<ECN_Framework_Entities.Communicator.CampaignItemSuppression>();
                SaveCampaignItemSuppression(campaignItem);
                CampaignItem.Save(campaignItem, ECNSession.CurrentSession().CurrentUser);
                CampaignItemBlast.Save(ciBlast, ECNSession.CurrentSession().CurrentUser);
                Blast.CreateBlastsFromCampaignItem(campaignItem.CampaignItemID, ECNSession.CurrentSession().CurrentUser, true);

                scope.Complete();
            }

            return true;
        }
        
        private CommCampaignItemBlast GetCampaignItemBlast(CommCampaignItem campaignItem)
        {
            var ciBlast = new CommCampaignItemBlast
            {
                CreatedUserID = ECNSession.CurrentSession().CurrentUser.UserID
            };

            if (campaignItem.BlastList.Count > 0)
            {
                ciBlast = campaignItem.BlastList[0];
                ciBlast.UpdatedUserID = ECNSession.CurrentSession().CurrentUser.UserID;
            }

            ciBlast.CustomerID = ECNSession.CurrentSession().CurrentUser.CustomerID;
            ciBlast.CampaignItemID = CampaignItemID;
            ciBlast.GroupID = Convert.ToInt32(ViewState[SelectedGroupIdKey]);

            if ((GroupObject)ViewState[SelectedFilterIdKey] != null)
            {
                var filterIDs = (GroupObject)ViewState[SelectedFilterIdKey];
                if (ciBlast.CampaignItemBlastID > 0)
                {
                    ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.DeleteByCampaignItemBlastID(ciBlast.CampaignItemBlastID);
                }

                ciBlast.Filters.AddRange(CreateCampaignItemBlastFilter(filterIDs.filters));
            }

            return ciBlast;
        }

        private void SaveCampaignItemSuppression(CommCampaignItem campaignItem)
        {
            var suppressionGroups = (List<GroupObject>)ViewState[SelectedSuppressionGroupsKey];
            if (suppressionGroups != null)
            {
                foreach (var groupObject in suppressionGroups)
                {
                    var campaignItemSuppression = new ECN_Framework_Entities.Communicator.CampaignItemSuppression
                    {
                        CampaignItemID = campaignItem.CampaignItemID,
                        CreatedUserID = ECNSession.CurrentSession().CurrentUser.UserID,
                        CustomerID = campaignItem.CustomerID.Value,
                        GroupID = groupObject.GroupID,
                        IsDeleted = false,
                        Filters = new List<CampaignItemBlastFilter>()
                    };

                    campaignItemSuppression.Filters.AddRange(CreateCampaignItemBlastFilter(groupObject.filters));
                    CampaignItemSuppression.Save(campaignItemSuppression, ECNSession.CurrentSession().CurrentUser);
                }
            }
        }

        private IList<CampaignItemBlastFilter> CreateCampaignItemBlastFilter(IList<CampaignItemBlastFilter> campaignItemBlastFilters)
        {
            var filters = new List<CampaignItemBlastFilter>();

            foreach (var campaignItemBlastFilter in campaignItemBlastFilters)
            {
                var itemBlastFilter = new CampaignItemBlastFilter();

                if (campaignItemBlastFilter.FilterID.HasValue)
                {
                    itemBlastFilter.FilterID = campaignItemBlastFilter.FilterID;
                }
                else
                {
                    itemBlastFilter.RefBlastIDs = campaignItemBlastFilter.RefBlastIDs;
                    itemBlastFilter.SmartSegmentID = campaignItemBlastFilter.SmartSegmentID;
                }

                itemBlastFilter.IsDeleted = false;
                filters.Add(itemBlastFilter);
            }

            return filters;
        }

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.BlastChampion, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            throw new ECNException(errorList, Enums.ExceptionLayer.WebSite);
        }

        protected void LoadSampleValues(object sender, EventArgs e)
        {
            LoadSample();
        }        

        protected void gvFilters_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string filterName = string.Empty;
                ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf = (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter)e.Row.DataItem;
                if (cibf.SmartSegmentID != null)
                {
                    ECN_Framework_Entities.Communicator.SmartSegment ss = ECN_Framework_BusinessLayer.Communicator.SmartSegment.GetSmartSegmentByID(cibf.SmartSegmentID.Value);
                    filterName = ss.SmartSegmentName + " " + cibf.RefBlastIDs + "<BR/>";
                }
                else if (cibf.FilterID != null)
                {
                    ECN_Framework_Entities.Communicator.Filter f = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID_NoAccessCheck(cibf.FilterID.Value);
                    filterName = f.FilterName + "<BR/>";
                }
                Label lblFilterName = (Label)e.Row.FindControl("lblFilterName");
                lblFilterName.Text = filterName;
            }
        }

        protected void chkLosingCampaign_CheckedChanged(object sender, EventArgs e)
        {
            rblLosingAction.Visible = chkLosingCampaign.Checked;
        }

        private void LoadSample(bool setOptions = false)
        {
            var sampleId = Convert.ToInt32(AbSampleBlast.SelectedItem.Value);
            var dataTable = Blast.GetSampleInfo(
                ECNSession.CurrentSession().CurrentUser.CustomerID,
                sampleId,
                ECNSession.CurrentSession().CurrentUser);
            var filterIDs = new GroupObject
            {
                filters = new List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter>()
            };
            if (dataTable.Rows.Count > 0)
            {
                LoadSampleRows(setOptions, dataTable, filterIDs, sampleId);
            }
            else
            {
                LoadSampleNoRows();
            }
        }

        private void LoadSampleNoRows()
        {
            ViewState[SelectedSuppressionGroupsKey] = null;
            ViewState[SelectedGroupIdKey] = string.Empty;
            ViewState[SelectedFilterIdKey] = string.Empty;

            lblGroup.Text = string.Empty;
            lblFilter.Text = string.Empty;
            lblSuppressionGroups.Text = string.Empty;
        }

        private void LoadSampleRows(
            bool setOptions,
            DataTable dataTable,
            GroupObject filterIds,
            int sampleId)
        {
            var campaignItemBlast = CampaignItemBlast.GetByBlastID_NoAccessCheck(Convert.ToInt32(dataTable.Rows[0][BlastIdColumn].ToString()), true);

            LoadFilters(filterIds, campaignItemBlast);

            ViewState[SelectedGroupIdKey] = dataTable.Rows[0][GroupIdColumn].ToString();
            ViewState[SelectedFilterIdKey] = filterIds;

            lblGroup.Text = dataTable.Rows[0][GroupNameColumn].ToString();
            InitializeGridViewFilters(campaignItemBlast);
            LoadAbWinner(dataTable);
            LoadSuppressionGroupNames(dataTable);
            LoadSuppressionGroupFilters(campaignItemBlast);
            LoadAndDataBindSampleInfo(sampleId);
            LoadChampCompainItem(setOptions, sampleId);
        }

        private void LoadChampCompainItem(bool setOptions, int sampleId)
        {
            var champCompainItem = CampaignItem.GetByCampaignItemID_NoAccessCheck(CampaignItemID, false);
            if (setOptions && champCompainItem.SampleID.HasValue)
            {
                var sample = Sample.GetBySampleID(sampleId, ECNSession.CurrentSession().CurrentUser);
                chkAorB.Checked = sample.DidNotReceiveAB ?? true;
                chkLosingCampaign.Checked = !string.IsNullOrWhiteSpace(sample.DeliveredOrOpened);
                rblLosingAction.Visible = chkLosingCampaign.Checked;
                if (chkLosingCampaign.Checked)
                {
                    rblLosingAction.SelectedValue = sample.DeliveredOrOpened;
                }
            }
        }

        private void LoadAndDataBindSampleInfo(int sampleId)
        {
            var sampleInfo = BlastActivity.ChampionByProc(sampleId,
                false,
                ECNSession.CurrentSession().CurrentUser,
                lblABWinnerType.Text.ToLower());
            rptrSample.DataSource = sampleInfo;
            rptrSample.DataBind();

            rptrEnvelope.DataSource = sampleInfo;
            rptrEnvelope.DataBind();
        }

        private void LoadSuppressionGroupFilters(ECN_Framework_Entities.Communicator.CampaignItemBlast campaignItemBlast)
        {
            var suppGroupFilters = new StringBuilder();
            var suppGroups = GetSuppressionGroups(campaignItemBlast);
            ViewState[SelectedSuppressionGroupsKey] = suppGroups;
            foreach (var go in suppGroups.Where(go => go.filters != null))
            {
                foreach (var cibf in go.filters)
                {
                    if (cibf.FilterID != null && cibf.FilterID > 0)
                    {
                        var filter = Filter.GetByFilterID_NoAccessCheck(cibf.FilterID.Value);
                        suppGroupFilters.Append($"{filter.FilterName}<br />");
                    }
                    else if (cibf.SmartSegmentID != null && cibf.SmartSegmentID > 0)
                    {
                        var smartSegment = SmartSegment.GetSmartSegmentByID(cibf.SmartSegmentID.Value);
                        suppGroupFilters.Append($"[{cibf.RefBlastIDs}] {smartSegment.SmartSegmentName}<br />");
                    }
                }
            }

            lblSuppGroupFilters.Text = suppGroupFilters.ToString();
        }

        private void LoadSuppressionGroupNames(DataTable dt)
        {
            var suppGroupString = new StringBuilder();
            foreach (var s in dt.Rows[0][BlastSuppressionColumn].ToString().Split(',').Where(t => !string.IsNullOrWhiteSpace(t)))
            {
                var suppressionGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(Convert.ToInt32(s));
                if (suppressionGroup != null)
                {
                    suppGroupString.Append($"{suppressionGroup.GroupName}, ");
                }
            }

            lblSuppressionGroups.Text = suppGroupString.ToString().Trim().TrimEnd(',');
        }

        private void LoadAbWinner(DataTable dt)
        {
            if (!string.IsNullOrWhiteSpace(dt.Rows[0][AbWinnerTypeColumn].ToString()))
            {
                lblABWinnerType.Text = dt.Rows[0][AbWinnerTypeColumn].ToString().ToLower();
            }
            else if (ECNSession.CurrentSession().CurrentCustomer.ABWinnerType != null)
            {
                lblABWinnerType.Text = ECNSession.CurrentSession().CurrentCustomer.ABWinnerType;
            }
            else
            {
                lblABWinnerType.Text = ConfigurationManager.AppSettings["KMWinnerTypeDefault"];
            }
        }

        private void InitializeGridViewFilters(ECN_Framework_Entities.Communicator.CampaignItemBlast campaignItemBlast)
        {
            if (campaignItemBlast.Filters.Any(x =>
                x.CampaignItemSuppresionID == null &&
                x.CampaignItemBlastID != null &&
                x.IsDeleted == false))
            {
                gvFilters.DataSource = campaignItemBlast.Filters.Where(x =>
                        x.CampaignItemSuppresionID == null && x.CampaignItemBlastID != null && x.IsDeleted == false)
                    .ToList();
                gvFilters.DataBind();
                gvFilters.Visible = true;
            }
            else
            {
                gvFilters.Visible = false;
                lblFilter.Text = string.Empty;
            }
        }

        private static void LoadFilters(GroupObject filterIDs, ECN_Framework_Entities.Communicator.CampaignItemBlast campaignItemBlast)
        {
            var listFilter = campaignItemBlast.Filters.Where(x =>
                    x.CampaignItemSuppresionID == null && x.CampaignItemBlastID != null && x.IsDeleted == false)
                .ToList();
            foreach (var filter in listFilter)
            {
                filterIDs.filters.Add(filter);
            }
        }

        private List<GroupObject> GetSuppressionGroups(ECN_Framework_Entities.Communicator.CampaignItemBlast campaignItemBlast)
        {
            var campaignItem = CampaignItem.GetByCampaignItemID_NoAccessCheck(campaignItemBlast.CampaignItemID.GetValueOrDefault(), true);
            var suppGroups = new List<GroupObject>();
            foreach (var suppression in campaignItem.SuppressionList)
            {
                var goSupp = new GroupObject
                {
                    filters = new List<CampaignItemBlastFilter>(),
                    GroupID = suppression.GroupID.GetValueOrDefault()
                };
                foreach (var filter in suppression.Filters)
                {
                    goSupp.filters.Add(filter);
                }

                suppGroups.Add(goSupp);
            }

            return suppGroups;
        }
    }
}