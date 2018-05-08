using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkSubGen.Response
{
    [Serializable]
    [DataContract]
    public class CountryPrice
    {
        public CountryPrice()
        {
            countryprices = new List<Entity.CountryPrice>();
        }
        [DataMember]
        public List<Entity.CountryPrice> countryprices { get; set; }
    }
}
