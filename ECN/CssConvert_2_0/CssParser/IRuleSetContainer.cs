using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CssConvert.CssParser
{
    public interface IRuleSetContainer
    {
        /// <summary></summary>
        List<RuleSet> RuleSets { get; set; }
    }
}