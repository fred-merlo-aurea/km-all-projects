using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkSubGen.Response
{
    [Serializable]
    [DataContract]
    public class Publication
    {
        public Publication()
        {
            publications = new List<Entity.Publication>();
        }
        [DataMember]
        public List<Entity.Publication> publications { get; set; }
    }
}
