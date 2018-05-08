using System;
using System.Xml.Serialization;

namespace CssConvert.CssParser
{
    public class SimpleSelector
    {
        private Combinator? combinator = null;
        private string elementname;
        private string id;
        private string cls;
        private Attribute attribute;
        private string pseudo;
        private Function function;
        private SimpleSelector child;

        /// <summary></summary>
        [XmlAttribute("combinator")]
        public Combinator? Combinator
        {
            get { return combinator; }
            set { combinator = value; }
        }

        /// <summary></summary>
        [XmlAttribute("element")]
        public string ElementName
        {
            get { return elementname; }
            set { elementname = value; }
        }

        /// <summary></summary>
        [XmlAttribute("id")]
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary></summary>
        [XmlAttribute("class")]
        public string Class
        {
            get { return cls; }
            set { cls = value; }
        }

        /// <summary></summary>
        [XmlAttribute("pseudo")]
        public string Pseudo
        {
            get { return pseudo; }
            set { pseudo = value; }
        }

        /// <summary></summary>
        [XmlElement("Attribute")]
        public Attribute Attribute
        {
            get { return attribute; }
            set { attribute = value; }
        }

        /// <summary></summary>
        [XmlElement("Function")]
        public Function Function
        {
            get { return function; }
            set { function = value; }
        }

        /// <summary></summary>
        [XmlElement("Child")]
        public SimpleSelector Child
        {
            get { return child; }
            set { child = value; }
        }

        /// <summary></summary>
        /// <returns></returns>
        public override string ToString()
        {
            System.Text.StringBuilder txt = new System.Text.StringBuilder();
            if (combinator.HasValue)
            {
                switch (combinator.Value)
                {
                    case CssConvert.CssParser.Combinator.PrecededImmediatelyBy: txt.Append(" + "); break;
                    case CssConvert.CssParser.Combinator.ChildOf: txt.Append(" > "); break;
                    case CssConvert.CssParser.Combinator.PrecededBy: txt.Append(" ~ "); break;
                }
            }
            if (elementname != null) { txt.Append(elementname); }
            if (id != null) { txt.AppendFormat("#{0}", id); }
            if (cls != null) { txt.AppendFormat(".{0}", cls); }
            if (pseudo != null) { txt.AppendFormat(":{0}", pseudo); }
            if (attribute != null) { txt.Append(attribute.ToString()); }
            if (function != null) { txt.Append(function.ToString()); }
            if (child != null)
            {
                if (child.ElementName != null) { txt.Append(" "); }
                txt.Append(child.ToString());
            }
            return txt.ToString();
        }
    }
}