using System;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public abstract class ModelBase
    {
        /// <summary>
        /// ID of the created user
        /// </summary>
        [DataMember]
        public int? CreatedUserID { get; set; }

        /// <summary>
        /// Date when it was created
        /// </summary>
        [DataMember]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// ID of the user who updated it
        /// </summary>
        [DataMember]
        public int? UpdatedUserID { get; set; }

        /// <summary>
        /// Updated date of this gateway
        /// </summary>
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
    }
}
