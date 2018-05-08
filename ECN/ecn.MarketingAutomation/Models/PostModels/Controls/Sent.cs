using System;
using System.Collections.Generic;
using System.Linq;
using ECN_Framework_Common.Objects;
using CommunicatorEnums = ECN_Framework_Common.Objects.Communicator;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;

namespace ecn.MarketingAutomation.Models.PostModels.Controls
{
    public class Sent : CampaignControlBase, IEstimatedSendTime, IValidateControl
    {
        private DateTime? _EstSendTime;

        [Newtonsoft.Json.JsonProperty(PropertyName = ControlConsts.EstSendTimeProperty)]
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

        public Sent()
        {
            _type = CommunicatorEnums.Enums.MarketingAutomationControlType.Sent;
            editable = new shapeEditable();
        }

        public void Validate(Wait sentWait,
            CampaignItem sentCampaignItem,
            CommunicatorEntities.MarketingAutomation marketingAutomation, 
            KMPlatform.Entity.User currentUser)
        {
            var masterException = new ECNException(new List<ECNError>());

            ValidateText(masterException);
            ValidateCampaignItem(masterException, true, sentCampaignItem);
            ValidateSendTime(masterException, sentCampaignItem, sentWait, marketingAutomation);
            ValidateMessageId(masterException, sentCampaignItem.CustomerID);
            ValidateEmail(masterException);
            ValidateParentCampaignItem(masterException, sentCampaignItem, currentUser);

            if (masterException.ErrorList.Any())
            {
                throw masterException;
            }
        }
    }
}