using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecn.MarketingAutomation.Tests.Setup.Interfaces;
using Moq;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using System.Diagnostics.CodeAnalysis;

namespace ecn.MarketingAutomation.Tests.Setup
{
    [ExcludeFromCodeCoverage]
    public class MAConnectorMock : Mock<IMAConnector>
    {
        public MAConnectorMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            ShimMAConnector.SaveMAConnector = Save;
        }

        private int Save(MAConnector connector)
        {
            return Object.Save(connector);
        }
    }
}
