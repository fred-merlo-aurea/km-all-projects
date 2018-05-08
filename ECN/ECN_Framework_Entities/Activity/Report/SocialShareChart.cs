using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Activity.Report
{
    [Serializable]
    [DataContract]
    public class SocialShareChart
    {
        public SocialShareChart()
        {
            Share = null;
            DisplayName = string.Empty;
        }

        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public int? Share { get; set; }
    }
}
