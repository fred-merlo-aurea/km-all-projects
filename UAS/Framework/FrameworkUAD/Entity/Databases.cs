using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class Databases
    {
        #region
        [DataMember]
        public string DatabaseName { get; set; }
        #endregion
    }
}
