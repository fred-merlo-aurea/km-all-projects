using System;
using System.Collections.Generic;
using System.Linq;

namespace ADMS.Services.Validator
{
    public class SubPubList
    {
        public static List<SubPubMap> SubToPubMap
        {
            get
            {
                var lst = new List<SubPubMap>
                {
                    new SubPubMap("AccountNumber", "AccountNumber"),
                    new SubPubMap("Address", "Address1"),
                    new SubPubMap("MailStop", "Address2"),
                    new SubPubMap("Address3", "Address3"),
                    new SubPubMap("City", "City"),
                    new SubPubMap("Company", "Company"),
                    new SubPubMap("", "Copies"),
                    new SubPubMap("Country", "Country"),
                    new SubPubMap("CountryID", "CountryID"),
                    new SubPubMap("County", "County"),
                    new SubPubMap("Demo7", "Demo7"),
                    new SubPubMap("Email", "Email"),
                    new SubPubMap("EmailID", "EmailID"),
                    new SubPubMap("", "EmailStatusID"),
                    new SubPubMap("ExternalKeyId", "ExternalKeyID"),
                    new SubPubMap("Fax", "Fax"),
                    new SubPubMap("Fname", "FirstName"),
                    new SubPubMap("Gender", "Gender"),
                    new SubPubMap("", "GraceIssues"),
                    new SubPubMap("", "IsComp"),
                    new SubPubMap("", "IsPaid"),
                    new SubPubMap("", "IsSubscribed"),
                    new SubPubMap("Lname", "LastName"),
                    new SubPubMap("Latitude", "Latitude"),
                    new SubPubMap("Longitude", "Longitude"),
                    new SubPubMap("Mobile", "Mobile"),
                    new SubPubMap("", "Occupation"),
                    new SubPubMap("OrigsSrc", "OrigsSrc"),
                    new SubPubMap("Par3C", "Par3CID"),
                    new SubPubMap("Phone", "Phone"),
                    new SubPubMap("Plus4", "Plus4"),
                    new SubPubMap("CategoryID", "PubCategoryID"),
                    new SubPubMap("", "PubCode"),
                    new SubPubMap("TransactionDate", "PubTransactionDate"),
                    new SubPubMap("QDate", "Qualificationdate"),
                    new SubPubMap("State", "RegionCode"),
                    new SubPubMap("Sequence", "SequenceID"),
                    new SubPubMap("", "SubGenIsLead"),
                    new SubPubMap("", "SubGenRenewalCode"),
                    new SubPubMap("", "SubGenSubscriptionExpireDate"),
                    new SubPubMap("", "SubGenSubscriptionLastQualifiedDate"),
                    new SubPubMap("", "SubGenSubscriptionRenewDate"),
                    new SubPubMap("SubSrc", "SubscriberSourceCode"),
                    new SubPubMap("", "SubscriptionStatusID"),
                    new SubPubMap("", "SubSrcID"),
                    new SubPubMap("TextPermission", "TextPermission"),
                    new SubPubMap("Title", "Title"),
                    new SubPubMap("Verified", "Verify"),
                    new SubPubMap("", "Website"),
                    new SubPubMap("Zip", "ZipCode"),
                    new SubPubMap("ForZip", ""),
                    new SubPubMap("MailPermission", "Demo31"),
                    new SubPubMap("FaxPermission", "Demo32"),
                    new SubPubMap("PhonePermission", "Demo33"),
                    new SubPubMap("OtherProductsPermission", "Demo34"),
                    new SubPubMap("ThirdPartyPermission", "Demo35"),
                    new SubPubMap("EmailRenewPermission", "Demo36")
                };
                return lst;
            }
        }

        public static string GetSub(string pub)
        {
            if (SubToPubMap.Exists(x => x.PubField.Equals(pub, StringComparison.CurrentCultureIgnoreCase)))
            {
                return SubToPubMap.Single(x => x.PubField.Equals(pub, StringComparison.CurrentCultureIgnoreCase)).SubField;
            }

            return string.Empty;
        }

        public static string GetPub(string sub)
        {
            if (SubToPubMap.Exists(x => x.SubField.Equals(sub, StringComparison.CurrentCultureIgnoreCase)))
            {
                return SubToPubMap.Single(x => x.SubField.Equals(sub, StringComparison.CurrentCultureIgnoreCase)).PubField;
            }

            return string.Empty;
        }

        public static SubPubMap GetSubMap(string pub)
        {
            if (SubToPubMap.Exists(x => x.PubField.Equals(pub, StringComparison.CurrentCultureIgnoreCase)))
            {
                return SubToPubMap.Single(x => x.PubField.Equals(pub, StringComparison.CurrentCultureIgnoreCase));
            }

            return new SubPubMap(string.Empty, pub);
        }

        public static SubPubMap GetPubMap(string sub)
        {
            if (SubToPubMap.Exists(x => x.SubField.Equals(sub, StringComparison.CurrentCultureIgnoreCase)))
            {
                return SubToPubMap.Single(x => x.SubField.Equals(sub, StringComparison.CurrentCultureIgnoreCase));
            }

            return new SubPubMap(sub, string.Empty);
        }

        public class SubPubMap
        {
            public SubPubMap()
            {

            }

            public SubPubMap(string sub, string pub)
            {
                SubField = sub;
                PubField = pub;
            }

            public string SubField { get; set; }
            public string PubField { get; set; }
        }
    }
}