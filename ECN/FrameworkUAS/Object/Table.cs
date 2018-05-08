using System;
using System.Linq;
using System.Runtime.Serialization;

namespace KMPlatform.Object
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
