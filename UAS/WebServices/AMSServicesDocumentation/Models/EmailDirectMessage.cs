using System;
using System.Linq;

namespace AMSServicesDocumentation.Models
{
    /// <summary>
    /// The EmailDirectMessage object.
    /// </summary>
    public class EmailDirectMessage
    {
        /// <summary>
        /// Identification Number for your EmailDirect Message.
        /// </summary>
        public int EmailDirectID { get; set; }

        /// <summary>
        /// Where this EmailDirect message was sent from.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Who you're sending this EmailDirect message to.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Name this EmailDirect message will be sent from.
        /// </summary>
        public string FromName { get; set; }

        /// <summary>
        /// Where this EmailDirect message will be sent from.
        /// </summary>
        public string FromEmailAddress { get; set; }

        /// <summary>
        /// Who the recipient should reply to.
        /// </summary>
        public string ReplyEmailAddress { get; set; }
        /// <summary>
        /// Subject line of this EmailDirect message.
        /// </summary>
        public string EmailSubject { get; set; }
        /// <summary>
        /// Body of this EmailDirect message.
        /// </summary>
        public string Content { get; set; }
    }
}