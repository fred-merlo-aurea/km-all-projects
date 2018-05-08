using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecn.communicator.classes.EmailWriter
{
    public class TransnippetHolder
    {
        public static int TransnippetsCount { get; set; }

        public static List<string> Transnippet;

        public static List<string> TransnippetTablesHTML;

        public static List<string> TransnippetTablesTxt;
    }

}
