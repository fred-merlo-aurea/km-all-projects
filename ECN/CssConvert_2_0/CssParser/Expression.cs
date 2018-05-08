using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CssConvert.CssParser
{
    public class Expression
    {
        private List<Term> terms = new List<Term>();

        /// <summary></summary>
        [XmlArrayItem("Term", typeof(Term))]
        [XmlArray("Terms")]
        public List<Term> Terms
        {
            get { return terms; }
            set { terms = value; }
        }

        /// <summary></summary>
        /// <returns></returns>
        public override string ToString()
        {
            System.Text.StringBuilder txt = new System.Text.StringBuilder();
            bool first = true;
            foreach (Term t in terms)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    txt.AppendFormat("{0} ", t.Seperator.HasValue ? t.Seperator.Value.ToString() : "");
                }
                //if (first) { first = false; } else { txt.Append(" "); }
                txt.Append(t.ToString());
            }
            return txt.ToString();
        }
    }
}