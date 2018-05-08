using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.ComponentModel;
using ecn.MarketingAutomation.Models.PostModels.ECN_Objects;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;
using CommunicatorEnums = ECN_Framework_Common.Objects.Communicator;

namespace ecn.MarketingAutomation.Models.PostModels.Controls
{
    public class CampaignItem : CampaignControlBase
    {
        private const string CreateCampaignProperty = "campaign_value";
        private const string CreateCampaignItemProperty = "campaign_item_value";
        private const string CampaignItemIdProperty = "campaign_itemID";
        private const string CampaignNameProperty = "campaign_name";
        private const string CampaignIdProperty = "campaign_ID";
        private const string SendTimeProperty = "schedule";
        private const string SuppressedGroupFiltersProperty = "suppression_groups_filter";
        private const string SelectedGroupFiltersProperty = "groups_filter";
        private const string SuppressedGroupsProperty = "suppression_groups";
        private const string SelectedGroupsProperty = "groups";
        private const string SubCategoryProperty = "subcategory";
        private const int DefaultNullableIntPropretyValue = -1;

        private string _CustomerName;
        private string _SubCategory;
        private List<ECN_Objects.GroupSelect> _SelectedGroups;
        private List<ECN_Objects.GroupSelect> _SuppressedGroups;
        private List<ECN_Objects.FilterSelect> _SelectedGroupFilters;
        private List<ECN_Objects.FilterSelect> _SuppressedGroupFilters;
        private DateTime? _SendTime;
        private int? _CampaignID;
        private string _CampaignName;
        private int? _CampaignItemID;
        private bool _CreateCampaignItem;
        private bool _CreateCampaign;
        private new int? _ECNID;
        private new int? _MAControlID;

        [JsonProperty(PropertyName = ControlConsts.EcnIdProperty)]
        public override int ECNID
        {
            get
            {
                return _ECNID ?? DefaultNullableIntPropretyValue;
            }

            set
            {
                _ECNID = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.MAControlIdProperty)]
        public override int MAControlID
        {
            get
            {
                return _MAControlID ?? DefaultNullableIntPropretyValue;
            }

            set
            {
                _MAControlID = value;
            }
        }

        [JsonProperty(PropertyName = "customerid")]
        public override int CustomerID
        {
            get
            {
                return _CustomerID;
            }
            set
            {
                _CustomerID = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.CustomerProperty)]
        public string CustomerName
        {
            get
            {
                return _CustomerName;
            }
            set
            {
                _CustomerName = value;
            }
        }

        [JsonProperty(PropertyName = SubCategoryProperty)]
        public string SubCategory
        {
            get
            {
                return _SubCategory;
            }
            set
            {
                _SubCategory = value;
            }
        }

        [JsonProperty(PropertyName = SelectedGroupsProperty)]
        public List<ECN_Objects.GroupSelect> SelectedGroups
        {
            get
            {
                return _SelectedGroups;
            }
            set
            {
                _SelectedGroups = value;
            }
        }

        [JsonProperty(PropertyName = SuppressedGroupsProperty)]
        public List<ECN_Objects.GroupSelect> SuppressedGroups
        {
            get
            {
                return _SuppressedGroups;
            }
            set
            {
                _SuppressedGroups = value;
            }
        }

        [JsonProperty(PropertyName = SelectedGroupFiltersProperty)]
        public List<ECN_Objects.FilterSelect> SelectedGroupFilters
        {
            get
            {
                return _SelectedGroupFilters;
            }
            set
            {
                _SelectedGroupFilters = value;
            }
        }

        [JsonProperty(PropertyName = SuppressedGroupFiltersProperty)]
        public List<ECN_Objects.FilterSelect> SuppressedGroupFilters
        {
            get
            {
                return _SuppressedGroupFilters;
            }
            set
            {
                _SuppressedGroupFilters = value;
            }
        }

        [JsonProperty(PropertyName = SendTimeProperty)]
        public DateTime? SendTime
        {
            get
            {
                if (_SendTime == null)
                {
                    return DateTime.MinValue;
                }

                return _SendTime;
            }
            set
            {
                _SendTime = value;
            }
        }

        [JsonProperty(PropertyName = CampaignIdProperty)]
        public int CampaignID
        {
            get
            {
                return _CampaignID ?? DefaultNullableIntPropretyValue;
            }
            set
            {
                _CampaignID = value;
            }
        }

