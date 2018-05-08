using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SourceMediaPaidPub.Objects
{
    [XmlRoot("Magazines")]
    public class MagazineXML
    {
        public string PubCode { get; set; }

        public string CoverImage { get; set; }

        public int GroupID { get; set; }

        public int CustomerID { get; set; }

        public string Title { get; set; }

        public int Term { get; set; }

        public bool HasPremium { get; set; }

        public List<PriceXML> DefaultPrice{ get; set; }

        public List<PriceRangeXML> PriceRange { get; set; }

        public bool IsPremium { get; set; }

    }

    public class PriceXML
    {
        public int Term { get; set; }
        public string Type { get; set; }
        public decimal USPrice { get; set; }
        public decimal CANPrice { get; set; }
        public decimal INTPrice { get; set; }
    }

    public class PriceRangeXML
    {
        public decimal PaidFrom { get; set; }
        public decimal PaidTo { get; set; }
        public List<PriceXML> Price{ get; set; }
    }

}