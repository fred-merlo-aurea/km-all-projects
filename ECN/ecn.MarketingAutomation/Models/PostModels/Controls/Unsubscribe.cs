using System;
using System.Collections.Generic;
using System.Linq;
using ECN_Framework_Common.Objects;
using Newtonsoft.Json;
using CommunicatorEnums = ECN_Framework_Common.Objects.Communicator;

namespace ecn.MarketingAutomation.Models.PostModels.Controls
{
    public class Unsubscribe : CampaignControlBase, ICancellable, IValidateSubscribeControl
    {
        private const string CriteriaUnsubscribe = "U";

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

        public Unsubscribe()
        {
            _type = CommunicatorEnums.Enums.MarketingAutomationControlType.Unsubscribe;
            editable = new shapeEditable();
        }

        public void Validate(Group unSubscribeGroup, KMPlatform.Entity.User CurrentUser)
        {
            var masterException = new ECNException(new List<ECNError>());

            ValidateText(masterException);
            ValidateCampaignItem(masterException, true, null, unSubscribeGroup);
            ValidateUnsubscribeTrigger(masterException, unSubscribeGroup);
            ValidateMessageId(masterException, unSubscribeGroup.CustomerID);
            ValidateEmail(masterException);
            ValidateParentGroup(masterException, unSubscribeGroup, CurrentUser, false);

            if (masterException.ErrorList.Any())
            {
                throw masterException;
            }
        }

        private void ValidateUnsubscribeTrigger(ECNException ecnMaster, Group unSubscribeGroup)
        {
            if (ECNID <= 0)
            {
                if (LayoutPlansManager.Exists(unSubscribeGroup.GroupID, CriteriaUnsubscribe))
                {
                    var errorMessage = string.Format(ControlConsts.GroupHasUnsubscribeTriggerErrorMessage, unSubscribeGroup.GroupName);
                    ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
                }
            }
        }
    }
}