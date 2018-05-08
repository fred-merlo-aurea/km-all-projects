using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECN_Framework_Entities.Communicator
{
    public class MarketingAutomation
    {
        public MarketingAutomation()
        {
            MarketingAutomationID = -1;
            Name = "";
            State = ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationStatus.Saved; //Bug 36972:Remove 'Active' from status drop down list in home page
            CustomerID = -1;
            CreatedDate = null;
            UpdatedDate = null;
            CreatedUserID = null;
            UpdatedUserID = null;
            LastPublishedDate = null;
            Goal = "";
            StartDate = null;
            EndDate = null;
            JSONDiagram = "";
            IsDeleted = false;
            Controls = new List<Communicator.MAControl>();
            Connectors = new List<MAConnector>();
            Type = ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationType.Simple;
        }

        public int MarketingAutomationID { get; set; }

        public string Name { get; set; }

        public int CustomerID { get; set; }

        public ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationStatus State { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? CreatedUserID { get; set; }

        public int? UpdatedUserID { get; set; }

        public bool IsDeleted { get; set; }

        public string Goal { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? LastPublishedDate { get; set; }

        public string JSONDiagram { get; set; }

        public ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationType Type {get;set;}

        public List<ECN_Framework_Entities.Communicator.MAControl> Controls { get; set; }

        public List<ECN_Framework_Entities.Communicator.MAConnector> Connectors { get; set; }
    }
}
