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
    public class Open : CampaignControlBase, IEstimatedSendTime, IValidateControl
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
                return new shapecontent(string.Empty, String.Empty, ControlConsts.AllignCenterMiddle);
            }
        }

        public Open()
        {
            _type = CommunicatorEnums.Enums.MarketingAutomationControlType.Open;
            editable = new shapeEditable();
        }

        public void Validate(
            Wait parentWait,
            CampaignItem parentCampaignItem,
            CommunicatorEntities.MarketingAutomation marketingAutomation,
            User currentUser)
        {
            var masterException = new ECNException(new List<ECNError>());

            ValidateCampaignItem(masterException, true, parentCampaignItem);
            ValidateText(masterException);
            ValidateSendTime(masterException, parentCampaignItem, parentWait, marketingAutomation);
            ValidateMessageId(masterException, parentCampaignItem.CustomerID);
            ValidateEmail(masterException);
            ValidateParentCampaignItem(masterException, parentCampaignItem, currentUser);

            if (masterException.ErrorList.Any())
            {
                throw masterException;
            }
        }
    }
}