using System;
using System.Collections.Generic;
using System.Linq;
using ECN_Framework_Common.Objects;
using KMPlatform.Entity;
using CommunicatorEnums = ECN_Framework_Common.Objects.Communicator;
using Newtonsoft.Json;

namespace ecn.MarketingAutomation.Models.PostModels.Controls
{
    public class Open_NoClick : CampaignControlBase, IEstimatedSendTime, IValidateControl
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
                return ControlConsts.DefaultVeryLargeWidth;
            }
        }

        public override shapecontent content
        {
            get
            {
                return new shapecontent(string.Empty, string.Empty, string.Empty);
            }
        }

        public Open_NoClick()
        {
            _type = CommunicatorEnums.Enums.MarketingAutomationControlType.Open_NoClick;
            editable = new shapeEditable();
        }

        public void Validate(
            Wait parentWait,
            CampaignItem parentCampaignItem,
            ECN_Framework_Entities.Communicator.MarketingAutomation ma,
            User CurrentUser)
        {
            var masterException = new ECNException(new List<ECNError>());

            ValidateText(masterException);
            ValidateCampaignItem(masterException, true, parentCampaignItem);
            ValidateSendTime(masterException, parentCampaignItem, parentWait, ma);
            ValidateMessageId(masterException, parentCampaignItem.CustomerID);
            ValidateEmail(masterException);
            ValidateParentCampaignItem(masterException, parentCampaignItem, CurrentUser);

            if (masterException.ErrorList.Any())
            {
                throw masterException;
            }
        }
    }
}