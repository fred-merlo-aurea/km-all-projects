using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecn.activityengines.Tests.Setup.Interfaces
{
    public interface IEmailHistory
    {
        int FindMergedEmailID(int oldEmailId);
    }
}
