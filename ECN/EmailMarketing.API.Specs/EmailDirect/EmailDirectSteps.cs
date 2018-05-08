using System;
using TechTalk.SpecFlow;

using Should;

using APIModel = EmailMarketing.API.Models.EmailDirectMessage;

namespace EmailMarketing.API.Specs.EmailDirect
{
    [Binding]
    public class BaseChannelSteps
    {
        public readonly TestHelper Helper;
        public BaseChannelSteps(TestHelper helper)
        {
            Helper = helper;
        }
        [Given(@"I have a new EmailDirect Object")]
        public void GivenIHaveANewEmailDirectObject()
        {
            Helper.ContextObject = new APIModel
            {
                Content = "the content",
                EmailAddress = "email.address@test.com",
                EmailSubject = "the subject",
                Source = "EmailMarketing.API.Specs.EmailDirect.POST",
                FromEmailAddress = "email.from@test.com",
//                Fromname = "from name",
                ReplyEmailAddress = "email.reply@test.com"
            };
        }

        [Then(@"The HTTP Response Content should be an EmailDirect object")]
        public void ThenTheHTTPResponseContentShouldBeAnEmailDirectObject()
        {
            Helper.ResponseContentShouldBeType<APIModel>();
        }

        [Then(@"the EmailDirectID property should be greater than (.*)")]
        public void ThenTheEmailDirectIDPropertyShouldBeGreaterThan(int p0)
        {
            APIModel o = (APIModel)Helper.ContextObject ?? new APIModel();
            o.EmailDirectID.ShouldBeGreaterThan(p0);
        }
    }
}
