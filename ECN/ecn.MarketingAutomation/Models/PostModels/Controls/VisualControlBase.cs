using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Interfaces;
using ECN_Framework_Common.Objects;
using Newtonsoft.Json;
using CommunicatorEnums = ECN_Framework_Common.Objects.Communicator;
using FrameworkEnums = ECN_Framework_Common.Objects;

namespace ecn.MarketingAutomation.Models.PostModels.Controls
{
    public class VisualControlBase : ControlBase, IVisualControl
    {
        protected CommunicatorEnums.Enums.MarketingAutomationControlType _type;
        protected int _MAControlID;
        protected string _ControlID;
        protected int _ECNID;
        protected string _ExtraText;
        protected string _Text;
        protected bool _IsDirty;
        protected int _MarketingAutomationID;
        protected decimal _xPosition;
        protected decimal _yPosition;

        protected IGroupManager _groupManager;
        protected ILayoutManager _layoutManager;
        protected ILinkAliasManager _linkAliasManager;
        protected IEmailManager _emailManager;
        protected ICampaignItemManager _campaignItemManager;
        protected ICampaignManager _campaignManager;
        protected ILayoutPlansManager _layoutPlansManager;

        public override CommunicatorEnums.Enums.MarketingAutomationControlType ControlType
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        [Newtonsoft.Json.JsonProperty(PropertyName = ControlConsts.MAControlIdProperty)]
        public override int MAControlID
        {
            get
            {
                return _MAControlID;
            }

            set
            {
                _MAControlID = value;
            }
        }

        [Newtonsoft.Json.JsonProperty(PropertyName = ControlConsts.IdProperty)]
        public override string ControlID
        {
            get
            {
                return _ControlID;
            }

            set
            {
                _ControlID = value;
            }
        }

        [Newtonsoft.Json.JsonProperty(PropertyName = ControlConsts.EcnIdProperty)]
        public override int ECNID
        {
            get
            {
                return _ECNID;
            }

            set
            {
                _ECNID = value;
            }
        }

        [Newtonsoft.Json.JsonProperty(PropertyName = ControlConsts.ExtraTextProperty)]
        public override string ExtraText
        {
            get
            {
                return _ExtraText;
            }

            set
            {
                _ExtraText = value;
            }
        }

        [Newtonsoft.Json.JsonProperty(PropertyName = ControlConsts.IsDirtyProperty)]
        public override bool IsDirty
        {
            get
            {
                return _IsDirty;
            }

            set
            {
                _IsDirty = value;
            }
        }

        public override int MarketingAutomationID
        {
            get
            {
                return _MarketingAutomationID;
            }

            set
            {
                _MarketingAutomationID = value;
            }
        }

        [Newtonsoft.Json.JsonProperty(PropertyName = ControlConsts.ControlTextProperty)]
        public override string Text
        {
            get
            {
                return _Text;
            }

            set
            {
                _Text = value;
            }
        }

        [Newtonsoft.Json.JsonProperty(PropertyName = ControlConsts.XProperty)]
        public override decimal xPosition
        {
            get
            {
                return _xPosition;
            }

            set
            {
                _xPosition = value;
            }
        }

        [Newtonsoft.Json.JsonProperty(PropertyName = ControlConsts.YProperty)]
        public override decimal yPosition
        {
            get
            {
                return _yPosition;
            }

            set
            {
                _yPosition = value;
            }
        }

        public override decimal width
        {
            get
            {
                return ControlConsts.DefaultWidth;
            }
        }

        public override decimal height
        {
            get
            {
                return ControlConsts.DefaultHeight;
            }

        }

        public override shapecontent content
        {
            get
            {
                return new shapecontent(string.Empty, string.Empty);
            }
        }

        public override string fill
        {
            get
            {
                return ControlConsts.DefaultFillColorWhite;
            }

        }

        public override string type
        {
            get
            {
                return ControlConsts.DefaultTypeRectangle;
            }

        }

        [Newtonsoft.Json.JsonProperty(PropertyName = ControlConsts.EditableProperty)]
        public override shapeEditable editable { get; set; }

        [JsonIgnore]
        public IGroupManager GroupManager
        {
            get
            {
                if (_groupManager == null)
                {
                    _groupManager = new GroupManager();
                }
                return _groupManager;
            }
            set { _groupManager = value; }
        }

        [JsonIgnore]
        public ILayoutManager LayoutManager
        {
            get
            {
                if (_layoutManager == null)
                {
                    _layoutManager = new LayoutManager();
                }
                return _layoutManager;
            }
            set { _layoutManager = value; }
        }

        [JsonIgnore]
        public ILinkAliasManager LinkAliasManager
        {
            get
            {
                if (_linkAliasManager == null)
                {
                    _linkAliasManager = new LinkAliasManager();
                }
                return _linkAliasManager;
            }
            set { _linkAliasManager = value; }
        }

        [JsonIgnore]
        public IEmailManager EmailManager
        {
            get
            {
                if (_emailManager == null)
                {
                    _emailManager = new EmailManager();
                }
                return _emailManager;
            }
            set { _emailManager = value; }
        }

        [JsonIgnore]
        public ICampaignItemManager CampaignItemManager
        {
            get
            {
                if (_campaignItemManager == null)
                {
                    _campaignItemManager = new CampaignItemManager();
                }
                return _campaignItemManager;
            }
            set { _campaignItemManager = value; }
        }

        [JsonIgnore]
        public ICampaignManager CampaignManager
        {
            get
            {
                if (_campaignManager == null)
                {
                    _campaignManager = new CampaignManager();
                }
                return _campaignManager;
            }
            set { _campaignManager = value; }
        }

        [JsonIgnore]
        public ILayoutPlansManager LayoutPlansManager
        {
            get
            {
                if (_layoutPlansManager == null)
                {
                    _layoutPlansManager = new LayoutPlansManager();
                }
                return _layoutPlansManager;
            }
            set { _layoutPlansManager = value; }
        }

        protected void ValidateText(ECNException ecnMaster)
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                var errorMessage = string.Format(ControlConsts.ControlNameEmptyErrorMessage, ControlType);
                ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
            }
            else
            {
                if (Text.Length > ControlConsts.MaxLength)
                {
                    ecnMaster.ErrorList.Add(GetCampaignItemError(ControlConsts.ControlNameTooLongErrorMessage));
                }
            }
        }

        public int SafeGetMessageId(ControlBase control)
        {
            var campaignControl = control as ICampaignControl;
            if (campaignControl != null)
            {
                return campaignControl.MessageID;
            }

            return -1;
        }

        protected ECNError GetCampaignItemError(string errorMessage)
        {
            return new ECNError(
                FrameworkEnums.Enums.Entity.CampaignItem,
                FrameworkEnums.Enums.Method.Validate,
                errorMessage);
        }

        protected ECNError GetGroupError(string errorMessage)
        {
            return new ECNError(
                FrameworkEnums.Enums.Entity.Group,
                FrameworkEnums.Enums.Method.Validate,
                errorMessage);
        }
    }
}