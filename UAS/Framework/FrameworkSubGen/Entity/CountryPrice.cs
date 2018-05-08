using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkSubGen.Entity
{
    [Serializable]
    [DataContract]
    public class CountryPrice
    {
        public CountryPrice() { }
        #region Properties
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public double Price { get; set; }
        [DataMember]
        public int account_id { get; set; }
        #endregion
    }
}
