using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KMPS_JF_Objects.Objects
{
    public enum MagazineType
    {
        PRINT,DIGITAL
    }; 
        
    public class Price
    {
        public string PubCode { get; set; } 
        public int Term { get; set; }
        public decimal PubPrice { get; set; }
        public MagazineType MagazineType { get; set; }  
    }
}
