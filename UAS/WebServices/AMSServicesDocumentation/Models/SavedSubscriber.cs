using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AMSServicesDocumentation.Models
{
    /// <summary>
    /// The SavedSubscriber object.
    /// </summary>
    [Serializable]
    [DataContract]
    public class SavedSubscriber
    {
        public SavedSubscriber() 
        {
            ProcessCode = string.Empty;
            LogMessage = string.Empty;
            IsPubCodeValid = false;
            PubCodeMessage = string.Empty;
            IsCodeSheetValid = false;
            CodeSheetMessage = string.Empty;
            IsProductSubscriberCreated = false;
            SubscriberProductMessage = string.Empty;
            SubscriberProductIdentifiers = new Dictionary<Guid, string>();
        }
        #region Properties
        /// <summary>
        /// Process code for the SavedSubscriber object.
        /// </summary>
        [DataMember]
        public string ProcessCode { get; set; }
        /// <summary>
        /// LogMessage for the SavedSubscriber object.
        /// </summary>
        [DataMember]
        public string LogMessage { get; set; }
        /// <summary>
        /// If the pub code for the SavedSubscriber object is valid.
        /// </summary>
        [DataMember]
        public bool IsPubCodeValid { get; set; }
        /// <summary>
        /// PubCodeMessage for the SavedSubscriber object.
        /// </summary>
        [DataMember]
        public string PubCodeMessage { get; set; }
        /// <summary>
        /// If the codesheet for the SavedSubscriber object is valid.
        /// </summary>
        [DataMember]
        public bool IsCodeSheetValid { get; set; }
        /// <summary>
        /// CodeSheetMessage for the SavedSubscriber object.
        /// </summary>
        [DataMember]
        public string CodeSheetMessage { get; set; }
        /// <summary>
        /// If a ProductSubscriber is created for the SavedSubscriber object.
        /// </summary>
        [DataMember]
        public bool IsProductSubscriberCreated { get; set; }
        /// <summary>
        /// SubscriberProductMessage for the SavedSubscriber object.
        /// </summary>
        [DataMember]
        public string SubscriberProductMessage { get; set; }
        /// <summary>
        /// Dictionary of SubscriberProductIdentifiers for the SavedSubscriber object.
        /// </summary>
        [DataMember]
        public Dictionary<Guid, string> SubscriberProductIdentifiers { get; set; }
        #endregion
    }
}