using System;
using System.Linq;
using System.Runtime.Serialization;

namespace AMSServicesDocumentation.Models
{
    /// <summary>
    /// The generic Response object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    [DataContract(Name = "ResponseOf{0}")]
    public class Response<T> 
    {
        /// <summary>
        /// ProcessCode for the Response object.
        /// </summary>
        #region Properties
        [DataMember]
        public string ProcessCode { get; set; }
        /// <summary>
        /// Status for the Response object.
        /// </summary>
        [DataMember]
        public FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes Status { get; set; }
        /// <summary>
        /// Message for the Response object.
        /// </summary>
        [DataMember]
        public string Message { get; set; }
        /// <summary>
        /// Result for the Response object.
        /// </summary>
        [DataMember]
        public T Result { get; set; }
        #endregion
    }
}