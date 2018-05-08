using System;

namespace ecn.MarketingAutomation.Models
{
    public class GetFromFieldAttribute : Attribute
    {
        public string FieldName;

        public GetFromFieldAttribute(string name)
        {
            FieldName = name;
        }
    }
}