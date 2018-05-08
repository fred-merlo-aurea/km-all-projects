using System;
using System.Collections.Generic;
using System.Text;

namespace CssConvert_2_0
{
    public class Style
    {
        public Style()
        {
            Format = string.Empty;
        }
        public Style(string format)
        {
            Format = format;
        }
        public string Format { get; set; }
    }
}
