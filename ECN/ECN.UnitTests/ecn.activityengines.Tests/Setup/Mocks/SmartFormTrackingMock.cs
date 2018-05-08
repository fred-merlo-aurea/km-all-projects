using ecn.activityengines.Tests.Setup.Interfaces;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class SmartFormTrackingMock : Mock<ISmartFormTracking>
    {
        public SmartFormTrackingMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            ShimSmartFormTracking.InsertSmartFormTracking = Insert;
        }

        private void Insert(SmartFormTracking smartFormTracking)
        {
            Object.Insert(smartFormTracking);
        }
    }
}
