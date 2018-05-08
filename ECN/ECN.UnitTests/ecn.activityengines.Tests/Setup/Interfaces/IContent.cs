using System.Collections.Generic;
using System.Data;

namespace ecn.activityengines.Tests.Setup.Interfaces
{
    public interface IContent
    {
        List<string> GetTags(List<string> toParse, bool fullTag);

        int CheckForTransnippet(string html);

        string ModifyHTML(string htmlOriginal, DataTable emailProfil);
    }
}
