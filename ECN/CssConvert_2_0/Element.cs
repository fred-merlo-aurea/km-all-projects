using System;
using System.Collections.Generic;
using System.Text;

namespace CssConvert_2_0
{
    public class Element
    {
        public Element()
        {
            Name = string.Empty;
            ListElementStyle = null;
            ListPsuedo = null;
            Parent = null;
        }
        public string Name { get; set; }
        public List<Style> ListElementStyle { get; set; }
        public List<Psuedo> ListPsuedo { get; set; }
        public Element Parent { get; set; }
    }
}
