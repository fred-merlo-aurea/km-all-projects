using System;
using System.Collections.Generic;
using System.Linq;
using ECN_Framework_Common.Objects;
using KMPlatform.Entity;
using CommunicatorEnums = ECN_Framework_Common.Objects.Communicator;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;

namespace ecn.MarketingAutomation.Models.PostModels.Controls
{
    public class Suppressed : CampaignControlBase, IEstimatedSendTime, IValidateControl
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

        public Suppressed()
        {
            _type = CommunicatorEnums.Enums.MarketingAutomationControlType.Suppressed;
            editable = new shapeEditable();
        }

        public void Validate(
            Wait parentWait,
            CampaignItem parentCampaignItem,
            CommunicatorEntities.MarketingAutomation ma,
            User currentUser)
        {
            var masterException = new ECNException(new List<ECNError>());

            ValidateText(masterException);
            ValidateCampaignItem(masterException, true, parentCampaignItem);
            ValidateSendTime(masterException, parentCampaignItem, parentWait, ma);
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