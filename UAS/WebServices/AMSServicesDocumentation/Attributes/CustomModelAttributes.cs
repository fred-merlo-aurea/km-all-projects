using System;
using System.Linq;

namespace AMSServicesDocumentation.Attributes
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