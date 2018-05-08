using Newtonsoft.Json;
using CommunicatorEnums = ECN_Framework_Common.Objects.Communicator;

namespace ecn.MarketingAutomation.Models.PostModels.Controls
{
    public class Start : VisualControlBase
    {
        private const decimal ControlWidth = 138;
        private const decimal ControlHeight = 38;

        public override decimal width
        {
            get
            {
                return ControlWidth;
            }
        }

        public override decimal height
        {
            get
            {
                return ControlHeight;
            }

        }

        public override shapecontent content
        {
            get
            {
                return new shapecontent(
                    ControlConsts.ControlTextStart,
                    ControlConsts.DefaultContentColorBlue,
                    ControlConsts.AllignCenterMiddle);
            }
        }

        [JsonProperty(PropertyName = "text")]
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

        public Start()
        {
            _type = CommunicatorEnums.Enums.MarketingAutomationControlType.Start;
            editable = new shapeEditable();
        }
    }
}