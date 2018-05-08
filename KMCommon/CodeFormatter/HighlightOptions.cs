using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KM.Common.CodeFormatter
{
    // <summary>
    /// Handles all of the options for changing the rendered code.
    /// </summary>
    public partial class HighlightOptions
    {
        private string language, title, code;
        private bool displayLineNumbers;
        private bool alternateLineNumbers;

        public HighlightOptions()
        {
        }

        public HighlightOptions(string language, string title, bool linenumbers, string code, bool alternateLineNumbers)
        {
            this.language = language;
            this.title = title;
            this.alternateLineNumbers = alternateLineNumbers;
            this.code = code;
            this.displayLineNumbers = linenumbers;
        }

        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        public bool DisplayLineNumbers
        {
            get { return displayLineNumbers; }
            set { displayLineNumbers = value; }
        }
        public string Language
        {
            get { return language; }
            set { language = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public bool AlternateLineNumbers
        {
            get { return alternateLineNumbers; }
            set { alternateLineNumbers = value; }
        }
    }
}
