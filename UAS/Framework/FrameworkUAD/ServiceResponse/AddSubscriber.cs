using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.ServiceResponse
{
    [Serializable]
    [DataContract]
    public class AddSubscriber
    {
        public AddSubscriber() 
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
        [DataMember]
        public string ProcessCode { get; set; }
        [DataMember]
        public string LogMessage { get; set; }
        [DataMember]
        public bool IsPubCodeValid { get; set; }
        [DataMember]
        public string PubCodeMessage { get; set; }
        [DataMember]
        public bool IsCodeSheetValid { get; set; }
        [DataMember]
        public string CodeSheetMessage { get; set; }
        [DataMember]
        public bool IsProductSubscriberCreated { get; set; }
        [DataMember]
        public string SubscriberProductMessage { get; set; }
        [DataMember]
        public Dictionary<Guid, string> SubscriberProductIdentifiers { get; set; }
        #endregion
    }
}
