using System.Collections.Generic;
using System.Data;

namespace ecn.activityengines.Tests.Setup.Interfaces
{
    public interface IPublicPreview
    {
        void WriteToLog(string log);

        bool IsMobileBrowser();
      
        string DoDynamicTags(string html, List<string> dynamicTags, DataRow row, int customerId);
    }
}
