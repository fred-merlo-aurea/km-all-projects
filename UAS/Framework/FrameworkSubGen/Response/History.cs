using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkSubGen.Response
{
    [Serializable]
    [DataContract]
    public class History
    {
        public History()
        {
            historyEntries = new List<Entity.History>();
        }
        [DataMember]
        public List<Entity.History> historyEntries { get; set; }
    }
}
