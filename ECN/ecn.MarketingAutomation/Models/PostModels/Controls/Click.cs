using System;
using System.Collections.Generic;
using System.Linq;
using ECN_Framework_Common.Objects;
using KMPlatform.Entity;
using CommunicatorEnums = ECN_Framework_Common.Objects.Communicator;
using Newtonsoft.Json;

namespace ecn.MarketingAutomation.Models.PostModels.Controls
{
    public class Click : CampaignControlBase, IEstimatedSendTime, IValidateControl
    {
        private const string EstSendTimeProperty = "estSendTime";

        private DateTime? _EstSendTime;

        [JsonProperty(PropertyName = EstSendTimeProperty)]
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
                return new shapecontent(string.Empty, ControlConsts.DefaultContentColorBlue);
            }
        }

        public Click()
        {
            _type = CommunicatorEnums.Enums.MarketingAutomationControlType.Click;
            _Text = ControlConsts.ControlTextClick;
            editable = new shapeEditable();
        }

        public void Validate(Wait clickWait, CampaignItem clickCI, ECN_Framework_Entities.Communicator.MarketingAutomation ma, User currentUser)
        {
            var ecnMaster = new ECNException(new List<ECNError>());

            ValidateText(ecnMaster);
            ValidateMessageId(ecnMaster, clickCI.CustomerID);
            ValidateCampaignItem(ecnMaster, true, clickCI);
            ValidateEmail(ecnMaster);
            ValidateParentCampaignItem(ecnMaster, clickCI, currentUser);
            ValidateSendTime(
                ecnMaster,
                clickCI,
                clickWait,
                ma,
                ControlConsts.GroupClickSendTimeOutsideDateRangeErrorMessage,
                ControlConsts.GroupClickSendTimeInThePastErrorMessage);

            if (ecnMaster.ErrorList.Any())
            {
                throw ecnMaster;
            }
        }
    }
}