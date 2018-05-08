using System;

namespace KMModels
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