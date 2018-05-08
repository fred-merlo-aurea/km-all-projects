using System.Collections.Generic;

namespace KMPS_Tools.Classes
{
    public class BpaFileArgs
    {
        public Blast Blast { get; set; }
        public int Count { get; set; }
        public string Subscriber { get; set; } = string.Empty;
        public string Line { get; set; } = string.Empty;
        public IDictionary<string, int> SendDictionary { get; set; }
        public IReadOnlyDictionary<string, string> BounceDictionary { get; set; }
        public IDictionary<string, string> IpDictionary { get; set; } = new Dictionary<string, string>();
    }
}
