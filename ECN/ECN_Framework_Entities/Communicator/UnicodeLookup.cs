using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECN_Framework_Entities.Communicator
{
    public class UnicodeLookup
    {
        public UnicodeLookup()
        {

        }

        public int Id { get; set; }

        public string Glyph { get; set; }

        public string UnicodeNum { get; set; }

        public string HTML { get; set; }

        public bool IsEnabled { get; set; }

        public string Base64String { get; set; }

        public string UnicodeBytes { get; set; }

    }
}
