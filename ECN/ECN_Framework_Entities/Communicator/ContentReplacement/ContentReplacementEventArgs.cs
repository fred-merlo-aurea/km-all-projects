using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECN_Framework_Entities.Communicator.ContentReplacement
{
    public class ContentReplacementEventArgs<ContextT> : EventArgs
    {
        public int? BlastID { get; set; }
        public string FeedName { get; set; }
        public int CustomerID { get; set; }
        public System.Text.RegularExpressions.Match Match { get; set;}
        public ContextT Context { get; set; }
        public string Text { get; set; }
        public string HTML { get; set; }
    }
}
