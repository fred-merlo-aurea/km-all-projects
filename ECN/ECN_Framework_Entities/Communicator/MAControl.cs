using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECN_Framework_Entities.Communicator
{
    public class MAControl
    {
        public MAControl()
        {
            MAControlID = -1;
            ControlID = "";
            ECNID = -1;
            xPosition = 0.0M;
            yPosition = 0.0M;
            MarketingAutomationID = -1;
            Text = "";
            ExtraText = "";

        }

        public int MAControlID { get; set; }

        public string ControlID { get; set; }

        public int ECNID { get; set; }

        public decimal xPosition { get; set; }

        public decimal yPosition { get; set; }

        public int MarketingAutomationID { get; set; }

        public string Text { get; set; }

        public string ExtraText { get; set; }

        public ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType ControlType { get; set; }
    }
}
