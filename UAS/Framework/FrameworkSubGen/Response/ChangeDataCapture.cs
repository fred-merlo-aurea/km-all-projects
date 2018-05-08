using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkSubGen.Response
{
    [Serializable]
    [DataContract]
    public class ChangeDataCapture
    {
        public ChangeDataCapture()
        {
            changeDataCaptures = new Entity.ChangeDataCapture();
        }
        [DataMember]
        public Entity.ChangeDataCapture changeDataCaptures { get; set; }
    }
}
