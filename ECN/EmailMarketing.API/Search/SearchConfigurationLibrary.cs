using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailMarketing.API.Search
{
    /// <summary>
    /// Contains groups of search configuration organized by controller name.
    /// </summary>
    public class SearchConfigurationLibrary : Dictionary<string, SearchConfigurationGroup>
    {
    }
}