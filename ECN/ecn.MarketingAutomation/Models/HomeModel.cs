using System.Collections.Generic;

namespace ecn.MarketingAutomation.Models
{
    public class HomeModel
    {
        public IEnumerable<DiagramViewModel> ActiveDiagrams { get; set; }

        public IEnumerable<MarketingAutomtionViewModel> ActiveAutomations { get; set; }
    }
}