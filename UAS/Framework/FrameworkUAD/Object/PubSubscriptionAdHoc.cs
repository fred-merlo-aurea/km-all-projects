using System;
using System.Linq;

namespace FrameworkUAD.Object
{
    public class PubSubscriptionAdHoc
    {
        public PubSubscriptionAdHoc()
        {

        }
        public PubSubscriptionAdHoc(string field, string val)
        {
            this.AdHocField = field;
            this.Value = val;
        }
        public string AdHocField
        {
            get;
            set;
        }
        public string Value
        {
            get;
            set;
        }
    }
}
