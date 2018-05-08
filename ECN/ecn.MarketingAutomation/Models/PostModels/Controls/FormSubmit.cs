﻿using System;
using System.Collections.Generic;
using ECN_Framework_Common.Objects;
using KMPlatform.Entity;
using CommunicatorEnums = ECN_Framework_Common.Objects.Communicator;
using Newtonsoft.Json;

namespace ecn.MarketingAutomation.Models.PostModels.Controls
{
    public class FormSubmit : CampaignControlBase, IValidateFormAndDirectClick
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

        public FormSubmit()
        {
            editable = new shapeEditable();
            _type = CommunicatorEnums.Enums.MarketingAutomationControlType.FormSubmit;
        }

        public void Validate(
            Wait wait,
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
            ValidateActualLink(masterException, parentLayoutID, ActualLink);
            ValidateEmail(masterException);
            ValidateParent(masterException, hasCampaignItem, parentCampaignItem, parentGroup, currentUser);

            if (masterException.ErrorList.Count > 0)
            {
                throw masterException;
            }
        }
    }
}