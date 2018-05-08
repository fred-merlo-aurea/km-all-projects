using System.Collections.Generic;
using System.Linq;
using ECN_Framework_Common.Objects;
using CommunicatorEnums = ECN_Framework_Common.Objects.Communicator;
using Newtonsoft.Json;

namespace ecn.MarketingAutomation.Models.PostModels.Controls
{
    public class Group: VisualControlBase
    {
        private const string CustomerIdProperty = "customerid";

        public Group()
        {
            _type = CommunicatorEnums.Enums.MarketingAutomationControlType.Group;
            editable = new shapeEditable();
        }

        private int _CustomerID;
        private string _CustomerName;
        private int _GroupID;
        private string _GroupName;
        private int? _Subscribed;
        private int? _Unsubscribed;
        private int? _Suppressed;
        private int? _HeatMapStats;

        [JsonProperty(PropertyName = CustomerIdProperty)]
        public int CustomerID
        {
            get
            {
                return _CustomerID;
            }
            set
            {
                _CustomerID = value;
            }
        }
        [JsonProperty(PropertyName = ControlConsts.CustomerProperty)]
        public string CustomerName
        {
            get
            {
                return _CustomerName;
            }
            set
            {
                _CustomerName = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.GroupIdProperty)]
        public int GroupID
        {
            get
            {
                return _GroupID;
            }
            set
            {
                _GroupID = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.GroupProperty)]
        public string GroupName
        {
            get
            {
                return _GroupName;

            }
            set
            {
                _GroupName = value;
            }
        }
        
        [JsonProperty(PropertyName = ControlConsts.SubscribedProperty)]
        public int? Subscribed
        {
            get
            {
                return _Subscribed;
            }
            set
            {
                _Subscribed = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.UnsubscribedProperty)]
        public int? Unsubscribed
        {
            get
            {
                return _Unsubscribed;
            }
            set
            {
                _Unsubscribed = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.SuppressedProperty)]
        public int? Suppressed
        {
            get
            {
                return _Suppressed;
            }
            set
            {
                _Suppressed = value;
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

        public void Validate()
        {
            var ecnMaster = new ECNException(new List<ECNError>());

            ValidateText(ecnMaster);
            ValidateGroupId(ecnMaster);

            if (ecnMaster.ErrorList.Any())
            {
                throw ecnMaster;
            }
        }

        private void ValidateGroupId(ECNException ecnMaster)
        {
            if (GroupID > 0)
            {
                if (!GroupManager.Exists(GroupID, CustomerID))
                {
                    var errorMessage = string.Format(ControlConsts.GroupDoesNotExistErrorMessage, GroupName);
                    ecnMaster.ErrorList.Add(GetGroupError(errorMessage));
                }
                else
                {
                    if (GroupManager.IsArchived(GroupID, CustomerID))
                    {
                        var errorMessage = string.Format(ControlConsts.ArchivedGroupNotAllowedErrorMessage, GroupName);
                        ecnMaster.ErrorList.Add(GetGroupError(errorMessage));
                    }
                }
            }
            else
            {
                var errorMessage = string.Format(ControlConsts.PleaseSelectGroupForControlTypeErrorMesage, ControlType);
                ecnMaster.ErrorList.Add(GetGroupError(errorMessage));
            }
        }
    }
}