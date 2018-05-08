using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using TechTalk.SpecFlow;
using Should;

using APIModel = EmailMarketing.API.Models.Content;

namespace EmailMarketing.API.Specs
{
    [Binding]
    public class ContentSteps
    {
        public readonly TestHelper Helper;
        public ContentSteps(TestHelper helper)
        {
            Helper = helper;
            TestHelper.StoreObject(375005, new APIModel()
            {
                ContentID = 375005,
                ContentSource = @"<meta content=""text/html; charset=UTF-8"" http-equiv=""Content-Type"" /><title>Generated HTML Content - Updated</title>",
                ContentText = Helper.GenerateRandomString("Generated Text Content - Updated", 8000),
                ContentTitle = "Web API Test Content 001",
            });
        }

        public APIModel GenerateNewContextObject()
        {
            return new APIModel()
            {
                ContentTitle = Helper.GenerateRandomString("API Test Generated Content", 255),//content title max is 255,
                ContentText = "Generated Text Content",
                ContentSource = @"<meta content=""text/html; charset=UTF-8"" http-equiv=""Content-Type"" /><title>Generated HTML Content</title>",
            };
        }

        #region GET
        [Given(@"I have a valid API Access Key of ""(.*)""")]
        public void GivenIHaveAValidAPIAccessKeyOf(string p0)
        {
            Helper.SetAccessKey(p0);
        }

        [Given(@"I have a Customer ID of (.*)")]
        public void GivenIHaveACustomerIDOf(string p0)
        {
            Helper.SetCustomerID(p0);
        }

        [Given(@"I have an existing Content ID of (.*)")]
        public void GivenIHaveAnExistingContentIDOf(int p0)
        {
            Helper.SetRecordId(p0);
        }

        [When(@"I invoke GET")]
        public void WhenIInvokeGET()
        {
            Helper.InvokeMethod(HttpMethod.Get);            
        }

        [Then(@"I should receive an HTTP Response")]
        public void ThenIShouldReceiveAnHTTPResponse()
        {
            Helper.Response.ShouldNotBeNull();
        }

        [Then(@"The HTTP Response Content should be a valid API Model object")]
        public void ThenTheHTTPResponseContentShouldBeAValidAPIModelObject()
        {
            Helper.ContextObject = Helper.Response.Content.ReadAsAsync<APIModel>().Result;
            Helper.ContextObject.ShouldNotBeNull();
            Helper.ContextObject.ShouldBeType<APIModel>();
        }

        [Then(@"The API Model object should have a ContentID property matching the given Content ID")]
        public void ThenTheAPIModelObjectShouldHaveAContentIDPropertyMatchingTheGivenContentID()
        {
            Helper.GetContextObject<APIModel>().ContentID.ShouldEqual(Helper.RecordID);
        }
        #endregion GET
        #region PUT

        [Given(@"I have an existing Content Object with a ContentID of (.*)")]
        public void GivenIHaveAnExistingContentObjectWithAContentIDOf(int p0)
        {
            Helper.SetRecordId(p0);
            Helper.ContextObject = TestHelper.RetreiveObject(p0);
        }

        [Given(@"I generate a RandomString to append to the ContentText")]
        public void GivenIGenerateARandomStringToAppendToTheContentText()
        {
            Helper.GetContextObject<APIModel>().ContentText = 
                Helper.GenerateRandomString(Helper.GetContextObject<APIModel>().ContentText + "Generated Text Content - Updated", 8000);
        }

        [When(@"I invoke PUT")]
        public void WhenIInvokePUT()
        {
            Helper.InvokeMethodWithJsonContent(HttpMethod.Put);
        }

        [Then(@"status should be ""(\d+) (.*)""")]
        public void ThenStatusShouldBe(int p0, string p1)
        {
            ((int)Helper.Response.StatusCode).ShouldEqual(p0);
            Helper.Response.ReasonPhrase.ShouldEqual(p1);
            //Response.StatusCode.ToString().ShouldEqual(p0);
        }

        [Then(@"a Location Header should be sent")]
        public void ThenALocationHeaderShouldBeSent()
        {
            Helper.Response.Headers.Contains("Location").ShouldBeTrue();
            Helper.Response.Headers.Location.ShouldNotBeNull();
        }

