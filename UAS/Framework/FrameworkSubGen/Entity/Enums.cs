using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkSubGen.Entity
{
    [Serializable]
    [DataContract]
    public class Enums
    {
        [DataContract]
        public enum SubscriptionType
        {
            [EnumMember]
            Print,
            [EnumMember]
            Digital,
            [EnumMember]
            Both
        }
        [DataContract]
        public enum Client
        {
            [EnumMember]
            Master,
            [EnumMember]
            Knowledge_Marketing,
            [EnumMember]
            Source_Media,
            [EnumMember]
            ABC_Publishing,
            [EnumMember]
            KM_API_Testing,
            [EnumMember]
            MacFadden,
            [EnumMember]
            Chief_Executive_Group
        }
        public static Client GetClient(string clientDisplayName)
        {
            try
            {
                clientDisplayName = clientDisplayName.Replace(" ", "_");
                return (Client)System.Enum.Parse(typeof(Client), clientDisplayName, true);
            }
            catch { return Client.Knowledge_Marketing; }
        }

        [DataContract]
        public enum HtmlFieldType
        {
            [EnumMember]
            checkbox,
            [EnumMember]
            select,
            [EnumMember]
            radio,
            [EnumMember]
            text,
            [EnumMember]
            textarea
        }
        [DataContract]
        public enum Entities
        {
            [EnumMember]
            customfields,
            [EnumMember]
            subscriptions,
            [EnumMember]
            subscribers,
            [EnumMember]
            addresses,
            [EnumMember]
            listdownloads,
            [EnumMember]
            purchases,
            [EnumMember]
            bundles
        }
        [DataContract]
        public enum Status
        {
            [EnumMember]
            Auto_renewal_Pending,
            [EnumMember]
            Auto_renewal_Processing,
            [EnumMember]
            Auto_renewal_Complete,
            [EnumMember]
            Pending,
            [EnumMember]
            Generating_Galley,
            [EnumMember]
            Generating,
            [EnumMember]
            Error,
            [EnumMember]
            Complete
        }
        [DataContract]
        public enum PaymentType
        {
            [EnumMember]
            Cash,
            [EnumMember]
            Credit,
            [EnumMember]
            PayPal,
            [EnumMember]
            Check,
            [EnumMember]
            Imported,
            [EnumMember]
            Other
        }
        [DataContract]
        public enum HttpMethod
        {
            [EnumMember]
            POST,
            [EnumMember]
            PUT,
            [EnumMember]
            GET,
            [EnumMember]
            DELETE
        }
        [DataContract]
        public enum Plan
        {
            [EnumMember]
            Standard,
            [EnumMember]
            Premium
        }
    }
}