        [JsonProperty(PropertyName = CampaignNameProperty)]
        public string CampaignName
        {
            get
            {
                return _CampaignName;
            }
            set
            {
                _CampaignName = value;
            }
        }

        [JsonProperty(PropertyName = CampaignItemIdProperty, DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue(-1)]
        public int CampaignItemID
        {
            get
            {
                return _CampaignItemID ?? DefaultNullableIntPropretyValue;
            }
            set
            {
                _CampaignItemID = value;
            }
        }

        [JsonProperty(PropertyName = CreateCampaignItemProperty)]
        public bool CreateCampaignItem
        {
            get
            {
                return _CreateCampaignItem;
            }
            set
            {
                _CreateCampaignItem = value;
            }
        }

        [JsonProperty(PropertyName = CreateCampaignProperty)]
        public bool CreateCampaign
        {
            get
            {
                return _CreateCampaign;
            }
            set
            {
                _CreateCampaign = value;
            }
        }

        public override decimal width
        {
            get
            {
                return ControlConsts.DefaultVeryLargeWidth;
            }
        }

        public override shapecontent content
        {
            get
            {
                return new shapecontent(string.Empty, string.Empty, ControlConsts.AllignCenterMiddle);
            }
        }

        public CampaignItem()
        {
            _type = CommunicatorEnums.Enums.MarketingAutomationControlType.CampaignItem;
            Text = ControlConsts.ControlTextCampaignItem;
            editable = new shapeEditable();
        }

        public void Validate(User currentUser)
        {
            var ecnMaster = new ECNException(new List<ECNError>());

            ValidateText(ecnMaster);
            ValidateSendTimeInThePast(ecnMaster);

            if (CreateCampaignItem)
            {
                ValidateForCreateCampaignItem(currentUser, ecnMaster);
            }
            else
            {
                ValidateForNotCreateCampaignItem(ecnMaster);
            }

            if (ecnMaster.ErrorList.Any())
            {
                throw ecnMaster;
            }
        }

        private void ValidateSendTimeInThePast(ECNException ecnMaster)
        {
            if (CreateCampaignItem && SendTime < DateTime.Now)
            {
                var errorMessage = string.Format(ControlConsts.CampaignItemSendTimeInThePastErrorMessage, Text);
                ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
            }
        }

        private void ValidateForNotCreateCampaignItem(ECNException ecnMaster)
        {
            if (string.IsNullOrWhiteSpace(CampaignItemName))
            {
                var errorMessage = string.Format(ControlConsts.SelectCampaignItemErrorMessage, Text);
                ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
            }
            else
            {
                var campaingItemToCheck = CampaignItemManager.GetByCampaignItemID_NoAccessCheck(ECNID, true);

                if (campaingItemToCheck.CampaignItemType.ToLower() == ControlConsts.CampaignItemTypeChampion)
                {
                    return;
                }

                foreach (var campaignItemBlast in campaingItemToCheck.BlastList)
                {
                    ValidateBlastEmail(ecnMaster, campaignItemBlast.Blast);
                }
            }
        }

        private void ValidateBlastEmail(ECNException ecnMaster, BlastAbstract blast)
        {
            if (string.IsNullOrWhiteSpace(blast.EmailFrom))
            {
                var errorMessage = string.Format(ControlConsts.FromEmailEmptyErrorMessage, Text);
                ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
            }

            if (string.IsNullOrWhiteSpace(blast.EmailFromName))
            {
                var errorMessage = string.Format(ControlConsts.FromNameEmptyErrorMessage, Text);
                ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
            }
            if (string.IsNullOrWhiteSpace(blast.EmailSubject))
            {
                var errorMessage = string.Format(ControlConsts.EmailSubjectEmptyErrorMessage, Text);
                ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
            }
            if (string.IsNullOrWhiteSpace(blast.ReplyTo))
            {
                var errorMessage = string.Format(ControlConsts.ReplyToEmptyErrorMessage, Text);
                ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
            }
        }

        private void ValidateForCreateCampaignItem(User currentUser, ECNException ecnMaster)
        {
            ValidateCampaignItem(ecnMaster);
            ValidateCampaign(ecnMaster);
            ValidateCampaignItemTemplate(ecnMaster);
            ValidateEmail(ecnMaster);
            ValidateMessageId(ecnMaster, CustomerID);
            ValidateSelectedGroups(ecnMaster, currentUser);
            ValidateSuppressedGroups(ecnMaster);
        }

        private void ValidateCampaignItem(ECNException ecnMaster)
        {
            if (string.IsNullOrWhiteSpace(_CampaignItemName))
            {
                var errorMessage = string.Format(ControlConsts.CampaignItemNameEmptyErrorMessage, Text);
                ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
            }
            else if (
                CampaignID > 0 &&
                ECNID <= 0 &&
                CampaignItemManager.Exists(CampaignItemName, CampaignID, CustomerID))
            {
                var errorMessage = string.Format(ControlConsts.CampaignItemAlreadyExistsErrorMessage, Text);
                ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
            }
        }

        private void ValidateCampaign(ECNException ecnMaster)
        {
            if (CampaignID <= 0 && CreateCampaign)
            {
                if (string.IsNullOrWhiteSpace(CampaignName))
                {
                    var errorMessage = string.Format(ControlConsts.CampaignNameEmptyErrorMessage, Text);
                    ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
                }
                else if (CampaignManager.Exists(-1, CampaignName, CustomerID))
                {
                    var errorMessage = string.Format(ControlConsts.CampaignNameAlreadyExistsErrorMessage, Text);
                    ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
                }
            }
            else if (CampaignID <= 0 && !CreateCampaign)
            {
                var errorMessage = string.Format(ControlConsts.SelectCampaignErrorMessage, Text);
                ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
            }
        }

        private void ValidateSuppressedGroups(ECNException ecnMaster)
        {
            if (SuppressedGroups != null)
            {
                foreach(var suppresedGroup in SuppressedGroups)
                {
                    if (suppresedGroup.CustomerID <= 0)
                    {
                        suppresedGroup.CustomerID = CustomerID;
                    }

                    if (!GroupManager.Exists(suppresedGroup.GroupID, suppresedGroup.CustomerID))
                    {
                        var errorMessage = string.Format(
                            ControlConsts.GroupDoesNotExistForErrorMessage,
                            suppresedGroup.GroupName, Text);
                        ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
                    }

                    if (SuppressedGroupFilters != null)
                    {
                        var groupFilters = SuppressedGroupFilters.Where(x => x.GroupID == suppresedGroup.GroupID).ToList();

                        ValidateFilterSelectList(ecnMaster, groupFilters);
                    }
                }
            }
        }

        private void ValidateSelectedGroups(ECNException ecnMaster, User currentUser)
        {
            if (SelectedGroups != null)
            {
                foreach (var selectedGroup in SelectedGroups)
                {
                    if (!GroupManager.Exists(selectedGroup.GroupID, selectedGroup.CustomerID))
                    {
                        var errorMessage = string.Format(
                            ControlConsts.GroupDoesNotExistForErrorMessage,
                            selectedGroup.GroupName, Text);
                        ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
                    }
                    else
                    {
                        if (GroupManager.IsArchived(selectedGroup.GroupID, selectedGroup.CustomerID))
                        {
                            var errorMessage = string.Format(ControlConsts.ArchivedGroupNotAllowedErrorMessage, Text);
                            ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
                        }
                    }

                    if (MessageID > 0)
                    {
                        try
                        {
                            GroupManager.ValidateDynamicTags(selectedGroup.GroupID, MessageID, currentUser);
                        }
                        catch (ECNException ecnException)
                        {
                            foreach (var error in ecnException.ErrorList)
                            {
                                ecnMaster.ErrorList.Add(error);
                            }
                        }
                    }

                    ValidateSelectedGroupFilters(ecnMaster, selectedGroup);
                }
            }
            else
            {
                var errorMessage = string.Format(ControlConsts.SelectGroupToSendToErrorMessage, Text);
                ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
            }
        }

        private void ValidateSelectedGroupFilters(ECNException ecnMaster, GroupSelect selectedGroup)
        {

            if (SelectedGroupFilters != null)
            {
                var groupFilters = SelectedGroupFilters.Where(x => x.GroupID == selectedGroup.GroupID).ToList();
                ValidateFilterSelectList(ecnMaster, groupFilters);
            }
        }

        private void ValidateFilterSelectList(ECNException ecnMaster, IList<FilterSelect> groupFilters)
        {
            foreach (var suppressedFilter in groupFilters)
            {
                var filterExists = ECN_Framework_BusinessLayer.Communicator.Filter.Exists(
                    suppressedFilter.FilterID,
                    suppressedFilter.CustomerID);

                if (!filterExists)
                {
                    var errorMessage = string.Format(ControlConsts.FilterNotExistsErrorMessage, suppressedFilter.FilterName, Text);
                    ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
                }
            }
        }
    }
}