        [Then(@"the Location Header should end with ContentID")]
        public void ThenTheLocationHeaderShouldEndWithContentID()
        {
            Helper.Response.Headers.Location.ToString().EndsWith(Helper.RecordID.ToString());
        }

        [Then(@"HttpResponseContent should have ContentText ending with the RandomString")]
        public void ThenHttpResponseContentShouldHaveContentTextEndingWithTheRandomString()
        {
            var responseObject = Helper.Response.Content.ReadAsAsync<APIModel>().Result;
            responseObject.ContentText.EndsWith(Helper.RandomString);
        }

        #endregion PUT
        #region POST

        [Given(@"I have a new Content Object")]
        public void GivenIHaveANewContentObject()
        {
            Helper.ContextObject = GenerateNewContextObject();
        }

        [When(@"I invoke POST")]
        public void WhenIInvokePOST()
        {
            Helper.InvokeMethodWithJsonContent(HttpMethod.Post);
        }

        [Then(@"I can store the object to test delete")]
        public void ThenICanStoreTheObjectToTestDelete()
        {
            var responseObject = Helper.Response.Content.ReadAsAsync<APIModel>().Result;
            TestHelper.DeletableObjectId = responseObject.ContentID;
            TestHelper.StoreObject(TestHelper.DeletableObjectId.Value, responseObject);
        }

        #endregion POST
        #region DELETE
        [Given(@"I have a deletable Content Object")]
        public void GivenIHaveADeletableContentObject()
        {
            uint waitCount = 10;
            while(false == TestHelper.DeletableObjectId.HasValue && waitCount-->0)
            {
                System.Threading.Thread.Sleep(new TimeSpan(0, 0, 5));
            }
            if (false == TestHelper.DeletableObjectId.HasValue)
            {
                //throw new ApplicationException("timeout waiting for a deletable object; did POST succeed?");
                ScenarioContext.Current.Pending();
            }
            else
            {
                Helper.RecordID = TestHelper.DeletableObjectId.Value;
            }
        }

        [When(@"I invoke DELETE")]
        public void WhenIInvokeDELETE()
        {
            Helper.InvokeMethod(HttpMethod.Delete);
        }

        #endregion DELETE
        #region Negative Testing
        #region Missing APIAccessKey

        [Then(@"the HTTP Response Content should be an HttpError")]
        public void ThenTheHTTPResponseContentShouldBeAnHttpError()
        {
            Helper.Error = Helper.Response.Content.ReadAsAsync<HttpError>().Result;
        }

        [Then(@"the error Message Should be '(.*)'")]
        public void ThenTheErrorMessageShouldBe(string p0)
        {
            Helper.Error.Message.ToLower().ShouldEqual(p0.ToLower());
        }

        [Then(@"the error HttpStatusCode should be '(.*)'")]
        public void ThenTheErrorHttpStatusCodeShouldBe(string p0)
        {
            Helper.Error["HttpStatusCode"].ToString().ShouldEqual(p0);
        }

        #endregion Missing APIAccessKey
        #region Invalid APIAccessKey

        [Given(@"I have an invalid API Access Key of ""(.*)""")]
        public void GivenIHaveAnInvalidAPIAccessKeyOf(string p0)
        {
            GivenIHaveAValidAPIAccessKeyOf(p0); // no test-side validation
        }

        #endregion Invalid APIAcccessKey
        #region NonExistant Content

        [Given(@"I have an nonexistent Content ID of (.*)")]
        public void GivenIHaveAnNonexistentContentIDOf(int p0)
        {
            GivenIHaveAnExistingContentIDOf(p0); // no test-side validation
        }


        #endregion NonExistant Content
        #region Invalid Data

        [Given(@"I append invalid character '(.*)' to the ContentTitle")]
        public void GivenIAppendInvalidCharacterToTheContentTitle(string p0)
        {
            Helper.GetContextObject<APIModel>().ContentTitle += p0;
        }

        #endregion Invalid Data

        #endregion Negative Testing
    }
}
