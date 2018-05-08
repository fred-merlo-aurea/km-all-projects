using System;
using System.Collections.Generic;
using System.Linq;
using ECN_Framework_Common.Objects;
using CommunicatorEnums = ECN_Framework_Common.Objects.Communicator;
using Newtonsoft.Json;

namespace ecn.MarketingAutomation.Models.PostModels.Controls
{
    public class Subscribe : CampaignControlBase, ICancellable, IValidateSubscribeControl
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

        public Subscribe()
        {
            _type = CommunicatorEnums.Enums.MarketingAutomationControlType.Subscribe;
            editable = new shapeEditable();
        }

        public void Validate(Group subscribeGroup, KMPlatform.Entity.User CurrentUser)
        {
            var ecnMaster = new ECNException(new List<ECNError>());

            ValidateCampaignItem(ecnMaster, true, null, subscribeGroup);
            ValidateText(ecnMaster);
            ValidateMessageId(ecnMaster, subscribeGroup.CustomerID);
            ValidateEmail(ecnMaster);
            ValidateParentGroup(ecnMaster, subscribeGroup, CurrentUser, false);

            if (ecnMaster.ErrorList.Any())
            {
                throw ecnMaster;
            }
        }
    }
}