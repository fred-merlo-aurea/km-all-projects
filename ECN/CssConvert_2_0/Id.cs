using System;
using System.Collections.Generic;
using System.Text;

namespace CssConvert_2_0
{
    public class Id
    {
        public Id()
        {
            Name = string.Empty;
            ListIdStyle = null;
            ListElement = null;
        }
        public string Name { get; set; }
        public List<Style> ListIdStyle { get; set; }
        public List<Element> ListElement { get; set; }
    }
}
