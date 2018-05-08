using System;
using System.Collections.Generic;
using KM.Common;
using static ECN_Framework_Common.Objects.Enums;

namespace ECN_Framework_Common.Objects
{
    public class QueryString : QueryString<ParameterTypes, QueryStringParameters>
    {
        private const string EmailAtChar = "@";

        protected static readonly IReadOnlyDictionary<string, ParameterTypes> QueryStringMappings =
            new Dictionary<string, ParameterTypes>
            {
                ["b="] = ParameterTypes.BlastID,
                ["redirectapp="] = ParameterTypes.BlastID,
                ["bid="] = ParameterTypes.BlastID,
                ["blastid="] = ParameterTypes.BlastID,
                ["s="] = ParameterTypes.Subscribe,
                ["f="] = ParameterTypes.Format,
                ["campaignitemid="] = ParameterTypes.CampaignItemID,
                ["g="] = ParameterTypes.GroupID,
                ["gid="] = ParameterTypes.GroupID,
                ["e="] = ParameterTypes.EmailID,
                ["ei="] = ParameterTypes.EmailID,
                ["eid="] = ParameterTypes.EmailID,
                ["emailid="] = ParameterTypes.EmailID,
                ["lid="] = ParameterTypes.BlastLinkID,
                ["m="] = ParameterTypes.SocialMediaID,
                ["c="] = ParameterTypes.CustomerID,
                ["preview="] = ParameterTypes.Preview,
                ["sfid="] = ParameterTypes.SmartFormID,
                ["url="] = ParameterTypes.URL,
                ["layoutid="] = ParameterTypes.LayoutID,
                ["monitor="] = ParameterTypes.Monitor,
                ["bcid="] = ParameterTypes.BaseChannelID,
                ["newemail="] = ParameterTypes.NewEmail,
                ["oldemail="] = ParameterTypes.OldEmail,
                ["edid="] = ParameterTypes.EmailDirectID
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
                    if (parameter == ParameterTypes.EmailID)
                    {
                        var atIndex = parameterValue.IndexOf(EmailAtChar, StringComparison.OrdinalIgnoreCase);
                        if (atIndex > 0)
                        {
                            queryStringParameter.Parameter = ParameterTypes.EmailAddress;
                        }
                    }
                    queryString.ParameterList.Add(queryStringParameter);
                });

            return queryString;
        }
    }
}
