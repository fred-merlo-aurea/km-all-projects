using System.Collections.Generic;
using System.Linq;
using ECN_Framework_Common.Objects;
using CommunicatorEnums = ECN_Framework_Common.Objects.Communicator;
using Newtonsoft.Json;

namespace ecn.MarketingAutomation.Models.PostModels.Controls
{
    public class Form : VisualControlBase
    {
        private bool _AnyLink;
        private string _SpecificLink;
        private string _ActualLink;
        private int _FormID;
        private string _FormName;
        private int? _HeatMapStats;

        [JsonProperty(PropertyName = ControlConsts.AnyLinkProperty, NullValueHandling = NullValueHandling.Ignore)]
        public bool AnyLink
        {
            get
            {
                return _AnyLink;
            }
            set
            {
                _AnyLink = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.SpecificLinkProperty, NullValueHandling = NullValueHandling.Ignore)]
        public string SpecificLink
        {
            get
            {
                return _SpecificLink;
            }
            set
            {
                _SpecificLink = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.ActualLinkProperty, NullValueHandling = NullValueHandling.Ignore)]
        public string ActualLink
        {
            get
            {
                return _ActualLink;
            }
            set
            {
                _ActualLink = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.FormIdProperty)]
        public int FormID
        {
            get
            {
                return _FormID;
            }
            set
            {
                _FormID = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.FormNameProperty)]
        public string FormName
        {
            get
            {
                return _FormName;

            }
            set
            {
                _FormName = value;
            }
        }
     
        [JsonProperty(PropertyName = ControlConsts.HeatMapStatsProperty)]
        public int? HeatMapStats
        {
            get
            {
                return _HeatMapStats;
            }
            set
            {
                _HeatMapStats = value;
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

        public Form()
        {
            _type = CommunicatorEnums.Enums.MarketingAutomationControlType.Form;
            editable = new shapeEditable();
        }

        public void Validate(ControlBase parent, KMPlatform.Entity.User CurrentUser)
        {
            var ecnMaster = new ECNException(new List<ECNError>());

            ValidateText(ecnMaster);
            ValidateFormId(ecnMaster);
            ValidateLinks(ecnMaster, parent);

            if (ecnMaster.ErrorList.Any())
            {
                throw ecnMaster;
            }
        }

        private void ValidateLinks(ECNException ecnMaster, ControlBase parent)
        {
            if (string.IsNullOrWhiteSpace(SpecificLink))
            {
                var errorMessage = string.Format(ControlConsts.LinkEmptyErrorMessage, Text);
                ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
            }
            else if (!string.IsNullOrWhiteSpace(ActualLink))
            {
                var parentLayoutID = SafeGetMessageId(parent);

                if (!LinkAliasManager.Exists(parentLayoutID, ActualLink))
                {
                    var errorMessage = string.Format(ControlConsts.LinkDoesNotExistErrorMessage, Text);
                    ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
                }
            }
        }

        private void ValidateFormId(ECNException ecnMaster)
        {
            if (FormID <= 0)
            {
                ecnMaster.ErrorList.Add(GetCampaignItemError(ControlConsts.PleaseSelectFormErrorMessage));
            }
        }
    }
}