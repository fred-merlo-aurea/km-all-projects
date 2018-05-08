using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace EmailMarketing.API.Models.EmailGroup
{
    [Flags]
    [Serializable]
    public enum Statuses
    {
        /// <summary>Indicates failure due to an unknown issue.</summary>
        [XmlEnum]
        Unknown = 0,
        /// <summary>Indicates the subscriber was successfully added</summary>
        [XmlEnum]
        New = 1,
        /// <summary>Indicates an existing subscriber record was successfully modified</summary>
        [XmlEnum]
        Updated = 2,
        /// <summary>Indicates the subscriber record could not be added due to an issue with the request.</summary>
        [XmlEnum]
        Skipped = 4,
        /// <summary>Indicates the subscriber is a member of the group</summary>

        [XmlEnum]
        Subscribed = 8,
        /// <summary>Indicates the subscriber is unsubscribed from the group</summary>
        [XmlEnum]
        Unsubscribed = 16,
        /// <summary>Indicates failure because completing the requested action would duplicate the email address within the group.</summary>
        [XmlEnum]
        Duplicate = 32,
        /// <summary>Indicates the email address is (or as a result of this request was added to) the Master Suppression list.</summary>
        [XmlEnum]
        MasterSuppressed = 64,
        /// <summary>Indicates failure due to an email address which is not valid or is incorrectly formatted</summary>

        [XmlEnum]
        InvalidEmailAddress = 128,
        /// <summary>Indicates failure due to an invalid or unknown GroupID</summary>
        [XmlEnum]
        InvalidGroupId = 256,
        /// <summary>Indicates failure due to an unknown SubscribeTypeCode.</summary>
        [XmlEnum]
        InvalidSubscribeTypeCode = 512,
        /// <summary>Indicates failure due to an unknown FormatTypeCode</summary>
        [XmlEnum]
        InvalidFormatTypeCode = 1024,
        /// <summary>Indicates failure to unsubscribe due to the subscriber not being found in association with the Group.</summary>
        [XmlEnum]
        UnknownSubscriber = 2048,
        /// <summary>Indicates failure due to one or more custom fields not being found in association with the Group.</summary>
        [XmlEnum]
        UnknownCustomField = 4096,
        /// <summary>Indicates failure due to one or more transactional custom fields not being found in association with the Group.</summary>
        [XmlEnum]
        UnknownTransactionalField = 8192,

        /// <summary>Indicates failure due to one or more custom field transactional field sets not supplying a value for the primary key field.</summary>
        [XmlEnum]
        MissingTransactionalPrimaryKeyField = 16384,

        /// <summary>Indicates failure of a method that does not support duplicate email addresses due to an email address already having
        /// been defined in the target group multiple times.</summary>
        [XmlEnum]
        DuplicateEmailAddress = 32768
    }
}