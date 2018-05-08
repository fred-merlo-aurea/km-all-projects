using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAS.Object
{
    [Serializable]
    [DataContract]
    public class Table
    {
        #region
        [DataMember]
        public string TableName { get; set; }
        #endregion
    }
}
