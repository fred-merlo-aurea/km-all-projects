using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkSubGen.Response
{
    [Serializable]
    [DataContract]
    public class CustomField
    {
        public CustomField()
        {
            customfields = new List<Entity.CustomField>();
        }
        [DataMember]
        public List<Entity.CustomField> customfields { get; set; }
    }
}
