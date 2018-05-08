using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.MarketingAutomation.Models.PostModels.ECN_Objects
{
    public class BlastFieldSelect
    {
        public BlastFieldSelect()
        {
            BlastFieldName= "";
        }
        public string BlastFieldName { get; set; }

        public List<string> BlastFieldValues { get; set; }


    }
}