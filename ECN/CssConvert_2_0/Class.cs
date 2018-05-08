using System;
using System.Collections.Generic;
using System.Text;

namespace CssConvert_2_0
{
    public class Class
    {
        public Class()
        {
            Name = string.Empty;
            ListClassStyle = null;
            ListElement = null;
        }
        public string Name { get; set; }
        public List<Style> ListClassStyle { get; set; }
        public List<Element> ListElement { get; set; }
    }
}
