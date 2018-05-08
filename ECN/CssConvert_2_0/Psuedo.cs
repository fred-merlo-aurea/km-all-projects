using System;
using System.Collections.Generic;
using System.Text;

namespace CssConvert_2_0
{
    public class Psuedo
    {
        public Psuedo()
        {
            Name = string.Empty;
            ListStyle = null;
        }
        public string Name { get; set; }
        public List<Style> ListStyle { get; set; }
    }
}
