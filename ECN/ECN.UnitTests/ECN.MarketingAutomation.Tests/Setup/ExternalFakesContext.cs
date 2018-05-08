using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecn.MarketingAutomation.Tests.Setup.Interfaces;
using Moq;

namespace ecn.MarketingAutomation.Tests.Setup
{
    [ExcludeFromCodeCoverage]
    public class ExternalFakesContext
    {
        public ExternalFakesContext()
        {
            MAControlMock = new MAControlMock();
            DiagramsControllerMock = new DiagramsControllerMock();
            CampaignItemMock = new CampaignItemMock();
            MAConnectorMock = new MAConnectorMock();
        }

        public MAControlMock MAControlMock { get; private set; }

        public DiagramsControllerMock DiagramsControllerMock { get; private set; }

        public CampaignItemMock CampaignItemMock { get; set; }

        public MAConnectorMock MAConnectorMock { get; }
    }
}
