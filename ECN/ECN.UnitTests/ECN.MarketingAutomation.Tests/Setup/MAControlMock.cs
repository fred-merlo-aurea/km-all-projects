using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecn.MarketingAutomation.Tests.Setup.Interfaces;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using Moq;

namespace ecn.MarketingAutomation.Tests.Setup
{
    [ExcludeFromCodeCoverage]
    public class MAControlMock : Mock<IMAControl>
    {
        public MAControlMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            ShimMAControl.SaveMAControl = Save;
            ShimMAControl.GetByControlIDStringInt32 = GetByControlID;
        }

        private MAControl GetByControlID(string controlId, int maId)
        {
            return Object.GetByControlID(controlId, maId);
        }

        private int Save(MAControl mAControl)
        {
            return Object.Save(mAControl);
        }
    }
}
