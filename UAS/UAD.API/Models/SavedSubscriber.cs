using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml;
using System.Runtime.Serialization;
using System.Collections;
using System.Xml.Serialization;

namespace UAD.API.Models
{
    [Serializable]
    [DataContract(Namespace = "")]
    public class SavedSubscriber
    {
        #region Exposed Properties
        #region new fields
        /// <summary>
        /// Process Code
        /// </summary>
        [DataMember]
        public string ProcessCode { get; set; }
        /// <summary>
        /// Pub ID
        /// </summary>
        [DataMember]
        public int PubID { get; set; }
        /// <summary>
        /// StatusCode
        /// </summary>
        [DataMember]
        public int StatusCode { get; set; }
        /// <summary>
        /// StatusMessage
        /// </summary>
        [DataMember]
        public string StatusMessage { get; set; }
        #endregion

        #region Hidden Fields
        ///// <summary>
        ///// Subscription ID
        ///// </summary>
        //[DataMember]
        //public int SubscriptionID { get; set; }
        ///// <summary>
        ///// Is Pub Code Valid
        ///// </summary>
        //[DataMember]
        //public bool IsPubCodeValid { get; set; }
        ///// <summary>
        ///// Is Code Sheet Valid
        ///// </summary>
        //[DataMember]
        //public bool IsCodeSheetValid { get; set; }
        ///// <summary>
        ///// Is Product Subscriber Created
        ///// </summary>
        //[DataMember]
        //public bool IsProductSubscriberCreated { get; set; }

        ///// <summary>
        ///// Log Message
        ///// </summary>
        //[DataMember]
        //public string LogMessage { get; set; }
        ///// <summary>
        ///// Pub Code Message
        ///// </summary>
        //[DataMember]
        //public string PubCodeMessage { get; set; }
        ///// <summary>
        ///// Code Sheet Message
        ///// </summary>
        //[DataMember]
        //public string CodeSheetMessage { get; set; }
        ///// <summary>
        ///// Subscriber Produc tMessage
        ///// </summary>
        //[DataMember]
        //public string SubscriberProductMessage { get; set; }

        ///// <summary>
        ///// Subscriber Product Identifiers
        ///// </summary>
        //[DataMember]
        //public Dictionary<Guid, string> SubscriberProductIdentifiers { get; set; }
        #endregion
        #endregion

        public SavedSubscriber()
        {            
            ProcessCode = string.Empty;
            PubID = 0;
            StatusCode = 200;
            StatusMessage = "";
        }

        public SavedSubscriber(Models.SaveSubscriber s)
        {                    
            this.ProcessCode = s.ProcessCode;
            this.PubID = s.PubID;

            if (s.IsPubCodeValid == false)            
                this.StatusCode = 400;            
            else if (s.IsCodeSheetValid == false)            
                this.StatusCode = 207;            
            else
                this.StatusCode = 201;            

            string LogMessage = s.LogMessage.Trim();
            string PubCodeMessage = s.PubCodeMessage.Trim();
            string CodeSheetMessage = s.CodeSheetMessage.Trim();
            string SubscriberProductMessage = s.SubscriberProductMessage.Trim();

            this.StatusMessage = SubscriberProductMessage + " " + PubCodeMessage + " " + CodeSheetMessage + " " + LogMessage;
            this.StatusMessage = this.StatusMessage.Trim();
        }
    }
}