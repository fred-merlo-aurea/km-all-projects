using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECN_Framework_Entities.Activity.Report
{    
    public enum FormatType
    {
        Currency,
        Percent
    };

    [Serializable]    
    public class ExportAttribute : Attribute
    {
        public ExportAttribute()
        {
            Ignore = false;
        }

        public int FieldOrder { get; set; }
        public FormatType Format { get; set; }
        public int Total { get; set; }

        public bool Ignore { get; set; }

        public string Header { get; set; }
    }    
}
