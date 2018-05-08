using System;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public abstract class GatewayBase : ModelBase
    {
        /// <summary>
        /// ID of the Gateway
        /// </summary>
        [DataMember]
        public int GatewayID { get; set; }

        /// <summary>
        /// Whether gateway is deleted or not
        /// </summary>
        [DataMember]
        public bool IsDeleted { get; set; }
    }
}
