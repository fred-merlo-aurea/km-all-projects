using System;
using TechTalk.SpecFlow;

using Should;

using APIModel = EmailMarketing.API.Models.BaseChannel;

namespace EmailMarketing.API.Specs.BaseChannel
{
    [Binding]
    public class BaseChannelSteps
    {
        public readonly TestHelper Helper;
        public BaseChannelSteps(TestHelper helper)
        {
            Helper = helper;
        }

        [Then(@"The HTTP Response Content should be a valid BaseChannel object")]
        public void ThenTheHTTPResponseContentShouldBeAValidBaseChannelObject()
        {
            Helper.ResponseContentShouldBeType<APIModel>();
        }

        [Then(@"the object should have a BaseChannelID greater than (.*)")]
        public void ThenTheObjectShouldHaveABaseChannelIDGreaterThan(int p0)
        {
            APIModel o = (APIModel)Helper.ContextObject ?? new APIModel();
            o.BaseChannelID.ShouldBeGreaterThan(p0);
        }

    }
}
