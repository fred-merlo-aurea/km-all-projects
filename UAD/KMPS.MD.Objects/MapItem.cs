using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace KMPS.MD.Objects
{
    public class MapItem
    {
        #region properties
        [DataMember]
        public string SubscriberID { get; set; }
        [DataMember]
        public string MapAddress { get; set; }
        [DataMember]
        public decimal Latitude { get; set; }
        [DataMember]
        public decimal Longitude { get; set; }
        [DataMember]
        public string MarkerImage { get; set; }
        [DataMember]
        public string ZipCode { get; set; }
        #endregion

        public MapItem() { }
    }
}