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
    public class NoOpen : CampaignControlBase, IEstimatedSendTime, IValidateControl
    {
        private DateTime? _EstSendTime;

        [JsonProperty(PropertyName = ControlConsts.EstSendTimeProperty)]
        public DateTime? EstSendTime
        {
            get
            {
                return _EstSendTime;
            }
            set
            {
                _EstSendTime = value;
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
                return new shapecontent(string.Empty, string.Empty, ControlConsts.AllignCenterMiddle);
            }
        }
        public NoOpen()
        {
            _type = CommunicatorEnums.Enums.MarketingAutomationControlType.NoOpen;
            editable = new shapeEditable();
        }

        public void Validate(
            Wait parentWait,
            CampaignItem parentCampaignItem,
            CommunicatorEntities.MarketingAutomation marketingAutomation,
            User currentUser)
        {
            var ecnMaster = new ECNException(new List<ECNError>());

            ValidateCampaignItem(ecnMaster, true, parentCampaignItem);
            ValidateText(ecnMaster);
            ValidateSendTime(ecnMaster, parentCampaignItem, parentWait, marketingAutomation);
            ValidateMessageId(ecnMaster, parentCampaignItem.CustomerID);
            ValidateEmail(ecnMaster);
            ValidateParentCampaignItem(ecnMaster, parentCampaignItem, currentUser);

            if (ecnMaster.ErrorList.Any())
            {
                throw ecnMaster;
            }
        }
    }
}