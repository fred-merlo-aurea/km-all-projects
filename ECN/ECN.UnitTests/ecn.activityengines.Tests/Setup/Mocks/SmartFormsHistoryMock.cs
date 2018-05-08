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
    public class SmartFormsHistoryMock:Mock<ISmartFormsHistory>
    {
        public SmartFormsHistoryMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            ShimSmartFormsHistory.GetBySmartFormID_NoAccessCheckInt32Int32 = GetBySmartFormID_NoAccessCheck;
        }

        private SmartFormsHistory GetBySmartFormID_NoAccessCheck(int formId, int groupId)
        {
            return Object.GetBySmartFormID_NoAccessCheck(formId, groupId);
        }
    }
}
