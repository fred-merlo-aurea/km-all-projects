using System.Collections.Generic;
using CommunicatorEnums = ECN_Framework_Common.Objects.Communicator;
namespace ecn.MarketingAutomation.Models.PostModels.Controls
{
    public interface IVisualControl
    {
        CommunicatorEnums.Enums.MarketingAutomationControlType ControlType { get; set; }

        int MAControlID { get; set; }

        string ControlID { get; set; }

        int ECNID { get; set; }

        string ExtraText { get; set; }

        bool IsDirty { get; set; }

        string Text { get; set; }

        decimal xPosition { get; set; }

        decimal yPosition { get; set; }

        decimal width { get; }

        decimal height { get; }

        string fill { get; }

        string type { get; }

        shapeEditable editable { get; set; }

        string ControlTypeAsString { get; set; }

        string cursor { get; }

        bool selectable { get; }

        bool serializable { get; }

        bool enable { get; }

        string path { get; }

        bool autoSize { get; }

        bool? visual { get; }

        int minWidth { get; }

        int minHeight { get; }

        shapecontent content { get; }

        List<connectors> connectors { get; }

        rotation rotation { get; }

        int MarketingAutomationID { get; set; }
    }
}