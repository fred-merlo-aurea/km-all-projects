using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAD.API.Attributes
{
    public class CustomModelAttributes : Attribute
    {
       
    }

    public class ExcludeFromSearch : CustomModelAttributes
    {
        public ExcludeFromSearch()
        {
            Exclude = false;
            ExclusionMessage = string.Empty;
        }

        #region properties
        public bool Exclude { get; set; }

        public string ExclusionMessage { get; set; }
        #endregion
    }
}