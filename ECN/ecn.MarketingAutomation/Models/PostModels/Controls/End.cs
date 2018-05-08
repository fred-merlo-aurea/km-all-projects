using CommunicatorEnums = ECN_Framework_Common.Objects.Communicator;

namespace ecn.MarketingAutomation.Models.PostModels.Controls
{
    public class End : VisualControlBase
    {
        public End()
        {
            _type = CommunicatorEnums.Enums.MarketingAutomationControlType.End;
            _Text = ControlConsts.ControlTextEnd;
            editable = new shapeEditable();
        }

        public override shapecontent content
        {
            get
            {
                return new shapecontent(
                    ControlConsts.ControlTextEnd,
                    ControlConsts.DefaultContentColorBlue,
                    ControlConsts.AllignCenterMiddle);
            }
        }
    }
}