using System.Collections.Generic;
using System.Collections.Specialized;

namespace KM.Common
{
    public enum ECNParameterTypes
    {
        EmailID,
        BlastID,
        GroupID,
        BlastLinkID,
        SocialMediaID,
        CampaignItemID,
        CustomerID,
        UserName,
        Password,
        RedirectApp,
        CampaignItemSocialMediaID
        //ContentID,
        //GroupID,
        //LayoutID,
        //FilterID,        
        //Link
    };

    public class QueryString : QueryString<ECNParameterTypes, QueryStringParameters>
    {
        protected static readonly IReadOnlyDictionary<string, ECNParameterTypes> QueryStringMappings =
            new Dictionary<string, ECNParameterTypes>
            {
                ["b="] = ECNParameterTypes.BlastID,
                ["redirectapp="] = ECNParameterTypes.BlastID,
                ["bid="] = ECNParameterTypes.BlastID,
                ["blastid="] = ECNParameterTypes.BlastID,
                ["campaignitemid="] = ECNParameterTypes.CampaignItemID,
                ["g="] = ECNParameterTypes.GroupID,
                ["gid="] = ECNParameterTypes.GroupID,
                ["e="] = ECNParameterTypes.EmailID,
                ["eid="] = ECNParameterTypes.EmailID,
                ["emailid="] = ECNParameterTypes.EmailID,
                ["lid="] = ECNParameterTypes.BlastLinkID,
                ["m="] = ECNParameterTypes.SocialMediaID,
                ["cism="] = ECNParameterTypes.CampaignItemSocialMediaID,
                ["c="] = ECNParameterTypes.CustomerID
            };

        public static QueryString GetECNParameters(string stringToParse)
        {
            var queryString = new QueryString
            {
                StringToParse = stringToParse,
                ParameterList = new List<QueryStringParameters>()
            };

            if (string.IsNullOrWhiteSpace(stringToParse))
            {
                return queryString;
            }

            queryString.ProcessQueryString(
                stringToParse, 
                QueryStringMappings,
                (parameterValue, parameter) =>
                {
                    var queryStringParameter = new QueryStringParameters
                    {
                        Parameter = parameter,
                        ParameterValue = parameterValue
                    };
                    queryString.ParameterList.Add(queryStringParameter);
                });

            return queryString;
        }

        public static void ParseEncryptedSSOQuerystring(string encryptedQueryString, int applicationID,
            out string userNameValue, out string passwordValue)
        {
            NameValueCollection queryStringValues = ParseEncryptedQueryString(encryptedQueryString, applicationID);

            userNameValue = queryStringValues.Get(KM.Common.ECNParameterTypes.UserName.ToString());
            passwordValue = queryStringValues.Get(KM.Common.ECNParameterTypes.Password.ToString());
        }

        public static void ParseEncryptedSSOQuerystring(string encryptedQueryString, int applicationID,
            out string userNameValue, out string passwordValue, out string redirectApp)
        {
            NameValueCollection queryStringValues = ParseEncryptedQueryString(encryptedQueryString, applicationID);

            userNameValue = queryStringValues.Get(KM.Common.ECNParameterTypes.UserName.ToString());
            passwordValue = queryStringValues.Get(KM.Common.ECNParameterTypes.Password.ToString());
            redirectApp = queryStringValues.Get(KM.Common.ECNParameterTypes.RedirectApp.ToString());
        }

        public static NameValueCollection ParseEncryptedQueryString(string encryptedQueryString, int applicationID)
        {
            KM.Common.Entity.Encryption ec = KM.Common.Entity.Encryption.GetCurrentByApplicationID(applicationID);
            string queryString =
                KM.Common.Encryption.Decrypt(System.Web.HttpUtility.UrlDecode(encryptedQueryString), ec);
            return System.Web.HttpUtility.ParseQueryString(queryString);
        }
    }
}
