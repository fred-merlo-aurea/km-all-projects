using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAS.Web.Models.Circulations
{
    public class Publication
    {
        public Publication() { }

        public Publication(int pubID, string pubCode, bool isCirc)
        {
            PubID = pubID;
            Pubcode = pubCode;
            IsCirc = isCirc;
        }

        public int PubID { get; set; }
        public string Pubcode { get; set; }
        public bool IsCirc { get; set; }
    }
}