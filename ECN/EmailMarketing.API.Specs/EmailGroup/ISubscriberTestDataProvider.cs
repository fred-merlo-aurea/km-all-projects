using EmailMarketing.API.Models.EmailGroup;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
namespace EmailMarketing.API.Specs.EmailGroup
{
    public interface ISubscriberTestDataProvider
    {
        string ApiAccessKey { get; set; }
        int BaseChannelID { get; set; }
        int CustomerID { get; set; }
        string GetCachedTestValueOrExecuteString(string testName, Func<string> makeSql);
        int GroupID { get; set; }
        IEnumerable<Email> TransformEmailList(Table table, TestHelper helper);
        IEnumerable<SubscriptionResult> TransformResultList(IEnumerable<SubscriptionResult> results);
    }
}
