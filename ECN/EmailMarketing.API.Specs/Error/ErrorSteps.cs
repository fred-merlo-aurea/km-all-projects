using System;
using TechTalk.SpecFlow;

using Should;

using APIModel = EmailMarketing.API.Models.Error;

namespace EmailMarketing.API.Specs.Error
{
    [Binding]
    public class ErrorSteps
    {
        public readonly TestHelper Helper;
        public ErrorSteps(TestHelper helper)
        {
            Helper = helper;
        }

        public APIModel GenerateNewContextObject()
        {
            return new APIModel
            {
                ApplicationID = 24,
                SeverityID = 3,
                LogNote = "created by API",
                SourceMethod = "EmailMarketing.API.Specs.Error.POST",
                Exception = "ERROR HTML GOES HERE"
            };
        }

        [Given(@"I have an Exception object")]
        public void GivenIHaveAnExceptionObject()
        {
            Helper.ContextObject = new Exception("outer message", new Exception("inner message"));
        }

        [Then(@"The HTTP Response Content should be a string")]
        public void ThenTheHTTPResponseContentShouldBeAString()
        {
            Helper.ResponseContentShouldBeType<string>();
        }

        [Then(@"the string should contain ""(.*)""")]
        public void ThenTheStringShouldContain(string p0)
        {
            ((string)Helper.ContextObject).ShouldContain(p0);
        }

        [Given(@"I have a new Error Object")]
        public void GivenIHaveANewErrorObject()
        {
            Helper.ContextObject = GenerateNewContextObject();
        }

        [Then(@"The HTTP Response Content should be an Error object")]
        public void ThenTheHTTPResponseContentShouldBeAnErrorObject()
        {
            Helper.ResponseContentShouldBeType<APIModel>();
        }

        [Then(@"the LogID property should be greater than (.*)")]
        public void ThenTheLogIDPropertyShouldBeGreaterThan(int p0)
        {
            APIModel e = (APIModel)Helper.ContextObject;
            e.ShouldNotBeNull();
            e.LogID.ShouldBeGreaterThan(p0);
        }

    }
}
