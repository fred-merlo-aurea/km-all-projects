using System;
using System.Collections.Generic;
using System.Linq;
using ECN_Framework_Common.Objects;
using KMPlatform.Entity;
using Newtonsoft.Json;

namespace ecn.MarketingAutomation.Models.PostModels.Controls
{
    public class Direct_Click : CampaignControlBase, ICancellable, IValidateFormAndDirectClick
    {
        private bool _AnyLink;
        private string _SpecificLink;
        private string _ActualLink;
        private bool _isCancelled;
        private DateTime? _cancelDate;

        [JsonProperty(PropertyName = ControlConsts.AnyLinkProperty, NullValueHandling = NullValueHandling.Ignore)]
        public bool AnyLink
        {
            get
            {
                return _AnyLink;
            }
            set
            {
                _AnyLink = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.SpecificLinkProperty, NullValueHandling = NullValueHandling.Ignore)]
        public string SpecificLink
        {
            get
            {
                return _SpecificLink;
            }
            set
            {
                _SpecificLink = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.ActualLinkProperty, NullValueHandling = NullValueHandling.Ignore)]
        public string ActualLink
        {
            get
            {
                return _ActualLink;
            }
            set
            {
                _ActualLink = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.IsCancelledProperty)]
        public bool IsCancelled
        {
            get
            {
                return _isCancelled;
            }
            set
            {
                _isCancelled = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.CancelDateProperty)]
        public DateTime? CancelDate
        {
            get
            {
                return _cancelDate;

            }
            set
            {
                _cancelDate = value;
            }
        }

        public override decimal width
        {
            get
            {
                return ControlConsts.DefaultLargeWidth;
            }
        }

        public override shapecontent content
        {
            get
            {
                return new shapecontent(string.Empty, ControlConsts.DefaultContentColorBlue);
            }
        }

        public Direct_Click()
        {
            _type = ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_Click;
            editable = new PostModels.shapeEditable();
        }

        public void Validate(
            Wait parentWait,
            bool hasCampaignItem,
            CampaignItem parentCampaignItem,
            Group parentGroup,
            ControlBase parent,
            User currentUser)
        {
            var masterException = new ECNException(new List<ECNError>());
            var customerId = GetCustomerId(parentCampaignItem, parentGroup);
            var parentLayoutID = SafeGetMessageId(parent);

            ValidateText(masterException);
            ValidateCampaignItem(masterException);
            ValidateMessageId(masterException, customerId);
            ValidateEmail(masterException);
            ValidateParent(masterException, hasCampaignItem, parentCampaignItem, parentGroup, currentUser);
            if (!AnyLink)
            {
                ValidateActualLink(masterException, parentLayoutID, ActualLink);
            }
            
            if (masterException.ErrorList.Any())
            {
                throw masterException;
            }
        }
    }
}