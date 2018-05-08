using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class Frequency
    {
        public Frequency() { }

        #region Properties
        [DataMember]
        public int FrequencyID { get; set; }
        [DataMember]
        public string FrequencyName { get; set; }
        [DataMember]
        public int Issues { get; set; }
        #endregion
    }
}
