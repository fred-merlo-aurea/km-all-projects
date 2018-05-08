using System;
using System.Collections.Generic;
using System.Linq;
using ECN_Framework_Common.Objects;
using KMPlatform.Entity;
using Newtonsoft.Json;
using CommunicatorEnums = ECN_Framework_Common.Objects.Communicator;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;

namespace ecn.MarketingAutomation.Models.PostModels.Controls
{
    public class Direct_NoOpen : CampaignControlBase, ICancellable, IValidateDirectOpenAndNoOpen
    {
        private bool _isCancelled;
        private DateTime? _cancelDate;
        
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

        public Direct_NoOpen()
        {
            _type = CommunicatorEnums.Enums.MarketingAutomationControlType.Direct_NoOpen;
            editable = new shapeEditable();
        }

        public void Validate(
            Wait parentWait,
            bool hasCampaignItem,
            CampaignItem parentCampaignItem,
            Group parentGroup,
            CommunicatorEntities.MarketingAutomation marketingAutomation,
            User currentUser)
        {
            var ecnMaster = new ECNException(new List<ECNError>());
            var customerID = GetCustomerId(parentCampaignItem, parentGroup);

            ValidateText(ecnMaster);
            ValidateCampaignItem(ecnMaster);
            ValidateMessageId(ecnMaster, customerID);
            ValidateEmail(ecnMaster);
            ValidateParent(ecnMaster, hasCampaignItem, parentCampaignItem, parentGroup, currentUser);

            if (ecnMaster.ErrorList.Any())
            {
                throw ecnMaster;
            }
        }
    }
}