using System;
using TechTalk.SpecFlow;

using System.Net.Http;


using Should;

using APIModel = EmailMarketing.API.Models.User;


namespace EmailMarketing.API.Specs.User
{
    [Binding]
    public class FolderSteps
    {
        public readonly TestHelper Helper;
        public FolderSteps(TestHelper helper)
        {
            Helper = helper;
        }

        public APIModel GenerateNewContextObject()
        {
            throw new NotImplementedException("POST is not supported for User API");
        }

        /* [Given(@"I have an existing Folder ID of (.*)")]
        public void GivenIHaveAnExistingFolderIDOf(int p0)
        {
            Helper.SetRecordId(p0);
        }*/


        [Given(@"I set ControllerName to ""(.*)""")]
        public void GivenISetControllerNameTo(string p0)
        {
            Helper.ControllerName = p0;
        }


        [Then(@"The HTTP Response Content should be a valid User object")]
        public void ThenTheHTTPResponseContentShouldBeAValidUserObject()
        {
            Helper.ResponseContentShouldBeType<APIModel>();
        }

    }
}
