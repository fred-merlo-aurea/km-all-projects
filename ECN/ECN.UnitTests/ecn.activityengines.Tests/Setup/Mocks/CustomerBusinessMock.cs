using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecn.activityengines.Tests.Setup.Interfaces;
using ECN_Framework_Entities.Accounts;
using Moq;
using Shim = ECN_Framework_BusinessLayer.Accounts.Fakes.ShimCustomer;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class CustomerBusinessMock : Mock<ICustomerBusiness>
    {
        public CustomerBusinessMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            Shim.GetByCustomerIDInt32Boolean = GetByCustomerID;
        }

        private Customer GetByCustomerID(int customerId, bool getChildren)
        {
            return Object.GetByCustomerID(customerId, getChildren);
        }
    }
}
