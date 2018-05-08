using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KM.Integration.Marketo
{
    public class Result
    {
        public string type { get; set; }

        public string id { get; set; }

        public string status { get; set; }

        public List<Reason> reasons { get; set; }
    }
}
