using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CssConvert.CssParser
{
    public class Selector
    {
        private List<SimpleSelector> simpleselectors = new List<SimpleSelector>();

        /// <summary></summary>
        [XmlArrayItem("SimpleSelector", typeof(SimpleSelector))]
        [XmlArray("SimpleSelectors")]
        public List<SimpleSelector> SimpleSelectors
        {
            get { return simpleselectors; }
            set { simpleselectors = value; }
        }

        /// <summary></summary>
        /// <returns></returns>
        public override string ToString()
        {
            System.Text.StringBuilder txt = new System.Text.StringBuilder();
            bool first = true;
            foreach (SimpleSelector ss in simpleselectors)
            {
                if (first) { first = false; } else { txt.Append(" "); }
                txt.Append(ss.ToString());
            }
            return txt.ToString();
        }
    }
}