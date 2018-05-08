using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkSubGen.Response
{
    [Serializable]
    [DataContract]
    public class Bundle
    {
        public Bundle()
        {
            bundles = new List<Entity.Bundle>();
        }
        [DataMember]
        public List<Entity.Bundle> bundles { get; set; }
    }
}
