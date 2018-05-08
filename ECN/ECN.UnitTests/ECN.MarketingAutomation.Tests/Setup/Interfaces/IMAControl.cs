using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECN_Framework_Entities.Communicator;
namespace ecn.MarketingAutomation.Tests.Setup.Interfaces
{
    public interface IMAControl
    {
        int Save(MAControl MAC);
        MAControl GetByControlID(string ControlID, int maControlID);
    }
}
