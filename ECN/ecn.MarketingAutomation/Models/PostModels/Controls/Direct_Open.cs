using System;
using System.Collections.Generic;
using System.Linq;
using ECN_Framework_Common.Objects;
using KMPlatform.Entity;
using CommunicatorEnums = ECN_Framework_Common.Objects.Communicator;
using Newtonsoft.Json;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;

namespace ecn.MarketingAutomation.Models.PostModels.Controls
{
    public class Direct_Open : CampaignControlBase, ICancellable, IValidateDirectOpenAndNoOpen
    {
        public Direct_Open()
        {
            _type = CommunicatorEnums.Enums.MarketingAutomationControlType.Direct_Open;
            editable = new shapeEditable();
        }
       
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

        public void Validate(
            Wait parentWait,
            bool hasCampaignItem,
            CampaignItem parentCampaingItem,
            Group parentGroup,
            CommunicatorEntities.MarketingAutomation marketingAutomation,
            User currentUser)
        {
            var ecnMaster = new ECNException(new List<ECNError>());
            var customerId = GetCustomerId(parentCampaingItem, parentGroup);

            ValidateText(ecnMaster);
            ValidateCampaignItem(ecnMaster);
            ValidateMessageId(ecnMaster, customerId);
            ValidateEmail(ecnMaster);
            ValidateParent(ecnMaster, hasCampaignItem, parentCampaingItem, parentGroup, currentUser);

            if (ecnMaster.ErrorList.Any())
            {
                throw ecnMaster;
            }
        }
    }
}